String inCharSTR = ""; // Входная строка

int vibDelay = 2000;   // Время работы виброплатформы (=> вибро)
int pointCount = 14;   // Ко-во вибро

uint32_t timer[14]; // Время для каждой вибро
bool timerB[14];    // true - если работает (так-же для коректных логов)

void setup()
{
  Serial.begin(9600);
  
  for(int i = 0; i < pointCount; i++)
  {
    pinMode(i, OUTPUT);
    timer[i] = millis();
    timerB[i] = false;
  }
}

void loop()
{
  CheckData();
  CheckTime();
}

void CheckTime() // Таймер работы вибро
{
  for(int pin = 0; pin < pointCount; pin++)
  {
    // Если время закончилось, то вырубаем вибро
    if(timer[pin] < millis()) //если(время на момент включения + добавочное время < время(сейчас))
    {
      if(timerB[pin] == true)
      {
        analogWrite(pin, LOW);//Выключаем вибро
        timerB[pin] = false;
        
        Serial.println("вибро[" + String(pin) + "]-выключен");
      }
    }
  }
}

void CheckData() // Проверяем входный данны
                 // Входящая строка число_e
                 // Примеры: 12e 7e 2e 9e...
{
  while (Serial.available() > 0)
  { 
    int inChar = Serial.read();
    if (inChar == 'e') // Проверка конца команды символом 'e'
    {
      int pin = inCharSTR.toInt(); // Входящее число
      inCharSTR = "";              // Обнуляем строку для следующих команд

      analogWrite(pin,HIGH);
      timer[pin] = millis() + vibDelay; // устанавливаем время окончание работы вибро
      timerB[pin] = true;

      Serial.println("вибро[" + String(pin) + "]-включен");
    }
    else
    {
      inCharSTR += (char)inChar;
    }
  }
}
