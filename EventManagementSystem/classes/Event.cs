using EventManagementSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace EventManagementSystem.classes
{
    internal class Event
    {//Purpose :  create event objects

        //Attributes
        private string eventId;
        private string name;
        private string venue;
        private string time;
        private string date;
        private int maxParticipants;
        private int currentParticipants;
        private string organizer;
        private int tPrice;

        //Constructor
        public Event(string eventId, string name,string venue, string time, string date, int maxParticipants, int currentParticipants, string organizer,int ticketPrice) 
        {
            this.eventId = eventId;
            this.name = name;
            this.venue = venue;
            this.time = time;
            this.date = date;
            this.maxParticipants = maxParticipants;
            this.currentParticipants = currentParticipants;
            this.organizer = organizer;
            this.tPrice = ticketPrice;
        }

        //Methods

        public string getEventId()
        {//Purpose :  return event ID
            return eventId;
        }
        public string getName()
        {//Purpose :  return event name
            return name;
        }
        public string getVenue()
        {//Purpose :  return event venue
            return venue;
        }
        public string getTime()
        {//Purpose :  return the time of the event happening
            return time.Substring(0, 5);
        }
        public string getDate()
        {//Purpose :  return the event date in the format of "YYYY-MM-DD"

            string Date = date.Substring(0, 10);// the Date variable has the date in "MM/DD/YYYY" format
            int indexOfDash = Date.IndexOf("/");// gets the index of the first occurence of "/".
            int lastIndexOfDash = Date.LastIndexOf("/");// gets the index of the last occurence of "/"
            string month = Date.Substring(0, indexOfDash);// selects the month 
            string day = Date.Substring(indexOfDash+1,lastIndexOfDash-2);//selects the day

            //removes the '/' character from the day string
            if (day.Contains("/")) 
            {
                day = day.Replace("/","");
                
            }
            // removes any " " characters from the  year string
            int year =Convert.ToInt32(Date.Substring(lastIndexOfDash+1));
            
            // formats the date "YYYY-MM-DD"
            string newDate = $"{year}-{month}-{day}";
            
            

            return newDate ;
            //Reason for the date formatting : when we need to store a date in the database it must be in YYYY-MM-DD format.
            //therefore formatting the date here itself is more convenient

        }
        public int getMaxParticipants()
        {//Purpose :  return the maximum participants the event can have
            return maxParticipants;
        }
        public int getCurrentParticipants()
        {//Purpose :  return how many participants are there already.
            return currentParticipants;
        }
        public string getOrganizer()
        {//Purpose :  returns the organizer of the event
            return organizer;
        }

        public int getTicketPrice()
        {//Purpose :  returns the ticket price of the event
            return tPrice;
        }
    }
}


