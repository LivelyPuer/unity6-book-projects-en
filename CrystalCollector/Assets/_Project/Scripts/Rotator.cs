using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 speed = new Vector3(0f, 90f, 0f); // degrees per second

    void Update()
    {
        transform.Rotate(speed * Time.deltaTime);
    }
}