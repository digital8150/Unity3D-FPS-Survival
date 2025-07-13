using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeaponController : MonoBehaviour
{


    //���� ������ Hand�� Ÿ�� ����
    [SerializeField]
    protected MeleeWeapon currentMeleeWeapon;

    //������?
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;
    [SerializeField]
    protected LayerMask layerMask = 0;



    protected void TryAttack()
    {
        if (Input.GetButton("Fire1") && !Inventory.inventoryActivated)
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;

        currentMeleeWeapon.anim.SetTrigger("Attack");
        yield return new WaitForSeconds(currentMeleeWeapon.attackDelayA);
        isSwing = true;

        //���� Ȱ��ȭ ����
        StartCoroutine(HitCoroutine());
        yield return new WaitForSeconds(currentMeleeWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentMeleeWeapon.attackDelay - currentMeleeWeapon.attackDelayA - currentMeleeWeapon.attackDelayB);
        isAttack = false;
    }

    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, currentMeleeWeapon.range, layerMask))
        {
            return true;
        }
        return false;
    }

    //���� �Լ� 
    public virtual void MeleeWEaponChange(MeleeWeapon _currentMeleeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        currentMeleeWeapon = _currentMeleeWeapon;
        WeaponManager.currentWeapon = currentMeleeWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentMeleeWeapon.anim;

        currentMeleeWeapon.transform.localPosition = Vector3.zero;
        currentMeleeWeapon.gameObject.SetActive(true);
    }
}
