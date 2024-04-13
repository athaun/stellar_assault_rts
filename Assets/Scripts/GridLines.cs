using UnityEngine;

public class GridLines : MonoBehaviour {

    public Transform gridPlane;
    public UnitController unitController;
    public Material gridMaterial;

    void Update() {
        Vector3 mousePosition = unitController.GetPointUnderCursor();

        // Normalize the mouse position to a 0-1 range, top left is 0,0, bottom right is 1,1
        Vector3 normalizedMousePosition = new Vector3(
            1 - ((mousePosition.x / (gridPlane.localScale.x * 5.0f) + 1) / 2), // Normalize x from 0 to 1
            0, // Ignore y
            1 - ((mousePosition.z / (gridPlane.localScale.z * 5.0f) + 1) / 2)  // Normalize z
        );

        gridMaterial.SetVector("_MousePosition", normalizedMousePosition);
    }
}
