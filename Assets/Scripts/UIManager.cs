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
                //FindObjectOfType �Լ��� ������� ����.
            }

            return m_instance;
        }
    }
    private static UIManager m_instance; // �̱����� �Ҵ�� ����

    public TextMeshProUGUI interactTxt;
    //public Slider dataProgress;
    public Slider healthBar;
    public GameObject gameClearUI; // ���� ������ Ȱ��ȭ�� UI 
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
        //Acion ������ �Լ��� �����ϴ� ���ٽ��� ����

        //�Ű�����(����, �μ�, Argument)�� ���� �� 
        //��������ƮŸ�� ������ = (�Ű�����1, �Ű�����2 ...) => ��;
        //��������ƮŸ�� ������ = (�Ű�����1, �Ű�����2 ...) => {����1;, ����2; ...};

        //�Ű�����(����, �μ�, Argument)�� ���� �� 
        //��������ƮŸ�� ������ = () => ��;
        //��������ƮŸ�� ������ = () => {����1;, ����2; ...};
        //*/

        //// UnityAction�� ����� �̺�Ʈ ���� ���
        //action = () => OnStartClick();
        //startButton.onClick.AddListener(action);

        //// ����޼��带 Ȱ���� �̺�Ʈ ���� ���
        //optionButton.onClick.AddListener(delegate { OnButtonClick(optionButton.name); });

        //// ���ٽ��� Ȱ���� �̺�Ʈ ���� ���
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

        // �ð��� �ؽ�Ʈ�� ǥ�� (��:��)
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
