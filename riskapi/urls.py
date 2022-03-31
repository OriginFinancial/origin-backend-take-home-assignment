from django.urls import path

from . import views

urlpatterns = [
    path('v1/users/risk_score', views.risk_score, name='risk_score')
]