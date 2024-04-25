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
    [HideInInspector] protected int health;

    [Header("Combat")]
    [SerializeField] protected bool isPassive;
    [SerializeField] protected int attackDamage;
    [SerializeField] protected int awarenessRange = 20;
    [SerializeField] protected float attackRange = 14;

    [Header("Friendly Bullets")]
    [SerializeField] protected GameObject f_muzzleFlashPrefab;
    [SerializeField] protected GameObject f_bulletPrefab;
    [SerializeField] protected GameObject f_impactPrefab;

    [Header("Enemy Bullets")]
    [SerializeField] protected GameObject e_muzzleFlashPrefab;
    [SerializeField] protected GameObject e_bulletPrefab;
    [SerializeField] protected GameObject e_impactPrefab;

    private bool moveOrders = false;


    public int ScrapCost { get => scrapCost; set => scrapCost = value; }
    public int ElectricityConsumption { get => electricityConsumption; set => electricityConsumption = value; }
    public int ElectricityGeneration { get => electricityGeneration; set => electricityGeneration = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int Health { get => health; set => health = value; }
    public int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public bool HasMoveOrders { get => moveOrders; set => moveOrders = value; }

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

        if (!isPassive) {
            if (faction == 0) {
                turret.muzzleFlashPrefab = f_muzzleFlashPrefab;
                turret.bulletPrefab = f_bulletPrefab;
                turret.impactPrefab = f_impactPrefab;
            } else {
                turret.muzzleFlashPrefab = e_muzzleFlashPrefab;
                turret.bulletPrefab = e_bulletPrefab;
                turret.impactPrefab = e_impactPrefab;
            }
        }

        targets = new List<Ship>();
        selectedTargets = new List<Ship>();
    }

    void Update() {
        if (HasMoveOrders) {
            mover.Agent.isStopped = false;
            mover.moveTo(mover.Agent.destination);
            if (Vector3.Distance(transform.position, mover.Agent.destination) < 0.1f) {
                HasMoveOrders = false;
                mover.Agent.isStopped = true;
            }
        } else if (selectedTargets.Count > 0) {
            Attack(selectedTargets[0]);
        } else if (targets.Count > 0) {
            Attack(targets[0]);
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

    public void clearTargets() {
        selectedTargets.Clear();
        targets.Clear();
    }

    protected void Attack(Ship target) {
        // If the target is null, return
        if (target == null || isPassive) return;

        turret.Aim(target.transform.position);

        // If the target is not in range, move towards it
        if (Vector3.Distance(transform.position, target.transform.position) < attackRange) {
            Mover.Agent.isStopped = true;
            HasMoveOrders = false;

            Mover.rotateShip(target.transform.position);
            
            // If the target is in range, attack it
            // Get the direction of the target
            Vector3 targetDirection = (target.transform.position - transform.position).normalized;

            // Get the current direction of the ship
            Vector3 shipDirection = transform.forward;

            // Calculate the angle between the ship's forward direction and the target
            float angle = Vector3.Angle(shipDirection, targetDirection);

            // If the angle is small enough (e.g., less than 5 degrees), fire the turret
            if (angle < 10f) {
                turret.Fire();
            }
            
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