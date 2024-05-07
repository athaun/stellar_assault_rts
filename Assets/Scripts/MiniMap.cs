using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour {
    public RawImage minimapImage;
    private Camera minimapCamera;

    void Start() {
        // Create a new Camera for the Minimap
        GameObject minimapCameraObject = new GameObject("Minimap Camera");
        minimapCamera = minimapCameraObject.AddComponent<Camera>();
        minimapCamera.transform.position = new Vector3(0, 100, 0);
        minimapCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
        minimapCamera.orthographic = true;
        minimapCamera.orthographicSize = 50;
        minimapCamera.cullingMask = LayerMask.GetMask("Minimap");
        minimapCamera.renderingPath = RenderingPath.Forward; // Use forward rendering
        minimapCamera.clearFlags = CameraClearFlags.SolidColor; // Clear the camera with a solid color
        minimapCamera.backgroundColor = new Color32(81, 81, 81, 255); // Set the clear color to transparent

        // Create a new Render Texture
        RenderTexture minimapTexture = new RenderTexture(256, 256, 16);

        // Assign the Render Texture to the Minimap Camera
        minimapCamera.targetTexture = minimapTexture;

        // Assign the Render Texture to the Raw Image
        minimapImage.texture = minimapTexture;
    }

    void Update() {
        // Update minimap icons here
    }
}