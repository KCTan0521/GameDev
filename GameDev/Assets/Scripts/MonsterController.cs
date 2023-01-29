using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{

    [SerializeField]
    private GameObject[] monsterReference;

    private GameObject spawnedMonster;
    private List<GameObject> spawnedMonsterList;

    [SerializeField]
    private Transform leftPos;
    public float distanceX;
    public int monsterSpawnLimit;
    public float distanceBetweenWahmen;
    public float distanceBetweenGiant;

    private int randomIndex;


    // Start is called before the first frame update
    void Start()
    {
        spawnedMonsterList = new List<GameObject>();
        StartCoroutine(SpawnMonster());
    }

    private void Update()
    {
        spawnedMonsterList.RemoveAll(monster => monster == null);
    }

    IEnumerator SpawnMonster()
    {
        while (true)
        {
            // set the time gap for monster generation to 1 - 3 seconds
            yield return new WaitForSeconds(Random.Range(1, 3));

            if (spawnedMonsterList.Count <= monsterSpawnLimit)
            {
                // to randonmly generate the monster
                randomIndex = Random.Range(0, monsterReference.Length);

                //check woman if it is certain tiles apart from last one
                if (monsterReference[randomIndex].tag == "Wahmen")
                {
                    bool canSpawnWoman = true;

                    foreach(GameObject monster in spawnedMonsterList)
                    {
                        float newSpawnPos = leftPos.position.x + distanceX;

                        if (monster.tag == "Wahmen" && newSpawnPos < (monster.transform.position.x + distanceBetweenWahmen))
                        {
                            canSpawnWoman = false;
                            break;
                        }
                            
                    }

                    if(!canSpawnWoman)
                        continue;
                }

                //check woman if it is certain tiles apart from last one
                if (monsterReference[randomIndex].tag == "Giant")
                {
                    bool canSpawnGiant = true;

                    foreach (GameObject monster in spawnedMonsterList)
                    {
                        float newSpawnPos = leftPos.position.x + distanceX;

                        if (monster.tag == "Monster" && newSpawnPos < (monster.transform.position.x + distanceBetweenGiant))
                        {
                            canSpawnGiant = false;
                            break;
                        }

                    }

                    if (!canSpawnGiant)
                        continue;
                }

                spawnedMonster = Instantiate(monsterReference[randomIndex]);
                spawnedMonsterList.Add(spawnedMonster);

                // spawn the monster with on the ground with a certain distance away
                spawnedMonster.transform.position = new Vector2(leftPos.position.x + distanceX, spawnedMonster.GetComponent<SpriteRenderer>().bounds.size.y / 2);
            }

        } // while loop
    }

}
