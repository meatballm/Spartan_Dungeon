using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class MovingPlatform : MonoBehaviour
{
    [Header("Path")]
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 endPoint;
    [SerializeField] private float moveDuration = 3f;

    private Rigidbody _rb;
    private Vector3 _lastPosition;
    private Vector3 _deltaPosition;
    private List<Rigidbody> _passengers = new List<Rigidbody>();

    private void Reset()
    {
        startPoint = transform.position;
        endPoint = transform.position + Vector3.right * 5f;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
    }

    private void Start()
    {
        _lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        float t = Mathf.PingPong(Time.time / moveDuration, 1f);
        Vector3 newPos = Vector3.Lerp(startPoint, endPoint, t);
        _deltaPosition = newPos - _lastPosition;
        _rb.MovePosition(newPos);

        foreach (var passengerRb in _passengers)
        {
            Vector3 platformV = _deltaPosition / Time.fixedDeltaTime;
            passengerRb.AddForce(platformV, ForceMode.VelocityChange);
        }

        _lastPosition = newPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var rb = collision.rigidbody;
        if (rb != null && collision.gameObject.CompareTag("Player"))
            if (!_passengers.Contains(rb))
                _passengers.Add(rb);
    }

    private void OnCollisionExit(Collision collision)
    {
        var rb = collision.rigidbody;
        if (rb != null && collision.gameObject.CompareTag("Player"))
            _passengers.Remove(rb);
    }
}
