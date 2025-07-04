using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp; //������ ü��

    [SerializeField]
    private float destroyTime; //���� ���� �ð�

    [Header("������Ʈ ����")]
    [SerializeField]
    private SphereCollider col; //��ü �ݶ��̴�
    [SerializeField]
    private GameObject go_rock; //�Ϲ� ����
    [SerializeField]
    private GameObject go_debris; //���� ����
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
