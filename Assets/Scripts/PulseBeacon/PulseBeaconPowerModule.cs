using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseBeaconPowerModule : MonoBehaviour
{
    public static event Action OnBeaconPowerToggle;

    private bool _playerInRange = false;

    void Update()
    {
        if (!_playerInRange) return;

        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetButtonDown("Interact"))
        {
            OnBeaconPowerToggle?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerInRange = true;

        Debug.Log("Beacon Control: player in range - hit 'E' to interact.");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _playerInRange = false;
    }
}
