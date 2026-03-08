using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI countText;
    public Button button;
    private int index;
    private InventoryUI parentUI;
    private InventorySlot slot;

    private void Start()
    {
        if (button != null) button.onClick.AddListener(OnClick);
    }

    public void SetData(InventorySlot s, int idx, InventoryUI ui)
    {
        slot = s;
        index = idx;
        parentUI = ui;
        if (s.IsEmpty)
        {
            icon.enabled = false;
            countText.text = "";
        }
        else
        {
            icon.enabled = true;
            icon.sprite = s.item.icon;
            countText.text = s.item.stackable ? s.count.ToString() : "";
        }
    }

    private void OnClick()
    {
        if (slot == null || slot.IsEmpty) return;
        parentUI.RequestRemoveFromSlot(index);
    }

    public void SetVisible(bool v)
    {
        gameObject.SetActive(v);
    }
}