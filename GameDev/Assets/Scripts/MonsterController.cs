using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{

    [SerializeField]
    private GameObject[] monsterReference;

    private GameObject spawnedMonster;

    [SerializeField]
    private Transform leftPos;
    public float distanceX;
    public int monsterSpawnLimit;

    private int randomIndex;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnMonster());
    }

    IEnumerator SpawnMonster()
    {
        while (true)
        {
            // set the time gap for monster generation to 1 - 3 seconds
            yield return new WaitForSeconds(Random.Range(1, 3));

            if (GameObject.FindGameObjectsWithTag("Monster").Length <= monsterSpawnLimit)
            {
                // to randonmly generate the monster
                randomIndex = Random.Range(0, monsterReference.Length);
                spawnedMonster = Instantiate(monsterReference[randomIndex]);

                // spawn the monster with on the ground with a certain distance away
                spawnedMonster.transform.position = new Vector2(leftPos.position.x + distanceX, spawnedMonster.GetComponent<SpriteRenderer>().bounds.size.y / 2);
            }
            
        } // while loop
    }

}
