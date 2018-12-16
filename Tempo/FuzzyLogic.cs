using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tempo
{
    /// <summary>
    /// Fuzzy Logic core of heating system
    /// </summary>
    public class FuzzyLogic
    {
        /// <summary>
        /// Gets power of heating system based on input
        /// </summary>
        public double GetPower(int input)
        {
            int Arraylength = 61;

            double[,] X = new double[5, Arraylength]; //temperatura
            double[,] Y = new double[5, Arraylength]; //moc pieca

            double[,] wnioskowanie_Y = new double[5, Arraylength];

            double[,] agregacja_Y = new double[2, Arraylength];

            //double uA, uB, uC, uD, uE, uF;

            double tempL, tempM, tempMH, tempH;
            double heatL, heatM, heatMH, heatH;
            double a, x0, x1, x2, b, x;

            double zmienna_wejsciowa;
            double roz_tempL = 0, roz_tempM = 0, roz_tempMH = 0, roz_tempH = 0;

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
                x = (i * 0.5) - 15;
                X[0, i] = x;

                //trapez tempL - temperatura niska
                a = -15;
                x1 = -15;
                x2 = 0;
                b = 5;

                if (a <= x && x <= x1)
                {
                    double temp = x1 - a;
                    if (temp != 0)
                    {
                        tempL = (x - a) / (x1 - a);
                    }
                    else
                    {
                        tempL = 1;
                    }
                }
                else if (x1 <= x && x <= x2)
                {
                    tempL = 1;
                }
                else if (x2 <= x && x <= b)
                {
                    tempL = (b - x) / (b - x2);
                }
                else
                {
                    tempL = 0;
                }

                //trójkąt tempM - temperatura średnia
                a = 0;
                x0 = 5;
                b = 10;

                if (a <= x && x <= x0)
                {
                    double temp = x0 - a;
                    if (temp != 0)
                    {
                        tempM = (x - a) / (x0 - a);
                    }
                    else
                    {
                        tempM = 1;
                    }
                }
                else if (x0 <= x && x <= b)
                {
                    tempM = (b - x) / (b - x0);
                }
                else
                {
                    tempM = 0;
                }

                //trójkąt tempMH - temperatura średnio-wysoka
                a = 5;
                x0 = 10;
                b = 15;

                if (a <= x && x <= x0)
                {
                    tempMH = (x - a) / (x0 - a);
                }
                else if (x0 <= x && x <= b)
                {
                    tempMH = (b - x) / (b - x0);
                }
                else
                {
                    tempMH = 0;
                }

                //trójkąt tempH - temperatura wysoka
                a = 10;
                x0 = 15;
                b = 15;

                if (a <= x && x <= x0)
                {
                    tempH = (x - a) / (x0 - a);
                }
                else if (x0 <= x && x <= b)
                {
                    double temp = b - x0;
                    if (temp != 0)
                    {
                        tempH = (b - x) / (b - x0);
                    }
                    else
                    {
                        tempH = 1;
                    }
                }
                else
                {
                    tempH = 0;
                }

                X[1, i] = tempL;
                X[2, i] = tempM;
                X[3, i] = tempMH;
                X[4, i] = tempH;
            }

            //---------------------------------------------------------------------------
            //zmienna lingwistyczna Y - moc pieca

            for (int i = 0; i < Arraylength; i++)
            {
                x = (i * 2.63934426) - 30;
                Y[0, i] = x;

                //trapez heatL - moc pieca niska
                a = -30;
                x1 = -10;
                x2 = 10;
                b = 30;

                if (a <= x && x <= x1)
                {
                    double temp = x1 - a;
                    if (temp != 0)
                    {
                        heatL = (x - a) / (x1 - a);
                    }
                    else
                    {
                        heatL = 1;
                    }
                }
                else if (x1 <= x && x <= x2)
                {
                    heatL = 1;
                }
                else if (x2 <= x && x <= b)
                {
                    heatL = (b - x) / (b - x2);
                }
                else
                {
                    heatL = 0;
                }

                //trójkąt heatM - moc pieca średnia
                a = 20;
                x0 = 40;
                b = 60;

                if (a <= x && x <= x0)
                {
                    heatM = (x - a) / (x0 - a);
                }
                else if (x0 <= x && x <= b)
                {
                    heatM = (b - x) / (b - x0);
                }
                else
                {
                    heatM = 0;
                }

                //trójkąt heatMH - moc pieca średnio-wysoka
                a = 40;
                x0 = 60;
                b = 80;

                if (a <= x && x <= x0)
                {
                    heatMH = (x - a) / (x0 - a);
                }
                else if (x0 <= x && x <= b)
                {
                    heatMH = (b - x) / (b - x0);
                }
                else
                {
                    heatMH = 0;
                }

                //trapez heatH - moc pieca wysoka
                a = 70;
                x1 = 90;
                x2 = 110;
                b = 130;

                if (a <= x && x <= x1)
                {
                    heatH = (x - a) / (x1 - a);
                }
                else if (x1 <= x && x <= x2)
                {
                    heatH = 1;
                }
                else if (x2 <= x && x <= b)
                {
                    double temp = b - x2;
                    if (temp != 0)
                    {
                        heatH = (b - x) / (b - x2);
                    }
                    else
                    {
                        heatH = 1;
                    }
                }
                else
                {
                    heatH = 0;
                }

                Y[1, i] = heatL;
                Y[2, i] = heatM;
                Y[3, i] = heatMH;
                Y[4, i] = heatH;
            }

            zmienna_wejsciowa = input;

            for (int i = 0; i < Arraylength; i++)
            {
                if (X[0, i] == zmienna_wejsciowa)
                {
                    roz_tempL = X[1, i];
                    roz_tempM = X[2, i];
                    roz_tempMH = X[3, i];
                    roz_tempH = X[4, i];
                }
            }

            //----wnioskowanie

            for (int i = 0; i < Arraylength; i++)
            {
                wnioskowanie_Y[0, i] = (i * 2.63934426) - 30;
                agregacja_Y[0, i] = (i * 2.63934426) - 30;

                if (Y[1, i] > roz_tempL) //tempL do heatL
                {
                    wnioskowanie_Y[1, i] = roz_tempL;
                }
                else
                {
                    wnioskowanie_Y[1, i] = Y[1, i];
                }

                if (Y[2, i] > roz_tempM) //tempM do heatM
                {
                    wnioskowanie_Y[2, i] = roz_tempM;
                }
                else
                {
                    wnioskowanie_Y[2, i] = Y[2, i];
                }

                if (Y[3, i] > roz_tempMH) //tempMH do heatMH
                {
                    wnioskowanie_Y[3, i] = roz_tempMH;
                }
                else
                {
                    wnioskowanie_Y[3, i] = Y[3, i];
                }

                if (Y[4, i] > roz_tempH) //tempH do heatH
                {
                    wnioskowanie_Y[4, i] = roz_tempH;
                }
                else
                {
                    wnioskowanie_Y[4, i] = Y[4, i];
                }

                double temp, temp2;
                temp = Math.Max(wnioskowanie_Y[1, i], wnioskowanie_Y[2, i]);
                temp2 = Math.Max(temp, wnioskowanie_Y[3, i]);
                agregacja_Y[1, i] = Math.Max(temp2, wnioskowanie_Y[4, i]);

                gora += agregacja_Y[1, i] * agregacja_Y[0, i];
                dol += agregacja_Y[1, i];
            }

            srodek_ciezkosci = gora / dol;
            return srodek_ciezkosci;
        }
    }
}