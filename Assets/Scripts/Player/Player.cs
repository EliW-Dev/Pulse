using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    private int _shieldStrength = 0;
    private int _playerCoins = 0;

    public void SetupPlayer(int shieldLevel)
    {
        _shieldStrength = shieldLevel;
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
    }

    //Coins not implemented yet - collected coins do nothing.
    public void UpdatePlayerCoins(int value)
    {
        _playerCoins += value;
        Debug.Log(string.Format("Coins collected! {0}", _playerCoins));
    }

    public void TakeDamage(int damageValue)
    {
        if (GameManager.current.GameState != EGameState.GameActive) return;

        _shieldStrength -= damageValue;

        GameManager.current.PlayerShieldHit(_shieldStrength);

        if(_shieldStrength < 0 )
        {
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
