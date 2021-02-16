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
        }
        Ascensore Ascensore;
        Piano Destinazione;
        Thread muovi;


        private void btnPrenota_Click(object sender, RoutedEventArgs e)
        {
            //riconosco il piano in base al nome del bottone
            Button myButton = (Button)sender;
            int piano = int.Parse(myButton.Name.Split('_')[1]);            
            if(Ascensore.Piani[piano].NPersone > 0)
            {
                Ascensore.Prenota(piano);
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
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                Thickness t = grdAscensore.Margin;
                grdAscensore.Margin = new Thickness(t.Left, Destinazione.MarginTop, 0, 0);
                pSalite = Ascensore.Arrivato();
                lbl4.Content = Ascensore.Piani[4].NPersone;
                lbl3.Content = Ascensore.Piani[3].NPersone;
                lbl2.Content = Ascensore.Piani[2].NPersone;
                lbl1.Content = Ascensore.Piani[1].NPersone;
                lbl0.Content = Ascensore.Piani[0].NPersone;
                lblNumPersone.Content = Ascensore.Persone;
            }));

            if(Ascensore.Count() > 0 && !Ascensore.AspettaPrenotazione)
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
        }

        private void btnTastierino_Click(object sender, RoutedEventArgs e)
        {
            if(Ascensore.Persone > 0)
            {
                Button b = (Button)sender;
                int piano = int.Parse(b.Name.Split('_')[1]);
                Ascensore.Tastierino(piano);
                //MessageBox.Show(Ascensore.Fila[0].ToString());
                Muovi();
            }
        }

        private void Muovi()
        {
            
            //MessageBox.Show(muovi.IsAlive.ToString());
            if(!muovi.IsAlive)
            {
                
                //il thread è fermo, lo faccio ripartire
                Destinazione = Ascensore.Vai();
                muovi = new Thread(new ThreadStart(MuoviAscensore));
                muovi.Start();
            }
        }
    }
}
