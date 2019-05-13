using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MathWorks.MATLAB.ProductionServer.Client;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace BBHA_Desktop
{
    /// <summary>
    /// Interaction logic for UserControlFS.xaml
    /// </summary>
    public partial class UserControlFS : System.Windows.Controls.UserControl
    {
        const string seperator = "; ";
        int columns, rows;
        string fileName = "", fullPath = "", tblData = "", str = "", header = "", tbl = "";
        int[] sel;
        byte[] imgArray;
        public UserControlFS()
        {
            InitializeComponent();
        }
        public interface IBBHA_Select
        {
            void BBHA_Select(out string newFileName, out int featuresCountOld, out int featuresCountNew,
                  out double acc, out byte[] imgArr, out string result, out int[] selection,
                  string tblData, string str, int columns, int rows); // has to be the same name as function in MATLAB
        }
        private void BrowseBtn_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".csv";
            dlg.Filter = "(.csv)|*.csv";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                var len = new FileInfo(dlg.FileName).Length;
                if (len > 1048576)
                    System.Windows.Forms.MessageBox.Show("Maximum File Size Exceeded. Please Add a 1 MB File and Try Again");
                else
                {
                    image1.Source = null;
                    fileName = ""; fullPath = ""; tblData = ""; str = ""; header = ""; tbl = ""; sel = null;
                    fullPath = dlg.FileName;
                    var lines = File.ReadAllLines(fullPath);
                    rows = lines.Count() - 1;
                    columns = lines[0].Split(',').Count();
                    TextBox1.Content = "File Uploaded Successfully";
                    lblresult.Content = "";
                }
            }
        }
        private void RunBtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty((string)TextBox1.Content))
            {
                System.Windows.Forms.MessageBox.Show("Please Select a File and Try Again!");
            }
            else
            {
                int column = 1;
                while (column != columns)
                {
                    str += "%f,";
                    column++;
                }
                column = 1;
                using (FileStream fs = File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (BufferedStream bs = new BufferedStream(fs))
                using (StreamReader sr = new StreamReader(bs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (column > 1)
                        {
                            tblData += line + ",";
                        }
                        else
                        {
                            header = line;
                        }
                        column++;
                    }
                }

                MWClient client = new MWHttpClient();
                try
                {
                    IBBHA_Select bh = client.CreateProxy<IBBHA_Select>
                    (new Uri("http://d613ecc4.ngrok.io/BBHA"));
                    bh.BBHA_Select(out string newFileName, out int featuresCountOld, out int featuresCountNew,
                    out double acc, out byte[] imgArr, out string result, out int[] selection,
                    tblData, str, columns, rows);

                    sel = selection;
                    imgArray = imgArr;
                    fileName = newFileName;
                    result = string.Concat(result, ';');
                    tbl = result;
                    lblresult.Content = "Old Number of Features = " + featuresCountOld.ToString() + "\nNew Number of Features = " + featuresCountNew.ToString() + "\nAccuracy Rate = " + String.Format("{0:0.00}%", acc);
                    TextBox1.Content = "";
                }
                catch (MATLABException ex)
                {
                    TextBox1.Content = "Error in our servers has occurred!";
                    Console.WriteLine("{0} MATLAB exception caught.", ex);
                    Console.WriteLine(ex.StackTrace);
                }
                catch (WebException ex)
                {
                    TextBox1.Content = "Check your connection!";
                    Console.WriteLine("{0} Web exception caught.", ex);
                    Console.WriteLine(ex.StackTrace);
                }
                catch(ArgumentException ex)
                {
                    TextBox1.Content = "Error in our servers has occurred!";
                    Console.WriteLine("{0} MATLAB argument exception caught.", ex);
                    Console.WriteLine(ex.StackTrace);
                }
                finally
                {
                    client.Dispose();
                }
                BitmapImage image;
                try
                {
                    using (MemoryStream imageStream = new MemoryStream(imgArray))
                    {
                        image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = imageStream;
                        image.EndInit();
                    }
                    image1.Source = image;
                    image1.Stretch = Stretch.Uniform;
                }
                catch (Exception ex)
                {
                    TextBox1.Content = "Error loading image! Try again.";
                    Console.WriteLine("{0} Exception caught.", ex);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }
        private void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty((string)lblresult.Content))
            {
                System.Windows.Forms.MessageBox.Show("Please Press Run and Try Again!");
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                string filter = "CSV file (.csv)|.csv| All Files (.)|.";
                sfd.Filter = filter;
                sfd.FileName = fileName;
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    StringBuilder csvcontent = new StringBuilder();
                    StringBuilder csvheader = new StringBuilder();
                    string newPath = sfd.FileName;
                    List<String> newHeader = new List<String>();
                    string headerString = "";

                    string[] headers = header.Split(',');
                    Array.Resize(ref sel, sel.Length + 1);
                    sel[sel.Length - 1] = 1;
                    for (int i = 0; i < sel.Length; i++)
                    {
                        if (sel[i] == 1)
                        {
                            newHeader.Add(headers[i]);
                        }
                    }
                    headerString = string.Join(",", newHeader);
                    csvheader.AppendLine(" ," + headerString);

                    string[] DataRows = tbl.Split(seperator.ToCharArray());


                    foreach (string DataRow in DataRows)
                    {
                        csvcontent.AppendLine(DataRow);
                    }

                    string resultString = Regex.Replace(" ," + csvcontent.ToString(), @"^\s+$[\r\n]*", " ,", RegexOptions.Multiline);
                    resultString = Regex.Replace(resultString, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
                    File.WriteAllText(newPath, csvheader.ToString());
                    File.AppendAllText(newPath, resultString);
                    Process.Start(newPath);
                }
            }
        }
    }
}
