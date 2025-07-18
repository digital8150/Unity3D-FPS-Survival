using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class actionController : MonoBehaviour
{
    [SerializeField]
    private float range; //습득 가능한 최대 거리

    private bool pickupActivated = false; //습득 가능할 시 true

    private RaycastHit hitInfo;

    //아이템 레이어에만 반응하도록 레이어 마스크 설정
    [SerializeField]
    private LayerMask layerMask;

    [Header("필요한 컴포넌트 연결")]
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory theInventory;


    // Update is called once per frame
    void Update()
    {
        CheckItem();
        TryAction();
    }

    void TryAction()
    {

        if(Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickup();
        }
    }
    
    void CanPickup()
    {
        if (pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                Debug.Log($"{hitInfo.transform.GetComponent<ItemPickup>().item.itemName}를 획득했습니다.");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickup>().item);
                Destroy(hitInfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }

    void CheckItem()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            if(hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
            else
            {
                ItemInfoDisappear();
            }
        }
        else
        {
            ItemInfoDisappear() ;
        }
    }

    void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = $"{hitInfo.transform.GetComponent<ItemPickup>().item.itemName} 획득 <color=yellow> (E) </color> ";
    }

    void ItemInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);

    }
}
