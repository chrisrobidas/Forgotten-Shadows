using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 0.001f;

    [SerializeField] private Climb _playerCharacterClimb;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Transform _cameraFollowTarget;
    [SerializeField] private ConfigurableJoint _hipJoint;
    [SerializeField] private ConfigurableJoint _stomachJoint;

    private Vector2 _rawLookInput;
    private float _mouseX;
    private float _mouseY;
    private bool _isDeltaInput;

    private Vector3 _cameraInitialLocalPosition;

    public void OnLookInput(CallbackContext context)
    {
        if (context.action.activeControl.device.name == "Mouse")
        {
            _isDeltaInput = true;
            _rawLookInput += context.ReadValue<Vector2>();
        }
        else
        {
            _isDeltaInput = false;
            _rawLookInput = context.ReadValue<Vector2>();
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = -1;
        Cursor.lockState = CursorLockMode.Locked;
        _cameraInitialLocalPosition = transform.localPosition;
    }

    private void FixedUpdate()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        // Camera rotation
        _mouseX += (_rawLookInput.x * _rotationSpeed) / Time.fixedDeltaTime;
        _mouseY -= (_rawLookInput.y * _rotationSpeed) / Time.fixedDeltaTime;

        if (_isDeltaInput)
        {
            _rawLookInput = Vector3.zero;
        }

        float cameraLerpSpeed = 10f;

        // Adjust camera angle when moving and not climbing
        if (_playerController.IsBeingMoved && _playerController.IsGrounded() && !_playerCharacterClimb.IsClimbing())
        {
            if (!(_cameraFollowTarget.eulerAngles.x >= 330.0f && _cameraFollowTarget.eulerAngles.x <= 359.0f))
            {
                cameraLerpSpeed = 1.5f;
            }

            _mouseY = Mathf.Clamp(_mouseY, -30, -1);
        }
        else
        {
            _mouseY = Mathf.Clamp(_mouseY, -60, 60);
        }

        _cameraFollowTarget.rotation = Quaternion.Euler(_cameraFollowTarget.eulerAngles.x, _mouseX, 0);
        _cameraFollowTarget.rotation = Quaternion.Lerp(_cameraFollowTarget.rotation, Quaternion.Euler(_mouseY, _mouseX, 0), Time.deltaTime * cameraLerpSpeed);

        // Player body rotation depends on camera
        float adjustedMouseY = _mouseY < 0 ? -1 + (_mouseY * 0.08f) : 1 + (_mouseY * 0.08f);
        adjustedMouseY = adjustedMouseY < 0 ? -adjustedMouseY : adjustedMouseY;
        adjustedMouseY = Mathf.Sqrt(adjustedMouseY);
        adjustedMouseY = Mathf.Clamp(_mouseY * adjustedMouseY, -30, 90);

        _hipJoint.targetRotation = Quaternion.Euler(0, -_mouseX, 0);
        _stomachJoint.targetRotation = Quaternion.Euler(-adjustedMouseY, 0, 0);

        // Camera collision with environment
        int layerMask = ~((1 << LayerMask.NameToLayer("NoSelfCollision")) | (1 << LayerMask.NameToLayer("NoCameraCollision")));

        Debug.DrawLine(_cameraFollowTarget.position, _cameraFollowTarget.position + _cameraFollowTarget.localRotation * _cameraInitialLocalPosition, Color.red);

        if (Physics.Linecast(_cameraFollowTarget.position, _cameraFollowTarget.position + _cameraFollowTarget.localRotation * _cameraInitialLocalPosition, out RaycastHit hit, layerMask))
        {
            transform.position = hit.point + _cameraFollowTarget.localRotation * new Vector3(0, 0, 0.1f);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _cameraInitialLocalPosition, Time.deltaTime);
        }
    }
}
