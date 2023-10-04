using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �յ� �������� �ӵ�
    public float rotateSpeed = 180f; // �¿� ȸ�� �ӵ�
    public float jumpForce = 5f;

    private Vector3 direction;
    private bool isJumping = false;

    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private Rigidbody playerRigidbody; // �÷��̾� ĳ������ ������ٵ�
    private Animator playerAnimator; // �÷��̾� ĳ������ �ִϸ�����

    public LayerMask layerMask; // ��Ʈ�÷���ó�� ��� ����
    private Camera worldCam;

    private void Start()
    {
        // ����� ������Ʈ���� ������ ��������
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        //playerAnimator = GetComponent<Animator>();
        worldCam = Camera.main; //������ ���� ī�޶� �±װ� ���� ���ӿ�����Ʈ�� GetCompnent �ؼ� ������
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
        //playerAnimator.SetFloat("Move", direction.magnitude);

        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            Jump();
        }
    }

    // �Է°��� ���� ĳ���͸� �յڷ� ������
    private void Move()
    {
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
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}
