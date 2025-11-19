using DefaultNamespace;
using UnityEngine;

public class CameraController : MonoSingleton<CameraController>
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

    public float minX = 0f;
    public float maxX = 100f;
    public float minY = 0f;
    public float maxY = 100f;

    [Header("Smoothing")]
    public float smoothTime = 0.1f;

    private Vector3 targetPosition;
    private float targetZoom;
    private Vector3 velocity = Vector3.zero;
    private float currentZoomVelocity = 0f;
    
    [Header("Follow Target")]
    [SerializeField] private Vector3 _followOffset = new Vector3(0, 10, 0);
    
    private Transform _followTarget;
    private bool _isFollowing = false;
    private Vector3 _startingPosition;
    void Start()
    {
        _startingPosition = transform.position;
        targetPosition = transform.position;
        targetZoom = transform.position.y; // Zoom based on Y-position
    }

    void Update()
    {
        var userMoved = HandlePan(); 
        HandleZoom();
        
        if (userMoved && _isFollowing)
            StopFollowing();

        if (_isFollowing && _followTarget != null)
            targetPosition = _followTarget.position + _followOffset;
        
        ApplySmoothing();
        ApplyBounds();
    }

    bool HandlePan()
    {
        Vector3 panInput = Vector3.zero;
        var userMoved = false;
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
            // use raw mouse axes and apply panSpeed and unscaledDeltaTime when applying movement below
            panInput.x -= Input.GetAxis("Mouse X");
            panInput.y -= Input.GetAxis("Mouse Y");
            userMoved = true;
        }

        // Adjust pan speed based on zoom level (closer = slower pan)
        float zoomFactor = Mathf.Lerp(0.5f, 1.5f, (targetZoom - minZoom) / (maxZoom - minZoom));
        // Apply pan movement using unscaled delta time so camera still moves when timeScale == 0
        targetPosition += panInput * panSpeed * zoomFactor * Time.unscaledDeltaTime;
        return userMoved;
    }

    void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0f)
        {
            // Use unscaledDeltaTime so zoom still works when timeScale == 0
            float newZoom = targetZoom + scroll * zoomSpeed * Time.unscaledDeltaTime;
            targetZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);
        }
    }

    void ApplySmoothing()
    {
        // Smooth position (XY plane for panning) using unscaledDeltaTime
        Vector3 newPos = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, Mathf.Infinity, Time.unscaledDeltaTime);

        // Smooth zoom (z-position) using unscaledDeltaTime
        float newZ = Mathf.SmoothDamp(transform.position.z, targetZoom, ref currentZoomVelocity, smoothTime, Mathf.Infinity, Time.unscaledDeltaTime);
        newPos.z = newZ;

        transform.position = newPos;
    }

    void ApplyBounds()
    {
        if (!useBounds) return;

        Vector3 pos = targetPosition;
        float zoom = targetPosition.y;

        // Adjust bounds based on zoom (perspective cameras see more when higher)
        float boundPadding = zoom * 0.5f; // Rough estimate for FOV
        pos.x = Mathf.Clamp(pos.x, _startingPosition.x - minX, _startingPosition.x + maxX);
        pos.y = Mathf.Clamp(pos.y, _startingPosition.y - minY, _startingPosition.y + maxY);

        targetPosition = pos;
    }

    public void Follow(Transform colonistTransform)
    {
        _followTarget = colonistTransform;
        _isFollowing = true;
    }
    
    public void StopFollowing()
    {
        _isFollowing = false;
        _followTarget = null;
    }
}