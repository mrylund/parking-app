using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking_App_WPF
{
    class User
    {
        private string username, name, room, licenseplate, rank;

        public User(string username, string name, string room, string licenseplate, string rank)
        {
            this.username = username;
            this.name = name;
            this.room = room;
            this.licenseplate = licenseplate;
            this.rank = rank;
        }
    }
}
