using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    #region Fields
    [SerializeField] private TMP_Text _ammoText = null;


    #endregion Fields
    #region Property
   

    public TMP_Text AmmoText
    {
        get
        {
            return _ammoText;
        }
        set
        {
            _ammoText = value;
        }
    }


    #endregion Property


    #region Methods
    private void Awake()
    {
        UIManager.Instance.UIController = this;    
    }

    public void TooglePause()
    {
        GameManager.Instance.TogglePause();
    }

    private void Restart()
    {
        GameStateManager.Instance.LaunchTransition(EGameState.MAINMENU);
    }

    private void Update()
    {

    }
    #endregion Methods


}
