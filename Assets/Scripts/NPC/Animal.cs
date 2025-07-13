using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour
{
    [Header("기본 정보")]
    [SerializeField]
    protected string animalName;
    [SerializeField]
    protected int hp;
    [SerializeField]
    protected float walkSpeed;
    [SerializeField]
    protected float runSpeed;
    protected float applySpeed;
    [SerializeField]
    protected float turningSpeed;
    protected Vector3 direction;

    //상태 변수
    protected bool isAction; //행동중
    protected bool isWalking; //걷는 중
    protected bool isRunning;
    protected bool isDead;

    [Header("행동 시간")]
    [SerializeField]
    protected float walkTime; //걷기시간
    [SerializeField]
    protected float waitTime; //대기시간
    [SerializeField]
    protected float runTime;
    protected float currentTime;

    [Header("필요한 컴포넌트")]
    [SerializeField]
    protected Animator anim;
    [SerializeField]
    protected Rigidbody rigid;
    [SerializeField]
    protected BoxCollider boxCol;
    [SerializeField]
    protected AudioClip[] sound_normal;
    [SerializeField]
    protected AudioClip sound_hurt;
    [SerializeField]
    protected AudioClip sound_dead;
    protected AudioSource audioSource;



    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        ElapseTime();
    }

    protected void FixedUpdate()
    {
        if (isDead) return;

        Move();
        Rotation();
    }

    void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0)
            {
                //다음 랜덤 행동 게시
                ReSet();
            }
        }
    }

    protected void Move()
    {
        if (isWalking || isRunning)
        {
            rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
        }
    }

    protected void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), turningSpeed);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    protected virtual void ReSet()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;
        applySpeed = walkSpeed;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
    }

    protected void TryWalk()
    {
        isWalking = true;
        applySpeed = walkSpeed;
        currentTime = walkTime;
        anim.SetBool("Walking", isWalking);
        Debug.Log("걷기");

    }

    void Dead()
    {
        PlaySE(sound_dead);
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");

    }

    public virtual void Damage(int _dmg, Vector3 _targetPos)
    {
        if (isDead) return;

        hp -= _dmg;
        if (hp <= 0)
        {
            Dead();
            return;
        }

        PlaySE(sound_hurt);
        anim.SetTrigger("Hurt");
    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, 3);
        PlaySE(sound_normal[_random]);
    }

    void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
