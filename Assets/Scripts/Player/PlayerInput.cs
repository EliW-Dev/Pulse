using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static event Action OnPlayerJump;

    private float _horizontalInputValue = 0.0f;

    public float HorizontalInputVaue
    {
        get => _horizontalInputValue;
    }

    void Update()
    {
        ProcessInputs();
    }

    private void ProcessInputs()
    {
        _horizontalInputValue = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump"))
        {
            OnPlayerJump?.Invoke();
        }
    }
}
