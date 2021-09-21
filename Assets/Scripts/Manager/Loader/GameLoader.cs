using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{

    #region Fields

    [SerializeField] private InputManager _inputManager = null;

    [SerializeField] private GameManager _gameManager = null;
    [SerializeField] private UIManager _uiManager = null;
    [SerializeField] private AudioManager _audioManager = null;
    [SerializeField] private GameStateManager _gameStateManager = null;

    [SerializeField] private EGameState _sceneToLoadAtLaunch = EGameState.MAINMENU;
    #endregion Fields

    #region Properties
    #endregion Properties

    #region Methods
    private void Start()
    {
        _gameManager.Initialize();
        _uiManager.Initialize();
        _audioManager.Initialize();
        _gameStateManager.Initialize();
        _inputManager.Initialize();


         GameStateManager.Instance.LaunchTransition(_sceneToLoadAtLaunch);
    }

    #endregion Methods



}
