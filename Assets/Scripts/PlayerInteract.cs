using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private PlayerInput playerinput;

    private FindObjectInFov findObjectInFov;

    public MissionData interactObj;
    public GameObject bookObj;
    public GameObject keyObj;
    public GameObject doorObj;

    private bool hasKey = false;

    private void Start()
    {
        playerinput = GetComponent<PlayerInput>();
        findObjectInFov = GetComponent<FindObjectInFov>();
    }


    private void Update()
    {
        if (playerinput.move != 0 || playerinput.rotate != 0) //이동하면 일시 정지
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
            else if (findObjectInFov.bookObject != null)
            {
                bookObj = findObjectInFov.bookObject;
                GetBookData();
            }
            else if(findObjectInFov.keyObject != null)
            {
                keyObj = findObjectInFov.keyObject;
                GetKey();
            }
            else if(findObjectInFov.doorObject != null)
            {
                doorObj = findObjectInFov.doorObject;
                if(hasKey)
                {
                    DoorOpen();
                }
                else
                {
                    UIManager.instance.AcitveNeedKey();
                }

            }
            else
            {
                Debug.Log("hitObject null");
                return;
            }

        }
    

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
        if (interactObj.isCapturing || interactObj.isCaptured) //캡쳐 중인 오브젝트에 상호작용 재시도 or 이미 수집 완료된상태 -> 아무일도 하지 않음
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

    public void GetKey()
    {
        keyObj.SetActive(false);
        hasKey = true;
    }

    public void DoorOpen()
    {
        doorObj.GetComponent<Door>().ActionDoor();
    }
}
