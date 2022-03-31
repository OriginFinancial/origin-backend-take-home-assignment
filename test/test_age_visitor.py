import unittest

from riskapi.risk_engine.entities import User
from riskapi.risk_engine.risk_calculation_visitors import AgeVisitor


class Over60VisitorTestcase(unittest.TestCase):

    def test_user_is_over_60(self):

        user = User(age=61, dependents=2, house=None, income=0,
                    marital_status='married', risk_questions=[0, 1, 0],
                    vehicle=None)

        user.accept(AgeVisitor())

        self.assertEqual('ineligible', user.disability_insurance, 'Incorrect disability score.')
        self.assertEqual('ineligible', user.life_insurance, 'Incorrect life score.')

    def test_user_is_between_30_and_40(self):

        user = User(age=35, dependents=2, house={"ownership_status": "owned"}, income=100000,
                    marital_status='married', risk_questions=[0, 1, 0],
                    vehicle={"year": 2018})

        user.accept(AgeVisitor())

        self.assertEqual(-1, user.disability_insurance_points, 'Incorrect disability score.')

    def test_user_is_less_than_30(self):

        user = User(age=29, dependents=2, house={"ownership_status": "owned"}, income=100000,
                    marital_status='married', risk_questions=[0, 1, 0],
                    vehicle={"year": 2018})

        user.accept(AgeVisitor())

        self.assertEqual(-2, user.disability_insurance_points, 'Incorrect disability score.')


if __name__ == '__main__':
    unittest.main()
