using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private int _isLeftOrRight;

    private Rigidbody _rigidbody;
    private GameObject _grabbedObject;
    private float _grabbedObjectInitialMass;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(_isLeftOrRight))
        {
            if (_isLeftOrRight == 0)
            {
                _animator.SetBool("IsLeftHandUp", true);
            }
            else if (_isLeftOrRight == 1)
            {
                _animator.SetBool("IsRightHandUp", true);
            }

            if (_grabbedObject != null)
            {
                _grabbedObject.GetComponent<Rigidbody>().mass = 0.0001f;

                FixedJoint fixedJoint = _grabbedObject.AddComponent<FixedJoint>();
                fixedJoint.connectedBody = _rigidbody;
                fixedJoint.breakForce = 9001;
            }
        }
        else if (Input.GetMouseButtonUp(_isLeftOrRight))
        {
            if (_isLeftOrRight == 0)
            {
                _animator.SetBool("IsLeftHandUp", false);
            }
            else if (_isLeftOrRight == 1)
            {
                _animator.SetBool("IsRightHandUp", false);
            }

            if (_grabbedObject != null)
            {
                _grabbedObject.GetComponent<Rigidbody>().mass = _grabbedObjectInitialMass;

                Destroy(_grabbedObject.GetComponent<FixedJoint>());
            }

            _grabbedObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_grabbedObject == null && other.gameObject.CompareTag("Item"))
        {
            _grabbedObject = other.gameObject;
            _grabbedObjectInitialMass = _grabbedObject.GetComponent<Rigidbody>().mass;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _grabbedObject = null;
    }
}
