using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage;
    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    private LayerMask whatIsDamageable;
    public bool gotImput, isAttacking, isFirstAttack;

    private float lastInputTime = Mathf.NegativeInfinity;

    private Animator anim;

    public PlayerController playerController;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
    }


    private void Update()
    {
        // CheckCombatInput();
        // Attack1();
        CheckAttacks();
    }

    public void Attack1()
    {
        if (combatEnabled)
        {
            // Attempt combat
            gotImput = true;
            lastInputTime = Time.time;
        }
    }
    // private void CheckCombatInput()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         if (combatEnabled)
    //         {
    //             // Attempt combat
    //             gotImput = true;
    //             lastInputTime = Time.time;
    //         }
    //     }
    // }

    private void CheckAttacks()
    {
        if (gotImput)
        {
            // Attack 1
            if (!isAttacking)
            {
                gotImput = false;
                isAttacking  = true;
                isFirstAttack = !isFirstAttack;
                anim.SetBool("attack1", true);
                anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("isAttacking", isAttacking);
            }
        }

        if (Time.time >= lastInputTime + inputTimer)
        {
            // Wait 
            gotImput = false;
        }
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);

        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", attack1Damage);
        }
    }

    private void FinishAttack1()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }
}