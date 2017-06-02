#define LOOP_DELAY      2   
#define SENDING_DELAY   50  
#define LAMBDA          0.2   // smoothing parameter < 1 

#define ANGLE_PREFIX      "AN::"
#define VELOCITY_PREFIX   "VE::"
#define SOUND_PREFIX      "DB::"

// set pin numbers:
const int buttonPin = 2;       // calibration button pin 
const int switchPin = 3;      // lead switch pin
const int potentPin = A1;     // analog input pin for potentiomenter 
const int soundPin = A2;      // analog input pin for soundsensor

// variables will change:
int buttonState = 0;          // variable for reading the button status
int switchState = 0;          // variable for reading the lead switch status
int potentValue = 0;          // value read from the pot
int soundLevel = 0;           // value from sound sensor

int switchStateLast = 0;      // switch value of t-1

// timers
unsigned long data_timer;
unsigned long velo_timer;

// variable for velocity
unsigned int  cnt;
unsigned long velocity;

// variable for zero calibration
int zeropoint = 512;

void setup() {

  // initialize timer
  data_timer = 0;
  velo_timer = 0;

  // initialize velocity (cycle per second)
  cnt = 0;
  velocity = 0;
  
  // initialize the pushbutton pin as an input:
  pinMode(switchPin, INPUT);

  // initialize serial
  Serial.begin(9600);
}

void loop() {

  // read the state of the calibration button value
  buttonState = digitalRead(buttonPin);
  
  // read the state of the lead switch value
  switchState = digitalRead(switchPin);
  
  // potentValue
  // 0 - 1023
  potentValue = analogRead(potentPin);

  // soundLevel 
  soundLevel = analogRead(soundPin);

  unsigned long current_time = millis();

  if (switchState == 1 && switchStateLast == 0) {
    cnt++;
  } else {
    // for debug
    //Serial.println("button not pressed");
  }

  if (buttonState) {
    // calibration angle 
    caliZero();
  }
  
  if (current_time - data_timer > SENDING_DELAY) {
    // calculate velocity 
    calcVelo();
    
    // invoke sendVelData per 50 ms
    sendVelData();

    // invoke sendPotData per 50 ms
    sendPotData();

    // invoke sendSndData per 50ms
    sendSndData();

    // update data_timer
    data_timer = current_time;
  }
  
  // adjust delay
  delay(LOOP_DELAY);

  // last state update
  switchStateLast = switchState;
}

void calcVelo() {
  velocity = cnt;
//  velocity = 1000 * LAMBDA * cnt / SENDING_DELAY + (1 - LAMBDA) * velocity;
  cnt = 0;
}

void caliZero() {
  zeropoint = potentValue;
}

void sendVelData() {
  Serial.println(VELOCITY_PREFIX + String(velocity));
}

void sendPotData() {
  Serial.println(ANGLE_PREFIX + String(potentValue - zeropoint));
}

void sendSndData() {
  Serial.println(SOUND_PREFIX + String(soundLevel));
}
