using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �յ� �������� �ӵ�
    public float rotateSpeed = 360f; // �¿� ȸ�� �ӵ�
    public float jumpForce = 5f;
    public float runSpeed = 6;
    public float walkSpeed = 3;

    private Vector3 direction;
    private bool isJumping = false;

    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private PlayerInteract playerInteract;
    private Rigidbody playerRigidbody; // �÷��̾� ĳ������ ������ٵ�
    private Animator playerAnimator; // �÷��̾� ĳ������ �ִϸ�����

    public LayerMask layerMask; // ��Ʈ�÷���ó�� ��� ����
    private Camera worldCam;

    private void Start()
    {
        // ����� ������Ʈ���� ������ ��������
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerInteract = GetComponent<PlayerInteract>();
        //playerAnimator = GetComponent<Animator>();
        worldCam = Camera.main; //������ ���� ī�޶� �±װ� ���� ���ӿ�����Ʈ�� GetCompnent �ؼ� ������
        playerAnimator = GetComponent<Animator>();
    }

    // FixedUpdate�� ���� ���� �ֱ⿡ ���� �����
    private void FixedUpdate()
    {
        // ���� ���� �ֱ⸶�� ������, ȸ��, �ִϸ��̼� ó�� ����
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

        if (direction.magnitude > 1f) //Ű 2�� ���� �Է� �� 1 �̻� => �밢�� �̵��� �� ������ => ����ȭ�� ����
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

    // �Է°��� ���� ĳ���͸� �յڷ� ������
    private void Move()
    {
        //if (playerInteract.interactObj != null) //�̵��ϸ� ĸ�� �Ͻ�����
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

    // �Է°��� ���� ĳ���͸� �¿�� ȸ��
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
