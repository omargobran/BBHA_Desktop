using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BBHA_Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            UserControl usc = null;
            GridMain.Children.Clear();
            usc = new UserControlHome();
            GridMain.Children.Add(usc);
        }

        private void MainWindow_Resize(object sender, System.EventArgs e)
        {
            Control control = (Control)sender;
            if (this.WindowState == WindowState.Maximized)
            {
                Restore.Visibility = Visibility.Visible;
                Maximize.Visibility = Visibility.Hidden;
            }
            else if (this.WindowState == WindowState.Normal)
            {
                Restore.Visibility = Visibility.Hidden;
                Maximize.Visibility = Visibility.Visible;
            }
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "ItemHome":
                    usc = new UserControlHome();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemFS":
                    usc = new UserControlFS();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemPrediction":
                    usc = new UserControlPrediction();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemHelp":
                    usc = new UserControlHelp();
                    GridMain.Children.Add(usc);
                    break;
                default:
                    break;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal || this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Minimized;
            else if (this.WindowState == WindowState.Minimized)
                this.WindowState = WindowState.Normal;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
            }
            this.DragMove();
        }

        private void Logout_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
