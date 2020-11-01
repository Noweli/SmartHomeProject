import requests
class SensorRequests:
    def __init__(self, inputLabels):
        self.labels = inputLabels

    def sendTemperatureRequest(self):
        request = requests.get(url="http://192.168.0.144/temperature")
        data = request.json()
        self.labels['temperatureDataLabel'].config(text=data['temperature'])

    def sendHumidityRequest(self):
        request = requests.get(url="http://192.168.0.144/humidity")
        data = request.json()
        self.labels['humidityDataLabel'].config(text=data['humidity'])

    def sendSoundRequest(self):
        request = requests.get(url="http://192.168.0.144/sound")
        data = request.json()
        self.labels['soundDataLabel'].config(text=data['lastDetectionDate'])

    def sendLightRequest(self):
        request = requests.get(url="http://192.168.0.144/light")
        data = request.json()
        self.labels['lightDataLabel'].config(text=data['light'])

    def RefreshAllSensors(self):
        self.sendTemperatureRequest()
        self.sendHumidityRequest()
        self.sendSoundRequest()
        self.sendLightRequest()
