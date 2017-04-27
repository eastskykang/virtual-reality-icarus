# include <Arduino.h>

// pins
const int analogInPin = A0;   // Analog input pin that the potentiometer is attached to
const int analogOutPin = 9;   // Analog output pin that the LED is attached to

int sensorValue = 0;          // value read from the pot
int outputValue = 0;          // value output to the PWM (analog out)

void setup() {
  // initialize serial communications at 9600 bps:
  Serial.begin(9600);
}

void loop() {

  // sensorValue
  // 0    - 511   : left    -90 to  0
  // 512  - 1023  : right   0   to  +90
  sensorValue = analogRead(analogInPin);

  Serial.flush();
  Serial.print(sensorValue);
  Serial.println();

  // do not change
  delay(50);
}
