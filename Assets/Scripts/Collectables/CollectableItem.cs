using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECollectableType
{
    Shield,
    Coin,
    MAX
}

public class Collectables : MonoBehaviour, ICollectable
{
    [SerializeField] private ECollectableType _collectableType;
    [SerializeField] private int _collectableValue = 1;

    public void ItemCollected(Player collectingPlayer)
    {
        if(collectingPlayer == null) { return; }

        switch(_collectableType)
        {
            case ECollectableType.Shield:
                collectingPlayer.UpdateShieldLevel(_collectableValue);
                break;
            case ECollectableType.Coin:
                collectingPlayer.UpdatePlayerCoins(_collectableValue);
                break;
            default:
                break;

        }

        Destroy(gameObject);
    }
}
