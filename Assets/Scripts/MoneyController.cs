using Eiko.YaSDK;
using UnityEngine;
using UnityEngine.Events;

public class MoneyController : Singletone<MoneyController>
{
    private int _money;

    public UnityEvent<int> OnChangeMoney;

    public void Start()
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
        _money = GetMoney();
    }

    public void AddMoney(int count)
    {
        _money += count;
        SaveController.Instance.SetMoney(_money);
        OnChangeMoney?.Invoke(_money);
    }

    public int GetMoney()
    {
        return SaveController.Instance.GetMoney();
    }

    [ContextMenu("add 200")]
    private void Add200()
    {
        AddMoney(200);
    }
}
