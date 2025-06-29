using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    //ÃÑÀÇ Á¤È®µµ
    private float gunAccuracy;

    //control crosshair when its not need to be shown
    [SerializeField]
    private GameObject go_crosshairHUD;
    [SerializeField]
    private GunController theGunController;

    public void WalkingAnimation(bool _flag)
    {
        animator.SetBool("Walking", _flag);
    }

    public void RunningAnimation(bool _flag)
    {
        animator.SetBool("Running", _flag);
    }

    public void CrouchingAnimation(bool _flag)
    {
        animator.SetBool("Crouching", _flag);
    }
    
    public void ADSAnimation(bool _flag)
    {
        animator.SetBool("ADS", _flag);
    }

    public void FireAnimation()
    {
        if (animator.GetBool("Walking"))
        {
            animator.SetTrigger("Walk_Fire");
        }
        else if (animator.GetBool("Crouching"))
        {
            animator.SetTrigger("Crouch_Fire");
        }
        else
        {
            animator.SetTrigger("idle_Fire");
        }
    }

    public float GetAccuracy()
    {
        if (theGunController.GetADSMode())
        {
            gunAccuracy = 0.001f;
        }
        else if (animator.GetBool("Walking"))
        {
            gunAccuracy = 0.08f;
        }
        else if (animator.GetBool("Crouching"))
        {
            gunAccuracy = 0.02f;
        }
        else
        {
            gunAccuracy = 0.04f;
        }

        return gunAccuracy;
    }

}
