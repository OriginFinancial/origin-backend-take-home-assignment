from abc import ABC, abstractmethod
from datetime import date


class Visitor(ABC):

    @abstractmethod
    def visit(self, entity):
        pass


class BaseScoreVisitor(Visitor):

    def visit(self, user):
        for v in user.risk_questions:
            user.base_points += v

        user.update_insurance_points(user.base_points, 'auto')
        user.update_insurance_points(user.base_points, 'home')
        user.update_insurance_points(user.base_points, 'disability')
        user.update_insurance_points(user.base_points, 'life')

class IncomeVisitor(Visitor):

    def visit(self, user):
        if user.income == 0:
            user.home_insurance = 'ineligible'
            user.auto_insurance = 'ineligible'
            user.disability_insurance = 'ineligible'
        elif user.income > 200000:
            user.update_insurance_points(-1, 'auto')
            user.update_insurance_points(-1, 'home')
            user.update_insurance_points(-1, 'disability')
            user.update_insurance_points(-1, 'life')


class VehicleVisitor(Visitor):

    def visit(self, user):
        if user.vehicle is None:
            user.home_insurance = 'ineligible'
            user.auto_insurance = 'ineligible'
            user.disability_insurance = 'ineligible'
        else:
            cur_year = date.today().year
            if (cur_year - 5) <= user.vehicle['year'] <= cur_year:
                user.update_insurance_points(1, 'auto')

class HouseVisitor(Visitor):

    def visit(self, user):
        if user.house is None:
            user.home_insurance = 'ineligible'
            user.auto_insurance = 'ineligible'
            user.disability_insurance = 'ineligible'
        elif user.house['ownership_status'] == 'mortgaged':
            user.update_insurance_points(1, 'home')
            user.update_insurance_points(1, 'disability')


class MaritalStatusVisitor(Visitor):

    def visit(self, user):
        if user.marital_status == 'married':
            user.update_insurance_points(1, 'life')
            user.update_insurance_points(-1, 'disability')


class DependentsVisitor(Visitor):

    def visit(self, user):
        if user.dependents > 0:
            user.update_insurance_points(1, 'life')
            user.update_insurance_points(1, 'disability')


class AgeVisitor(Visitor):

    def visit(self, user):
        if user.age > 60:
            user.disability_insurance = 'ineligible'
            user.life_insurance = 'ineligible'
        elif 30 <= user.age <= 40:
            user.update_insurance_points(-1, 'auto')
            user.update_insurance_points(-1, 'home')
            user.update_insurance_points(-1, 'disability')
            user.update_insurance_points(-1, 'life')
        elif user.age < 30:
            user.update_insurance_points(-2, 'auto')
            user.update_insurance_points(-2, 'home')
            user.update_insurance_points(-2, 'disability')
            user.update_insurance_points(-2, 'life')

