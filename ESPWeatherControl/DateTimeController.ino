void SetUpDateTime()
{
    DateTime.setServer("ntp1.tp.pl");
    DateTime.setTimeZone(2);
    DateTime.begin();
    if (!DateTime.isTimeValid())
    {
        Serial.println("Failed to get time from server.");
    }
    else
    {
        Serial.print("DateTime.now(): ");
        Serial.println(DateTime.now());
        Serial.print("DateTime.getTime(): ");
        Serial.println(DateTime.getTime());
        Serial.print("DateTime.utcTime(): ");
        Serial.println(DateTime.utcTime());
        Serial.print("DateTime.getTimeZone(): ");
        Serial.println(DateTime.getTimeZone());
        soundDetectionDate = DateTime.format(DateFormatter::HTTP);
    }
}