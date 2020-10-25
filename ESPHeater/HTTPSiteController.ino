String WrapDataInBody(String data){
  Serial.print("WrapDataInBody method called with following data - ");
  Serial.println(data);
  
  String site = "<!DOCTYPE html> <html>\n";
  site +="<head>\n";
  site +="<body>\n";
  site += data;
  site +="\n</body>";
  site +="</head>\n";
  site +="</html>\n";

  return site;
}