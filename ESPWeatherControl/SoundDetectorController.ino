void CheckForSoundDetection()
{
    if (digitalRead(SoundSensorPin) == 1)
    {
        soundDetectionDate = DateTime.now();
    }
}