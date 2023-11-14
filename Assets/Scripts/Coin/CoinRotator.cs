using System;
using UnityEngine;

public class CoinRotator : MonoBehaviour
{
    public float rotationSpeed = 30;

    private Vector3 startPos;

    private void Start()
    {
        startPos = this.transform.position;
    }
    
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);

        transform.position = startPos + new Vector3(0,Mathf.Sin(Time.time) * .3f,0);
    }
}
