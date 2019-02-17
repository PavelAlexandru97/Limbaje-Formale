using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formal_Language
{
    class Functie
    {

       public int IndexIInitil, Ifinal;
       public string modificator;
        public Functie(int IndexIInitil, string modificator, int Ifinal)
        {
            this.IndexIInitil = IndexIInitil;
            this.Ifinal = Ifinal;
            this.modificator = modificator;
        }
    }
}
