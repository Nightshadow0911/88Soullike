using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Archer_Animation : MonoBehaviour
{
    private Animator anim;

    private readonly int run = Animator.StringToHash("Run");
    private readonly int dodge = Animator.StringToHash("Dodge");
    private readonly int meleeAttack = Animator.StringToHash("MeleeAttack");
    private readonly int rangedAttack = Animator.StringToHash("RangedAttack");
    private readonly int backstepAttack = Animator.StringToHash("BackstepAttack");
    private readonly int trackingAttack = Animator.StringToHash("TrackingAttack");
    private readonly int tracking = Animator.StringToHash("Tracking");
    private readonly int snapshot = Animator.StringToHash("Snapshot");
    private readonly int stun = Animator.StringToHash("Stun");
    private readonly int stunning = Animator.StringToHash("Stunning");
    private readonly int death = Animator.StringToHash("Death");
    private readonly int cancel = Animator.StringToHash("Cancel");
    private readonly int flip = Animator.StringToHash("Flip");
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Running(Vector2 speed)
    {
        anim.SetFloat(run, speed.magnitude);
    }

    public void OnDodge()
    {
        anim.SetTrigger(dodge);
    }

    public void OnMeleeAttack()
    {
        anim.SetTrigger(meleeAttack);
    }

    public void OnRangedAttack()
    {
        anim.SetTrigger(rangedAttack);
    }

    public void OnBakcstepAttack()
    {
        anim.SetTrigger(backstepAttack);
    }
    
    public void OnTrakingAttack()
    {
        if (anim.GetBool(tracking))
            EndTracking();
        anim.SetTrigger(trackingAttack);
    }
    
    public void StartTracking()
    {
        anim.SetBool(tracking, true);
    }
    
    public void EndTracking()
    {
        anim.SetBool(tracking, false);
    }

    public void OnSnapshot(bool spriteFlip)
    {
        if (spriteFlip)
        {
            anim.SetBool(flip, true);
        }
        else
        {
            anim.SetBool(flip, false);

        }
        anim.SetTrigger(snapshot);
    }

    public void OnStun()
    {
        OnCancel();
        anim.SetTrigger(stun);
    }
    
    public void Stunning()
    {
        anim.SetBool(stunning, true);
    }
    
    public void EndStun()
    {
        OffCancel();
        anim.SetBool(stunning, false);
    }
    
    public void OnDeath()
    {
        anim.SetTrigger(death);
    }

    public void OnCancel()
    {
        anim.SetBool(cancel, true);
    }
    
    public void OffCancel()
    {
        anim.SetBool(cancel, false);
    }
}
