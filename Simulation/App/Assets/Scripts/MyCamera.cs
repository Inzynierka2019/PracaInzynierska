using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float dragSpeed = 6f;
    private float zoom = 0f;

    private void Awake()
    {
        zoom = GetComponent<Camera>().orthographicSize;
    }

    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0);
        }

        zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        GetComponent<Camera>().orthographicSize = zoom;
    }
}