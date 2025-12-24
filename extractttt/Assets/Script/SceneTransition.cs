using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    public SpriteRenderer circleSprite;
    public float transitionDuration = 0.5f; // 더 빠르게

    void Awake()
    {
        // 싱글톤 패턴
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

    void Start()
    {
        // 시작할 때는 원이 작게 (화면 보임)
        if (circleSprite != null)
        {
            circleSprite.transform.localScale = Vector3.zero;
            circleSprite.enabled = false; // 평소엔 안 보이게
        }
    }

    public void LoadSceneWithTransition(string sceneName)
    {
        StartCoroutine(TransitionCoroutine(sceneName));
    }

    IEnumerator TransitionCoroutine(string sceneName)
    {
        // 스프라이트 활성화
        circleSprite.enabled = true;

        // 원이 완전히 커질 때까지 대기 (점점 빨라지게)
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;
            // EaseIn 효과 (세제곱으로 점점 빨라짐)
            float easedT = t * t * t;
            float scale = Mathf.Lerp(0f, 12f, easedT); // 0에서 1(원본 크기)로
            circleSprite.transform.localScale = Vector3.one * scale;
            yield return null;
        }

        // 완전히 커진 상태로 고정 (원본 크기)
        circleSprite.transform.localScale = Vector3.one;

        // 씬 전환
        SceneManager.LoadScene(sceneName);

        // 새 씬에서 원이 작아지면서 화면 나타나기 (점점 느려지게)
        elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;
            // EaseOut 효과
            float easedT = 1f - (1f - t) * (1f - t) * (1f - t);
            float scale = Mathf.Lerp(12f, 0f, easedT); // 1(원본)에서 0으로
            circleSprite.transform.localScale = Vector3.one * scale;
            yield return null;
        }

        circleSprite.transform.localScale = Vector3.zero;
        circleSprite.enabled = false; // 다시 안 보이게
    }
}