using UnityEngine;

public enum ItemType
{
    Weapon,
    Consumable,
    Material,
    Quest
}

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemDefinition : ScriptableObject
{
    public Sprite icon;
    public string itemName;
    public ItemType itemType;
    public int maxStack = 99;
    public bool stackable;
}