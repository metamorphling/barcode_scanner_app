using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraTexture : MonoBehaviour
{
    public Image DebugScanOutputImage;

    private WebCamTexture _webCamTexture;
    [SerializeField]
    private RawImage _cameraImage;
    private Texture2D _debugTexture;

    void Start()
    {
        _webCamTexture = new WebCamTexture();
        _cameraImage.material.mainTexture = _webCamTexture;
        _webCamTexture.Play();

        _debugTexture = new Texture2D(_webCamTexture.width, _webCamTexture.height);
        _debugTexture.SetPixels(_webCamTexture.GetPixels());
        _debugTexture.Apply();
    }

    void Reset()
    {
        _cameraImage = GetComponent<RawImage>();
    }

    private float _threshold = 1f;
    private float _time = 0;

    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= _threshold)
        {
            _debugTexture.SetPixels(_webCamTexture.GetPixels());
            _debugTexture.Apply();
            DebugScanOutputImage.sprite = Sprite.Create(_debugTexture,
                new Rect(0.0f, 0.0f, _debugTexture.width, _debugTexture.height),
                new Vector2(0.5f, 0.5f),
                100.0f);
            _time = 0;
        }
    }
}
