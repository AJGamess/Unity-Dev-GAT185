using UnityEngine;
using UnityEngine.AI;

public class AgentDestination : MonoBehaviour
{
	[SerializeField] LayerMask layerMask;

	void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, layerMask))
			{
				var movements = FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None);
				foreach (var movement in movements)
				{
					movement.destination = hitInfo.point;
				}
			}
		}
	}
}
