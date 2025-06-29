using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("필요한 컴포넌트 연결")]
    [SerializeField]
    private GunController theGunController;
    private Gun currentGun;

    //필요하면 HUD 호출
    [SerializeField]
    private GameObject go_BulletHUD;

    [SerializeField]
    private Text[] text_Bullet;

    private void Update()
    {
        CheckBullet();
    }

    void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        text_Bullet[0].text = currentGun.carryBulletCount.ToString();
        text_Bullet[1].text = currentGun.reloadBulletCount.ToString();
        text_Bullet[2].text = currentGun.currentBulletCount.ToString();
    }
}
