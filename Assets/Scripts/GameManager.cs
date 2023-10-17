using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// 점수와 게임 오버 여부를 관리하는 게임 매니저
public class GameManager : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

    public int score { get; private set; } // 현재 게임 점수
    public bool isGameover { get; private set; } // 게임 오버 상태
    public float gameOverTime { get; private set; }
    public float timer = 0;

    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FindObjectOfType<PlayerHealth>().onDeath += EndGame;
        score = 0;
        Cursor.visible = false;
        gameOverTime = 300f;
        timer = gameOverTime;
    }

    private void Update()
    {
        //if (score >= 4)
        //{
        //    ClearGame();
        //}

        if(!isGameover)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                UIManager.instance.SetActiveControlUI(true);
            }

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                UIManager.instance.SetActiveControlUI(false);
            }
            timer -= Time.deltaTime;
            UIManager.instance.SetTimerTxt(timer);

            if(timer < 0)
            {
                EndGame();
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddScore(6);
        }
    }

    // 점수를 추가하고 UI 갱신
    public void AddScore(int newScore)
    {
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (!isGameover)
        {
            // 점수 추가
            score += newScore;
        }
        Debug.Log(score);
    }

    // 게임 오버 처리
    public void ClearGame()
    {
        isGameover = true;
        UIManager.instance.SetActiveGameclearUI(true);
    }

    public void EndGame()
    {
        isGameover = true;
        UIManager.instance.SetAcitveGameOverUI(true);
    }

    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}