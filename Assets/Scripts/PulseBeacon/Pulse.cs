using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    [SerializeField] private float _pulseMoveSpeed = 10.0f;
    [SerializeField] private float _pulseLifeTime = 10.0f;
    private CircleCollider2D _circleCollider;

    // Start is called before the first frame update
    void Start()
    {
        _circleCollider = GetComponent<CircleCollider2D>();

        //a little lazy maybe - just need to auto destroy the pulse.
        Destroy(gameObject, _pulseLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += Vector3.one * (_pulseMoveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.activeInHierarchy) return;

        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(1);
        }
    }
}
