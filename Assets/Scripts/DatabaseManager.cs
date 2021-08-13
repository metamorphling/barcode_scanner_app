using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using DatabaseData;
using Newtonsoft.Json;
using SQLite4Unity3d;
using Unity.UIWidgets.ui;
using UnityEngine.Networking;
using Path = System.IO.Path;

public class DatabaseManager : MonoBehaviour
{
    public static string GameDatabaseName = "game_data.db";
    public static string StockDatabaseName = "stock_data.db";

    private static string _ps2_jsonDataPath = Path.Combine(Application.streamingAssetsPath, "ps2_db.json");

    public void InitializeDatabase()
    {
        InitializeDatabase(_ps2_jsonDataPath, "PS2");
    }

    public void Set<T>(string dbName, T insertData, bool canReplace) where T : new()
    {
        // can't touch persistentDataPath in fields
        var _databasePath = Path.Combine(Application.persistentDataPath, dbName);
        using (var conn = new SQLiteConnection(_databasePath))
        {
            conn.CreateTable<T>();
            var data = conn.Table<T>();
            if (canReplace)
            {
                conn.InsertOrReplace(insertData);
            }
            else
            {
                conn.Insert(insertData);
            }
        }
    }

    public IEnumerable<T> GetAll<T>(string dbName, ref List<T> list) where T : new()
    {
        // can't touch persistentDataPath in fields
        var _databasePath = Path.Combine(Application.persistentDataPath, dbName);
        using (var conn = new SQLiteConnection(_databasePath))
        {
            conn.CreateTable<T>();
            list.AddRange(conn.Table<T>());
            return list;
        }
    }

    public GameData GetGame(string id)
    {
        // can't touch persistentDataPath in fields
        var _databasePath = Path.Combine(Application.persistentDataPath, GameDatabaseName);
        using (var conn = new SQLiteConnection(_databasePath))
        {
            conn.CreateTable<GameData>();
            return conn
                .Table<GameData>()
                .FirstOrDefault(x => x.ProductCode == id);
        }
    }

    public IEnumerable<StockData> GetStock(string id)
    {
        // can't touch persistentDataPath in fields
        var _databasePath = Path.Combine(Application.persistentDataPath, StockDatabaseName);
        using (var conn = new SQLiteConnection(_databasePath))
        {
            conn.CreateTable<StockData>();
            return conn.Table<StockData>().Where(x => x.ItemId == id);
        }
    }

    public void ApplyData()
    {
        GameData gameData = ServiceLocator.Instance.GetService<DbAddUi>().GetGameDataEdits();
        StockData stockData = ServiceLocator.Instance.GetService<DbAddUi>().GetStockDataEdits();
        stockData.StockId = GetLastId(
            StockDatabaseName,
            nameof(StockData),
            nameof(StockData.StockId)) + 1;

        Set<GameData>(GameDatabaseName, gameData, true);
        Set<StockData>(StockDatabaseName, stockData, false);
    }

    private uint GetLastId(string dbName, string tableName, string key)
    {
        // can't touch persistentDataPath in fields
        var _databasePath = Path.Combine(Application.persistentDataPath, dbName);
        using (var conn = new SQLiteConnection(_databasePath))
        {
            return conn.GetLastId(tableName, key);
        }
    }

    private void InitializeDatabase(string dbPath, string platform)
    {
        string dataText;
        if (Application.platform == RuntimePlatform.Android)
        {
            UnityWebRequest www = UnityWebRequest.Get(dbPath);
            www.SendWebRequest();
            while (!www.isDone) ;
            dataText = www.downloadHandler.text;
        }
        else
        {
            dataText = File.ReadAllText(dbPath);
        }
        
        if (string.IsNullOrEmpty(dataText))
        {
            return;
        }

        var dataJson = JsonConvert.DeserializeObject<List<GameData>>(dataText);
        var dataJsonClean = dataJson
            .Where(x => false == string.IsNullOrEmpty(x.ProductCode))
            .GroupBy(x => x.ProductCode)
            .Select(y => y.First()).ToList();

        foreach (var item in dataJsonClean)
        {
            item.Platform = platform;
        }

        // can't touch persistentDataPath in fields
        var _databasePath = Path.Combine(Application.persistentDataPath, GameDatabaseName);
        using (var conn = new SQLiteConnection(_databasePath))
        {
            conn.CreateTable<GameData>();
            conn.InsertOrReplaceAll(dataJsonClean);
        }
        _databasePath = Path.Combine(Application.persistentDataPath, StockDatabaseName);
        using (var conn = new SQLiteConnection(_databasePath))
        {
            conn.CreateTable<StockData>();
        }
    }

    private void Awake()
    {
        ServiceLocator.Instance.SetService(this);
    }

    private static void FrameRateSpeedUp()
    {
    }

    private static void FrameRateCooldown()
    {
    }

    private void Start()
    {
        Window.onFrameRateSpeedUp = FrameRateSpeedUp;
        Window.onFrameRateCoolDown = FrameRateCooldown;
    }
}
