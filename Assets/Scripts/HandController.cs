using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public static bool isActivate = false;

    //현재 장착된 Hand형 타입 무기
    [SerializeField]
    private Hand currentHand;

    //공격중?
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
        {
            TryAttack();
        }

    }

    void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;

        currentHand.anim.SetTrigger("Attack");
        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;

        //공격 활성화 시점
        StartCoroutine(HitCoroutine());
        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;
        
        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if(CheckObject())
            {
                isSwing = false;
                Debug.Log($"공격 감지 : {hitInfo.transform.name}");
            }
            yield return null;
        }
    }
    
    bool CheckObject()
    {
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }


    public void HandChange(Hand _hand)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        currentHand = _hand;
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentHand.anim;

        currentHand.transform.localPosition = Vector3.zero;
        currentHand.gameObject.SetActive(true);
        isActivate = true;
    }
}
