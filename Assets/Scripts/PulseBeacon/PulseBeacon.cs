using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseBeacon : MonoBehaviour
{
    [SerializeField] GameObject _pulsePrefab;
    [SerializeField] private Transform _pulseSpawnPoint;
    [SerializeField] private float _pulseDelay = 0.0f;
    private float _timeSinceLastPulse = 0.0f;

    private bool _beaconActive = true;

    // Start is called before the first frame update
    void Start()
    {
        _beaconActive = true;
        _timeSinceLastPulse = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_beaconActive) return;

        _timeSinceLastPulse += Time.deltaTime;
        if(_timeSinceLastPulse > _pulseDelay)
        {
            SpawnPulse();
            _timeSinceLastPulse = 0.0f;
        }
    }

    private void SpawnPulse()
    {
        if (_pulsePrefab == null) return;

        GameObject pulse = (GameObject)Instantiate(_pulsePrefab);
        pulse.transform.position = _pulseSpawnPoint.position;
    }
}
