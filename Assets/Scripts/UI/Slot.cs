using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public int itemCount;
    public Image itemImage; //아이템의 이미지
    [Header("필요한 컴포넌트 연결")]
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_Count_Image;
    private ItemEffectDatabase itemEffectDatabase;

    void Start()
    {
        itemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
    }

    //이미지 투명도 조절
    void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    //슬롯 초기화
    void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "";
        go_Count_Image.SetActive(false);
    }

    //아이템 획득
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment)
        {
            go_Count_Image.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "";
            go_Count_Image.SetActive(false);

        }

        SetColor(1);
    }

    //아이템 개수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right && item != null)
        {
            itemEffectDatabase.UseItem(item);
            if(item.itemType == Item.ItemType.Used) SetSlotCount(-1);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;
        DragSlot.instance.dragSlot = this;
        DragSlot.instance.DragSetImage(itemImage);
        DragSlot.instance.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item == null) return;
        DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
        }
        Debug.Log("OnDrop");
    }

    void ChangeSlot()
    {
        Item tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
    
        if(tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(tempItem, _tempItemCount);
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }

    //마우스가 슬롯에 들어갈 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item is null) return;
        itemEffectDatabase.ShowToolTip(item, transform.position);
    }

    //마우스가 슬롯에서 빠져나갈 때
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item is null) return;
        itemEffectDatabase.HideToolTip();
    }
}
