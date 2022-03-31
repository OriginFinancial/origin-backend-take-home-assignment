import unittest

from riskapi.risk_engine.message_handler import MessageHandler


class RiskEngineMessageHandlerTestcase(unittest.TestCase):

    def test_handle_risk_score_message_success(self):

        message_handler = MessageHandler.create()

        self.assertIsNotNone(message_handler, 'Message Handler was not created.')

        message = {
            "type" : 'risk_score_calculation',
            "age": 35,
            "dependents": 2,
            "house": {"ownership_status": "owned"},
            "income": 0,
            "marital_status": "married",
            "risk_questions": [0, 1, 0],
            "vehicle": {"year": 2018}
        }

        result = message_handler.handle(message)

        self.assertIsNotNone(result, 'Message Handler did not return a result.')

        expected_result = {
            "auto": "ineligible",
            "disability": "ineligible",
            "home": "ineligible",
            "life": "regular"
        }

        self.assertEqual(expected_result, result, "Message handler did not return accurate score.")