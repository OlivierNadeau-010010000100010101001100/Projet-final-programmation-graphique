using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFinal
{
    internal class ActivitePersonne
    {
        string nom_activite;
        int nbr_personne;
        public ActivitePersonne() { }

        public ActivitePersonne(string nom_activite, int nbr_personne)
        { 
            this.nom_activite = nom_activite;
            this.nbr_personne = nbr_personne;
        }

        public string Nom_Activite { get => nom_activite; set => nom_activite = value; }

        public int Nbr_personne {  get => nbr_personne; set => nbr_personne = value;}
    }
}
