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
    private Vector3 direction;

    //���� ����
    private bool isAction; //�ൿ��
    private bool isWalking; //�ȴ� ��

    [Header("�ൿ �ð�")]
    [SerializeField]
    private float walkTime; //�ȱ�ð�
    [SerializeField]
    private float waitTime; //���ð�
    private float currentTime;

    [Header("�ʿ��� ������Ʈ")]
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rigid;
    [SerializeField]
    private BoxCollider boxCol;


    // Start is called before the first frame update
    void Start()
    {
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        ElapseTime();
    }

    private void FixedUpdate()
    {
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
        if (isWalking)
        {
            rigid.MovePosition(transform.position + transform.forward * walkSpeed * Time.deltaTime);
        }
    }

    void Rotation()
    {
        if (isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    void ReSet()
    {
        isWalking = false;
        isAction = true;
        anim.SetBool("Walking", isWalking);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }

    void RandomAction()
    {
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
        currentTime = walkTime;
        anim.SetBool("Walking", isWalking);
        Debug.Log("�ȱ�");

    }
}
