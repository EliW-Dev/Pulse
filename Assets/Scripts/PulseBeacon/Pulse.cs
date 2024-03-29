using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    [SerializeField] private float _pulseMoveSpeed = 10.0f;
    [SerializeField] private float _pulseLifeTime = 10.0f;
    private CircleCollider2D _circleCollider;

    void Start()
    {
        _circleCollider = GetComponent<CircleCollider2D>();

        GameManager.OnGameStateChanged += GameStateChanged;

        //a little lazy maybe - just need to auto destroy the pulse.
        Destroy(gameObject, _pulseLifeTime);
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= GameStateChanged;
    }

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

    private void GameStateChanged(EGameState gameState)
    {
        this.Invoke("DisablePulse", 0.75f);
    }

    private void DisablePulse()
    {
        gameObject.SetActive(false);
    }
}
