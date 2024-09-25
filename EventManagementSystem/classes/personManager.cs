using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509.SigI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace EventManagementSystem.classes
{
    internal static class personManager
    {//Purpose :  Manage the data in the database 'person' table and manage person objects
        public static person authenticateUser(string username, string password)
        {//Purpose :  to authenticate the user who is loggin in to the system 
         //Purpose :  Create and return person object(the logged in user)

            // temporary variable to store the response from the DB
            string Username = "";
            string Password = "";
            string PersonId = "";
            string MobileNumber = "";
            string Email = "";
            string Role = "";

            
            DbConnections dbConnections = new DbConnections();

            // search for records  using the username the user enterd
            string query = $"SELECT * FROM `person` where username='{username}'";
            MySqlCommand cmd = new MySqlCommand(query, dbConnections.GetConnection());

            try
            {
                dbConnections.OpenConnection();
                MySqlDataReader reader = cmd.ExecuteReader();

                
                while (reader.Read())
                {
                    // save the response in the temporary variables.
                    Username = reader["username"].ToString();
                    Password = reader["password"].ToString();
                    PersonId = reader["personid"].ToString();
                    MobileNumber = reader["mobilenumber"].ToString();
                    Role = reader["role"].ToString();
                    Email = reader["email"].ToString();





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


            //check if the temporary variable Username is empty..if so then the record doesnt exist in the DB
            if (Username == "")
            {// username doesnt exists
                MessageBox.Show("Invalid Username");
            }
            else
            {// username does exist

                // check if the password the user entered is the password which is in the DB
                if (Password != password)
                {// passwords doesnt match
                    MessageBox.Show("Invalid Password");

                }
                else
                {//passwords do match
                 // user is valid.

                    MessageBox.Show("Login in Success !!");

                    // check the role of the user and create Person object according to the role.
                    // return the person object
                    
                    if (Role == "admin")
                    {
                        person user = new admin(PersonId, Username, Password, MobileNumber, Email);
                        return user;
                    }
                    else if (Role == "organizer")
                    {
                        person user = new organizer(PersonId, Username, Password, MobileNumber, Email);
                        return user;
                    }
                    else
                    {
                        person user = new participant(PersonId, Username, Password, MobileNumber, Email);
                        return user;
                    }


                }
            }
            // no preson object will be returned if the entered credentials are wrong.
            return null;
        }

        public static void registerUser(string username, string password, string confpassword, string email, string number, string role)
        {//Purpose :  Add new users to the Database.

            //validate user inputs
            if (InputValidator.validateRegisterInput(password, confpassword, username))
            {//inputs are valid

                DbConnections dbConnection = new DbConnections();


                try
                {
                    dbConnection.OpenConnection();


                    //insert new user's data into the 'person' table 
                    string query2 = $"INSERT INTO `person`(`username`, `password`, `role`, `mobilenumber`, `email`) VALUES ('{username}','{password}','{role}','{number}','{email}')";
                    MySqlCommand cmd2 = new MySqlCommand(query2, dbConnection.GetConnection());
                    cmd2.ExecuteNonQuery();

                    MessageBox.Show("Regististration success!! \nLogin to use the application.");

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


       


        public static List<person> getAllUsersList()
        {//Purpose : return a list of all the users (in the type of person)

            //List declaration (empty at this point)
            List<person> personList = new List<person>();


            


            DbConnections dbConnections = new DbConnections();
            // get all the records from the 'person' table
            string query = $"SELECT * FROM `person`";
            MySqlCommand cmd = new MySqlCommand(query, dbConnections.GetConnection());

            try
            {
                dbConnections.OpenConnection();
                MySqlDataReader reader = cmd.ExecuteReader();

                //read all records one at a time
                while (reader.Read())
                {
                    //get all the data in columns and save it in variables
                    string personName = reader["username"].ToString();
                    string personID = reader["personId"].ToString();
                    string personPass = reader["password"].ToString();
                    string personRole = reader["role"].ToString();
                    string personMN = reader["mobilenumber"].ToString();
                    string personEmail = reader["email"].ToString();

                    //create person object according to the role. and add it to the the personList.

                    if (personRole == "admin")
                    {
                        personList.Add(new admin(personID, personName, personPass, personMN, personEmail));
                    }
                    else if (personRole == "organizer")
                    {
                        personList.Add(new organizer(personID, personName, personPass, personMN, personEmail));
                    }
                    else 
                    {
                        personList.Add(new participant(personID, personName, personPass, personMN, personEmail));
                    }

                }
                // return person list(contains all the users )
                return personList;
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
            // if theres no users in the database return null 
            return null;
        }

        public static void changeRole(string user,string role)
        {//Purpose : change the role of a user.( to an admin or an organizer )
         // edit the role record in the database for a selected user


            DbConnections dbConnection = new DbConnections();

            try
            {
                dbConnection.OpenConnection();


               
                string query2 = $"UPDATE person SET role = '{role}' WHERE username = '{user}';";
                MySqlCommand cmd2 = new MySqlCommand(query2, dbConnection.GetConnection());
                cmd2.ExecuteNonQuery();

                MessageBox.Show($"User : {user} is now an {role}");

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
