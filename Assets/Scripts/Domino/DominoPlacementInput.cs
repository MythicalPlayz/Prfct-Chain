using UnityEngine;
using UnityEngine.EventSystems;

public class DominoPlacementInput : MonoBehaviour
{
    [SerializeField] private DominoPlacementController placementController;
    [SerializeField] private Camera placementCamera;
    [SerializeField] private LayerMask placementSurfaceMask;

    [Header("Drag Detection")]
    [SerializeField] private float dragThreshold = 0.15f;

    private Vector3 _startPosition;
    private Vector3 _currentPosition;

    private bool _isPointerDown;
    private bool _isDragging;

    private void Awake()
    {
        if (placementController == null)
        {
            Debug.LogError(
                "DominoPlacementInput: " +
                "Placement Controller is missing.",
                this
            );

            enabled = false;
            return;
        }

        if (placementCamera == null)
        {
            placementCamera = Camera.main;
        }

        if (placementCamera == null)
        {
            Debug.LogError(
                "DominoPlacementInput: " +
                "Placement Camera is missing.",
                this
            );

            enabled = false;
        }
    }

    private void Update()
    {
        HandlePointerInput();
    }

    private void HandlePointerInput()
    {
        if (IsPointerOverUI())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            BeginPointerAction();
        }

        if (!_isPointerDown)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            UpdatePointerAction();
        }

        if (Input.GetMouseButtonUp(0))
        {
            CompletePointerAction();
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelPointerAction();
        }
    }

    private void BeginPointerAction()
    {
        if (!TryGetMouseWorldPosition(out _startPosition))
        {
            return;
        }

        _currentPosition = _startPosition;
        _isPointerDown = true;
        _isDragging = false;
    }

    private void UpdatePointerAction()
    {
        if (!TryGetMouseWorldPosition(out _currentPosition))
        {
            return;
        }

        if (!_isDragging &&
            Vector3.Distance(
                _startPosition,
                _currentPosition) >= dragThreshold)
        {
            _isDragging = true;

            placementController.BeginLinePlacement(
                _startPosition
            );
        }

        if (_isDragging)
        {
            placementController.UpdateLinePlacement(
                _currentPosition
            );
        }
    }

    private void CompletePointerAction()
    {
        if (!TryGetMouseWorldPosition(out _currentPosition))
        {
            CancelPointerAction();
            return;
        }

        if (_isDragging)
        {
            placementController.CompleteLinePlacement(
                _currentPosition
            );
        }
        else
        {
            placementController.PlaceSingle(
                _startPosition
            );
        }

        ResetPointerState();
    }

    private void CancelPointerAction()
    {
        if (_isDragging)
        {
            placementController.CancelLinePlacement();
        }

        ResetPointerState();
    }

    private bool TryGetMouseWorldPosition(
        out Vector3 worldPosition)
    {
        Ray ray = placementCamera.ScreenPointToRay(
            Input.mousePosition
        );

        if (Physics.Raycast(
                ray,
                out RaycastHit hit,
                Mathf.Infinity,
                placementSurfaceMask,
                QueryTriggerInteraction.Ignore))
        {
            worldPosition = hit.point;
            return true;
        }

        worldPosition = Vector3.zero;
        return false;
    }

    private void ResetPointerState()
    {
        _isPointerDown = false;
        _isDragging = false;

        _startPosition = Vector3.zero;
        _currentPosition = Vector3.zero;
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null &&
               EventSystem.current.IsPointerOverGameObject();
    }
}