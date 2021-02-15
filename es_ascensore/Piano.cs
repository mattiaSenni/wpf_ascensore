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
                if (value < 0)
                {
                    throw new Exception("id non valido");
                }
                _id = value;
            }
        }

        private int _marginTop;
        public int MarginTop
        {
            get
            {
                return _marginTop;
            }
            set
            {
                if (value < 0)
                    throw new Exception();
                _marginTop = value;
            }
        }

        public Piano(int nmPersone, int id, int mt)
        {
            try
            {
                Persone = new List<Persona>();
                NumMaxPersone = nmPersone;
                Id = id;
                MarginTop = mt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
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