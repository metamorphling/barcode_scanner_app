using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{
    private void Awake() // Initialize all the managers here
    {
        new ServiceLocator();
    }

    private void Start()
    {
        ServiceLocator.Instance.GetService<SceneManager>()?.GoTo(SceneManager.SceneStates.Menu);
    }
}
