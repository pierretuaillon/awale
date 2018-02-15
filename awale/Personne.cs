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
        public int score { get; set; }
        private string _nom;
        public List<Trou> listTrouPerso { get; set; }

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
        }



        public event PropertyChangedEventHandler PropertyChanged;
    }
}
