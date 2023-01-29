using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsController : MonoBehaviour
{

    [SerializeField]
    private GameObject[] powerUpsReference;
    public int powerUpsLimit;
    public float distanceX;

    private GameObject _player;

    private GameObject spawnedPowerUps;
    private List<GameObject> spawnedPowerUpsList;


    // Start is called before the first frame update
    void Awake()
    {
        _player = GameObject.Find("Player");
        spawnedPowerUpsList = new List<GameObject>();
        StartCoroutine(SpawnPowerUps());
    }

    // Update is called once per frame
    void Update()
    {
        spawnedPowerUpsList.RemoveAll(powerUps => powerUps == null);
    }

    IEnumerator SpawnPowerUps()
    {
        while (true)
        {
            // set the time gap for power ups generation to 30 - 120 seconds
            yield return new WaitForSeconds(Random.Range(30, 120));

            if (spawnedPowerUpsList.Count <= powerUpsLimit)
            {
                // to randonmly generate the monster
                int randomIndex = Random.Range(0, powerUpsReference.Length);

                spawnedPowerUps = Instantiate(powerUpsReference[randomIndex]);
                spawnedPowerUpsList.Add(spawnedPowerUps);

                // spawn the monster with on the ground with a certain distance away
                spawnedPowerUps.transform.position = new Vector2(_player.transform.position.x + distanceX, spawnedPowerUps.GetComponent<SpriteRenderer>().bounds.size.y / 2);
            }

        } // while loop
    }
}
