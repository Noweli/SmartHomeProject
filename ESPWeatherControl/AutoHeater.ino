void CheckAndControllAutoHeater()
{
    if(!isAutoHeatEnabled){
        return;
    }

    temperature = (float)dht.readTemperature();

    if(temperature < (float)minAutoHeaterTemp && !isHeaterEnabled) {
        //Turn on heater
        Serial.println("turn on heater called");
        TurnOnHeater();
    } else if (temperature > (float)maxAutoHeaterTemp && isHeaterEnabled) {
        //Turn off heater
        Serial.println("turn off heater called");
        TurnOffHeater();
    }
}

void TurnOnHeater() 
{
    HTTPClient http;

    String url = heaterAddress;
    url += "turnOn";
    Serial.println(url);

    http.begin(url);
    int httpCode = http.GET();

    if(http.connected())
    {
        http.end();
    }

    isHeaterEnabled = true;
}

void TurnOffHeater() 
{
    HTTPClient http;

    String url = heaterAddress;
    url += "turnOff";

    http.begin(url);
    int httpCode = http.GET();

    if(http.connected())
    {
        http.end();
    }

    isHeaterEnabled = false;
}
