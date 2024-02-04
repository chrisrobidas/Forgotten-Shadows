using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public bool IsBeingMoved;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Collider _rightFootCollider;

    [SerializeField]
    private Collider _leftFootCollider;

    [SerializeField]
    private float _speed = 100f;

    [SerializeField]
    private float _strafeSpeed = 100f;

    [SerializeField]
    private float _jumpForce = 8000f;

    private Rigidbody _hips;
    private float _distanceToGround;

    public bool IsGrounded()
    {
        bool rightFootIsGrounded = Physics.Raycast(_rightFootCollider.transform.position, -Vector3.up, _distanceToGround + 0.1f);
        bool leftFootIsGrounded = Physics.Raycast(_leftFootCollider.transform.position, -Vector3.up, _distanceToGround + 0.1f);
        return rightFootIsGrounded || leftFootIsGrounded;
    }

    private void Start()
    {
        _hips = GetComponent<Rigidbody>();
        _distanceToGround = _rightFootCollider.bounds.extents.y;
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(_rightFootCollider.transform.position, -Vector3.up * (_distanceToGround + 0.1f), Color.blue);
        Debug.DrawRay(_leftFootCollider.transform.position, -Vector3.up * (_distanceToGround + 0.1f), Color.blue);

        IsBeingMoved = false;

        if (Input.GetKey(KeyCode.W))
        {
            IsBeingMoved = true;
            _animator.SetBool("IsWalking", true);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                _animator.SetBool("IsRunning", true);
                _hips.AddForce(_hips.transform.forward * _speed * 1.5f);
            }
            else
            {
                _animator.SetBool("IsRunning", false);
                _hips.AddForce(_hips.transform.forward * _speed);
            }
        }
        else
        {
            _animator.SetBool("IsWalking", false);
            _animator.SetBool("IsRunning", false);
        }

        if (Input.GetKey(KeyCode.A)) 
        {
            IsBeingMoved = true;
            _animator.SetBool("IsMovingLeft", true);
            _hips.AddForce(-_hips.transform.right * _strafeSpeed);
        }
        else
        {
            _animator.SetBool("IsMovingLeft", false);
        }

        if (Input.GetKey(KeyCode.S))
        {
            IsBeingMoved = true;
            _animator.SetBool("IsWalking", true);
            _hips.AddForce(-_hips.transform.forward * _speed);
        }
        else if (!Input.GetKey(KeyCode.W))
        {
            _animator.SetBool("IsWalking", false);
        }


        if (Input.GetKey(KeyCode.D))
        {
            IsBeingMoved = true;
            _animator.SetBool("IsMovingRight", true);
            _hips.AddForce(_hips.transform.right * _strafeSpeed);
        }
        else
        {
            _animator.SetBool("IsMovingRight", false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                _hips.AddForce(new Vector3(0, _jumpForce, 0));
            }
        }
    }
}
