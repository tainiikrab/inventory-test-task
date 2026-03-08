using System;

[Serializable]
public class InventorySlot
{
    public ItemDefinition item;
    public int count;

    public InventorySlot()
    {
        item = null;
        count = 0;
    }

    public bool IsEmpty => item == null;

    public void Clear()
    {
        item = null;
        count = 0;
    }
}