import json

from django.http import HttpResponse

# Create your views here.
from django.views.decorators.csrf import csrf_exempt
from riskapi.risk_engine.risk_engine_client import RiskEngineClient


@csrf_exempt
def risk_score(request):
    response = None
    if request.method == 'POST':
        risk_engine = RiskEngineClient.create()
        message = json.loads(request.body.decode('utf-8').strip('\n'))
        message['type'] = 'risk_score_calculation'
        print('Message is ', message)
        result = risk_engine.process(message)
        response = HttpResponse(str(result))
    return response