using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMover : MonoBehaviour {

    private NavMeshAgent agent;

    Vector3 newPosition;
    bool move = false;

    public float defaultSpeed = 3;
    private float speed;
    public float rotationSpeed = 200;
    public bool closeToTarget = false;

    public float Speed {
        get { return speed; }
        set { speed = value; }
    }

    void Awake() {
        agent = GetComponentInParent<NavMeshAgent>();
    }

    void Update() {
        if (newPosition != null && move) {
            if (newPosition != transform.position) {

                rotateShip(newPosition);

                agent.SetDestination(newPosition);


                // // Find the vector pointing from our position to the target
                // Vector3 direction = (newPosition - transform.position).normalized;

                // // Create the rotation we need to be in to look at the target
                // Quaternion lookRotation = Quaternion.LookRotation(direction);

                // float angle = Quaternion.Angle(transform.rotation, lookRotation);
                // float timeToComplete = angle / rotationSpeed;
                // float percentageDone = Mathf.Min(1F, Time.deltaTime / timeToComplete);

                // // Rotate towards a direction, but not immediately (rotate a little every frame)
                // if (transform.position != newPosition) {
                //     transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Mathf.Clamp(percentageDone * angle, 0, rotationSpeed));
                // }

                // // Move towards (0, 0, 1) relative to ship rotation
                // float distToTarget = Vector3.Distance(newPosition, transform.position);

                // if (!closeToTarget) {
                //     speed = Mathf.Clamp(Vector3.Distance(newPosition, transform.position), 0, defaultSpeed);
                // } else {
                //     speed = Mathf.Clamp(Vector3.Distance(newPosition, transform.position) / 2, 0, defaultSpeed);
                // }

                // if (speed < 0.06) {
                //     speed = 0;
                //     move = false;
                // }

                // transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
            }
        }
    }

    private void rotateShip(Vector3 targetPosition) {
        // Calculate the direction to the target
        Vector3 targetDirection = (targetPosition - transform.position).normalized;

        // Align the agent's rotation smoothly towards the target direction
        if (targetDirection != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void moveTo(Vector3 newPos) {
        newPosition = newPos;
        move = true;
    }
}