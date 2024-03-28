using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Road : MonoBehaviour
{
    public GameObject[] roadSection;

    void Start()
    {

    }

    //logic for spawnning road tiles
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RoadTrigger"))

        {
            int toSpawn = Random.Range(0, roadSection.Length);
            GameObject newRoadSection = Instantiate(roadSection[toSpawn], new Vector3(0, 0, 65), Quaternion.identity);
        }

        //logic for destorying passed road tiles
        if (other.gameObject.CompareTag("Destroy"))
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
