using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    // Ship stats
    public int faction;

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
    [SerializeField] protected int awarenessRange = 20;
    [SerializeField] protected float attackRange = 14;


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
    public List<Ship> PlayerSelectedTargets { get => selectedTargets; set => selectedTargets = value; }
    
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
        awarenessCollider.gameObject.layer = LayerMask.NameToLayer("Awareness");

        turret = GetComponent<BL_Turret>();
        isPassive = (turret == null); // If the ship has no turret, make it passive

        targets = new List<Ship>();
        selectedTargets = new List<Ship>();
    }

    void Update() {
        if (selectedTargets.Count > 0) {
            Attack(selectedTargets[0]);
            Debug.Log("Attacking selected target");
        } else if (targets.Count > 0) {
            Attack(targets[0]);
            Debug.Log("Attacking target");
        }
    }

    public void TakeDamage(int damage) {
        // Reduce health by the damage amount
        health -= damage;

        // If health is 0 or less, destroy the unit
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    public void addSelectedTarget(Ship target) {
        selectedTargets.Add(target);
    }

    public void removeSelectedTarget(Ship target) {
        selectedTargets.Remove(target);
    }

    public void clearSelectedTargets() {
        selectedTargets.Clear();
    }

    protected void Attack(Ship target) {
        // If the target is null, return
        if (target == null || isPassive) return;

        // turret.Aim(target.transform.position);

        // If the target is not in range, move towards it
        if (Vector3.Distance(transform.position, target.transform.position) < attackRange) {
            // If the target is in range, attack it
            turret.Fire();
            Mover.rotateShip(target.transform.position);
            Mover.Agent.isStopped = true;
        } else {
            mover.moveTo(target.transform.position);
            mover.Agent.isStopped = false;
        }
    }

    void OnTriggerEnter(Collider other) {
        // If the collider is a ship, add it to the list of targets
        Ship target = other.GetComponent<Ship>();
        if (target != null && target.faction != faction) {
            targets.Add(target);
        }
    }

    void OnTriggerExit(Collider other) {
        // If the collider is a ship, remove it from the list of targets
        Ship target = other.GetComponent<Ship>();
        if (target != null && target.faction != faction) {
            targets.Remove(target);
        }
    }
}