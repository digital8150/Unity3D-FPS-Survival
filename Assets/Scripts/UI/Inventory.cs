using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    [Header("�ʿ��� ������Ʈ ����")]
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_Slots_Parent;

    private Slot[] slots;


    // Start is called before the first frame update
    void Start()
    {
        slots = go_Slots_Parent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            go_InventoryBase.SetActive(inventoryActivated);
        }
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if(_item.itemType != Item.ItemType.Equipment) //��� �������� �ƴ� ��쿡�� ���� ������ ���Կ� ���� ����
        {
            foreach (Slot slot in slots)
            {
                if (slot.item != null && slot.item.itemName == _item.itemName)
                {
                    slot.SetSlotCount(_count);
                    return;
                }
            }
        }


        foreach(Slot slot in slots)
        {
            if(slot.item is null)
            {
                slot.AddItem(_item, _count);
                return;
            }
        }
    }
}

