using UnityEngine;

public class Climb : MonoBehaviour
{
    [HideInInspector]
    public bool IsSoftened;

    [SerializeField]
    private Grab _rightGrab;

    [SerializeField]
    private Grab _leftGrab;

    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private Transform _cameraFollowTarget;

    [SerializeField]
    private ConfigurableJoint[] _playerCharacterJoints;

    private float[] _originalJointXDrivePositionSprings;
    private float[] _originalJointYZDrivePositionSprings;

    private float _elapsedNotGroundedTime;

    public bool IsClimbing()
    {
        return _cameraFollowTarget.eulerAngles.x >= 30.0f && _cameraFollowTarget.eulerAngles.x <= 61.0f && _rightGrab.IsGrabingKinematicObject() && _leftGrab.IsGrabingKinematicObject();
    }

    private void Start()
    {
        _rightGrab.OnDoGrab.AddListener(CheckHanging);
        _leftGrab.OnDoGrab.AddListener(CheckHanging);

        _originalJointXDrivePositionSprings = new float[_playerCharacterJoints.Length];
        _originalJointYZDrivePositionSprings = new float[_playerCharacterJoints.Length];

        for (int i = 0; i < _playerCharacterJoints.Length; i++)
        {
            _originalJointXDrivePositionSprings[i] = _playerCharacterJoints[i].angularXDrive.positionSpring;
            _originalJointYZDrivePositionSprings[i] = _playerCharacterJoints[i].angularYZDrive.positionSpring;
        }
    }

    private void Update()
    {
        if (!_playerController.IsGrounded())
        {
            if (IsSoftened && IsClimbing())
            {
                HardenPlayerCharacterJoints();
            }
            else
            {
                _elapsedNotGroundedTime += Time.deltaTime;

                if (!IsSoftened && ((!IsClimbing() && (_rightGrab.IsHanging() || _leftGrab.IsHanging()))
                    || (!_rightGrab.IsHanging() && !_leftGrab.IsHanging() && _elapsedNotGroundedTime > 2)))
                {
                    SoftenPlayerCharacterJoints();
                }
            }
        }
        else
        {
            _elapsedNotGroundedTime = 0;

            if (IsSoftened && !_rightGrab.IsHanging() && !_leftGrab.IsHanging())
            {
                HardenPlayerCharacterJoints();
            }
        }
    }

    private void CheckHanging()
    {
        if (_rightGrab.IsHanging() || _leftGrab.IsHanging())
        {
            SoftenPlayerCharacterJoints();
        }
    }

    private void SoftenPlayerCharacterJoints()
    {
        for (int i = 0; i < _playerCharacterJoints.Length; i++)
        {
            JointDrive jointXDrive = _playerCharacterJoints[i].angularXDrive;
            jointXDrive.positionSpring = 70f;
            _playerCharacterJoints[i].angularXDrive = jointXDrive;

            JointDrive jointYZDrive = _playerCharacterJoints[i].angularYZDrive;
            jointYZDrive.positionSpring = 70f;
            _playerCharacterJoints[i].angularYZDrive = jointYZDrive;
        }

        IsSoftened = true;
    }

    private void HardenPlayerCharacterJoints()
    {
        for (int i = 0; i < _playerCharacterJoints.Length; i++)
        {
            JointDrive jointXDrive = _playerCharacterJoints[i].angularXDrive;
            jointXDrive.positionSpring = _originalJointXDrivePositionSprings[i];
            _playerCharacterJoints[i].angularXDrive = jointXDrive;

            JointDrive jointYZDrive = _playerCharacterJoints[i].angularYZDrive;
            jointYZDrive.positionSpring = _originalJointYZDrivePositionSprings[i];
            _playerCharacterJoints[i].angularYZDrive = jointYZDrive;
        }

        IsSoftened = false;
    }
}
