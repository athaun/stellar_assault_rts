using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMover : MonoBehaviour {

    Vector3 newPosition;
    bool move = false;

    public float defaultSpeed = 3;
    public float speed;
    public float rotationSpeed = 200;
    public bool closeToTarget = false;

    // Start is called before the first frame update
    void Start() {
        
    }

    void Update() {
        if (newPosition != null && move) {
            if (newPosition != transform.position) {

                // Find the vector pointing from our position to the target
                Vector3 direction = (newPosition - transform.position).normalized;

                // Create the rotation we need to be in to look at the target
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                float angle = Quaternion.Angle(transform.rotation, lookRotation);
                float timeToComplete = angle / rotationSpeed;
                float percentageDone = Mathf.Min(1F, Time.deltaTime / timeToComplete);

                // Rotate towards a direction, but not immediately (rotate a little every frame)
                if (transform.position != newPosition) {
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Mathf.Clamp(percentageDone * angle, 0, rotationSpeed));
                }

                // Move towards (0, 0, 1) relative to ship rotation
                float distToTarget = Vector3.Distance(newPosition, transform.position);
                
                if (!closeToTarget) {
                    speed = Mathf.Clamp(Vector3.Distance(newPosition, transform.position), 0, defaultSpeed);
                } else {
                    speed = Mathf.Clamp(Vector3.Distance(newPosition, transform.position)/2, 0, defaultSpeed);
                }

                if (speed < 0.06) {
                    speed = 0;
                    move = false;
                }
                
                transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
            }
        }
    }

    public void moveTo (Vector3 newPos) {
        newPosition = newPos;
        move = true;
    }
}
