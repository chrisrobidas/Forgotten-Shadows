using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsGrounded;

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
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _hips.AddForce(_hips.transform.forward * _speed * 1.5f);
            }
            else
            {
                _hips.AddForce(_hips.transform.forward * _speed);
            }
        }

        if (Input.GetKey(KeyCode.A)) 
        {
            _hips.AddForce(-_hips.transform.right * _strafeSpeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            _hips.AddForce(-_hips.transform.forward * _speed);
        }


        if (Input.GetKey(KeyCode.D))
        {
            _hips.AddForce(_hips.transform.right * _strafeSpeed);
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
