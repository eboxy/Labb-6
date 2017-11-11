using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Threading;

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

        }  //mainwindow-method ends here!!!


        bool barOpen;

        //Slaskdemo-metod för start av baren....bättre sätt att göra det här senare!!!
        public void TheBar()
        {
            
            
            //Agenter
            Bouncer bounce = new Bouncer();
            Bartender b = new Bartender();
            Guest g = new Guest();
            Waiter w = new Waiter();

            //baren öpnnas events och prenumerationer
            b.BarenOppnas += Bartender_BarenOppnas;
            b.BarenOppnas += Guest_BarenOppnas;
            b.BarenOppnas += Waiter_BarenOppnas;

            //baren stängs events och prenumerationer
            b.Bartender_BarenStangsMetod();
            g.Guest_BarenStangsMetod();
            w.Waiter_BarenStangsMetod();


            /*
            if (barOpen)
            {
                //while (barOpen)   //Hur få while-loop att funka om vi nu skall ha en??
                //{
                    //Slask-demo för events och prennumeration etc.
                    b.BarenOppnas += Bartender_BarenOppnas;
                    b.BarenOppnas += Guest_BarenOppnas;
                    b.BarenOppnas += Waiter_BarenOppnas;

                    
                    b.Bartender_BarenOppnasMetod();
                    g.Guest_BarenOppnasMetod();
                    w.Waiter_BarenOppnasMetod();
                    InviteGuest();
                    b.VantaiBar += Bartender_VantaIBaren;
                    b.VantaiBarMetod();   //Skippa dessa metoder??  känns onödiga!!        
                  
                }
            
                
            }
            else if (barOpen == false)
            {
                b.BarenStangs += Bartender_BarenStangs;
                b.BarenStangs += Guest_BarenStrangs;
                b.BarenStangs += Waiter_BarenStangs;

                
                b.Bartender_BarenStangsMetod();
                g.Guest_BarenStangsMetod();
                w.Waiter_BarenStangsMetod();
                
        }
        */


            //Resterande mess här

            Task Bartender = Task.Run(() =>
            {
                if (barOpen)
                {
                    b.Bartender_BarenOppnasMetod();
                    g.Guest_BarenOppnasMetod();
                    w.Waiter_BarenOppnasMetod();
                    InviteGuest();
                    b.VantaiBar += Bartender_VantaIBaren;
                    b.VantaiBarMetod();   //Skippa dessa metoder??  känns onödiga!!    
                }
                else if (barOpen == false)
                {

                    b.Bartender_BarenStangsMetod();
                    g.Guest_BarenStangsMetod();
                    w.Waiter_BarenStangsMetod();
                }

            });

       }


         
        //Utskrifts-metoder för Bartender    
        public void Bartender_BarenOppnas()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbBartender.Items.Add(timeStamp + "_Baren öppnas!!"); }); }

        public void Bartender_BarenStangs()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbBartender.Items.Add(timeStamp + "_Baren stängs!!"); }); }

        public void Bartender_VantaIBaren()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbBartender.Items.Add(timeStamp + "_Väntar i baren"); }); }

        public void Bartender_PlockaGlasFranHyllan()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbBartender.Items.Add(timeStamp + "_Plockar fram glas från hyllan'"); }); }

        public void Bartender_HallaUppOl()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbBartender.Items.Add(timeStamp + "_Häller upp öl till kund"); }); }

        public void Bartender_GarHem()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbBartender.Items.Add(timeStamp + "_Går hem!!"); }); }


        //Utskrifts-metoder för Waiter
        public void Waiter_BarenOppnas()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbServitor.Items.Add(timeStamp + "_Baren öppnas!!"); }); }

        public void Waiter_BarenStangs()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbServitor.Items.Add(timeStamp + "_Baren stängs!!"); }); }


        public void Waiter_PlockaTommaGlas()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbServitor.Items.Add(timeStamp + "_Plockar upp tomma glas'"); }); }

        public void Waiter_DiskarGlas()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbServitor.Items.Add(timeStamp + "_Diskar glas"); }); }

        public void Waiter_StallerGlasIHyllan()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbServitor.Items.Add(timeStamp + "_Ställer glas i hyllan"); }); }

        //Utskrifts-metoder för Bouncer
        public void Bouncer_GarDirektHem()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbGaster.Items.Add(timeStamp + "_GarDirektHem"); }); }

        //Utskrifts-metoder för Guest
        public void Guest_BarenOppnas()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbGaster.Items.Add(timeStamp + "_Baren öppnas!!"); }); }

        public void Guest_BarenStrangs()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbGaster.Items.Add(timeStamp + "_Baren stängs!!"); }); }

        public void Guest_KommerInIPub()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbGaster.Items.Add(timeStamp + "_Kommer in i puben"); }); }

        public void Guest_LetarEfterLedigStrol()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbGaster.Items.Add(timeStamp + "_Letar efter ledig stol"); }); }

        public void Guest_SattSigNer()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbGaster.Items.Add(timeStamp + "_Sätter sig ned på stol"); }); }

        public void Guest_DrickerOlGarHem()
        { String timeStamp = GetTimestamp(DateTime.Now); Dispatcher.Invoke(() => { lstbGaster.Items.Add(timeStamp + "_Dricker ur ölen och går hem!!"); }); }


        //hade en version förut där man bara skicka ett värde men då blev det massa text i anropen och rördigt när metoderna skulle prenumerera på en händelse samt metoderna fick ja ha ett generiskt namn för aktuell agent. så blir fulare å klumpigare här bland utskrifterna men tror det blirn bättre sedan vid anropen etc.  något att tänka på men gör så här så länge. annars är det ju ett solklart fall av att brya ner i metod!!!




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
                //bool pubOpen = true;
                while (barOpen)
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

        private void btnOpenCloseBar_Click(object sender, RoutedEventArgs e)
        {
            //STARTAR OCH STOPPAR PROGRAMMET OCH BAREN!!!!!!!!!!  :D
            barOpen = !barOpen;
            TheBar();
           
        }




    }  //mainwindow class ends here





    // OTHER CLASSES HERE:

    public class Bouncer
    {
        public event Action GaDirektHem;

        public void DoSomethingMethodBouncer()
        { Task.Run(() => { GaDirektHem?.Invoke(); }); }


    }



    public class Guest
    {

       public event Action KommerInIPub, LetarLedigStol, SatterSigNed, DrickaOlLamnaBar, BarenOppnas, BarenStangs;

        public void Guest_BarenOppnasMetod()
        { BarenOppnas?.Invoke(); }
            //{ Task.Run(() => {BarenOppnas?.Invoke(); }); }

        public void Guest_BarenStangsMetod()
        { Task.Run(() => { BarenStangs?.Invoke(); }); }

        public void KommerInIPubMetod()
        { Task.Run(() => { KommerInIPub?.Invoke(); }); }

        public void LetarLedigStolMetod()
        { Task.Run(() => { LetarLedigStol?.Invoke(); }); }

        public void SatterSigNedMetod()
        { Task.Run(() => { SatterSigNed?.Invoke(); }); }

        public void DrickaOlLamnaBarMetod()
        { Task.Run(() => { DrickaOlLamnaBar?.Invoke(); }); }

        //utskrifts-callback till motsvarande listbox
    }


    public class Bartender
    {
        //utskrifts-callback till motsvarande listbox
    
        public event Action VantaiBar, PlockaGlasFranHylla, HallaUppOl, BartenderGarHem, BarenOppnas, BarenStangs;

        public void Bartender_BarenOppnasMetod()
        { Task.Run(() => { BarenOppnas?.Invoke(); }); }

        public void Bartender_BarenStangsMetod()
        { Task.Run(() => { BarenStangs?.Invoke(); }); }

        public void VantaiBarMetod()
        { Task.Run(() => { VantaiBar?.Invoke(); }); }

        public void PlockaGlasFranHyllaMetod()
        { Task.Run(() => { PlockaGlasFranHylla?.Invoke(); }); }

        public void HallaUppOlMetod()
        { Task.Run(() => { HallaUppOl?.Invoke(); }); }

        public void BartenderGarHemMetod()
        { Task.Run(() => { BartenderGarHem?.Invoke(); }); }



        //ingen task, metod som anropar häll upp öl osv. task i main dock!! (dave)  SKALL VI BAARA EN LR NÅGRA TASKS I MIAN DÅ OCH FRÅN DEN/DEMStrANROPA METODER                                                                           I KLASSER ETC???  JAG HAR DOCK GJORT  LITE TRÅDAR I VARJE KLASS. FÅR VÄL                                                                              ÄNDRA PÅ DET SEDAN ISF??


    }




    public class Waiter
    {

        public event Action PlockarTommaGlas, DiskarGlas, StallarGlasIHylla, BarenOppnas, BarenStangs;

        public void Waiter_BarenOppnasMetod()
        {
            Task.Run(() => { BarenOppnas?.Invoke(); });
        }

        public void Waiter_BarenStangsMetod()
        {
            Task.Run(() => { BarenStangs?.Invoke(); });
        }

        public void PlockarTommaGlasMetod()
        {
            Task.Run(() => { PlockarTommaGlas?.Invoke(); });
        }

        public void DiskarGlasMetod()
        {
            Task.Run(() => { DiskarGlas?.Invoke(); });
        }

        public void StallarGlasIHyllaMetod()
        {
            Task.Run(() => { StallarGlasIHylla?.Invoke(); });
        }

        //utskrifts-callback till motsvarande listbox

     }


    


    





} //EOF
    