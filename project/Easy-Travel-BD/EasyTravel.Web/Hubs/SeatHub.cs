using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace EasyTravel.Web.Hubs
{
    public class SeatHub : Hub
    {
        public async Task JoinBusGroup(string busId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, busId);
        }

        public async Task SeatSelected(string busId, string seatNumber, string seatId)
        {
            await Clients.OthersInGroup(busId).SendAsync("SeatStatusChanged", seatNumber, seatId, "selected");
        }

        public async Task SeatUnselected(string busId, string seatNumber, string seatId)
        {
            await Clients.OthersInGroup(busId).SendAsync("SeatStatusChanged", seatNumber, seatId, "available");
        }
    }
}