using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterChasingMob : MonoBehaviour
{
    
    [SerializeField]
    private float FIRST_GAP_DISTANCE;
    [SerializeField] // 0.045
    private float mobSpeed;
    [SerializeField]
    private float MIN_MOB_DISTANCE;
    [SerializeField]
    private GameObject warningScreen;
    [SerializeField]
    private float DISTANCE_TO_START_ALERT;

    private PlayerBehaviour playerBehaviour;
    private Transform mobTrans;
    private float mobTransX = 0f;
    private const float mobTransY = 1.3f;
    private float runningNum = 0f;
    private float mobPlayerDistance = 0f;
    private float redScreenIntensity = 0f;

    public delegate void enterBossMode();
    public static event enterBossMode echoEnterBossMode;

    void Awake()
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

        runningNum += 1f;
        mobTransX = runningNum * mobSpeed;
        mobTrans.transform.position = new Vector2(mobTransX, mobTransY);
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
            warningScreen.GetComponent<Image>().color = new Color32(255, 0, 0, 100);
            executeEchoEnterBossMode();
        }

        if(mobPlayerDistance <= DISTANCE_TO_START_ALERT)
        {
            redScreenIntensity = 1 - (mobPlayerDistance / DISTANCE_TO_START_ALERT);
            redScreenIntensity = Mathf.Round(redScreenIntensity * 100);
            warningScreen.GetComponent<Image>().color = new Color32(255, 0, 0, (byte) redScreenIntensity);

        }
    }

    void resetMonsterPlayerDistance()
    {

        runningNum = playerBehaviour.transform.position.x - FIRST_GAP_DISTANCE;
        mobTrans.transform.position = new Vector2(runningNum, mobTransY);
    }

    void executeEchoEnterBossMode()
    {
        if (echoEnterBossMode != null)
        {
            echoEnterBossMode();
        }
    }
}
