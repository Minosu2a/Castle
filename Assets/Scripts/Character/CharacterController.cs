using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{

    #region Fields
    [Header("Movement")]
    [SerializeField] private Rigidbody _rb = null;
    [SerializeField] private float _walkSpeed = 250f;
    [SerializeField] private float _sprintSpeed = 500f;
    private bool _isMoving = false;

    [Header("Look")]
    private Vector3 _posMouse = Vector3.zero;
    private Vector3 _mouseRotdir = Vector3.zero;

    [Header("Gun")]
    [SerializeField] private Weapon _currentWeapon = null;
    [SerializeField] private Weapon _secondaryWeapon = null;




    [Header("Aim")]
    [SerializeField] private LineRenderer _aimLine = null;
    private bool _isAiming = false;


    [Header("Ammunition")]
    [SerializeField] private List<int> _pistolMagazine = null; //Maximum number should be 9
    [SerializeField] private List<int>  _rifleMagazine = null; //Maximum number should be 30
    [SerializeField] private int _shotgunAmmo = 12;


    #endregion Fields


    #region Properties

    public Rigidbody Rb => _rb;

    public Vector3 PosMouse => _posMouse;

    public LineRenderer AimLine
    {
        get
        {
            return _aimLine;
        }
        set
        {
            _aimLine = value;
        }
    }

    public bool IsAiming => _isAiming;

    #region Ammunition
    public List<int> RifleMagazine
    {
        get
        {
            return _rifleMagazine;
        }
        set
        {
            _rifleMagazine = value;
        }
    }

    public List<int> PistolMagazine
    {
        get
        {
            return _pistolMagazine;
        }
        set
        {
            _pistolMagazine = value;
        }
    }

    public int ShotgunAmmo
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
    #endregion Ammunition
    public Weapon CurrentWeapon
    {
        get
        {
            return _currentWeapon;
        }
        set
        {
            _currentWeapon = value;
        }
    }

    
    #endregion Properties


    #region Methods

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        CharacterManager.Instance.CharacterController = this;

        _pistolMagazine = new List<int>();
        _rifleMagazine = new List<int>();    
        

    }

    private void Update()
    {
        if(InputManager.Instance.MouseActivated == true)
        {
            float distance = Vector3.Distance(transform.position, Camera.main.transform.position) - Camera.main.nearClipPlane;

            Vector3 vector = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            _posMouse = Camera.main.ScreenToWorldPoint(vector);

            _mouseRotdir = _posMouse - transform.position;
            _mouseRotdir.y = 0f;
            transform.forward = _mouseRotdir;
        } 
        else if (InputManager.Instance.RotDir != Vector3.zero)
        {
            Look();
        }

        Walk();

        if(_rb.velocity != Vector3.zero)
        {
            _isMoving = true;
        }
        else
        {
            _isMoving = false; 
        }
    }


    private void Look()
    {
        transform.forward = InputManager.Instance.RotDir;
    }

    public void Walk()
    {
        _rb.velocity = InputManager.Instance.MoveDir * _walkSpeed;
    }

    public void Sprint()
    {
        _rb.velocity = InputManager.Instance.MoveDir * _sprintSpeed;
    }

    #region Main Fire Click
    public void GunMainShot()
    {
        _currentWeapon.Fire();
    }

    public void GunMainShotAuto()
    {
        _currentWeapon.AutoFire();
    }

    public void GunMainShotRelease()
    {
        _currentWeapon.Release();
    }
    #endregion Main Fire Click


    #region Secondary Click

    public void GunSecondMode()
    {
        _isAiming = true;
        _aimLine.gameObject.SetActive(true);
        _currentWeapon.Secondary();
    }

    public void GunSecondModeRelease()
    {
        _isAiming = false;
        _aimLine.gameObject.SetActive(false);
    }
    #endregion Secondary Click

    public void AmmoTest() //THIS SYSTEM HIGHLY NEED A REWORK SINCE IT WONT WORK CORRECTLY FOR EVERY GUNS
    {
       // Debug.Log("Test");
        Vector2 magazineOffsetmin = UIManager.Instance.UIController.RifleMagazine1Slider.rectTransform.offsetMin;
        Vector2 magazineOffsetmax = UIManager.Instance.UIController.RifleMagazine1Slider.rectTransform.offsetMax;


        UIManager.Instance.UIController.RifleMagazine1Slider.rectTransform.offsetMin = new Vector2(magazineOffsetmin.x, 
        Mathf.Lerp(magazineOffsetmax.y, UIManager.Instance.UIController.BottomValueOfMagazine, (float)_rifleMagazine[0] / (float)_currentWeapon.MagazineAmmoCount));

        Debug.Log(UIManager.Instance.UIController.RifleMagazine1Slider.rectTransform.offsetMin);
        Debug.Log((float)_rifleMagazine[0]);
        Debug.Log((float)_currentWeapon.MagazineAmmoCount);

        Debug.Log(Mathf.Lerp(magazineOffsetmax.y, UIManager.Instance.UIController.BottomValueOfMagazine, (float)_rifleMagazine[0] / (float)_currentWeapon.MagazineAmmoCount));

        //IT IS BECAUSE WE MODIFY THE MAGAZINEOFFSETMIN, BECAUSE OF THIS THE 'a' and 'b' value of the calcul is different everytime. 
        //It means the solution is to set the offsetMin of the magazine at start or to have a fixed value =/
    }

    public void MagazineUpdate()    //THERE IS FOR SURE A BETTER LOGIC TO THIS BUT IT  TAKE A WHILE TO MAKE IT
    {
        switch (_currentWeapon.WeaponType)
        {
            case EWeaponType.RIFLE:

                Vector2 currentMagazineMin = UIManager.Instance.UIController.RifleMagazine1Slider.rectTransform.offsetMin;
                Vector2 currentMagazineMax = UIManager.Instance.UIController.RifleMagazine1Slider.rectTransform.offsetMax;

                UIManager.Instance.UIController.RifleMagazine1Slider.rectTransform.offsetMin = new Vector2(currentMagazineMin.x,
                Mathf.Lerp(-currentMagazineMax.y, UIManager.Instance.UIController.BottomValueOfMagazine,
                (float)_currentWeapon.AmmoCount / (float)_currentWeapon.MagazineAmmoCount));




                if (_rifleMagazine.Count > 0)
                {

                    if (_rifleMagazine.Count > 2)
                    {
                        UIManager.Instance.UIController.PlusSign.gameObject.SetActive(true);
                        UIManager.Instance.UIController.MagazineNumber.gameObject.SetActive(true);
                        UIManager.Instance.UIController.MagazineNumber.text = (_rifleMagazine.Count +1).ToString();

                    }
                    else
                    {
                        UIManager.Instance.UIController.PlusSign.gameObject.SetActive(false);
                        UIManager.Instance.UIController.MagazineNumber.gameObject.SetActive(false);
                    }

                    for(int i = 0; i < _rifleMagazine.Count; i++)
                    {
                        Debug.Log("i = " + i + " / Count = " + _rifleMagazine.Count);
                        if (i == 0)
                        {
                            UIManager.Instance.UIController.RifleObj2Magazine.SetActive(true);
                            UIManager.Instance.UIController.RifleObj3Magazine.SetActive(false);


                            int maxAmmoInMagazineList = _rifleMagazine.Max();   //We start by finding what's the fullest magazine
                            int fullestMagazine = _rifleMagazine.IndexOf(maxAmmoInMagazineList);


                            RawImage magazineIcon = UIManager.Instance.UIController.RifleMagazine2Slider; //We tell what magazine to change

                            Vector2 magazineOffsetmin = magazineIcon.rectTransform.offsetMin; //We get the necessary value
                            Vector2 magazineOffsetmax = magazineIcon.rectTransform.offsetMax;

                            magazineIcon.rectTransform.offsetMin = new Vector2(magazineOffsetmin.x, //We do a bit of math here to correctly setup the magazine color depending on the ammunition count
                            Mathf.Lerp(-magazineOffsetmax.y, UIManager.Instance.UIController.BottomValueOfMagazine,
                            (float)_rifleMagazine[fullestMagazine] / (float)_currentWeapon.MagazineAmmoCount));



                        }
                        else if (i== 1)
                        {
                            UIManager.Instance.UIController.RifleObj3Magazine.SetActive(true);


                            Debug.Log("TEST");
                            UIManager.Instance.UIController.RifleMagazine3Slider.gameObject.SetActive(true);

                            int firstMagazineList = _rifleMagazine.Max();   //Same system but with the third magazine this time
                            int firstMagazineIndex = _rifleMagazine.IndexOf(firstMagazineList);

                            int temporarySecondMagazine = firstMagazineList; //We place this magazine ammo count in a temporary variable
                            Debug.Log(firstMagazineIndex);
                            _rifleMagazine.RemoveAt(firstMagazineIndex); //We remove it from the list to be able to find the fullest second magazine, dw we are adding it later



                            int maxAmmoInMagazineList = _rifleMagazine.Max();   //This time we do search the second highest magazine
                            int fullestMagazine = _rifleMagazine.IndexOf(maxAmmoInMagazineList);



                            RawImage magazineIcon = UIManager.Instance.UIController.RifleMagazine3Slider;

                            Vector2 magazineOffsetmin = magazineIcon.rectTransform.offsetMin;
                            Vector2 magazineOffsetmax = magazineIcon.rectTransform.offsetMax;

                            magazineIcon.rectTransform.offsetMin = new Vector2(magazineOffsetmin.x,
                            Mathf.Lerp(-magazineOffsetmax.y, UIManager.Instance.UIController.BottomValueOfMagazine,
                            (float)_rifleMagazine[fullestMagazine] / (float)_currentWeapon.MagazineAmmoCount));


                            _rifleMagazine.Add(temporarySecondMagazine);
                        }

                    }

             
                }
                else
                {
                    UIManager.Instance.UIController.PlusSign.gameObject.SetActive(false);
                    UIManager.Instance.UIController.MagazineNumber.gameObject.SetActive(false);
                    UIManager.Instance.UIController.RifleObj2Magazine.SetActive(false);
                    UIManager.Instance.UIController.RifleObj3Magazine.SetActive(false);

                }

                //SHOW THE AMMO COUNT AND THE MAGAZINE ICON
                break;
            case EWeaponType.PISTOL:

                break;
            case EWeaponType.SHOTGUN:

                break;
        }

    }

    public void SecondarySwitch()
    {
        Weapon tempWeapon = _currentWeapon;
        _currentWeapon = _secondaryWeapon;
        _secondaryWeapon = tempWeapon;

        //TO PLACE ON ANOTHER METHOD PROBABLY
       
    }

    #region Reload & Repack
    public void GunReload()
    {
        _currentWeapon.Reload();
    }

    public void FastGunReload()
    {
        _currentWeapon.FastReload();
    }

    public void Repack()
    {
        UIManager.Instance.UIController.RifleMagazine1Slider.rectTransform.offsetMin = new Vector2(UIManager.Instance.UIController.RifleMagazine1Slider.rectTransform.offsetMin.x, 30);
    }
    #endregion Reload & Repack

    #endregion Methods



}
