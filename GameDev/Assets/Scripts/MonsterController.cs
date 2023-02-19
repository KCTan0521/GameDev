using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private GameObject _wahmen;
    [SerializeField] private GameObject _giant;
    [SerializeField] private GameObject _powerUpJump;
    [SerializeField] private GameObject _powerUpRegen;
    private Rigidbody2D _player;
    private CinemachineVirtualCamera playerCam;
    private float leftScreen;
    private float orthoSize;
    private float aspectRatio;
    private float camEnd;
    public float spawnX;
    private float spawnY;
    private float lastWahmenX;
    private float mobCount;
    private List<GameObject> platforms = new List<GameObject>();

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        orthoSize = playerCam.m_Lens.OrthographicSize;
        aspectRatio = GetComponent<Camera>().aspect;
        leftScreen = orthoSize * aspectRatio;
    }  

    private void Update()
    {
        camEnd = transform.position.x + leftScreen;
        if (spawnX < camEnd)
        {
            MobSpawn(MobSelector());
        }
    }

    private void PlatformFinder()
    {
        platforms = GetComponent<PlatformGeneration>().platforms;

        foreach (GameObject platform in platforms)
        {
            if (platform.transform.position.x == spawnX + 20f)
            {
                spawnY = platform.transform.position.y + 2f;
                break;
            }

            else
            {
                spawnY = 0f;
            }
        }
    }

    private void MobSpawn(GameObject item)
    {
        PlatformFinder();
        int[] upOrDown = { 1, 2 };
        System.Random rnd = new System.Random();
        int randIndex = rnd.Next(upOrDown.Length);
        int rand = upOrDown[randIndex];
        spawnX += 20f;

        if (item == _wahmen)
        {
            if (rand == 2 && spawnY > 0f)
            {
                Instantiate(_wahmen, new Vector2(spawnX, spawnY), Quaternion.identity);
                lastWahmenX = spawnX;
            }

            else if (spawnY > 5f && spawnY > 0f)
            {
                Instantiate(_wahmen, new Vector2(spawnX, 1f), Quaternion.identity);
                lastWahmenX = spawnX;
            }

            else
            {
                Instantiate(_giant, new Vector2(spawnX, 1f), Quaternion.identity);
            }
        }

        if (item == _giant)
        {
            Instantiate(_giant, new Vector2(spawnX, 1f), Quaternion.identity);
        }

        if (item == _powerUpJump || item == _powerUpRegen)
        {
            if (item == _powerUpJump)
            {
                Instantiate(_powerUpJump, new Vector2(spawnX, spawnY + 1f), Quaternion.identity);
            }
            else
            {
                Instantiate(_powerUpRegen, new Vector2(spawnX, spawnY + 1f), Quaternion.identity);
            }
        }
    }

    private GameObject MobSelector()
    {
        int[] monsters = { 1, 2 };
        System.Random rnd = new System.Random();
        int randIndex = rnd.Next(monsters.Length);
        int mob = monsters[randIndex];

        if (mobCount == 10)
        {
            int[] powerUps = { 1, 2 };
            System.Random rnd_2 = new System.Random();
            int randIndex_2 = rnd_2.Next(powerUps.Length);
            int powerUp = powerUps[randIndex];
            mobCount = 0;

            if (powerUp == 1)
            {
                return _powerUpJump;
            }

            else
            {
                return _powerUpRegen;
            }
        }

        else if (mob == 1 && spawnX - lastWahmenX >= 40f)
        {
            mobCount += 1;
            return _wahmen;
        }

        else
        {
            mobCount += 1;
            return _giant;
        } 
    }
}
