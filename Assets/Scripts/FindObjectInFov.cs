using UnityEngine;

public class FindObjectInFov : MonoBehaviour
{
    private Camera mainCamera;
    private float rayLength = 7f; // 레이의 길이 (조정 가능)  
    public LayerMask targetLayer;
    public LayerMask bookLayer;
    public GameObject hitObject { get; private set; }
    public GameObject bookObject { get; private set; }

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
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayLength, bookLayer))
            {
                bookObject = hit.transform.gameObject;
            }
            if (Physics.Raycast(ray, out hit, rayLength, targetLayer))
            {
                hitObject = hit.transform.gameObject;
            //    var hitMisiondata = hitObject.GetComponent<MissionData>();
            //    if (!hitMisiondata.isCapturing) 
            //    {
            //        if (UIManager.instance != null)
            //        {
            //            UIManager.instance.SetActiveInteractUI(true);
            //        }
            //        else
            //        {
            //            Debug.Log("uimanagernoindstance");
            //        }
            //    }
            //    else
            //    {
            //        UIManager.instance.SetActiveInteractUI(false);
            //    }
            //    //return hit.transform.gameObject;
            //    //Debug.Log(hitObject.name);
            }
            //else
            //{
            //    UIManager.instance.SetActiveInteractUI(false);
            //}
        }
    }
}
