using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject slotPrefab;
    public Transform grid;
    public ItemDefinition[] testItems;
    private readonly List<SlotUI> slotUIs = new();
    private ItemType? currentFilter;

    private void Start()
    {
        BuildUI();
        RefreshAll();
    }

    private void BuildUI()
    {
        for (var i = 0; i < inventory.slotCount; i++)
        {
            var go = Instantiate(slotPrefab, grid, false);
            var s = go.GetComponent<SlotUI>();
            slotUIs.Add(s);
        }
    }

    public void RefreshAll()
    {
        for (var i = 0; i < slotUIs.Count; i++)
        {
            slotUIs[i].SetData(inventory.slots[i], i, this);
            var visible = SlotPassesFilter(inventory.slots[i]);
            slotUIs[i].SetVisible(visible);
        }
    }

    private bool SlotPassesFilter(InventorySlot slot)
    {
        if (currentFilter == null) return true;
        if (slot.IsEmpty) return true;
        return slot.item.itemType == currentFilter.Value;
    }

    public void RequestRemoveFromSlot(int slotIndex)
    {
        inventory.RemoveFromSlot(slotIndex);
        RefreshAll();
    }

    public void AddTestItemByIndex(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= testItems.Length) return;
        var ok = inventory.AddItem(testItems[itemIndex]);
        if (!ok) Debug.Log("Не удалось добавить предмет");
        RefreshAll();
    }

    public void ShowAll()
    {
        currentFilter = null;
        RefreshAll();
    }

    public void ShowWeapons()
    {
        currentFilter = ItemType.Weapon;
        RefreshAll();
    }

    public void ShowConsumables()
    {
        currentFilter = ItemType.Consumable;
        RefreshAll();
    }
}