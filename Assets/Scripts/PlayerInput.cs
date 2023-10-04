using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float move { get; private set; } // 감지된 움직임 입력값, 오토 프로퍼티
    public float rotate { get; private set; } // 감지된 회전 입력값
    public bool fire { get; private set; } // 감지된 발사 입력값
    public bool reload { get; private set; } // 감지된 재장전 입력값

    // Update is called once per frame
    void Update()
    {
        // move에 관한 입력 감지
        move = Input.GetAxis("Vertical");
        // rotate에 관한 입력 감지
        rotate = Input.GetAxis("Horizontal");
        // fire에 관한 입력 감지
        fire = Input.GetButton("Fire1");
    }
}
