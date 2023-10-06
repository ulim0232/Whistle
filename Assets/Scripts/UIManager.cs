using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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

    public void SetActiveInteractUI(bool active)
    {
        interactTxt.gameObject.SetActive(active);
    }

    public void SetDataProgress(float value)
    {
        dataProgress.value = value;
    }
}
