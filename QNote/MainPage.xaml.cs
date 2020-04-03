using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using QNote.Resources;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Tasks;

namespace QNote
{
    public partial class MainPage : PhoneApplicationPage
    {
        IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
        public MainPage()
        {
            InitializeComponent();
            try
            {
                IsolatedStorageFileStream savedNote = storage.OpenFile("notes.txt", FileMode.Open, FileAccess.Read); //restores notes
                StreamReader reader = new StreamReader(savedNote);
                notes.Text = reader.ReadToEnd();
                reader.Dispose();
                savedNote.Dispose();
            }
            catch
            {
                notes.Text = "Looks like you are running this app for the first time. This place is for you to take notes and it extends as you write notes. Clear notes to get started!";
            }
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to erase your notes?", "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                notes.Text = "";
            }
        }

        private void On_Exit(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream("notes.txt", FileMode.Create, FileAccess.Write, storage));
            writer.WriteLine(notes.Text);
            writer.Close();
        }

        private void notes_GotFocus(object sender, RoutedEventArgs e)
        {
            ApplicationBar.IsVisible = false;
        }

        private void notes_LostFocus(object sender, RoutedEventArgs e)
        {
            ApplicationBar.IsVisible = true;
        }

        private void about_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/about.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ProvideFeedback_Click(object sender, EventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.To = "celikkoseoglu@yahoo.com";
            emailComposeTask.Subject = "Feedback for QNote";
            emailComposeTask.Show();
        }
    }
}