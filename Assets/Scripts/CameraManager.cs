using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public Texture2D PhotoTexture { get; set; }

    [SerializeField] private float _timeThreshold = 1f;
    private float _timeAccumulated = 0;

    [SerializeField] private Canvas _cameraCanvas;

    [SerializeField] private RawImage _cameraImage;
    private WebCamTexture _webCamTexture;

    private BarcodeManager _barcodeManager;

    private float _cameraVerticalScale = 0;

    void Awake()
    {
        Assert.IsNotNull(_cameraCanvas);
        Assert.IsNotNull(_cameraImage);

        _webCamTexture = new WebCamTexture();
        _webCamTexture.requestedWidth = 1280;
        _webCamTexture.requestedHeight = 720;
        _cameraImage.rectTransform.sizeDelta = new Vector2(_webCamTexture.width, _webCamTexture.height);
        _cameraImage.material.mainTexture = _webCamTexture;

        ServiceLocator.Instance.SetService(this);
    }

    public void StartCamera()
    {
        if (_webCamTexture.isPlaying)
        {
            return;
        }
        _webCamTexture.Play();
    }

    public void StopCamera()
    {
        if (false == _webCamTexture.isPlaying)
        {
            return;
        }
        _webCamTexture.Stop();
    }

    private void Start()
    {
        _barcodeManager = ServiceLocator.Instance.GetService<BarcodeManager>();
        ServiceLocator.Instance.GetService<SceneManager>().SceneStateChanged += OnSceneStateChanged;
    }

    private void OnSceneStateChanged(object sender, SceneManager.SceneStateChangedEventArgs e)
    {
        if (SceneManager.SceneStates.Camera == e.State)
        {
            StartCamera();
        }
        else
        {
            StopCamera();
        }
    }

    private void Update()
    {
        if (false == _webCamTexture.isPlaying)
        {
            return;
        }
        if (null == PhotoTexture) // have to init it here cause webcam startup takes some time
        {
            // scale camera output to match screen
            _cameraVerticalScale = _cameraCanvas.GetComponent<RectTransform>().sizeDelta.y / _webCamTexture.height;
            PhotoTexture = new Texture2D(_webCamTexture.width, _webCamTexture.height);
            _cameraImage.transform.localScale = _cameraImage.transform.localScale * _cameraVerticalScale;
        }
        // rotation depends on camera since we are getting data raw and it might be rotated
        _cameraImage.transform.rotation = Quaternion.AngleAxis(90, Vector3.back);
        _cameraImage.rectTransform.sizeDelta = new Vector2(_webCamTexture.width, _webCamTexture.height);
        // update camera texture
        PhotoTexture.SetPixels(_webCamTexture.GetPixels());
        PhotoTexture.Apply();
        // send barcode detect request
        _timeAccumulated += Time.deltaTime;
        if (_timeAccumulated >= _timeThreshold)
        {
            _barcodeManager.ScanCode();
            _timeAccumulated = 0;
        }
    }
}
