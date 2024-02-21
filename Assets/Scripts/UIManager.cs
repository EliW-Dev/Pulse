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

    //TODO
    //Instuctions/info popup
    //Screen fade panel

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

        if (_maxShieldBar == null)
        {
            Debug.LogError("maxShieldBar not found!");
        }

        if (_currentShieldBar == null)
        {
            Debug.LogError("currentShieldBar not found!");
        }

        UpdateMaxShieldBar(0);
        UpdateCurrentShieldBar(0);
    }

    public void UpdateMaxShieldBar(int value)
    {
        if (_maxShieldBar == null) return;

        _maxShieldBar.value = (float)value;
    }

    public void UpdateCurrentShieldBar(int value)
    {
        if (_currentShieldBar == null) return;

        _currentShieldBar.value = (float)value;
    }
}
