using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
                //FindObjectOfType 함수는 사용하지 말것.
            }

            return m_instance;
        }
    }
    private static UIManager m_instance; // 싱글톤이 할당될 변수

    public TextMeshProUGUI interactTxt;
    //public Slider dataProgress;
    public Slider healthBar;
    public GameObject gameClearUI; // 게임 오버시 활성화할 UI 
    public GameObject controlUI;
    public GameObject gameOverUI;
    public TextMeshProUGUI TimeTxt;
    public Michsky.MUIP.ProgressBar dataBar; // ImgsFD;
    public Michsky.MUIP.NotificationManager notification;
    public Michsky.MUIP.NotificationManager npcNotification;
    public GameObject menuList;
    //public Michsky.MUIP.RangeSlider healthBar;


    //private void Start()
    //{
    //    SetActivePorgressUI(false);
    //}

    private void Start()
    {
        SetActivePorgressUI(false);
        ///*
        //Acion 변수에 함수를 연결하는 람다식의 문법

        //매개변수(인자, 인수, Argument)가 있을 때 
        //델리게이트타입 변수명 = (매개변수1, 매개변수2 ...) => 식;
        //델리게이트타입 변수명 = (매개변수1, 매개변수2 ...) => {로직1;, 로직2; ...};

        //매개변수(인자, 인수, Argument)가 없을 때 
        //델리게이트타입 변수명 = () => 식;
        //델리게이트타입 변수명 = () => {로직1;, 로직2; ...};
        //*/

        //// UnityAction을 사용한 이벤트 연결 방식
        //action = () => OnStartClick();
        //startButton.onClick.AddListener(action);

        //// 무명메서드를 활용한 이벤트 연결 방식
        //optionButton.onClick.AddListener(delegate { OnButtonClick(optionButton.name); });

        //// 람다식을 활용한 이벤트 연결 방식
        //exitButton.onClick.AddListener(() => OnButtonClick(exitButton.name));

        //backButton.onClick.AddListener(() => OnButtonClick(backButton.name));

        //stage1Button.onClick.AddListener(() => OnButtonClick(stage1Button.name));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            AcitveNeedKey();
        }
    }

    public void SetActiveInteractUI(bool active)
    {
        interactTxt.gameObject.SetActive(active);
    }

    public void SetActivePorgressUI(bool active)
    {
        //dataProgress.gameObject.SetActive(active);
        dataBar.gameObject.SetActive(active);
    }

    public void SetDataProgress(float value)
    {
        //dataProgress.value = value;
        dataBar.ChangeValue(value);
        //dataBar.SetValue(value/100, true);
    }

    public void SetHeathBar(float value)
    {
        healthBar.value = value;
    }

    public void SetActiveGameclearUI(bool active)
    {
        gameClearUI.SetActive(active);
    }

    public void SetActiveControlUI(bool active)
    {
        controlUI.SetActive(active);
    }

    public void SetAcitveGameOverUI(bool active)
    {
        gameOverUI.SetActive(active);
    }

    public void SetTimerTxt(float text)
    {
        int minutes = Mathf.FloorToInt(text / 60);
        int seconds = Mathf.FloorToInt(text % 60);

        // 시간을 텍스트로 표시 (분:초)
        TimeTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void TestONClick()
    {
        Debug.Log("click");
    }

    public void AcitveNeedKey()
    {
        notification.Open();
    }

    public void SetActiveNPC()
    {
        npcNotification.Open();
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene("MainTitleUI");
    }

    public void SetActiveMenuList(bool isActive)
    {
        menuList.SetActive(isActive);
    }
}
