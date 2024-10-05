using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemsObject")]
public class ShopItemsObject : ScriptableObject
{
    public ShopItemData[] ShopItemData;
}

[Serializable]
public class ShopItemData
{
    public Sprite Icon;
    public int Cost;
    public string Name;
}