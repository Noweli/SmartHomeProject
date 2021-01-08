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

void Handle_SetAutoHeater()
{
  if(server.args() != 3) 
  {
    server.send(400, "application/json", WrapDataInBody("error: Invalid number of parameters! Usage: ?minTemp=[number]&maxTemp=[number]&heaterAddress=[ip_address]"));
    return;
  }

  if(server.argName(0) != "minTemp" || server.argName(1) != "maxTemp" || server.argName(2) != "heaterAddress") 
  {
    server.send(400, "application/json", WrapDataInBody("error: Invalid parameters! Usage: ?minTemp=[number]&maxTemp=[number]&heaterAddress=[ip_address]"));
    return;
  }

  minAutoHeaterTemp = server.arg(0).toInt();
  maxAutoHeaterTemp = server.arg(1).toInt();
  heaterAddress = server.arg(2);

  String response = "minTemp";
  response += String(minAutoHeaterTemp);
  response += "\nmaxTemp";
  response += String(maxAutoHeaterTemp);
  response += "\nheaterAddress";
  response += String(heaterAddress);

  server.send(200, "application/json", WrapDataInBody(response));
}

void Handle_TurnOnAutoHeater()
{
  if(minAutoHeaterTemp == -99 || maxAutoHeaterTemp == -99 || heaterAddress == NULL){
    server.send(400, "application/json", WrapDataInBody("error: Cannot turn on auto heater. Min and max temperature is not set!"));
    return;
  }

  isAutoHeatEnabled = true;
  server.send(200, "application/json", WrapDataInBody(String(isAutoHeatEnabled)));
}

void Handle_TurnOffAutoHeater()
{
  isAutoHeatEnabled = false;
  TurnOffHeater();
  server.send(200, "application/json", WrapDataInBody(String(isAutoHeatEnabled)));
}

void Handle_NotFound()
{
  server.send(404, "text/plain", "Not found");
}