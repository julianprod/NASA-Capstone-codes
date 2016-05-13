#include <nRF24L01.h>
#include <RF24.h>
#include <SPI.h>

RF24 radio(48, 49);  

int message=0;   // variable to store character received through Serial to send it through radio.

const byte rxAddr[6] = "00001";  //Code to match with the radio receiver(in the robot).

void setup()
{
  Serial.begin(9600);
  radio.begin();                  //Start Radio antenna.
  radio.setRetries(15, 15);       
  radio.openWritingPipe(rxAddr);  //Open writing pipe to send characters through radio.
  radio.setChannel(120);          //Set channel to communicate with the radio receiver(in the robot).
  radio.setDataRate(RF24_250KBPS);  //Set data rate to communicate at certain KBPS speed.
  radio.setPALevel(RF24_PA_MAX);    
  
  radio.stopListening();          //Radio set to stop listening in order to send messages to the receiver.
}

void loop()
{
  char text = 'Z';              //declaration of the variable used to send characters through radio.
 
  delay(5);
  
  if(Serial.available()>0)    //check if character is received through serial port.
  {
    message=Serial.read();     //store in message variable character from serial port.
    text = message ;                   //convert message from serial into a character variable to send it through radio (int to char)
    radio.write(&text, sizeof(text));  //send character through radio to the receiver(the robot).
  
    delay(5);
  }
}
