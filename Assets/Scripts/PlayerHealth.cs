using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private Animator playerAnimator; // 플레이어의 애니메이터

    private PlayerMovement playerMovement; // 플레이어 움직임 컴포넌트
    public float playerStartingHealth = 30f;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    protected override void OnEnable()
    {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();

        //죽은 후 재시작하면 false 일 수 있기 때문에 true로 세팅해줌
        playerMovement.enabled = true;
        health = playerStartingHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            OnDamage(100);
        }
    }

    //// 체력 회복
    //public override void RestoreHealth(float newHealth)
    //{
    //    // LivingEntity의 RestoreHealth() 실행 (체력 증가)
    //    base.RestoreHealth(newHealth);

    //    healthSlider.value = health;
    //}

    // 데미지 처리
    public override void OnDamage(float damage)
    {
        // LivingEntity의 OnDamage() 실행(데미지 적용)
        base.OnDamage(damage);
        Debug.Log(health);
        UIManager.instance.SetHeathBar(health);
    }

    // 사망 처리
    public override void Die()
    {
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();
        playerAnimator.SetTrigger("Die");
        playerMovement.enabled = false;
        playerAnimator.SetTrigger("Die");

    }
}
