//*Polytechnic University of Puerto Rico.
//*Computer Engineering Capstone Project 2015-2016.
//*Auralee Alvarez
//*Gerardo Fernandez
//*Julian Peña
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//*"Hydroponic Crop on Mars performed by an autonomous programmed robot" project.  This program allows the user to control the robot without been autonomous.

#include <SPI.h>       //SPI library to communicate with radio antennae
#include <nRF24L01.h>  //Library for the radio antennae.
#include <RF24.h>      //Second library for the radio antennae.
#include <Servo.h>     //Library for the servo.

RF24 radio(48, 49);
const int SPD1 = 12;  //green 11,  39  //6 MAIN WHEELLS SPEED
const int DIR1 = 40;  //darkgreen 10,  41  //1-3 MAIN WHEELS DIRECTION
const int DIR2 = 28; // yellow          //4-6 MAIN WHEELS DIRECTION


Servo ser1;
int pos;        //variable to set the angle of the servo inside a for loop.

const int motoraspd = 4;   //declaration of motor A speed(arm's wrist)
const int motoradir = 26;  //declaration of motor A direction

const int motorbspd = 3;   //declaration of motor B speed(arm's elbow)
const int motorbdir = 24;  //declaration of motor B direction

const int motorcdspd = 2;  //declaration of motors C & D speed(arm's shoulder)
const int motorcddir = 22;  //declaration of motors C & D direction.



int b = 0;  //1. MEANS THAT ROBOT IS CONTROLLED BY KINECT. 2.MEANS THAT ROBOT IS CONTROLLED BY BUTTONS.
int ledtest=5;  //variable to turn on and off the led to test radio signal.
int vartest = 0; //variable to let code know if led is already on.

const int motor7spd = 9;      //orange motor 7 speed REAR LEFT WHEEL STEERING
const int motor7dir = 32;    // blue motor 7 direction

const int motor8spd = 10;    // yellow motor 8 speed FRONT LEFT WHEEL STEERING
const int motor8dir = 34;    // green motor 8 direction

const int motor9spd = 7;    //blue motor 9 speed  FRONT RIGHT WHEEL STEERING
const int motor9dir = 36;   // brown motor 9 direction

const int motor10spd = 6;    //blue motor 10 speed REAR RIGHT WHEEL STEERING
const int motor10dir = 38;   //purple motor 10 direction

int encoder7 = 0;//ENCODER ON MOTOR 7 PIN 20,
int encoder8 = 0;//ENCODER ON MOTOR 8 PIN 21,
int encoder9 = 0;//ENCODER ON MOTOR 9 PIN 18 
int encoder10 = 0;//ENCODER ON MOTOR 10 PIN 19


int wstatus = 0; // Actual Position of the wheels.(0.all wheels are straight, 1.forwardright,  2.forwardleft, 3.backright, 4.backleft, 5.360right, 6.360left)
int astatus = 0; //Actual Position of the arm (0.arm not stretched, 1. arm is stretched)

void count7(void);  //declaration of functions to manage encoder values.
void count8(void);
void count9(void);
void count10(void);

const byte rxAddr[6] = "00001";  //variable to match with the variable of the radio transmitter.

///////////////////////////////////////////////////SETUP///////////////////////////////////////////////////////////////
void setup() {

  //Serial.begin(9600);

  ser1.attach(8);
  ser1.writeMicroseconds(1500);
  
  pinMode(21,INPUT);
  pinMode(20,INPUT);
  pinMode(19,INPUT);
  pinMode(18,INPUT);

  pinMode(ledtest, OUTPUT);

  pinMode(motoraspd, OUTPUT);
    pinMode(motoradir, OUTPUT);
    pinMode(motorbspd, OUTPUT);
    pinMode(motorbdir, OUTPUT);
    pinMode(motorcdspd, OUTPUT);
    pinMode(motorcddir, OUTPUT);

  pinMode(motor7spd, OUTPUT);
  pinMode(motor7dir, OUTPUT);
  pinMode(motor8spd, OUTPUT);
  pinMode(motor8dir, OUTPUT);
  pinMode(motor9spd, OUTPUT);
  pinMode(motor9dir, OUTPUT);
  pinMode(motor10spd, OUTPUT);
  pinMode(motor10dir, OUTPUT);
  
  pinMode(SPD1, OUTPUT);
  pinMode(DIR1, OUTPUT);
  pinMode(DIR2, OUTPUT);
  // pinMode(IC, OUTPUT);
  // pinMode(ID, OUTPUT);

  radio.begin();
  radio.openReadingPipe(0, rxAddr);
  radio.setChannel(120);

  radio.setDataRate(RF24_250KBPS);
  radio.setPALevel(RF24_PA_MAX);

  radio.printDetails();

  radio.startListening();
  

  attachInterrupt(3,count7,FALLING);  //FOR ENCODER 7
  attachInterrupt(2,count8,FALLING);  //FOR ENCODER 8
  attachInterrupt(5,count9,FALLING);  //FOR ENCODER 9
  attachInterrupt(4,count10,FALLING); //FOR ENCODER 10

  encoder7 = 0;
  encoder8 = 0;
  encoder9 = 0;
  encoder10 = 0;

  wstatus = 0;

}

//////////////////////////////////////////////////////////////////////FUNCTIONS FOR WHEELS////////////////////////////////////////////////

void putwheelsstraight() //ALIGN ALL WHEELS TOGETHER FUNCTION
{
    Stop();
  
   encoder7 = 0; 
   encoder8 = 0;
   encoder9 = 0;
   encoder10 = 0;
   delay(280);
   
  if(wstatus==1 || wstatus==3)  //Wheels are in forwardright position or in backright position.
  { 
  do
  {
    if(encoder8<=120)
    {
    analogWrite(motor8spd, 70);
    digitalWrite(motor8dir, 0);
    }else{ analogWrite(motor8spd, 0);}

    if(encoder7<=120)
    {
    analogWrite(motor7spd, 70);
    digitalWrite(motor7dir, 1);
    }else{analogWrite(motor7spd, 0);}

  }while(encoder8<=120 || encoder7<=120);
  analogWrite(motor8spd, 0);
  analogWrite(motor7spd, 0);

  do
  {
    if(encoder9<=120)
    {
    analogWrite(motor9spd, 70);
    digitalWrite(motor9dir, 0);
    }else{analogWrite(motor9spd, 0);}
    
    
    if(encoder10<=120)
    {
     analogWrite(motor10spd, 70);
    digitalWrite(motor10dir,1);
    }else{analogWrite(motor10spd, 0);}

    
  }while(encoder9<=120 || encoder10<=120);
  analogWrite(motor9spd, 0);
  analogWrite(motor10spd, 0);
 
  }


  if(wstatus==2 || wstatus==4) //Wheels are in forwardleft position or in backleft position.
  {
    do
  {
    if(encoder8<=110)
    {
    analogWrite(motor8spd, 70);
    digitalWrite(motor8dir, 1);
    }else{analogWrite(motor8spd, 0);}

    if(encoder7<=110)
    {
    analogWrite(motor7spd, 70);
    digitalWrite(motor7dir, 0);
    }else{analogWrite(motor7spd, 0);}

  }while(encoder8<=110 || encoder7<=110);
    analogWrite(motor8spd, 0);
    analogWrite(motor7spd, 0);

    do
    {
    
    if(encoder9<=110)
    {
    analogWrite(motor9spd, 70);
    digitalWrite(motor9dir, 1);
    }else{analogWrite(motor9spd, 0);}

    if(encoder10<=110)
    {
    analogWrite(motor10spd, 70);
    digitalWrite(motor10dir,0);
    }else{analogWrite(motor10spd, 0);}
    
  }while(encoder9<=110 || encoder10<=110);
  analogWrite(motor9spd, 0);
  analogWrite(motor10spd, 0);
  
  }

    if(wstatus==5 || wstatus==6) //Wheels are in 360 position.
  {
    do
  {
    if(encoder8<=150)
    {
    analogWrite(motor8spd, 70);
    digitalWrite(motor8dir, 0);
    }else{analogWrite(motor8spd, 0);}

    if(encoder7<=150)
    {
    analogWrite(motor7spd, 70);
    digitalWrite(motor7dir, 1);
    }else{analogWrite(motor7spd, 0);}

  }while(encoder8<=150 || encoder7<=150);
    analogWrite(motor8spd, 0);
    analogWrite(motor7spd, 0);

    do
    {
    if(encoder9<=150)
    {
    analogWrite(motor9spd, 70);
    digitalWrite(motor9dir, 1);
    }else{analogWrite(motor9spd, 0);}

    if(encoder10<=150)
    {
    analogWrite(motor10spd, 70);
    digitalWrite(motor10dir,0);
    }else{analogWrite(motor10spd, 0);}
    
  }while(encoder9<=150 || encoder10<=150);
  analogWrite(motor9spd, 0);
  analogWrite(motor10spd, 0);
  
  }
  
  wstatus = 0;
  delay(280);

}

void forwardstr() //FORWARD STRAIGHT FUNCTION
{
  Stop();

  if(!wstatus==0)
  {
  putwheelsstraight();
  }

  if(wstatus==0)
  {
  forward();
  }
}

void backstr() //BACKWARD STRAIGHT FUNCTION
{
  Stop();

  if(!wstatus==0)
  {
  putwheelsstraight();
  }

  if(wstatus==0)
  {
   back(); 
  }

}

void forwardright()  //FORWARD TO THE RIGHT FUNCTION
{ 
  
  Stop(); 

  if(wstatus==1 || wstatus==3)
  {
    forward();
  }else
  {
  if(wstatus==0) 
  {
    
  encoder7 = 0;  
  encoder8 = 0;
  encoder9 = 0;
  encoder10 = 0;
   
   do
  {

    if(encoder8<=120)
    {
    analogWrite(motor8spd, 70);
    digitalWrite(motor8dir, 1);
    }else{analogWrite(motor8spd, 0);}

    if(encoder7<=120)
    {
    analogWrite(motor7spd, 70);
    digitalWrite(motor7dir, 0);
    }else{analogWrite(motor7spd, 0);}

  }while(encoder8<=120 || encoder7<=120);
  analogWrite(motor8spd, 0);
  analogWrite(motor7spd, 0);

  do
  {
    if(encoder9<=120)
    {
    analogWrite(motor9spd, 70);
    digitalWrite(motor9dir, 1);
    }else{ analogWrite(motor9spd, 0);}

    if(encoder10<=120)
    {
    analogWrite(motor10spd, 70);
    digitalWrite(motor10dir,0);
    }else{analogWrite(motor10spd, 0);}
    
    }while(encoder9<=120 || encoder10<=120);
    analogWrite(motor9spd, 0);
    analogWrite(motor10spd, 0);

  wstatus=1;
  
  forward();
  }else
    {
      putwheelsstraight();
      forwardright();
    }}
}

void forwardleft() //FORWARD TO THE LEFT FUNCTION
{
  Stop();

  if(wstatus==2 || wstatus==4)
  {
    forward();
  }else
  {
  if(wstatus==0)
  {

  
  encoder7 = 0;  
  encoder8 = 0;
  encoder9 = 0;
  encoder10 = 0;
   
  do
  {
    if(encoder8<=110)
    {
    analogWrite(motor8spd, 70);
    digitalWrite(motor8dir, 0);
    }else{ analogWrite(motor8spd, 0);}

    if(encoder7<=110)
    {
    analogWrite(motor7spd, 70);
    digitalWrite(motor7dir, 1);
    }else{ analogWrite(motor7spd, 0);}

  }while(encoder8<=110 || encoder7<=110);
    analogWrite(motor8spd, 0);
    analogWrite(motor7spd, 0);

    do
    {
     if(encoder9<=110)
    {
    analogWrite(motor9spd, 70);
    digitalWrite(motor9dir, 0);
    }else{analogWrite(motor9spd, 0);}
      
    if(encoder10<=110)
    {
     analogWrite(motor10spd, 70);
    digitalWrite(motor10dir,1);
    }else{analogWrite(motor10spd, 0);}

    
   }while(encoder9<=110 || encoder10<=110);
   analogWrite(motor10spd, 0);
   analogWrite(motor9spd, 0);
   
  
  wstatus=2;
  
  forward();
  }else
    {
      putwheelsstraight();
      
      forwardleft();
    }}
  
    
}

void backright()  //BACKWARDS TO THE RIGHT
{
  Stop();
  if(wstatus==3 || wstatus==1)
  {
    back();
  }else
  {
    if(wstatus==0)
  {
    
  
  encoder7=0;  
  encoder8=0;
  encoder9=0;
  encoder10=0;
   
  do
  {
    if(encoder8<120)
    {
    analogWrite(motor8spd, 70);
    digitalWrite(motor8dir, 1);
    }else{analogWrite(motor8spd, 0);}

    if(encoder7<120)
    {
    analogWrite(motor7spd, 70);
    digitalWrite(motor7dir, 0);
    }else{analogWrite(motor7spd, 0);}
    
  }while(encoder8<=120 || encoder7<=120);
    analogWrite(motor8spd, 0);
    analogWrite(motor7spd, 0);

  do
  {
    if(encoder9<120)
    {
    analogWrite(motor9spd, 70);
    digitalWrite(motor9dir, 1);
    }else{analogWrite(motor9spd, 0);}

    if(encoder10<120)
    {
    analogWrite(motor10spd, 70);
    digitalWrite(motor10dir, 0);
    }else{analogWrite(motor10spd, 0);}
    
  }while(encoder9<=120 || encoder10<=120);
  analogWrite(motor9spd, 0);
  analogWrite(motor10spd, 0);
  
  wstatus=3;
  
  back();
  }else
  {
    putwheelsstraight();
    backright();
  }}
  
}

void backleft() //BACKWARDS TO THE LEFT
{
  Stop();
  if(wstatus==4 || wstatus==2)
  {
    back();
  }else
  {
    if(wstatus==0)
  {
    
  
  encoder7=0;  
  encoder8=0;
  encoder9=0;
  encoder10=0;
   
  do
  {
    if(encoder8<120)
    {
    analogWrite(motor8spd, 70);
    digitalWrite(motor8dir, 0);
    }else{analogWrite(motor8spd, 0);}

    if(encoder7<120)
    {
    analogWrite(motor7spd, 70);
    digitalWrite(motor7dir, 1);
    }else{analogWrite(motor7spd, 0);}
    
  }while(encoder8<=120 || encoder7<=120);
    analogWrite(motor8spd, 0);
    analogWrite(motor7spd, 0);

  do
  {
    if(encoder9<120)
    {
    analogWrite(motor9spd, 70);
    digitalWrite(motor9dir, 0);
    }else{analogWrite(motor9spd, 0);}

    if(encoder10<120)
    {
    analogWrite(motor10spd, 70);
    digitalWrite(motor10dir, 1);
    }else{analogWrite(motor10spd, 0);}
    
  }while(encoder9<=120 || encoder10<=120);
  analogWrite(motor9spd, 0);
  analogWrite(motor10spd, 0);
  
  wstatus=4;
  
  back();
  }else
  {
    putwheelsstraight();
    backleft();
  }}
  
  
}

void right360() //TURNS RIGHT IN 360
{
  Stop();
  
  if(wstatus==5 || wstatus==6)
  {
    analogWrite(SPD1, 120);
    digitalWrite(DIR1, 0);
    digitalWrite(DIR2, 1);
  }else
  {
    if(wstatus==0)
    {
      encoder7 = 0;
      encoder8 = 0;
      encoder9 = 0;
      encoder10 = 0;

    do
    {
     if(encoder8<150)
    {
    analogWrite(motor8spd, 70);
    digitalWrite(motor8dir, 1);
    }else{ analogWrite(motor8spd, 0);}

    if(encoder7<150)
    {
    analogWrite(motor7spd, 70);
    digitalWrite(motor7dir, 0);
    }else{ analogWrite(motor7spd, 0);}

  }while(encoder8<=150 || encoder7<=150);
    analogWrite(motor8spd, 0);
    analogWrite(motor7spd, 0);

    do
    {
    if(encoder9<150)
    {
    analogWrite(motor9spd, 70);
    digitalWrite(motor9dir, 0);
    }else{analogWrite(motor9spd, 0);}

    if(encoder10<150)
    {
     analogWrite(motor10spd, 70);
    digitalWrite(motor10dir,1);
    }else{analogWrite(motor10spd, 0);}
    
   }while(encoder9<=150 || encoder10<=150);
   analogWrite(motor10spd, 0);
    analogWrite(motor9spd, 0); 
    
  wstatus=5;
  
  analogWrite(SPD1, 120);
  digitalWrite(DIR1, 0);
  digitalWrite(DIR2, 1);
  }else
    {
      putwheelsstraight();
      
      right360();
    }}
}

void left360() //TURNS LEFT IN 360
{
  Stop();
  
  if(wstatus==6 || wstatus==5)
  {
    analogWrite(SPD1, 120);
    digitalWrite(DIR1, 1);
    digitalWrite(DIR2, 0);
  }else
  {
    if(wstatus==0)
    {
      encoder7 = 0;
      encoder8 = 0;
      encoder9 = 0;
      encoder10 = 0;

    do
    {
     if(encoder8<150)
    {
    analogWrite(motor8spd, 70);
    digitalWrite(motor8dir, 1);
    }else{ analogWrite(motor8spd, 0);}

    if(encoder7<150)
    {
    analogWrite(motor7spd, 70);
    digitalWrite(motor7dir, 0);
    }else{ analogWrite(motor7spd, 0);}

  }while(encoder8<=150 || encoder7<=150);
    analogWrite(motor8spd, 0);
    analogWrite(motor7spd, 0);

    do
    {
    if(encoder9<150)
    {
    analogWrite(motor9spd, 70);
    digitalWrite(motor9dir, 0);
    }else{analogWrite(motor9spd, 0);}

    if(encoder10<150)
    {
     analogWrite(motor10spd, 70);
    digitalWrite(motor10dir,1);
    }else{analogWrite(motor10spd, 0);}
    
   }while(encoder9<=150 || encoder10<=150);
   analogWrite(motor10spd, 0);
    analogWrite(motor9spd, 0); 
    
  wstatus=6;
  
  analogWrite(SPD1, 120);
  digitalWrite(DIR1, 1);
  digitalWrite(DIR2, 0);
  }else
    {
      putwheelsstraight();
      
      left360();
    }}
 
}

void t360()    ///  PUT WHEELS IN 360 POSITION
{
  Stop();
  
  if(wstatus==5 || wstatus==6)
  {
    analogWrite(SPD1, 0);
    digitalWrite(DIR1, 0);
    digitalWrite(DIR2, 0);
  }else
  {
    if(wstatus==0)
    {
      encoder7 = 0;
      encoder8 = 0;
      encoder9 = 0;
      encoder10 = 0;

    do
    {
     if(encoder8<150)
    {
    analogWrite(motor8spd, 70);
    digitalWrite(motor8dir, 1);
    }else{ analogWrite(motor8spd, 0);}

    if(encoder7<150)
    {
    analogWrite(motor7spd, 70);
    digitalWrite(motor7dir, 0);
    }else{ analogWrite(motor7spd, 0);}

  }while(encoder8<=150 || encoder7<=150);
    analogWrite(motor8spd, 0);
    analogWrite(motor7spd, 0);

    do
    {
    if(encoder9<150)
    {
    analogWrite(motor9spd, 70);
    digitalWrite(motor9dir, 0);
    }else{analogWrite(motor9spd, 0);}

    if(encoder10<150)
    {
     analogWrite(motor10spd, 70);
    digitalWrite(motor10dir,1);
    }else{analogWrite(motor10spd, 0);}
    
   }while(encoder9<=150 || encoder10<=150);
   analogWrite(motor10spd, 0);
    analogWrite(motor9spd, 0); 
    
  wstatus=5;
  
  analogWrite(SPD1, 0);
  digitalWrite(DIR1, 0);
  digitalWrite(DIR2, 0);
  }else
    {
      putwheelsstraight();
      
      t360();
    }}
}

void forward()  //ALL WHEELS FORWARD
{
  analogWrite(SPD1, 140);
  digitalWrite(DIR1, 0);
  digitalWrite(DIR2, 0);
}

void back()   //ALL WHEELS BACKWARD
{
   analogWrite(SPD1, 140);
  digitalWrite(DIR1, 1);
  digitalWrite(DIR2, 1);
}
  
void Stop()  //ALL WHEELS STOP
{
  digitalWrite(SPD1, LOW);
  //digitalWrite(DIR1, 0);
  //digitalWrite(DIRB, 0);
  //digitalWrite(motor7spd, LOW);
  //digitalWrite(motor8spd, LOW);
  //digitalWrite(motor9spd, LOW);
  //digitalWrite(motor10spd, LOW);
}

///////////////////////////////////////////////////////FUNCTIONS FOR ARM//////////////////////////////////////////////////////////////

void astop()  //ARM MOTORS STOP
{
  analogWrite(motoraspd, 0);
  analogWrite(motorbspd, 0);
  analogWrite(motorcdspd, 0);
  
}

void sup() //SHOULDER UP
{
  analogWrite(motorcdspd, 100);
  digitalWrite(motorcddir, 0);
}

void sdown() //SHOULDER DOWN
{
  analogWrite(motorcdspd, 100);
  digitalWrite(motorcddir, 1);
}

void eup()  //ELBOW UP
{
  analogWrite(motorbspd,100);
  digitalWrite(motorbdir, 0);
}

void edown()  //ELBOW DOWN
{
  analogWrite(motorbspd, 100);
  digitalWrite(motorbdir, 1);
}

void wup()  //WRIST UP
{
  analogWrite(motoraspd, 40);
  digitalWrite(motoradir, 0);
}

void wdown() //WRIST DOWN
{
  analogWrite(motoraspd, 40);
  digitalWrite(motoradir, 1);
}

void gclose()  //CLOSE THE GRIPPER
{
  ser1.attach(8);
  
  for(pos = 0; pos <= 110; pos += 1)
 {
  ser1.write(pos);
  delay(15);
 }
 delay(250);
 ser1.detach();
 
}

void gopen() //OPEN THE GRIPPER
{
  ser1.attach(8);
  
  for(pos = 0; pos <= 96; pos += 1)
 {
  ser1.write(pos);
  delay(15);
 }
 delay(200);
 ser1.detach();
}

void Sout()
{
   sdown();
   eup();
}

void Sin()
{
  sup();
  edown();
}

////////////////////////////////////////////////////////////LOOP///////////////////////////////////////////////////////////////////////////

void loop() {
  
  Stop();
  

  
  
  if(radio.available())
  {
    char text = {0};
    radio.read(&text, sizeof(text));

    if(text=='Z')
    {
      if(vartest==1)
      {
      digitalWrite(ledtest,LOW);
      vartest=0;
      }
      else{digitalWrite(ledtest,HIGH); vartest=1;}
     
      delay(5);    
    }
    
    if(text=='A')
    {
      forwardstr();
      delay(250);
    }
    
    if(text=='B')
    {
      backstr();
      delay(350);
    }

    if(text=='C')
    {
      forwardright();
      delay(250);
    }

    if(text=='D')
    {
      forwardleft();
      delay(250);
    }

    if(text=='E')
    {
      backright();
      delay(250);
    }

    if(text=='F')
    {
      backleft();
      delay(250);
    }

    if(text=='G')
    {
      t360();
    }

    if(text=='H')
    {
      right360();
      delay(250);
    }

    if(text=='I')
    {
      left360();
      delay(250);
    }

    if(text=='J')
    {
      putwheelsstraight();
      
    }

    if(text=='K')
    {
      sup();
      delay(50);
    }

    if(text=='L')
    {
      sdown();
      delay(50);
    }

    if(text=='M')
    {
      eup();
      delay(50);
    }

    if(text=='N')
    {
      edown();
      delay(50);
    }

    if(text=='O')
    {
      wup();
      delay(50);
    }

    if(text=='P')
    {
      wdown();
      delay(50);
    }

    if(text=='Q')
    {
      gopen();
      astop();
    }

    if(text=='R')
    {
      gclose();
      astop();
    }

    if(text=='S')
    {
      Sout();
      delay(50);
    }

    if(text=='T')
    {
      Sin();
      delay(50);
    }
    
    
    if(text=='a')
    {
      Stop();
    }

   delay(5);
  }else
  {
    Stop();
    astop();
  }
  
 

}

/////////////////////////////////////////////////////////////////COUNT FUNCTIONS FOR INTERRUPTS ON ENCODERS///////////////////////////////////////////////////////////////////////////////////

void count7()
{
  encoder7++;
}

void count8()
{
  encoder8++;
}

void count9()
{
  encoder9++;
}

void count10()
{
  encoder10++;
}


