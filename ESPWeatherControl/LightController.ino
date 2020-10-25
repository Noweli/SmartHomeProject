void ControllTemperature()
{
    if (temperature < 27.0 && !digitalRead(LightPin))
    {
        digitalWrite(LightPin, HIGH);
    }
    else
    {
        if (digitalRead(LightPin))
        {
            digitalWrite(LightPin, LOW);
        }
    }
}