#define LOOP_DELAY      5   
#define SENDING_DELAY   50  
#define LAMBDA          0.2   // smoothing parameter < 1 

// set pin numbers:
const int buttonPin = 2;     // the number of the pushbutton pin

// variables will change:
int buttonState = 0;         // variable for reading the pushbutton status

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
  pinMode(buttonPin, INPUT);

  // initialize serial
  Serial.begin(9600);
}

void loop() {
  // read the state of the pushbutton value:
  buttonState = digitalRead(buttonPin);

  unsigned long current_time = millis();

  if (buttonState) {
    cnt++;
  } else {
    // for debug
    //Serial.println("button not pressed");
  }
  
  if (current_time - data_timer > SENDING_DELAY) {
    // calculate velocity 
    calcVelo();
    
    // invoke sendData per 50 ms
    sendData();

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

void sendData() {
  Serial.println(velocity);
}

