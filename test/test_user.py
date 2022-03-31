import random
import unittest


from riskapi.risk_engine.entities import User


class UserTestcase(unittest.TestCase):

    def test_update_insurance_points(self):
        user = User(age=35,
                    dependents=2,
                    house={"ownership_status": "owned"},
                    income=0,
                    marital_status='married',
                    risk_questions=[0, 1, 0],
                    vehicle={"year": 2018}
                    )

        user.update_insurance_points(1, 'auto')
        user.update_insurance_points(2, 'home')
        user.update_insurance_points(3, 'disability')
        user.update_insurance_points(4, 'life')

        self.assertEqual(1, user.auto_insurance_points, 'Incorrect result after insurance points deduction.')
        self.assertEqual(2, user.home_insurance_points, 'Incorrect result after insurance points deduction.')
        self.assertEqual(3, user.disability_insurance_points, 'Incorrect result after insurance points deduction.')
        self.assertEqual(4, user.life_insurance_points, 'Incorrect result after insurance points deduction.')

        user.update_insurance_points(-2, 'auto')
        user.update_insurance_points(-2, 'home')
        user.update_insurance_points(-2, 'disability')
        user.update_insurance_points(-2, 'life')

        self.assertEqual(-1, user.auto_insurance_points, 'Incorrect result after insurance points deduction.')
        self.assertEqual(0, user.home_insurance_points, 'Incorrect result after insurance points deduction.')
        self.assertEqual(1, user.disability_insurance_points, 'Incorrect result after insurance points deduction.')
        self.assertEqual(2, user.life_insurance_points, 'Incorrect result after insurance points deduction.')


    def test_calculate_score(self):

        user = User()
        economic_values = random.sample(range(0, -100, -1), 10)
        economic_values.append(0)

        regular_values = [1, 2]

        responsible_values = random.sample(range(3, 100, 1), 10)
        responsible_values.append(3)

        for score in [{'name': 'economic', 'values': economic_values},
                      {'name': 'regular', 'values': regular_values},
                      {'name': 'responsible', 'values': responsible_values}]:

            expected_score = {
                "auto": score['name'],
                "disability": score['name'],
                "home": score['name'],
                "life": score['name']
            }

            for value in score['values']:
                user.auto_insurance_points = value
                user.disability_insurance_points = value
                user.home_insurance_points = value
                user.life_insurance_points = value

                score = user.calculate_score()

                self.assertEqual(expected_score, score, 'Incorrect score calculated.')

