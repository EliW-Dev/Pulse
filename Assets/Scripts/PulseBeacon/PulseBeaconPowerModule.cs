using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PulseBeaconPowerModule : MonoBehaviour
{
    public static event Action<bool> OnBeaconPowerToggle;

    [SerializeField] private GameObject _instructionPopup;
    [SerializeField] private TextMeshProUGUI _instructionText;

    [Header("Instructions Text")]
    [SerializeField] private string _deactivateMessage = "[ E ]\nDisable Beacon";

    private bool _playerInRange = false;

    private void Start()
    {
        if(_instructionPopup == null)
        {
            _instructionPopup = (GameObject)transform.Find("Interact_Popup").gameObject;
        }
        if(_instructionPopup && _instructionText == null)
        {
            _instructionText = _instructionPopup.GetComponentInChildren<TextMeshProUGUI>();
        }

        _instructionPopup.SetActive(false);
        _playerInRange = false;
    }

    void Update()
    {
        if (!_playerInRange) return;

        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetButtonDown("Interact"))
        {
            OnBeaconPowerToggle?.Invoke(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerInRange = true;
        _instructionPopup.SetActive(true);
        SetPopupText(_deactivateMessage);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _playerInRange = false;
        _instructionPopup.SetActive(false);
    }

    //not really needed at the moment, but just incase we need message variation later.
    private void SetPopupText(string message)
    {
        if(_instructionText == null) { return; }

        _instructionText.text = message;
    }
}
