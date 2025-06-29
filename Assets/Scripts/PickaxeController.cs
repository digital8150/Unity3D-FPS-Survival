using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : MeleeWeaponController
{
    public static bool isActivate = true;

    private void Start()
    {
        WeaponManager.currentWeapon = currentMeleeWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentMeleeWeapon.anim;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
        {
            TryAttack();
        }

    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    public override void MeleeWEaponChange(MeleeWeapon _currentMeleeWeapon)
    {
        base.MeleeWEaponChange(_currentMeleeWeapon);
        isActivate = true;
    }
}
