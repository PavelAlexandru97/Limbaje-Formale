using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace Formal_Language.UserControls
{
    public partial class TableGeneration : UserControl
    {
        public TableGeneration()
        {
            InitializeComponent();
        }

        List<string> contents;
        string[,] matriceAfisareTerminali;
        string[,] matriceAfisareNeterminali;
        List<string> terminali;
        List<string> neterminali;
        List<string> productii;
        List<string> elemente;
        List<Functie> functii = new List<Functie>();
        List<string> urmatorii = new List<string>();
        int lini;
        int coloaneTerminali;
        int coloaneNeterminali;
        List<List<string>> listeGenerate;

        #region Citire din fisier

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                contents = new List<string>();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(openFileDialog.FileName);
                string line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    contents.Add(line);
                }

                int ceva = 1;

                terminali = ObtineTerminai(contents);
                neterminali = ObtineNeterminali(contents);
                productii = ObtineProductii(contents);
                elemente = ObtineElemente();

                buttonGenerate.Enabled = true;
              
            }
        }

        List<string> ObtineTerminai(List<string> contents)
        {
            string linieTerminali = contents[0];
            string[] vectorTerminali = linieTerminali.Split();

            List<string> terminali = new List<string>();
            foreach (string terminal in vectorTerminali)
                terminali.Add(terminal);

            return terminali;
        }

        List<string> ObtineNeterminali(List<string> contents)
        {
            string linieNeterminali = contents[1];
            string[] vectorNeterminali = linieNeterminali.Split();

            List<string> neterminali = new List<string>();
            foreach (string neterminal in vectorNeterminali)
                neterminali.Add(neterminal);

            return neterminali;
        }

        List<string> ObtineProductii(List<string> contents)
        {
            List<string> productii = new List<string>();
            for (int index = 2; index < contents.Count; index++)
            {
                productii.Add(contents[index]);
            }
            return productii;
        }

        List<string> ObtineElemente()
        {
            List<string> elemente = new List<string>();

            foreach (string terminal in terminali)
                elemente.Add(terminal);

            foreach (string neterminal in neterminali)
                elemente.Add(neterminal);

            return elemente;
        }

        #endregion

        #region Generare

        private void buttonGenerate_Click(object sender, EventArgs e)
        {


            string primaProductie = productii[0];

            string[] elementeProductie = primaProductie.Split();
            string neterminal = elementeProductie[0];

            string rezultat = neterminal + " ." + neterminal;

            List<string> listaStart = new List<string>();
            listaStart.Add(rezultat);

            listeGenerate = new List<List<string>>();
            List<List<string>> listeMarcate = new List<List<string>>();

            List<string> I0 = new List<string>();
            foreach (string start in listaStart)
            {
                I0.Add(start);
            }

            List<string> chestii = GenereazaIn(listaStart, productii);
            foreach (string chestie in chestii)
            {
                I0.Add(chestie);
            }

            listeGenerate.Add(I0);


            List<string> elemente = ElementeDupaPunct(I0);
            foreach (string element in elemente)
            {
                List<string> salt = Salt(I0, element);
                listeGenerate.Add(salt);
                Functie functie = new Functie(0, element, listeGenerate.Count() - 1);
                functii.Add(functie);
            }
            int buff;
            int count = 0;
            List<Functie> theFunctii = new List<Functie>();
            while (listeGenerate.Count - 1 != listeMarcate.Count)
            {
                buff = count;


                theFunctii = FaCeva(listeGenerate, listeMarcate);
                count = theFunctii.Count();
                for (int i = 0; i < theFunctii.Count() - buff; i++)
                {
                    functii.Add(theFunctii[i + (buff)]);
                }
            }
            lini = listeGenerate.Count();
            coloaneTerminali = terminali.Count();
            coloaneNeterminali = neterminali.Count();
             matriceAfisareTerminali = new string[lini, coloaneTerminali];
             matriceAfisareNeterminali = new string[lini, coloaneNeterminali];
            ObtineUrmatorii();
            for (int i = 0; i < lini; i++)
            {
                for (int j = 0; j < coloaneTerminali; j++)
                {
                    matriceAfisareTerminali[i, j] = "NULL ";
                }
            }
            for (int i = 0; i < lini; i++)
            {
                for (int j = 0; j < coloaneNeterminali; j++)
                {
                    matriceAfisareNeterminali[i, j] = "NULL ";
                }
            }
            PopuleazaMatriceTerminali(matriceAfisareTerminali);
            GeneraTabelaNetermiali(matriceAfisareNeterminali);
            buttonExport.Enabled = true;
        }

        void GeneraTabelaNetermiali(string[,] matriceAfisareNeterminali)
        {
            foreach (var functie in functii)
            {
                if (neterminali.Contains(functie.modificator))
                {
                    matriceAfisareNeterminali[functie.IndexIInitil, neterminali.IndexOf(functie.modificator)] = functie.Ifinal.ToString() + " ";
                }
            }
        }

        #region GenerareIuri
        List<string> GenereazaProductii(List<string> lista)
        {
            List<string> rezultat = new List<string>();

            List<string> neterminaliGasiti = new List<string>();


            return rezultat;
        }

        List<string> GenereazaIn(List<string> start, List<string> productii)
        {
            List<string> rezultat = new List<string>();

            List<string> neterminaliGasiti = new List<string>();
            string neterminal = NeterminalDupaPunct(start, neterminali, neterminaliGasiti);
            while (neterminal != null)
            {
                foreach (string productie in productii)
                {
                    string[] elementeProductie = productie.Split();
                    string neterminalProductie = elementeProductie[0];
                    string valoareProductie = elementeProductie[1];

                    if (neterminalProductie == neterminal)
                    {
                        string productieGenerata = neterminalProductie + " ." + valoareProductie;
                        rezultat.Add(productieGenerata);
                    }
                }

                neterminal = NeterminalDupaPunct(rezultat, neterminali, neterminaliGasiti);
            }

            return rezultat;
        }

        string NeterminalDupaPunct(List<string> productii, List<string> neterminali, List<string> neterminaliGasiti)
        {
            foreach (string productie in productii)
            {
                string[] elementeProductie = productie.Split();
                string valoareProductie = elementeProductie[1];
                int indexPunct = valoareProductie.IndexOf('.');

                // N-a fost gasit punctul
                if (indexPunct == -1)
                {
                    continue;
                }

                string dupaPunct = valoareProductie.Substring(indexPunct + 1);
                foreach (string neterminal in neterminali)
                {

                    if (dupaPunct.StartsWith(neterminal))
                    {
                        if (!neterminaliGasiti.Contains(neterminal))
                        {
                            neterminaliGasiti.Add(neterminal);
                            return neterminal;
                        }

                    }
                }
            }

            return null;
        }

        List<string> ElementeDupaPunct(List<string> In)
        {
            List<string> elementeDupaPunct = new List<string>();

            foreach (string productie in In)
            {
                string[] elementeProductie = productie.Split();
                string valoareProductie = elementeProductie[1];
                int indexPunct = valoareProductie.IndexOf('.');

                // N-a fost gasit punctul
                if (indexPunct == -1)
                {
                    continue;
                }

                string dupaPunct = valoareProductie.Substring(indexPunct + 1);
                foreach (string element in elemente)
                {
                    if (dupaPunct.StartsWith(element))
                    {
                        if (!elementeDupaPunct.Contains(element))
                        {
                            elementeDupaPunct.Add(element);
                        }
                    }
                }
            }

            return elementeDupaPunct;
        }

        List<string> ProductiiCuElementDupaPunctCuTerminal(List<string> In, string element)
        {
            List<string> productii = new List<string>();

            foreach (string productie in In)
            {
                string[] elementeProductie = productie.Split();
                string valoareProductie = elementeProductie[1];
                int indexPunct = valoareProductie.IndexOf('.');

                // N-a fost gasit punctul
                if (indexPunct == -1)
                {
                    continue;
                }

                string dupaPunct = valoareProductie.Substring(indexPunct + 1);

                if (dupaPunct.StartsWith(element))
                {
                    if (!productii.Contains(productie))
                    {
                        productii.Add(productie);
                    }
                }

            }

            return productii;
        }

        List<string> Salt(List<string> In, string elementSalt)
        {
            List<string> rezultat = new List<string>();

            if (EsteNeterminal(elementSalt))
            {
                foreach (string productie in In)
                {
                    string[] elementeProductie = productie.Split();
                    string neterminalProductie = elementeProductie[0];
                    string valoareProductie = elementeProductie[1];


                    int indexPunct = valoareProductie.IndexOf('.');

                    // N-a fost gasit punctul
                    if (indexPunct == -1)
                    {
                        return null;
                    }

                    // Punctul e la capatul sirului
                    if (indexPunct == valoareProductie.Length - 1)
                    {
                        return null;
                    }

                    string dupaPunct = valoareProductie.Substring(indexPunct + 1);
                    foreach (string element in elemente)
                    {
                        if (dupaPunct.StartsWith(element) && element == elementSalt)
                        {
                            string inchidere = InchidereMultime(productie);
                            rezultat.Add(inchidere);
                        }
                    }


                }
            }

            if (EsteTerminal(elementSalt))
            {
                List<string> productiiCuElementDupaPunct = ProductiiCuElementDupaPunctCuTerminal(In, elementSalt);
                List<string> toateProductiile = new List<string>();

                for (int index = 0; index < productiiCuElementDupaPunct.Count; index++)
                {
                    productiiCuElementDupaPunct[index] = InchidereMultime(productiiCuElementDupaPunct[index]);
                    rezultat.Add(productiiCuElementDupaPunct[index]);
                }


                List<string> generare = GenereazaIn(productiiCuElementDupaPunct, productii);
                foreach (string productie in generare)
                {
                    rezultat.Add(productie);
                }
            }

            return rezultat;
        }

        string InchidereMultime(string productie)
        {
            string rezultat = null;

            string[] elementeProductie = productie.Split();

            string neterminalProductie = elementeProductie[0];
            string valoareProductie = elementeProductie[1];
            int indexPunct = valoareProductie.IndexOf('.');

            // N-a fost gasit punctul
            if (indexPunct == -1)
            {
                return null;
            }

            // Punctul e la capatul sirului
            if (indexPunct == valoareProductie.Length - 1)
            {
                return null;
            }

            string dupaPunct = valoareProductie.Substring(indexPunct + 1);
            foreach (string element in elemente)
            {
                if (dupaPunct.StartsWith(element))
                {
                    int numarCaractereElement = element.Length;

                    // Inserare punct dupa element
                    rezultat = valoareProductie.Insert(indexPunct + numarCaractereElement + 1, ".");

                    // Stergere punct din fara elementului
                    rezultat = rezultat.Remove(indexPunct, 1);
                }
            }

            rezultat = neterminalProductie + " " + rezultat;
            return rezultat;
        }

        bool EsteTerminal(string element)
        {
            foreach (string terminal in terminali)
            {
                if (element == terminal)
                {
                    return true;
                }
            }

            return false;
        }

        bool EsteNeterminal(string element)
        {
            foreach (string neterminal in neterminali)
            {
                if (element == neterminal)
                {
                    return true;
                }
            }

            return false;
        }

        List<Functie> FaCeva(List<List<string>> listeGenerate, List<List<string>> listeMarcate)
        {
            List<Functie> functii = new List<Functie>();
            int ceva = 0;
            List<List<string>> listeDeAdaugat = new List<List<string>>();
            foreach (List<string> In in listeGenerate)
            {
                List<string> elemente = ElementeDupaPunct(In);
                foreach (string element in elemente)
                {
                    List<string> salt = Salt(In, element);

                    if (!ContineLista(listeGenerate, salt))
                    {
                        listeDeAdaugat.Add(salt);

                        if (listeGenerate.IndexOf(In) != 0)
                        {
                            int index = 0;
                            index = CautaIndex(listeGenerate, listeDeAdaugat, index, salt);
                            int nr = listeGenerate.IndexOf(In);
                            Functie func = new Functie(nr++, element, index);
                            functii.Add(func);
                        }
                    }
                    else
                    {
                        if (listeGenerate.IndexOf(In) != 0)
                        {
                            int index = 0;
                            index = CautaIndex(listeGenerate, listeDeAdaugat, index, salt);
                            int nr = listeGenerate.IndexOf(In);
                            Functie func = new Functie(nr++, element, index);
                            functii.Add(func);
                        }
                        if (!ContineLista(listeMarcate, salt))
                        {
                            listeMarcate.Add(salt);

                        }

                    }
                }
            }

            foreach (List<string> lista in listeDeAdaugat)
            {
                listeGenerate.Add(lista);
            }
            return functii;
        }

        public bool ContineLista(List<List<string>> liste, List<string> listaCautata)
        {
            bool rezultat = false;

            foreach (List<string> lista in liste)
            {
                int counterElementeGasite = 0;
                foreach (string elementLista in lista)
                {
                    if (listaCautata.Contains(elementLista))
                        counterElementeGasite++;
                }

                if (counterElementeGasite == listaCautata.Count)
                {
                    rezultat = true;
                    break;
                }
            }

            return rezultat;
        }

        public int DaIndexList(List<List<string>> mareList, List<string> list)
        {
            int index = -1;
            for (int i = 0; i < mareList.Count(); i++)
            {
                if (mareList[i].SequenceEqual(list))
                    return i;
            }
            return index;

        }

        public int CautaIndex(List<List<string>> listeGenerate, List<List<string>> listeDeAdaugat, int index, List<string> list)
        {
            if (ContineLista(listeGenerate, list))
            {
                index = DaIndexList(listeGenerate, list);
            }
            else
                index = listeGenerate.Count() + listeDeAdaugat.IndexOf(list);

            return index;
        }
        #endregion

        #region GenerareTabelaTerminali
        public void PopuleazaMatriceTerminali(string[,] matriceTerm)
        {
            Deplasarile(matriceTerm);
            Reducerile(matriceTerm);

        }

        public void Deplasarile(string[,] matriceTerm)
        {
            foreach (var functie in functii)
            {
                if (terminali.Contains(functie.modificator))
                {
                    matriceTerm[functie.IndexIInitil, terminali.IndexOf(functie.modificator)] = ("d" + functie.Ifinal + " ");
                }
            }
        }
        public void Reducerile(string[,] matriceTerm)
        {
            productii.Add("E E");
            foreach (var In in listeGenerate)
            {

                foreach (string Ins in In)
                {
                    //E E.   
                    if (Ins.IndexOf(".") == (Ins.Count() - 1))
                    {
                        // E E.
                        int indexIn = DaIndexList(listeGenerate, In);
                        string ceva = EliminaPuncat(Ins);
                        // E E
                        char urmator = Ins[0];

                        string prod = eliminaPrmElementProductie(ceva);
                        // E
                       
                        for (int i = 0; i < productii.Count(); i++)
                        {
                            string fff = productii[i];
                            string pro = eliminaPrmElementProductie(productii[i]);
                            int indexProductie = i + 1;

                            if (pro == " E" && ceva == "E E" && urmator == 'E')
                            {

                                int coloana = terminali.IndexOf("$");
                                matriceTerm[indexIn, coloana] = "acc" + " ";
                                
                            }


                            else if (prod == pro)
                            {
                                for (int j = 0; j < urmatorii.Count; j++)
                                {
                                    if (urmatorii[j].StartsWith(urmator.ToString()))
                                    {
                                        string[] urmatorul = urmatorii[j].Split(' ');
                                        foreach (var item in urmatorul[1])
                                        {
                                            int coloana = terminali.IndexOf(item.ToString());
                                            matriceTerm[indexIn, coloana] = "r" + indexProductie + " ";

                                        }
                                    }
                                }
                            }
                        }

                    }

                }

            }
        }
        string EliminaPuncat(string stringul)
        {
            string theString = "";
            foreach (var subS in stringul)
            {
                if (subS != '.')
                {
                    theString += subS;
                }
            }
            return theString;
        }

        string eliminaPrmElementProductie(string prod)
        {
            string productie = "";
            for (int i = 1; i < prod.Length; i++)
            {
                productie += prod[i];
            }
            return productie;
        }
        #endregion

        #region URMATORI
        public void ObtineUrmatorii()
        {
            List<string> terminaliGasiti = new List<string>();
            urmatorii.Add("E $");

            foreach (var item in neterminali)
            {
                string urmat = "";
                urmat += item + " " + "$";
                TerminaliDupaNeterminali(productii, terminali, terminaliGasiti, item);
                foreach (var item1 in terminaliGasiti)
                {
                    urmat += item1;
                }
                urmatorii.Add(urmat);
            }
        }


        void TerminaliDupaNeterminali(List<string> productii, List<string> terminali, List<string> terminaliGasiti, string neterminal)
        {
            foreach (string productie in productii)
            {
                string[] elementeProductie = productie.Split();
                string valoareProductie = elementeProductie[1];
                char neterm = Convert.ToChar(neterminal);
                int indexNeterm = valoareProductie.IndexOf(neterm);

                // N-a fost gasit punctul
                if (indexNeterm == -1)
                {
                    continue;
                }

                string dupaNeterm = valoareProductie.Substring(indexNeterm + 1);
                foreach (string terminal in terminali)
                {

                    if (dupaNeterm.StartsWith(terminal))
                    {
                        if (!terminaliGasiti.Contains(terminal))
                        {
                            terminaliGasiti.Add(terminal);

                        }

                    }
                }
            }

        }

        #endregion

        #endregion

        private void buttonExport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
         int liniile = listeGenerate.Count();
          int   coloaneTerminali = terminali.Count();
           int coloaneNeterminali = neterminali.Count();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var w = new StreamWriter(openFileDialog.FileName))
                {
                    string lini = "";
                    for (int i = 0; i < listeGenerate.Count; i++)
                    {
                        lini += i.ToString() + " ";
                    }

                    w.WriteLine(lini);
                    foreach (var terminal in terminali)
                    {

                        w.Write(terminal+" ");
                        w.Flush();
                    }
                    w.WriteLine();
                    for (int i = 0; i < liniile; i++)
                    {
                        for (int j = 0; j < coloaneTerminali; j++)
                        {
                            w.Write(matriceAfisareTerminali[i,j]);
                            w.Flush();
                        }
                        w.WriteLine();
                    }

                    foreach (var neterminal in neterminali)
                    {

                        w.Write(neterminal + " ");
                        w.Flush();
                    }
                    w.WriteLine();
                    for (int i = 0; i < liniile; i++)
                    {
                        for (int j = 0; j < coloaneNeterminali; j++)
                        {
                            w.Write(matriceAfisareNeterminali[i, j]);
                            w.Flush();
                        }
                        w.WriteLine();
                    }
                    for (int i = 0; i < productii.Count-1; i++)
                    {
                        w.Write(productii[i]);
                        w.Flush();
                        w.WriteLine();
                    }


                }

            }
        }
    }
}
