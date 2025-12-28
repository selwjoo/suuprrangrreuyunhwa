from django.db import models
        
class StudyTimeScore(models.Model):
    nickname = models.CharField(max_length=20)
    study_seconds = models.IntegerField()

class WordScore(models.Model):
    nickname = models.CharField(max_length=20)
    score = models.IntegerField()
    created_at = models.DateTimeField(auto_now_add=True)

    def __str__(self):
        return f"{self.nickname} - {self.score}"