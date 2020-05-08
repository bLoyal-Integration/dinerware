using Dinerware;
using Dinerware.WorkstationInterfaces;
using System;

namespace DinerwareSystem.Models
{
    public abstract class OrderEntryExtension : IOrderEntryExtension
    {
        public string Author { get; } = "I am the author";
        public string Copyright { get; } = "Copyright Me, Now";
        public Guid LicenseFeature { get; } = Guid.Empty;
        public string Name { get; } = "My Addin That I Made";
        public bool OnBrain { get; } = false;
        public bool OnWorkstation { get; } = true;
        public static string currentOpenTicketId { get; set; }
        public static Dinerware.Ticket currentOpenTicket { get; set; }
        public static string currentUserId { get; set; }

        public abstract string displayName { get; }
        public abstract void ButtonPressed(object parentForm, TicketCollection theTickets, Ticket currentTicket, User currentUser, Person currentCustomer);
    }
}
