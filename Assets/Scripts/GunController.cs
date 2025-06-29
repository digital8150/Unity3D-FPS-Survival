using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;

    private bool isReload = false;
    private bool isADSMode = false;

    [SerializeField]
    private Vector3 originPos; //���� ������ �� 

    private AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //originPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryADS();
    }

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

    void ADS()
    {
        isADSMode = !isADSMode;
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
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; //���� �ӵ� ����
        PlaySE(currentGun.fire_sound);
        currentGun.muzleFlash.Play();

        //�ѱ� �ݵ� �ڷ�ƾ
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
        Debug.Log("�Ѿ� �߻���");
    }

    IEnumerator RetroActionCoroutine()
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

    IEnumerator ReloadCoroutine()
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
}
