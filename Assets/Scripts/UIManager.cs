using TMPro;
using UnityEngine;
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
    public Slider dataProgress;
    public GameObject gameClearUI; // ���� ������ Ȱ��ȭ�� UI 
    public GameObject controlUI;

    private void Start()
    {
        SetActivePorgressUI(false);
    }

    public void SetActiveInteractUI(bool active)
    {
        interactTxt.gameObject.SetActive(active);
    }

    public void SetActivePorgressUI(bool active)
    {
        dataProgress.gameObject.SetActive(active);
    }

    public void SetDataProgress(float value)
    {
        dataProgress.value = value;
    }

    public void SetActiveGameclearUI(bool active)
    {
        gameClearUI.SetActive(active);
    }

    public void SetActiveControlUI(bool active)
    {
        controlUI.SetActive(active);
    }
}
