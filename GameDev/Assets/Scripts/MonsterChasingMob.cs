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
    private const float MIN_MOB_DISTANCE = 3f;


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
        resetMonsterPlayerDistance();
    }


    void Update()
    {
        // temporarily make it follow player
        // can add speed at here
        // maybe use a loop count * speed
        mobTransX = (mobTransX + 1f) * mobSpeed;
        mobTrans.transform.position = new Vector2(mobTransX, 0f);
        distancePlayerAlert();
    }

    void distancePlayerAlert()
    {

        // red screen : either use analog method, or use switch case method to change red intensity
        // mobPlayerDistance;

        mobPlayerDistance = playerBehaviour.transform.position.x - mobTrans.transform.position.x;
        
        Debug.Log("Mob & Player distance : " + mobPlayerDistance);

        if (mobPlayerDistance <= MIN_MOB_DISTANCE)
        {
            executeEchoEnterBossMode();
        }
    }

    void resetMonsterPlayerDistance()
    {
        mobTransX = playerBehaviour.transform.position.x - FIRST_GAP_DISTANCE;
        mobTrans.transform.position = new Vector2(mobTransX, 0f);
    }
}
