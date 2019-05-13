using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace BBHA_Desktop
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
            FocusManager.SetFocusedElement(this, tbname);
        }
        private void Btnsumbit_Click(object sender, RoutedEventArgs e)
        {
            if (pbpassword.Password.Trim() == "" || tbusername.Text.Trim() == "" || pbpassword1.Password.Trim() == "" || tbname.Text.Trim() == "")
            {
                MessageBox.Show("Please Fill All Fields");
            }
            else
            {
                SqlConnection con = new SqlConnection(@"Data Source=SQL6007.site4now.net;Initial Catalog=DB_A48894_BBHA;User Id=DB_A48894_BBHA_admin;Password=1q2w3e4r5t;");

                if (pbpassword.Password != pbpassword1.Password)
                {
                    MessageBox.Show("The Entered Passwords Don't Match!");
                }

                else
                {

                    SqlCommand cmd = new SqlCommand("select * from Users where Username ='" + tbusername.Text.Trim() + "'", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    int i = ds.Tables[0].Rows.Count;
                    if (i > 0)
                    {
                        MessageBox.Show("Username is already taken.");
                    }
                    else
                    {
                        try
                        {
                            if (con.State == ConnectionState.Closed)
                            {
                                con.Open();
                            }
                            String query = "insert into Users (Username,Password,FullName) values ('" + this.tbusername.Text.Trim() + "','" + this.pbpassword.Password.Trim()
                                + "','" + this.tbname.Text.Trim() + "')";
                            cmd = new SqlCommand(query, con);
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                            con.Close();
                            btnsubmit.IsEnabled = false;
                            Login lg = new Login();
                            lg.Show();
                            this.Close();
                        }
                        catch (Exception err)
                        {
                            throw err;
                        }
                    }
                }
            }
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Login lg = new Login();
            lg.Show();
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
