using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp; //바위의 체력

    [SerializeField]
    private float destroyTime; //파편 제거 시간

    [SerializeField]
    private int dropCount;

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
    private GameObject go_rock_item_prefab;

    [Header("필요한 사운드 이름")]
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
