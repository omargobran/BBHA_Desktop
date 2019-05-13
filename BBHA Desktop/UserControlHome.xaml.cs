using System.Windows.Controls;

namespace BBHA_Desktop
{
    /// <summary>
    /// Interação lógica para UserControlHome.xam
    /// </summary>
    public partial class UserControlHome : UserControl
    {
        public UserControlHome()
        {
            InitializeComponent();
            Login login = new Login();
            Label.Content = "Welcome, " + login.GetNAME()+"!";
        }
    }
}
