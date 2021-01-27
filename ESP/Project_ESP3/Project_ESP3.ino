// Libraries importen
#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <Adafruit_NeoPixel.h>
#include <ArduinoJson.h>

// WIFI variabelen
//const char* ssid = "telenet-Dcc"; // Enter your WiFi name
//const char* password =  ""; // Enter WiFi password

const char* ssid = "Robin's 4G"; // Enter your WiFi name
const char* password =  "12345678"; // Enter WiFi password

// MQTT variabelen
const char* mqttServer = "13.81.105.139";
const int mqttPort = 1883;
const char* mqttUser = "test2";
const char* mqttPassword = "test";

// pinnen declareren
int adc_val = 1000; // Analoge pin value


// to char variabelen
const int numcharsplayer = 11;
char textarrayplayername[numcharsplayer];

const int numcharsjson = 210;
char textarrayjson[numcharsjson];

// player variabels
String playerName = "";

boolean Parkourstop = false;

unsigned long Time;

WiFiClient espClient;
PubSubClient client(espClient);
Adafruit_NeoPixel strip = Adafruit_NeoPixel(4, 13, NEO_RGB + NEO_KHZ800);

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);

  // setup WIFI
  WiFi.begin(ssid, password);
  while ((!(WiFi.status() == WL_CONNECTED))){
    delay(300);
    Serial.println(".");
  }
  Serial.println("Connected to the WiFi network");
  

  // setup MQTT
  client.setServer(mqttServer, 1883);
  client.setCallback(callback);


  // MQTT Messages
  //sendMessage();
  recieveMessage();

  //RGB STRIP
  strip.begin();
  strip.show();

  // Show visual connected to WiFI
  setwhite();
}

String x = "";
void callback(char* topic, byte* payload, unsigned int length) {
  // reset van variabelen
  x = "";
 
  Serial.print("Message arrived in topic: ");
  Serial.println(topic);
 
  Serial.print("Message:");
  for (int i = 0; i < length; i++) {
    Serial.print((char)payload[i]);
    x = x + (char)payload[i];
    playerName = playerName + (char)payload[i];
  }
 
  Serial.println();
  Serial.println("-----------------------");


  if (String(topic) == "ESP/3/Parkour"){
    if(!Parkourstop){
      StartParkour();
    }
    Parkourstop = false;
  }

  if (String(topic) == "ESP/3/TimeRace"){
    //if(!Parkourstop){
      //StartParkour();
    //}
    //Parkourstop = false;

    StartTimeRace();
  }

  
    
}

void loop() {
  client.loop();
}

void StartParkour(){
  // hier komt code voor gamemode "Parkour"
  const char* Parkour_score;
  int old_adc_val = 1000;
  
  setgreen();
  delay(1000);
  for (int i = 0; i <= 10; i++) {
    adc_val = analogRead(A0);
     Serial.println(adc_val);
    if (adc_val > old_adc_val){
      old_adc_val = adc_val;
    }
    delay(1000);
  }

  if (old_adc_val < 1024){
    setorange();
    for (int i = 0; i <= 10; i++) {
      adc_val = analogRead(A0);
       Serial.println(adc_val);
      if (adc_val > old_adc_val){
        old_adc_val = adc_val;
      }
      delay(1000);
    }

    if (old_adc_val < 1024){
      setred();
    }else
    {
      Parkour_score = "2";
    }
  }else
  {
    Parkour_score = "3";
  }

  Serial.println("u heeft " + String(Parkour_score) + " Gescoord");
  setwhite();

  //sendMessage("ESP/3/Parkour", Parkour_score);

   playerName.toCharArray(textarrayplayername, numcharsplayer);
  //sendMessage("ESP/test", textarrayplayername);
  
  sendMessage("ESP/1/Parkour", textarrayplayername);

  StaticJsonBuffer<200> jsonBuffer;
  JsonObject& root = jsonBuffer.createObject();
  root["name"] = textarrayplayername;
  root["points"] = Parkour_score;

  String jsonmessage;
  root.printTo(jsonmessage);

  jsonmessage.toCharArray(textarrayjson, numcharsjson);
  sendMessage("ESP/Parkour/ToDB", textarrayjson);

  playerName = "";

  for (int i = 0; i < numcharsplayer; i++) {
    textarrayplayername[i] = char(0);
  }
    
  recieveMessage();
}

void StartTimeRace(){
  // hier komt code voor gamemode "Survival"
  setgreen();
  do {
        //else
    //{
      delay(1000);
      Serial.println("checking pressure");
      adc_val = analogRead(A0);
      Serial.println(adc_val);
    //}
  } while (adc_val < 1000);

  Serial.println("Sending time");
  playerName.toCharArray(textarrayplayername, numcharsplayer);
  
  StaticJsonBuffer<200> jsonBuffer;
  JsonObject& root = jsonBuffer.createObject();
  root["name"] = textarrayplayername;
  root["isFinished"] = 1;
  root["totalTime"] = 0;

  String TimeRaceResult;
  root.printTo(TimeRaceResult);
  Serial.println(TimeRaceResult);

  TimeRaceResult.toCharArray(textarrayjson, numcharsjson);
  sendMessage("ESP/TimeRace/ToDB", textarrayjson);
  setred();
  recieveMessage();
  delay(5000);
  setwhite();


  playerName = "";

  for (int i = 0; i < numcharsplayer; i++) {
    textarrayplayername[i] = char(0);
  }

}

void sendMessage(const char* topic, const char* message){
    Serial.println("Connecting to MQTT...");
    if (client.connect("ESP8266Client2", mqttUser, mqttPassword )) {
      Serial.println("connected"); 
      // client.publish("/test", "Pressure detected"); //Topic name
      client.publish(topic, message); //Topic name
    } 
    else 
    {
      Serial.print("failed with state ");
      Serial.print(client.state());
      delay(2000);
    }
}

void recieveMessage(){
    Serial.println("Connecting to MQTT...");
    if (client.connect("ESP8266Client3", mqttUser, mqttPassword )) {
      Serial.println("connected"); 
      client.subscribe("ESP/3/Parkour");
      client.subscribe("ESP/3/TimeRace");
      client.subscribe("ESP/GameMode/Stop");
    } 
    else 
    {
      Serial.print("failed with state ");
      Serial.print(client.state());
      delay(2000);
    }
}


void setgreen(){
  strip.setPixelColor(0, 0, 255, 0); // 0 255 0 = GREEN // 255 0 0 = RED // 255 50 0 = ORANJE
  strip.setPixelColor(1, 0, 255, 0);
  strip.setPixelColor(2, 0, 255, 0);
  strip.setPixelColor(3, 0, 255, 0);
  strip.show();
}

void setred(){
  strip.setPixelColor(0,255, 0, 0); 
  strip.setPixelColor(1,255, 0, 0);
  strip.setPixelColor(2,255, 0, 0);
  strip.setPixelColor(3,255, 0, 0);
  strip.show();
}

void setorange(){
  strip.setPixelColor(0,255, 50, 0);
  strip.setPixelColor(1,255, 50, 0);
  strip.setPixelColor(2,255, 50, 0);
  strip.setPixelColor(3,255, 50, 0);
  strip.show();
}

void setwhite(){
  strip.setPixelColor(0,255, 255, 255);
  strip.setPixelColor(1,255, 255, 255);
  strip.setPixelColor(2,255, 255, 255);
  strip.setPixelColor(3,255, 255, 255);
  strip.show();
}
void SetColorReset(){
  strip.setPixelColor(0,0, 0, 0);
  strip.setPixelColor(1,0, 0, 0);
  strip.setPixelColor(2,0, 0, 0);
  strip.setPixelColor(3,0, 0, 0);
  strip.show();
}
