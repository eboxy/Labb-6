using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labb_6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void ShowValue();

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("[HH:mm:ss]");

        }

        List<string>_theNames = new List<string>();
        
        
        
        public MainWindow()
        {
            InitializeComponent();

            InviteGuest();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void InviteGuest()
        {
            //Bouncer
            //Inkastaren släpper in kunder slumpvis, efter tre till tio sekunder. Inkastaren kontrollerar leg, så att alla i baren kan veta vad kunden heter. (Slumpa ett namn åt nya kunder från en lista) Inkastaren slutar släppa in nya kunder när baren stänger och går hem direkt.

            _theNames.Add("Izola ");
            _theNames.Add("Jeane");
            _theNames.Add("Christiane");
            _theNames.Add("Taneka");
            _theNames.Add("Debbra");
            _theNames.Add("Terrilyn");
            _theNames.Add("Fransisca");
            _theNames.Add("Zetta");
            _theNames.Add("Zina");


            Task t1 = Task.Run(() =>
            {
                //int actionCount = 0; 
                bool pubOpen = true;
                while (pubOpen)
                {
                    Random timeRandom = new Random();
                    String timeStamp = GetTimestamp(DateTime.Now);
                    Thread.Sleep(timeRandom.Next(3000,10000));
                    Dispatcher.Invoke(() =>
                    {
                        int r = timeRandom.Next(_theNames.Count);
                        lstbGaster.Items.Add((timeStamp) + " " +_theNames.ElementAt(r) + " kommer in i baren.");


                    });
                }
            });

        }
        static void Display(string message)
        {
            Console.WriteLine("Hej");
        }

    }  //mainwindow class ends here

    
   


    // OTHER CLASSES HERE:

    public class Bouncer
    {
        
    }

    public class Patron
    {
        
    }


    public class Bartender
    {
        

    }

    public class Waiter
    {
        
    }

    


} //EOF
