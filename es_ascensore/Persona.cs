using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace es_ascensore
{
    public class Persona : IEquatable<Persona>
    {
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            private set
            {
                if (value < 0)
                    throw new Exception("id non valido");
                _id = value;
            }
        }

        private int _destinazione;
        public int Destinazione
        {
            get
            {
                return _destinazione;
            }
            set
            {
                if (value < 0)
                    throw new Exception("destinazione non valida, deve essere maggiore di 0");
                _destinazione = value;
            }
        }

        public Persona(int id, int destinazione)
        {
            Id = 0; Destinazione = destinazione;
        }

        public bool Equals(Persona other)
        {
            if (this.Id == other.Id)
                return true;
            return false;
        }
    }
}