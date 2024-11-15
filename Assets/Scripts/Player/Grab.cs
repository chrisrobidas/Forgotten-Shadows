using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

public class Grab : MonoBehaviour
{
    [HideInInspector] public GameObject GrabbedObject;
    [HideInInspector] public UnityEvent OnDoGrab;

    [SerializeField] private Climb _playerCharacterClimb;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Transform _cameraFollowTargetTransform;
    [SerializeField] private ConfigurableJoint _configurableJoint;

    [SerializeField] private Vector3 _jointOffset;

    [SerializeField] private float _speed = 7f;

    private Rigidbody _handRigidbody;
    private Quaternion _initialTargetRotation;

    private bool _isHoldingGrabInput;
    private FixedJoint _grabbedObjectJoint;
    private Rigidbody _grabbedObjectRigidbody;
    private float _grabbedObjectInitialMass;
    private bool _isGrabbedObjectKinematic;
    private bool _isGrabbedObjectARope;

    public bool IsGrabbing()
    {
        return _grabbedObjectJoint != null && GrabbedObject != null;
    }

    public bool IsHanging()
    {
        return IsGrabbing() && (_isGrabbedObjectKinematic || _isGrabbedObjectARope) && !_playerController.IsGrounded();
    }

    public bool IsGrabingKinematicObject()
    {
        return IsGrabbing() && _isGrabbedObjectKinematic;
    }

    public void OnGrab(CallbackContext context)
    {
        if (context.started)
        {
            _isHoldingGrabInput = true;
        }
        else if (context.canceled)
        {
            _isHoldingGrabInput = false;
        }
    }

    private void Start()
    {
        _handRigidbody = GetComponent<Rigidbody>();
        _initialTargetRotation = _configurableJoint.targetRotation;
    }

    private void Update()
    {
        if (_isHoldingGrabInput)
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
            newTargetRotation.y = Mathf.Sqrt(-newTargetRotation.y) * jointRotationModifier;
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
            if (_grabbedObjectRigidbody != null && _grabbedObjectRigidbody.CompareTag(Constants.ITEM_TAG))
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
            if (_grabbedObjectRigidbody != null && _grabbedObjectRigidbody.CompareTag(Constants.ITEM_TAG))
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
        if (GrabbedObject == null && !other.gameObject.CompareTag(Constants.PLAYER_TAG) && !other.gameObject.CompareTag(Constants.PLAYER_FOOT_TAG) && !other.isTrigger)
        {
            GrabbedObject = other.gameObject;
            _isGrabbedObjectARope = GrabbedObject.CompareTag(Constants.ROPE_TAG);
            _grabbedObjectRigidbody = GrabbedObject.GetComponent<Rigidbody>();

            if (_grabbedObjectRigidbody != null)
            {
                _grabbedObjectInitialMass = _grabbedObjectRigidbody.mass;
                _isGrabbedObjectKinematic = _grabbedObjectRigidbody.isKinematic;
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
