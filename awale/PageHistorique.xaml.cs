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

namespace awale
{
    /// <summary>
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Historique historique { get; set; }

        public Page1()
        {
            this.historique = new Historique("./res.csv");
            InitializeComponent();

            List<Personne> joueur1 = new List<Personne>();
            List<Personne> joueur2 = new List<Personne>();

            List<Personne> AllPlayer = this.historique.ReadCSV().ToList<Personne>();

            for (int i = 0; i < AllPlayer.Count; i++)
            {
                if(i%2 == 0)
                {
                    joueur1.Add(AllPlayer.ElementAt(i));
                }
                else
                {
                    joueur2.Add(AllPlayer.ElementAt(i));
                }
            }


            List<Partie> parties = new List<Partie>();
            for (int i = 0; i < joueur1.Count; i++)
            {
                parties.Add(new Partie(joueur1.ElementAt(i), joueur2.ElementAt(i)));
            }

            ListViewParties.ItemsSource = parties;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
