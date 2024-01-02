using UnityEngine;

public class LimbCollision : MonoBehaviour
{
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = FindAnyObjectByType<PlayerController>().GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _playerController.IsGrounded = true;
    }
}
