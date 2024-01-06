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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        _mouseX += Input.GetAxis("Mouse X") * _rotationSpeed;
        _mouseY -= Input.GetAxis("Mouse Y") * _rotationSpeed;
        _mouseY = Mathf.Clamp(_mouseY, -35, 60);

        Quaternion cameraFollowTargetRotation = Quaternion.Euler(_mouseY, _mouseX, 0);

        _cameraFollowTarget.rotation = cameraFollowTargetRotation;

        _hipJoint.targetRotation = Quaternion.Euler(0, -_mouseX, 0);
        _stomachJoint.targetRotation = Quaternion.Euler(-_mouseY, 0, 0);
    }
}
