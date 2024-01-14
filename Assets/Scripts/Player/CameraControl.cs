using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 1.0f;

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
        _mouseY = Mathf.Clamp(_mouseY, -60, 60);

        _cameraFollowTarget.rotation = Quaternion.Euler(_mouseY, _mouseX, 0);

        _hipJoint.targetRotation = Quaternion.Euler(0, -_mouseX, 0);
        _stomachJoint.targetRotation = Quaternion.Euler(-_mouseY, 0, 0);

        int layerMask = ~(1 << LayerMask.NameToLayer("NoSelfCollision"));

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
