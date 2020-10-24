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