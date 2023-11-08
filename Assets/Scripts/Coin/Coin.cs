using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    public static event Action<Coin> OnCoinCollected;

    public int coinValue { get; private set; }

    private void OnDisable()
    {
        // Invoke OnCoinCollected
        OnCoinCollected?.Invoke(this);
    }

}
