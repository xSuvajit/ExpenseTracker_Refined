using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker_Sln
{    
    public class User
    {
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }        
        public User() { }
        public User(string _name, DateTime _dob,string _address, 
            string _gender, string _email, string _password)
        {
            this.Name = _name;
            this.DOB = _dob;
            this.Address = _address;
            this.Gender = _gender;
            this.Email = _email;
            this.Password = _password;
        }
    }

    public class expence_Details
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public float Amount { get; set; }
        public string MyRemarks { get; set; }
        public ExpenceCatagory expenceCatagory { get; set; }
        public expence_Details() { }
        public expence_Details(int _id, string _name, DateTime _dob, float _amount,
            ExpenceCatagory _expCat, string _remrks)
        {
            this.ID = _id;
            this.Name = _name;
            this.Date = _dob;
            this.Amount = _amount;
            this.expenceCatagory = _expCat;
            this.MyRemarks = _remrks;
        }
    }

    public enum ExpenceCatagory
    {
        Credit = 1,
        Debit = 2
    }

    //for storing all user level data
    public class User_data
    {
        static BindingList<User> userData = new BindingList<User>();
        public static void setUserData(User u1) => userData.Add(u1);//User_data.setUserData()
        public static BindingList<User> getUserData() => userData;//User_data.getUserData()
    }

    //for storing all expence data for individual user
    public class expence_Details_data
    {
        static BindingList<expence_Details> expence_Details = new BindingList<expence_Details>();
        public static void setExpence_Details(expence_Details u_exp) => expence_Details.Add(u_exp);
        public static BindingList<expence_Details> getExpence_Details() => expence_Details;        
    }

    //for mapping expence details and user data
    public class expence_userDetails_data
    {
        public static Dictionary<User, BindingList<expence_Details>> users_expenceDetails = 
            new Dictionary<User, BindingList<expence_Details>>();
        public static void set_Users_expenceDetails(User usr, BindingList<expence_Details> exp_details) =>
            users_expenceDetails.Add(usr, exp_details);
        public static Dictionary<User, BindingList<expence_Details>> get_Users_expenceDetails() => users_expenceDetails;
    }        
}
