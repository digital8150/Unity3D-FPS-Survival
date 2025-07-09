using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class ItemEffect
{
    public string itemName;
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY 만 가능")]
    public string[] part;
    public int[] num;
}

public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;
    [SerializeField]
    private StatusController statusController;

    [Header("필요한 컴포넌트 연결")]
    [SerializeField]
    private WeaponManager weaponManager;

    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    public void UseItem(Item _item)
    {

        if (_item.itemType == Item.ItemType.Equipment)
        {
            //장착
            StartCoroutine(weaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
            
        }
        else if (_item.itemType == Item.ItemType.Used)
        {
            for (int x = 0; x < itemEffects.Length; x++)
            {
                if (itemEffects[x].itemName == _item.itemName)
                {
                    for(int y = 0; y < itemEffects[x].part.Length; y++)
                    {
                        switch (itemEffects[x].part[y])
                        {
                            case HP:
                                statusController.IncreaseHP(itemEffects[x].num[y]);
                                break;
                            case SP:
                                statusController.IncreaseSP(itemEffects[x].num[y]);
                                break;
                            case DP:
                                statusController.IncreaseDP(itemEffects[x].num[y]);

                                break;
                            case HUNGRY:
                                statusController.IncreaseHungry(itemEffects[x].num[y]);

                                break;
                            case THIRSTY:
                                statusController.IncreaseThristy(itemEffects[x].num[y]);

                                break;
                            case SATISFY:
                                statusController.IncreaseSatisfy(itemEffects[x].num[y]);
                                break;
                            default:
                                Debug.Log("잘못된 Status 부위");
                                break;
                        }

                    }
                    return;
                }
            }
            Debug.Log($"itemEffectDatabse에 일치하는 아이템 이름 {_item.itemName}이 없습니다.");
        }
    }
}
