using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public WordScoreSender scoreSender;
    public string nickname = "selwjoo";

    [Header("게임 설정")]
    public GameObject wordPrefab; // 단어 Prefab
    public Canvas gameCanvas; // 메인 Canvas
    public float initialSpawnInterval = 1.8f; // 초기 생성 간격
    public float initialFallSpeed = 5f;
    public float speedIncreaseRate = 0.5f;


    [Header("UI")]
    public Text ScoreText;
    public Text speedText;
    public InputField answerInput;
    public GameObject gameOverPanel;

    private int score = 0;
    private float currentSpeed = 1f;
    private float spawnTimer = 0f;
    private float currentSpawnInterval;
    private bool isGameOver = false;

    private List<FallingWord> activeWords = new List<FallingWord>();
    private RectTransform canvasRect;

    // 단어 목록
    private WordData[] words = new WordData[]
    {
        new WordData { word = "apple", meaning = "사과" },
        new WordData { word = "book", meaning = "책" },
        new WordData { word = "happy", meaning = "행복한" },
        new WordData { word = "beautiful", meaning = "아름다운" },
        new WordData { word = "computer", meaning = "컴퓨터" },
        new WordData { word = "dream", meaning = "꿈" },
        new WordData { word = "friend", meaning = "친구" },
        new WordData { word = "house", meaning = "집" },
        new WordData { word = "love", meaning = "사랑" },
        new WordData { word = "music", meaning = "음악" },
        new WordData { word = "ocean", meaning = "바다" },
        new WordData { word = "peace", meaning = "평화" },
        new WordData { word = "quick", meaning = "빠른" },
        new WordData { word = "river", meaning = "강" },
        new WordData { word = "smile", meaning = "미소" },
        new WordData { word = "tree", meaning = "나무" },
        new WordData { word = "water", meaning = "물" },
        new WordData { word = "yellow", meaning = "노란색" },
        new WordData { word = "courage", meaning = "용기" },
        new WordData { word = "wisdom", meaning = "지혜" }
    };

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


        TimeDataManager.Instance.studyTime = 0;
        TimeDataManager.Instance.breakTime = 0;
    }

    void Start()
    {
        canvasRect = gameCanvas.GetComponent<RectTransform>();
        currentSpawnInterval = initialSpawnInterval;
        currentSpeed = 1f;
        gameOverPanel.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        if (isGameOver) return;

        // 단어 생성 타이머
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= currentSpawnInterval)
        {
            SpawnWord();
            spawnTimer = 0f;
        }

        // Enter 키로 답 제출
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckAnswer();
        }
    }

    void SpawnWord()
    {
        float randomX = Random.Range(-3f, 3f);
        float spawnY = 5f;

        GameObject wordObj = Instantiate(wordPrefab, gameCanvas.transform);
        wordObj.transform.position = new Vector3(randomX, spawnY, 0f);

        FallingWord fallingWord = wordObj.GetComponent<FallingWord>();
        fallingWord.wordData = words[Random.Range(0, words.Length)];
        fallingWord.fallSpeed = initialFallSpeed * currentSpeed;

        activeWords.Add(fallingWord);
    }


    public void CheckAnswer()
    {
        if (string.IsNullOrEmpty(answerInput.text)) return;

        string userAnswer = answerInput.text;

        // 모든 활성화된 단어 중에서 정답 찾기
        for (int i = activeWords.Count - 1; i >= 0; i--)
        {
            if (activeWords[i] == null)
            {
                activeWords.RemoveAt(i);
                continue;
            }

            if (activeWords[i].CheckAnswer(userAnswer))
            {
                // 정답!
                int points = Mathf.CeilToInt(10 * currentSpeed);
                score += points;
                ScoreText.text = " " + score;

                // 단어 제거
                Destroy(activeWords[i].gameObject);
                activeWords.RemoveAt(i);

                // 속도 증가
                currentSpeed += speedIncreaseRate;
                currentSpawnInterval = Mathf.Max(0.5f, initialSpawnInterval / currentSpeed);

                UpdateUI();
                answerInput.text = "";
                answerInput.ActivateInputField(); // 포커스 유지
                return;
            }
        }

        // 오답 처리
        answerInput.text = "";
        answerInput.ActivateInputField(); // 포커스 유지
    }

    void UpdateUI()
    {
        speedText.text = " " + currentSpeed.ToString("F1") + "x";
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        gameOverPanel.SetActive(true);

        // 모든 단어 제거
        foreach (var word in activeWords)
        {
            if (word != null)
                Destroy(word.gameObject);
        }
        activeWords.Clear();

        // 영어 점수 서버 전송
        if (scoreSender != null)
            scoreSender.SendWordScore(nickname, score);
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );

    }

    public void QuitGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Play");
    }
}