using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _maxPlayerHealth;
    private float _playerHealth;
    private int _shieldStrength = 0;
    private int _playerCoins = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupPlayer()
    {
        _shieldStrength = GameManager.current.GetPlayerShieldLevel();
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
        Debug.Log(string.Format("Shield Updated! {0}", _shieldStrength));
    }

    public void UpdatePlayerCoins(int value)
    {
        _playerCoins += value;
        Debug.Log(string.Format("Coins collected! {0}", _playerCoins));
    }
}
