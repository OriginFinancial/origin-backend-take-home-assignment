import unittest

from riskapi.risk_engine.entities import User
from riskapi.risk_engine.risk_calculation_visitors import DependentsVisitor


class DependentsVisitorTestcase(unittest.TestCase):

    def test_user_has_no_dependents(self):

        user = User(age=61, dependents=0, house={"ownership_status": "owned"}, income=0,
                    marital_status='married', risk_questions=[0, 1, 0],
                    vehicle={"year": 2018})

        user.accept(DependentsVisitor())

        self.assertIsNone(user.disability_insurance, 'Incorrect disability insurance score.')
        self.assertIsNone(user.auto_insurance, 'Incorrect auto insurance score.')
        self.assertIsNone(user.home_insurance, 'Incorrect home insurance score.')

        self.assertEqual(0, user.disability_insurance_points, 'Incorrect disability insurance points.')
        self.assertEqual(0, user.auto_insurance_points, 'Incorrect auto insurance points.')
        self.assertEqual(0, user.home_insurance_points, 'Incorrect home insurance points.')
        self.assertEqual(0, user.life_insurance_points, 'Incorrect life insurance points.')

    def test_user_has_dependents(self):

        user = User(age=61, dependents=2, house={"ownership_status": "owned"}, income=0,
                    marital_status='married', risk_questions=[0, 1, 0],
                    vehicle={"year": 2018})

        user.accept(DependentsVisitor())

        self.assertIsNone(user.disability_insurance, 'Incorrect disability insurance score.')
        self.assertIsNone(user.auto_insurance, 'Incorrect auto insurance score.')
        self.assertIsNone(user.home_insurance, 'Incorrect home insurance score.')

        self.assertEqual(1, user.disability_insurance_points, 'Incorrect disability insurance points.')
        self.assertEqual(0, user.auto_insurance_points, 'Incorrect auto insurance points.')
        self.assertEqual(0, user.home_insurance_points, 'Incorrect home insurance points.')
        self.assertEqual(1, user.life_insurance_points, 'Incorrect life insurance points.')

if __name__ == '__main__':
    unittest.main()