using UnityEngine;

public class PersistentDataManager<TData>
{
    /// <summary>
    /// 儲存資料
    /// </summary>
    /// <param name="data">資料源</param>
    /// <param name="dataString">資料源字串</param>
    public static void SaveData(TData data , string dataString)
    {
        var serializedData = JsonUtility.ToJson(data);

        PlayerPrefs.SetString(dataString , serializedData);

        Debug.Log("Data Saved!");
    }

    /// <summary>
    /// 取得資料
    /// </summary>
    /// <param name="dataString">資料源字串</param>
    /// <remarks>沒過濾該字串有無資料。該函式可用在過濾時需要特別處理的情況</remarks>
    public static TData GetDataNoChecked(string dataString)
    {
        var unSerializedData = JsonUtility.FromJson<TData>(PlayerPrefs.GetString(dataString));

        Debug.Log("Data Loading…");

        return unSerializedData;
    }

    /// <summary>
    /// 取得資料
    /// </summary>
    /// <param name="dataString">資料源字串</param>
    /// <remarks>有過濾該字串有無資料，沒資料則回傳 default。該函式可用在過濾時不需特別處理的情況</remarks>
    public static TData GetDataChecked(string dataString)
    {
        if(PlayerPrefs.HasKey(dataString))
        {
            var unSerializedData = JsonUtility.FromJson<TData>(PlayerPrefs.GetString(dataString));

            Debug.Log("Data Loading");

            return unSerializedData;
        }
        
        return default;
    }

    public static void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
    }
}
