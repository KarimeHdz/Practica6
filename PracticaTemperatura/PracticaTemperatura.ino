// Define el pin en el que está conectado el LM35
#define LM35PIN A0 // Pin analógico donde está conectado el LM35

void setup() {
  Serial.begin(9600);  // Inicializa la comunicación serie
}

void loop() {
  // Lee el valor analógico del LM35
  int rawValue = analogRead(LM35PIN); // Lee el valor del pin analógico

  // Convierte el valor analógico a voltaje (0-5V) y luego a grados Celsius
  float voltage = rawValue * (5.0 / 1023.0); // Convierte el valor a voltaje
  float tempC = voltage * 100; // Convierte el voltaje a temperatura en Celsius

  // Enviar la temperatura en formato "T:XX.XX"
  Serial.print("T:");
  Serial.println(tempC, 2); // Envía la temperatura con 2 decimales

  // Espera 2 segundos antes de la siguiente lectura
  delay(2000);
}
