using UnityEngine;
using UnityEngine.AI;

public class ShipController : MonoBehaviour
{
    private NavMeshAgent agent;
    public float rotationSpeed = 10f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("BasePlane"))
                {
                    RotateShip(hit.point); // Rotate ship towards the target direction first
                    agent.SetDestination(hit.point); // Then set the destination
                }
            }
        }
    }

    void RotateShip(Vector3 targetPosition)
    {
        // Calculate the direction to the target
        Vector3 targetDirection = (targetPosition - transform.position).normalized;

        // Align the agent's rotation smoothly towards the target direction
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
