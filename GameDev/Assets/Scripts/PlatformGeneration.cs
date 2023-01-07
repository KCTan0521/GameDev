using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class PlatformGeneration : MonoBehaviour
{
    [SerializeField] private GameObject _platformRange;
    [SerializeField] private GameObject _platform;
    private Collider2D _screen;
    private CinemachineVirtualCamera playerCam;
    private float leftScreen;
    private float orthoSize;
    private float aspectRatio;
    private float camEnd;
    private float leftMostPlatform;
    private Vector2 platformPos;
    private List<GameObject> platformRanges = new List<GameObject>();
    private List<GameObject> platforms = new List<GameObject>();
    Dictionary<int, int> randomPos = new Dictionary<int, int>();

    private void Awake()
    {
        platformPos = new Vector2(0f, 5f);
        playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        orthoSize = playerCam.m_Lens.OrthographicSize;
        aspectRatio = GetComponent<Camera>().aspect;
        leftScreen = orthoSize * aspectRatio;
        for (int i = 0; i < 3; i++)
        {
            GameObject newRange = Instantiate(_platformRange, new Vector2(i * 40f, 0f), Quaternion.identity);
            platformRanges.Add(newRange);
        }
        while (platformPos.x < platformRanges.Last().transform.position.x + 40f)
        {
            generatePlatform();
            GameObject newPlatform = Instantiate(_platform, platformPos, Quaternion.identity);
            platforms.Add(newPlatform);
        }
    }

    private void Start()
    {
        _screen = platformRanges[0].transform.GetChild(0).gameObject.GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        camEnd = GetComponent<Transform>().position.x - leftScreen;
        leftMostPlatform = platformRanges[0].transform.position.x + 40f;
        if (camEnd > leftMostPlatform)
        {
            Destroy(platformRanges[0]);
            platformRanges.RemoveAt(0);
            GameObject newRange = Instantiate(_platformRange, new Vector2(leftMostPlatform + 80f, 0f), Quaternion.identity);
            platformRanges.Add(newRange);
            _screen = platformRanges[0].transform.GetChild(0).gameObject.GetComponent<PolygonCollider2D>();
        }
        playerCam.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = _screen;
        while (platformPos.x < platformRanges.Last().transform.position.x)
        {
            generatePlatform();
            GameObject newPlatform = Instantiate(_platform, platformPos, Quaternion.identity);
            platforms.Add(newPlatform);
            Destroy(platforms[0]);
            platforms.RemoveAt(0);
        }
    }

    private void posUp(int space)
    {
        randomPos.Clear();
        
        int x, y;

        for (int i = 2 ; i <= space; i++)
        {
            x = i;
            y = space - i;
            randomPos.Add(y, x);
        }
    }

    private void posDown()
    {
        randomPos.Clear();

        int x, y;
        int maxSpace = 6;

        for (int i = 2; i <= maxSpace; i++)
        {
            x = i;
            y = maxSpace - i;
            randomPos.Add(-y, x);
        }
    }

    private void generatePlatform()
    {
        if (platformPos.y < 4f)
        {
            moveUp();
        }

        else if (platformPos.y > 6f)
        {
            moveDown();
        }

        else
        {
            if (IsUp())
            {
                moveUp();
            }
            else
            {
                moveDown();
            }
        }
    }

    private bool IsUp()
    {
        int[] upOrDown = { 1, 2 };
        System.Random rnd = new System.Random();
        int randIndex = rnd.Next(upOrDown.Length);
        int rand = upOrDown[randIndex];

        if (rand == 1)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private void moveUp()
    {
        int indexY = 0;
        int posY = 0;

        posUp(3);

        System.Random rnd = new System.Random();
        indexY = rnd.Next(randomPos.Count);
        posY = randomPos.Keys.ElementAt(indexY);

        while (platformPos.y + posY > 8f)
        {
            indexY = rnd.Next(randomPos.Count);
            posY = randomPos.Keys.ElementAt(indexY);
        }

        platformPos += new Vector2(randomPos[posY] + 8f, posY);
    }

    private void moveDown()
    {
        int indexX = 0;
        int posX = 0;

        posDown();

        System.Random rnd = new System.Random();
        indexX = rnd.Next(randomPos.Count);
        posX = randomPos.Keys.ElementAt(indexX);

        while (platformPos.y + posX < 2f)
        {
            indexX = rnd.Next(randomPos.Count);
            posX = randomPos.Keys.ElementAt(indexX);
        }

        platformPos += new Vector2(randomPos[posX] + 8f, posX);
    }
}
