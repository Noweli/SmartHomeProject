void SetUpDateTime()
{
    DateTime.setTimeZone(8);
    DateTime.begin();
    if (!DateTime.isTimeValid())
    {
        Serial.println("Failed to get time from server.");
    }
    else
    {
        soundDetectionDate = DateTime.now();
    }
}