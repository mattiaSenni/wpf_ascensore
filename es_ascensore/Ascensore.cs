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
        public List<Persona> Persone { get; set; }
        public Queue<int> Fila { get; set; }
        private Semaphore _pool { get; set; }
        public bool StaAndando { get; private set; }

        public Ascensore(int maxPersone)
        {
            Piano = 0;
            MaxPersone = maxPersone;
            Piani = new List<Piano>() { new Piano(5, 0, 385), new Piano(5, 1, 200), new Piano(5, 2, 5) };
            Persone = new List<Persona>();
            Fila = new Queue<int>();
            _pool = new Semaphore(0, 1);
            _pool.Release(1);
            StaAndando = false;
        }

        public Piano Vai()
        {
            try
            {
                Piano toReturn = GetPiano(Fila.Peek());
                Piano = Dequeue(); //sezione critica : il dequeue
                StaAndando = true;
                return toReturn;
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Arrivato()
        {
            Scendi(Piano);
            Sali(Piani[Piano].Persone);
            if (Count() == 0)
                StaAndando = false;
        }
        public bool Continua()
        {
            if (Count() > 0)
                return true;
            return false;
        }
        private void Scendi(int numPiano)
        {
            //faccio scendere le persone
            foreach (Persona p in Persone)
            {
                if (p.Destinazione == numPiano)
                {
                    //la faccio scendere
                    Persone.Remove(p); //Persona implementa l'Equals
                }
            }
        }
        public int QuantiScendono(int numPiano)
        {
            int count = 0;
            foreach (Persona p in Persone)
            {
                if (p.Destinazione == numPiano)
                {
                    count++;
                }
            }
            return count;
        }
        public int QuantiSalgono(List<Persona>personas)
        {
            int personeCheNonSalgono = 0;

            foreach (Persona p in personas)
            {
                if (Fila.Count >= MaxPersone)
                {
                    personeCheNonSalgono++;
                }                    
            }

            return personeCheNonSalgono;
        }
        private int Sali(List<Persona> personas)
        {
            //la destinazione la chiedo appena creo la persona
            //faccio salire le persone
            int personeCheNonSalgono = 0;
            
            foreach (Persona p in personas)
            {
                if (Fila.Count < MaxPersone)
                {                    
                    Enqueue(p.Destinazione);
                }
                else
                    personeCheNonSalgono++;
            }

            return personeCheNonSalgono;

        }
        public void Prenota(int piano)
        {
            Enqueue(piano);
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

        public int Dequeue()
        {
            _pool.WaitOne();
            int num = Fila.Dequeue();
            _pool.Release(1);
            return num;
        }
        public void Enqueue(int piano)
        {
            _pool.WaitOne();
            if(!Fila.Contains(piano))
            {
                Fila.Enqueue(piano);
            }
            _pool.Release(1);
        }
        public int Count()
        {
            _pool.WaitOne();
            int res = Fila.Count;
            _pool.Release(1);
            return res;
        }
    }
}