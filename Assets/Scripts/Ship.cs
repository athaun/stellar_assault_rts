using UnityEngine;

public class Ship : MonoBehaviour {

    protected UnitMover mover;
    protected Outline outline;

    public float maxHealth;
    public float health;

    private Vector3 destination;

    [HideInInspector] public bool selectedByClick = false; // Prevents reset from deselection if outside selection box

    public UnitMover Mover { get { return mover; } }
    public Outline Outline { get { return outline; } }

    public int faction;

    void Start() {
        mover = GetComponent<UnitMover>();
        outline = GetComponentInChildren<Outline>();

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