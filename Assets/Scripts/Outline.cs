using UnityEngine;

[DisallowMultipleComponent]

public class Outline : MonoBehaviour {

    private Color outlineColor = Color.white;

    public new bool enabled = false;

    private Renderer[] renderers;

    void Awake() {
        // Cache renderers
        renderers = GetComponentsInChildren<Renderer>();

        UpdateShaders();
    }

    private void UpdateShaders() {
        foreach (var renderer in renderers) {
            var propertyBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_SelectionColor", outlineColor);
            renderer.SetPropertyBlock(propertyBlock);
        }
    }

    public void SetColor(Color color) {
        outlineColor = color;
        UpdateShaders();
    }

    void Update() {
        if (enabled) {
            SetLayerRecursively(gameObject, 13);
        } else {
            SetLayerRecursively(gameObject, 12);
        }
    }

    void SetLayerRecursively(GameObject obj, int newLayer) {
        if (null == obj || obj.CompareTag("no_outline")) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform) {
            if (null == child) continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}