using TMPro;
using UnityEngine;

// Quick slot: how many of an item the hero is carrying
public class ItemSlot : MonoBehaviour
{
    [SerializeField] private PlayerInventory owner;
    [SerializeField] private ItemData item;
    [SerializeField] private TMP_Text count;
    [SerializeField] private CanvasGroup group;

    void Start()
    {
        owner.Inventory.Changed += Redraw;
        Redraw();
    }

    void OnDestroy() => owner.Inventory.Changed -= Redraw;

    private void Redraw()
    {
        owner.Inventory.Items.TryGetValue(item, out int n);
        count.text = n.ToString();
        group.alpha = n > 0 ? 1f : 0.35f;
    }
}
