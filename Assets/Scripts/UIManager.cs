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
                //FindObjectOfType 함수는 사용하지 말것.
            }

            return m_instance;
        }
    }
    private static UIManager m_instance; // 싱글톤이 할당될 변수

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
