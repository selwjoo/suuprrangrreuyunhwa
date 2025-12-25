using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StudyTimer : MonoBehaviour
{
    private float currentTime = 0f;   // 누적 공부 시간 (초)

    public Text timerText;
    private bool isRunning = false;
    public int finalSeconds = 0;
    public string nickname = "selwjoo";

    void Start()
    {
        isRunning = true;
        UpdateTimerText();
    }

    void Update()
    {
        if (!isRunning) return;

        // 🔥 증가형 타이머
        currentTime += Time.deltaTime;
        UpdateTimerText();
    }

    public void StopTimer()
    {
        isRunning = false;

        // 현재 씬에 따라 공부시간 또는 쉬는시간 저장
        if (SceneManager.GetActiveScene().name == "Timer 1")
        {
            TimeDataManager.Instance.studyTime = currentTime;
        }
        else if (SceneManager.GetActiveScene().name == "Timer")
        {
            TimeDataManager.Instance.breakTime = currentTime;
        }

        // 최종 공부시간 계산 (쉬는시간 차감)
        finalSeconds = TimeDataManager.Instance.GetFinalStudySeconds();
        Debug.Log("최종 공부시간 (서버로 전송): " + finalSeconds);

        // 서버로 전송
        StudyTimeSender sender = FindAnyObjectByType<StudyTimeSender>();
        if (sender != null)
        {
            sender.SendStudyTime(nickname, finalSeconds);
        }
        else
        {
            Debug.LogError("StudyTimeSender를 찾을 수 없습니다!");
        }
        Debug.Log("currentTime: " + currentTime);
        Debug.Log("TimeDataManager.studyTime: " + TimeDataManager.Instance.studyTime);
        Debug.Log("TimeDataManager.breakTime: " + TimeDataManager.Instance.breakTime);
        Debug.Log("GetFinalStudySeconds(): " + TimeDataManager.Instance.GetFinalStudySeconds());
        // 씬 전환
        SceneTransition.Instance.LoadSceneWithTransition("Play");


    }


    void UpdateTimerText()
    {
        int totalSeconds = Mathf.FloorToInt(currentTime);

        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        if (hours > 0)
        {
            // HH:MM:SS
            timerText.text =
                hours.ToString("00") + ":" +
                minutes.ToString("00") + ":" +
                seconds.ToString("00");
        }
        else
        {
            // MM:SS
            timerText.text =
                minutes.ToString("00") + ":" +
                seconds.ToString("00");
        }
    }
}
