using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 camOffset;
    [SerializeField] private float smoothDelta = 5.0f;
    private Vector3 velocity = Vector2.zero;

    
    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + camOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothDelta * Time.deltaTime);
    }
}
