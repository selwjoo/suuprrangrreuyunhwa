using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WordScoreUI : MonoBehaviour
{
    [System.Serializable]
    class ServerResponse
    {
        public string nickname;
        public int total_score;
    }

    public Text bestScoreText;
    public string nickname = "selwjoo";
    void Start()
    {
        StartCoroutine(GetBestScore());
    }

    public IEnumerator GetBestScore()
    {
        string url = "http://127.0.0.1:8000/scores/get_word_score/?nickname=" + nickname;
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);
            bestScoreText.text = "BestScore: " + response.total_score.ToString();
        }
        else
        {
            Debug.LogError("BestScore 불러오기 실패: " + request.error);
        }
    }
}
