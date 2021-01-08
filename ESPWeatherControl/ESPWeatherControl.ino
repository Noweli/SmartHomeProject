#include <DNSServer.h>
#include <ESP8266WebServer.h>
#include <ESP8266HTTPClient.h>
#include <WiFiManager.h>
#include <ESPDateTime.h>
#include <BH1750.h>
#include <Wire.h>
#include <i2cdetect.h>
#include "DHT.h"

#define DHTTYPE DHT11

String soundDetectionDate;
String heaterAddress;
bool isAutoHeatEnabled = false;
bool isHeaterEnabled = false;
int minAutoHeaterTemp = -99;
int maxAutoHeaterTemp = -99;
float temperature = 0;
float humidity = 0;

uint8_t HeaterPin = D5;
uint8_t DHTPin = D6;
uint8_t SoundSensorPin = D7;

ESP8266WebServer server(80);
DHT dht(DHTPin, DHTTYPE);
BH1750 lightSensor(0x23);

void setup()
{
  //Serial communication setup for logs
  Serial.begin(9600);
  delay(100);

  pinMode(HeaterPin, OUTPUT);
  pinMode(DHTPin, INPUT);
  pinMode(SoundSensorPin, INPUT);

  Wire.begin(D2, D1); //I2C bus initialization - D2 SDA, D1 SCL
  i2cdetect();  //Shows devices connected to I2C as a table in Serial output

  dht.begin();
  lightSensor.begin(BH1750::CONTINUOUS_HIGH_RES_MODE);

  //Used for WiFi connection
  //If ESP cannot connect using saved connections then is switches to Access Point
  //Default IP for configuration is 192.168.4.1
  WiFiManager wifiManager;
  wifiManager.autoConnect("Module1-ESP");

  //Loop as long as WiFi is not connected
  WaitForConnection();
  GetConnectionData();
  SetUpServer();
  SetUpDateTime();
}

void loop()
{
  server.handleClient();

  CheckForSoundDetection();
  CheckAndControllAutoHeater();
}