#define LOOP_DELAY      5   
#define SENDING_DELAY   50  
#define LAMBDA          0.2   // smoothing parameter < 1 

#define ANGLE_PREFIX      "AN::"
#define VELOCITY_PREFIX   "VE::"
#define SOUND_PREFIX      "DB::"

// set pin numbers:
const int switchPin = 2;      // pushbutton pin
const int potentPin = A0;     // analog intput pin 

// variables will change:
int switchState = 0;          // variable for reading the pushbutton status
int potentValue = 0;          // value read from the pot

// timers
unsigned long data_timer;
unsigned long velo_timer;

// variable for velocity
unsigned int  cnt;
unsigned long velocity;

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
  // read the state of the pushbutton value:
  switchState = digitalRead(switchPin);
  
  // potentValue
  // 0    - 511   : left    -90 to  0
  // 512  - 1023  : right   0   to  +90
  potentValue = analogRead(potentPin);

  unsigned long current_time = millis();

  if (switchState) {
    cnt++;
  } else {
    // for debug
    //Serial.println("button not pressed");
  }
  
  if (current_time - data_timer > SENDING_DELAY) {
    // calculate velocity 
    calcVelo();
    
    // invoke sendVelData per 50 ms
    sendVelData();

    // invoke sendPotData per 50 ms
    sendPotData();

    // update data_timer
    data_timer = current_time;
  }
  
  // adjust delay
  delay(LOOP_DELAY);
}

void calcVelo() {
  velocity = 1000 * LAMBDA * cnt / SENDING_DELAY + (1 - LAMBDA) * velocity;
  cnt = 0;
}

void sendVelData() {
  Serial.println(VELOCITY_PREFIX + String(velocity));
}

void sendPotData() {
  Serial.println(ANGLE_PREFIX + String(potentValue));
}
