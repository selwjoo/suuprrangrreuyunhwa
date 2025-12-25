using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class StudyTimeSender : MonoBehaviour
{
    public StudyUI studyUI; // Inspector에서 연결

    [System.Serializable]
    class StudyTimeData
    {
        public string nickname;
        public int study_seconds;
    }
    [System.Serializable]
    class ServerResponse
    {
        public string message;
        public int total_seconds;  // 누적 공부시간
    }
    public void SendStudyTime(string nickname, int studySeconds)
    {
        Debug.Log("🔥 SendStudyTime 호출됨");
        StartCoroutine(PostStudyTime(nickname, studySeconds));
    }

    IEnumerator PostStudyTime(string nickname, int studySeconds)
    {
        StudyTimeData data = new StudyTimeData
        {
            nickname = nickname,
            study_seconds = studySeconds
        };

        string json = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(
            "http://127.0.0.1:8000/scores/submit_study_time/",
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
            if (studyUI != null)
            {
                studyUI.UpdateStudyTimeUI(response.total_seconds);
            }
        }
        else
        {
            Debug.LogError("공부시간 전송 실패: " + request.error);
        }
    }
}
