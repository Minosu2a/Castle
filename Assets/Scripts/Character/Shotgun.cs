using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private SoundData _pumpSound = null;
    [SerializeField] private int _shotgunProjectileNumber = 6;

        protected override void Start()
        {
            
           base.Start();

        }

    protected override void ProjectileSpawn()
    {
        for (int i = 0; i < _shotgunProjectileNumber; i++)
        {
            Bullet bulletClone = Instantiate(_bullet, _projectileStartPos.position, _projectileStartPos.rotation, _container); //CREATE THE BULLET
            bulletClone.Init(transform.forward, _weaponSpread); //PLAYER IS NOT AIMING SO THE WEAPON SPREAD IS NOT DIVIDED BY ACCURACY FACTOR (_aimDivider)
        }

    }

    public override void Secondary()
    {

    }


    protected override IEnumerator ReloadDelay(float reloadTime)
    {
        while (_ammoCount < _magazineAmmoCount)
        {
            AudioManager.Instance.Start2DSound(_soundReloadNormal.name);
            yield return new WaitForSeconds(reloadTime);
            Debug.Log("1 Shell loaded after : " + reloadTime + " seconds");

            _ammoCount++;
            CharacterManager.Instance.CharacterController.ShotgunAmmo--;

            UIManager.Instance.UIController.MagazineNumber.text = _ammoCount.ToString(); //UPDATE THE AMMO ACCOUNT ON THE UI
        }

        //DO THE SATSFYING 'CHOCK CHOCK' SOUND TO LET THE PLAYER KNOW HE IS GOOD TO KILL SOME FREAKS AGAIN
        AudioManager.Instance.Start2DSound(_soundReloadFast.name);
        yield return new WaitForSeconds(_reloadFastTime);

        InputManager.Instance.IsReloading = false;

    }
}
