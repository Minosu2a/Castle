using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    #region Fields
    [SerializeField] private TMP_Text _ammoText = null; //Debug

    [Header("Rifle Ammunition")]
    [SerializeField] private RawImage _rifleMagazine1Slider = null;
    [SerializeField] private RawImage _rifleMagazine2Slider = null;
    [SerializeField] private RawImage _rifleMagazine3Slider = null;
    [SerializeField] private GameObject _rifleObj2Magazine = null;
    [SerializeField] private GameObject _rifleObj3Magazine = null;

    [SerializeField] private float _bottomValueOfMagazine = 25f;

    [Header("Shotgun Ammunition")]
    [SerializeField] private TMP_Text _shotgunAmmo = null;
    [SerializeField] private GameObject _shotgunMiddleBar = null;


    [Header("Global Ammunition")]
    [SerializeField] private TMP_Text _magazineNumber = null;
    [SerializeField] private TMP_Text _plusSign = null;
    //[SerializeField] private TMP_Text _shotgunAmmo = null;



    #endregion Fields

    #region Property

    public float BottomValueOfMagazine => _bottomValueOfMagazine;

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

    public GameObject ShotgunMiddleBar
    {
        get
        {
            return _shotgunMiddleBar;
        }
        set
        {
            _shotgunMiddleBar = value;
        }
    }

    public TMP_Text ShotgunAmmo
    {
        get
        {
            return _shotgunAmmo;
        }
        set
        {
            _shotgunAmmo = value;
        }
    }

    public GameObject RifleObj2Magazine
    {
        get
        {
            return _rifleObj2Magazine;
        }
        set
        {
            _rifleObj2Magazine = value;
        }
    }

    public GameObject RifleObj3Magazine
    {
        get
        {
            return _rifleObj3Magazine;
        }
        set
        {
            _rifleObj3Magazine = value;
        }
    }


    public RawImage RifleMagazine1Slider
    {
        get
        {
            return _rifleMagazine1Slider;
        }
        set
        {
            _rifleMagazine1Slider = value;
        }
    }

    public RawImage RifleMagazine2Slider
    {
        get
        {
            return _rifleMagazine2Slider;
        }
        set
        {
            _rifleMagazine2Slider = value;
        }
    }

    public RawImage RifleMagazine3Slider
    {
        get
        {
            return _rifleMagazine3Slider;
        }
        set
        {
            _rifleMagazine3Slider = value;
        }
    }

    public TMP_Text PlusSign
    {
        get
        {
            return _plusSign;
        }
        set
        {
            _plusSign = value;
        }
    }

    public TMP_Text MagazineNumber
    {
        get
        {
            return _magazineNumber;
        }
        set
        {
            _magazineNumber = value;
        }
    }

    #endregion Property


    #region Methods
    private void Awake()
    {
        UIManager.Instance.UIController = this;    
    }

    private void Start()
    {
        _bottomValueOfMagazine = _rifleMagazine1Slider.rectTransform.offsetMin.y;
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
