using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace es_ascensore
{
    public class OminoContrario
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

        private double _decrementa;
        public double Decrementa
        {
            get
            {
                return _decrementa;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("incremento non valido");
                }
                _decrementa = value;
            }
        }

        public double InizioSalita { get; set; }
        public double InizioDiscesa { get; set; }
        public double FineDiscesa { get; set; }
        public double IncrementoSD { get; set; }


        public OminoContrario(double mt, double ml, double d)
        {
            MarginTop = mt; MarginLeft = ml; Decrementa = d;
        }

        public OminoContrario(Thickness m, double d)
        {
            MarginTop = m.Top;
            MarginLeft = m.Left;
            Decrementa = d;
        }

        public Thickness Mossa()
        {
            MarginLeft -= Decrementa;
            return new Thickness(MarginLeft, MarginTop, 0, 0);
        }

    }
}
