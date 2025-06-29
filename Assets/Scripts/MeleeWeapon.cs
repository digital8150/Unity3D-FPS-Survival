using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public string meleeWeaponName; //���� ���� �̸�

    //���� ����
    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;

    public float range; //���� ����
    public int damage;
    public float workSpeed; //�۾��ӵ�
    public float attackDelay; //���� ������
    public float attackDelayA; //���� Ȱ��ȭ ����
    public float attackDelayB; //���� ��Ȱ��ȭ ����

    public Animator anim;
    
}
