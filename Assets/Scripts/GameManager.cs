using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// ������ ���� ���� ���θ� �����ϴ� ���� �Ŵ���
public class GameManager : MonoBehaviour
{
    // �̱��� ���ٿ� ������Ƽ
    public static GameManager instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<GameManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    private static GameManager m_instance; // �̱����� �Ҵ�� static ����

    public int score { get; private set; } // ���� ���� ����
    public bool isGameover { get; private set; } // ���� ���� ����
    public float gameOverTime { get; private set; }
    public float timer = 0;

    private void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        if (instance != this)
        {
            // �ڽ��� �ı�
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

    // ������ �߰��ϰ� UI ����
    public void AddScore(int newScore)
    {
        // ���� ������ �ƴ� ���¿����� ���� ���� ����
        if (!isGameover)
        {
            // ���� �߰�
            score += newScore;
        }
        Debug.Log(score);
    }

    // ���� ���� ó��
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