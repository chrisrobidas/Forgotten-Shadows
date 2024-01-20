using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private Transform _cameraFollowTargetTransform;

    [SerializeField]
    private ConfigurableJoint _configurableJoint;

    [SerializeField]
    private Vector3 _jointOffset;

    [SerializeField]
    private int _isLeftOrRight;

    [SerializeField]
    private float _speed = 7f;

    private Rigidbody _handRigidbody;
    private Quaternion _initialTargetRotation;

    private GameObject _grabbedObject;
    private FixedJoint _grabbedObjectJoint;
    private Rigidbody _grabbedObjectRigidbody;
    private float _grabbedObjectInitialMass;

    private void Start()
    {
        _handRigidbody = GetComponent<Rigidbody>();
        _initialTargetRotation = _configurableJoint.targetRotation;
    }

    private void Update()
    {
        if (Input.GetMouseButton(_isLeftOrRight))
        {
            Vector3 newTargetRotation = _cameraFollowTargetTransform.forward + _jointOffset;

            float jointRotationModifier = 90.0f;

            if (_playerController.IsBeingMoved)
            {
                jointRotationModifier = 360f;
            }

            if (newTargetRotation.y >= 0)
            {
                newTargetRotation.y = -Mathf.Sqrt(newTargetRotation.y) * jointRotationModifier;
            }
            else
            {
                newTargetRotation.y = Mathf.Sqrt(-newTargetRotation.y) * (jointRotationModifier / 2);
            }

            _configurableJoint.targetRotation = Quaternion.Lerp(_configurableJoint.targetRotation, Quaternion.LookRotation(newTargetRotation), Time.deltaTime * _speed);

            JointDrive jointXDrive = _configurableJoint.angularXDrive;
            jointXDrive.positionSpring = 1000f;
            _configurableJoint.angularXDrive = jointXDrive;

            JointDrive jointYZDrive = _configurableJoint.angularYZDrive;
            jointYZDrive.positionSpring = 1000f;
            _configurableJoint.angularYZDrive = jointYZDrive;

            if (_grabbedObject != null && _grabbedObjectJoint == null)
            {
                if (_grabbedObjectRigidbody != null)
                {
                    _grabbedObjectRigidbody.mass = 0.0001f;
                }

                _grabbedObjectJoint = _grabbedObject.AddComponent<FixedJoint>();
                _grabbedObjectJoint.connectedBody = _handRigidbody;
                _grabbedObjectJoint.breakForce = 9001;
            }
        }
        else
        {
            _configurableJoint.targetRotation = Quaternion.Lerp(_configurableJoint.targetRotation, _initialTargetRotation, Time.deltaTime * _speed);

            JointDrive jointXDrive = _configurableJoint.angularXDrive;
            jointXDrive.positionSpring = 1f;
            _configurableJoint.angularXDrive = jointXDrive;

            JointDrive jointYZDrive = _configurableJoint.angularYZDrive;
            jointYZDrive.positionSpring = 1f;
            _configurableJoint.angularYZDrive = jointYZDrive;

            if (_grabbedObject != null && _grabbedObjectJoint != null)
            {
                if (_grabbedObjectRigidbody != null)
                {
                    _grabbedObjectRigidbody.mass = _grabbedObjectInitialMass;
                }

                Destroy(_grabbedObjectJoint);

                _grabbedObject = null;
                _grabbedObjectJoint = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_grabbedObject == null && !other.gameObject.CompareTag("Player"))
        {
            _grabbedObject = other.gameObject;

            _grabbedObjectRigidbody = _grabbedObject.GetComponent<Rigidbody>();
            if (_grabbedObjectRigidbody != null)
            {
                _grabbedObjectInitialMass = _grabbedObjectRigidbody.mass;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_grabbedObjectJoint == null)
        {
            _grabbedObject = null;
        }
    }
}
