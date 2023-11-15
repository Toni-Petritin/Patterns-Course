using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Monster monster;

    public float x = 1f;
    public float y = 1f;
    public float z = 1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(x,y,z));
    }

    void Update()
    {
        
    }
}
