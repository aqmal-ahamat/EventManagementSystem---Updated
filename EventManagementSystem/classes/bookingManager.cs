using EventManagementSystem;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace EventManagementSystem.classes
{
    internal static class bookingManager
    {//Purpose :  Manage the data in the database 'booking' table and manage booking objects
        public static void registerEvent(Event ev, person participant)
        {//Purpose :  Create bookings in the database
         // takes 2 parameters event(type of Event) and participant(type of person)

            
            string eventName = ev.getName();
            string participantName = participant.getUsername();

            //validate the booking (check if the booking is already there in the database)
            if (InputValidator.checkBookingDoesNotExist(participantName, eventName))
            {// booking is not there.

                //check if the event is overbooked 
                //logic : current participants count should be lower than maximum participant count
                if (ev.getCurrentParticipants() < ev.getMaxParticipants())
                {
                    //event is not overbooked

                    
                    

                        DbConnections dbConnection = new DbConnections();
                        try
                        {
                            dbConnection.OpenConnection();


                            //add booking to the database.
                            string query2 = $"INSERT INTO `booking`( `participant`, `event`) VALUES ('{participantName}','{eventName}')";
                            MySqlCommand cmd2 = new MySqlCommand(query2, dbConnection.GetConnection());
                            cmd2.ExecuteNonQuery();

                            eventManager.updateCurrentParticipants(ev);

                            MessageBox.Show("Booking added succesfully..!");



                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred: " + ex.Message);
                        }
                        finally
                        {
                            dbConnection.CloseConnection();
                        }
                    
                }
                else
                {
                    //event is overbooked
                    MessageBox.Show("Selected event is Overbooked! ");
                }
            }



        }


        public static void unregisterEvent(Event ev, person participant)
        {//Purpose :  Delete bookings from the database
         // takes 2 parameters event(type of Event) and participant(type of person)

            string eventName = ev.getName();
            string participantName = participant.getUsername();

            //validate the booking (check if the booking is not already there in the database)
            if (InputValidator.checkBookingExist(participantName, eventName))
            {//booking is there in the database


                DbConnections dbConnection = new DbConnections();
                try
                {
                    dbConnection.OpenConnection();


                    // delete the booking from the database
                    string query2 = $"DELETE FROM `booking` WHERE participant='{participantName}' and event='{eventName}';";
                    MySqlCommand cmd2 = new MySqlCommand(query2, dbConnection.GetConnection());
                    cmd2.ExecuteNonQuery();

                    eventManager.updateCurrentParticipants(ev);

                    MessageBox.Show("Booking removed succesfully..!");



                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
                finally
                {
                    dbConnection.CloseConnection();
                }
            }



        }

        



        
        public static List<Event> getAllMyEventsList(person user)
        {// Purpose : return a list of all the events a 'users' has registerd/Booked.

            //empty list to store the events
            List<Event> eventList = new List<Event>();


            


            DbConnections dbConnections = new DbConnections();

            //get all the booking user made from the 'booking' table
            //using that get records details for each event 

            //final result : Records about all the events a user booked.
            string query = $"SELECT event.* FROM event JOIN booking ON event.name = booking.event WHERE booking.participant = '{user.getUsername()}' ORDER BY event.date ASC, event.time ASC;";
            MySqlCommand cmd = new MySqlCommand(query, dbConnections.GetConnection());

            try
            {
                dbConnections.OpenConnection();
                MySqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    //store data about the event in variables
                    string EventId = reader["eventId"].ToString();
                    string Name = reader["name"].ToString();
                    string Venue = reader["venue"].ToString();
                    string Time = reader["time"].ToString();
                    string Date = reader["date"].ToString();
                    int MaxParticipants = Convert.ToInt32(reader["maxParticipants"].ToString());
                    int CurrentParticipants = Convert.ToInt32(reader["currentParticipants"].ToString());
                    string Organizer = reader["organizer"].ToString();
                    int Price = Convert.ToInt32(reader["price"].ToString());

                    //create event object and add it to the eventList.
                    eventList.Add(new Event(EventId, Name, Venue, Time, Date, MaxParticipants, CurrentParticipants, Organizer, Price));

                }
                //return a list of events a given user Booked.
                return eventList;
                reader.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);

            }
            finally
            {
                dbConnections.CloseConnection();
            }
            return null;
        }

        public static List<Booking> getAllBookingList()
        {// Purpose : return a list of all the bookings in the 'booking' table (item type of Booking)

            //empty List to store all the bookings
            List<Booking> bookingList = new List<Booking>();

            //List(type of Event) to store all the events in the DB
            List<Event> allEventList = eventManager.getAllEventsList();

            //List(type of Person) to store all the users in the DB
            List<person> allUsersList = personManager.getAllUsersList();

            //create an empty list to store all event names
            List<string> allEventNames = new List<string>();

            //iterate through all the events in allEventslist
            foreach(Event ev in allEventList)
            {
                //get the name of the event and add it to allEventNames list
                allEventNames.Add(ev.getName());
            }

            //create an empty list to store every user's username
            List<string> allUserNames = new List<string>();

            //iterate through all the users in allUsersList
            foreach (person p in allUsersList)
            {
                //get the usernameof the user and add it to allUserNames list
                allUserNames.Add(p.getUsername());
            }
            


            DbConnections dbConnections = new DbConnections();

            //select all the records from the 'booking' table
            string query = $"SELECT * FROM `booking`";
            MySqlCommand cmd = new MySqlCommand(query, dbConnections.GetConnection());

            try
            {
                dbConnections.OpenConnection();
                MySqlDataReader reader = cmd.ExecuteReader();

                //iterate through each record
                while (reader.Read())
                {


                    //read the bookingID of the booking
                    string BookingID = reader["bookingID"].ToString();

                    //read the Participant Name of the booking
                    string PARTICIPANT = reader["participant"].ToString();

                    //read the eventName of the booking
                    string EVENT = reader["event"].ToString();



                    //Task : select the Event object from allEventList and select the Person object from allUsersList to create the Booking object
                    //(because creating a Booking object requires paremeters of 'event' in Event object type and 'participant' in Person object type)

                    //Logic :the index of a Name in the names list = index of the object in the object list
                    //(logic is valid because both the namesList and objectsList store thier items in the same orther. therefore same indexes)

                    //get the index of eventName from allEventNames list
                    int IndexOftheEvent = allEventNames.IndexOf(EVENT);

                    //Using the index of eventName we cn get the Object of that event  from allEventList
                    Event bookedEvent = allEventList[IndexOftheEvent];


                    //get the index of Participant Name from allUserNames list
                    int IndexOftheParticipant = allUserNames.IndexOf(PARTICIPANT);

                    //Using the index of participant Name we cn get the Object of that Participant  from allEventList
                    person bookedUser = allUsersList[IndexOftheParticipant];


                   //create the Booking objects using the event object and person object and bookingID
                   //add the Booking object to the bookingList
                   bookingList.Add(new Booking(BookingID,bookedEvent,bookedUser));














                }
                //return bookinglist with all the bookings 
                return bookingList;
                reader.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);

            }
            finally
            {
                dbConnections.CloseConnection();
            }
            return null;
        }



    }

    


}




