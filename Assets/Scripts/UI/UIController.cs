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
    [SerializeField] private GameObject _rifleObj1Magazine = null;
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

    private float _timeStamp = 0f;
    private bool _needVisual = true;
    [Header("Other")]
    [SerializeField] private float _timerToHideVisual = 1.5f;

    [Header("Post")]
    [SerializeField] private GameObject _zombieWave1 = null;
    [SerializeField] private GameObject _zombieWave2 = null;
    [SerializeField] private GameObject _zombieWave3 = null;
    [SerializeField] private GameObject _zombieWave4 = null;

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

    public GameObject RifleObj1Magazine
    {
        get
        {
            _needVisual = true;
            return _rifleObj1Magazine;
        }
        set
        {
            _rifleObj1Magazine = value;
        }
    }

    public GameObject RifleObj2Magazine
    {
        get
        {
            _needVisual = true;
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
            _needVisual = true;
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
            _needVisual = true;
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
            _needVisual = true;
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
            _needVisual = true;
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
        if(_needVisual == true)
        {
            _timeStamp += Time.deltaTime;

            if(_timeStamp >= _timerToHideVisual)
            {
                _needVisual = false;
                HideMagazineInfo();
                _timeStamp = 0f;
            }
        }


        //FOR ADVERTISEMENT PURPOSE 


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _zombieWave1.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _zombieWave2.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _zombieWave3.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _zombieWave4.SetActive(true);
        }

    }

    private void HideMagazineInfo()
    {
            _rifleObj1Magazine.SetActive(false);
            _rifleObj2Magazine.SetActive(false);
            _rifleObj3Magazine.SetActive(false);
            _rifleMagazine3Slider.gameObject.SetActive(false);
            _plusSign.gameObject.SetActive(false);
            _magazineNumber.gameObject.SetActive(false);
            _shotgunMiddleBar.SetActive(false);
            _shotgunAmmo.gameObject.SetActive(false);

    }
    #endregion Methods


}
