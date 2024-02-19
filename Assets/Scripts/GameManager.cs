using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    [SerializeField] private int _playerMaxShieldLevel = 10;
    //the player's starting shield strength for the current level - increment through game-play, not reset on death. 
    private int _playerShieldLevelRef = 0; //TODO - read/write to json on death/respawn.

    private void Awake()
    {
        //there can be only one! destroy this gameObject if an instance of GameManager already exists.
        if(current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }

        current = this;

        DontDestroyOnLoad(gameObject);
    }

    public int IncrementPlayerShieldLevel(int addValue)
    {
        _playerShieldLevelRef += addValue;

        _playerShieldLevelRef = Mathf.Min(_playerMaxShieldLevel, _playerShieldLevelRef);

        //TODO - save data

        return _playerShieldLevelRef;
    }

    //using this instead of a Property to allow for saved data check 
    public int GetPlayerShieldLevel()
    {
        //TODO - read from save file, check if ref is higher than saved (something went wrong)

        return _playerShieldLevelRef;
    }
}
