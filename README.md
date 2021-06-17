# SignalRTest

A basic implementation for using SignalR. Also has swagger ui for development purposes
This app features:
* Client: written as a Worker Service (.Net Core)
  * Database: which uses SqLite (Entity Framework Core) and is for the client app
  * Model: which contains all the entities and business logic for the client app
* Api: written as a Asp.Net Core Web Api

## SignalR.Client
- Client can be run on a Raspberry Pi as worker service and console app

### LCD
* LCD can print any message sent to it using the POST endpoint api/raspberrypi/lcd
* Many manufacturers sell 20x4 LCD character displays with an integrated GPIO expander. The character display connects directly to the GPIO expander, which then connects to the Raspberry Pi via the Inter-Integrated Circuit (I2C) serial protocol.
* Use the raspi-config command to ensure the following two services are enabled: 
  * SSH
  * I2C

For more information on raspi-config visit https://www.raspberrypi.org/documentation/configuration/raspi-config.md

### LED
* LED can be toggled on and off using the POST endpoint api/raspberrypi/led

### DisplayCurrentTime
* LCD can print any message sent to it using the POST endpoint api/raspberrypi/displaycurrenttime

### ReceiveMessage
* LCD can print any message sent to it using the POST endpoint api/raspberrypi
