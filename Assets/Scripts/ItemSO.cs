using UnityEngine;

public enum ItemType
{
    Weapon,
    Consumable,
    Material,
    Quest
}

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public Sprite Icon;
    public string ItemName;
    public ItemType ItemType;
    public int MaxStack = 99;
    public bool Stackable;
}