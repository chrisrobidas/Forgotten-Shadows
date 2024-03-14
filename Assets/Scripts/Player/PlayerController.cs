using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public bool IsBeingMoved;

    [SerializeField] private Animator _animator;
    [SerializeField] private Collider _rightFootCollider;
    [SerializeField] private Collider _leftFootCollider;
    [SerializeField] private Rigidbody _hips;

    [SerializeField] private float _speed = 100f;
    [SerializeField] private float _strafeSpeed = 100f;
    [SerializeField] private float _jumpForce = 150f;
    
    private float _distanceToGround;

    private Vector3 _rawMoveInput;
    private bool _isSprinting;

    public bool IsGrounded()
    {
        bool rightFootIsGrounded = Physics.Raycast(_rightFootCollider.transform.position, -Vector3.up, _distanceToGround + 0.1f);
        bool leftFootIsGrounded = Physics.Raycast(_leftFootCollider.transform.position, -Vector3.up, _distanceToGround + 0.1f);
        return rightFootIsGrounded || leftFootIsGrounded;
    }

    public void OnMoveInput(CallbackContext context)
    {
        Vector2 axis = context.ReadValue<Vector2>();
        _rawMoveInput = new Vector3(axis.x, 0, axis.y);
        IsBeingMoved = axis.x != 0 || axis.y != 0;

        if (axis.y <= 0)
        {
            _isSprinting = false;
        }
    }

    public void OnSprintInput(CallbackContext context)
    {
        if (context.started)
        {
            _isSprinting = !_isSprinting;
        }
    }

    public void OnJumpInput(CallbackContext context)
    {
        if (context.started)
        {
            Jump();
        }
    }

    public void OnPauseInput(CallbackContext context)
    {
        if (context.started)
        {
            UIManager.Instance.ShowPauseMenuPanel();
        }
    }

    private void Start()
    {
        _distanceToGround = _rightFootCollider.bounds.extents.y;
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(_rightFootCollider.transform.position, -Vector3.up * (_distanceToGround + 0.1f), Color.blue);
        Debug.DrawRay(_leftFootCollider.transform.position, -Vector3.up * (_distanceToGround + 0.1f), Color.blue);

        Move();
    }

    private void Move()
    {
        Vector3 forceToAdd = _rawMoveInput;
        forceToAdd.x = forceToAdd.x * _strafeSpeed;
        forceToAdd.z = forceToAdd.z * _speed;

        if (_isSprinting && forceToAdd.z > 0)
        {
            forceToAdd.z = _speed * 1.5f;
        }

        _hips.AddForce(_hips.transform.TransformDirection(forceToAdd));
        _animator.SetFloat("Velocity X", forceToAdd.x / _strafeSpeed);
        _animator.SetFloat("Velocity Z", forceToAdd.z / _speed);
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            _hips.AddForce(new Vector3(0, _jumpForce, 0), ForceMode.Impulse);
        }
    }
}
