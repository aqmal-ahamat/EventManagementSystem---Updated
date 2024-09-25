using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EventManagementSystem
{
    internal static class InputValidator
    {


        public static bool Validate(params string[]fields)
        {//Purpose : Check for any empty fields
         //return true if there are empty fields. return false if there are no empty fields
         //take a variable amount of arguements(the text of each input field) as save it in parameter array 'fields'
            bool emptyFields = false;

            // check if atleast one input text is empty
            foreach (string field in fields) 
            {
                if (field == "")
                {// an empty field 
                    emptyFields = true;
                    
                }
            
            }
            if (emptyFields)
            {

                MessageBox.Show("Please fill all fields");
            }
            return emptyFields;

        }



        public static bool CheckString(string input)
        {// Purpose : to check if an input has atleast one alphabatic letter
            bool containsString = false;
            //iterate through each character of the input
            foreach(char character in input)
            {
                //if atleast one chatacter is a letter return true
                if (char.IsLetter(character))
                {
                    containsString = true;
                }
            }
            //if the input doesnt contain any letters return false
            return containsString;
        }


        public static bool validateRegisterInput(string password, string confpassword, string username)
        {//Purpose :  Validate inputs when a new user is registering
         // return true if the inputs are valid. return false if the inputs are not valid.

            //temporary variable to store if the username already exists in the database
            bool usernameValid = false;

            DbConnections dbConnections5 = new DbConnections();
            string query5 = $"SELECT * FROM `person` where username='{username}'";
            MySqlCommand cmd5 = new MySqlCommand(query5, dbConnections5.GetConnection());

            try
            {
                dbConnections5.OpenConnection();
                MySqlDataReader reader5 = cmd5.ExecuteReader();

                //check if theres any records for the entered username
                if (!reader5.HasRows)
                {
                    //username doesnt exist
                    //username is valid

                    usernameValid = true;




                }

                reader5.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);

            }
            finally
            {
                dbConnections5.CloseConnection();
            }




            if (usernameValid)
            {
                // username is valid

                // check if the password and confirm password fields match
                if (password == confpassword)
                {// fields do match


                    return true;



                }
                else
                {//fields doesnt match

                    MessageBox.Show("Passwords do not match");
                    return false;
                }
            }
            else
            {//username is not valid
                MessageBox.Show("Username already exists..\nPlease Login..");
                return false;
            }



        }

        public static bool validateEventName(string name)
        {//Purpose :  check if event the event name is valid when creating a new event
         //return true if the event name is valid and return false if the event name is not valid

            bool nameValid = false;

            DbConnections dbConnections5 = new DbConnections();
            // get all the records in the DB with the event name user entered
            string query5 = $"SELECT * FROM `event` where name='{name}'";
            MySqlCommand cmd5 = new MySqlCommand(query5, dbConnections5.GetConnection());

            try
            {
                dbConnections5.OpenConnection();
                MySqlDataReader reader5 = cmd5.ExecuteReader();

                //check if there are any records
                if (!reader5.HasRows)
                {//doesnt have any records with the event name
                 // event name valid

                    nameValid = true;




                }
                else
                {//there are records with the event name
                 // event name is not valid
                    MessageBox.Show("Event name already Exists..");
                }

                reader5.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);

            }
            finally
            {
                dbConnections5.CloseConnection();
            }
            return nameValid;

        }

        public static bool checkBookingDoesNotExist(string participant, string ev)
        {//Purpose :  check if the booking does not exists in the database
         //returns true if the booking doesnt exist in the database
         //returns false if the booking is there in the database

            bool nameValid = false;

            DbConnections dbConnections5 = new DbConnections();

            //get booking records for the booking
            string query5 = $"SELECT * FROM `booking` where participant='{participant}' and event='{ev}'";
            MySqlCommand cmd5 = new MySqlCommand(query5, dbConnections5.GetConnection());

            try
            {
                dbConnections5.OpenConnection();
                MySqlDataReader reader5 = cmd5.ExecuteReader();

                //check if theres any records
                if (!reader5.HasRows)
                {//theres no records 

                    //booking is valid

                    nameValid = true;




                }
                else
                {//there are records
                 // booking is not valid
                    MessageBox.Show("Booking  already Exists..");
                }

                reader5.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);

            }
            finally
            {
                dbConnections5.CloseConnection();
            }
            return nameValid;

        }

        public static bool checkBookingExist(string participant, string ev)
        {//Purpose :  check if the booking exists in the database
         //returns true if the booking  exist in the database
         //returns false if the booking  is not there in the database
            bool hasRows = false;

            //get booking records for the booking
            DbConnections dbConnections5 = new DbConnections();
            string query5 = $"SELECT * FROM `booking` where participant='{participant}' and event='{ev}'";
            MySqlCommand cmd5 = new MySqlCommand(query5, dbConnections5.GetConnection());

            try
            {
                dbConnections5.OpenConnection();
                MySqlDataReader reader5 = cmd5.ExecuteReader();

                //check if theres any records
                if (reader5.HasRows)
                {//there are records


                    hasRows = true;




                }
                else
                {
                    // there are no records

                    MessageBox.Show("Cannot delete a booking which does not exists.");


                }

                reader5.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);

            }
            finally
            {
                dbConnections5.CloseConnection();
            }
            return hasRows;

        }

    }

}
