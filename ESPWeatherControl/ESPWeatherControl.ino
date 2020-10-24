#include <DNSServer.h>
#include <ESP8266WebServer.h>
#include <WiFiManager.h>
#include "DHT.h"

#define DHTTYPE DHT11

uint8_t DHTPin = D6;

ESP8266WebServer server(80);
DHT dht(DHTPin, DHTTYPE);
 
void setup() {
  //Serial communication setup for logs
  Serial.begin(9600);
  delay(100);

  //Enable DHT sensor
  pinMode(DHTPin, INPUT);
  dht.begin();              

  //Used for WiFi connection
  //If ESP cannot connect using saved connections then is switches to Access Point
  //Default IP for configuration is 192.168.4.1
  WiFiManager wifiManager;
  wifiManager.autoConnect("Module1-ESP");

  //Loop as long as WiFi is not connected
  WaitForConnection();
  GetConnectionData();
  SetUpServer();

}

void loop() {
  server.handleClient();
}

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

void SetUpServer(){
  server.on("/", Handle_MainSite);
  server.on("/temperature", Handle_TemperatureRequest);
  server.on("/humidity", Handle_HumidityRequest);
  server.onNotFound(Handle_NotFound);
  
  server.begin();
  Serial.println("HTTP server started");
}

void Handle_MainSite(){
  server.send(200, "text/html", WrapDataInBody("Currently available:</br>/temperature</br>/humidity"));
}

void Handle_TemperatureRequest() {
  server.send(200, "text/html", SendHTMLWithSensorData((float)dht.readTemperature())); 
}

void Handle_HumidityRequest() {
  server.send(200, "text/html", SendHTMLWithSensorData((float)dht.readHumidity())); 
}

void Handle_NotFound(){
  server.send(404, "text/plain", "Not found");
}

String SendHTMLWithSensorData(float sensorData){
  Serial.println("SendHTMLWithSensorData method called");
  return WrapDataInBody(String(sensorData));
}

String WrapDataInBody(String data){
  Serial.print("WrapDataInBody method called with following data - ");
  Serial.println(data);
  
  String site = "<!DOCTYPE html> <html>\n";
  site +="<head>\n";
  site +="<body>\n";
  site += data;
  site +="\n</body>";
  site +="</head>\n";
  site +="</html>\n";

  return site;
}
