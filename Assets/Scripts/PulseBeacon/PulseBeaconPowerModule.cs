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
    [SerializeField] private string _disabledMessage = "Beacon\nDisabled!";

    [Header("Button Emissive")]
    [SerializeField] private float _minButtonEmissiveIntencity = 1.5f;
    [SerializeField] private float _maxButtonEmissiveIntencity = 5.5f;
    [SerializeField] private float _emissivePulseTime = 3.0f;

    private bool _playerInRange = false;
    private bool _isActive = false;
    private Material _material;
    private int _materialEmissiveProperty;

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
        _isActive = true;

        //material setup
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        if (renderer != null)
        {
            _material = renderer.material;
            _materialEmissiveProperty = Shader.PropertyToID("_Intencity");
        }

        _material.SetFloat(_materialEmissiveProperty, _minButtonEmissiveIntencity);
    }

    void Update()
    {
        if (!_isActive) return;

        UpdateEmissiveMaterial();
        
        if (!_playerInRange) return;

        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetButtonDown("Interact"))
        {
            OnBeaconPowerToggle?.Invoke(false);

            _instructionPopup.SetActive(false);
            _isActive = false;
            UpdateEmissiveMaterial(false); //disable the button emissive "pulse"
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerInRange = true;
        _instructionPopup.SetActive(true);
        SetPopupText(_isActive ? _deactivateMessage : _disabledMessage);
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

    //Update the Emissive Material on this GO.
    private void UpdateEmissiveMaterial(bool isActive = true)
    {
        //letting the shader handle this would probably be better - have a bool to enable/disable the emissive "pulse"
        float step = Mathf.Sin(Time.time * _emissivePulseTime);
        float emissiveValue = isActive ? Mathf.Lerp(_minButtonEmissiveIntencity, _maxButtonEmissiveIntencity, step) : _minButtonEmissiveIntencity / 2.0f;

        _material.SetFloat(_materialEmissiveProperty, emissiveValue);
    }
}
