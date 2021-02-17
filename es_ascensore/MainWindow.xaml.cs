using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Threading;

namespace es_ascensore
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Ascensore = new Ascensore(3);
            muovi = new Thread(new ThreadStart(MuoviAscensore));
            mossa = 10;
            sleep = 100;
            
            x = new object();

            o0 = new Omino(p_0.Margin, 50);
            o1 = new Omino(p_1.Margin, 50);
            o2 = new Omino(p_2.Margin, 50);
            o3 = new Omino(p_3.Margin, 50);
            o4 = new Omino(p_4.Margin, 50);
            c0 = new OminoContrario(s0.Margin, 50);
            c1 = new OminoContrario(s1.Margin, 50);
            c2 = new OminoContrario(s2.Margin, 50);
            c3 = new OminoContrario(s3.Margin, 50);
            c4 = new OminoContrario(s4.Margin, 50);
        }
        Ascensore Ascensore;
        Piano Destinazione;
        Thread muovi;
        private double mossa;
        private int sleep;
        
        object x;

        

        private void btnPrenota_Click(object sender, RoutedEventArgs e)
        {
            //riconosco il piano in base al nome del bottone
            Button myButton = (Button)sender;
            int piano = int.Parse(myButton.Name.Split('_')[1]);            
            if(Ascensore.Piani[piano].NPersone > 0)
            {
                Ascensore.Prenota(piano);
                //lstPiani.ItemsSource = Ascensore.Fila;
                if(Ascensore.Persone == 0)
                {
                    Muovi();
                }
            }
        }
        
        private void MuoviAscensore()
        {
            //devo capire se scende o se sale
            //TODO : Animazione
            int pSalite, pScese;
            Thickness t = new Thickness(0);

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                t = grdAscensore.Margin;
            }));

            Thread.Sleep(150);
            topCorrente = t.Top;
            topDestinazione = Destinazione.MarginTop;
            //AnimazioneAscensore();
            /*Thread m = new Thread(new ThreadStart(AnimazioneAscensore));
            m.Start();
            m.Join();*/
            AnimazioneAscensore();

            pScese = Ascensore.QuantiScendono();
            pSalite = Ascensore.Arrivato();
            bool cancellaOmino = true;
            if (Ascensore.Piani[Ascensore.Piano].NPersone > 0)
                cancellaOmino = false;
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if(cancellaOmino)
                {
                    switch(Ascensore.Piano)
                    {
                        case 0:
                            o0 = new Omino(new Thickness(0, o0.MarginTop, 0, 0), 50);
                            p_0.Visibility = Visibility.Hidden;
                            p_0.Margin = new Thickness(0, o0.MarginTop, 0, 0);
                            break;
                        case 1:
                            o1 = new Omino(new Thickness(0, o1.MarginTop, 0, 0), 50);
                            p_1.Visibility = Visibility.Hidden;
                            p_1.Margin = new Thickness(0, o1.MarginTop, 0, 0);
                            break;
                        case 2:
                            o2 = new Omino(new Thickness(0, o2.MarginTop, 0, 0), 50);
                            p_2.Visibility = Visibility.Hidden;
                            p_2.Margin = new Thickness(0, o2.MarginTop, 0, 0);
                            break;
                        case 3:
                            o3 = new Omino(new Thickness(0, o3.MarginTop, 0, 0), 50);
                            p_3.Visibility = Visibility.Hidden;
                            p_3.Margin = new Thickness(0, o3.MarginTop, 0, 0);
                            break;
                        case 4:
                            o4 = new Omino(new Thickness(0, o4.MarginTop, 0, 0), 50);
                            p_4.Visibility = Visibility.Hidden;
                            p_4.Margin = new Thickness(0, o4.MarginTop, 0, 0);
                            break;
                    }
                }
                if(pScese > 0)
                {
                    Thread tr = new Thread(new ThreadStart(Scendi0));
                    
                    switch (Ascensore.Piano)
                    {
                        case 0:
                            tr = new Thread(new ThreadStart(Scendi0));
                            break;
                        case 1:
                            tr = new Thread(new ThreadStart(Scendi1));
                            break;
                        case 2:
                            tr = new Thread(new ThreadStart(Scendi2));
                            break;
                        case 3:
                            tr = new Thread(new ThreadStart(Scendi3));
                            break;
                        case 4:
                            tr = new Thread(new ThreadStart(Scendi4));
                            break;
                    }
                    //MessageBox.Show("anima");
                    tr.Start();
                }
                t = grdAscensore.Margin;
                lbl4.Content = Ascensore.Piani[4].NPersone;
                lbl3.Content = Ascensore.Piani[3].NPersone;
                lbl2.Content = Ascensore.Piani[2].NPersone;
                lbl1.Content = Ascensore.Piani[1].NPersone;
                lbl0.Content = Ascensore.Piani[0].NPersone;
                lblNumPersone.Content = Ascensore.Persone;
            }));

            Thread.Sleep(20);//per dare il tempo alla grafica di aggiornarsi
            while (true)
            {
                //MessageBox.Show(Ascensore.Count().ToString());
                if (Ascensore.Count() > 0)
                {
                    try
                    {
                        string x = "";
                        foreach (int n in Ascensore.Fila)
                        {
                            x += n + ", ";
                        }
                        //MessageBox.Show(x);
                        Destinazione = Ascensore.Vai();
                        MuoviAscensore();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("errore");
                    }

                }
                Thread.Sleep(20);
            }
            

        }

        private double topCorrente;
        private double topDestinazione;

        OminoContrario c0;
        OminoContrario c1;
        OminoContrario c2;
        OminoContrario c3;
        OminoContrario c4;
        private void AnimazioneAscensore()
        {
            double differenza = topDestinazione - topCorrente;
            bool sale = true;
            if (differenza < 0)
            {
                sale = false;
                differenza *= -1; //la rendo positiva
            }

            if (differenza > mossa)
            {
                if (sale)
                    topCorrente += mossa;
                else
                    topCorrente -= mossa;
                Thread.Sleep(sleep);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    //Thickness t = grdAscensore.Margin;
                    grdAscensore.Margin = new Thickness(434, topCorrente, 0, 0);

                }));
                
                //MessageBox.Show("top corrente = " + topCorrente);
                AnimazioneAscensore();

            }
            else
            {
                if (sale)
                    topCorrente += differenza;
                else
                    topCorrente -= differenza;
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Thickness t = grdAscensore.Margin;
                    grdAscensore.Margin = new Thickness(t.Left, topCorrente, 0, 0);

                }));

            }
        }


        private void btnAggiungiPersona_Click(object sender, RoutedEventArgs e)
        {
            //riconosco il piano dal nome di sender
            Button b = (Button)sender;
            int piano = int.Parse(b.Name.Split('_')[1]);
            Ascensore.Piani[piano].AggiungiPersona();
            //aggiorno tutti i contatori
            lbl4.Content = Ascensore.Piani[4].NPersone;
            lbl3.Content = Ascensore.Piani[3].NPersone;
            lbl2.Content = Ascensore.Piani[2].NPersone;
            lbl1.Content = Ascensore.Piani[1].NPersone;
            lbl0.Content = Ascensore.Piani[0].NPersone;
            //TODO : fare l'animazione dell'aggiunta della persona
            switch (piano)
            {
                case 0:
                    //p_0.Margin = new Thickness(0, p_0.Margin.Top, 0, 0);
                    p_0.Visibility = Visibility.Visible;
                    Thread t = new Thread(new ThreadStart(aggiungi0));
                    t.Start();
                    break;
                case 1:
                    //p_1.Margin = new Thickness(0, p_1.Margin.Top, 0, 0);
                    p_1.Visibility = Visibility.Visible;
                    Thread t1 = new Thread(new ThreadStart(aggiungi1));
                    t1.Start();
                    break;
                case 2:
                    //p_2.Margin = new Thickness(0, p_2.Margin.Top, 0, 0);
                    p_2.Visibility = Visibility.Visible;
                    Thread t2 = new Thread(new ThreadStart(aggiungi2));
                    t2.Start();
                    break;
                case 3:
                    //p_3.Margin = new Thickness(0, p_3.Margin.Top, 0, 0);
                    p_3.Visibility = Visibility.Visible;
                    Thread t3 = new Thread(new ThreadStart(aggiungi3));
                    t3.Start();
                    break;
                case 4:
                    //p_4.Margin = new Thickness(0, p_4.Margin.Top, 0, 0);
                    p_4.Visibility = Visibility.Visible;
                    Thread t4 = new Thread(new ThreadStart(aggiungi4));
                    t4.Start();
                    break;
            }
        }
        Omino o0;
        Omino o1;
        Omino o2;
        Omino o3;
        Omino o4;
        private void aggiungi0()
        {
            if (o0.MarginLeft >= 300)
            {
                return;
            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    p_0.Margin = o0.Mossa();
                }));
                Thread.Sleep(50);
                aggiungi0();
            }
        }
        private void aggiungi1()
        {
            if (o1.MarginLeft >= 300)
            {
                return;
            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    p_1.Margin = o1.Mossa();
                }));
                Thread.Sleep(50);
                aggiungi1();
            }
        }
        private void aggiungi2()
        {
            if (o2.MarginLeft >= 300)
            {
                return;
            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    p_2.Margin = o2.Mossa();
                }));
                Thread.Sleep(50);
                aggiungi2();
            }
        }
        private void aggiungi3()
        {
            if (o3.MarginLeft >= 300)
            {
                return;
            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    p_3.Margin = o3.Mossa();
                }));
                Thread.Sleep(50);
                aggiungi3();
            }
        }
        private void aggiungi4()
        {
            if (o4.MarginLeft >= 300)
            {
                return;
            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    p_4.Margin = o4.Mossa();
                }));
                Thread.Sleep(50);
                aggiungi4();
            }
        }

        private void Scendi0()
        {
            if(c0.MarginLeft <= 0)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    s0.Visibility = Visibility.Hidden;
                    s0.Margin = new Thickness(400, c0.MarginTop, 0, 0);
                    c0 = new OminoContrario(s0.Margin, 50);
                }));
                
            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    s0.Visibility = Visibility.Visible;
                    s0.Margin = c0.Mossa();
                }));
                Thread.Sleep(50);
                Scendi0();
            }
        }

        private void Scendi4()
        {
            if (c4.MarginLeft <= 0)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    s4.Visibility = Visibility.Hidden;
                    s4.Margin = new Thickness(400, c4.MarginTop, 0, 0);
                    c4 = new OminoContrario(s4.Margin, 50);
                }));

            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    s4.Visibility = Visibility.Visible;
                    s4.Margin = c4.Mossa();
                }));
                Thread.Sleep(50);
                Scendi4();
            }
        }

        private void Scendi1()
        {
            if (c1.MarginLeft <= 0)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    s1.Visibility = Visibility.Hidden;
                    s1.Margin = new Thickness(400, c1.MarginTop, 0, 0);
                    c1 = new OminoContrario(s1.Margin, 50);
                }));

            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    s1.Visibility = Visibility.Visible;
                    s1.Margin = c1.Mossa();
                }));
                Thread.Sleep(50);
                Scendi1();
            }
        }

        private void Scendi2()
        {
            if (c2.MarginLeft <= 0)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    s2.Visibility = Visibility.Hidden;
                    s2.Margin = new Thickness(400, c2.MarginTop, 0, 0);
                    c2 = new OminoContrario(s2.Margin, 50);
                }));

            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    s2.Visibility = Visibility.Visible;
                    s2.Margin = c2.Mossa();
                }));
                Thread.Sleep(50);
                Scendi2();
            }
        }

        private void Scendi3()
        {
            if (c3.MarginLeft <= 0)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    s3.Visibility = Visibility.Visible;
                    s3.Visibility = Visibility.Hidden;
                    s3.Margin = new Thickness(400, c3.MarginTop, 0, 0);
                    c3 = new OminoContrario(s3.Margin, 50);
                }));

            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    s3.Margin = c3.Mossa();
                }));
                Thread.Sleep(50);
                Scendi3();
            }
        }

        private void btnTastierino_Click(object sender, RoutedEventArgs e)
        {
            if(Ascensore.Persone > 0)
            {
                Button b = (Button)sender;
                int piano = int.Parse(b.Name.Split('_')[1]);
                Ascensore.Tastierino(piano);
                //lstPiani.ItemsSource = Ascensore.Fila;
                //MessageBox.Show(Ascensore.Fila[0].ToString());
                Muovi();
            }
        }

        private void Muovi()
        {
            
            if(!muovi.IsAlive)
            {
                lock (x)
                {
                    //lstPiani.ItemsSource = Ascensore.Fila;
                    //il thread è fermo, lo faccio ripartire
                    Destinazione = Ascensore.Vai();
                    muovi = new Thread(new ThreadStart(MuoviAscensore));
                    muovi.Start();
                }
            }
        }
    }
}
