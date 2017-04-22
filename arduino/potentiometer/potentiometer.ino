// These constants won't change.  They're used to give names
// to the pins used:
const int analogInPin = A0;  // Analog input pin that the potentiometer is attached to
const int analogOutPin = 9; // Analog output pin that the LED is attached to

int sensorValue = 0;        // value read from the pot
int outputValue = 0;        // value output to the PWM (analog out)

void setup() {
  // initialize serial communications at 9600 bps:
  Serial.begin(9600);
}

void loop() {

  // 0-511: left (-90~0)
  // 512-1023: right(0~+90)
  // read the analog in value:
  sensorValue = analogRead(analogInPin);
/*  // map it to the range of the analog out:
  outputValue = map(sensorValue, 0, 1023, 0, 255);
  // change the analog out value:
  analogWrite(analogOutPin, outputValue);
*/
  // print the results to the serial monitor:
  //Serial.print("sensor = ");
  Serial.flush();
  Serial.print(sensorValue);
  Serial.println();
 
  // wait 2 milliseconds before the next loop
  // for the analog-to-digital converter to settle
  // after the last reading:
  delay(50);
}
