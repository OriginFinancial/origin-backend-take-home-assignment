

import unittest

from riskapi.risk_engine.entities import User
from riskapi.risk_engine.risk_calculation_visitors import VehicleVisitor


class VehicleVisitorTestcase(unittest.TestCase):

    def test_no_vehicle(self):
        user = User(age=61, dependents=2, house={"ownership_status": "owned"}, income=25000,
                    marital_status='married', risk_questions=[0, 1, 0],
                    vehicle=None)

        user.accept(VehicleVisitor())

        self.assertEqual('ineligible', user.disability_insurance, 'Incorrect disability insurance score.')
        self.assertEqual('ineligible', user.auto_insurance, 'Incorrect auto insurance score.')
        self.assertEqual('ineligible', user.home_insurance, 'Incorrect home insurance score.')

        self.assertEqual(0, user.disability_insurance_points, 'Incorrect disability insurance points.')
        self.assertEqual(0, user.auto_insurance_points, 'Incorrect auto insurance points.')
        self.assertEqual(0, user.home_insurance_points, 'Incorrect home insurance points.')
        self.assertEqual(0, user.life_insurance_points, 'Incorrect life insurance points.')

    def test_calculate_vehicle_over_5_years(self):
        user = User(age=61, dependents=2, house={"ownership_status": "owned"}, income=25000,
                                    marital_status='married', risk_questions=[0, 1, 0],
                                    vehicle={"year": 2015})
        user.accept(VehicleVisitor())

        self.assertIsNone(user.disability_insurance, 'Incorrect disability insurance score.')
        self.assertIsNone(user.auto_insurance, 'Incorrect auto insurance score.')
        self.assertIsNone(user.home_insurance, 'Incorrect home insurance score.')

        self.assertEqual(0, user.disability_insurance_points, 'Incorrect disability insurance points.')
        self.assertEqual(0, user.auto_insurance_points, 'Incorrect auto insurance points.')
        self.assertEqual(0, user.home_insurance_points, 'Incorrect home insurance points.')
        self.assertEqual(0, user.life_insurance_points, 'Incorrect life insurance points.')

    def test_calculate_vehicle_whithin_5_years(self):
        user = User(age=61, dependents=2, house={"ownership_status": "owned"}, income=25000,
                    marital_status='married', risk_questions=[0, 1, 0],
                    vehicle={"year": 2019})

        user.accept(VehicleVisitor())

        self.assertIsNone(user.disability_insurance, 'Incorrect disability insurance score.')
        self.assertIsNone(user.auto_insurance, 'Incorrect auto insurance score.')
        self.assertIsNone(user.home_insurance, 'Incorrect home insurance score.')

        self.assertEqual(0, user.disability_insurance_points, 'Incorrect disability insurance points.')
        self.assertEqual(1, user.auto_insurance_points, 'Incorrect auto insurance points.')
        self.assertEqual(0, user.home_insurance_points, 'Incorrect home insurance points.')
        self.assertEqual(0, user.life_insurance_points, 'Incorrect life insurance points.')
