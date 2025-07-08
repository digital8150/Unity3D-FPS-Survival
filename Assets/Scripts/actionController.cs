using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class actionController : MonoBehaviour
{
    [SerializeField]
    private float range; //���� ������ �ִ� �Ÿ�

    private bool pickupActivated = false; //���� ������ �� true

    private RaycastHit hitInfo;

    //������ ���̾�� �����ϵ��� ���̾� ����ũ ����
    [SerializeField]
    private LayerMask layerMask;

    [Header("�ʿ��� ������Ʈ ����")]
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
                Debug.Log($"{hitInfo.transform.GetComponent<ItemPickup>().item.itemName}�� ȹ���߽��ϴ�.");
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
        actionText.text = $"{hitInfo.transform.GetComponent<ItemPickup>().item.itemName} ȹ�� <color=yellow> (E) </color> ";
    }

    void ItemInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);

    }
}
