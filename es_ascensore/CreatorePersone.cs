using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace es_ascensore
{
    public static class CreatorePersone
    {
        public static int idCorrente;
        public static Persona CreaPersona(int pianoDestinazione)
        {
            idCorrente++;
            return new Persona(idCorrente, pianoDestinazione);
        }
    }
}