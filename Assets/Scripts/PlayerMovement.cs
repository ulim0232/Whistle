using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 360f; // 좌우 회전 속도
    public float jumpForce = 5f;
    public float runSpeed = 6;
    public float walkSpeed = 3;

    private Vector3 direction;
    private bool isJumping = false;

    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private PlayerInteract playerInteract;
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

    public LayerMask layerMask; // 비트플래그처럼 사용 가능
    private Camera worldCam;

    private void Start()
    {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerInteract = GetComponent<PlayerInteract>();
        //playerAnimator = GetComponent<Animator>();
        worldCam = Camera.main; //씬에서 메인 카메라 태그가 붙은 게임오브젝트를 GetCompnent 해서 리턴함
        playerAnimator = GetComponent<Animator>();
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate()
    {
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행
        Move();
        Rotate();
    }

    private void Update()
    {
        if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            return;
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = runSpeed;
            //playerAnimator.SetFloat("Speed", moveSpeed);
        }
        else
        {
            moveSpeed = walkSpeed;
        }

        playerAnimator.SetFloat("Speed", moveSpeed);

        var forward = worldCam.transform.forward;
        forward.y = 0f;
        forward.Normalize();

        var right = worldCam.transform.right;
        right.y = 0f;
        right.Normalize();

        direction = forward * playerInput.move;
        direction += worldCam.transform.right * playerInput.rotate;

        if (direction.magnitude > 1f) //키 2개 동시 입력 시 1 이상 => 대각선 이동이 더 빨라짐 => 정규화로 보정
        {
            direction.Normalize();
        }
        playerAnimator.SetFloat("Move", direction.magnitude);

        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            if (isJumping)
            {
                return;
            }
            Jump();
        }
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move()
    {
        //if (playerInteract.interactObj != null) //이동하면 캡쳐 일시정지
        //{
        //    if(playerInteract.interactObj.isCapturing)
        //    {
        //        playerInteract.interactObj.PauseCapture();
        //    }
        //}
        var position = playerRigidbody.position;

        position += direction * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(position);
    }

    // 입력값에 따라 캐릭터를 좌우로 회전
    private void Rotate()
    {
        if(direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    private void Jump()
    {
        isJumping = true;
        
        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        if (!playerAnimator.GetBool("Jump"))
        {
            playerAnimator.SetBool("Jump", true);
            playerAnimator.SetTrigger("Jump1");
        }   
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            playerAnimator.SetBool("Jump", false);
        }
    }
}
