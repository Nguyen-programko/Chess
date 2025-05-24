﻿using System;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace šachys
{
    internal class realizacePohybu
    {
        [SupportedOSPlatform("windows6.1")]
        TridaFigurky TridaFigurky = new TridaFigurky();
        public int[] hledacSouradnic(Button kordButton, Button[,] hraciPole)
        {
            int[] kords = new int[2];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (hraciPole[i, j].Equals(kordButton))
                    {
                        kords[0] = i;
                        kords[1] = j;
                    }
                }
            }
            return kords;
        }

        private bool volnoCasteling(int[] pole, int[] pole2, Button[,] hraciPole)
        {
            if (pole[1] < pole2[1])
            {
                for (int i = pole[1] + 1; i < pole2[1]; i++)
                {
                    if (hraciPole[pole[0], i].BackgroundImage != null)
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = pole[1] - 1; i > pole2[1]; i--)
                {
                    if (hraciPole[pole[0], i].BackgroundImage != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Button[,] polozeniFigurek(string kodfigurky, int[] From, int[] To, Button[,] hraciPole)
        {
            hraciPole[From[0], From[1]].BackgroundImage = null;
            hraciPole[From[0], From[1]].Tag = null;
            hraciPole[To[0], To[1]].BackgroundImage = TridaFigurky.rozpoznavaniFigurek(kodfigurky);
            hraciPole[To[0], To[1]].Tag = kodfigurky;
            return hraciPole;
        }

        public Button[,] enPeasentMove(string kodfigurky, int[] pole, int[] pole2, int[] pole3, Button[,] hraciPole)
        {
            hraciPole[pole[0], pole[1]].BackgroundImage = null;
            hraciPole[pole[0], pole[1]].Tag = null;
            hraciPole[pole3[0], pole3[1]].BackgroundImage = null;
            hraciPole[pole3[0], pole3[1]].Tag = null;
            hraciPole[pole2[0], pole2[1]].BackgroundImage = TridaFigurky.rozpoznavaniFigurek(kodfigurky);
            hraciPole[pole2[0], pole2[1]].Tag = kodfigurky;
            return hraciPole;
        }

        public Button[,] enPeasantCapture(string kodfigurky, string captureFigurka, int[] pole, int[] pole2, int[] pole3, Button[,] hraciPole, bool bilyNaRade)
        {
            if (captureFigurka != null && ((!Char.IsUpper(captureFigurka[0]) && bilyNaRade) || (Char.IsUpper(captureFigurka[0]) && !bilyNaRade)))
            {
                hraciPole = enPeasentMove(kodfigurky, pole, pole2, pole3, hraciPole);
            }
            return hraciPole;
        }

        public Button[,] castelingMove(int[] pole, int[] pole2, string vybranaFigurka, string aktualniFigurka, Button[,] hraciPole)
        {
            if (pole2[1] > pole[1])
            {
                hraciPole[pole[0], pole[1] + 1].BackgroundImage = TridaFigurky.rozpoznavaniFigurek(aktualniFigurka);
                hraciPole[pole[0], pole[1] + 1].Tag = aktualniFigurka;
                hraciPole[pole2[0], pole2[1] - 1].BackgroundImage = TridaFigurky.rozpoznavaniFigurek(vybranaFigurka);
                hraciPole[pole2[0], pole2[1] - 1].Tag = vybranaFigurka;
                hraciPole[pole[0], pole[1]].BackgroundImage = null;
                hraciPole[pole[0], pole[1]].Tag = null;
                hraciPole[pole2[0], pole2[1]].BackgroundImage = null;
                hraciPole[pole2[0], pole2[1]].Tag = null;
            }
            else
            {
                hraciPole[pole[0], pole[1] - 1].BackgroundImage = TridaFigurky.rozpoznavaniFigurek(aktualniFigurka);
                hraciPole[pole[0], pole[1] - 1].Tag = aktualniFigurka;
                hraciPole[pole2[0], pole2[1] + 2].BackgroundImage = TridaFigurky.rozpoznavaniFigurek(vybranaFigurka);
                hraciPole[pole2[0], pole2[1] + 2].Tag = vybranaFigurka;
                hraciPole[pole[0], pole[1]].BackgroundImage = null;
                hraciPole[pole[0], pole[1]].Tag = null;
                hraciPole[pole2[0], pole2[1]].BackgroundImage = null;
                hraciPole[pole2[0], pole2[1]].Tag = null;
            }
            return hraciPole;
        }

        bool castled = false;

        public Button[,] castlingMoveHandle(int[] pole, int[] pole2, string vybranaFigurka, string aktualniFigurka, Button[,] hraciPole, string[] RookMoves, bool KingBMove, bool KingWMove)
        {
            castled = false;
            bool jeVolno = volnoCasteling(pole, pole2, hraciPole);

            if (((Char.IsUpper(vybranaFigurka[0]) && !KingWMove) || (!Char.IsUpper(vybranaFigurka[0]) && !KingBMove)))
            {
                if (jeVolno)
                {
                    if (!KingBMove && pole2[0] == 0 && pole2[1] == 0 && RookMoves[0] == null)
                    {
                        hraciPole = castelingMove(pole, pole2, vybranaFigurka, aktualniFigurka, hraciPole);
                    }
                    else if (!KingBMove && pole2[0] == 0 && pole2[1] == 7 && RookMoves[1] == null)
                    {
                        hraciPole = castelingMove(pole, pole2, vybranaFigurka, aktualniFigurka, hraciPole);
                    }
                    else if (!KingWMove && pole2[0] == 7 && pole2[1] == 0 && RookMoves[2] == null)
                    {
                        hraciPole = castelingMove(pole, pole2, vybranaFigurka, aktualniFigurka, hraciPole);
                    }
                    else if (!KingWMove && pole2[0] == 7 && pole2[1] == 7 && RookMoves[3] == null)
                    {
                        hraciPole = castelingMove(pole, pole2, vybranaFigurka, aktualniFigurka, hraciPole);
                    }
                    castled = true;
                }
            }
            return hraciPole;
        }

        public bool vratitCastled()
        {
            return castled;
        }
    }
}
