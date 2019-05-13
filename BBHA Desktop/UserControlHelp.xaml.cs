using System.Windows.Controls;
using System.Windows.Navigation;
using System.Diagnostics;

namespace BBHA_Desktop
{
    /// <summary>
    /// Interaction logic for UserControlHelp.xaml
    /// </summary>
    public partial class UserControlHelp : UserControl
    {
        public UserControlHelp()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
