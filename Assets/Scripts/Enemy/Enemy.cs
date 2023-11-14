using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // The event / action "list" that has all "observers" registered
    public static event Action OnEnemyDestroyed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            DestroyedByPlayer();
            Destroy(this.gameObject);
        }
    }

    private void DestroyedByPlayer() {
        OnEnemyDestroyed?.Invoke();
    }
}
