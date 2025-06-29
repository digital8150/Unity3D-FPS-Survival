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

    public int damage; //총의 데미지

    public int reloadBulletCount; //총알 재장전 개수
    public int currentBulletCount;
    public int maxBulletCount; // 최대 소유 가능 총알 개수
    public int carryBulletCount; //현재 소유하고 있는 총알 개수

    public float retroActionForce; //반동세기
    public float retroActionADSForce; //정조준시의 반동 세기

    public Vector3 ADSOriginPos; //정조준 위치
    public Animator anim;

    public ParticleSystem muzleFlash; //총구화염파티클

    public AudioClip fire_sound;
}
