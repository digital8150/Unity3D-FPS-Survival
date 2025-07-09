using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    [SerializeField]
    private int hp; //최대 체력
    private int currentHp;

    [SerializeField]
    private int sp; //최대 스태미너
    private int currentSp;
    [SerializeField]
    private int spIncreaseSpeed; //스태미너 증가량
    [SerializeField]
    private int spRechargeTime; //스태미너 회복 시작 지연 시간
    private int currentSpRechargeTime;
    private bool spUsed; //스태미너 감소 여부

    [SerializeField]
    private int dp;
    private int currentDp;

    [SerializeField]
    private int hungry;
    private int currentHungry;
    [SerializeField]
    private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    [SerializeField]
    private int thirsty;
    private int currentThirsty;
    [SerializeField]
    private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    [Header("필요한 UI(이미지) 연결")]
    [SerializeField]
    private Image[] images_Gauge;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        currentSp = sp;
        currentDp = dp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Hungry();
        Thirsty();
        SPRechargeTime();
        SPRecover();
    }

    private void LateUpdate()
    {
        GaugeUpdate();
    }

    void Hungry()
    {
        if (currentHungry > 0)
        {
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
            {
                currentHungryDecreaseTime++;
            }
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }
        }
        else
        {
            Debug.Log("배고픔 수치가 0 이 되었습니다.");
        }
    }

    void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
            {
                currentThirstyDecreaseTime++;
            }
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        else
        {
            Debug.Log("목마름 수치가 0 이 되었습니다.");
        }
    }

    void GaugeUpdate()
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp;
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    void SPRechargeTime()
    {
        if (spUsed)
        {
            if(currentSpRechargeTime < spRechargeTime)
            {
                currentSpRechargeTime++;
            }
            else
            {
                spUsed = false;
            }
        }
    }

    void SPRecover()
    {
        if (!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }

    public void DecreaseStamina(int _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        if(currentSp - _count > 0)
        {
            currentSp -= _count;
        }
        else
        {
            currentSp = 0;
        }
    }

    public void IncreaseHP(int _count)
    {
        if(currentHp + _count < hp)
        {
            currentHp += _count;
        }
        else
        {
            currentHp = hp;
        }
    }

    public void IncreaseSP(int _count)
    {
        if(currentSp + _count < sp)
        {
            currentSp += _count;
        }
        else
        {
            currentSp = sp;
        }
    }

    public void DecreaseHP(int _count)
    {
        if(currentDp > 0)
        {
            DecreaseDP(_count);
            return;
        }

        currentHp -= _count;

        if(currentHp <= 0)
        {
            Debug.Log("캐릭터의 HP가 0이 되었습니다.");
        }
    }

    public void IncreaseDP(int _count)
    {
        if (currentDp + _count < dp)
        {
            currentDp += _count;
        }
        else
        {
            currentDp = dp;
        }
    }

    public void DecreaseDP(int _count)
    {
        currentDp -= _count;

        if (currentDp <= 0)
        {
            currentDp = 0;
            Debug.Log("캐릭터의 DP가 0이 되었습니다.");
        }
    }

    public void IncreaseHungry(int _count)
    {
        if (currentHungry + _count < hungry)
        {
            currentHungry += _count;
        }
        else
        {
            currentHungry = hungry;
        }
    }

    public void DecreaseHungry(int _count)
    {
        currentHungry -= _count;

        if (currentHungry <= 0)
        {
            currentHungry = 0;
        }
    }

    public void IncreaseThristy(int _count)
    {
        if (currentThirsty + _count < thirsty)
        {
            currentThirsty += _count;
        }
        else
        {
            currentThirsty = thirsty;
        }
    }

    public void IncreaseSatisfy(int _count)
    {
        if(currentSatisfy + _count < satisfy)
        {
            currentSatisfy += _count;
        }
        else
        {
            currentSatisfy = satisfy;
        }
    }

    public void DecreaseThirsty(int _count)
    {
        currentThirsty -= _count;

        if (currentThirsty <= 0)
        {
            currentThirsty = 0;
        }
    }

    public int GetCurrentSP()
    {
        return currentSp;
    }
}
