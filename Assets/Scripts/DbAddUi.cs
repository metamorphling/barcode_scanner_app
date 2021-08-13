using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatabaseData;
using UnityEngine;
using UnityEngine.UI;

public class DbAddUi : MonoBehaviour
{
    [SerializeField] private Image _boxArt; 
    [SerializeField] private InputField _productName ;
    [SerializeField] private InputField _productCode ;
    [SerializeField] private InputField _serialCode ;
    [SerializeField] private InputField _developer ;
    [SerializeField] private InputField _publisher ;
    [SerializeField] private InputField _dateReleased ;
    [SerializeField] private InputField _genre ;
    [SerializeField] private InputField _region ;
    [SerializeField] private InputField _platform ;

    [SerializeField] private InputField _boughPrice;
    [SerializeField] private InputField _soldPrice;
    [SerializeField] private InputField _boughtDate;
    [SerializeField] private InputField _soldDate;
    [SerializeField] private InputField _description;

    public void Setup(GameData gameData)
    {
        _productName.text = string.Empty;
        _productCode.text = string.Empty;
        _serialCode.text = string.Empty;
        _developer.text = string.Empty;
        _publisher.text = string.Empty;
        _dateReleased.text = string.Empty;
        _genre.text = string.Empty;
        _region.text = string.Empty;
        _platform.text = string.Empty;
        _boxArt.sprite = null;

        if (null != gameData)
        {
            _productName.text = gameData.ProductName;
            _productCode.text = gameData.ProductCode;
            _serialCode.text = gameData.SerialCode;
            _developer.text = gameData.Developer;
            _publisher.text = gameData.Publisher;
            _dateReleased.text = gameData.DateReleased;
            _genre.text = gameData.Genre;
            _region.text = gameData.Region;
            _platform.text = gameData.Platform;

            if (false == string.IsNullOrEmpty(gameData.Platform) && 
                false == string.IsNullOrEmpty(gameData.ProductCode))
            {
                var sprite = Resources.Load<Sprite>($"{gameData.Platform.ToLower()}_images/{gameData.ProductCode}") as Sprite;
                _boxArt.sprite = sprite;
            }
        }

        _boughPrice.text = "";
        _soldPrice.text = "";
        _boughtDate.text = DateTime.Now.ToString("MM/dd/yyyy");
        _soldDate.text = "";
        _description.text = "";
    }

    public GameData GetGameDataEdits()
    {
        return new GameData(
            _productName.text,
            _productCode.text,
            _serialCode.text,
            _developer.text,
            _publisher.text,
            _dateReleased.text,
            _genre.text,
            _region.text,
            _platform.text
            );
    }

    public StockData GetStockDataEdits()
    {
        uint boughtPrice;
        uint soldPrice;
        DateTime boughtDate;
        DateTime soldDate;

        uint.TryParse(_boughPrice.text, out boughtPrice);
        uint.TryParse(_soldPrice.text, out soldPrice);
        DateTime.TryParse(_boughtDate.text, out boughtDate);
        DateTime.TryParse(_soldDate.text, out soldDate);

        return new StockData(
            _productCode.text,
            boughtPrice,
            soldPrice,
            boughtDate,
            soldDate,
            _description.text
        );
    }

    private void Awake()
    {
        ServiceLocator.Instance.SetService(this);
    }

    private void Start()
    {
        ServiceLocator.Instance.GetService<SceneManager>().SceneStateChanged += OnSceneStateChanged;
    }

    private void OnSceneStateChanged(object sender, SceneManager.SceneStateChangedEventArgs e)
    {
        if (SceneManager.SceneStates.DatabaseEdit == e.State)
        {
            var productCode = ServiceLocator.Instance.GetService<BarcodeManager>().Barcode;
            var gameData = string.IsNullOrEmpty(productCode) ?
                null :
                ServiceLocator.Instance.GetService<DatabaseManager>().GetGame(productCode);

            if (null == gameData)
            {
                gameData = new GameData();
                gameData.ProductCode = productCode;
            }
            Setup(gameData);
        }
    }
}
