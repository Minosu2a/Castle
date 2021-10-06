using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    #region Fields
    private Vector3 _moveDir = Vector3.zero;
    private Vector3 _rotDir = Vector3.zero;

    [Header("Mouse")]
    [SerializeField] private bool _mouseActivated = true;

    [Header("Action")]
    [SerializeField] private KeyCode _mainFireInput = KeyCode.Mouse0;
    [SerializeField] private KeyCode _secondFireInput = KeyCode.Mouse1;
    [SerializeField] private KeyCode _reloadInput = KeyCode.R;
    [SerializeField] private KeyCode _gunSwitchInput = KeyCode.Tab;
    [SerializeField] private KeyCode _repackAmmoInput = KeyCode.F;



    [SerializeField] private float _timerForFastReloadInput = 0.5f;
    [SerializeField] private float _timerForAmmoCheck = 0.65f;


    private float _timeStamp = 0;
    private bool _hasPressedReloadOnce = false;
    private bool _ammoCheckDone = false;
    private bool _isReloading = false;


    #endregion Fields

    #region Property
    public Vector3 MoveDir => _moveDir.normalized;
    public Vector3 RotDir => _rotDir.normalized;


    public bool MouseActivated
    {
        get
        {
            return _mouseActivated;
        }
        set
        {
            _mouseActivated = value;
        }
    }

    public bool IsReloading
    {
        get
        {
            return _isReloading;
        }
        set
        {
            _isReloading = value;
        }
    }

    #endregion Property

    #region Methods
    public void Initialize()
    {

    }


    protected override void Update()
    {
        #region Movement & Rotation
        _moveDir.x = Input.GetAxis("Horizontal");
        _moveDir.z = Input.GetAxis("Vertical");


        if (_mouseActivated == true)
        {

        }
        else
        {
            _rotDir.x = Input.GetAxis("RotationX");
            _rotDir.z = Input.GetAxis("RotationZ");
        }
        #endregion Movement & Rotation

        #region Main Click
        if (Input.GetKey(_mainFireInput))
        {
            CharacterManager.Instance.CharacterController.GunMainShotAuto();
        }

        if (Input.GetKeyDown(_mainFireInput))
        {
            CharacterManager.Instance.CharacterController.GunMainShot();
        }

        if (Input.GetKeyUp(_mainFireInput))
        {
            CharacterManager.Instance.CharacterController.GunMainShotRelease();
        }
        #endregion Main Click

        #region Secondary Click
        if (Input.GetKey(_secondFireInput))
        {
            CharacterManager.Instance.CharacterController.GunSecondMode();
        }

        if (Input.GetKeyUp(_secondFireInput))
        {
            CharacterManager.Instance.CharacterController.GunSecondModeRelease();
        }
        #endregion Secondary Click

        #region Reload
        //NOT SO SURE THAT THIS IF + SWITCH IS WELL OPTIMISED, THERE IS MAYBE BE A BETTER WAY TO DO IT.

        if (CharacterManager.Instance.CharacterController != null) //IF TO PREVENT THE NEXT LINES TO TRIGGER BEFORE THE CHARACTERCONTROLLER IS EVEN LOADED IN
        {
            switch (CharacterManager.Instance.CharacterController.CurrentWeapon.WeaponType) //WE CHECK WHAT WEAPON THE PLAYER HAS TO HAVE THE ACCORDING AMMUNITION CHECK
            {
                case EWeaponType.PISTOL:
                    if (CharacterManager.Instance.CharacterController.PistolMagazine.Count > 0)
                    {
                        ReloadCheck();
                    }
                    else if (Input.GetKeyDown(_reloadInput))
                    {
                        //IN THEORY THE PLAYER IS OUT OF MAGAZINE HERE
                        CharacterManager.Instance.CharacterController.MagazineUpdate();
                    }
                    break;
                case EWeaponType.RIFLE:
                    if (CharacterManager.Instance.CharacterController.RifleMagazine.Count > 0)
                    {
                        ReloadCheck();
                    }
                    else if (Input.GetKeyDown(_reloadInput))
                    {
                        //IN THEORY THE PLAYER IS OUT OF MAGAZINE HERE
                        CharacterManager.Instance.CharacterController.MagazineUpdate();
                    }
                    break;
                case EWeaponType.SHOTGUN:
                    if (CharacterManager.Instance.CharacterController.ShotgunAmmo > 0)
                    {
                        if(Input.GetKeyDown(_reloadInput) && _isReloading == false) //WE REPLACE THE RELOADCHECK() BY THIS NEW SYSTEM FOR THE SHOTGUN
                        {
                            CharacterManager.Instance.CharacterController.GunReload();
                            _isReloading = true;
                            CharacterManager.Instance.CharacterController.MagazineUpdate();
                        }
                    }
                    else if (Input.GetKeyDown(_reloadInput))
                    {
                        //IN THEORY THE PLAYER IS OUT OF MAGAZINE HERE
                        CharacterManager.Instance.CharacterController.MagazineUpdate();
                    }
                    break;
            }
        }



        #endregion Reload


        if (Input.GetKeyUp(_repackAmmoInput))
        {
            CharacterManager.Instance.CharacterController.MagazineUpdate();
        }

        if (Input.GetKeyUp(_gunSwitchInput))
        {
            CharacterManager.Instance.CharacterController.MagazineUpdate();
        }
    }

    private void ReloadCheck()
    {

        if (Input.GetKey(_reloadInput))
        {
            _timeStamp += Time.deltaTime;
            if (_timeStamp >= _timerForAmmoCheck)   
            {
                CharacterManager.Instance.CharacterController.MagazineUpdate();
                _ammoCheckDone = true;
                _timeStamp = 0;
            }
        }
        else if (Input.GetKeyUp(_reloadInput) && _hasPressedReloadOnce == false && _isReloading == false && _ammoCheckDone == false) //PRESS RELOAD A FIRST TIME (THE SYSTEM WAIT FOR THE PLAYER TO EITHER : 
        {
            _hasPressedReloadOnce = true;
        }
        else if (Input.GetKeyDown(_reloadInput) && _hasPressedReloadOnce == true) //1 - PRESS R AGAIN TO TRIGGER THE FAST RELOAD
        {
            CharacterManager.Instance.CharacterController.FastGunReload();
            _hasPressedReloadOnce = false;
            _timeStamp = 0;
            _isReloading = true;
        }
        else if (_hasPressedReloadOnce == true)
        {
            _timeStamp += Time.deltaTime;
            if (_timeStamp >= _timerForFastReloadInput)   //2 - WAIT A FEW (Depending on _timerForFastReloadInput) AND RELOAD IF THE TIMER HAS RUN OUT (SYSTEM STOP TO WAIT TO SEE IF THE PLAYER IS GOING TO DO A FAST RELOAD INSTEAD)
            {
                CharacterManager.Instance.CharacterController.GunReload();
                _hasPressedReloadOnce = false;
                _timeStamp = 0;
                _isReloading = true;
            }
        }
        else if(_ammoCheckDone == true)
        {
            _ammoCheckDone = false;
        }
    }


    #endregion Methods

}
