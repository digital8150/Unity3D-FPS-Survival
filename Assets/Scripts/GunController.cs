using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //현재 활성화
    public static bool isActivate = true;

    //장착된 총
    [SerializeField]
    private Gun currentGun;

    //연사 속도 계산
    private float currentFireRate;

    //상태 변수
    private bool isReload = false;
    [HideInInspector]
    public bool isADSMode = false;

    private Vector3 originPos; //원래 포지션 값 

    //효과음 재생
    private AudioSource audioSource;

    private RaycastHit hitInfo;

    [Header("필요한 컴포넌트 연결")]
    [SerializeField]
    private Camera theCam;
    [SerializeField]
    private GameObject hitEffectPrefab;
    private Crosshair theCrosshair;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originPos = Vector3.zero;
        theCrosshair = FindObjectOfType<Crosshair>();
        
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            TryADS();
        }

    }

    //연사속도 재계산
    void GunFireRateCalc()
    {
        if(currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    void TryADS()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            ADS();
        }
    }

    public void CancelADS()
    {
        if (isADSMode)
        {
            ADS();
        }

    }

    void TryFire()
    {
        if(Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelADS();
            StartCoroutine(ReloadCoroutine());
        }
    }

    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    void ADS()
    {
        isADSMode = !isADSMode;
        theCrosshair.ADSAnimation(isADSMode);
        currentGun.anim.SetBool("FineSightMode", isADSMode);

        if (isADSMode)
        {
            StopAllCoroutines();
            StartCoroutine(ADSActivateCoroutine());
        }
        else
        { 
            StopAllCoroutines();
            StartCoroutine(ADSDeactivateCoroutine());
        }
    }

    IEnumerator ADSActivateCoroutine()
    {
        while (currentGun.transform.localPosition != currentGun.ADSOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.ADSOriginPos, 0.02f);
            yield return null;
        }
    }

    IEnumerator ADSDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.02f);
            yield return null;
        }
    }

    void Fire() //발사 전
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
            {
                Shoot();
            }
            else
            {
                CancelADS();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    void Shoot() //발사 후
    {
        theCrosshair.FireAnimation();
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; //연사 속도 재계산
        PlaySE(currentGun.fire_sound);
        currentGun.muzleFlash.Play();
        
        Hit(); //히트스캔 방식

        //총기 반동 코루틴
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }

    void Hit()
    {

        if(Physics.Raycast(theCam.transform.position, theCam.transform.forward + 
            new Vector3(Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        0)
            , out hitInfo, currentGun.range))
        {
            GameObject clone = Instantiate(hitEffectPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 2f);
        }
    }

    IEnumerator RetroActionCoroutine() //반동
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionADSForce, currentGun.ADSOriginPos.y, currentGun.ADSOriginPos.z);

        if (!isADSMode)
        {
            currentGun.transform.localPosition = originPos;
            //반동시작
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.2f);
                yield return null;
            }
            //원위치
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.05f);
                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.ADSOriginPos;
            //반동시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionADSForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.2f);
                yield return null;
            }
            //원위치
            while (currentGun.transform.localPosition != currentGun.ADSOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.ADSOriginPos, 0.05f);
                yield return null;
            }
        }
    }

    IEnumerator ReloadCoroutine() //재장전 코루틴
    {
        if(currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);


            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }
            isReload = false;
        }
        else
        {
            Debug.Log("총알 오링남");
        }
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    public Gun GetGun()
    {
        return currentGun;
    }

    public bool GetADSMode()
    {
        return isADSMode;
    }

    public void GunChange(Gun _gun)
    {
        if(WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        currentGun = _gun;
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);
        isActivate = true;
    }
}
