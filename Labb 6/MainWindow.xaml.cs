using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
using System.Windows.Threading;

namespace Labb_6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        

        public MainWindow()
        {
            InitializeComponent();

            Bartender b = new Bartender();

            b.PourBeer += WaitingForGuest;
            b.PourBeer += OnBeer;
            b.PourBeer += GastDrinksBeer;
            b.PouringBeer();



            //int t = 0;

            //lstbBartender.Items.Add("´Puben frågar bartender");
            //Task taskToBartender = new Task(new Action<int>(PrintGuest(t));  //   VARFÖR FUNKAR NEDANSTÅENDE TASK OCH INTE DENNA??

            //Task taskToBartender = new Task(() => PrintGuest(t));
            //b.InviteGuest(taskToBartender);






        }  //mainwindow-method ends here!!!

        //public void PrintGuest(int numOfGuests)
        //{ Dispatcher.Invoke(() => { lstbBartender.Items.Add("Svar från bartender " + numOfGuests);});}

        public void GastDrinksBeer()
        { Dispatcher.Invoke(() => { lstbGaster.Items.Add(3 + "_Gästen dricker upp ölen"); });}

        public void OnBeer()
        { Dispatcher.Invoke(() => { lstbBartender.Items.Add(2 + "_Har hällt bärs här hela dan "); }); }

        public void WaitingForGuest()
        { Dispatcher.Invoke(() => { lstbBartender.Items.Add(1 + "_Väntar på gästen "); }); }

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

        //public event Action Run, Roar; 

        public event Action PourBeer;


        public void PouringBeer()
        {

            Task.Run(() =>
            {
               PourBeer?.Invoke();

            });

            //ingen task, metod som anropar häll upp öl osv. task i main dock!!

        }

        
        
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
