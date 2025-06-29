using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //무기 중복 교체 실행 방지
    public static bool isChangeWeapon = false;

    //현재 무기와 현재 무기의 에니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;
    [SerializeField]
    private string currentWeaponType;

    //무기 교체 딜레이
    [SerializeField]
    private float changeWeaponDelayTime;

    //무기 교체가 완전히 끝난 시점
    [SerializeField]
    private float changeWeaponEndDelayTime;

    //무기 종류들 전부 관리
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private MeleeWeapon[] hands;
    [SerializeField]
    private MeleeWeapon[] axes;
    [SerializeField]
    private MeleeWeapon[] Pickaxes;


    //관치 차원에서 쉽게 무기 접근이 가능하도록 Dicitionary 를 이용
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, MeleeWeapon> handDictionary = new Dictionary<string, MeleeWeapon>();
    private Dictionary<string, MeleeWeapon> axeDictionary = new Dictionary<string, MeleeWeapon>();
    private Dictionary<string, MeleeWeapon> PickaxeDictionary = new Dictionary<string, MeleeWeapon>();


    //필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private AxeController theAxeController;
    [SerializeField]
    private PickaxeController thePickaxeController;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < guns.Length; i++) {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }

        for (int i = 0; i < hands.Length; i++) {
            handDictionary.Add(hands[i].meleeWeaponName, hands[i]);
        }

        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].meleeWeaponName, axes[i]);
        }

        for (int i = 0; i < Pickaxes.Length; i++)
        {
            PickaxeDictionary.Add(Pickaxes[i].meleeWeaponName, Pickaxes[i]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
            }
            else if(Input.GetKeyDown(KeyCode.Alpha4))
            {
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "Pickaxe"));
            }
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPrevWeaponAction();

        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);
        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    void CancelPrevWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":

                theGunController.CancelADS();
                theGunController.CancelReload();
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                break;
            case "AXE":
                AxeController.isActivate=false;
                break;
            case "PICKAXE":
                PickaxeController.isActivate=false;
                break;
            default:
                break;
        }
    }

    void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")
        {
            theGunController.GunChange(gunDictionary[_name]);
        }
        else if (_type == "HAND")
        {
            theHandController.MeleeWEaponChange(handDictionary[_name]);
        }else if(_type == "AXE")
        {
            theAxeController.MeleeWEaponChange(axeDictionary[_name]);
        }else if(_type == "PICKAXE")
        {
            thePickaxeController.MeleeWEaponChange(PickaxeDictionary[_name]);
        }
    }
}
