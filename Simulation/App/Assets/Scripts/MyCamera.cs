using UnityEngine;

public class MyCamera : MonoBehaviour
{
    [SerializeField] float zoomSpeed;
    [SerializeField] float maxZoom;

    private float zoom = 0f;
    Vector2 oldPosition;

    Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        zoom = camera.orthographicSize;
        oldPosition = camera.ScreenToViewportPoint(Input.mousePosition);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            oldPosition = camera.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 newPosition = camera.ScreenToViewportPoint(Input.mousePosition);
            transform.Translate(camera.ViewportToWorldPoint(oldPosition) - camera.ViewportToWorldPoint(newPosition));
            oldPosition = newPosition;
        }

        zoom -= Input.mouseScrollDelta.y * zoomSpeed;
        if(zoom < maxZoom)
        {
            zoom = maxZoom;
        }
        GetComponent<Camera>().orthographicSize = zoom;
    }
}