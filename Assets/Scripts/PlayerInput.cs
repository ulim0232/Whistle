using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public FindObjectInFov findObjectInFov;
    public float move { get; private set; } // ������ ������ �Է°�, ���� ������Ƽ
    public float rotate { get; private set; } // ������ ȸ�� �Է°�
    public bool fire { get; private set; } // ������ �߻� �Է°�
    public bool interact { get ; private set; } //��ȣ�ۿ� Ű �Է�

    // Update is called once per frame
    void Update()
    {
        // move�� ���� �Է� ����
        move = Input.GetAxis("Vertical");
        // rotate�� ���� �Է� ����
        rotate = Input.GetAxis("Horizontal");

        interact = Input.GetKeyDown(KeyCode.E);
    }
}
