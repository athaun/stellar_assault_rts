using UnityEngine;

public class Ship : MonoBehaviour {

    private UnitMover mover;
    private Outline outline;

    public float maxHealth;
    public float health;

    private Vector3 destination;

    public bool selectedByClick = false;

    public UnitMover Mover { get { return mover; } }
    public Outline Outline { get { return outline; } }

    void Start() {
        mover = GetComponent<UnitMover>();
        outline = GetComponent<Outline>();

        UnitSelectionManager managerInstance = FindFirstObjectByType<UnitSelectionManager>();
        if (managerInstance != null) {
            managerInstance.registerShip(this);
        } else {
            Debug.LogError("UnitSelectionManager not found in the scene! Cannot register ship");
        }

        health = maxHealth;
    }

    void Update() {

    }

    public void TakeDamage(float damage) {
        // Reduce health by the damage amount
        health -= damage;

        // If health is 0 or less, destroy the unit
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}