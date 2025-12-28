from django.http import JsonResponse
from .models import Word

def word_list(request):
    words = Word.objects.all()
    data = [
        {
            "english": w.english,
            "meaning": w.meaning
        } for w in words
    ]
    return JsonResponse(data, safe=False)