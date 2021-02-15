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
        }
        Ascensore Ascensore;
        Piano Destinazione;


        private void btnPrenota_Click(object sender, RoutedEventArgs e)
        {
            //riconosco il piano in base al nome del bottone
            Button myButton = (Button)sender;
            int piano = int.Parse(myButton.Name.Split('_')[1]);            
            Ascensore.Prenota(piano);
            //MessageBox.Show(Ascensore.Fila.Peek().ToString());
            if (!Ascensore.StaAndando)//controllo se l'ascensore sta andando
            {
                //MessageBox.Show("parti");
                Destinazione = Ascensore.Vai();//se non sta andando la faccio partire
                Thread muovi = new Thread(new ThreadStart(MuoviAscensore));
                muovi.Start();
            }
        }

        private void MuoviAscensore()
        {
            //devo capire se scende o se sale
            //TODO : Animazione
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                Thickness t = grdAscensore.Margin;
                grdAscensore.Margin = new Thickness(t.Left, Destinazione.MarginTop, 0, 0);
                Ascensore.Arrivato();
                lbl2.Content = Ascensore.Piani[2].Persone.Count;
                lbl1.Content = Ascensore.Piani[1].Persone.Count;
                lbl0.Content = Ascensore.Piani[0].Persone.Count;
                lblNumPersone.Content = Ascensore.Persone.Count;
            }));
            

            //assegno l'effettivo numero di persone
            
            if (Ascensore.Continua())
            {
                Destinazione = Ascensore.Vai();
                MuoviAscensore();
            }
        }
    }
}
