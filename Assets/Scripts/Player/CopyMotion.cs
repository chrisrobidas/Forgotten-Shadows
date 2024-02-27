using UnityEngine;

public class CopyMotion : MonoBehaviour
{
    [SerializeField] private Transform _targetLimb;

    private ConfigurableJoint _configurableJoint;
    private Quaternion _initialRotation;

    private void Start()
    {
        _configurableJoint = GetComponent<ConfigurableJoint>();
        _initialRotation = transform.localRotation;
    }

    private void Update()
    {
        _configurableJoint.targetRotation = Quaternion.Inverse(_targetLimb.rotation * _initialRotation);
    }
}
