using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 1.0f;

    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private Transform _cameraFollowTarget;

    [SerializeField]
    private ConfigurableJoint _hipJoint;

    [SerializeField]
    private ConfigurableJoint _stomachJoint;

    private float _mouseX;
    private float _mouseY;
    private Vector3 _cameraInitialLocalPosition;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _cameraInitialLocalPosition = transform.localPosition;
    }

    private void FixedUpdate()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        _mouseX += Input.GetAxis("Mouse X") * _rotationSpeed;
        _mouseY -= Input.GetAxis("Mouse Y") * _rotationSpeed;

        float cameraLerpSpeed = 10f;

        if (_playerController.IsBeingMoved)
        {
            if (!(_cameraFollowTarget.eulerAngles.x >= 330.0f && _cameraFollowTarget.eulerAngles.x <= 359.0f))
            {
                cameraLerpSpeed = 3f;
            }

            _mouseY = Mathf.Clamp(_mouseY, -30, -1);
        }
        else
        {
            _mouseY = Mathf.Clamp(_mouseY, -60, 60);
        }

        _cameraFollowTarget.rotation = Quaternion.Euler(_cameraFollowTarget.eulerAngles.x, _mouseX, 0);
        _cameraFollowTarget.rotation = Quaternion.Lerp(_cameraFollowTarget.rotation, Quaternion.Euler(_mouseY, _mouseX, 0), Time.deltaTime * cameraLerpSpeed);

        _hipJoint.targetRotation = Quaternion.Euler(0, -_mouseX, 0);
        _stomachJoint.targetRotation = Quaternion.Euler(-_mouseY, 0, 0);

        int layerMask = ~((1 << LayerMask.NameToLayer("NoSelfCollision")) | (1 << LayerMask.NameToLayer("NoSelfCameraCollision")));

        Debug.DrawLine(_cameraFollowTarget.position, _cameraFollowTarget.position + _cameraFollowTarget.localRotation * _cameraInitialLocalPosition, Color.red);

        if (Physics.Linecast(_cameraFollowTarget.position, _cameraFollowTarget.position + _cameraFollowTarget.localRotation * _cameraInitialLocalPosition, out RaycastHit hit, layerMask))
        {
            transform.position = hit.point + _cameraFollowTarget.localRotation * new Vector3(0, 0, 0.1f);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _cameraInitialLocalPosition, Time.deltaTime);
        }

        transform.LookAt(_cameraFollowTarget);
    }
}
