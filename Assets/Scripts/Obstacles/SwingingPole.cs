using UnityEngine;

public class SwingingPole : MonoBehaviour
{
    [SerializeField] private float _swingingPoleSpeed = 1;
    [SerializeField] private float _maxRotation = 60;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(_maxRotation * Mathf.Sin(Time.time * _swingingPoleSpeed), transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
