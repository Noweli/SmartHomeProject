from tkinter import Tk, Button
from SensorRequestController import SensorRequests
from GUIControllers import GUIControllers
import time
import threading

root = Tk()
root.title("SmartHome Project")
root.geometry("500x400")
guiControllers = GUIControllers()

dataNameLabels = guiControllers.GetSensorDataNameLabels(root)
dataLabels = guiControllers.GetSensorLabels(root)
sensorController = SensorRequests(dataLabels)
guiControllers.SetControllersOnGrid(dataLabels, dataNameLabels)

shouldStop = False
def sensorCheckLoop():
    while(True):
        sensorController.RefreshAllSensors()
        time.sleep(1)
        if shouldStop:
            break

sensorCheckThread = threading.Thread(target=sensorCheckLoop)
sensorCheckThread.daemon = True
sensorCheckThread.start()

root.mainloop()