#pragma warning disable CS0414 // fields exist only to demo the Inspector
using UnityEngine;

// Weapon stub for the attributes example (Chapter 6)
public class Weapon : MonoBehaviour
{
    [SerializeField] private int damage = 10;
}
