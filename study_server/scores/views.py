from django.http import JsonResponse
from django.views.decorators.csrf import csrf_exempt
import json
from .models import WordScore, StudyTimeScore

# ======================
# 공부 시간
# ======================
@csrf_exempt
def submit_study_time(request):
    if request.method != "POST":
        return JsonResponse({"error": "POST only"}, status=405)

    try:
        data = json.loads(request.body.decode('utf-8'))
    except json.JSONDecodeError:
        return JsonResponse({"error": "Invalid JSON"}, status=400)

    nickname = data.get("nickname")
    study_seconds = data.get("study_seconds")

    if not nickname or study_seconds is None:
        return JsonResponse({"error": "Invalid data"}, status=400)

    # 기존 기록이 있으면 누적, 없으면 새로 생성
    obj, created = StudyTimeScore.objects.get_or_create(nickname=nickname)
    obj.study_seconds += study_seconds  # 이전 값에 더하기
    obj.save()

    return JsonResponse({"message": "Study time updated", "total_seconds": obj.study_seconds})



# ======================
# 영어 단어 점수
# ======================

@csrf_exempt
def submit_word_score(request):
    if request.method != "POST":
        return JsonResponse({"error": "POST only"}, status=405)

    data = json.loads(request.body.decode("utf-8"))
    nickname = data.get("nickname")
    score = data.get("score")

    if not nickname or score is None:
        return JsonResponse({"error": "Invalid data"}, status=400)

    # 새 객체 생성 시 score 기본값 0 지정
    obj, created = WordScore.objects.get_or_create(nickname=nickname, defaults={'score': 0})

    # 기존 점수보다 크면 갱신
    if score > obj.score:
        obj.score = score
        obj.save()

    return JsonResponse({
        "message": "Word score updated",
        "total_score": obj.score
    })



@csrf_exempt
def get_word_score(request):
    nickname = request.GET.get("nickname")
    if not nickname:
        return JsonResponse({"error": "닉네임 필요"}, status=400)

    try:
        obj = WordScore.objects.get(nickname=nickname)
        total_score = obj.score
    except WordScore.DoesNotExist:
        total_score = 0

    return JsonResponse({
        "nickname": nickname,
        "total_score": total_score
    })
