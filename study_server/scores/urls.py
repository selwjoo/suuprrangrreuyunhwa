from django.urls import path
from . import views

urlpatterns = [
    path('submit_study_time/', views.submit_study_time),
    path('submit_word_score/', views.submit_word_score),
    path('get_word_score/', views.get_word_score),  # ← 반드시 있어야 함
]
