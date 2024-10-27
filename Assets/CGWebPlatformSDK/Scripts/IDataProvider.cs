public interface IDataProvider
{
    public void Init();

    public void SetInt(string key,int value);

    public void SetString(string key,string value);

    public int GetInt(string key);

    public int GetInt(string key, int defaultValue);

    public string GetString(string key);

    public string GetString(string key, string defaultValue);
}
