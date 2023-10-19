using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public FindObjectInFov findObjectInFov;
    private Animator playerAnimator;
    public float move { get; private set; } // 감지된 움직임 입력값, 오토 프로퍼티
    public float rotate { get; private set; } // 감지된 회전 입력값
    public bool fire { get; private set; } // 감지된 발사 입력값
    public bool interact { get ; private set; } //상호작용 키 입력

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        //if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        //{
        //    move = Input.GetAxis("Vertical");
        //    // rotate에 관한 입력 감지
        //    rotate = Input.GetAxis("Horizontal");
        //}
        // move에 관한 입력 감지
        move = Input.GetAxis("Vertical");
        // rotate에 관한 입력 감지
        rotate = Input.GetAxis("Horizontal");

        interact = Input.GetKeyDown(KeyCode.E);

        fire = Input.GetButtonDown("Fire2");
    }
}
