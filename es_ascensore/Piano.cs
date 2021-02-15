using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace es_ascensore
{
    public class Piano : IEquitableNPiano, IEquatable<Piano>
    {
        public List<Persona> Persone;
        private int _numMaxPersone;
        public int NumMaxPersone
        {
            get
            {
                return _numMaxPersone;
            }
            set
            {
                if(value <= 0)
                {
                    throw new Exception("numero di persone massimo non valido");
                }
                _numMaxPersone = NumMaxPersone;
            }
        }

        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("id non valido");
                }
                _id = Id;
            }
        }

        public Piano(int nmPersone, int id)
        {
            Persone = new List<Persona>();
            NumMaxPersone = nmPersone;
            Id = id;
        }

        public void AggiungiPersona(Persona p)
        {
            Persone.Add(p);
        }

        public bool GetEquals(int nPiano)
        {
            if (Id == nPiano)
                return true;
            return false;
        }

        public bool Equals(Piano other)
        {
            if (this.Id == other.Id)
                return true;
            return false;
        }
    }
}