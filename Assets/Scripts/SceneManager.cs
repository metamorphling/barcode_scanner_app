using System;
using UnityEngine;
using UnityEngine.Assertions;

public class SceneManager : MonoBehaviour
{
    public event EventHandler<SceneStateChangedEventArgs> SceneStateChanged;
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private Canvas _cameraCanvas;
    [SerializeField] private Canvas _dbAddCanvas;
    private SceneStates _state = SceneStates.Menu;

    public enum SceneStates
    {
        Menu,
        Camera,
        DatabaseEdit,
    }

    public class SceneStateChangedEventArgs : System.EventArgs
    {
        public SceneStates State;
        public SceneStateChangedEventArgs(SceneStates state)
        {
            State = state;
        }
    }

    public void GoTo(SceneStates state)
    {
        _state = state;

        if (null != SceneStateChanged)
        {
            SceneStateChanged(this, new SceneStateChangedEventArgs(_state));
        }

        switch (_state)
        {
            case SceneStates.Menu:
                _cameraCanvas.enabled = false;
                _mainCanvas.enabled = true;
                _dbAddCanvas.enabled = false;
                break;
            case SceneStates.Camera:
                _cameraCanvas.enabled = true;
                _mainCanvas.enabled = false;
                _dbAddCanvas.enabled = false;
                break;
            case SceneStates.DatabaseEdit:
                _cameraCanvas.enabled = false;
                _mainCanvas.enabled = false;
                _dbAddCanvas.enabled = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void GoToCamera()
    {
        GoTo(SceneStates.Camera);
    }

    public void GoToMain()
    {
        GoTo(SceneStates.Menu);
    }

    public void GoToAddToDb()
    {
        GoTo(SceneStates.DatabaseEdit);
    }

    private void Awake()
    {
        Assert.IsNotNull(_cameraCanvas);
        Assert.IsNotNull(_mainCanvas);
        Assert.IsNotNull(_dbAddCanvas);

        ServiceLocator.Instance.SetService(this);
    }
}
