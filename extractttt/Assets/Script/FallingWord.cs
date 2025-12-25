using UnityEngine;
using UnityEngine.UI;

public class FallingWord : MonoBehaviour
{
    public WordData wordData;
    public float fallSpeed;
    private Text text;

    void Start()
    {
        text = GetComponentInChildren<Text>(); // ⭐ 핵심
        text.text = wordData.word;
    }
    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y < -5f)
        {
            GameManager.Instance.GameOver();
            Destroy(gameObject);
        }
    }

    public bool CheckAnswer(string userAnswer)
    {
        return userAnswer.Trim() == wordData.meaning;
    }
}
