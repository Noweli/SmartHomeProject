void SetUpServer(){
  server.on("/", Handle_MainSite);
  server.on("/temperature", Handle_TemperatureRequest);
  server.on("/humidity", Handle_HumidityRequest);
  server.on("/sound", Handle_SoundSensorRequest);
  server.onNotFound(Handle_NotFound);
  
  server.begin();
  Serial.println("HTTP server started");
}