using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpsController : MonoBehaviour
{

    [SerializeField]
    private GameObject[] powerUpsReference;
    public int powerUpsLimit;
    public float distanceX;
    public PlatformGeneration platformGeneration;

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
            // set the time gap for power ups generation to 15 - 50 seconds
            yield return new WaitForSeconds(Random.Range(15, 50));

            if (spawnedPowerUpsList.Count <= powerUpsLimit)
            {
                // to randonmly generate the monster
                int randomIndex = Random.Range(0, powerUpsReference.Length);

                spawnedPowerUps = Instantiate(powerUpsReference[randomIndex]);
                spawnedPowerUpsList.Add(spawnedPowerUps);

                //decide randomly spawn on platform or on ground
                if (Random.Range(0, 2) == 0)
                {
                    // spawn the monster with on the ground with a certain distance away
                    spawnedPowerUps.transform.position = new Vector2 (
                        platformGeneration.platforms.Last().transform.position.x + 0.5f,
                        platformGeneration.platforms.Last().transform.position.y + 1 + (spawnedPowerUps.GetComponent<SpriteRenderer>().bounds.size.y / 2)
                    );
                } else
                {
                    // spawn the monster with on the ground with a certain distance away
                    spawnedPowerUps.transform.position = new Vector2 (
                        _player.transform.position.x + distanceX, 
                        spawnedPowerUps.GetComponent<SpriteRenderer>().bounds.size.y / 2
                    );
                }

                
            }

        } // while loop
    }
}
