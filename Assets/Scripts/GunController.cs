using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //���� Ȱ��ȭ
    public static bool isActivate = true;

    //������ ��
    [SerializeField]
    private Gun currentGun;

    //���� �ӵ� ���
    private float currentFireRate;

    //���� ����
    private bool isReload = false;
    [HideInInspector]
    public bool isADSMode = false;

    private Vector3 originPos; //���� ������ �� 

    //ȿ���� ���
    private AudioSource audioSource;

    private RaycastHit hitInfo;

    [Header("�ʿ��� ������Ʈ ����")]
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

    //����ӵ� ����
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

    void Fire() //�߻� ��
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

    void Shoot() //�߻� ��
    {
        theCrosshair.FireAnimation();
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; //���� �ӵ� ����
        PlaySE(currentGun.fire_sound);
        currentGun.muzleFlash.Play();
        
        Hit(); //��Ʈ��ĵ ���

        //�ѱ� �ݵ� �ڷ�ƾ
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

    IEnumerator RetroActionCoroutine() //�ݵ�
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionADSForce, currentGun.ADSOriginPos.y, currentGun.ADSOriginPos.z);

        if (!isADSMode)
        {
            currentGun.transform.localPosition = originPos;
            //�ݵ�����
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.2f);
                yield return null;
            }
            //����ġ
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.05f);
                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.ADSOriginPos;
            //�ݵ�����
            while (currentGun.transform.localPosition.x <= currentGun.retroActionADSForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.2f);
                yield return null;
            }
            //����ġ
            while (currentGun.transform.localPosition != currentGun.ADSOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.ADSOriginPos, 0.05f);
                yield return null;
            }
        }
    }

    IEnumerator ReloadCoroutine() //������ �ڷ�ƾ
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
            Debug.Log("�Ѿ� ������");
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
