using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp; //������ ü��

    [SerializeField]
    private float destroyTime; //���� ���� �ð�

    [SerializeField]
    private int dropCount;

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
    private GameObject go_rock_item_prefab;

    [Header("�ʿ��� ���� �̸�")]
    [SerializeField]
    private string strikeSound;
    [SerializeField]
    private string destorySound;

    public void Mining()
    {
        SoundManager.instance.PlaySE(strikeSound);
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
        SoundManager.instance.PlaySE(destorySound);
        col.enabled = false;

        for(int i = 0; i < dropCount; i++) Instantiate(go_rock_item_prefab, go_rock.transform.position, Quaternion.identity);

        Destroy(go_rock);
        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }
}
