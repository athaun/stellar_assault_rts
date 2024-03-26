using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]

public class Outline: MonoBehaviour {

    [SerializeField]
    public Color outlineColor = Color.white;

    public bool enabled = false;

    // [SerializeField, Range(0f, 10f)]
    // private float outlineWidth = 2f;

    private Renderer[] renderers;

    private bool needsUpdate;

    void Awake() {

        // Cache renderers
        renderers = GetComponentsInChildren < Renderer > ();
        foreach(var renderer in renderers) {
            var propertyBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_SelectionColor", outlineColor);
            renderer.SetPropertyBlock(propertyBlock);
        }

    }

    void OnEnable() {

    }

    void Update() {
        if (enabled) {
            SetLayerRecursively(gameObject, 13);
        } else {
            SetLayerRecursively(gameObject, 12);
        }
    }

    void SetLayerRecursively(GameObject obj, int newLayer) {
        if (null == obj) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform) {
            if (null == child) continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    void OnDisable() {
        foreach(var renderer in renderers) {

        }
    }

    void UpdateMaterialProperties() {
        // outlineFillMaterial.SetColor("_OutlineColor", outlineColor);
    }
}