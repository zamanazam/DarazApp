using System.ComponentModel;
using System.Reflection.Emit;

namespace DarazApp.Services
{
    class Program
    {
        public enum RolesEnum
        {
            Buyer = 1,Store = 2 , Admin=3 , SuperAdmin=4 , Guest=5
        }
        public enum ShippingStatus
        {
            Pending = 1, Waiting = 2, Shipped = 3, Delivered =4, Cancelled = 5
        }
        public enum ProductRating
        {
            A = 1, B = 2, C = 3, D=4, E = 5, 
        }
        public enum MessageStatus 
        {
            Read = 1, UnRead = 2
        }
        public enum RoomStatus 
        {
            UnAssigned=1, Assigned=2
        }

        public enum ResponseStatus 
        {
            Yes = 1, No =2
        }
        public enum PasswordStatus 
        {
            NoPassword = 33331212        
        }
    }
}
