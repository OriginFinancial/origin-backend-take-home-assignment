

import unittest

from riskapi.risk_engine.entities import User
from riskapi.risk_engine.risk_calculation_visitors import HouseVisitor


class HouseVisitorTestcase(unittest.TestCase):

    def test_no_house(self):
        user = User(age=61, dependents=2, house=None, income=25000,
                    marital_status='married', risk_questions=[0, 1, 0],
                    vehicle={"year": 2015})

        user.accept(HouseVisitor())

        self.assertEqual('ineligible', user.disability_insurance, 'Incorrect disability insurance score.')
        self.assertEqual('ineligible', user.auto_insurance, 'Incorrect auto insurance score.')
        self.assertEqual('ineligible', user.home_insurance, 'Incorrect home insurance score.')

        self.assertEqual(0, user.disability_insurance_points, 'Incorrect disability insurance points.')
        self.assertEqual(0, user.auto_insurance_points, 'Incorrect auto insurance points.')
        self.assertEqual(0, user.home_insurance_points, 'Incorrect home insurance points.')
        self.assertEqual(0, user.life_insurance_points, 'Incorrect life insurance points.')

    def test_has_mortgaged_house(self):
        user = User(age=61, dependents=2, house={"ownership_status": "mortgaged"}, income=25000,
                    marital_status='married', risk_questions=[0, 1, 0],
                    vehicle={"year": 2015})

        user.accept(HouseVisitor())

        self.assertIsNone(user.disability_insurance, 'Incorrect disability insurance score.')
        self.assertIsNone(user.auto_insurance, 'Incorrect auto insurance score.')
        self.assertIsNone(user.home_insurance, 'Incorrect home insurance score.')

        self.assertEqual(1, user.disability_insurance_points, 'Incorrect disability insurance points.')
        self.assertEqual(0, user.auto_insurance_points, 'Incorrect auto insurance points.')
        self.assertEqual(1, user.home_insurance_points, 'Incorrect home insurance points.')
        self.assertEqual(0, user.life_insurance_points, 'Incorrect life insurance points.')

    def test_has_owned_house(self):
        user = User(age=61, dependents=2, house={"ownership_status": "owned"}, income=25000,
                    marital_status='married', risk_questions=[0, 1, 0],
                    vehicle={"year": 2015})

        user.accept(HouseVisitor())

        self.assertIsNone(user.disability_insurance, 'Incorrect disability insurance score.')
        self.assertIsNone(user.auto_insurance, 'Incorrect auto insurance score.')
        self.assertIsNone(user.home_insurance, 'Incorrect home insurance score.')

        self.assertEqual(0, user.disability_insurance_points, 'Incorrect disability insurance points.')
        self.assertEqual(0, user.auto_insurance_points, 'Incorrect auto insurance points.')
        self.assertEqual(0, user.home_insurance_points, 'Incorrect home insurance points.')
        self.assertEqual(0, user.life_insurance_points, 'Incorrect life insurance points.')