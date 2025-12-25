using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WordScoreSender : MonoBehaviour
{
    public WordScoreUI scoreUI;

    [System.Serializable]
    class WordScoreData
    {
        public string nickname;
        public int score;
    }

    [System.Serializable]
    class ServerResponse
    {
        public string message;
        public int total_score;
    }

    public Text wordScoreText; // UI 연결

    public void SendWordScore(string nickname, int score)
    {
        Debug.Log($"SendWordScore 호출됨: {nickname}, {score}");
        StartCoroutine(PostWordScore(nickname, score));
    }

    IEnumerator PostWordScore(string nickname, int score)
    {
        WordScoreData data = new WordScoreData { nickname = nickname, score = score };
        string json = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(
            "http://127.0.0.1:8000/scores/submit_word_score/",
            "POST"
        );
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);
            if (wordScoreText != null)
            {
                wordScoreText.text = response.total_score.ToString();
                scoreUI.StartCoroutine(scoreUI.GetBestScore());
            }
                
        }
        else
        {
            Debug.LogError("영어 점수 전송 실패: " + request.error);
        }
    }
}
