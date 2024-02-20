using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseBeacon : MonoBehaviour
{
    [SerializeField] GameObject _pulsePrefab;
    [SerializeField] private Transform _pulseSpawnPoint;

    private float _pulseDelay = 0.0f;
    private float _timeSinceLastPulse = 0.0f;

    private bool _beaconActive = false;

    void Start()
    {
        _beaconActive = false;

        PulseBeaconPowerModule.OnBeaconPowerToggle += ToggleBeaconActive;
    }

    private void OnDestroy()
    {
        PulseBeaconPowerModule.OnBeaconPowerToggle -= ToggleBeaconActive;
    }

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

    public void SetBeaconState(bool beaconActive, float pulseDelay = 0.0f)
    {
        _beaconActive = beaconActive;
        _pulseDelay = pulseDelay;
        _timeSinceLastPulse = 0.0f;
    }

    private void SpawnPulse()
    {
        if (_pulsePrefab == null) return;

        //I would normally use a Object Pool for something like this, but we have 1 pulse every few seconds, so this is fine.
        GameObject pulse = (GameObject)Instantiate(_pulsePrefab);
        pulse.transform.position = _pulseSpawnPoint.position;
    }

    private void ToggleBeaconActive(bool isActive)
    {
        _beaconActive = isActive;
        _timeSinceLastPulse = 0.0f;
    }
}
