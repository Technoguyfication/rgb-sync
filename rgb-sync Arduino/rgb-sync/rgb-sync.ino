void setup()
{
   pinMode(D0, OUTPUT);
   pinMode(D1, OUTPUT);
   pinMode(D2, OUTPUT);

   digitalWrite(D0, HIGH);
   digitalWrite(D1, HIGH);
   digitalWrite(D2, HIGH);

   Serial.begin(115200);
}

void loop()
{
  if (Serial.available() > 2)
  {
    analogWrite(D0, Serial.read());
    analogWrite(D1, Serial.read());
    analogWrite(D2, Serial.read());
  }
}
