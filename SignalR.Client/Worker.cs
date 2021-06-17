using System;
using System.Device.Gpio;
using System.Device.I2c;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iot.Device.CharacterLcd;
using Iot.Device.Pcx857x;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SignalR.Client.ApiClient.Authorization.Managers;
using SignalR.Client.Core.Management;
using SignalR.Client.Core.Management.Interface;
using SignalR.Database;
using SignalR.Database.Helpers;
using SignalR.Model.Configuration;
using SignalR.Model.Db;

namespace SignalR.Client
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public IClientCredentialManagerAsync<ClientCredential> ClientCredentialManager { get; }

        private HubConnection Connection { get; }

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            IConfigurationSettings configurationSettings = new JsonConfigurationSettings(configuration);

            using ISignalRDbContext context = new SignalRDbContext(DatabaseHelpers.CreateOptionsBuilder<SignalRDbContext>(configurationSettings.ConnectionString("DefaultConnection")).Options);
            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();

            ClientCredentialManager = new ClientCredentialManagerAsync<ClientCredential>(
                new SystemAuthenticatorAsync(configurationSettings.ConnectionInfo), configurationSettings,
                configurationSettings.ConnectionString("DefaultConnection"), "");

            Connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44397/devicehub", options =>
                {
                    //options.AccessTokenProvider = async () =>
                    //{
                    //    await ClientCredentialManager.CheckTokenExpirationAsync(CancellationToken.None);
                    //    return await ClientCredentialManager.GetAccessTokenAsync(CancellationToken.None);
                    //    //Task.FromResult(_myAccessToken)
                    //};
                })
                .WithAutomaticReconnect()
                .Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000, stoppingToken);
                await Connection.StartAsync(stoppingToken);
            };

            Connection.On<string, string>("ReceiveMessage", async (user, message) =>
            {
                var newMessage = $"{user}: {message}";
                Console.WriteLine(newMessage);
                await Connection.SendAsync("SendMessage", "MessageReceived", cancellationToken: stoppingToken);
            });

            Connection.On<bool>("LEDToggle", (ledOn) =>
            {
                Console.WriteLine("LED Toggle");
                const int pin = 18;
                using var controller = new GpioController();
                controller.OpenPin(pin, PinMode.Output);
                controller.Write(pin, ledOn ? PinValue.High : PinValue.Low);
            });

            Connection.On("DisplayCurrentTime", () =>
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
            });

            Connection.On<string>("LcdMessage", (message) =>
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
            });

            try
            {
                await Connection.StartAsync(stoppingToken);
                Console.WriteLine("Connection started");
                await Connection.SendAsync("JoinGroup", "Devices", cancellationToken: stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }

            var cancellationToken = new CancellationToken();
            await Connection.SendAsync("LeaveGroup", "Devices", cancellationToken);
            await Connection.StopAsync(cancellationToken);
        }
    }
}
