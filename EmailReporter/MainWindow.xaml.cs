using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net.Mail;

namespace EmailReporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            LoadFiles();
        }

        private void LoadFiles()
        {
            try
            {
                string[] fileNames = Directory.GetFiles(Directory.GetCurrentDirectory());
                foreach (string filename in fileNames)
                {
                    if (filename.Contains(".exe"))
                        continue;
                    this.listBox1.Items.Add(filename);
                }
            }
            catch
            {
                MessageBox.Show("An error occurred loading the files!");
                throw new Exception();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string[] emails = this.textBox1.Text.Split(new[] { ',' });
            string file = this.listBox1.SelectedItem.ToString();
            EmailReport(file, emails);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void EmailReport(string path, string[] recipients)
        {
            MailMessage email = new MailMessage(); //Create message object
            SmtpClient smtpserver = new SmtpClient("smtp.curse.local"); //Set mail server

            // read the report from the file into memory...
            string rawFile;
            using (StreamReader file = new StreamReader(path))
            {
                rawFile = file.ReadToEnd();
            }

            //Set mail recipients
            foreach (string recipient in recipients)
            {
                email.To.Add(recipient);
            }
            
            //Set email properties
            email.From = new MailAddress("SeleniumTests@curse.com", "Selenium Tests");
            email.Subject = "Selenium Tests Report";
            email.Body = rawFile;
            email.IsBodyHtml = true; //Send this as plain-text

            //Send the email
            smtpserver.Send(email);
        }
    }
}
