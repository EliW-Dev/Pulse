using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum EGameState
{
    Paused,
    GameStartCountdown,
    GameActive,
    PlayerDead,
    GameOver_Fail,
    GameOver_Success,
    MAX
}

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    public static event Action<EGameState> OnGameStateChanged;

    [SerializeField] private int _maxShieldLevel = 0;
    [SerializeField] private int _playerStartingShieldLevel = 0;
    [SerializeField] private float _pulseBeaconPulseDelay = 3.0f;

    [Header("Player")]
    [SerializeField] private Player _player;
    [SerializeField] private Transform _playerStart;

    [Header("Game Setup")]
    [SerializeField] private float _gameStartDelay = 2.0f;

    //the player's starting shield strength for the current level - increment through game-play, not reset on death. 
    private int _playerShieldLevelRef = 0; //TODO - read/write to json on death/respawn.

    private PulseBeacon _pulseBeacon;
    private float _gameStartCountdown = 0.0f;

    //game state
    private EGameState _gameState = EGameState.Paused;

    public EGameState GameState
    {
        get => _gameState;
    }

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

    private void Start()
    {
        if(_pulseBeacon == null)
        {
            _pulseBeacon = GameObject.FindGameObjectWithTag("PulseBeacon").GetComponent<PulseBeacon>();
        }
        if(_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        _gameState = EGameState.Paused;

        SetupLevel();
    }

    private void Update()
    {
        if(_gameState == EGameState.GameStartCountdown)
        {
            _gameStartCountdown += Time.deltaTime;

            if(_gameStartCountdown >= _gameStartDelay)
            {
                SetGameState(EGameState.GameActive);

                RespawnPlayer();
                StartGameRound();
            }
        }
        
    }

    void SetupLevel()
    {
        _pulseBeacon.SetBeaconState(false);
        _player.gameObject.SetActive(false);
        _gameStartCountdown = 0.0f;
        _playerShieldLevelRef = _playerStartingShieldLevel;
        UIManager.current.UpdateMaxShieldBar(_playerStartingShieldLevel);
        UIManager.current.UpdateCurrentShieldBar(0);
        SetGameState(EGameState.GameStartCountdown);
    }

    //spawn the player on game start and respawn on player death
    void RespawnPlayer()
    {
        _player.transform.position = _playerStart.position;
        _player.gameObject.SetActive(true);
        _player.SetupPlayer(_playerShieldLevelRef);
        UIManager.current.UpdateCurrentShieldBar(_playerShieldLevelRef);

        //Debug.Log(string.Format("Set Player Shield Level! {0}", _playerShieldLevelRef));
    }

    void StartGameRound()
    {
        _pulseBeacon.SetBeaconState(true, _pulseBeaconPulseDelay);
    }

    public void PlayerDied()
    {
        if(_player.gameObject.activeInHierarchy)
        {
            _player.gameObject.SetActive(false);
        }

        //TODO - jumping straight back into the countdown for testing. should show UI here, use EGameState.PlayerDead
        SetupLevel();
    }

    public void GameWon()
    {
        SetGameState(EGameState.GameOver_Success);
    }

    public int IncrementPlayerShieldLevel(int addValue)
    {
        if (_playerShieldLevelRef >= _maxShieldLevel) { return _playerShieldLevelRef; }

        _playerShieldLevelRef += addValue;

        //_playerShieldLevelRef = Mathf.Min(_playerStartingShieldLevel, _playerShieldLevelRef);
        _playerShieldLevelRef = Mathf.Clamp(_playerShieldLevelRef, 0, _maxShieldLevel);

        _playerStartingShieldLevel = Mathf.Min(_playerStartingShieldLevel += addValue, _maxShieldLevel);

        UIManager.current.UpdateCurrentShieldBar(_playerShieldLevelRef);
        UIManager.current.UpdateMaxShieldBar(_playerStartingShieldLevel);

        //TODO - save data

        return _playerShieldLevelRef;
    }

    public void PlayerShieldHit(int shieldValue)
    {
        _playerShieldLevelRef = shieldValue;
        UIManager.current.UpdateCurrentShieldBar(_playerShieldLevelRef);
    }

    //using this instead of a Property to allow for saved data check 
    public int GetPlayerShieldLevel()
    {
        //TODO - read from save file, check if ref is higher than saved (something went wrong)

        return _playerShieldLevelRef;
    }

    private void SetGameState(EGameState gameState)
    {
        _gameState = gameState;
        OnGameStateChanged?.Invoke(_gameState);
    }
}
