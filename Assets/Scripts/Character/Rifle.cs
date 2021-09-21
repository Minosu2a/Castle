using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rifle : Weapon
{
    private float _timeStamp = 0;

    protected override void Start()
    {
       base.Start();
    }

    public override void AutoFire()
    {
        //CHECK IF PLAYER IS RELOADING -> IF ITS THE CASE IT WILL CANCEL THE RELOAD
        if (InputManager.Instance.IsReloading == true)
        {
            Cancel();
        }

        if (_ammoCount <= 0 && _triggerReleased == true) //CHECK IF PLAYER GOT BULLETS LEFT IN THE GUN || IF PLAYER HAS RELEASED THE TRIGGER (TO AVOID THE POSSIBILITIES OF TRIGGERING IT EVERY FRAME)
        {
            AudioManager.Instance.Start2DSound("S_PullEmpty");
            _triggerReleased = false;
        }
        else if (_ammoCount > 0 && _weaponOnCoolDown == false) //IF WEAPON HAS AMMO AND IF WEAPON IS STILL IN COOL DOWN (DEPENDING ON _firerate VALUE)
        {
          //  _triggerReleased = false; //ONLY UTILITY FOR THE RIFLE IS LINE 16

            _weaponOnCoolDown = true;
            ProjectileSpawn();

            int i = Random.Range(0, _soundFire.Length);
            AudioManager.Instance.Start2DSound(_soundFire[i].name);

            _ammoCount--;

            UIManager.Instance.UIController.AmmoText.text = _ammoCount.ToString(); //UPDATE THE AMMO ACCOUNT ON THE UI

        }
    }

    public override void Fire() //WE OVERRIDE THIS METHODS TO PREVENT TWO BULLET TO FIRE AT THE SAME TIME (THE FIRE AND AUTOFIRE CAN BOTH BE ACTIVATED AT THE SAME TIME)
    { 
        //PLACE NOTHING IN HERE, USE AUTOFIRE INSTEAD 
    }

    protected override void AmmunitionUpdate(bool isFastReload)
    {

        int oldAmmoCount = _ammoCount;

        int maximumAmmoInMagazineList = CharacterManager.Instance.CharacterController.RifleMagazine.Max(); //WE LOOK FOR THE FULLEST MAGAZINE TO LOAD IT


        _ammoCount = maximumAmmoInMagazineList; //WE PLACE THE AMMO ACCOUNT OF THE MAGAZINE IN THE GUN


        int index = CharacterManager.Instance.CharacterController.RifleMagazine.IndexOf(maximumAmmoInMagazineList);
        CharacterManager.Instance.CharacterController.RifleMagazine.Remove(CharacterManager.Instance.CharacterController.RifleMagazine[index]); //SINCE WE JUST LOADED THE MAGAZINE NOW WE CAN REMOVE IT FROM THE LIST


        if (oldAmmoCount > 0 && isFastReload == false) //CHECK AMMO ACCOUNT IF THE PREVIOUS MAGAZINE WAS EMPTY, NOTHING IS DONE. IF PLAYER DID THE FAST RELOAD NOTHING IS DONE (MAGAZINE IS NOT ADDED BACK TO THE PLAYER KIT)
        {
            CharacterManager.Instance.CharacterController.RifleMagazine.Add(oldAmmoCount); //WE ADD THE OLD MAGAZINE TO THE MAGAZINE LIST
        }

    }


    protected override void Update()
    {
        if(_weaponOnCoolDown == true)  //IF THE WEAPON IS ON COOLDOWN
        {
            _timeStamp += Time.deltaTime; //STARTING THE TIMER

            if(_timeStamp >= _fireRate)
            {
                _weaponOnCoolDown = false;
                _timeStamp = 0;
            }
        }
    }
}
