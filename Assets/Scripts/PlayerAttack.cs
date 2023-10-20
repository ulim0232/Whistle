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
    private float timeBetAttack = 2f;

    public AudioSource playerAudio;
    public AudioClip playerAttackClip;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(playerInput.fire)
        {
            if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                playerAnimator.SetTrigger("Attack");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(playerInput.fire)
        {
           
            if (other.gameObject.tag == "Enemy")
            {
                if (Time.time > lastAttackTime + timeBetAttack)
                {
                    lastAttackTime = Time.time;
                    other.gameObject.GetComponent<LivingEntity>().OnDamage(damage);
                    playerAudio.PlayOneShot(playerAttackClip);
                    
                }
            }
        }
    }
}
