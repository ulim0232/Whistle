using UnityEngine;

public class FindObjectInFov : MonoBehaviour
{
    public string doorTag = "LockDoor";
    private Camera mainCamera;
    private float rayLength = 6f; // 레이의 길이 (조정 가능)  
    public LayerMask targetLayer;
    public LayerMask bookLayer;
    public LayerMask keyLayer;
    public LayerMask doorLayer;
    public GameObject hitObject { get; private set; }
    public GameObject bookObject { get; private set; }
    public GameObject keyObject { get; private set; }
    public GameObject doorObject { get; private set; }

    void Start()
    {
        mainCamera = Camera.main; // 메인 카메라를 가져옴
    }

    void Update()
    {
        FindObject();
    }

    public void FindObject()
    {
        if (mainCamera != null)
        {
            hitObject = null;
            bookObject = null;
            keyObject = null;
            doorObject = null;

            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayLength, bookLayer))
            {
                bookObject = hit.transform.gameObject;
            }
            if (Physics.Raycast(ray, out hit, rayLength, targetLayer))
            {
                hitObject = hit.transform.gameObject;
            }
            if (Physics.Raycast(ray, out hit, rayLength, keyLayer))
            {
                keyObject = hit.transform.gameObject;
                Debug.Log("key");
            }
            if (Physics.Raycast(ray, out hit, rayLength, doorLayer))
            {
                doorObject = hit.transform.gameObject;
                Debug.Log("LockDoor");
            }
            //if(Physics.Raycast(ray, out hit, rayLength))
            //{
            //    Debug.Log(hit.transform.tag);
            //    if(hit.transform.tag == doorTag)
            //    {
            //        Debug.Log("LockDoor");
            //    }
            //}
        }
    }
}
