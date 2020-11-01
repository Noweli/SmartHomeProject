from tkinter import Label

class GUIControllers:
    def GetSensorDataNameLabels(self, root):
        dataNameLabels = {
            "temperatureLabel": Label(root, text="Temperature"),
            "humidityLabel": Label(root, text ="Humidity"),
            "lightLabel": Label(root, text ="Light level"),
            "soundLabel": Label(root, text ="Last sound detection")
        }

        return dataNameLabels

    def GetSensorLabels(self, root):
        sensorLabels = {
            "temperatureDataLabel": Label(root, text="temperature"),
            "humidityDataLabel": Label(root, text ="humidity"),
            "lightDataLabel": Label(root, text ="light"),
            "soundDataLabel": Label(root, text ="lastSoundDetection")
        }

        return sensorLabels

    def SetControllersOnGrid(self, dataLabels, dataNameLabels):
        gridColumn = 0
        for label in dataNameLabels.values():
            label.grid(row = 0, column = gridColumn)
            gridColumn += 1

        gridColumn = 0
        for label in dataLabels.values():
            label.grid(row = 1, column = gridColumn)
            gridColumn += 1
