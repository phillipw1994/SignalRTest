using System;
using System.Device.Gpio;
using System.Device.I2c;
using System.Threading;
using System.Threading.Tasks;
using Iot.Device.CharacterLcd;
using Iot.Device.Pcx857x;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalR.Client.Core
{
    public class HubConnectionManager : IHubConnectionManager
    {
        private HubConnection Connection { get; }

        public HubConnectionManager(IHubConnectionBuilder hubConnectionBuilder)
        {
            Connection = hubConnectionBuilder.Build();
        }

        public void RegisterEndPoints(CancellationToken stoppingToken)
        {
            Connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000, stoppingToken);
                await Connection.StartAsync(stoppingToken);
            };

            Connection.On<string, string>("ReceiveMessage", async (user, message) =>
            {
                try
                {
                    var newMessage = $"{user}: {message}";
                    Console.WriteLine(newMessage);
                    await Connection.SendAsync("SendMessage", "MessageReceived", cancellationToken: stoppingToken);
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, "An error occurred while trying to process ReceiveMessage from SignalR connection");
                }
            });

            Connection.On<bool>("LEDToggle", (ledOn) =>
            {
                try
                {
                    Console.WriteLine("LED Toggle");
                    const int pin = 18;
                    using var controller = new GpioController();
                    controller.OpenPin(pin, PinMode.Output);
                    controller.Write(pin, ledOn ? PinValue.High : PinValue.Low);
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, "An error occurred while trying to process LEDToggle from SignalR connection");
                }
            });

            Connection.On("DisplayCurrentTime", () =>
            {
                try
                {
                    Console.WriteLine("Display Current Time");
                    using var i2c = I2cDevice.Create(new I2cConnectionSettings(1, 0x27));
                    using var driver = new Pcf8574(i2c);
                    using var lcd = new Lcd2004(registerSelectPin: 0,
                        enablePin: 2,
                        dataPins: new int[] { 4, 5, 6, 7 },
                        backlightPin: 3,
                        backlightBrightness: 0.1f,
                        readWritePin: 1,
                        controller: new GpioController(PinNumberingScheme.Logical, driver));
                    lcd.Clear();
                    lcd.SetCursorPosition(0, 0);
                    lcd.Write(DateTime.Now.ToShortTimeString());
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, "An error occurred while trying to process DisplayCurrentTime from SignalR connection");
                }
            });

            Connection.On<string>("LcdMessage", (message) =>
            {
                try
                {
                    Console.WriteLine("Lcd Message");
                    using var i2c = I2cDevice.Create(new I2cConnectionSettings(1, 0x27));
                    using var driver = new Pcf8574(i2c);
                    using var lcd = new Lcd2004(registerSelectPin: 0,
                        enablePin: 2,
                        dataPins: new int[] { 4, 5, 6, 7 },
                        backlightPin: 3,
                        backlightBrightness: 0.1f,
                        readWritePin: 1,
                        controller: new GpioController(PinNumberingScheme.Logical, driver));
                    lcd.Clear();
                    lcd.SetCursorPosition(0, 0);
                    lcd.Write(message);
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, "An error occurred while trying to process LcdMessage from SignalR connection");
                }
            });
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            await Connection.StartAsync(stoppingToken);
        }

        public async Task JoinGroupAsync(CancellationToken stoppingToken)
        {
            await Connection.SendAsync("JoinGroup", "Devices", cancellationToken: stoppingToken);
        }

        public async Task LeaveGroupAsync(CancellationToken stoppingToken)
        {
            await Connection.SendAsync("LeaveGroup", "Devices", stoppingToken);
        }

        public async Task StopAsync(CancellationToken stoppingToken)
        {
            await Connection.StopAsync(stoppingToken);
        }
    }
}