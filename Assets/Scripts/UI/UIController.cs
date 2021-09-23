using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    #region Fields
    [SerializeField] private TMP_Text _ammoText = null; //Debug

    [Header("Ammunition")]
    [SerializeField] private RawImage _magazine1Slider = null;
    [SerializeField] private RawImage _magazine2Slider = null;
    [SerializeField] private RawImage _magazine3Slider = null;

    [SerializeField] private TMP_Text _magazineNumber = null;
    [SerializeField] private TMP_Text _plusSign = null;
    //[SerializeField] private TMP_Text _shotgunAmmo = null;



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

    public RawImage Magazine1Slider
    {
        get
        {
            return _magazine1Slider;
        }
        set
        {
            _magazine1Slider = value;
        }
    }

    public RawImage Magazine2Slider
    {
        get
        {
            return _magazine2Slider;
        }
        set
        {
            _magazine2Slider = value;
        }
    }

    public RawImage Magazine3Slider
    {
        get
        {
            return _magazine3Slider;
        }
        set
        {
            _magazine3Slider = value;
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
