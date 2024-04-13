using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class AiBehaviorMoving : MonoBehaviour
{
    private NavMeshAgent agent;

    private GameObject selectedUnit;
    private LayerMask unitMask;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        unitMask = LayerMask.GetMask("Unit");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check if left mouse button is clicked
        {
            if (!IsPointerOverUIObject()) // Check if the click is not on a UI element
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Create ray from mouse position

                if (Physics.Raycast(ray, out hit)) // Cast ray and check if it hits something
                {
                    if (hit.collider != null) // Ensure the ray hits a collider
                    {
                        if (hit.collider.CompareTag("SelectableUnit")) // Check if the hit object is a selectable unit
                        {
                            selectedUnit = hit.collider.gameObject; // Set the selected unit
                        }
                        else // If the hit object is not a selectable unit, move the agent
                        {
                            agent.destination = hit.point; // Set agent's destination to hit point
                        }
                    }
                }
            }
        }

        // If there's a selected unit, move it to the clicked position
        if (selectedUnit != null && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Create ray from mouse position

            if (Physics.Raycast(ray, out hit)) // Cast ray and check if it hits something
            {
                if (hit.collider != null) // Ensure the ray hits a collider
                {
                    agent.destination = hit.point; // Set agent's destination to hit point
                }
            }
        }
    }

    // Function to check if the mouse pointer is over a UI element
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
