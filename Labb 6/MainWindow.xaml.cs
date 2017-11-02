using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

    public partial class MainWindow : Window
    {

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


            Bartender b = new Bartender();
            int t = 0;

            lstbBartender.Items.Add("´Puben frågar bartender");
            //Task taskToBartender = new Task(new Action<int>(PrintGuest(t));  //   VARFÖR FUNKAR NEDANSTÅENDE TASK OCH INTE DENNA??

            Task taskToBartender = new Task(() => PrintGuest(t));
            b.InviteGuest(taskToBartender);

            
        }  //mainwindow-method ends here!!!

        public void PrintGuest(int numOfGuests)
        {

            Dispatcher.Invoke(() =>
            {
                lstbBartender.Items.Add("Svar från bartender " + numOfGuests);
            });
           
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
                        lstbGaster.Items.Insert(0,timeStamp + " " +_theNames.ElementAt(r) + " kommer in i baren.");
                    });
                }
            });

        }
     


    }  //mainwindow class ends here

    
   


    // OTHER CLASSES HERE:

    public class Bouncer
    {
        
        
   }

    public class Patron
    {
        //utskrifts-callback till motsvarande listbox



    }


    public class Bartender
    {
        //utskrifts-callback till motsvarande listbox

        public void InviteGuest(Task fromPub)
        {
          fromPub.Start();
        }

    }

    public class Waiter
    {
        //utskrifts-callback till motsvarande listbox

        
    }

    


} //EOF
