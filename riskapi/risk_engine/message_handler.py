from riskapi.risk_engine.service_locator import ServiceLocator


class MessageHandler(object):
    """
    Handles messages that should be routed to specific services within the engine.
    """
    def __init__(self, service_locator=None):
        self.__service_locator = service_locator if service_locator else ServiceLocator.create()

    def handle(self, message):
        result = self.__service_locator.get(message['type']).process(message)
        return result

    @staticmethod
    def create():
        service_locator = ServiceLocator.create()
        return MessageHandler(service_locator)