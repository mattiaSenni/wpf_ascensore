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
            Ascensore = new Ascensore(1);
            muovi = new Thread(new ThreadStart(MuoviAscensore));
            mossa = 10;
            sleep = 100;
            inMovimento = false;
            x = new object();

            o0 = new Omino(p_0.Margin, 50);
            o1 = new Omino(p_1.Margin, 50);
            o2 = new Omino(p_2.Margin, 50);
            o3 = new Omino(p_3.Margin, 50);
            o4 = new Omino(p_4.Margin, 50);
        }
        Ascensore Ascensore;
        Piano Destinazione;
        Thread muovi;
        private double mossa;
        private int sleep;
        private bool inMovimento;
        object x;

        

        private void btnPrenota_Click(object sender, RoutedEventArgs e)
        {
            //riconosco il piano in base al nome del bottone
            Button myButton = (Button)sender;
            int piano = int.Parse(myButton.Name.Split('_')[1]);            
            if(Ascensore.Piani[piano].NPersone > 0)
            {
                Ascensore.Prenota(piano);
                lstPiani.ItemsSource = Ascensore.Fila;
                if(!Ascensore.AspettaPrenotazione || Ascensore.Persone == 0)
                {
                    Muovi();
                }
            }
        }
        
        private void MuoviAscensore()
        {
            //devo capire se scende o se sale
            //TODO : Animazione
            int pSalite;
            Thickness t = new Thickness(0);

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                t = grdAscensore.Margin;
            }));

            Thread.Sleep(150);
            topCorrente = t.Top;
            topDestinazione = Destinazione.MarginTop;
            //AnimazioneAscensore();
            Thread m = new Thread(new ThreadStart(AnimazioneAscensore));
            m.Start();
            m.Join();

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
                t = grdAscensore.Margin;
                lbl4.Content = Ascensore.Piani[4].NPersone;
                lbl3.Content = Ascensore.Piani[3].NPersone;
                lbl2.Content = Ascensore.Piani[2].NPersone;
                lbl1.Content = Ascensore.Piani[1].NPersone;
                lbl0.Content = Ascensore.Piani[0].NPersone;
                lblNumPersone.Content = Ascensore.Persone;
            }));

            if (Ascensore.Count() > 0 && !Ascensore.AspettaPrenotazione)
            {
                try
                {
                    Destinazione = Ascensore.Vai();
                    MuoviAscensore();
                }
                catch (Exception ex)
                {

                    MessageBox.Show("zeru tituli");
                }
            }

        }

        private double topCorrente;
        private double topDestinazione;

        private void AnimazioneAscensore()
        {
            double differenza = topDestinazione - topCorrente;
            bool sale = true;
            if (differenza < 0)
            {
                sale = false;
                differenza *= -1; //la rendo positiva
            }
            //MessageBox.Show("differenza = " + differenza + ", mossa = " + mossa);
            if (differenza > mossa)
            {
                if (sale)
                    topCorrente += mossa;
                else
                    topCorrente -= mossa;
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    //Thickness t = grdAscensore.Margin;
                    grdAscensore.Margin = new Thickness(434, topCorrente, 0, 0);

                }));
                Thread.Sleep(sleep);
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
                    p_0.Margin = new Thickness(0, p_0.Margin.Top, 0, 0);
                    p_0.Visibility = Visibility.Visible;
                    Thread t = new Thread(new ThreadStart(aggiungi0));
                    t.Start();
                    break;
                case 1:
                    p_1.Margin = new Thickness(0, p_1.Margin.Top, 0, 0);
                    p_1.Visibility = Visibility.Visible;
                    Thread t1 = new Thread(new ThreadStart(aggiungi1));
                    t1.Start();
                    break;
                case 2:
                    p_2.Margin = new Thickness(0, p_2.Margin.Top, 0, 0);
                    p_2.Visibility = Visibility.Visible;
                    Thread t2 = new Thread(new ThreadStart(aggiungi2));
                    t2.Start();
                    break;
                case 3:
                    p_3.Margin = new Thickness(0, p_3.Margin.Top, 0, 0);
                    p_3.Visibility = Visibility.Visible;
                    Thread t3 = new Thread(new ThreadStart(aggiungi3));
                    t3.Start();
                    break;
                case 4:
                    p_4.Margin = new Thickness(0, p_4.Margin.Top, 0, 0);
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
                    p_3.Margin = o1.Mossa();
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

        private void btnTastierino_Click(object sender, RoutedEventArgs e)
        {
            if(Ascensore.Persone > 0)
            {
                Button b = (Button)sender;
                int piano = int.Parse(b.Name.Split('_')[1]);
                Ascensore.Tastierino(piano);
                lstPiani.ItemsSource = Ascensore.Fila;
                //MessageBox.Show(Ascensore.Fila[0].ToString());
                Muovi();
            }
        }

        private void Muovi()
        {
            
            lock(x)
            {
                //MessageBox.Show(muovi.IsAlive.ToString());
                if (!inMovimento && !muovi.IsAlive)
                {
                    lstPiani.ItemsSource = Ascensore.Fila;
                    //il thread è fermo, lo faccio ripartire
                    Destinazione = Ascensore.Vai();
                    muovi = new Thread(new ThreadStart(MuoviAscensore));
                    muovi.Start();
                    inMovimento = true;
                    muovi.Join();
                    inMovimento = false;
                }
            }
        }
    }
}
