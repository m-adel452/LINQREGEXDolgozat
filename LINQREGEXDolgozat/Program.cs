using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LINQREGEXDolgozat
{
    internal class Program
    {
        static List<Felhasznalo> Felhasznaloklista = new List<Felhasznalo>();
        static List<string> HelyesSorok = new List<string>();
        static void Main(string[] args)
        {
            #region 1.BEOLVASÁS ÉS SZŰRÉS: Olvasd be a "feladat.txt" fájl sorait!Hozz létre egy listát, amely csak a helyes, pontosvesszővel tagolt(legalább 5 adategységet tartalmazó) sorokat tartja meg.
            StreamReader sr = new StreamReader("feladat.txt");

            while (!sr.EndOfStream)
            {
                string sor = sr.ReadLine();
                if (sor.Split(';').Length >= 5)
                {
                    HelyesSorok.Add(sor);
                    Felhasznaloklista.Add(new Felhasznalo(sor));
                }
            }
            sr.Close();
            Console.WriteLine("1.feladat: Fájl beolvasás sikeres volt");
            #endregion

            #region 2. MEGJELENÍTÉS:Írd ki a konzolra a feldolgozott felhasználók adatait áttekinthető formában!
            Console.WriteLine("\n2.feladat: Megjelenités");
            foreach (var felhasznalo in Felhasznaloklista)
            {
                Console.WriteLine(felhasznalo.ToString());
            }
            #endregion

            #region 3.EMAIL CÍMEK KINYERÉSE(REGEX): Elleneőrizd, hogy érvényesek-e az email címek!
            Console.WriteLine("\n3. feladat: email címek kinyerése");
            Regex emailRegex = new Regex(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");
            List<string> emailek = new List<string>();
            foreach (var item in Felhasznaloklista)
            {
                if (emailRegex.IsMatch(item.Email))
                {
                    Console.WriteLine($"{item.Email} - Érvényes");
                    emailek.Add(item.Email);
                }
                else
                {
                    Console.WriteLine($"{item.Email} - Hibás");
                }
            }
            #endregion

            #region 4.MAGYAR TELEFONSZÁMOK ÉRVÉNYESÍTése(REGEX): Készíts egy Regex kifejezést, amely felismeri a különböző formátumú magyar mobiltelefonszámokat(pl. `+36 - 30 - ...`, `0620...`, `+36 70...`), és listázd ki azokat a sorokat, amelyekben érvényes mobilszám van!
            Console.WriteLine("\n4.feladat magyar telefonszámok érvényesitése");
            Regex telRegex = new Regex(@"(?:\+36|06)[\s-]?\d{2}[\s-]?\d{3}[\s-]?\d{4}");
            foreach (var item in Felhasznaloklista)
            {
                if (telRegex.IsMatch(item.Telefonszam))
                {
                    Console.WriteLine(item);
                }
            }
            #endregion

            #region 5.IRÁNYÍTÓSZÁMOK GYŰJTÉSE(REGEX / LINQ): Gyűjtsd ki az összes irányítószámot(a 4 jegyű számokat) egy listába, és írd ki a duplikációkat kiszűrve, növekvő sorrendbe rendezve!
            Console.WriteLine("\n5.feladat: irányitószámok gyüjtése");
            Regex irszRegex = new Regex(@"\b\d{4}\b");
            List<int> iranyitoszamok = new List<int>();
            foreach (var item in Felhasznaloklista)
            {
                if (irszRegex.IsMatch(item.Iranyitoszam.ToString()))
                {
                    iranyitoszamok.Add(item.Iranyitoszam);
                }
            }
            foreach (var irsz in iranyitoszamok.Distinct().OrderBy(x => x))
            {
                Console.WriteLine(irsz);
            }
            #endregion

            #region 6. DOMAIN SZERINTI CSOPORTOSÍTÁS (LINQ): Csoportosítsd a felhasználókat az email címük domainje(pl. `gmail.com`, `freemail.hu`) szerint, és írd ki, hogy melyik domainhez hány felhasználó tartozik!
            Console.WriteLine("\n6.feladat: domainév szerinti csoportositás");
            foreach (var item in emailek.GroupBy(e => e.Split('@')))
            {
                Console.WriteLine($"{item.Key}: {item.Count()} db");
            }
            #endregion

            #region 7. SZOLGÁLTATÓ AZONOSÍTÁSA (REGEX CSOPORTOKKal): A telefonszámokból a Regex csoportok(`match.Groups`) segítségével olvasd ki a mobilszolgáltató előtagját(pl. 30, 20, 70), és számold meg, hány 30 -as, 20 -as és 70 - es szám van!
            Console.WriteLine("\n7.feladat: szolgáltató azonosítása");
            Regex providerRegex = new Regex(@"(?:\+36|06)[\s-]?(?<szolgaltato>20|30|70)[\s-]?\d{3}[\s-]?\d{4}");
            var szolgaltatok = Felhasznaloklista.Where(x => providerRegex.IsMatch(x.Telefonszam)).GroupBy(x => providerRegex.Match(x.Telefonszam).Groups["szolgaltato"].Value);
            foreach (var item in szolgaltatok)
            {
                Console.WriteLine($"{item.Key}-as: {item.Count()} db");
            }
            #endregion

            #region 8. FELHASZNÁLÓNÉV HOSSZA (LINQ):Keresd meg a leghosszabb felhasználónevet(`User123`, `Admin99`, stb.) a adatokból, és írd ki a nevét és a hosszát is !
            Console.WriteLine("\n8.feladat:felhasználó hossza:");
            var leghosszabbFelhasznalo = Felhasznaloklista.OrderByDescending(x => x.Felhasznalonev.Length).First();
            Console.WriteLine($"Leghosszabb felhasználónév: {leghosszabbFelhasznalo.Felhasznalonev}, Hossza: {leghosszabbFelhasznalo.Felhasznalonev.Length}");
            #endregion

            #region 9. CÉGES VAGY INTÉZMÉNYI EMAILEK KISZŰRÉSE (REGEX): Listázd ki azokat a felhasználókat, akiknek az email címe *nem * a megszokott lakossági(`gmail`, `freemail`, `citromail`, `index`) szolgáltatókhoz tartozik(azaz céges vagy egyetemi domain, pl. `uni-miskolc.hu` vagy `co.uk`).
            Console.WriteLine("\n9.feladat: céges vagy intézményi emailek kiszűrése");
            Regex cegesEmailRegex = new Regex(@"@[a-zA-Z0-9.-]+\.(?!gmail|freemail|citromail|index)[a-zA-Z]{2,}");
            foreach (var item in Felhasznaloklista)
            {
                if (emailRegex.IsMatch(item.Email) && cegesEmailRegex.IsMatch(item.Email) && !item.Email.Contains("@gmail.") && !item.Email.Contains("@freemail.") && !item.Email.Contains("@citromail.") && !item.Email.Contains("@index."))
                {
                    Console.WriteLine(item);
                }
            }
            #endregion
            Console.ReadKey();
        }
    }
}