void Handle_MainSite()
{
  server.send(200, "text/html", WrapDataInBody("Currently available:</br>/temperature</br>/humidity<br>/sound"));
}

void Handle_TemperatureRequest()
{
  temperature = (float)dht.readTemperature();
  server.send(200, "text/html", SendHTMLWithSensorData(temperature));
}

void Handle_HumidityRequest()
{
  humidity = (float)dht.readHumidity();
  server.send(200, "text/html", SendHTMLWithSensorData(humidity));
}

void Handle_SoundSensorRequest()
{
  server.send(200, "text/html", WrapDataInBody(soundDetectionDate));
}

void Handle_NotFound()
{
  server.send(404, "text/plain", "Not found");
}