using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsGrounded;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float _speed = 2.0f;

    [SerializeField]
    private float _strafeSpeed;

    [SerializeField]
    private float _jumpForce;

    private Rigidbody _hips;

    private void Start()
    {
        _hips = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
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
            _animator.SetBool("IsMovingLeft", true);
            _hips.AddForce(-_hips.transform.right * _strafeSpeed);
        }
        else
        {
            _animator.SetBool("IsMovingLeft", false);
        }

        if (Input.GetKey(KeyCode.S))
        {
            _animator.SetBool("IsWalking", true);
            _hips.AddForce(-_hips.transform.forward * _speed);
        }
        else if (!Input.GetKey(KeyCode.W))
        {
            _animator.SetBool("IsWalking", false);
        }


        if (Input.GetKey(KeyCode.D))
        {
            _animator.SetBool("IsMovingRight", true);
            _hips.AddForce(_hips.transform.right * _strafeSpeed);
        }
        else
        {
            _animator.SetBool("IsMovingRight", false);
        }

        if (Input.GetAxis("Jump") > 0)
        {
            if (IsGrounded)
            {
                _hips.AddForce(new Vector3(0, _jumpForce, 0));
                IsGrounded = false;
            }
        }
    }
}
