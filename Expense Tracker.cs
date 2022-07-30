using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Expense_Tracker_Sln
{
    public partial class Expense_Tracker : Form
    {
        #region all variables

        DateTime dobVal, detDate;
        string genderVal = "";
        User user = null, afterloginUser = null;
        int _id;
        ExpenceCatagory expCatVal;
        float _amount;
        expence_Details expUsrDet;
        BindingList<expence_Details> expence_Details_values = null;

        #endregion

        #region from functionalities

        public Expense_Tracker()
        {
            InitializeComponent();
            allOtherOperations.initilizeDummyDetails();
            dataGridView1.DataSource = User_data.getUserData();
            user_details.TabPages.Clear();
            user_details.TabPages.Add(login_tabpage);
            user_details.TabPages.Add(Register_tabPage);
            user_details.SelectTab(login_tabpage);
            lblClear();
            reg_clearAll();
        }

        public void lblClear()
        {
            progressBar_lbl.Text = "";
        }

        #endregion

        #region register page functionalityes       

        private void reg_submt_btn_Click(object sender, EventArgs e)
        {
            lblClear();
            if (reg_firstName_txtbox.TextLength > 0 && reg_Email_txtbox.TextLength > 0
                && reg_password_txtbox.TextLength > 0
                && (reg_male_radioBtn.Checked || reg_female_radioBtn.Checked))
            {
                if (!allOtherOperations.isUserPresent(reg_Email_txtbox.Text))
                {
                    if (reg_password_txtbox.Text.Equals(reg_confirmPass_txtbox.Text))
                    {
                        DateTime.TryParse(reg_DOB_txtbox.Text, out dobVal);
                        user = new User(reg_firstName_txtbox.Text, dobVal, reg_address_txtbox.Text,
                            genderVal, reg_Email_txtbox.Text, reg_password_txtbox.Text);
                        User_data.setUserData(user);
                        expence_Details_values = new BindingList<expence_Details>();
                        expence_userDetails_data.set_Users_expenceDetails(user, expence_Details_values);
                        user_details.SelectTab(login_tabpage);
                        reg_clearAll();
                        progressBar_lbl.Text = "Data added successfully!!";
                    }
                    else
                        progressBar_lbl.Text = "Password Mismatch!!";
                }
                else
                    progressBar_lbl.Text = "User already added!!";
            }
            else
                progressBar_lbl.Text = "Please enter all details!!";

        }

        public void reg_clearAll()
        {
            lblClear();
            reg_confirmPass_txtbox.Clear();
            reg_firstName_txtbox.Clear();
            reg_DOB_txtbox.Clear();
            reg_address_txtbox.Clear();
            reg_Email_txtbox.Clear();
            reg_password_txtbox.Clear();
            reg_male_radioBtn.Checked = false;
            reg_female_radioBtn.Checked = false;
            user = null;
            expence_Details_values = null;
        }

        private void reg_male_radioBtn_CheckedChanged(object sender, EventArgs e)
        {
            reg_female_radioBtn.Checked = false;
            RadioButton b = (RadioButton)sender;
            genderVal = b.Text;
        }

        private void reg_female_radioBtn_CheckedChanged(object sender, EventArgs e)
        {
            reg_male_radioBtn.Checked = false;
            RadioButton b = (RadioButton)sender;
            genderVal = b.Text;
        }

        private void reg_goToLoginPage_linkLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            reg_clearAll();
            user_details.SelectTab(login_tabpage);
        }

        private void reg_reset_btn_Click(object sender, EventArgs e)
        {
            reg_clearAll();
        }

        #endregion

        #region login page functionalities

        private void lgn_login_btn_Click(object sender, EventArgs e)
        {
            bool isfound = false;
            lblClear();
            if (lgn_Email_txtbox.TextLength > 0 && lgn_Password_txtbox.TextLength > 0)
            {
                foreach (User u in User_data.getUserData())
                {
                    if (u.Email.Equals(lgn_Email_txtbox.Text) && u.Password.Equals(lgn_Password_txtbox.Text))
                    {
                        progressBar_lbl.Text = "Login Successfull!!";
                        user_details.TabPages.Add(UserDetails_tabpage);
                        UserDetails_tabpage.Text = u.Name + " Details";
                        afterloginUser = u;
                        user_details.TabPages.Remove(login_tabpage);
                        user_details.TabPages.Remove(Register_tabPage);
                        isfound = true;
                        BindingList<expence_Details> ed = null;
                        expence_userDetails_data.get_Users_expenceDetails().TryGetValue(u, out ed);
                        exp_dataGridView.DataSource = ed;
                        break;
                    }
                }
                if (!isfound)
                {
                    lgn_Password_txtbox.Clear();
                    progressBar_lbl.Text = "Invalid email or password!!";
                }
            }
            else
                progressBar_lbl.Text = "Enter all details!!";
        }

        private void lgn_reset_btn_Click(object sender, EventArgs e)
        {
            lblClear();
            if (lgn_Email_txtbox.TextLength > 0 || lgn_Password_txtbox.TextLength > 0)
            {
                lgn_Password_txtbox.Clear();
                lgn_Email_txtbox.Clear();
            }
            else
            {
                progressBar_lbl.Text = "Nothing to reset!!";
            }
        }

        private void lgn_goToRegisterPage_linl_lbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {          
            if (!user_details.TabPages.Contains(Register_tabPage))
            {
                user_details.TabPages.Add(Register_tabPage);
            }
            user_details.SelectTab(Register_tabPage);
            reg_clearAll();
        }

        private void lgn_ShowPass_btn_Click(object sender, EventArgs e)
        {
            lblClear();
            if (lgn_Password_txtbox.TextLength > 0)
            {
                if (lgn_Password_txtbox.PasswordChar == '*')
                {
                    lgn_HidePass_btn.BringToFront();
                    lgn_Password_txtbox.PasswordChar = '\0';
                }
            }
            else
            {
                progressBar_lbl.Text = "Nothing to show!!";
            }

        }

        private void lgn_HidePass_btn_Click(object sender, EventArgs e)
        {
            lblClear();
            if (lgn_Password_txtbox.PasswordChar == '\0')
            {
                lgn_ShowPass_btn.BringToFront();
                lgn_Password_txtbox.PasswordChar = '*';
            }
        }
        #endregion

        #region details Page functionalities

        private void details_filterData_btn_Click(object sender, EventArgs e)
        {
            lblClear();
            user_details.TabPages.Clear();
            user_details.TabPages.Add(searchExpenceData_tabPage);
        }

        public void clr_exp_det_txtbox()
        {
            exp_det_id_txtbox.Clear();
            exp_det_date_txtbox.Clear();
            exp_det_name_txtbox.Clear();
            exp_det_amount_txtbox.Clear();
            exp_det_remarks_txtbox.Clear();
            exp_det_debit_rediobtn.Checked = false;
            exp_det_credit_rediobtn.Checked = false;
        }

        private void details_addExpData_btn_Click(object sender, EventArgs e)
        {
            lblClear();
            if (!user_details.TabPages.Contains(exp_det_tabPage))
                user_details.TabPages.Add(exp_det_tabPage);
            user_details.SelectTab(exp_det_tabPage);
            clr_exp_det_txtbox();
        }
        private void details_removeExpData_btn_Click(object sender, EventArgs e)
        {
            lblClear();
            if (expence_userDetails_data.get_Users_expenceDetails()[afterloginUser].Count > 0)
            {
                expence_userDetails_data.get_Users_expenceDetails()[afterloginUser].RemoveAt(
                    expence_userDetails_data.get_Users_expenceDetails()[afterloginUser].Count - 1);
                progressBar_lbl.Text = "Data deleted!!";
            }
            else
                progressBar_lbl.Text = "No data to be deleted!!";
        }

        private void details_logout_Click(object sender, EventArgs e)
        {
            lblClear();
            afterloginUser = null;            
            lgn_Email_txtbox.Clear();
            lgn_Password_txtbox.Clear();
            user_details.TabPages.Clear();
            user_details.TabPages.Add(login_tabpage);
            user_details.TabPages.Add(Register_tabPage);
            user_details.SelectTab(login_tabpage);
            progressBar_lbl.Text = "Log out successfull!!";
        }

        #endregion

        #region expence details page functionalities

        private void exp_det_rediobtn_clicked(object sender, EventArgs e)
        {
            RadioButton b = (RadioButton)sender;
            if (b.Text.Equals("Credit"))
            {
                expCatVal = ExpenceCatagory.Credit;
            }
            else
                expCatVal = ExpenceCatagory.Debit;
        }

        private void exp_dat_submit_btn_Click(object sender, EventArgs e)
        {
            lblClear();
            if (exp_det_id_txtbox.TextLength > 0 && exp_det_name_txtbox.TextLength > 0 &&
                exp_det_date_txtbox.TextLength > 0 && exp_det_amount_txtbox.TextLength > 0 &&
                exp_det_remarks_txtbox.TextLength > 0 && (exp_det_credit_rediobtn.Checked ||
                exp_det_debit_rediobtn.Checked))
            {
                DateTime.TryParse(exp_det_date_txtbox.Text, out detDate);
                int.TryParse(exp_det_id_txtbox.Text, out _id);
                float.TryParse(exp_det_amount_txtbox.Text, out _amount);
                expUsrDet = new expence_Details(_id, exp_det_name_txtbox.Text, detDate,
                    _amount, expCatVal, exp_det_remarks_txtbox.Text);
                expence_Details_data.setExpence_Details(expUsrDet);
                expence_userDetails_data.get_Users_expenceDetails()[afterloginUser].Add(expUsrDet);
                progressBar_lbl.Text = "Expence Data added!!";
                user_details.TabPages.Remove(exp_det_tabPage);
                user_details.SelectTab(UserDetails_tabpage);
            }
            else
                progressBar_lbl.Text = "Enter all data!!";
        }

        #endregion

        #region private

        private void showAllUserDetails(object sender, EventArgs e)
        {
            if (lgn_Email_txtbox.Text == "Admin" && lgn_Password_txtbox.Text == "Admin")
            {
                lgn_Email_txtbox.Clear();
                lgn_Password_txtbox.Clear();
                user_details.TabPages.Clear();
                user_details.TabPages.Add(UserDetailsMaster_tabPage);
                progressBar_lbl.Text = "Welcome Admin!!";
            }
            else
                progressBar_lbl.Text = "🤐";
        }

        private void userDetPageClose_btn_Click(object sender, EventArgs e)
        {
            lblClear();
            user_details.TabPages.Clear();
            user_details.TabPages.Add(login_tabpage);
            user_details.TabPages.Add(Register_tabPage);
            user_details.SelectTab(login_tabpage);
        }

        private void removeRecord_btn_Click(object sender, EventArgs e)
        {
            lblClear();
            int index = 0;
            bool present = false;
            if (User_data.getUserData().Count > 0)
            {
                if (enterEmailForDelete_txtBox.TextLength == 0 && enterIndexForDelete_txtBox.TextLength == 0)
                {
                    User_data.getUserData().RemoveAt(User_data.getUserData().Count - 1);
                    progressBar_lbl.Text = "Data deleted successfully!!";
                }
                else
                {
                    try
                    {
                        if (enterEmailForDelete_txtBox.TextLength > 0 && enterIndexForDelete_txtBox.TextLength > 0)
                            throw new Exception();
                        else if (enterEmailForDelete_txtBox.TextLength > 0 && enterIndexForDelete_txtBox.TextLength == 0)
                        {
                            foreach (User user in User_data.getUserData())
                            {
                                if (enterEmailForDelete_txtBox.Text.Equals(user.Email))
                                {
                                    present = true;
                                    break;
                                }
                                else
                                    index++;
                            }
                            if (present)
                            {
                                User_data.getUserData().RemoveAt(index);
                                progressBar_lbl.Text = "Data deleted successfully!!";
                                enterEmailForDelete_txtBox.Clear();
                                enterIndexForDelete_txtBox.Clear();
                            }
                            else
                                progressBar_lbl.Text = "Email incorrect!!";
                        }
                        else if (enterEmailForDelete_txtBox.TextLength == 0 && enterIndexForDelete_txtBox.TextLength > 0)
                        {
                            index = int.Parse(enterIndexForDelete_txtBox.Text);
                            User_data.getUserData().RemoveAt(index - 1);
                            progressBar_lbl.Text = "Data deleted successfully!!";
                            enterEmailForDelete_txtBox.Clear();
                            enterIndexForDelete_txtBox.Clear();
                        }
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        progressBar_lbl.Text = "Invalid Index!!";
                    }
                    catch (Exception ex)
                    {
                        progressBar_lbl.Text = "Please enter either Email or Index!!";
                    }
                }
            }
            else
                progressBar_lbl.Text = "No Data to be deleted!!";
        }

        #endregion

        #region expence filter page functionalities

        public void clearFilterPageDetails()
        {
            lblClear();
            expence_filter_comboBox.SelectedItem = null;
            dataForSrch_txtBox.Clear();
            expenceDetails_filter_dataGridViwe.DataSource = null;
        }

        private void exp_filterPage_goBack_btn_Click(object sender, EventArgs e)
        {
            clearFilterPageDetails();
            user_details.TabPages.Clear();
            user_details.TabPages.Add(UserDetails_tabpage);
        }

        private void exp_filterPage_Clear_btn_Click(object sender, EventArgs e)
        {
            clearFilterPageDetails();
        }

        private void Exp_filter_srch_btn_Click(object sender, EventArgs e)
        {            
            try
            {
                if(dataForSrch_txtBox.TextLength>0)
                {
                    if (allOtherOperations.searchResultAfterFilter
                    (expence_filter_comboBox.SelectedItem.ToString(), afterloginUser, dataForSrch_txtBox.Text).Count > 0)
                    {
                        expenceDetails_filter_dataGridViwe.DataSource = allOtherOperations.searchResultAfterFilter
                            (expence_filter_comboBox.SelectedItem.ToString(), afterloginUser, dataForSrch_txtBox.Text);
                    }
                    else
                    {
                        progressBar_lbl.Text = "No data!!";
                    }
                }
                else
                    progressBar_lbl.Text = "Enter data for search!!";

            }
            catch(NullReferenceException ex)
            {
                progressBar_lbl.Text = "Please select options from drop down only!!";
            }
            
        }

        #endregion

    }
}

