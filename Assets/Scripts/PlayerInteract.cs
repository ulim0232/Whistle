using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public MissionData interactObj;
    private PlayerInput playerinput;
    private FindObjectInFov findObjectInFov;
    public GameObject bookObj;

    private void Start()
    {
        playerinput = GetComponent<PlayerInput>();
        findObjectInFov = GetComponent<FindObjectInFov>();
    }


    private void Update()
    {
        if (playerinput.move != 0 || playerinput.rotate != 0) //�̵��ϸ� �Ͻ� ����
        {
            if (interactObj != null && interactObj.isCapturing)
            {
                interactObj.PauseCapture();
            }
        }
        if (playerinput.interact)
        {
            if (findObjectInFov.hitObject != null)
            {
                interactObj = findObjectInFov.hitObject.GetComponent<MissionData>();
                CollectionData();
            }
            else if (findObjectInFov.bookObject!=null)
            {
                bookObj = findObjectInFov.bookObject;
                GetBookData();
            }
            else
            {
                Debug.Log("hitObject null");
                return;
            }
        }
                
            //else // ��ȣ �ۿ� ������ ������Ʈ�� �������� ������ �ʾ���
            //{
            //    Debug.Log("hitObject null");
            //    return;
            //}
            //if(interactObj.isCapturing || interactObj.isCaptured) //ĸ�� ���� ������Ʈ�� ��ȣ�ۿ� ��õ� or �̹� ���� �Ϸ�Ȼ��� -> �ƹ��ϵ� ���� ����
            //{
            //    return;
            //}
            //if (interactObj != null)
            //{
            //    if(interactObj.isPaused)
            //    {
            //        interactObj.ResumeCapture();
            //    }
            //    else
            //    {
            //        interactObj.StartCapture();
            //    }
            //}
            //else
            //{
            //    Debug.Log("don't exist obj");
            //}
    

        if(interactObj != null)
        {
            if (interactObj.gauge == 100)
            {
                interactObj = null;
            }
        }
       
    }

    public void CollectionData()
    {
        if (interactObj.isCapturing || interactObj.isCaptured) //ĸ�� ���� ������Ʈ�� ��ȣ�ۿ� ��õ� or �̹� ���� �Ϸ�Ȼ��� -> �ƹ��ϵ� ���� ����
        {
            return;
        }
        if (interactObj != null)
        {
            if (interactObj.isPaused)
            {
                interactObj.ResumeCapture();
            }
            else
            {
                interactObj.StartCapture();
            }
        }
        else
        {
            Debug.Log("don't exist obj");
        }
    }

    public void GetBookData()
    {
        GameManager.instance.AddScore(1);
        bookObj.SetActive(false);
    }
}
