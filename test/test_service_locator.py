import unittest

from riskapi.risk_engine.risk_calculation_service import RiskCalculationService
from riskapi.risk_engine.service_locator import ServiceLocator


class ServiceLocatorTestcase(unittest.TestCase):

    def test_get_risk_calculation_service(self):
        service_locator = ServiceLocator.create()
        self.assertIsNotNone(service_locator, "Service Locator was not created.")

        service_name = 'risk_score_calculation'
        service = service_locator.get(service_name)

        self.assertIsNotNone(service, 'Could not locate the service named ' + service_name)
        self.assertEqual(RiskCalculationService, type(service), 'Service Locator did not return the correct service.')