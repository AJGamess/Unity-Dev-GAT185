using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;

    private NavMeshAgent agent;
    private Transform waypoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (waypoints == null || waypoints.Length == 0)
        {
            waypoints = GameObject.FindGameObjectsWithTag("Waypoint").Select(go => go.transform).ToArray();
        }
        waypoint = waypoints[Random.Range(0, waypoints.Length)];

        agent.SetDestination(waypoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < 0.5f)
        {
            waypoint = waypoints[Random.Range(0, waypoints.Length)];
            agent.SetDestination(waypoint.position);
        }
    }
}
