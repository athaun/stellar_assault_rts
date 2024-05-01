using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {
    private Economy economy;

    // Ship stats
    public int faction;

    [Header("Cost")]
    [SerializeField] private int scrapGeneration;
    [SerializeField] private int scrapCost;
    [SerializeField] public int electricityConsumption;
    [SerializeField] private int electricityGeneration;

    [Header("Health")]
    [SerializeField] protected int maxHealth;
    [HideInInspector] protected int health;

    [Header("Combat")]
    [SerializeField] protected bool isPassive;
    [SerializeField] protected int attackDamage;
    [SerializeField] protected int awarenessRange = 14;
    [SerializeField] protected float attackRange = 14;

    [Header("Enemy Faction")]
    [SerializeField] protected bool isEnemy;
    [SerializeField] protected Ship spaceStation;

    [Header("Friendly Bullets")]
    [SerializeField] protected GameObject f_muzzleFlashPrefab;
    [SerializeField] protected GameObject f_bulletPrefab;
    [SerializeField] protected GameObject f_impactPrefab;

    [Header("Enemy Bullets")]
    [SerializeField] protected GameObject e_muzzleFlashPrefab;
    [SerializeField] protected GameObject e_bulletPrefab;
    [SerializeField] protected GameObject e_impactPrefab;

    [SerializeField] protected GameObject explosionPrefab;

    private bool moveOrders = false;

    [SerializeField] private bool isActiveElectricityGeneration = false;
    [SerializeField] private bool isActiveElectricityConsumption = true;
    [SerializeField] private bool isActiveScrapGeneration = false;

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
    protected Healthbar healthbar;

    public UnitMover Mover { get => mover; }
    public Outline Outline { get => outline; }

    // Combat variables
    protected List<Ship> targets;
    protected List<Ship> selectedTargets; // Targets selected by the player
    protected SphereCollider awarenessCollider;
    protected BL_Turret turret;

    public List<Ship> Targets { get => targets; }
    public List<Ship> PlayerSelectedTargets { get => selectedTargets; set => selectedTargets = value; }

    public bool IsEnemy { get => isEnemy; set => isEnemy = value; }
    public Ship SpaceStation { get => spaceStation; set => spaceStation = value; }
    
    // Prevents reset from deselection if outside selection box
    [HideInInspector] public bool selectedByClick = false; 

    void Start() {
        mover = GetComponent<UnitMover>();
        outline = GetComponentInChildren<Outline>();
        economy = FindFirstObjectByType<Economy>();
        healthbar = GetComponent<Healthbar>();


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
                // Debug.Log("Faction 0 " + f_muzzleFlashPrefab + " " + f_bulletPrefab + " " + f_impactPrefab);
            } else {
                turret.muzzleFlashPrefab = e_muzzleFlashPrefab;
                turret.bulletPrefab = e_bulletPrefab;
                turret.impactPrefab = e_impactPrefab;
            }
        }

        targets = new List<Ship>();
        selectedTargets = new List<Ship>();

        if(isActiveElectricityConsumption){
            StartCoroutine(economy.ConsumeElectricity(electricityConsumption, isActiveElectricityConsumption));//True by default
        }
        if(isActiveElectricityGeneration){
            StartCoroutine(economy.GenerateElectricity(electricityGeneration, isActiveElectricityGeneration));//False by default
        }
        if(isActiveScrapGeneration){
            StartCoroutine(economy.GenerateScrap(scrapGeneration, isActiveScrapGeneration));//False by default
        }

        if (isEnemy) {
            faction = 1;
        }
    }

    void Update() {
        checkRadius();
        removeDeadEnemies();

        if (!isEnemy) {
            if (HasMoveOrders) {
                mover.Agent.isStopped = false;
                mover.moveTo(mover.Agent.destination);
                if (Vector3.Distance(transform.position, mover.Agent.destination) < 0.1f) {
                    HasMoveOrders = false;
                    mover.Agent.isStopped = true;
                }
            } else if (selectedTargets.Count > 0) {
                Attack(closestTarget(selectedTargets));
            } else if (targets.Count > 0) {
                Attack(closestTarget(Targets));
            }
        } else {
            Outline.enabled = true;

            if (isPassive) return;

            if (Targets.Count > 0) {
                Attack(closestTarget(Targets));   
            } else {
                Attack(spaceStation);
            }
        }
    }

    private Ship closestTarget(List<Ship> t) {
        Ship closestShip = null;
        float closestDistance = Mathf.Infinity;
        
        foreach (Ship ship in t) {
            float distance = Vector3.Distance(transform.position, ship.transform.position);
            
            if (distance < closestDistance) {
                closestDistance = distance;
                closestShip = ship;
            }
        }
        
        return closestShip;
    }

    private void removeDeadEnemies() {
        for (int i = 0; i < targets.Count; i++) {
            if (!targets[i].isActiveAndEnabled) {
                targets.RemoveAt(i);
            }
        }
    }

    private void checkRadius() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, awarenessRange);
        foreach (Collider collider in colliders) {
            Ship target = collider.GetComponentInParent<Ship>();
            if (target != null && target.faction != faction) {
                targets.Add(target);
            }
        }
    }

    public void TakeDamage(int damage) {
        // Reduce health by the damage amount
        health -= damage;

        if (healthbar != null) healthbar.updateHealth(health, maxHealth);

        // If health is 0 or less, destroy the unit
        if (health <= 0) {
            isActiveElectricityGeneration = false;
            isActiveElectricityConsumption = false;
            isActiveScrapGeneration = false;
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            UnitSelectionManager.Instance.removeShip(this);
        }
    }

    public void Hit() {
        TakeDamage(10);
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
        Ship target = other.gameObject.GetComponentInParent<Ship>();

        if (target != null && target.faction != faction) {
            targets.Add(target);
        }
    }

    void OnTriggerExit(Collider other) {
        // If the collider is a ship, remove it from the list of targets
        Ship target = other.GetComponentInParent<Ship>();
        if (target != null && target.faction != faction) {
            targets.Remove(target);
        }
    }
}