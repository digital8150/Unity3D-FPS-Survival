using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    [Header("필요한 컴포넌트 연결")]
    [SerializeField]
    private GameObject go_Base;

    [SerializeField]
    private Text txt_itemName;
    [SerializeField]
    private Text txt_itemDesc;
    [SerializeField]
    private Text txt_itemHowTouse;


    public void ShowToolTip(Item _item, Vector3 _position)
    {
        go_Base.SetActive(true);
        _position += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f, -go_Base.GetComponent<RectTransform>().rect.height * 0.5f, 0f);
        go_Base.transform.position = _position;

        txt_itemName.text = _item.itemName;
        txt_itemDesc.text = _item.itemDescription;

        if (_item.itemType == Item.ItemType.Equipment)
        {
            txt_itemHowTouse.text = "우클릭 - 장착";
        } 
        else if (_item.itemType == Item.ItemType.Used) 
        {
            txt_itemHowTouse.text = "우클릭 - 먹기";
        }
        else
        {
            txt_itemHowTouse.text = "";
        }
    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
