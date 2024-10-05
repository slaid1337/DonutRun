using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private Button _buyState;
    [SerializeField] private Button _setState;
    [SerializeField] private GameObject _activeState;
    private ShopItemData _data;
    private ShopPanel _panel;

    public void Init(ShopItemData data, ShopPanel panel)
    {
        _data = data;
        _panel = panel;

        _icon.sprite = _data.Icon;
        _costText.text = _data.Cost.ToString();
        _nameText.text = _data.Name.ToString();

        if (SaveController.Instance.GetActiveDonut() == _data.Name)
        {
            UpdateState(_activeState.gameObject);
        }
        else if (SaveController.Instance.IsItemPurchased(_data.Name))
        {
            UpdateState(_setState.gameObject);
        }
        else
        {
            UpdateState(_buyState.gameObject);
        }
    }

    public void UpdateState(GameObject state)
    {
        _buyState.gameObject.SetActive(false);
        _setState.gameObject.SetActive(false);
        _activeState.gameObject.SetActive(false);

        state.SetActive(true);
    }

    public void UpdateState()
    {
        if (SaveController.Instance.GetActiveDonut() == _data.Name)
        {
            UpdateState(_activeState.gameObject);
        }
        else if (SaveController.Instance.IsItemPurchased(_data.Name) || _data.Name == "Gentleness")
        {
            UpdateState(_setState.gameObject);
        }
        else
        {
            UpdateState(_buyState.gameObject);
        }
    }

    public void Buy()
    {
        if (MoneyController.Instance.GetMoney() - _data.Cost >= 0)
        {
            MoneyController.Instance.AddMoney(-_data.Cost);

            SaveController.Instance.PurchaseItem(_data.Name);

            UpdateState(_setState.gameObject);
        }

        _panel.UpdateItems();
    }

    public void Set()
    {
        SaveController.Instance.SetActiveDonut(_data.Name);

        _panel.UpdateItems();
    }
}