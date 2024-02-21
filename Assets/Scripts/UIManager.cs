using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager current;

    private UIDocument _mainHUD;
    private ProgressBar _maxShieldBar;
    private ProgressBar _currentShieldBar;

    private void Awake()
    {
        //there can be only one! destroy this gameObject if an instance of GameManager already exists.
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }

        current = this;

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        _mainHUD = GetComponentInChildren<UIDocument>();

        if(_mainHUD == null)
        {
            Debug.LogError("mainHUD not found!");
        }

        _maxShieldBar = _mainHUD.rootVisualElement.Q("maxShieldValue") as ProgressBar;
        _currentShieldBar = _mainHUD.rootVisualElement.Q("currentShieldValue") as ProgressBar;

        _maxShieldBar.value = 0;
        _currentShieldBar.value = 0;
    }

    public void UpdateMaxShieldBar(int value)
    {
        _maxShieldBar.value = (float)value;
    }

    public void UpdateCurrentShieldBar(int value)
    {
        _currentShieldBar.value = (float)value;
    }
}
