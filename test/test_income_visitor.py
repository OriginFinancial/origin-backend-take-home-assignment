import unittest

from riskapi.risk_engine.entities import User
from riskapi.risk_engine.risk_calculation_visitors import IncomeVisitor


class IncomeVisitorTestcase(unittest.TestCase):

    def test_no_income(self):
        user = User(age=61, dependents=2, house={"ownership_status": "owned"}, income=0,
                    marital_status='married', risk_questions=[0, 1, 0],
                    vehicle={"year": 2018})

        user.accept(IncomeVisitor())

        self.assertEqual('ineligible', user.disability_insurance, 'Incorrect disability insurance score.')
        self.assertEqual('ineligible', user.auto_insurance, 'Incorrect auto insurance score.')
        self.assertEqual('ineligible', user.home_insurance, 'Incorrect home insurance score.')

        self.assertEqual(0, user.disability_insurance_points, 'Incorrect disability insurance points.')
        self.assertEqual(0, user.auto_insurance_points, 'Incorrect auto insurance points.')
        self.assertEqual(0, user.home_insurance_points, 'Incorrect home insurance points.')
        self.assertEqual(0, user.life_insurance_points, 'Incorrect life insurance points.')

    def test_income_greater_than_200k(self):
        user = User(age=61, dependents=2, house={"ownership_status": "owned"}, income=200001,
                    marital_status='married', risk_questions=[0, 1, 0],
                    vehicle={"year": 2018})

        user.accept(IncomeVisitor())

        self.assertIsNone(user.disability_insurance, 'Incorrect disability insurance score.')
        self.assertIsNone(user.auto_insurance, 'Incorrect auto insurance score.')
        self.assertIsNone(user.home_insurance, 'Incorrect home insurance score.')

        self.assertEqual(-1, user.disability_insurance_points, 'Incorrect disability insurance points.')
        self.assertEqual(-1, user.auto_insurance_points, 'Incorrect auto insurance points.')
        self.assertEqual(-1, user.home_insurance_points, 'Incorrect home insurance points.')
        self.assertEqual(-1, user.life_insurance_points, 'Incorrect life insurance points.')

    def test_income_200k_or_less(self):
        users = {
            '200k': User(age=61, dependents=2, house={"ownership_status": "owned"}, income=200000,
                    marital_status='married', risk_questions=[0, 1, 0],
                    vehicle={"year": 2018}),
            'less_than_200k': User(age=61, dependents=2, house={"ownership_status": "owned"}, income=199999.99,
                         marital_status='married', risk_questions=[0, 1, 0],
                         vehicle={"year": 2018})
        }

        for k, v in users.items():
            v.accept(IncomeVisitor())

            self.assertIsNone(v.disability_insurance, 'Incorrect disability insurance score.')
            self.assertIsNone(v.auto_insurance, 'Incorrect auto insurance score.')
            self.assertIsNone(v.home_insurance, 'Incorrect home insurance score.')

            self.assertEqual(0, v.disability_insurance_points, 'Incorrect disability insurance points.')
            self.assertEqual(0, v.auto_insurance_points, 'Incorrect auto insurance points.')
            self.assertEqual(0, v.home_insurance_points, 'Incorrect home insurance points.')
            self.assertEqual(0, v.life_insurance_points, 'Incorrect life insurance points.')
