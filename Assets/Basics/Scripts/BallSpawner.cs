using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private Pool ballPool;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            GameObject ball = ballPool.Get();
            ball.transform.position = transform.position;
            ball.SetActive(true);

        }
    }
}
