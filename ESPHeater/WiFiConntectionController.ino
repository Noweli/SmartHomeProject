void WaitForConnection(){
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.print(".");
  }
}

String GetConnectionData(){
  String data;

  data += "\n";
  data += "Connected to WiFi network\n";
  data += "Local ip: ";
  data += WiFi.localIP().toString();
  data += "\n";

  return data;
}