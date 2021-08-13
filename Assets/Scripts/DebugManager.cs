using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public void TestBarcode()
    {
        ServiceLocator.Instance.GetService<BarcodeManager>().Barcode = "4976219855693";
        ServiceLocator.Instance.GetService<SceneManager>().GoTo(SceneManager.SceneStates.DatabaseEdit);
    }
}
