using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private WaypointPath _waypointPath;
    [SerializeField] private int _initialWaypointIndex = 0;
    [SerializeField] private float _speed = 7;
    [SerializeField] private float _platformVelocityIntensity = 600;

    private int _targetWaypointIndex;

    private Transform _previousWaypoint;
    private Transform _nextWaypoint;

    private float _timeToWaypoint;
    private float _elapsedTime;

    private Vector3 _previousPosition;
    private Vector3 _platformVelocity;

    private void Start()
    {
        _previousPosition = transform.position;

        _targetWaypointIndex = _initialWaypointIndex;
        TargetNextWaypoint();
    }

    private void FixedUpdate()
    {
        _elapsedTime += Time.deltaTime;

        float elapsedPercentage = _elapsedTime / _timeToWaypoint;
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        transform.position = Vector3.Lerp(_previousWaypoint.position, _nextWaypoint.position, elapsedPercentage);
        transform.rotation = Quaternion.Lerp(_previousWaypoint.rotation, _nextWaypoint.rotation, elapsedPercentage);

        _platformVelocity = (transform.position - _previousPosition) / Time.fixedDeltaTime;
        _previousPosition = transform.position;

        if (elapsedPercentage >= 1 )
        {
            TargetNextWaypoint();
        }
    }

    private void TargetNextWaypoint()
    {
        _previousWaypoint = _waypointPath.GetWaypoint(_targetWaypointIndex);
        _targetWaypointIndex = _waypointPath.GetNextWaypointIndex(_targetWaypointIndex);
        _nextWaypoint = _waypointPath.GetWaypoint(_targetWaypointIndex);

        _elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(_previousWaypoint.position, _nextWaypoint.position);
        _timeToWaypoint = distanceToWaypoint / _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.PLAYER_FOOT_TAG))
        {
            other.transform.parent.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.PLAYER_FOOT_TAG))
        {
            other.transform.parent.transform.SetParent(null);
            other.transform.parent.Find("Hips").GetComponent<Rigidbody>().AddForce(_platformVelocity * _platformVelocityIntensity);
        }
    }
}
