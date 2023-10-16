using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;               // Unity-UI를 사용하기 위해 선언한 네임스페이스
using UnityEngine.Events;           // UnityEvent 관련 API(어플리케이션프로그래밍인터페이스, 미리만들어놓은 내장함수)를 사용하기 위해 선언한 네임스페이스
using UnityEngine.SceneManagement;

public class MainUIManager : MonoBehaviour
{
    // 버튼을 연결할 변수
    public Button startButton;
    public Button optionButton;
    public Button exitButton;
    public Button backButton;
    public Button stage1Button;


    public GameObject Main;
    public GameObject Stage;
    private UnityAction action;

    private void Start()
    {
        /*
        Acion 변수에 함수를 연결하는 람다식의 문법

        매개변수(인자, 인수, Argument)가 있을 때 
        델리게이트타입 변수명 = (매개변수1, 매개변수2 ...) => 식;
        델리게이트타입 변수명 = (매개변수1, 매개변수2 ...) => {로직1;, 로직2; ...};

        매개변수(인자, 인수, Argument)가 없을 때 
        델리게이트타입 변수명 = () => 식;
        델리게이트타입 변수명 = () => {로직1;, 로직2; ...};
        */

        // UnityAction을 사용한 이벤트 연결 방식
        action = () => OnStartClick();
        startButton.onClick.AddListener(action);

        // 무명메서드를 활용한 이벤트 연결 방식
        optionButton.onClick.AddListener(delegate { OnButtonClick(optionButton.name); });

        // 람다식을 활용한 이벤트 연결 방식
        exitButton.onClick.AddListener(() => OnButtonClick(exitButton.name));

        backButton.onClick.AddListener(() => OnButtonClick(backButton.name));

        stage1Button.onClick.AddListener(() => OnButtonClick(stage1Button.name));
    }

    public void OnButtonClick(string msg)
    {
        Debug.Log($"버튼 눌림 : {msg}");
    }

    public void OnStartClick()
    {
        Main.SetActive(false);
        Stage.SetActive(true);
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
    }

    public void OnBackClick()
    {
        Main.SetActive(true);
        Stage.SetActive(false);
    }

    public void OnStage1Click()
    {
        SceneManager.LoadScene("Develop");
    }
}
