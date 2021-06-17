using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Api.Web.Hubs
{
    public class DeviceHub : Hub
    {
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessage(string message)
        {
            //Call all clients
            await Clients.Group("WebApps").SendAsync("ReceiveMessage", message);

            //Call a group of clients
            //Used to send a broadcast to all clients that belong to a group
            //Multiple Groups
            //await Clients.Groups(new List<string>{"Test Group", "Test Group 2"})
            //    .SendAsync("ReceiveMessage", user, message);
           
            //Single Group
            //await Clients.Group("Test Group")
            //    .SendAsync("ReceiveMessage", user, message);

            //Call a list of users clients
            //Used to send a broadcast to all clients that are logged in with userId
            //Multiple Users
            //await Clients.Users("", "").SendAsync("ReceiveMessage", user, message);
            
            //Single User using userUid or userId
            //await Clients.User("").SendAsync("ReceiveMessage", user, message);

            //Call a list of clients
            //Used to send a broadcast to specific clients
            //Multiple Clients
            //await Clients.Clients("", "").SendAsync("ReceiveMessage", user, message);
            
            //Single Client
            //await Clients.Client("").SendAsync("ReceiveMessage", user, message);
        }
    }

}