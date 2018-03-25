using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;

namespace awale
{

    class MoteurDeJeu
    {
        private int nombreGrainesTotalBase;
        public string typeDepartie { get; set; }
        public Personne currentJoueur { get; set; }
        public ObservableCollection<Trou> trous { get; set; }
        public MoteurDeJeu(ObservableCollection<Trou> trous)
        {
            this.trous = trous;

            int nbGrainesEnJeu = 0;

            foreach (var trou in this.trous)
            {
                nbGrainesEnJeu += trou.nombreGraines;
            }
            nombreGrainesTotalBase = nbGrainesEnJeu;

            System.Console.WriteLine(nombreGrainesTotalBase);
        }

        public void faireAction(Trou trouDepart)
        {
            System.Console.WriteLine("/**** On effectue l'action ****/");
            //On récupére toutes les graines de la case de départ
            int graineEnMain = trouDepart.nombreGraines;
            trouDepart.nombreGraines = 0;
            //On met a jour le trou correspondant dans la liste
            updateElementList(trouDepart);

            Trou currentTrou = trouDepart;
            //Element pour tester si on met a jour en fonction 
            Boolean firstTour = true;

            //Tant qu'on a des graines en jeu
            while (graineEnMain != 0)
            {
                //Si c'est le premier tour
                if (firstTour)
                {
                    firstTour = false;
                    //On met pour le prochain tour le successeur de l'element de départ
                    currentTrou = trouDepart.successeur;
                }
                else
                {
                    //On ajoute une graine au trou
                    currentTrou.nombreGraines++;
                    //On met a jour le trou correspondant dans la liste
                    updateElementList(currentTrou);
                    //On met a jour le trou pour le tour suivant
                    currentTrou = currentTrou.successeur;
                    //On retire une graine de la main du joueur
                    graineEnMain--;
                }
            }

            getGraines(currentTrou.predecesseur);
        }

        //Mis a jour du nombre de graines dans un trou
        public void updateElementList(Trou trouAUpdate)
        {
            for (int i = 0; i < this.trous.Count; i++)
            {
                //Si c'est l'element qu'on cherche
                if (this.trous.ElementAt(i).nom == trouAUpdate.nom)
                {
                    //On modifie son nombre de graines
                    this.trous.ElementAt(i).nombreGraines = trouAUpdate.nombreGraines;
                    return;
                }
            }
        }

        public void getGraines(Trou trouATest)
        {
            while (trouATest.nombreGraines == 2 || trouATest.nombreGraines == 3)
            {
                this.currentJoueur.score += trouATest.nombreGraines;
                trouATest.nombreGraines = 0;
                updateElementList(trouATest);
                //On test le trou d'avant
                trouATest = trouATest.predecesseur;
            }
        }


        /*public Boolean actionPossible(Trou trouAtester)
        {
            for (int i = 0; i < this.currentJoueur.listTrouPerso.Count; i++)
            {
                //Si c'est l'element qu'on cherche
                if (this.currentJoueur.listTrouPerso.ElementAt(i).nom == trouAtester.nom)
                {
                    //Il est proprio du trou il peut donc jouer
                    return true;
                }
            }
            //Si on arrive ici c'est que l'utilisateur ne possede pas le trou
            //il ne peut donc pas jouer
            return false;
        }*/

        public Trou meilleureActionIA()
        {
            System.Console.WriteLine("/**** Choix de la meilleure action pour l'IA ****/");
            
            Trou bestTrou = null;
            Trou lePlusRempli = null;
            //pour chacun de ses trous
            foreach (var trou in this.currentJoueur.listTrouPerso)
            {
                if(trou.nombreGraines > 0)
                {
                    //on regarde le trou dans lequel la derniere graine tombe
                    Trou troufinal = trou;
                    int nb = trou.nombreGraines;
                    while (nb > 0) {
                        troufinal = troufinal.successeur;
                        nb--;
                    }
                    //si il contient déjà 1 ou 2 graine
                    if(troufinal.nombreGraines==1 || troufinal.nombreGraines==2)
                    {
                        //on garde ce coup
                        bestTrou = trou;
                    }

                    if (lePlusRempli == null)
                        lePlusRempli = trou;
                    else
                    //si il n'y a pas de bon coup on joue le dernier trou le plus rempli
                    if (trou.nombreGraines > lePlusRempli.nombreGraines)
                        lePlusRempli = trou;
                }
            }
            if(bestTrou != null)
                return bestTrou;
            return lePlusRempli;
        }

        internal void faireActionRecue(object trouRecu)
        {
            MessageBox.Show("TROU RECU : "+trouRecu);
        }
    }
}