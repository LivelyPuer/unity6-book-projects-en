using UnityEngine;

// Console window example (Chapter 2): a message, a warning and an error
public class ConsoleDemo : MonoBehaviour
{
    [SerializeField] private Transform target;

    void Start()
    {
        Debug.Log("Level Level01 loaded");
        Debug.Log("Crystals in the level: 12");
        Debug.LogWarning("Crystal (7): no collider, it can't be collected");
        FollowTarget();
    }

    private void FollowTarget()
    {
        // target is not assigned in the Inspector, so Unity reports an error
        transform.LookAt(target.position);
    }
}
