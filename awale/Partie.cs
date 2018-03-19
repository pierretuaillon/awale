using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awale
{
    class Partie
    {

        public Personne joueur1 { get; }
        public Personne joueur2 { get; }

        public Partie(Personne joueur1, Personne joueur2)
        {
            this.joueur1 = joueur1;
            this.joueur2 = joueur2;
        }
    }
}
