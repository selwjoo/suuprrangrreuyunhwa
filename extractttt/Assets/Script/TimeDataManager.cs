using UnityEngine;

public class TimeDataManager : MonoBehaviour
{
    public static TimeDataManager Instance;

    public float studyTime;   // √ 
    public float breakTime;   // √ 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetFinalStudySeconds()
    {
        float result = studyTime - breakTime;
        return Mathf.Max(0, Mathf.FloorToInt(result));
    }
}
