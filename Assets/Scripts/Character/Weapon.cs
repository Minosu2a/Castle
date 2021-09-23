using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public abstract class Weapon : MonoBehaviour
{
    #region Fields

    [SerializeField] private EWeaponType _weaponType = EWeaponType.PISTOL;


    [Header("References")]
    [SerializeField] protected Bullet _bullet = null;
    [SerializeField] protected Transform _container = null;
    [SerializeField] protected Transform _projectileStartPos = null;

    [Header("Values")]
    [SerializeField] protected int _magazineAmmoCount = 9;
    [SerializeField] protected float _fireRate = 0.2f;
    [SerializeField] protected float _weaponSpread = 5f;
    [SerializeField] protected float _aimDivider = 5f; //WHEN YOU AIM THE SPREAD IS DIVIDED BY THIS VALUE

    [HorizontalGroup("ReloadTime")]
    [SerializeField] protected float _reloadNormalTime = 1.8f;
    [HorizontalGroup("ReloadTime")]
    [SerializeField] protected float _reloadFastTime = 1.2f;

    IEnumerator _reloadingCoroutine;
    protected int _ammoCount = 0;

    protected bool _triggerReleased = true;

    protected bool _weaponOnCoolDown = false;

    [Header("Sounds")]
    [SerializeField] protected SoundData _soundReloadNormal = null;
    [SerializeField] protected SoundData _soundReloadFast = null;
    [SerializeField] protected SoundData[] _soundFire = null;


    #endregion Fields


    #region Property

    public EWeaponType WeaponType => _weaponType;

    public int MagazineAmmoCount => _magazineAmmoCount;

    public float Firerate
    {
        get
        {
           return _fireRate;
        }
        set 
        {
            _fireRate = value;
            Debug.Log("Done");
        }
    }
    #endregion Property

    #region Methods

    protected virtual void Start()
    {
        _ammoCount = _magazineAmmoCount;
    }

    #region Fire
    public virtual void Fire()
    {
        //CHECK IF PLAYER IS RELOADING -> IF ITS THE CASE IT WILL CANCEL THE RELOAD
        if(InputManager.Instance.IsReloading == true)
        {
            Cancel();
        }
        //CHECK IF PLAYER GOT MAGAZINES

        if (_ammoCount <= 0) //CHECK IF PLAYER GOT BULLETS LEFT IN THE GUN
        {
            AudioManager.Instance.Start2DSound("S_PullEmpty");
            _triggerReleased = false;
        }
        else if(_triggerReleased == true) //IF THE CHARACTER FINGER HAS ALREADY PULLED THE TRIGGER (IF NOT HE HAS TO REALEASE IT TO BE ABLE TO FIRE AGAIN)
        {
            _triggerReleased = false;

            ProjectileSpawn();

            int i = Random.Range(0, _soundFire.Length);
            AudioManager.Instance.Start2DSound(_soundFire[i].name);

            _ammoCount--;

            UIManager.Instance.UIController.AmmoText.text = _ammoCount.ToString(); //UPDATE THE AMMO ACCOUNT ON THE UI
 
        }

    }
    protected virtual void ProjectileSpawn() //YOU MAY NEED TO ADJUST THIS METHOD DEPENDING ON THE SPRAY/SPREAD RULES OF YOUR GUN (THE ACCURACY)
    {
        Bullet bulletClone = Instantiate(_bullet, _projectileStartPos.position, _projectileStartPos.rotation, _container); //CREATE THE BULLET
        
        if(CharacterManager.Instance.CharacterController.IsAiming == true)
        {
            bulletClone.Init(transform.forward, (_weaponSpread / _aimDivider)); //GIVE THE DIRECTION TO THE BULLET + THE WEAPON SPREAD (VALUE CHANGE DEPENDING IF THE PLAYER IS AIMING OR NOT)
        }
        else
        {
            bulletClone.Init(transform.forward, _weaponSpread); //PLAYER IS NOT AIMING SO THE WEAPON SPREAD IS NOT DIVIDED BY ACCURACY FACTOR (_aimDivider)
        }
    }

    public virtual void AutoFire()
    {
      //BY DEFAULT ALL WEAPON ARE ON SEMI-AUTO, OVERRIDE THIS METHOD IN CHILD CLASS
    }

    public virtual void Release() //WHEN THE CHARACTER RELEASE THE GUN TRIGGER
    {
        _triggerReleased = true;
    }


    #endregion Fire

    public virtual void Secondary() //SECONDARY MODE, BY DEFAULT ALL WEAPONS HAVE A AIM ACTION IN THEIR SECONDARY (RIGHT CLICK)
    {
    
        CharacterManager.Instance.CharacterController.AimLine.SetPosition(0, _projectileStartPos.position);
        CharacterManager.Instance.CharacterController.AimLine.SetPosition(1, CharacterManager.Instance.CharacterController.PosMouse);

        Debug.DrawLine(_projectileStartPos.position, CharacterManager.Instance.CharacterController.PosMouse);
    }

    #region Reload
    public virtual void Reload() //RELOADING METHOD (R)
    {
        _reloadingCoroutine = ReloadDelay(_reloadNormalTime);
       StartCoroutine(_reloadingCoroutine);
    }

    public virtual void FastReload() //FAST RELOADING METHOD (DOUBLE R)
    {
        _reloadingCoroutine = ReloadDelay(_reloadFastTime);
        StartCoroutine(_reloadingCoroutine);
    }

    protected virtual void AmmunitionUpdate(bool isFastReload)
    {

            int oldAmmoCount = _ammoCount;

            int maximumAmmoInMagazineList = CharacterManager.Instance.CharacterController.PistolMagazine.Max(); //WE LOOK FOR THE FULLEST MAGAZINE TO LOAD IT


            _ammoCount = maximumAmmoInMagazineList; //WE PLACE THE AMMO ACCOUNT OF THE MAGAZINE IN THE GUN


            int index = CharacterManager.Instance.CharacterController.PistolMagazine.IndexOf(maximumAmmoInMagazineList);
            CharacterManager.Instance.CharacterController.PistolMagazine.Remove(CharacterManager.Instance.CharacterController.PistolMagazine[index]); //SINCE WE JUST LOADED THE MAGAZINE NOW WE CAN REMOVE IT FROM THE LIST

            
            if (oldAmmoCount > 0 && isFastReload == false) //CHECK AMMO ACCOUNT IF THE PREVIOUS MAGAZINE WAS EMPTY, NOTHING IS DONE. IF PLAYER DID THE FAST RELOAD NOTHING IS DONE (MAGAZINE IS NOT ADDED BACK TO THE PLAYER KIT)
            {
                CharacterManager.Instance.CharacterController.PistolMagazine.Add(oldAmmoCount); //WE ADD THE OLD MAGAZINE TO THE MAGAZINE LIST
            }
        
    }

    protected virtual IEnumerator ReloadDelay(float reloadTime)
    {
        AudioManager.Instance.Start2DSound(_soundReloadNormal.name);
        yield return new WaitForSeconds(reloadTime);
        Debug.Log("Reloading Finished after : " + reloadTime + " seconds");

        if(reloadTime == _reloadFastTime)
        {
            AmmunitionUpdate(true);
        }
        else
        {
            AmmunitionUpdate(false);
        }

        InputManager.Instance.IsReloading = false;
        UIManager.Instance.UIController.AmmoText.text = _ammoCount.ToString(); //UPDATE THE AMMO ACCOUNT ON THE UI

    }


    protected virtual void Cancel()
    {
        StopCoroutine(_reloadingCoroutine);
        Debug.Log("Reloading Canceled");
        InputManager.Instance.IsReloading = false;

    }
    #endregion Reload


    protected virtual void Update()
    {

    }

 
    #endregion Methods

}
