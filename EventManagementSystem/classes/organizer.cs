using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.classes
{
    internal class organizer : person
    {//Purpose :  Create objects in the type of person withe organizer role.

        // constructor
        public organizer(string personId, string username, string password, string mobileNumber, string email ) : base(personId, username, password, mobileNumber, email, "organizer")
        { 
        }
    }
}
