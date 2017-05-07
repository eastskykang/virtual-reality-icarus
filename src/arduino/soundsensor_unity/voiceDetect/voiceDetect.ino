int snd_threshold = 100;
int soundInputPin = 0;

boolean detectSound();
boolean voiceflag;

void setup()
{
  Serial.begin(9600); // open serial port, set the baud rate to 9600 bps
}
void loop()
{
      voiceflag = detectSound();
      delay(100);
}

boolean detectSound()
{
  
    int soundlevel;
    boolean voiceflag;
    
    val = analogRead(soundInputPin);   //connect mic sensor to Analog 0
      
      if(soundlevel > snd_threshold){
        voiceflag = true;
        Serial.println(soundlevel,DEC);//print the sound value to serial        
        return voiceflag;
    }
    
}
