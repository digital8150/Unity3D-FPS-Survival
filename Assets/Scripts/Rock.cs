using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp; //바위의 체력

    [SerializeField]
    private float destroyTime; //파편 제거 시간

    [Header("컴포넌트 연결")]
    [SerializeField]
    private SphereCollider col; //구체 콜라이더
    [SerializeField]
    private GameObject go_rock; //일반 바위
    [SerializeField]
    private GameObject go_debris; //깨진 바위
    [SerializeField]
    private GameObject go_effect_prefabs;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip effectSound1;
    [SerializeField]
    private AudioClip effectSound2;

    public void Mining()
    {
        audioSource.clip = effectSound1;
        audioSource.Play();
        var clone = Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);
        hp--;
        if(hp <= 0)
        {
            Destruction();
        }
    }

    void Destruction()
    {
        audioSource.clip = effectSound2;
        audioSource.Play();
        col.enabled = false;
        Destroy(go_rock);
        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }
}
