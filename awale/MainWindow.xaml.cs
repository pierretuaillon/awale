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
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace awale
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public Historique historique { get; set; }

        private ObservableCollection<Trou> _trous;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Trou> trous
        {
            get
            {
                return _trous;
            }
            set
            {
                _trous = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("trous"));
            }
        }
           
        public Personne joueur1 { get; set; }
        public Personne joueur2 { get; set; }
        private List<TextBox> myTrous;
        
        MoteurDeJeu moteur { get; set; }

        private Socket socket;
        private System.Net.Sockets.TcpClient clientSocket;
        private BinaryFormatter binFormatter = new BinaryFormatter();

        public MainWindow()
        {
            this.historique = new Historique("./res.csv");
            //initializeList();
            InitializeComponent();      
            this.DataContext = this;
            //rendreInvisible();
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

            //Definition du predecesseur manquant
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
            
            if (elementRecherche.nombreGraines > 0)
            {
                this.moteur.faireAction(elementRecherche);

                System.Console.WriteLine("/**** MISE A JOUR DE LA VUE ****/");
                this.trous = this.moteur.trous;

                //On modifie le joueur courant
                updateCurrentJoueur();

                // si online envoi du trou joué
                if (moteur.typeDepartie == "online")
                    envoyerMsg(elementRecherche.nom);
                
            }
            else
            {
                labelInformation.Text = "Vous ne pouvez pas jouer une case vide";
            }
                

            /*
            System.Console.WriteLine("*** Contenu des trous ***");
            foreach (var trou in this.trous)
            {
                System.Console.WriteLine(trou.nombreGraines);
            }*/

            if (finPartie())
            {
                labelInformation.Text = "Fin de la partie ";
                if (this.joueur1.score > this.joueur2.score)
                {
                    labelInformation.Text += "\n Le joueur : " + this.joueur1.nom + " gagne !";
                }
                else
                {
                    labelInformation.Text += "\n Le joueur : " + this.joueur2.nom + " gagne !";
                }
                this.historique.enregistrerInfos(this.joueur1, this.joueur2);
            }
        }

        public Boolean finPartie()
        {
            if (joueur1.score >= 25)
                return true;
            if (joueur2.score >= 25)
                return true;
            if (joueur1.totalGraines() == 0)
                return true;
            if (joueur2.totalGraines() == 0)
                return true;
            return false;
            /*int nbGrainesEnJeu = 0;

            foreach (var trou in this.trous)
            {
                nbGrainesEnJeu += trou.nombreGraines;
            }

            if (24 > nbGrainesEnJeu)
            {
                return true;
            }
            else
            {
                return false;
            }*/
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

        public void joueursLocal(object sender, RoutedEventArgs ev)
        {
            System.Console.WriteLine("/**** Click sur 2 joueurs local ****/");
            initializeList();
            rendreVisible();
            this.moteur.typeDepartie = "local";
            labelInformation.Text = this.moteur.currentJoueur.nom + " commence";
            a.IsEnabled = false;
            b.IsEnabled = false;
            c.IsEnabled = false;
            d.IsEnabled = false;
            e.IsEnabled = false;
            f.IsEnabled = false;
            A.IsEnabled = true;
            B.IsEnabled = true;
            C.IsEnabled = true;
            D.IsEnabled = true;
            E.IsEnabled = true;
            F.IsEnabled = true;
        }

        public void joueurIA(object sender, RoutedEventArgs eventarg)
        {
            System.Console.WriteLine("/**** Click sur joueur IA ****/");
            initializeList();
            rendreVisible();
            this.moteur.typeDepartie = "IA";
            labelInformation.Text = this.moteur.currentJoueur.nom + " commence";
            a.IsEnabled = false;
            b.IsEnabled = false;
            c.IsEnabled = false;
            d.IsEnabled = false;
            e.IsEnabled = false;
            f.IsEnabled = false;
        }

        private void hebergerPartie(object sender, RoutedEventArgs ev)
        {
            System.Console.WriteLine("/**** Click sur heberger ****/");
            initializeList();
            rendreVisible();
            this.moteur.typeDepartie = "online";
            moteur.currentJoueur = joueur2;
            a.IsEnabled = false;
            b.IsEnabled = false;
            c.IsEnabled = false;
            d.IsEnabled = false;
            e.IsEnabled = false;
            f.IsEnabled = false;
            myTrous = new List<TextBox>();
            myTrous.Add(A);
            myTrous.Add(B);
            myTrous.Add(C);
            myTrous.Add(D);
            myTrous.Add(E);
            myTrous.Add(F);

            foreach (TextBox trou in myTrous)
            {
                trou.IsEnabled = false;
            }

            //connexion
            TcpListener serverSocket = new TcpListener(2323);
            clientSocket = default(TcpClient);
            serverSocket.Start();
            Console.WriteLine(" >> Server Started");
            labelInformation.Text = "En attente de connexion ...";
            clientSocket = serverSocket.AcceptTcpClient();
            Console.WriteLine(" >> Accept connection from client");
            MessageBox.Show("Connecté");
            labelInformation.Text = this.moteur.currentJoueur.nom + " commence";

            //attente du coup du joueur distant
            lireMsgAsync();
        }

        private void rejoindrePartie(object sender, RoutedEventArgs ev)
        {
            System.Console.WriteLine("/**** Click sur rejoindre ****/");
            var dialog = new Recherche_Joueur();
            string adress = null;
            if (dialog.ShowDialog() == true)
            {
                adress = dialog.ResponseText;
                System.Console.WriteLine("* texte entré : " + adress + " *");
            }
            if (adress != null && Regex.IsMatch(adress, @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"))
            {
                System.Console.WriteLine("* IP entrée OK *");

                initializeList();
                rendreVisible();
                this.moteur.typeDepartie = "online";
                moteur.currentJoueur = joueur2;
                clientSocket = new TcpClient();
                clientSocket.Connect(adress, 2323);
                MessageBox.Show("Connecté");
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                labelInformation.Text = this.moteur.currentJoueur.nom + " commence";
                A.IsEnabled = false;
                B.IsEnabled = false;
                C.IsEnabled = false;
                D.IsEnabled = false;
                E.IsEnabled = false;
                F.IsEnabled = false;
                myTrous = new List<TextBox>();
                myTrous.Add(a);
                myTrous.Add(b);
                myTrous.Add(c);
                myTrous.Add(d);
                myTrous.Add(e);
                myTrous.Add(f);

                foreach (TextBox trou in myTrous)
                {
                    trou.IsEnabled = true;
                }
                moteur.currentJoueur = joueur2;
            }
        }

        private async void lireMsgAsync()
        {
            //récupération du message
            NetworkStream networkStream = clientSocket.GetStream();
            byte[] bytesFrom = new byte[10025];
            try
            {
                await networkStream.ReadAsync(bytesFrom, 0, 10025);
            }
            catch
            {
                MessageBox.Show("Déconnecté");
                return;
            }
            string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
            dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
            Console.WriteLine(" >> Message recu : " + dataFromClient);

            //action du joueur distant
            foreach (Trou trou in trous)
            {
                if (trou.nom == dataFromClient)
                    moteur.faireAction(trou);
            }

            System.Console.WriteLine("/**** MISE A JOUR DE LA VUE ****/");
            this.trous = this.moteur.trous;

            updateCurrentJoueur();
        }
        

        private void envoyerMsg(string trou)
        {
            System.Console.WriteLine(" >> Envoi du msg : "+trou);
            NetworkStream stream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(trou + "$");
            stream.Write(outStream, 0, outStream.Length);
            stream.Flush();
            lireMsgAsync();            
        }

        private void updateCurrentJoueur()
        {
            System.Console.WriteLine("/**** Mise à jour du joueur courant : "+moteur.currentJoueur.nom);
            //Si le joueur courrant est le joueur 1
            if (this.moteur.currentJoueur.nom == this.joueur1.nom)
            {
                //this.joueur1.score = this.moteur.currentJoueur.score;
                PropertyChanged(this, new PropertyChangedEventArgs("joueur1"));
                this.moteur.currentJoueur = this.joueur2;
            }
            //Sinon c'est forcement le joueur 2
            else
            {
                //this.joueur2.score = this.moteur.currentJoueur.score;
                PropertyChanged(this, new PropertyChangedEventArgs("joueur2"));
                this.moteur.currentJoueur = this.joueur1;
            }

            if (this.moteur.typeDepartie == "local")
            {
                if(this.joueur2.nom == this.moteur.currentJoueur.nom)
                {
                    a.IsEnabled = true;
                    b.IsEnabled = true;
                    c.IsEnabled = true;
                    d.IsEnabled = true;
                    e.IsEnabled = true;
                    f.IsEnabled = true;
                    A.IsEnabled = false;
                    B.IsEnabled = false;
                    C.IsEnabled = false;
                    D.IsEnabled = false;
                    E.IsEnabled = false;
                    F.IsEnabled = false;
                }
                else
                {
                    a.IsEnabled = false;
                    b.IsEnabled = false;
                    c.IsEnabled = false;
                    d.IsEnabled = false;
                    e.IsEnabled = false;
                    f.IsEnabled = false;
                    A.IsEnabled = true;
                    B.IsEnabled = true;
                    C.IsEnabled = true;
                    D.IsEnabled = true;
                    E.IsEnabled = true;
                    F.IsEnabled = true;
                }

            }
            //Si c'est ensuite au tour du joueur 2 en vs ia
            if (this.moteur.typeDepartie == "IA" && this.joueur2.nom == this.moteur.currentJoueur.nom)
            {
                Trou bestChoix = this.moteur.meilleureActionIA();
                this.moteur.faireAction(bestChoix);
                PropertyChanged(this, new PropertyChangedEventArgs("joueur2"));
                this.moteur.currentJoueur = this.joueur1;
            }
            if (this.moteur.typeDepartie == "online")
            {
                foreach(TextBox trou in myTrous)
                {
                    trou.IsEnabled = !trou.IsEnabled;
                }
            }
            System.Console.WriteLine("/**** Mise à jour du joueur - maj : " + moteur.currentJoueur.nom);
            labelInformation.Text = "Au tour du joueur : " + this.moteur.currentJoueur.nom;
        }

        private void AllPartie(object sender, RoutedEventArgs e)
        {
            new HistoriqueWindow().Show();
        }
    }
}
