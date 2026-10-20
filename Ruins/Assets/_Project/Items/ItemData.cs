using UnityEngine;

public enum ItemKind { Key, Consumable, Quest }

[CreateAssetMenu(menuName = "Ruins/Item")]
public class ItemData : ScriptableObject
{
    public string id;              // stable key for saves
    public string displayName;
    [TextArea] public string description;
    public Sprite icon;
    public ItemKind kind;
    public int healAmount;         // for potions
    public int price;              // shop price
}
