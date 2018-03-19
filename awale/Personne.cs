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
        private int _score { get; set; }
        private string _nom;
        public List<Trou> listTrouPerso { get; set; }

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



        public Personne (string nom)
        {
            this._nom = nom;
            this.listTrouPerso = new List<Trou>();
            this.score = 0;
        }

        public Personne(string nom, int score)
        {
            this._nom = nom;
            this.listTrouPerso = new List<Trou>();
            this.score = score;
        }



        public event PropertyChangedEventHandler PropertyChanged;
    }
}
