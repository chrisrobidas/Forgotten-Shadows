using UnityEngine;

public class CopyMotion : MonoBehaviour
{
    [SerializeField]
    private Transform targetLimb;

    private ConfigurableJoint configurableJoint;
    private Quaternion initialRotation;

    private void Start()
    {
        configurableJoint = GetComponent<ConfigurableJoint>();
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        configurableJoint.targetRotation = Quaternion.Inverse(targetLimb.rotation * initialRotation);
    }
}
