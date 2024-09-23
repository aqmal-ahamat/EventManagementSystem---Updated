using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.classes
{
    public abstract class person
    {//Purpose :  Create person objects

        //attributes
        private string personId;
        private string username;
        private string password;
        private string mobileNumber;
        private string email;
        private string role;

        //constructor
        public person(string personId, string username, string password, string mobileNumber, string email, string role) 
        {
            this.personId = personId;
            this.username = username;
            this.password = password;
            this.mobileNumber = mobileNumber;
            this.email = email;
            this.role = role;
        
        
        }

        //methods


        public string getUsername()
        {//Purpose : return the username attribute 
            return username;

        }

        public string getRole()
        {//Purpose : Return the role 
            return role;
        }
        
    }
}
