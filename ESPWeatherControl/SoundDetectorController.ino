void CheckForSoundDetection()
{
    if (digitalRead(SoundSensorPin) == 1)
    {
        soundDetectionDate = DateTime.format("%d %b %Y %H:%M:%S");
    }
}