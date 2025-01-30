using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] Transform barrel;
    [SerializeField, Range(0.5f, 5)] float spawnTime;

    float spawnTimer;

    void Start()
    {
        StartCoroutine(SpawnFire());
        //spawnTimer = Time.time + spawnTime; 
    }

    void Update()
    {
        //spawnTimer -= Time.deltaTime;
        //if (Time.time >= spawnTimer) {
        //    spawnTimer = Time.time + spawnTime;
        //    Instantiate(rocketPrefab, barrel.position, barrel.rotation);
        //}
    }

    IEnumerator SpawnFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTimer);
            Instantiate(rocketPrefab, barrel.position, barrel.rotation);
        }
    }
}
