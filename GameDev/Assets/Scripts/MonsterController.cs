using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private GameObject _wahmen;
    private Rigidbody2D _player;
    private CinemachineVirtualCamera playerCam;
    private float leftScreen;
    private float orthoSize;
    private float aspectRatio;
    private float camEnd;
    private float spawnX;
    private float targettedX;
    private float spawnY;
    private float lastGiantX;
    private float lastWahmenX;
    public List<GameObject> platforms = new List<GameObject>();

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
            MobSpawn(_wahmen);
        }
    }

    private void PlatformFinder()
    {
        platforms = GetComponent<PlatformGeneration>().platforms;

        foreach (GameObject platform in platforms)
        {
            if (platform.transform.position.x == spawnX + 40f)
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

    private void MobSpawn(GameObject monster)
    {
        PlatformFinder();
        int[] upOrDown = { 1, 2 };
        System.Random rnd = new System.Random();
        int randIndex = rnd.Next(upOrDown.Length);
        int rand = upOrDown[randIndex];
        spawnX += 40f;

        if (monster == _wahmen)
        {
            if (rand == 1 || spawnY < 5)
            {
                Instantiate(_wahmen, new Vector2(spawnX, 1f), Quaternion.identity);
            }

            else
            {
                Instantiate(_wahmen, new Vector2(spawnX, spawnY), Quaternion.identity);
            }
        }
    }
}
