using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.classes
{
    internal class Booking
    {//Purpose : create booking objects 

        //Attributes
        private string bookingId;
        private Event bookingEvent;
        private person participant;

        //constructor
        public Booking(string bookingId, Event bookingEvent, person participant)
        {
            this.bookingId = bookingId;
            this.bookingEvent = bookingEvent;
            this.participant = participant;
        }

        //methods
        public string getBookingId()
        {//Purpose : return booking ID (in type of string)
            return bookingId;
        }

        public Event getBookingEvent()
        {//Purpose : return the event of the booking (in type of 'Event')
            return bookingEvent;
        }
        public person getParticipant()
        {//Purpose : return the person who booked the event (in type of 'Person')
            return participant;
        
        }
    }
}
