using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awale
{
    public class Historique
    {
        string filePath { get; set; }
        
        public Historique(string filePath)
        {
            this.filePath = filePath;
        }

        public void enregistrerInfos(Personne j1, Personne j2)
        {
            var csv = new StringBuilder();

            var nom_j1 = j1.nom;
            var nom_j2 = j2.nom;

            var score_j1 = j1.score;
            var score_j2 = j2.score;

            var newLineJ1 = string.Format("{0};{1}", nom_j1, score_j1);
            var newLineJ2 = string.Format("{0};{1}", nom_j2, score_j2);
            csv.AppendLine(newLineJ1);
            csv.AppendLine(newLineJ2);

            File.AppendAllText(this.filePath, csv.ToString());
        }

        public IEnumerable<Personne> ReadCSV()
        {

            string[] lines = File.ReadAllLines(this.filePath);

            // lines.Select allows me to project each line as a Person. 
            // This will give me an IEnumerable<Person> back.
            return lines.Select(line =>
            {
                string[] data = line.Split(';');
                //On reconstruit un objet personne
                return new Personne(data[0], Convert.ToInt32(data[1]));
            });
        }
    }
}
