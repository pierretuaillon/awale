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
using System.Collections.ObjectModel;

namespace awale
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {


        public ObservableCollection<Trou> trous { get; set;}
        public Personne joueur1 { get; set; }
        public Personne joueur2 { get; set; }

        MoteurDeJeu moteur { get; set; }
        public MainWindow()
        {
            initializeList();
            InitializeComponent();
            this.DataContext = this;
            rendreInvisible();
        }

        

        public void initializeList()
        {
            System.Console.WriteLine("/**** Initialisation de la list ****/");
            this.trous = new ObservableCollection<Trou>();
            //Trous "Ally"
            Trou trouA = new Trou("A", 4);
            Trou trouB = new Trou("B", 4, trouA);
            Trou trouC = new Trou("C", 4, trouB);
            Trou trouD = new Trou("D", 4, trouC);
            Trou trouE = new Trou("E", 4, trouD);
            Trou trouF = new Trou("F", 4, trouE);
            //Trous "Ennemy"
            Trou troua = new Trou("a", 4, trouF);
            Trou troub = new Trou("b", 4, troua);
            Trou trouc = new Trou("c", 4, troub);
            Trou troud = new Trou("d", 4, trouc);
            Trou troue = new Trou("e", 4, troud);
            Trou trouf = new Trou("f", 4, troue);

            //Definition du predecesseur manquan
            trouA.predecesseur = trouf;
        
            //Definition des successeur
            trouA.successeur = trouB;
            trouB.successeur = trouC;
            trouC.successeur = trouD;
            trouD.successeur = trouE;
            trouE.successeur = trouF;
            trouF.successeur = troua;
            troua.successeur = troub;
            troub.successeur = trouc;
            trouc.successeur = troud;
            troud.successeur = troue;
            troue.successeur = trouf;
            trouf.successeur = trouA;

            //Ajout des elements à la liste
            trous.Add(trouA);
            trous.Add(trouB);
            trous.Add(trouC);
            trous.Add(trouD);
            trous.Add(trouE);
            trous.Add(trouF);
            trous.Add(troua);
            trous.Add(troub);
            trous.Add(trouc);
            trous.Add(troud);
            trous.Add(troue);
            trous.Add(trouf);

            System.Console.WriteLine("/**** FIN Initialisation de la list ****/");
            this.joueur1 = new Personne("Joueur 1");
            this.joueur2 = new Personne("Joueur 2");


            //On assigne aux joueurs leurs trous
            for (int i = 0; i < this.trous.Count/2; i++)
            {
                this.joueur1.listTrouPerso.Add(this.trous.ElementAt(i));
            }
            System.Console.WriteLine("Nombre d'element dans la liste de trous " + this.trous.Count);
            for (int i = this.trous.Count-1; i >= this.trous.Count / 2; i--)
            {
                this.joueur2.listTrouPerso.Add(this.trous.ElementAt(i));
            }

            /*
            foreach (var trou in this.joueur1.listTrouPerso)
            {
                System.Console.WriteLine("J1 : " + trou.nom);
            }

            foreach (var trou in this.joueur2.listTrouPerso)
            {
                System.Console.WriteLine("J2 : " + trou.nom);
            }
            */
            //Normalement les joueurs ont le même nombre de trous sinon t'es pas serein
            System.Console.WriteLine("Le joueur 1 possede " + this.joueur1.listTrouPerso.Count + " trous");
            System.Console.WriteLine("Le joueur 2 possede " + this.joueur2.listTrouPerso.Count + " trous");
            this.moteur = new MoteurDeJeu(this.trous);
            this.moteur.currentJoueur = joueur1;
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Console.WriteLine("/**** Action sur un TextBox ****/");
            TextBox objclick = (TextBox)sender;

            Trou elementRecherche = null;
            for (int i = 0; i < this.trous.Count; i++)
            {
                if (this.trous.ElementAt(i).nom == objclick.Name)
                {
                    elementRecherche = this.trous.ElementAt(i);
                }
            }

            if (this.moteur.finPartie())
            {

                labelInformation.Text = "Fin de la partie ";
                if(this.joueur1.score > this.joueur2.score)
                {
                    labelInformation.Text += "\n Le joueur : " + this.joueur1.nom + " gagne !";
                }
                else
                {
                    labelInformation.Text += "\n Le joueur : " + this.joueur2.nom + " gagne !";
                }
            }
            else
            {
                if (this.moteur.actionPossible(elementRecherche))
                {
                    this.moteur.faireAction(elementRecherche);

                    //Si le joueur courrant est le joueur 1
                    if (this.moteur.currentJoueur.nom == this.joueur1.nom)
                    {
                        this.moteur.currentJoueur = this.joueur2;
                    }
                    //Sinon c'est forcement le joueur 2
                    else
                    {
                        this.moteur.currentJoueur = this.joueur1;
                    }
                    labelInformation.Text = "Au tour du joueur : " + this.moteur.currentJoueur.nom;
                }
                else
                {
                    labelInformation.Text = "Vous ne pouvez utiliser les graines de l'adversaire ! ";
                }
            }

            
                        

            System.Console.WriteLine("/**** MISE A JOUR DE LA VUE ****/");
            this.trous = this.moteur.trous;

            System.Console.WriteLine("/**** Contenue des trous ****/");
            foreach (var trou in this.trous)
            {
                System.Console.WriteLine(trou.nombreGraines);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine("/**** Click sur un element du menu Partie ****/");

        }


        public void rendreInvisible()
        {
            plateauJ1.Visibility = Visibility.Hidden;
            plateauJ2.Visibility = Visibility.Hidden;
            A.Visibility = Visibility.Hidden;
            B.Visibility = Visibility.Hidden;
            C.Visibility = Visibility.Hidden;
            D.Visibility = Visibility.Hidden;
            E.Visibility = Visibility.Hidden;
            F.Visibility = Visibility.Hidden;
            a.Visibility = Visibility.Hidden;
            b.Visibility = Visibility.Hidden;
            c.Visibility = Visibility.Hidden;
            d.Visibility = Visibility.Hidden;
            e.Visibility = Visibility.Hidden;
            f.Visibility = Visibility.Hidden;
            labelInformation.Visibility = Visibility.Hidden;
            plateauJ1.Visibility = Visibility.Hidden;
            plateauJ2.Visibility = Visibility.Hidden;
            J1.Visibility = Visibility.Hidden;
            J2.Visibility = Visibility.Hidden;
            score1.Visibility = Visibility.Hidden;
            score2.Visibility = Visibility.Hidden;
        }
        
        public void rendreVisible()
        {
            plateauJ1.Visibility = Visibility.Visible;
            plateauJ2.Visibility = Visibility.Visible;
            A.Visibility = Visibility.Visible;
            B.Visibility = Visibility.Visible;
            C.Visibility = Visibility.Visible;
            D.Visibility = Visibility.Visible;
            E.Visibility = Visibility.Visible;
            F.Visibility = Visibility.Visible;
            a.Visibility = Visibility.Visible;
            b.Visibility = Visibility.Visible;
            c.Visibility = Visibility.Visible;
            d.Visibility = Visibility.Visible;
            e.Visibility = Visibility.Visible;
            f.Visibility = Visibility.Visible;
            labelInformation.Visibility = Visibility.Visible;
            plateauJ1.Visibility = Visibility.Visible;
            plateauJ2.Visibility = Visibility.Visible;
            J1.Visibility = Visibility.Visible;
            J2.Visibility = Visibility.Visible;
            score1.Visibility = Visibility.Visible;
            score2.Visibility = Visibility.Visible;
        }

        public void joueursLocal(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine("/**** Click sur 2 joueurs local ****/");
            rendreVisible();
            this.moteur.typeDepartie = "local";
            labelInformation.Text = this.moteur.currentJoueur.nom + " commence";
        }

        public void joueurIA(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine("/**** Click sur joueur IA ****/");
            rendreVisible();
            this.moteur.typeDepartie = "IA";
        }
    }
}
