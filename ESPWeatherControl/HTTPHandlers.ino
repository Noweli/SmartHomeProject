void Handle_MainSite()
{
  server.send(200, "text/html", WrapDataInBody("Currently available:</br>/temperature</br>/humidity<br>/sound"));
}

void Handle_TemperatureRequest()
{
  server.send(200, "text/html", SendHTMLWithSensorData((float)dht.readTemperature()));
}

void Handle_HumidityRequest()
{
  server.send(200, "text/html", SendHTMLWithSensorData((float)dht.readHumidity()));
}

void Handle_SoundSensorRequest()
{
  server.send(200, "text/html", WrapDataInBody(soundDetectionDate));
}

void Handle_NotFound()
{
  server.send(404, "text/plain", "Not found");
}