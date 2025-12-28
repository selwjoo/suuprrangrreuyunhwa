from django.db import models

class Word(models.Model):
    english = models.CharField(max_length=50)
    meaning = models.CharField(max_length=100)

    def __str__(self):
        return self.english