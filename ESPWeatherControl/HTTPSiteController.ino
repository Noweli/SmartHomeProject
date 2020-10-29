String SendHTMLWithSensorData(float sensorData)
{
  Serial.println("SendHTMLWithSensorData method called");
  return WrapDataInBody(String(sensorData));
}

String SendJSONWithSensorData(String fieldName, float sensorData)
{
  Serial.println("SendJSONWithSensorData method called");
  return WrapDataInJSON(fieldName, String(sensorData));
}

String WrapDataInBody(String data)
{
  Serial.print("WrapDataInBody method called with following data - ");
  Serial.println(data);

  String site = "<!DOCTYPE html> <html>\n";
  site += "<head>\n";
  site += "<body>\n";
  site += data;
  site += "\n</body>";
  site += "</head>\n";
  site += "</html>\n";

  return site;
}

String WrapDataInJSON(String fieldName, String value)
{
  String json;
  json += "{\n";
  json += "\t\"";
  json += fieldName;
  json += "\": \"";
  json += value;
  json += "\"\n";
  json += "}";

  return json;
}