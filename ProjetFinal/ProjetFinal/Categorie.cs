using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFinal
{
    internal class Categorie
    {
        int categorie_id;
        string categorie_nom;


        public Categorie() 
        {
           
        }
        public Categorie(int categorie_id, string categorie_nom)
        {
            this.categorie_id = categorie_id;
            this.categorie_nom = categorie_nom;
        }

        public int Categorie_id { get => categorie_id; set => categorie_id = value; }

        public string Categorie_nom { get => categorie_nom; set => categorie_nom = value; }
    }
}
