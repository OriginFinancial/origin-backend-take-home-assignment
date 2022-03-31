from riskapi.risk_engine.entities import User
from riskapi.risk_engine.risk_calculation_visitors import AgeVisitor, HouseVisitor, VehicleVisitor, \
    IncomeVisitor, MaritalStatusVisitor, DependentsVisitor, BaseScoreVisitor


class RiskCalculationService(object):
    """
    Calculates a User's Risk Score.
    """
    def process(self, message):
        """
        Caculates a risk score based on a properly formated user data insurance message.
        Example message:
            {
              "message_type" : "risk_score_calculation"
              "age": 35,
              "dependents": 2,
              "house": {
                "ownership_status": "owned"
              },
              "income": 0,
              "marital_status": "married",
              "risk_questions": [
                0,
                1,
                0
              ],
              "vehicle": {
                "year": 2018
              }
            }
        :param message: a properly formated user data insurance message
        :return: the calculated score. I.e.:
            {
                "auto": "economic",
                "disability": "ineligible",
                "home": "regular",
                "life": "ineligible"
            }
        """
        result = None
        if message['type'] == 'risk_score_calculation':
            payload = self.__extract_payload(message, type(User))
            result = self.calculate_risk(payload)
        else:
            raise TypeError('Message type nor supported.')
        return result

    def __extract_payload(self, message, type):
        if type == type(User):
            house = None
            vehicle = None
            if 'house' in message.keys():
                house = message['house']
            if 'vehicle' in message.keys():
                vehicle = message['vehicle']

            payload = User(age=message['age'],
                        dependents=message['dependents'],
                        house=house,
                        income=message['income'],
                        marital_status=message['marital_status'],
                        risk_questions=message['risk_questions'],
                        vehicle=vehicle)
        return payload

    def calculate_risk(self, user):
        assert user is not None, 'User argument cannot be None'
        assert type(user) == User, 'This method only takes an entities.User as an argument'

        user.accept(BaseScoreVisitor())
        user.accept(HouseVisitor())
        user.accept(VehicleVisitor())
        user.accept(IncomeVisitor())
        user.accept(AgeVisitor())
        user.accept(MaritalStatusVisitor())
        user.accept(DependentsVisitor())

        return user.calculate_score()

    @staticmethod
    def create():
        return RiskCalculationService()