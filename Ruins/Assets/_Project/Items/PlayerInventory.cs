using UnityEngine;

// The view and the "glue" to the Unity world
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int capacity = 20;
    public Inventory Inventory { get; private set; }

    void Awake() => Inventory = new Inventory(capacity);
}
