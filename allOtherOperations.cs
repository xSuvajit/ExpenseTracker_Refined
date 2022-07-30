using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker_Sln
{
    public class allOtherOperations
    {
        public static void initilizeDummyDetails()
        {
            User suvajit = new User()
            {
                Name = "Suvajit Sarkar",
                DOB = new DateTime(1997, 2, 11),
                Address = "kolkata",
                Gender = "Male",
                Email = "suvajit@gmail.com",
                Password = "abc123"
            };
            User deepraj = new User(
                "Deepraj Sarkar", new DateTime(1998, 1, 2), "kolkata", "Male", "deepraj@gmail.com", "P@ssw0rd"
            );

            User_data.setUserData(suvajit);
            User_data.setUserData(deepraj);

            BindingList<expence_Details> suvajit_expence_Details = new BindingList<expence_Details>();
            BindingList<expence_Details> deepraj_expence_Details = new BindingList<expence_Details>();

            suvajit_expence_Details.Add(new expence_Details(101, "grocery",
                new DateTime(2020, 3, 4), 500, ExpenceCatagory.Credit, "feedback"));
            suvajit_expence_Details.Add(new expence_Details(102, "wearable",
                new DateTime(2020, 3, 5), 1000, ExpenceCatagory.Credit, "feedback"));
            suvajit_expence_Details.Add(new expence_Details(103, "food",
                new DateTime(2020, 3, 6), 1500, ExpenceCatagory.Credit, "feedback"));
            suvajit_expence_Details.Add(new expence_Details(103, "food",
                new DateTime(2020, 3, 6), 600, ExpenceCatagory.Debit, "feedback"));

            deepraj_expence_Details.Add(new expence_Details(102, "electronins",
                new DateTime(2022, 02, 01), 1500, ExpenceCatagory.Debit, "feedback"));

            expence_userDetails_data.set_Users_expenceDetails(suvajit, suvajit_expence_Details);
            expence_userDetails_data.set_Users_expenceDetails(deepraj, deepraj_expence_Details);
        }

        public static bool isUserPresent(string _email)
        {            
            foreach (User usr in User_data.getUserData())
            {
                if (usr.Email.Equals(_email))
                    return true;
            }            
            return false;
        }

        static void addDataToExpDetails(dynamic _data)
        {
            foreach (expence_Details det in _data)
            {
                exp_det_filter.Add(det);
            }
        }        

        static BindingList<expence_Details> exp_det_filter = new BindingList<expence_Details>();

        public static BindingList<expence_Details> searchResultAfterFilter
            (string choice,User afterloginUser, string dataForSrch_txtBox_Text)
        {
            exp_det_filter.Clear();
            switch (choice)
            {
                case "By ID":
                    {
                        dynamic data = from id in expence_userDetails_data.get_Users_expenceDetails()[afterloginUser]
                                   where id.ID.ToString().Equals(dataForSrch_txtBox_Text)
                                   select id;
                        addDataToExpDetails(data);                        
                        break;
                    }
                case "By Name":
                    {
                        dynamic data = from id in expence_userDetails_data.get_Users_expenceDetails()[afterloginUser]
                                   where id.Name.Equals(dataForSrch_txtBox_Text, StringComparison.InvariantCultureIgnoreCase)
                                   select id;
                        addDataToExpDetails(data);
                        break;
                    }
                case "By Date":
                    {
                        DateTime dateTime;
                        dynamic data = from date in expence_userDetails_data.get_Users_expenceDetails()[afterloginUser]
                                   where date.Date.Equals(DateTime.TryParse(dataForSrch_txtBox_Text, out dateTime))
                                   select date;
                        addDataToExpDetails(data);
                        break;
                    }
                case "By Catagory":
                    {
                        dynamic data = from cat in expence_userDetails_data.get_Users_expenceDetails()[afterloginUser]
                                   where cat.expenceCatagory.ToString().Equals(dataForSrch_txtBox_Text, StringComparison.InvariantCultureIgnoreCase)
                                   select cat;
                        addDataToExpDetails(data);
                        break;
                    }
                default:
                    break;
            }
            return exp_det_filter;
        }
    }
}
