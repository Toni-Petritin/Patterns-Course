using System;
using UnityEngine;

public class Achievements : MonoBehaviour {

    private const int nAchievements = 3;
    private enum Achievement_ID {
        CoinCollector,
        Terminator,
        SomeOtherFancyAchievement
    }
    
    // Are the achievements unlocked
    private bool[] bUnlockedAchievements = new bool[nAchievements];
    
    private int nCoins = 0;
    private int nEnemy = 0;
    
    private void OnEnable() {
        // Add our method to listen to Coin-class event:
        Coin.OnCoinCollected += CoinWasCollected;
        Enemy.OnEnemyDestroyed += EnemyWasDestroyed;
    }

    private void OnDisable()
    {
        // Yo dag! It be good practice to unsub from events to avoid memory leaks.
        Coin.OnCoinCollected -= CoinWasCollected;
        Enemy.OnEnemyDestroyed -= EnemyWasDestroyed;
    }

    void CoinWasCollected()
    {
        nCoins++;
        if (nCoins == 5) {
            int index = (int)Achievement_ID.CoinCollector;
            if (!bUnlockedAchievements[index]) {
                bUnlockedAchievements[index] = true;
                Debug.Log("You've unlocked: COIN COLLECTOR!!!");
                Coin.OnCoinCollected -= CoinWasCollected;
            }
        }
        else if (nCoins < 5)
        {
            Debug.Log(nCoins + "/5 coins collected.");
        }
    }

    void EnemyWasDestroyed()
    {
        nEnemy++;
        if (nEnemy == 10)
        {
            int index = (int)Achievement_ID.Terminator;
            if (!bUnlockedAchievements[index]) {
                bUnlockedAchievements[index] = true;
                Debug.Log("You've unlocked: TERMINATOR!!!");
                Enemy.OnEnemyDestroyed -= EnemyWasDestroyed;
            }
        }else if (nEnemy < 10)
        {
            Debug.Log(nEnemy + "/10 enemies destroyed.");
        }
    }

}