using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    public float speed = 5f;

    private Queue<Vector3> pathPoints = new Queue<Vector3>();
    private Vector3 currentTarget;
    private bool isMoving = false;
    private Animator animator;
    private bool isBlocked = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 마우스 왼쪽 클릭
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            // 새로운 클릭이면 블록 상태 해제
            isBlocked = false;

            // 경로 계산
            CalculatePath(transform.position, mousePos);

            if (pathPoints.Count > 0)
            {
                currentTarget = pathPoints.Dequeue();
                isMoving = true;
            }
        }

        // 이동 처리
        if (isMoving && !isBlocked)
        {
            Vector3 moveDirection = (currentTarget - transform.position).normalized;

            transform.position = Vector3.MoveTowards(
                transform.position,
                currentTarget,
                speed * Time.deltaTime
            );

            // 애니메이션 파라미터 업데이트
            animator.SetBool("isMove", true);
            animator.SetFloat("xDir", moveDirection.x);
            animator.SetFloat("yDir", moveDirection.y);

            // 현재 목표 지점에 도착했는지 확인
            if (Vector3.Distance(transform.position, currentTarget) < 0.05f)
            {
                // 다음 경로 지점이 있으면 계속 이동
                if (pathPoints.Count > 0)
                {
                    currentTarget = pathPoints.Dequeue();
                }
                else
                {
                    // 모든 경로를 다 이동했으면 멈춤
                    isMoving = false;
                    animator.SetBool("isMove", false);
                }
            }
        }
    }

    void CalculatePath(Vector3 start, Vector3 end)
    {
        pathPoints.Clear();

        float xDiff = Mathf.Abs(end.x - start.x);
        float yDiff = Mathf.Abs(end.y - start.y);

        // 클릭한 지점이 너무 가까우면 무시
        if (xDiff < 0.1f && yDiff < 0.1f)
        {
            return;
        }

        // 코너 지점 (꺾이는 지점)
        Vector3 cornerPoint;

        // 더 긴 방향을 먼저 이동
        if (xDiff >= yDiff)
        {
            // 가로 먼저, 세로 나중
            cornerPoint = new Vector3(end.x, start.y, 0f);
        }
        else
        {
            // 세로 먼저, 가로 나중
            cornerPoint = new Vector3(start.x, end.y, 0f);
        }

        // 코너 지점이 현재 위치와 충분히 멀면 추가
        if (Vector3.Distance(start, cornerPoint) > 0.1f)
        {
            pathPoints.Enqueue(cornerPoint);
        }

        // 최종 목표 지점 추가
        pathPoints.Enqueue(end);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌 시 이동 중지
        isBlocked = true;
        isMoving = false;
        pathPoints.Clear();
        animator.SetBool("isMove", false);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 충돌에서 벗어나면 다시 이동 가능
        isBlocked = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Tag 확인해서 씬 전환
        if (other.CompareTag("Bed"))
        {
            SceneTransition.Instance.LoadSceneWithTransition("Timer");
        }
        else if (other.CompareTag("Sofa"))
        {
            SceneTransition.Instance.LoadSceneWithTransition("English");
        }
        else if (other.CompareTag("Desk"))
        {
            SceneTransition.Instance.LoadSceneWithTransition("Timer 1");
        }
    }
}