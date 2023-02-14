using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChasingMob : MonoBehaviour
{
    private PlayerBehaviour playerBehaviour;
    private Transform mobTrans;
    private float mobTransX = 0f;
    private const float FIRST_GAP_DISTANCE = 3f;
    private float mobSpeed = 0.1f;
    private float mobPlayerDistance = 0f;


    public delegate void enterBossMode();
    public static event enterBossMode echoEnterBossMode;

    void executeEchoEnterBossMode()
    {
        if (echoEnterBossMode != null)
        {
            echoEnterBossMode();
        }
    }

    private void Awake()
    {
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        mobTrans = GetComponent<Transform>();
    }
    void Start()
    {
        mobTransX = playerBehaviour.transform.position.x - FIRST_GAP_DISTANCE;
    }


    void Update()
    {
        // temporarily make it follow player
        // can add speed at here
        // maybe use a loop count * speed
        mobTransX = (mobTransX + 1f) * mobSpeed;
        mobTrans.transform.position = new Vector2(mobTransX, 0f);  

    }

    void distancePlayerAlert()
    {

        // red screen : either use analog method, or use switch case method to change red intensity
        // mobPlayerDistance;
        Debug.Log("");
    }
}
