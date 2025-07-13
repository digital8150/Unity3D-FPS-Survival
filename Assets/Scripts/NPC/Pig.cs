using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [Header("�⺻ ����")]
    [SerializeField]
    private string animalName;
    [SerializeField]
    private int hp;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;
    private Vector3 direction;

    //���� ����
    private bool isAction; //�ൿ��
    private bool isWalking; //�ȴ� ��
    private bool isRunning;
    private bool isDead;

    [Header("�ൿ �ð�")]
    [SerializeField]
    private float walkTime; //�ȱ�ð�
    [SerializeField]
    private float waitTime; //���ð�
    [SerializeField]
    private float runTime;
    private float currentTime;

    [Header("�ʿ��� ������Ʈ")]
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rigid;
    [SerializeField]
    private BoxCollider boxCol;
    [SerializeField]
    private AudioClip[] sound_pig_normal;
    [SerializeField]
    private AudioClip sound_pig_hurt;
    [SerializeField]
    private AudioClip sound_pig_dead;
    private AudioSource audioSource;



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

    private void FixedUpdate()
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
                //���� ���� �ൿ �Խ�
                ReSet();
            }
        }
    }

    void Move()
    {
        if (isWalking || isRunning)
        {
            rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
        }
    }

    void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    void ReSet()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;
        applySpeed = walkSpeed;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }

    void RandomAction()
    {
        RandomSound();
        isAction = true;

        int _random = Random.Range(0, 4);

        if(_random == 0)
        {
            Wait();
        }
        else if (_random == 1)
        {
            Eat();
        }
        else if( _random == 2)
        {
            Peek();
        }
        else if(_random == 3)
        {
            TryWalk();
        }

    }

    void Wait()
    {
        currentTime = waitTime;
        Debug.Log("���");

    }
    void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("Ǯ���");
    }
    void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("�θ���");

    }
    void TryWalk()
    {
        isWalking = true;
        applySpeed = walkSpeed;
        currentTime = walkTime;
        anim.SetBool("Walking", isWalking);
        Debug.Log("�ȱ�");

    }
    public void Run(Vector3 _targetPos)
    {
        direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;
        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;
        anim.SetBool("Running", isRunning);
    }

    void Dead()
    {
        PlaySE(sound_pig_dead);
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");

    }

    public void Damage(int _dmg, Vector3 _targetPos)
    {
        if (isDead) return;

        hp -= _dmg;
        if (hp <= 0)
        {
            Dead();
            return;
        }

        PlaySE(sound_pig_hurt);
        anim.SetTrigger("Hurt");
        Run(_targetPos);
    }

    void RandomSound()
    {
        int _random = Random.Range(0, 3);
        PlaySE(sound_pig_normal[_random]);
    }

    void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
