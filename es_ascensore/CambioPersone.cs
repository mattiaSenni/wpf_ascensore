using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace es_ascensore
{
    public class CambioPersone
    {
        private int _personeScese;
        public int PersoneScese
        {
            get
            {
                return _personeScese;
            }
            set
            {
                if (value < 0)
                    throw new Exception();
                _personeScese = value;
            }
        }
        private int _personeSalite;
        public int PersoneSalite
        {
            get
            {
                return _personeSalite;
            }
            set
            {
                if (value < 0)
                    throw new Exception();
                _personeSalite = value;
            }
        }


    }
}