void Handle_MainSite()
{
  server.send(200, "text/html", WrapDataInBody("Currently available:</br>/turnOn</br>/turnOff"));
}

void Handle_TurnOnHeater()
{
  Serial.println("Received request to turn on heater");
  digitalWrite(HeaterPin, HIGH);
  server.send(200, "text/html", WrapDataInBody("Heater turned on"));
}

void Handle_TurnOffHeater()
{
  Serial.println("Received request to turn off heater");
  digitalWrite(HeaterPin, LOW);
  server.send(200, "text/html", WrapDataInBody("Heater turned off"));
}

void Handle_NotFound()
{
  server.send(404, "text/plain", "Not found");
}