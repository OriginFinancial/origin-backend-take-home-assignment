import unittest

from riskapi.risk_engine.entities import User
from riskapi.risk_engine.risk_calculation_service import RiskCalculationService


class RiskCalculationServiceTestcase(unittest.TestCase):

    def setUp(self) -> None:
        self.users = {
            'no_income' : User(age=35,
                               dependents=2,
                               house={"ownership_status": "owned"},
                               income=0,
                               marital_status='married',
                               risk_questions=[0, 1, 0],
                               vehicle={"year": 2018}
                               ),
            'no_house' : User(age=35,
                              dependents=2,
                              house=None,
                              income=100000,
                              marital_status='married',
                              risk_questions=[0, 1, 0],
                              vehicle={"year": 2018}
                              ),
            'no_vehicle' : User(age=35,
                                dependents=2,
                                house={"ownership_status": "owned"},
                                income=0,
                                marital_status='married',
                                risk_questions=[0, 1, 0],
                                vehicle={"year": 2018}
                                )
        }

        self.messages = {
            'no_income' : {
                "type" : 'risk_score_calculation',
                "age": 35,
                "dependents": 2,
                "house": {"ownership_status": "owned"},
                "income": 0,
                "marital_status": "married",
                "risk_questions": [0, 1, 0],
                "vehicle": {"year": 2018}
            },
            'no_house' : {
                "type" : 'risk_score_calculation',
                "age": 35,
                "dependents": 2,
                "income": 100000,
                "marital_status": "married",
                "risk_questions": [0, 1, 0],
                "vehicle": {"year": 2018}
            },
            'no_vehicle' : {
                "type" : 'risk_score_calculation',
                "age": 35,
                "dependents": 2,
                "house": {"ownership_status": "owned"},
                "income": 100000,
                "marital_status": "married",
                "risk_questions": [0, 1, 0]
            }
        }

    def test_calculate_risk_ineligible(self):

        expected_score = {
            "auto": "ineligible",
            "disability": "ineligible",
            "home": "ineligible",
            'life': 'ineligible'
        }

        risk_calculation_service = RiskCalculationService.create()

        user = User(age=61,
             dependents=2,
             house={"ownership_status": "owned"},
             income=0,
             marital_status='married',
             risk_questions=[0, 1, 0],
             vehicle={"year": 2018}
             )

        risk_score = risk_calculation_service.calculate_risk(user)

        self.assertEqual(expected_score, risk_score, "The Engine did not calculate the risk score accurately for user: " + str(user))

    def test_calculate_risk_economic(self):

        expected_score = {
            "auto": "economic",
            "disability": "economic",
            "home": "economic",
            'life': 'economic'
        }

        risk_calculation_service = RiskCalculationService.create()

        user = User(age=35,
                    dependents=0,
                    house={"ownership_status": "owned"},
                    income=200001,
                    marital_status='single',
                    risk_questions=[0, 1, 0],
                    vehicle={"year": 2015}
                    )

        risk_score = risk_calculation_service.calculate_risk(user)

        self.assertEqual(expected_score, risk_score, "The Engine did not calculate the risk score accurately for user: " + str(user))

    def test_process_success_regular(self):

        message = {
            "type" : 'risk_score_calculation',
            "age": 45,
            "dependents": 2, #disability = 1, life = 1
            "house": {"ownership_status": "mortgaged"}, #disability = 1, home = 1
            "income": 100000,
            "marital_status": "married", #life = 1, disability = -1
            "risk_questions": [0, 1, 0],
            "vehicle": {"year": 2020} #auto = 1
        }

        risk_calculation_service = RiskCalculationService.create()

        risk_score = risk_calculation_service.process(message)

        expected_score = {
            "auto": "regular",
            "disability": "regular",
            "home": "regular",
            'life': 'responsible'
        }

        risk_calculation_service = RiskCalculationService.create()

        risk_score = risk_calculation_service.process(message)
        self.assertEqual(expected_score, risk_score, "The Engine did not calculate the risk score accurately for message: " + str(message))


