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
    private const float distanceX = 18f;

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

            // will generate at most 3 monsters
            // because when length is 2 it will execute the code as well
            // so 3 monsters in total
            if (GameObject.FindGameObjectsWithTag("Monster").Length <= 2)
            {
                // to randonmly generate the monster
                randomIndex = Random.Range(0, monsterReference.Length);
                spawnedMonster = Instantiate(monsterReference[randomIndex]);

                // add a distance between the character
                spawnedMonster.transform.position = new Vector3(leftPos.position.x + distanceX, 0f, 0f);
            }
            
        } // while loop
    }

}
