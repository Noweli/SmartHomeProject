#include <DNSServer.h>
#include <ESP8266WebServer.h>
#include <WiFiManager.h>

uint8_t HeaterPin = D5;
ESP8266WebServer server(80);

void setup()
{
  //Serial communication setup for logs
  Serial.begin(9600);
  delay(100);

  pinMode(HeaterPin, OUTPUT);

  //Used for WiFi connection
  //If ESP cannot connect using saved connections then is switches to Access Point
  //Default IP for configuration is 192.168.4.1
  WiFiManager wifiManager;
  wifiManager.autoConnect("ModuleHeater-ESP");

  //Loop as long as WiFi is not connected
  WaitForConnection();
  GetConnectionData();
  SetUpServer();
}

void loop()
{
  server.handleClient();
}