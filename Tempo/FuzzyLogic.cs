using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tempo
{
    public class FuzzyLogic
    {
        public double GetPower(int input)
        {
            double[,] X = new double[4, 21]; //temperatura
            double[,] Y = new double[4, 21]; //moc pieca

            int Arraylength = X.GetLength(1);

            double[,] wnioskowanie_Y = new double[4, 21];

            double[,] agregacja_Y = new double[2, 21];

            double uA, uB, uC, uD, uE, uF;
            double a, x0, x1, x2, b, x;

            double zmienna_wejsciowa;
            double roz_uA = 0, roz_uB = 0, roz_uC = 0;

            double srodek_ciezkosci;
            double gora = 0, dol = 0;

            //zerowanie macierzy X i Y
            for (int i = 0; i < X.GetLength(0); i++)
            {
                for (int j = 0; j < Arraylength; j++)
                {
                    X[i, j] = 0;
                    Y[i, j] = 0;
                }
            }

            //---------------------------------------------------------------------------
            //zmienna lingwistyczna X - temperatura

            for (int i = 0; i < Arraylength; i++)
            {
                x = i + 10;
                X[0, i] = x;

                //trójkąt uA - temperatura niska
                a = 10;
                x0 = 10;
                b = 20;

                if (a <= x && x <= x0)
                {
                    double temp = x0 - a;
                    if (temp != 0)
                    {
                        uA = (x - a) / (x0 - a);
                    }
                    else
                    {
                        uA = 1;
                    }
                }
                else if (x0 <= x && x <= b)
                {
                    uA = (b - x) / (b - x0);
                }
                else
                {
                    uA = 0;
                }

                //trójkąt uB - temperatura średnia
                a = 15;
                x0 = 20;
                b = 25;

                if (a <= x && x <= x0)
                {
                    uB = (x - a) / (x0 - a);
                }
                else if (x0 <= x && x <= b)
                {
                    uB = (b - x) / (b - x0);
                }
                else
                {
                    uB = 0;
                }

                //trójkąt uC - temperatura wysoka
                a = 20;
                x0 = 30;
                b = 30;

                if (a <= x && x <= x0)
                {
                    uC = (x - a) / (x0 - a);
                }
                else if (x0 <= x && x <= b)
                {
                    double temp = b - x0;
                    if (temp != 0)
                    {
                        uC = (b - x) / (b - x0);
                    }
                    else
                    {
                        uC = 1;
                    }
                }
                else
                {
                    uC = 0;
                }

                X[1, i] = uA;
                X[2, i] = uB;
                X[3, i] = uC;
            }

            //---------------------------------------------------------------------------
            //zmienna lingwistyczna Y - moc pieca

            for (int i = 0; i < Arraylength; i++)
            {
                x = i * 5;
                Y[0, i] = x;

                //trapez uD - moc pieca niska
                a = 0;
                x1 = 0;
                x2 = 20;
                b = 40;

                if (a <= x && x <= x1)
                {
                    double temp = x1 - a;
                    if (temp != 0)
                    {
                        uD = (x - a) / (x1 - a);
                    }
                    else
                    {
                        uD = 1;
                    }
                }
                else if (x1 <= x && x <= x2)
                {
                    uD = 1;
                }
                else if (x2 <= x && x <= b)
                {
                    uD = (b - x) / (b - x2);
                }
                else
                {
                    uD = 0;
                }

                //trójkąt uE - moc pieca średnia
                a = 30;
                x0 = 50;
                b = 70;

                if (a <= x && x <= x0)
                {
                    uE = (x - a) / (x0 - a);
                }
                else if (x0 <= x && x <= b)
                {
                    uE = (b - x) / (b - x0);
                }
                else
                {
                    uE = 0;
                }

                //trapez uF - moc pieca wysoka
                a = 60;
                x1 = 80;
                x2 = 100;
                b = 100;

                if (a <= x && x <= x1)
                {
                    uF = (x - a) / (x1 - a);
                }
                else if (x1 <= x && x <= x2)
                {
                    uF = 1;
                }
                else if (x2 <= x && x <= b)
                {
                    double temp = b - x2;
                    if (temp != 0)
                    {
                        uF = (b - x) / (b - x2);
                    }
                    else
                    {
                        uF = 1;
                    }
                }
                else
                {
                    uF = 0;
                }

                Y[1, i] = uD;
                Y[2, i] = uE;
                Y[3, i] = uF;
            }

            zmienna_wejsciowa = input;

            for (int i = 0; i < Arraylength; i++)
            {
                if (X[0, i] == zmienna_wejsciowa)
                {
                    roz_uA = X[1, i];
                    roz_uB = X[2, i];
                    roz_uC = X[3, i];
                }
            }

            //REGUŁY:
            //R1: Jeżeli x=A to y=F - temperatura niska -> ogrzewanie wysokie
            //R2: Jeżeli x=B to y=E - temperatura średnia -> ogrzewanie średnie
            //R3: Jeżeli x=C to y=D - temperatura wysoka -> ogrzewanie niskie

            for (int i = 0; i < Arraylength; i++)
            {
                wnioskowanie_Y[0, i] = i*5;
                agregacja_Y[0, i] = i*5;

                if (Y[3, i] > roz_uA) //uF do uA
                {
                    wnioskowanie_Y[3, i] = roz_uA;
                }
                else
                {
                    wnioskowanie_Y[3, i] = Y[3, i];
                }

                if (Y[2, i] > roz_uB) //uE do uB
                {
                    wnioskowanie_Y[2, i] = roz_uB;
                }
                else
                {
                    wnioskowanie_Y[2, i] = Y[2, i];
                }

                if (Y[1, i] > roz_uC) //uD do uC
                {
                    wnioskowanie_Y[1, i] = roz_uC;
                }
                else
                {
                    wnioskowanie_Y[1, i] = Y[1, i];
                }

                double temp;
                temp = Math.Max(wnioskowanie_Y[1, i], wnioskowanie_Y[2, i]);
                agregacja_Y[1, i] = Math.Max(temp, wnioskowanie_Y[3, i]);

                gora += agregacja_Y[1, i] * agregacja_Y[0, i];
                dol += agregacja_Y[1, i];
            }

            srodek_ciezkosci = gora / dol;
            return srodek_ciezkosci;
        }
    }
}
