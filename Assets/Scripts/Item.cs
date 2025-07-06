using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject //게임 오브젝트에 부착할 필요 X
{
    public string itemName; //아이템 이름
    public ItemType itemType;
    public Sprite itemImage; //world
    public GameObject itemPrefab;

    public string weaponType;

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }

}
