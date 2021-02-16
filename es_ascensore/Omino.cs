using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace es_ascensore
{
    public class Omino
    {
        private double _marginTop;
        public double MarginTop
        {
            get
            {
                return _marginTop;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("margin top non valido");
                }
                _marginTop = value;
            }
        }

        private double _marginLeft;
        public double MarginLeft
        {
            get
            {
                return _marginLeft;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("margin left non valido");
                }
                _marginLeft = value;
            }
        }

        private double _incremento;
        public double Incremento
        {
            get
            {
                return _incremento;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("incremento non valido");
                }
                _incremento = value;
            }
        }

        public double InizioSalita { get; set; }
        public double InizioDiscesa { get; set; }
        public double FineDiscesa { get; set; }
        public double IncrementoSD { get; set; }


        public Omino(double mt, double ml, double i)
        {
            MarginTop = mt; MarginLeft = ml; Incremento = i;
        }

        public Omino(Thickness m, double i)
        {
            MarginTop = m.Top;
            MarginLeft = m.Left;
            Incremento = i;
        }

        public Thickness Mossa()
        {
            MarginLeft += Incremento;
            return new Thickness(MarginLeft, MarginTop, 0, 0);
        }

    }
}
