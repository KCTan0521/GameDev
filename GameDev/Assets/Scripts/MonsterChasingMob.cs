using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChasingMob : MonoBehaviour
{
    private PlayerBehaviour playerBehaviour;
    private Transform trans;


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
        trans = GetComponent<Transform>();
    }
    void Start()
    {
        
    }


    void Update()
    {
        // temporarily make it follow player
        // can add speed at here
        // maybe use a loop count * speed
        trans.transform.position = new Vector2(playerBehaviour.transform.position.x - 3f, 0f);  
    }
}
