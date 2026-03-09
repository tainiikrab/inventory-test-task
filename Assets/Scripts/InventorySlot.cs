using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    [field: SerializeField] public ItemSO Item { get; set; }

    [field: SerializeField] public int Count { get; set; }

    public InventorySlot()
    {
        Item = null;
        Count = 0;
    }

    public bool IsEmpty => Item == null;

    public void Clear()
    {
        Item = null;
        Count = 0;
    }
}