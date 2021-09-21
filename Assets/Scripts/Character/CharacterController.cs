using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
    }
    #endregion Reload & Repack

    #endregion Methods



}
