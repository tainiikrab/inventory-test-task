using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Button _button;

    private int _index;
    private InventoryUI _parentUI;
    private InventorySlot _slot;

    private void Start()
    {
        if (_button != null) _button.onClick.AddListener(OnClick);
    }

    public void SetData(InventorySlot s, int idx, InventoryUI ui)
    {
        _slot = s;
        _index = idx;
        _parentUI = ui;

        if (s.IsEmpty)
        {
            _icon.enabled = false;
            _countText.text = "";
        }
        else
        {
            _icon.enabled = true;
            _icon.sprite = s.Item.Icon;
            _countText.text = s.Item.Stackable ? s.Count.ToString() : "";
        }
    }

    public void Clear()
    {
        _icon.enabled = false;
        _countText.text = "";
        _slot = null;
    }

    private void OnClick()
    {
        if (_slot == null || _slot.IsEmpty) return;
        _parentUI.RequestRemoveFromSlot(_index);
    }
}