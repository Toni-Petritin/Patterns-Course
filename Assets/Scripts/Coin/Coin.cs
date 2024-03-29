using UnityEngine;
using System;

public class Coin : MonoBehaviour {
    // The event / action "list" that has all "observers" registered
    public static event Action OnCoinCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Collected();
            Destroy(this.gameObject);
        }
    }

    private void Collected() {
        OnCoinCollected?.Invoke();
    }
}