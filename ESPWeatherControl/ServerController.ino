void SetUpServer(){
  server.on("/", Handle_MainSite);
  server.on("/temperature", Handle_TemperatureRequest);
  server.on("/humidity", Handle_HumidityRequest);
  server.on("/sound", Handle_SoundSensorRequest);
  server.on("/light", Handle_LightSensorRequest);
  server.on("/setAutoHeater", Handle_SetAutoHeater);
  server.on("/turnOnAutoHeater", Handle_TurnOnAutoHeater);
  server.on("/turnOffAutoHeater", Handle_TurnOffAutoHeater);
  server.onNotFound(Handle_NotFound);
  
  server.begin();
  Serial.println("HTTP server started");
}