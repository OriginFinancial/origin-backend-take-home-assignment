import json
import unittest

from riskapi.risk_engine.risk_engine_client import RiskEngineClient


class RiskEngineClientTestscase(unittest.TestCase):

    def test_calculate_risk_success_1(self):

        risk_engine = RiskEngineClient.create()

        request = {
            "type" : 'risk_score_calculation',
            "age": 35,
            "dependents": 2,
            "house": {"ownership_status": "owned"},
            "income": 0,
            "marital_status": "married",
            "risk_questions": [0, 1, 0],
            "vehicle": {"year": 2018}
        }

        result = risk_engine.process(request)

        expected_result = {
            "auto": "ineligible",
            "disability": "ineligible",
            "home": "ineligible",
            "life": "regular"
        }

        self.assertEqual(json.dumps(expected_result), result, "Message handler did not return accurate score.")

    def test_calculate_risk_success_2(self):

        risk_engine = RiskEngineClient.create()

        request = {
            "type" : 'risk_score_calculation',
            "age": 35,
            "dependents": 2,
            "house": {"ownership_status": "owned"},
            "income": 0,
            "marital_status": "married",
            "risk_questions": [0, 1, 0],
            "vehicle": {"year": 2018}
        }

        result = risk_engine.process(request)

        expected_result = {
            "auto": "ineligible",
            "disability": "ineligible",
            "home": "ineligible",
            "life": "regular"
        }

        self.assertEqual(json.dumps(expected_result), result, "Message handler did not return accurate score.")


