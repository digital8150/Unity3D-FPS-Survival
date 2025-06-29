using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;
    public float range;
    public float accuracy;
    public float fireRate;
    public float reloadTime;

    public int damage; //���� ������

    public int reloadBulletCount; //�Ѿ� ������ ����
    public int currentBulletCount;
    public int maxBulletCount; // �ִ� ���� ���� �Ѿ� ����
    public int carryBulletCount; //���� �����ϰ� �ִ� �Ѿ� ����

    public float retroActionForce; //�ݵ�����
    public float retroActionADSForce; //�����ؽ��� �ݵ� ����

    public Vector3 ADSOriginPos; //������ ��ġ
    public Animator anim;

    public ParticleSystem muzleFlash; //�ѱ�ȭ����ƼŬ

    public AudioClip fire_sound;
}
