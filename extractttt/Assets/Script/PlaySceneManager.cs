using UnityEngine;

public class PlaySceneManager : MonoBehaviour
{
    public StudyTimeSender sender;
    public string nickname = "selwjoo";

    void Start()
    {
        int finalStudySeconds = TimeDataManager.Instance.GetFinalStudySeconds();
        sender.SendStudyTime(nickname, finalStudySeconds);

        TimeDataManager.Instance.studyTime = 0;
        TimeDataManager.Instance.breakTime = 0;
    }
}
