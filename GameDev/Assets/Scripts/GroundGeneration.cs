using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GroundGeneration : MonoBehaviour
{
    [SerializeField] private GameObject _ground;
    private CinemachineVirtualCamera playerCam;
    private float leftScreen;
    private float orthoSize;
    private float aspectRatio;
    private float camEnd;
    private float leftMostGround;
    private List<GameObject> grounds = new List<GameObject>();

    private void Awake()
    {
        playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        orthoSize = playerCam.m_Lens.OrthographicSize;
        aspectRatio = GetComponent<Camera>().aspect;
        leftScreen = orthoSize * aspectRatio;
        for (int i = 0; i < 4; i++)
        {
            GameObject newGround = Instantiate(_ground, new Vector2(i * 20f, 0f), Quaternion.identity);
            grounds.Add(newGround);
        }
    }

    // Update is called once per frame
    void Update()
    {
        camEnd = GetComponent<Transform>().position.x - leftScreen;
        leftMostGround = grounds[0].transform.position.x + 40f;
        if (camEnd > leftMostGround)
        {
            Destroy(grounds[0]);
            grounds.RemoveAt(0);
            GameObject newGround = Instantiate(_ground, new Vector2(leftMostGround + 40f, 0f), Quaternion.identity);
            grounds.Add(newGround);
        }
    }
}
