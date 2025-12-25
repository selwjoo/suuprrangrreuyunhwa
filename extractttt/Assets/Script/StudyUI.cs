using UnityEngine;
using UnityEngine.UI;
public class StudyUI : MonoBehaviour
{
    public Text studyTimeText;

    // 서버에서 받은 총 누적 초 단위 공부시간을 UI에 표시
    public void UpdateStudyTimeUI(int totalSeconds)
    {
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        if (hours > 0)
        {
            // 1시간 이상 → H M
            studyTimeText.text = string.Format("{0}H {1}M", hours, minutes);
        }
        else
        {
            // 1시간 미만 → M s
            studyTimeText.text = string.Format("{0}M {1}s", minutes, seconds);
        }
    }
}
