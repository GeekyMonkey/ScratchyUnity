using UnityEngine;

public static class CameraExtensions
{
    public static Bounds OrthographicBounds(this Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            new Vector3(camera.transform.position.x, camera.transform.position.y, 0),
            new Vector3(cameraHeight * screenAspect, cameraHeight, 1));
        return bounds;
    }
}
