using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class BarcodeManager : MonoBehaviour
{
    public string Barcode { get; set; }

    private AndroidJavaObject _ajo;
    [SerializeField] private Text _barcodeResult;
    [SerializeField] private Button _barcodeReadButton;
    private CameraManager _cameraManager;

    #region NativeCode
    private const string _nativeBarcodeModuleName = "com.example.barcoderecognizer.BarcodeProcessor";
    private const string _detectBarcode = "DetectInImageFromBytes";
    private const string _isBarcodeDetected = "IsResultAvailable";
    private const string _readDetectedBarcode = "ReadResults";
    #endregion


    void Awake()
    {
        Assert.IsNotNull(_barcodeResult);
        Assert.IsNotNull(_barcodeReadButton);

        _ajo = new AndroidJavaObject(_nativeBarcodeModuleName);

        ServiceLocator.Instance.SetService(this);
    }

    public void ScanCode()
    {
        var texture = _cameraManager?.PhotoTexture;
        if (null == texture)
        {
            Debug.Log("PhotoTexture queue is empty");
            return;
        }
        byte[] imageData = texture.EncodeToJPG().ToArray();
        _ajo.Call(_detectBarcode, imageData, texture.width, texture.height);
    }


    private void Start()
    {
        _cameraManager = ServiceLocator.Instance.GetService<CameraManager>();
        ServiceLocator.Instance.GetService<SceneManager>().SceneStateChanged += OnSceneStateChanged;
    }

    private void OnSceneStateChanged(object sender, SceneManager.SceneStateChangedEventArgs e)
    {
        if (SceneManager.SceneStates.Camera == e.State)
        {
        }
    }

    private void Update()
    {
        if (_ajo == null)
        {
            return;
        }
        bool isResultReady = _ajo.Call<bool>(_isBarcodeDetected);
        if (isResultReady)
        {
            char[] barcode = _ajo.Call<char[]>(_readDetectedBarcode);
            string barcodeStr = new string(barcode);
            if (barcodeStr == Barcode)
            {
                return;
            }
            Barcode = barcodeStr;
            _barcodeResult.text = new string(barcode);
            ServiceLocator.Instance.GetService<SceneManager>().GoTo(SceneManager.SceneStates.DatabaseEdit);
        }
    }
}
