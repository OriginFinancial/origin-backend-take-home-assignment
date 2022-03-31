import json

from riskapi.risk_engine.message_handler import MessageHandler


class RiskEngineClient(object):

    def __init__(self, request_handler=None):
        """
        The following approach for initialization of a class dependencies allows me to replace
        the depedency with a Fake or Mock version of it during unit testing.:

        RiskEngineClient(request_handler=FakeRequestHandler())

        """
        self.__request_handler = request_handler if request_handler else MessageHandler.create()

    def process(self, message):
        result = self.__request_handler.handle(message)
        return json.dumps(result)

    @staticmethod
    def create():
        return RiskEngineClient()

