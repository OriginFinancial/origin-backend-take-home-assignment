from riskapi.risk_engine.risk_calculation_visitors import Visitor


class User(object):
    """
    User domain object. Contains data and behavior pertinent to User Risk Score calculation.
    """
    def __init__(self, age=None, dependents=None, house=None, income=None, marital_status=None, risk_questions=None, vehicle=None):
        self.age = age
        self.dependents = dependents
        self.house = house
        self.income = income
        self.marital_status = marital_status
        self.risk_questions = risk_questions
        self.vehicle = vehicle
        self.base_points = 0
        self.auto_insurance_points = 0
        self.home_insurance_points = 0
        self.disability_insurance_points = 0
        self.life_insurance_points = 0
        self.auto_insurance = None
        self.disability_insurance = None
        self.home_insurance = None
        self.life_insurance = None

    def accept(self, visitor : Visitor):
        visitor.visit(self)

    def update_insurance_points(self, amount, insurance):
        if insurance == 'auto':
            self.auto_insurance_points += amount
        elif insurance == 'home':
            self.home_insurance_points += amount
        elif insurance == 'disability':
            self.disability_insurance_points += amount
        elif insurance == 'life':
            self.life_insurance_points += amount

    def calculate_score(self):

        score = {
            'auto' : 'ineligible' if self.auto_insurance == 'ineligible' else  self.__get_score(self.auto_insurance_points),
            'disability' : 'ineligible' if self.disability_insurance == 'ineligible' else self.__get_score(self.disability_insurance_points),
            'home' : 'ineligible' if self.home_insurance == 'ineligible' else  self.__get_score(self.home_insurance_points),
            'life' : 'ineligible' if self.life_insurance == 'ineligible' else self.__get_score(self.life_insurance_points)
        }

        print(str(self))
        print('Points(', f'auto: {self.auto_insurance_points}, home: {self.home_insurance_points}, disability: {self.disability_insurance_points}, life: {self.life_insurance_points})')

        return score

    def __get_score(self, points):
        score = None

        if points <= 0:
            score = 'economic'
        elif points == 1 or points == 2:
            score = 'regular'
        else:
            score = 'responsible'

        return score

    def __str__(self):
        return f'User(age={self.age}, dependents={self.dependents}, house={self.house}, income={self.income}, marital_status={self.marital_status}, risk_questions={self.risk_questions}, vehicle={self.vehicle})'