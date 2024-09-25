using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Data;
using Mysqlx.Crud;
using Google.Protobuf.WellKnownTypes;
using System.Runtime.InteropServices.WindowsRuntime;

namespace EventManagementSystem.classes
{
    internal static  class eventManager
    {//Purpose :  Manage the data in the database 'event' table and manage event objects
        public static List<Event> getAllEventsList()
        {//Purpose :  get a list of all the events ( in the type of Event)

            //empty list to store items
            List<Event> eventList = new List<Event>();


              
            DbConnections dbConnections = new DbConnections();

            //get all the event records from the database
            string query = $"SELECT * FROM `event` ORDER BY date ASC, time ASC;";
            MySqlCommand cmd = new MySqlCommand(query, dbConnections.GetConnection());

            try
            {
                dbConnections.OpenConnection();
                MySqlDataReader reader = cmd.ExecuteReader();

                
                while (reader.Read())
                {
                    //get values from the DB and store them in variables
                    string EventId = reader["eventId"].ToString();
                    string Name = reader["name"].ToString();
                    string Venue = reader["venue"].ToString();
                    string Time = reader["time"].ToString();
                    string Date = reader["date"].ToString();
                    int MaxParticipants = Convert.ToInt32( reader["maxParticipants"].ToString());
                    int CurrentParticipants = Convert.ToInt32(reader["currentParticipants"].ToString());
                    string Organizer = reader["organizer"].ToString();
                    int Tprice = Convert.ToInt32(reader["price"].ToString());

                    //create event object and add it to the event list
                    eventList.Add(new Event(EventId,Name,Venue,Time,Date,MaxParticipants,CurrentParticipants,Organizer,Tprice));
                    
                    












                }
                //return event list with all the events 
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
            //return null if there are no events in the DB
            return null;
        }

        public static void createEvent(string name, string venue, string time, string date, string maxparticipant,person organizer,string price)
        {//Purpose :  Add an event to the 'event' table in the DB 

            //validate the event name user entered
            if (InputValidator.validateEventName(name))
            {
                // event name is valid

                // add event record to the database

                DbConnections dbConnection = new DbConnections();
                try
                {
                    dbConnection.OpenConnection();



                    string query2 = $"INSERT INTO `event`(`name`, `venue`, `time`, `date`, `maxParticipants`, `organizer`,`price`) VALUES ('{name}','{venue}','{time}','{date}','{maxparticipant}','{organizer.getUsername()}','{price}')";
                    MySqlCommand cmd2 = new MySqlCommand(query2, dbConnection.GetConnection());
                    cmd2.ExecuteNonQuery();

                    MessageBox.Show("Event successfully created..");
                    
                    
                    

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

        public static void deleteEvent(Event ev)
        {//Purpose :  delete an event record from the DB
         // take event object as a parameter



            DbConnections dbConnection = new DbConnections();
                try
                {
                    dbConnection.OpenConnection();


                    //delete the event from the DB using event ID 
                    string query2 = $"DELETE FROM event WHERE eventid = {ev.getEventId()}";
                    MySqlCommand cmd2 = new MySqlCommand(query2, dbConnection.GetConnection());
                    cmd2.ExecuteNonQuery();

                    MessageBox.Show("Event successfully deleted..");
                    


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

        public static void editEvent(Event ev,string name,string venue,string time,string date,string maxParticipants,string price)
        {//Purpose :  Edit details about an event.
         //update the records in the database according to the changes user made.
         // take the event to change as an event object.
         // take the changes to make as strings.

            //saving the event id.
            //useful identify which event to update in the DB
            string evId = ev.getEventId();

            //store the changes user made to the event in variables
            string evName = name;
            string evVenue = venue;
            string evTime = time;
            string evdate = date;
            string evMaxP = maxParticipants;
            string evPrice = price;
            

            //Logic : if the change user made is an empty string , which means the user did not change that attribute of the event
            //if no changes has made to an attribute. the new value of that attribute is the current value 
            

            if (name == "")
            {// user did not change the event name

                //setting the new value for event name same as the current event name
                evName = ev.getName();
            }
            if (venue== "")
            {// user did not change the venue

                //setting the new value for venue same as the current venue
                evVenue = ev.getVenue();
            }
            if (time == "0:0")
            {// user did not change the time

                //setting the new value for time same as the current time
                evTime = ev.getTime();
            }
            if (date == "0-0-0")
            {// user did not change the date

                //setting the new value for date same as the current date
                evdate = ev.getDate();
                
            }
            if (maxParticipants == "0")
            {// user did not change the maximum participant count

                //setting the new value for Maxparcipant count same as the max participant count
                evMaxP = $"{ev.getMaxParticipants()}";
            }
            if(price == "")
            {// user did not change the  price

                //setting the new value for price same as the current price
                evPrice = $"{ev.getTicketPrice()}";
            }
            
            
            


            DbConnections dbConnection = new DbConnections();
            try
            {
                dbConnection.OpenConnection();

                //update the record for the selected event with the new values.
                string query2 = $"UPDATE event SET name='{evName}', venue='{evVenue}', time='{evTime}', date='{evdate}', maxParticipants='{evMaxP}',price={evPrice} WHERE eventID = '{evId}';";
                MySqlCommand cmd2 = new MySqlCommand(query2, dbConnection.GetConnection());
                cmd2.ExecuteNonQuery();

                MessageBox.Show("Event successfully Updated..");

               



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

        public static List<string> getAllEventNamesList()
        {// Purpose : return a list of All the event names in the DB

            // get a list of all the events in the DB(item type of Event)
            List<Event> allEvents = eventManager.getAllEventsList();

            // a List to store only the event Names(item type of string)
            List<string> allEventsNames = new List<string>();

            //iterate through each item in the allEvents list
            foreach (Event e in allEvents)
            {
                // get the event name for that event and add it to the allEventNames list 
                allEventsNames.Add(e.getName());
            }
            //return the list (has the names of all events)
            return allEventsNames;  
        }

        

        public static void updateCurrentParticipants(Event ev)
        {//Puspose : Update the column 'current participants' in the 'event' table
         //Method is useful when when a booking is made or when a booking is removed.

            //variable to store the participant count 
            int participantCount = 0;

            DbConnections dbConnections = new DbConnections();

            //select all the rows from 'booking' table for the selected event.
            string query = $"SELECT * FROM `booking` WHERE event='{ev.getName()}';";
            MySqlCommand cmd = new MySqlCommand(query, dbConnections.GetConnection());

            try
            {
                dbConnections.OpenConnection();
                MySqlDataReader reader = cmd.ExecuteReader();


                //Logic : the number of rows for a selected event in the 'booking'table  = how many bookings has made for that event = the number of participants

                // count the number of rows. and update the participant count variable
                while (reader.Read())
                {
                    participantCount += 1;
                    

                }
                
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





            DbConnections dbConnection = new DbConnections();
            try
            {
                dbConnection.OpenConnection();

                //update the 'currentParticipant' column in the booking table for the event
                string query2 = $"UPDATE event SET currentParticipants={participantCount} WHERE eventId = '{ev.getEventId()}';";
                MySqlCommand cmd2 = new MySqlCommand(query2, dbConnection.GetConnection());
                cmd2.ExecuteNonQuery();

                





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
}

