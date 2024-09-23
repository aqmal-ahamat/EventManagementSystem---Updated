using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EventManagementSystem
{
    internal static class InputValidator
    {//Purpose : Check for any empty fields
     //return true if there are empty fields. return false if there are no empty fields


        public static bool Validate(params string[]fields) 
        {//take a variable amount of arguements(the text of each input field) as save it in parameter array 'fields'
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
    }
}
