using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking_App_WPF
{
    class User
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Room { get; set; }
        public string LicensePlate { get; set; }
        public string Rank { get; set; }

        public User(string Username, string Name, string Room, string LicensePlate, string Rank)
        {
            this.Username = Username;
            this.Name = Name;
            this.Room = Room;
            this.LicensePlate = LicensePlate;
            this.Rank = Rank;
        }
    }
}
