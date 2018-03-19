using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awale
{
    public class Personne : INotifyPropertyChanged
    {
        private int _score;
        private string _nom;
        public List<Trou> listTrouPerso { get;  }

        public int score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("score"));
            }
        }

        public string nom
        {
            get
            {
                return _nom;
            }
            set
            {
                _nom = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("nom"));
            }
        }

        public int totalGraines()
        {
            int res = 0;
            foreach(var trou in listTrouPerso)
            {
                res += trou.nombreGraines;
            }
            return res;
        }

        public Personne (string nom)
        {
            this._nom = nom;
            this.listTrouPerso = new List<Trou>();
            this._score = 0;
        }

        public Personne(string nom, int score)
        {
            this._nom = nom;
            this.listTrouPerso = new List<Trou>();
            this._score = score;
        }

        public Personne(Personne p)
        {
            this._nom = p.nom;
            this.listTrouPerso = new List<Trou>(p.listTrouPerso);
            this._score = p.score;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
