using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    private int _shieldStrength = 0;
    private int _playerCoins = 0;

    //no player health - shieldStrength = health,  

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetupPlayer(int shieldLevel)
    {
        _shieldStrength = shieldLevel;
        //Debug.Log(string.Format("Shield set! {0}", _shieldStrength));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.activeInHierarchy) return;

        ICollectable collectable = collision.GetComponent<ICollectable>();

        if(collectable != null)
        {
            collectable.ItemCollected(this);
        }        
    }

    public void UpdateShieldLevel(int value)
    {
        //TODO - not sure if we should increment player shield immediately or on next respawn. 
        _shieldStrength = GameManager.current.IncrementPlayerShieldLevel(value);
        //Debug.Log(string.Format("Shield Updated! {0}", _shieldStrength));
    }

    public void UpdatePlayerCoins(int value)
    {
        _playerCoins += value;
        Debug.Log(string.Format("Coins collected! {0}", _playerCoins));
    }

    public void TakeDamage(int damageValue)
    {
        if (GameManager.current.GameState != EGameState.GameActive) return;

        _shieldStrength -= damageValue;

        if(_shieldStrength < 0 )
        {
            //Debug.Log(string.Format("Player is DEAD! shield strength: {0}", _shieldStrength));
            this.Invoke("PlayerDied", 0.5f);
        }

        Debug.Log(string.Format("OUCH! my shield is now {0}", _shieldStrength));
    }

    private void PlayerDied()
    {
        gameObject.SetActive(false);
        GameManager.current.PlayerDied();
    }
}
