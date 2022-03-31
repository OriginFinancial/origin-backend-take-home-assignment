from riskapi.risk_engine.risk_calculation_service import RiskCalculationService


class ServiceLocator(object):

    def __init__(self):
        self.__services = {
            'risk_score_calculation' : RiskCalculationService.create()
        }

    def get(self, service_name):
        assert service_name is not None
        return self.__services[service_name]

    @staticmethod
    def create():
        return ServiceLocator()