using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace es_ascensore
{
    public class Ascensore
    {
        private int _piano;
        public int Piano
        {
            get
            {
                return _piano;
            }
            set
            {
                if (value < 0)
                    throw new Exception("numero di piano non valido");
                _piano = value;
            }
        }

        private int _maxPersone;
        public int MaxPersone
        {
            get
            {
                return _maxPersone;
            }
            set
            {
                if (value <= 0)
                    throw new Exception("numero massimo di persone");
                _maxPersone = value;
            }
        }

        public List<Piano> Piani { get; set; }
        public int Persone { get; set; }
        //public Queue<int> Fila { get; set; }
        public List<int> Fila { get; set; }
        private Semaphore _pool { get; set; }
        private object x;
        //public bool AspettaPrenotazione { get; private set; }
        public List<int> Prenotazioni { get; set; }//da tastierino

        public Ascensore(int maxPersone)
        {
            Piano = 0;
            MaxPersone = maxPersone;
            Piani = new List<Piano>() { new Piano(5, 0, 464), new Piano(5, 1, 365), new Piano(5, 2, 250), new Piano(5, 3, 135), new Piano(5, 4, 15) };
            Persone = 0;
            //Fila = new Queue<int>();
            Fila = new List<int>();
            _pool = new Semaphore(0, 1);
            _pool.Release(1);
            //AspettaPrenotazione = false;
            x = new object();
            Prenotazioni = new List<int>();
        }

        public Piano Vai()
        {
            try
            {
                
                Piano toReturn = GetPiano(Peek());
                Piano = Pop(); //sezione critica : il dequeue
                
                return toReturn;
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Arrivato()
        {
            Scendi();
            int salite = Sali();
            if(salite > 0)
            {
                //sono salite persone
                //AspettaPrenotazione = true;
            }
            return salite;
        }
        public bool Continua()
        {
            if (Count() > 0)
                return true;
            return false;
        }
        private void Scendi()
        {
            if(Persone > 0 && Prenotazioni.Contains(Piano))
            {
                Persone--;
                Prenotazioni.Remove(Piano);
            }
                
        }
        public int QuantiScendono()
        {
            if (Persone > 0 && Prenotazioni.Contains(Piano))
                return 1;
            else
                return 0;
        }
        private int Sali()
        {
            int pSalite = 0;
            for(int i = 0; i < Piani[Piano].NPersone; i++)
            {
                if (Persone + pSalite < MaxPersone)
                {
                    //può salire
                    pSalite++;
                }
            }
            for(int i = 0; i < pSalite; i++)
            {
                Piani[Piano].NPersone--;
                Persone++;
            }
            //if (pSalite > 0)
                //AspettaPrenotazione = true;
            return pSalite;
        }
        public void Prenota(int piano)
        {
            Add(piano);
            //AspettaPrenotazione = true;
        }
        public void Tastierino(int piano)
        {
            PushFirst(piano);
            //AspettaPrenotazione = false;
            if(!Prenotazioni.Contains(piano))
            {
                Prenotazioni.Add(piano);
            }
        }

        private Piano GetPiano(int n)
        {
            for(int i = 0; i < Piani.Count; i++)
            {
                if(Piani[i].GetEquals(n))
                {
                    return Piani[i];
                }
            }
            throw new Exception();
        }
        public int Pop()
        {
            //_pool.WaitOne();

            int el;
            lock (x)
            {
                el = Fila[0];
                Fila.RemoveAt(0);            
            }
            return el;
        }

        public void Add(int piano)
        {
            //_pool.WaitOne();
            lock(x)
            {
                if (!Fila.Contains(piano))
                {
                    Fila.Add(piano);
                }
            }
            
            //_pool.Release(1);
        }
        public void PushFirst(int piano)
        {
            //_pool.WaitOne();
            lock(x)
            {
                if (!Fila.Contains(piano))
                {
                    List<int> n = new List<int>();
                    n.Add(piano);
                    foreach (int i in Fila)
                    {
                        n.Add(i);
                    }
                    Fila.Clear();
                    foreach (int i in n)
                    {
                        Fila.Add(i);
                    }
                }
            }
            //_pool.Release(1);
        }
        public int Count()
        {
            //_pool.WaitOne();
            int num;
            lock(x)
            {
                num = Fila.Count;
            }
            //_pool.Release(1);
            return num;
        }
        public int Peek()
        {
            try
            {
                //_pool.WaitOne();
                int p;
                lock(x)
                {
                    p = Fila[0];
                }
                //_pool.Release(1);
                return p;
            }
            catch ( Exception ex)
            {

                throw ex;
            }
        }

    }
}