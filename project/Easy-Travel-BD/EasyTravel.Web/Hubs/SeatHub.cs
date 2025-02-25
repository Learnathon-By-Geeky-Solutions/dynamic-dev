using Microsoft.AspNetCore.SignalR;

namespace EasyTravel.Web.Hubs
{
  
    public class SeatHub : Hub
    {
        private static readonly Dictionary<string, HashSet<string>> TemporarySeats = new(); // Holds temporarily selected seats
        private static readonly Dictionary<string, HashSet<string>> BookedSeats = new(); // Holds booked seats (Finalized selections)
        private static readonly Dictionary<string, string> SeatToConnectionMap = new(); // Maps seats to connection IDs

        // Select a seat
        public async Task SelectSeat(string busId, string seatNumber)
        {
            if (!TemporarySeats.ContainsKey(busId))
            {
                TemporarySeats[busId] = new HashSet<string>();
            }

            if (!TemporarySeats[busId].Contains(seatNumber) &&
                (!BookedSeats.ContainsKey(busId) || !BookedSeats[busId].Contains(seatNumber)))
            {
                // Add seat to the temporary list for the bus
                TemporarySeats[busId].Add(seatNumber);
                SeatToConnectionMap[seatNumber] = Context.ConnectionId; // Map the seat to the current user's connection

                // Notify other clients that the seat is locked for this user
                await Clients.Group(busId).SendAsync("SeatLocked", seatNumber);

                // Add user to the group and notify about temporary selection
                await Groups.AddToGroupAsync(Context.ConnectionId, busId);
                await Clients.Group(busId).SendAsync("SeatTemporarilySelected", seatNumber);
            }
        }

        // Deselect a seat
        public async Task DeselectSeat(string busId, string seatNumber)
        {
            if (TemporarySeats.ContainsKey(busId) && TemporarySeats[busId].Contains(seatNumber))
            {
                if (SeatToConnectionMap.TryGetValue(seatNumber, out var connectionId) && connectionId == Context.ConnectionId)
                {
                    // Remove seat from the temporary selected list
                    TemporarySeats[busId].Remove(seatNumber);
                    SeatToConnectionMap.Remove(seatNumber);

                    // Notify other clients that the seat is now available
                    await Clients.Group(busId).SendAsync("SeatAvailable", seatNumber);

                    // Notify that the seat was deselected
                    await Clients.Group(busId).SendAsync("SeatDeselected", seatNumber);
                }
            }
        }

        // Notify that a seat has been booked permanently (Optional - if applicable)
        public async Task BookSeat(string busId, string seatNumber)
        {
            if (!BookedSeats.ContainsKey(busId))
            {
                BookedSeats[busId] = new HashSet<string>();
            }

            if (!BookedSeats[busId].Contains(seatNumber))
            {
                BookedSeats[busId].Add(seatNumber);
                TemporarySeats[busId].Remove(seatNumber); // Remove from temporary list if it's booked

                // Notify all clients that the seat is now booked
                await Clients.Group(busId).SendAsync("SeatBooked", seatNumber);
            }
        }
    }

}

