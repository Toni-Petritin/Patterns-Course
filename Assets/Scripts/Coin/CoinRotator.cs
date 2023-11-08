using UnityEngine;

public class CoinRotator : MonoBehaviour
{
    public float rotationSpeed = 30;

    void Update()
    {
        transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
    }
}
