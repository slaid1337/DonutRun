using Eiko.YaSDK;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : BasePanel
{
    [SerializeField] private ShopItemsObject _data;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Transform _itemParent;
    private List<ShopItem> _items;

    private void Start()
    {
        if (!YandexSDK.instance.IsFirstOpen)
        {
            Init();
        }
        else
        {
            YandexSDK.instance.onInitializeData += Init;
        }
    }

    public void Init()
    {
        _items = new List<ShopItem>();

        foreach (var item in _data.ShopItemData)
        {
            ShopItem newItem = Instantiate(_itemPrefab, _itemParent).GetComponent<ShopItem>();

            newItem.Init(item, this);

            _items.Add(newItem);
        }
    }

    public void Open()
    {
        FadeBG.Instance.Fade();
        UpdateItems();
        OpenPanel();
    }

    public void Close()
    {
        FadeBG.Instance.UnFade();

        ClosePanel();
    }

    public void UpdateItems()
    {
        foreach (var item in _items)
        {
            item.UpdateState();
        }
    }
}