using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private float _spinnerSpeed = 100;

    private void FixedUpdate()
    {
        transform.Rotate(0, _spinnerSpeed * Time.deltaTime, 0);
    }
}
