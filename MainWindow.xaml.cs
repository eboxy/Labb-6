﻿using System;
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

        List<string> _theNames = new List<string>();


        public MainWindow()
        {
            InitializeComponent();
        } //mainwindow-method ends here!!!

        //bool för att öpnna och stänga bar
        bool barOpen;

        //Agenter
        Bouncer bounce = new Bouncer();

        Bartender b = new Bartender();
        Guest g = new Guest("");
        Waiter w = new Waiter();

        //Check if bartenderworks
        public bool BartenderPause = true;


        //Queues
        ConcurrentStack<Glass> glasstack = new ConcurrentStack<Glass>();
        ConcurrentStack<Chair> chairStack = new ConcurrentStack<Chair>();
        ConcurrentQueue<Guest> guestQueue = new ConcurrentQueue<Guest>();

        //Create Glasstack
        private void CreateAGlass()
        {
            Task.Run(() => { 
            for (int i = 0; i < 8; i++)
            {
                glasstack.Push(new Glass());
            }
            });
        }

        private void CreateAChair()
        {
            Task.Run(() => {
            for (int i = 0; i < 8; i++)
            {
                chairStack.Push(new Chair());
            }
            });
        }

        public void TheBar()
        {
            //Events och prenumerationer

            //Baren öpnnas events och prenumerationer
            b.BarenOppnas += Bartender_BarenOppnas;
            b.BarenOppnas += Guest_BarenOppnas;
            b.BarenOppnas += Waiter_BarenOppnas;

            //Baren stängs events och prenumerationer
            b.BarenStangs += Bartender_BarenStangs;
            b.BarenStangs += Guest_BarenStrangs;
            b.BarenStangs += Waiter_BarenStangs;


            //Bouncer events och prenumerationer
            bounce.GaDirektHem += Bouncer_GarDirektHem;

            //Guest events och prunumerationer
            //g.KommerInIPub += Guest_KommerInIPub;
            //g.LetarLedigStol += Guest_LetarEfterLedigStrol;
            //g.SatterSigNed += Guest_SattSigNer;
            //g.DrickaOlLamnaBar += Guest_DrickerOlGarHem;

            //Bartender evetns och prenumerationer
            //b.VantaiBar += Bartender_VantaIBaren;
            //b.PlockaGlasFranHylla += Bartender_PlockaGlasFranHyllan;
            //b.HallaUppOl += Bartender_HallaUppOl;
            b.BartenderGarHem += Bartender_GarHem;

            //Waiter events och prenumerationer
            w.PlockarTommaGlas += Waiter_PlockaTommaGlas;
            w.DiskarGlas += Waiter_DiskarGlas;
            w.StallarGlasIHylla += Waiter_StallerGlasIHyllan;


            //Tids-egenskapar sätts in till respektive labels
            w.AntalStolar = 2; //fejk-värde så länge.
            Dispatcher.Invoke(() => { lblAntalLedigaStolar.Content = "Antal lediga stolar " + w.AntalStolar; });

            b.AntalGlas = 2; //fejk-värde så länge.
            Dispatcher.Invoke(() => { lblAntalGlasIHylla.Content = "Antal glas i hyllan " + b.AntalGlas; });

            g.AntalGaster = 2; //fejk-värde så länge.
            Dispatcher.Invoke(() => { lblAntalGasterIBaren.Content = "Antal gäster i bar " + g.AntalGaster; });

            
            //Tasks

            //Task_Bar öppnas och stängs
            Task.Run(() =>
            {
                CreateAGlass();
                CreateAChair();
                if (barOpen)
                {
                    BarCountDown(); //barens öppettid: 120s.
                    b.Bartender_BarenOppnasMetod();
                    g.Guest_BarenOppnasMetod();
                    w.Waiter_BarenOppnasMetod();
                }
                else if (barOpen == false)
                {
                    b.Bartender_BarenStangsMetod();
                    g.Guest_BarenStangsMetod();
                    w.Waiter_BarenStangsMetod();
                }
            });


            //Task_Bouncer 
            Task.Run(() =>
            {
                bounce.Bouncer_GaDirektHemMethod();
                //Lägg in en till Action<Guest> CallGuest som param för kön.
                bounce.InviteGuest(Guest_KommerInIPub, AddGuestToQueue);
            });


            //Task_Guest
            Task.Run(() =>
            {
                
                g.KommerInIPubMetod();

                g.LetarLedigStolMetod();

                g.SatterSigNedMetod();

                g.DrickaOlLamnaBarMetod();
            });

            //Task_Bartender 
            Task.Run(() =>
            {
                b.Bartending(guestQueue, BartenderWork, glasstack, chairStack, BartenderPause);
                //b.VantaiBarMetod();

                //b.PlockaGlasFranHyllaMetod();

                //b.HallaUppOlMetod();

                //b.BartenderGarHemMetod();
            });


            //Task_Waiter 
            Task.Run(() =>
            {
                w.PlockarTommaGlasMetod();

                w.DiskarGlasMetod();

                w.StallarGlasIHyllaMetod();
            });
        } //method TheBar ends here!!


        //Nedräkningsmetod för barens öppettid: 120 sek (2min)
        public void BarCountDown()
        {
            int backwardnumber = 0; // skall vara global?

            for (backwardnumber = 120; backwardnumber >= 0; backwardnumber--)
            {
                Dispatcher.Invoke(() => { lblCountDown.Content = backwardnumber; });
                Thread.Sleep(1000);
            }
        }

        public void AddGuestToQueue(Guest guest)
        {
            Task.Run(() => { 
            guestQueue.Enqueue(guest);
            });
        }

        

        public void Bartender_BarenOppnas()
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            timeStamp += "_Baren öppnas!!";
            Dispatcher.Invoke(() =>
            {
                lstbBartender.Items.Insert(0, timeStamp);
            }); //Jari: Förslag på ny design, insert accepterade inte att man stoppa in datetime-objektet direkt i strängdelen som man kunde göra med add som du kan se nedan.
        }

        public void Bartender_BarenStangs()
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            timeStamp += "_Baren stängs!!";
            Dispatcher.Invoke(() => { lstbBartender.Items.Insert(0, timeStamp); });
        }

        public void BartenderWork(string whatBartenderDoes)
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            Dispatcher.Invoke(() =>
            {
                lstbBartender.Items.Insert(0, $"{timeStamp} " +  whatBartenderDoes);
            });
        }
        //public void Bartender_VantaIBaren()
        //{
        //    String timeStamp = GetTimestamp(DateTime.Now);
        //    timeStamp += "_Väntar i baren";
        //    Dispatcher.Invoke(() => { lstbBartender.Items.Insert(0, timeStamp); });
        //}

        //public void Bartender_PlockaGlasFranHyllan(string reachglass)
        //{
        //    String timeStamp = GetTimestamp(DateTime.Now);
        //    timeStamp += "_Plockar fram glas från hyllan";
        //    Dispatcher.Invoke(() => { lstbBartender.Items.Insert(0, timeStamp); });
        //}

        //public void Bartender_HallaUppOl(string pour)
        //{
        //    String timeStamp = GetTimestamp(DateTime.Now);
        //    timeStamp += "_Häller upp öl till kund";
        //    Dispatcher.Invoke(() => { lstbBartender.Items.Insert(0, timeStamp); });
        //}

        public void Bartender_GarHem()
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            timeStamp += "_Går hem!!";
            Dispatcher.Invoke(() => { lstbBartender.Items.Insert(0, timeStamp); });
        }


        //Utskrifts-metoder för Waiter
        public void Waiter_BarenOppnas()
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            timeStamp += "_Baren öppnas!!";
            Dispatcher.Invoke(() => { lstbServitor.Items.Insert(0, timeStamp); });
        }

        public void Waiter_BarenStangs()
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            timeStamp += "_Baren stängs!!";
            Dispatcher.Invoke(() => { lstbServitor.Items.Insert(0, timeStamp); });
        }


        public void Waiter_PlockaTommaGlas()
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            timeStamp += "_Plockar upp tomma glas'";
            Dispatcher.Invoke(() => { lstbServitor.Items.Insert(0, timeStamp); });
        }

        public void Waiter_DiskarGlas()
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            timeStamp += "_Diskar glas";
            Dispatcher.Invoke(() => { lstbServitor.Items.Insert(0, timeStamp); });
        }

        public void Waiter_StallerGlasIHyllan()
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            timeStamp += "_Ställer glas i hyllan";
            Dispatcher.Invoke(() => { lstbServitor.Items.Insert(0, timeStamp); });
        }

        //Utskrifts-metoder för Bouncer 
        public void Bouncer_GarDirektHem()
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            timeStamp += "_Går direkt hem";
            Dispatcher.Invoke(() => { lstbGaster.Items.Insert(0, timeStamp); });
        }

        //Utskrifts-metoder för Guest
        public void Guest_BarenOppnas()
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            timeStamp += "_Baren öppnas!!";
            Dispatcher.Invoke(() => { lstbGaster.Items.Insert(0, timeStamp); });
        }

        public void Guest_BarenStrangs()
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            timeStamp += "_Baren stängs!!";
            Dispatcher.Invoke(() => { lstbGaster.Items.Insert(0, timeStamp); });
        }

        public void Guest_KommerInIPub(string call)
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            timeStamp += call + " _Kommer in i puben";
            Dispatcher.Invoke(() => { lstbGaster.Items.Insert(0, timeStamp); });
        }

        public void Guest_LetarEfterLedigStrol(string chair)
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            timeStamp += "_Letar efter ledig stol";
            Dispatcher.Invoke(() => { lstbGaster.Items.Insert(0, timeStamp); });
        }

        public void Guest_SattSigNer(string sit)
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            timeStamp += "_Sätter sig ned på stol";
            Dispatcher.Invoke(() => { lstbGaster.Items.Insert(0, timeStamp); });
        }

        public void Guest_DrickerOlGarHem(string drick)
        {
            String timeStamp = GetTimestamp(DateTime.Now);
            timeStamp += "_Dricker ur ölen och går hem!!";
            Dispatcher.Invoke(() => { lstbGaster.Items.Insert(0, timeStamp); });
        }


        //hade en version förut där man bara skicka ett värde men då blev det massa text i anropen och rördigt när metoderna skulle prenumerera på en händelse samt metoderna fick ja ha ett generiskt namn för aktuell agent. så blir fulare å klumpigare här bland utskrifterna men tror det blirn bättre sedan vid anropen etc.  något att tänka på men gör så här så länge. annars är det ju ett solklart fall av att brya ner i metod!!!


        //public void InviteGuest()
        //{
        //    //Bouncer
        //    //Inkastaren släpper in kunder slumpvis, efter tre till tio sekunder. Inkastaren kontrollerar leg, så att alla i baren kan veta vad kunden heter. (Slumpa ett namn åt nya kunder från en lista) Inkastaren slutar släppa in nya kunder när baren stänger och går hem direkt.

        //    _theNames.Add("Izola ");
        //    _theNames.Add("Jeane");
        //    _theNames.Add("Christiane");
        //    _theNames.Add("Taneka");
        //    _theNames.Add("Debbra");
        //    _theNames.Add("Terrilyn");
        //    _theNames.Add("Fransisca");
        //    _theNames.Add("Zetta");
        //    _theNames.Add("Zina");

        //    Task t1 = Task.Run(() =>
        //    {
        //        //int actionCount = 0; 
        //        //bool pubOpen = true;
        //        while (barOpen)
        //        {
        //            Random timeRandom = new Random();
        //            String timeStamp = GetTimestamp(DateTime.Now);
        //            Thread.Sleep(timeRandom.Next(3000,10000));
        //            Dispatcher.Invoke(() =>
        //            {
        //                int r = timeRandom.Next(_theNames.Count);
        //                lstbGaster.Items.Insert(0,timeStamp + " " +_theNames.ElementAt(r) + " kommer in i baren.");
        //            });
        //        }
        //    });

        //}


        //barens startknapp :D
        private void btnOpenCloseBar_Click(object sender, RoutedEventArgs e)
        {
            //STARTAR OCH STOPPAR PROGRAMMET OCH BAREN!!!!!!!!!!  :D
            barOpen = !barOpen;
            TheBar();
        }

        private void btnBartender_Click(object sender, RoutedEventArgs e)
        {
            BartenderPause = !BartenderPause;
        }
    } //mainwindow class ends here


    // OTHER CLASSES HERE:

    public class Bouncer
    {
        public Action<string> CallBack;
        public Action<Guest> CallGuest;
        //Lägg in en till Action<Guest> CallGuest som param för kön.
        public void InviteGuest(Action<string> callback, Action<Guest> callGuest)
        {
            //Bouncer
            //Inkastaren släpper in kunder slumpvis, efter tre till tio sekunder. Inkastaren kontrollerar leg, så att alla i baren kan veta vad kunden heter. (Slumpa ett namn åt nya kunder från en lista) Inkastaren slutar släppa in nya kunder när baren stänger och går hem direkt.

            CallBack = callback;
            CallGuest = callGuest;

            List<string> _listOfGuests = new List<string>
            {
                "Izola",
                "Jeane",
                "Christiane",
                "Taneka",
                "Debbra",
                "Terrilyn",
                "Fransisca",
                "Zetta",
                "Zina"
            };


            Task t1 = Task.Run(() =>
            {
                //int actionCount = 0; 
                //bool pubOpen = true;
                while (true)
                {
                    Random timeRandom = new Random();
                    //String timeStamp = GetTimestamp(DateTime.Now);
                    Thread.Sleep(timeRandom.Next(3000, 10000));
                    int r = timeRandom.Next(_listOfGuests.Count);
                    string nameOfGuest = _listOfGuests[r];
                    callback($"{nameOfGuest}");
                    callGuest(new Guest(nameOfGuest));
                }
            });
        }

        public event Action GaDirektHem;

        private int slappaInGastTid = 0;
        public int SlappaInGastTid { get; set; } //(3-10s) 

        public void Bouncer_GaDirektHemMethod()
        {
            GaDirektHem?.Invoke();
        }
    }


    //Jari:  iom dessa klasser anropas från main via varsin respektive task så ingår ju redan de här metoderna i de trådarna som jag anser det.  de triggar ju vad som sedan via ovanstående utskriftsmetoder blir själva callback:et.  så om den tolkningen är rätt så blir det ju meninglösa att se de här metoderna i en task...när de redan är del av den iom anropen och deras triggning tillbaka till respektiva tråd i respektive task.  ällä???   låtar dem vara så länge iaf.   kanske itne ens går att dynga dit en task i varje klass här??
}


public class Guest
{
    public string Name { get; set; }
    public Guest(string name)
    {
        Name = name;
    }

    public event Action KommerInIPub, LetarLedigStol, SatterSigNed, DrickaOlLamnaBar, BarenOppnas, BarenStangs;

    private int antalGaster = 0;
    public int AntalGaster { get; set; } //gäster-räknare. skall vara global?

    private int kommaTillBarenTid = 0;
    public int KommaTillBarenTid { get; set; } //(1s)

    private int gaTillStolTid = 0;
    public int GaTillStolTid { get; set; } //(4s)

    private int drickaOlTid = 0;
    public int DrickaOlTid { get; set; } //(10-20s) 

    public void Guest_BarenOppnasMetod()
    {
        BarenOppnas?.Invoke();
    }

    public void Guest_BarenStangsMetod()
    {
        BarenStangs?.Invoke();
    }

    public void KommerInIPubMetod()
    {
        KommerInIPub?.Invoke();
    }

    public void LetarLedigStolMetod()
    {
        LetarLedigStol?.Invoke();
    }

    public void SatterSigNedMetod()
    {
        SatterSigNed?.Invoke();
    }

    public void DrickaOlLamnaBarMetod()
    {
        DrickaOlLamnaBar?.Invoke();
    }
}


public class Bartender
{
    private Action<string> Callback;
    private ConcurrentQueue<Guest> GuestQueue;
    private ConcurrentStack<Glass> GlassStack;
    private ConcurrentStack<Chair> ChairStack;


    public void Bartending(ConcurrentQueue<Guest> guestQueue, Action<string> callback, ConcurrentStack<Glass> glassStack, ConcurrentStack<Chair> chairStack, bool barOpen)
    {
       Callback = callback;
       GuestQueue = guestQueue;
       GlassStack = glassStack;
        ChairStack = chairStack;

        Task.Run(() =>
        {
            while (barOpen) //Kommer att kolla om baren är öppen
            {
                
                Thread.Sleep(1000);

                if (!guestQueue.IsEmpty)
                {
                    if (!glassStack.IsEmpty)


                    {
                        callback("Bartendern tar ett glas från glashyllan.");
                        Thread.Sleep(3000);
                        GlassStack.TryPop(out Glass glass);
                        if (!chairStack.IsEmpty)
                        {
                            ChairStack.TryPop(out Chair chair );
                            callback($"Bartendern häller upp en öl till {guestQueue.First().Name}.");
                            Thread.Sleep(3000);
                        }
                        else
                        {
                            callback($"{guestQueue.First().Name}. väntar på en stol");
                        }
                        guestQueue.TryDequeue(out Guest guest);
                    }


                    else
                    {
                        callback("Bartendern väntar på glass");
                    }




                }
                else
                {
                    callback("Bartendern löser sudoku medan han väntar på kunder.");
                }
            }
        });
    }
    public event Action VantaiBar, PlockaGlasFranHylla, HallaUppOl, BartenderGarHem, BarenOppnas, BarenStangs;

    private int antalGlas = 0;
    public int AntalGlas { get; set; } //skall vara global??

    private int hamtaGlasTid = 0;
    public int HamtaGlasTid { get; set; } //(3s)

    private int hallaOlTid = 0;
    public int HallaOlTid { get; set; } //(3s)

    public void Bartender_BarenOppnasMetod()
    {
        BarenOppnas?.Invoke();
    }

    public void Bartender_BarenStangsMetod()
    {
        BarenStangs?.Invoke();
    }

    public void VantaiBarMetod()
    {
        VantaiBar?.Invoke();
    }

    public void PlockaGlasFranHyllaMetod()
    {
        PlockaGlasFranHylla?.Invoke();
    }

    public void HallaUppOlMetod()
    {
        HallaUppOl?.Invoke();
    }

    public void BartenderGarHemMetod()
    {
        BartenderGarHem?.Invoke();
    }


    //ingen task, metod som anropar häll upp öl osv. task i main dock!! (dave)  SKALL VI BAARA EN LR NÅGRA TASKS I MIAN DÅ OCH FRÅN DEN/DEMStrANROPA METODER                                                                           I KLASSER ETC???  JAG HAR DOCK GJORT  LITE TRÅDAR I VARJE KLASS. FÅR VÄL                                                                              ÄNDRA PÅ DET SEDAN ISF??
}

public class Glass
{
    
}

public class Chair
{
    
}

public class Waiter
{
    public event Action PlockarTommaGlas, DiskarGlas, StallarGlasIHylla, BarenOppnas, BarenStangs;

    private int antalStolar = 0;
    public int AntalStolar { get; set; } //skall vara global??

    private int plockaTommaGlasTid = 0;
    public int PlockaTommaGlasTid { get; set; } //(10s)  

    private int diskaGlasTid = 0;
    public int DiskaGlasTid { get; set; } //(15s)

    public void Waiter_BarenOppnasMetod()
    {
        BarenOppnas?.Invoke();
    }

    public void Waiter_BarenStangsMetod()
    {
        BarenStangs?.Invoke();
    }

    public void PlockarTommaGlasMetod()
    {
        PlockarTommaGlas?.Invoke();
    }

    public void DiskarGlasMetod()
    {
        DiskarGlas?.Invoke();
    }

    public void StallarGlasIHyllaMetod()
    {
        StallarGlasIHylla?.Invoke();
    }
}


//EOF