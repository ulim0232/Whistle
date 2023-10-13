using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerInput playerInput;
    private Animator playerAnimator;

    public float damage = 10;

    private float lastAttackTime = 0;
    private float timeBetAttack = 1f;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(playerInput.fire)
        {
            //Debug.Log("fire");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(playerInput.fire)
        {
            if(other.gameObject.tag == "Enemy")
            {
                if (Time.time > lastAttackTime + timeBetAttack)
                {
                    lastAttackTime = Time.time;
                    other.gameObject.GetComponent<LivingEntity>().OnDamage(damage);
                    //playerAnimator.SetTrigger("Attack");
                }
            }
        }
    }
}
