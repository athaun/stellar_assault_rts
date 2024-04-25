using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    // Ship stats
    [Header("Cost")]
    [SerializeField] private int scrapCost;
    [SerializeField] private int electricityConsumption;
    [SerializeField] private int electricityGeneration;

    [Header("Health")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int health;

    [Header("Combat")]
    [SerializeField] protected bool isPassive;
    [SerializeField] protected int attackDamage;
    [SerializeField] protected int awarenessRange;
    [SerializeField] protected float attackRange;

    public int faction;

    public int ScrapCost { get => scrapCost; set => scrapCost = value; }
    public int ElectricityConsumption { get => electricityConsumption; set => electricityConsumption = value; }
    public int ElectricityGeneration { get => electricityGeneration; set => electricityGeneration = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int Health { get => health; set => health = value; }
    public int AttackDamage { get => attackDamage; set => attackDamage = value; }

    // Static components
    protected UnitMover mover;
    protected Outline outline;    

    public UnitMover Mover { get => mover; }
    public Outline Outline { get => outline; }

    // Combat variables
    protected List<Ship> targets;
    protected List<Ship> selectedTargets; // Targets selected by the player
    protected SphereCollider awarenessCollider;
    protected BL_Turret turret;

    public List<Ship> Targets { get => targets; }
    public List<Ship> SelectedTargets { get => selectedTargets; set => selectedTargets = value; }
    
    // Prevents reset from deselection if outside selection box
    [HideInInspector] public bool selectedByClick = false; 


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

        // Set up awareness collider
        awarenessCollider = gameObject.AddComponent<SphereCollider>();
        awarenessCollider.radius = awarenessRange;
        awarenessCollider.isTrigger = true;

        turret = GetComponent<BL_Turret>();
        isPassive = (turret == null); // If the ship has no turret, make it passive
    }

    void Update() {

    }

    public void TakeDamage(int damage) {
        // Reduce health by the damage amount
        health -= damage;

        // If health is 0 or less, destroy the unit
        if (health <= 0) {
            Destroy(gameObject);
        }

        if (targets.Count > 0 && !isPassive) {
            Attack(targets[0]);            
        }
    }

    private void Attack(Ship target) {
        // If the target is null, return
        if (target == null) return;

        // If the target is not in range, move towards it
        if (Vector3.Distance(transform.position, target.transform.position) > attackRange) {
            mover.moveTo(target.transform.position);
        } else {
            // If the target is in range, attack it
            target.TakeDamage(attackDamage);
        }
    }

    void OnTriggerEnter(Collider other) {
        // If the collider is a ship, add it to the list of targets
        Ship target = other.GetComponent<Ship>();
        if (target != null) {
            targets.Add(target);
        }
    }

    void OnTriggerExit(Collider other) {
        // If the collider is a ship, remove it from the list of targets
        Ship target = other.GetComponent<Ship>();
        if (target != null) {
            targets.Remove(target);
        }
    }
}