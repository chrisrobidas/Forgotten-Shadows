using UnityEngine;
using UnityEngine.Events;

public class Grab : MonoBehaviour
{
    [HideInInspector]
    public GameObject GrabbedObject;

    [HideInInspector]
    public UnityEvent OnDoGrab;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Climb _playerCharacterClimb;

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

    private FixedJoint _grabbedObjectJoint;
    private Rigidbody _grabbedObjectRigidbody;
    private float _grabbedObjectInitialMass;
    private bool _grabbedObjectIsKinematic;

    public bool IsGrabbing()
    {
        return _grabbedObjectJoint != null && GrabbedObject != null;
    }

    public bool IsHanging()
    {
        return IsGrabbing() && _grabbedObjectIsKinematic && !_playerController.IsGrounded();
    }

    private void Start()
    {
        _handRigidbody = GetComponent<Rigidbody>();
        _initialTargetRotation = _configurableJoint.targetRotation;
    }

    private void Update()
    {
        if (Input.GetMouseButton(_isLeftOrRight))
        {
            if (IsHanging() && !_playerCharacterClimb.IsClimbing()) return;
            DoGrab();
        }
        else
        {
            ReleaseGrab();
        }
    }

    private void DoGrab()
    {
        Vector3 newTargetRotation = _cameraFollowTargetTransform.forward + _jointOffset;

        float jointRotationModifier = 90.0f;

        if (_playerController.IsBeingMoved)
        {
            jointRotationModifier = 270f;
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

        float shoulderSpringForce = 50f;

        if (_playerCharacterClimb.IsClimbing())
        {
            shoulderSpringForce = 1000f;
        }

        JointDrive jointXDrive = _configurableJoint.angularXDrive;
        jointXDrive.positionSpring = shoulderSpringForce;
        _configurableJoint.angularXDrive = jointXDrive;

        JointDrive jointYZDrive = _configurableJoint.angularYZDrive;
        jointYZDrive.positionSpring = shoulderSpringForce;
        _configurableJoint.angularYZDrive = jointYZDrive;

        if (GrabbedObject != null && _grabbedObjectJoint == null)
        {
            if (_grabbedObjectRigidbody != null)
            {
                _grabbedObjectRigidbody.mass = 0.0001f;
            }

            _grabbedObjectJoint = GrabbedObject.AddComponent<FixedJoint>();
            _grabbedObjectJoint.connectedBody = _handRigidbody;
            _grabbedObjectJoint.breakForce = 9001;

            OnDoGrab.Invoke();
        }
    }

    private void ReleaseGrab()
    {
        _configurableJoint.targetRotation = Quaternion.Lerp(_configurableJoint.targetRotation, _initialTargetRotation, Time.deltaTime * _speed);

        JointDrive jointXDrive = _configurableJoint.angularXDrive;
        jointXDrive.positionSpring = 1f;
        _configurableJoint.angularXDrive = jointXDrive;

        JointDrive jointYZDrive = _configurableJoint.angularYZDrive;
        jointYZDrive.positionSpring = 1f;
        _configurableJoint.angularYZDrive = jointYZDrive;

        if (GrabbedObject != null && _grabbedObjectJoint != null)
        {
            if (_grabbedObjectRigidbody != null)
            {
                _grabbedObjectRigidbody.mass = _grabbedObjectInitialMass;
            }

            Destroy(_grabbedObjectJoint);

            GrabbedObject = null;
            _grabbedObjectJoint = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GrabbedObject == null && !other.gameObject.CompareTag("Player"))
        {
            GrabbedObject = other.gameObject;
            _grabbedObjectIsKinematic = GrabbedObject.GetComponent<Rigidbody>().isKinematic;

            _grabbedObjectRigidbody = GrabbedObject.GetComponent<Rigidbody>();
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
            GrabbedObject = null;
        }
    }
}
