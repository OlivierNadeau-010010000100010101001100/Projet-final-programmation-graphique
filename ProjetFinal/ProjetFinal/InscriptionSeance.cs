using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFinal
{
    internal class InscriptionSeance
    {
        string adherent_id_FK;
        int seance_id_FK, note_appreciation;
        InscriptionSeance() { }

        InscriptionSeance(string adherent_id_FK, int seance_id_FK, int note_appreciation)
        {
            this.adherent_id_FK = adherent_id_FK;
            this.seance_id_FK = seance_id_FK;
            this.note_appreciation = note_appreciation;
        }

        public string Adherent_id_FK { get => adherent_id_FK; set => adherent_id_FK = value;}
        
        public int Seance_id_FK { get => seance_id_FK; set => seance_id_FK = value;}

        public int Note_appreciation { get => note_appreciation; set => note_appreciation = value;}


    }
}
