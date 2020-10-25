void SetUpServer(){
  server.on("/", Handle_MainSite);
  server.on("/turnOn", Handle_TurnOnHeater);
  server.on("/turnOff", Handle_TurnOffHeater);
  server.onNotFound(Handle_NotFound);
  
  server.begin();
  Serial.println("HTTP server started");
}