using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace BBHA_Desktop
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public static string NAME;
        public Login()
        {
            InitializeComponent();
            FocusManager.SetFocusedElement(this, tbusername);
        }
        public string GetNAME()
        {
            return NAME;
        }
        private void Btnsumbit_Click(object sender, RoutedEventArgs e)
        {
            if (pbpassword.Password.Trim() == "" || tbusername.Text.Trim() == "")
            {
                MessageBox.Show("Please fill all fields");
            }
            else
            {
                SqlConnection con = new SqlConnection(@"Data Source=SQL6007.site4now.net;Initial Catalog=DB_A48894_BBHA;User Id=DB_A48894_BBHA_admin;Password=1q2w3e4r5t;");
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    String query = "SELECT * FROM Users WHERE Username = '" + tbusername.Text.Trim() + "' and Password = '" + pbpassword.Password.Trim() + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter sda = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        UserControlHome home = new UserControlHome();
                        //home.SetContent()
                        foreach (DataRow row in dt.Rows)
                        {
                            NAME = row["FullName"].ToString();
                        }
                        MainWindow mw = new MainWindow();
                        mw.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Username or Password is incorrect");
                    }

                }
                catch (Exception)
                {

                }
            }
        }
        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Register mw = new Register();
            mw.Show();
            this.Close();
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
