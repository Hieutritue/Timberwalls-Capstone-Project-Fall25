using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float panSpeed = 20f;
    public float zoomSpeed = 10f;
    public float edgeScrollWidth = 50f;
    public bool enableEdgeScrolling = true;
    public KeyCode panKey = KeyCode.Mouse2; // Middle mouse for drag-pan

    [Header("Zoom Limits (Y-position)")]
    public float minZoom = 5f;
    public float maxZoom = 50f;

    [Header("Bounds (Optional - for map limits)")]
    public bool useBounds = false;
    public Rect bounds = new Rect(-50, -50, 100, 100);

    [Header("Smoothing")]
    public float smoothTime = 0.1f;

    private Vector3 targetPosition;
    private float targetZoom;
    private Vector3 velocity = Vector3.zero;
    private float currentZoomVelocity = 0f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        targetPosition = transform.position;
        targetZoom = transform.position.y; // Zoom based on Y-position
    }

    void Update()
    {
        HandlePan();
        HandleZoom();
        ApplySmoothing();
        ApplyBounds();
    }

    void HandlePan()
    {
        Vector3 panInput = Vector3.zero;

        // Edge scrolling
        if (enableEdgeScrolling)
        {
            if (Input.mousePosition.x <= edgeScrollWidth) panInput.x -= 1f;
            if (Input.mousePosition.x >= Screen.width - edgeScrollWidth) panInput.x += 1f;
            if (Input.mousePosition.y <= edgeScrollWidth) panInput.z -= 1f;
            if (Input.mousePosition.y >= Screen.height - edgeScrollWidth) panInput.z += 1f;
        }

        // Mouse drag pan (middle mouse)
        if (Input.GetKey(panKey))
        {
            panInput.x -= Input.GetAxis("Mouse X") * panSpeed * 0.01f;
            panInput.y -= Input.GetAxis("Mouse Y") * panSpeed * 0.01f;
        }

        // WASD keyboard pan
        panInput.x += Input.GetAxis("Horizontal") * panSpeed * Time.deltaTime;
        panInput.y += Input.GetAxis("Vertical") * panSpeed * Time.deltaTime;

        // Adjust pan speed based on zoom level (closer = slower pan)
        float zoomFactor = Mathf.Lerp(0.5f, 1.5f, (targetZoom - minZoom) / (maxZoom - minZoom));
        targetPosition += panInput * zoomFactor;
    }

    void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0f)
        {
            float newZoom = targetZoom + scroll * zoomSpeed;
            targetZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);
        }
    }

    void ApplySmoothing()
    {
        // Smooth position (XY plane for panning)
        Vector3 newPos = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Smooth zoom (z-position)
        float newZ = Mathf.SmoothDamp(transform.position.z, targetZoom, ref currentZoomVelocity, smoothTime);
        newPos.z = newZ;

        transform.position = newPos;
    }

    void ApplyBounds()
    {
        if (!useBounds) return;

        Vector3 pos = transform.position;
        float zoom = pos.y;

        // Adjust bounds based on zoom (perspective cameras see more when higher)
        float boundPadding = zoom * 0.5f; // Rough estimate for FOV
        pos.x = Mathf.Clamp(pos.x, bounds.xMin + boundPadding, bounds.xMax - boundPadding);
        pos.z = Mathf.Clamp(pos.z, bounds.yMin + boundPadding, bounds.yMax - boundPadding);

        transform.position = pos;
    }
}