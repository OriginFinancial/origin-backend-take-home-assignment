import unittest

from riskapi.risk_engine.entities import User
from riskapi.risk_engine.risk_calculation_visitors import BaseScoreVisitor


class BaseScoreTestcase(unittest.TestCase):

    def test_base_score_calculation(self):
        risk_quetions_points = [
            {'points': [0, 0, 0], 'expected_result': 0}, {'points': [0, 1, 0], 'expected_result': 1}, {'points': [1, 0, 1], 'expected_result': 2},  {'points': [1, 1, 1], 'expected_result': 3}
        ]

        for item in risk_quetions_points:
            user = User(age=61, dependents=2, house=None, income=0,
                        marital_status='married', risk_questions=item['points'],
                        vehicle=None)

            user.accept(BaseScoreVisitor())

            self.assertEqual(item['expected_result'], user.base_points, 'The base score calculated is incorrect.')
            self.assertEqual(item['expected_result'], user.auto_insurance_points, 'Incorrect auto insurance.')
            self.assertEqual(item['expected_result'], user.home_insurance_points, 'Incorrect home insurance.')
            self.assertEqual(item['expected_result'], user.life_insurance_points, 'Incorrect life insurance.')
            self.assertEqual(item['expected_result'], user.disability_insurance_points, 'Incorrect disability insurance.')