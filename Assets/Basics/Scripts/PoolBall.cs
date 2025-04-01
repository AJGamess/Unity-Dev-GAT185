using System.Collections;
using UnityEngine;

public class PoolBall : MonoBehaviour, IPoolable<GameObject>
{
    public IPool<GameObject> Pool { get; set; }


    public void OnSpawn()
    {
        Debug.Log("Ball spawned");
        GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * 10, ForceMode.VelocityChange);
        StartCoroutine(WaitToRelease(2));
    }

    public void OnDespawn()
    {
        Debug.Log("Ball despawned");
    }
    bool proceed = false;
    IEnumerator WaitToRelease(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Pool.Release(gameObject);
    }
}
