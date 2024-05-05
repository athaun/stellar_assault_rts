using UnityEngine;

public class Healthbar : MonoBehaviour {
    public float yOffset = 3f;
    public Vector3 scale = new Vector3(0.6f, 0.2f, 0.2f); 

    private GameObject healthBar; 
    private Transform barTransform; 

    private GameObject referenceBar;
    private Transform referenceTransform; 

    public Color color = Color.green; 
    public Color referenceColor = Color.gray; 

    private void Awake() {
        createHealthBar();
        createReferenceBar();
    }

    private void createHealthBar() {
        // Create a new GameObject for the health bar
        healthBar = new GameObject("HealthBar");
        healthBar.transform.SetParent(transform);
        healthBar.transform.localPosition = new Vector3(0f, yOffset, 0f);

        // Create a cube for the health bar
        GameObject barObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        barObject.transform.SetParent(healthBar.transform);
        barObject.transform.localScale = scale * 1.3f;
        barTransform = barObject.transform;

        // Adjust position and color of the health bar
        barTransform.localPosition = new Vector3(scale.x * 1, 0f, 0f); // Adjust the position of the health bar
        Renderer barRenderer = barObject.GetComponent<Renderer>();
        barRenderer.material.color = color;
    }

    private void createReferenceBar() {
        // Create a new GameObject for the reference bar
        referenceBar = new GameObject("ReferenceBar");
        referenceBar.transform.SetParent(transform);
        referenceBar.transform.localPosition = Vector3.up * yOffset;

        // Create a cube for the reference bar
        GameObject referenceObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        referenceObject.transform.SetParent(referenceBar.transform);
        referenceObject.transform.localScale = scale * 0.9f;
        referenceTransform = referenceObject.transform;

        // Adjust position and color of the reference bar
        referenceTransform.localPosition = new Vector3(scale.x * 1, 0f, 0f);
        Renderer referenceRenderer = referenceObject.GetComponent<Renderer>();
        referenceRenderer.material.color = Color.gray;
    }

    public void updateHealth(float currentHealth, float maxHealth) {
        float percentage = Mathf.Clamp01(currentHealth / maxHealth);

        // Scale the health bar cube based on health percentage
        barTransform.localScale = new Vector3(scale.x * percentage, scale.y, scale.z);
    }
}
