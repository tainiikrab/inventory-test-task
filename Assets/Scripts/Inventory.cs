using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int slotCount = 20;
    public InventorySlot[] slots;

    private void Awake()
    {
        slots = new InventorySlot[slotCount];
        for (var i = 0; i < slotCount; i++) slots[i] = new InventorySlot();
    }

    public bool AddItem(ItemDefinition item, int amount = 1)
    {
        if (item == null || amount <= 0) return false;
        if (item.stackable)
        {
            var remaining = amount;
            for (var i = 0; i < slots.Length && remaining > 0; i++)
            {
                var s = slots[i];
                if (!s.IsEmpty && s.item == item && s.count < item.maxStack)
                {
                    var canAdd = Mathf.Min(item.maxStack - s.count,
                        remaining);
                    s.count += canAdd;
                    remaining -= canAdd;
                }
            }

            while (remaining > 0)
            {
                var emptyIndex = FindEmptySlot();
                if (emptyIndex == -1)
                {
                    Debug.Log("Inventory full, could not add all items");
                    return false;
                }

                var toPlace = Mathf.Min(item.maxStack, remaining);
                slots[emptyIndex].item = item;
                slots[emptyIndex].count = toPlace;
                remaining -= toPlace;
            }

            return true;
        }

        var free = FindEmptySlot();
        if (free == -1)
        {
            Debug.Log("Inventory full, cannot add item");
            return false;
        }

        slots[free].item = item;
        slots[free].count = 1;
        return true;
    }

    private int FindEmptySlot()
    {
        for (var i = 0; i < slots.Length; i++)
            if (slots[i].IsEmpty)
                return
                    i;
        return -1;
    }

    public bool RemoveFromSlot(int slotIndex, int amount = 1)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length) return false;
        var s = slots[slotIndex];
        if (s.IsEmpty) return false;
        if (s.item.stackable)
        {
            s.count -= amount;
            if (s.count <= 0) s.Clear();
            return true;
        }

        s.Clear();
        return true;
    }
}