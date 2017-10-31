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

            //Action < T >  ??????


            int t = 0;

            
            

            //Action<int> action = new Action<int>(PrintGuest);
            //action("Hello!!!");

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

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
            //Task t = fromPub;
           fromPub.Start();
           
        }

    }

    public class Waiter
    {
        //utskrifts-callback till motsvarande listbox

        
    }





} //EOF
