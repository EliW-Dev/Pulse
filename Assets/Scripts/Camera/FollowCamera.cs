using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _camOffset;
    [SerializeField] private float _smoothDelta = 5.0f;
    private Vector3 _velocity = Vector2.zero;

    
    void FixedUpdate()
    {
        if (_target == null) return;

        Vector3 targetPosition = _target.position + _camOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothDelta * Time.deltaTime);
    }
}
