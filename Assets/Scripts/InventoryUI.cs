using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _grid;
    [SerializeField] private ItemSO[] _testItems;
    [SerializeField] private Button _showAllButton;
    [SerializeField] private Button _showConsumablesButton;
    [SerializeField] private Button _showWeaponsButton;

    private readonly List<SlotUI> _slotUIs = new();
    private ItemType? _currentFilter;

    private void Start()
    {
        foreach (Transform child in transform) Destroy(child.gameObject);
        BuildUI();
        RefreshAll();

        if (_showAllButton == null || _showWeaponsButton == null || _showConsumablesButton == null)
            return;
        _showAllButton.onClick.AddListener(ShowAll);
        _showWeaponsButton.onClick.AddListener(ShowWeapons);
        _showConsumablesButton.onClick.AddListener(ShowConsumables);
    }

    private void BuildUI()
    {
        for (var i = 0; i < _inventory.SlotCount; i++)
        {
            var go = Instantiate(_slotPrefab, _grid, false);
            var s = go.GetComponent<SlotUI>();
            _slotUIs.Add(s);
        }

        UpdateFilterButtons();
    }

    public void RefreshAll()
    {
        var filtered = new List<int>();

        for (var i = 0; i < _inventory.Slots.Length; i++)
            if (SlotPassesFilter(_inventory.Slots[i]))
                filtered.Add(i);

        for (var i = 0; i < _slotUIs.Count; i++)
            if (i < filtered.Count)
            {
                var slotIndex = filtered[i];
                _slotUIs[i].SetData(_inventory.Slots[slotIndex], slotIndex, this);
            }
            else
            {
                _slotUIs[i].Clear();
            }
    }

    private void UpdateFilterButtons()
    {
        if (_showAllButton == null || _showWeaponsButton == null || _showConsumablesButton == null)
            return;
        _showAllButton.interactable = _currentFilter != null;
        _showWeaponsButton.interactable = _currentFilter != ItemType.Weapon;
        _showConsumablesButton.interactable = _currentFilter != ItemType.Consumable;
    }

    private bool SlotPassesFilter(InventorySlot slot)
    {
        if (_currentFilter == null) return true;
        if (slot.IsEmpty) return true;
        return slot.Item.ItemType == _currentFilter.Value;
    }

    public void RequestRemoveFromSlot(int slotIndex)
    {
        _inventory.RemoveFromSlot(slotIndex);
        RefreshAll();
    }

    public void AddTestItemByIndex(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= _testItems.Length) return;
        _inventory.TryAddItem(_testItems[itemIndex]);
        RefreshAll();
    }

    public void ShowAll()
    {
        _currentFilter = null;
        UpdateFilterButtons();
        RefreshAll();
    }

    public void ShowWeapons()
    {
        _currentFilter = ItemType.Weapon;
        UpdateFilterButtons();
        RefreshAll();
    }

    public void ShowConsumables()
    {
        _currentFilter = ItemType.Consumable;
        UpdateFilterButtons();
        RefreshAll();
    }
}