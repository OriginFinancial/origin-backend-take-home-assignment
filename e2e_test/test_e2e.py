import json
import unittest
import requests
import os


class E2ETestcase(unittest.TestCase):

    def setUp(self) -> None:
        test_data_path = os.path.dirname((os.path.realpath(__file__)))
        with open(test_data_path + '/payloads.json', 'r') as f:
            self.test_records = json.load(f)

    def test_calculate_risk(self):
        i = 0
        for idx, rec in enumerate(self.test_records):
            print('Test ' + str(idx) + ' - ', rec)
            r = requests.post('http://127.0.0.1:8000/riskapi/v1/users/risk_score', data=json.dumps(rec['payload']))
            result = json.loads(r.text)
            self.assertEqual(rec['expected_result'], result, 'Incorrect insurance scored.')