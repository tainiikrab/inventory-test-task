using UnityEngine;

public class Inventory : MonoBehaviour
{
    [field: SerializeField] public int SlotCount { get; private set; } = 20;
    [field: SerializeField] public InventorySlot[] Slots { get; private set; }

    private void Awake()
    {
        Slots = new InventorySlot[SlotCount];
        for (var i = 0; i < SlotCount; i++) Slots[i] = new InventorySlot();
    }

    public bool TryAddItem(ItemSO item, int amount = 1)
    {
        if (item == null || amount <= 0) return false;
        if (item.Stackable)
        {
            var remaining = amount;
            for (var i = 0; i < Slots.Length && remaining > 0; i++)
            {
                var s = Slots[i];
                if (!s.IsEmpty && s.Item == item && s.Count < item.MaxStack)
                {
                    var canAdd = Mathf.Min(item.MaxStack - s.Count, remaining);
                    s.Count += canAdd;
                    remaining -= canAdd;
                }
            }

            while (remaining > 0)
            {
                var emptyIndex = FindEmptySlot();
                if (emptyIndex == -1)
                {
                    Debug.Log("Inventory full, can't add all items");
                    return false;
                }

                var toPlace = Mathf.Min(item.MaxStack, remaining);
                Slots[emptyIndex].Item = item;
                Slots[emptyIndex].Count = toPlace;
                remaining -= toPlace;
            }

            return true;
        }

        var free = FindEmptySlot();
        if (free == -1)
        {
            Debug.Log("Inventory full, can't add item");
            return false;
        }

        Slots[free].Item = item;
        Slots[free].Count = 1;
        return true;
    }

    private int FindEmptySlot()
    {
        for (var i = 0; i < Slots.Length; i++)
            if (Slots[i].IsEmpty)
                return i;
        return -1;
    }

    public bool RemoveFromSlot(int slotIndex, int amount = 1)
    {
        if (slotIndex < 0 || slotIndex >= Slots.Length) return false;

        var s = Slots[slotIndex];
        if (s.IsEmpty) return false;

        if (s.Item.Stackable)
        {
            s.Count -= amount;
            if (s.Count <= 0) s.Clear();
        }
        else
        {
            s.Clear();
        }

        CompactSlots();
        return true;
    }

    private void CompactSlots()
    {
        var target = 0;

        for (var i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].IsEmpty) continue;

            if (i != target)
            {
                Slots[target].Item = Slots[i].Item;
                Slots[target].Count = Slots[i].Count;
                Slots[i].Clear();
            }

            target++;
        }
    }
}