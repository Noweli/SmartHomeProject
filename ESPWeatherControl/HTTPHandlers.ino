void Handle_MainSite()
{
  server.send(200, "text/html", WrapDataInBody("Currently available:</br>/temperature</br>/humidity<br>/sound<br>/light"));
}

void Handle_TemperatureRequest()
{
  temperature = (float)dht.readTemperature();
  server.send(200, "application/json", SendJSONWithSensorData("temperature", temperature));
}

void Handle_HumidityRequest()
{
  humidity = (float)dht.readHumidity();
  server.send(200, "application/json", SendJSONWithSensorData("humidity", humidity));
}

void Handle_LightSensorRequest()
{
  server.send(200, "application/json", SendJSONWithSensorData("light", lightSensor.readLightLevel(true)));
}

void Handle_SoundSensorRequest()
{
  server.send(200, "application/json", WrapDataInJSON("lastDetectionDate", soundDetectionDate));
}

void Handle_NotFound()
{
  server.send(404, "text/plain", "Not found");
}