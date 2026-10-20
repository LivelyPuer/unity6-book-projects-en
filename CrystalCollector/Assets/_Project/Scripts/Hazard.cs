using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private GameManager game;
    [SerializeField] private string message = "Those spikes were sharp!";

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
            game.PlayerDied(message);
    }
}