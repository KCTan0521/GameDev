using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterChasingMob : MonoBehaviour
{
    
    [SerializeField]
    private float FIRST_GAP_DISTANCE;
    [SerializeField] // 0.045 or 0.06
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
        // this will change the intensity of red screen,
        // depending on the mob distance with player

        // this will call the boss mode function when the mob is near to the player

        mobPlayerDistance = playerBehaviour.transform.position.x - mobTrans.transform.position.x;
        
        Debug.Log("Mob & Player distance : " + mobPlayerDistance);

        if (mobPlayerDistance <= MIN_MOB_DISTANCE)
        {
            setWarningScreenColor(255, 0, 0, 100);
            executeEchoEnterBossMode();
        }

        if(mobPlayerDistance <= DISTANCE_TO_START_ALERT)
        {
            redScreenIntensity = 1 - (mobPlayerDistance / DISTANCE_TO_START_ALERT);
            redScreenIntensity = Mathf.Round(redScreenIntensity * 100);
            setWarningScreenColor(255, 0, 0, (int)redScreenIntensity);

        }
    }

    public void resetMonsterPlayerDistance()
    {
        Debug.Log("player position x : " + playerBehaviour.transform.position.x);
        runningNum = playerBehaviour.transform.position.x - FIRST_GAP_DISTANCE;
        Debug.Log("running number : " + runningNum);
        mobTrans.transform.position = new Vector2(runningNum, mobTransY);
        Debug.Log("chasing mob position x : " + mobTrans.transform.position.x);
        setWarningScreenColor(255, 0, 0, 0);
    }

    void setWarningScreenColor(int red, int green, int blue, int transparency)
    {
        warningScreen.GetComponent<Image>().color = new Color32((byte)red, (byte)green, (byte)blue, (byte)transparency);
    }

    void executeEchoEnterBossMode()
    {
        if (echoEnterBossMode != null)
        {
            echoEnterBossMode();
        }
    }
}
