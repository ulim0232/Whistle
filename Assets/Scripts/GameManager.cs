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

    private int score = 0; // ���� ���� ����
    public bool isGameover { get; private set; } // ���� ���� ����

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

    }

    private void Update()
    {
        if (score >= 4)
        {
            EndGame();
        }

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
    public void EndGame()
    {
        // ���� ���� ���¸� ������ ����
        isGameover = true;
        UIManager.instance.SetActiveGameclearUI(true);
    }

    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}