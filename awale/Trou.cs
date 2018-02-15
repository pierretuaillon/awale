using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awale
{
    public class Trou : INotifyPropertyChanged
    {
        public string nom { get; set; }


        private int _nbGraines;

        public int nombreGraines
        {
            get
            {
                return _nbGraines;
            }
            set
            {
                _nbGraines = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("nombreGraines"));
            }
        }


        public Trou predecesseur { get; set; }
        public Trou successeur { get; set; }

        public Trou(string nom, int nombreGraines)
        {
            this.nom = nom;
            this.nombreGraines = nombreGraines;
        }

        public Trou(string nom, int nombreGraines, Trou predecesseur) : this(nom, nombreGraines)
        {
            this.predecesseur = predecesseur;
        }

        public Trou(string nom, int nombreGraines, Trou predecesseur, Trou successeur) : this(nom, nombreGraines)
        {
            this.predecesseur = predecesseur;
            this.successeur = successeur;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
