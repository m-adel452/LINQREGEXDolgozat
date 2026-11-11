using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LINQREGEXDolgozat
{
    #region Felhasznaló osztály
    internal class Felhasznalo
    {
        public string Nev { get; set; }
        public string Email { get; set; }
        public string Telefonszam { get; set; }
        public int Iranyitoszam { get; set; }
        public string Felhasznalonev { get; set; }

        public Felhasznalo(string sor)
        {
            string[] darabok = sor.Split(';');
            Nev = darabok[0].Trim();
            Email = darabok[1].Trim();
            Telefonszam = darabok[2].Trim();
            Iranyitoszam = Convert.ToInt32(darabok[3].Trim());
            Felhasznalonev = darabok[4].Trim();
        }

        public override string ToString()
        {
            return Nev + "; " + Email + "; " + Telefonszam + "; " + Iranyitoszam + "; " + Felhasznalonev;
        }
    }
}
#endregion
