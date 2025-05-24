using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace šachys
{
    internal class evaluaceFigurek
    {
        realizacePohybu realizacePohybu = new realizacePohybu();
        private static readonly Dictionary<string, int> hodnota = new Dictionary<string, int>
        {
            { "p", 100 }, { "r", 500 }, { "n", 320 }, { "b", 330 }, { "q", 900 }, { "k", 2000 }
        };
        private static readonly int[,] pawnWhite = 
        {
            { 60, 60, 60, 60, 60, 60, 60, 60 },
            { 50, 50, 50, 50, 50, 50, 50, 50 },
            { 10, 10, 20, 30, 30, 20, 10, 10 },
            { 5, 5, 10, 25, 25, 10, 5, 5 },
            { 0, 0, 0, 20, 20, 0, 0, 0 },
            { 5, -5, -10, 0, 0, -10, -5, 5 },
            { 5, 10, 10, -20, -20, 10, 10, 5 },
            { 0, 0, 0, 0, 0, 0, 0, 0 }
        };
        private static readonly int[,] pawnBlack =
        {
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 5, 10, 10, -20, -20, 10, 10, 5 },
            { 5, -5, -10, 0, 0, -10, -5, 5 },
            { 0, 0, 0, 20, 20, 0, 0, 0 },
            { 5, 5, 10, 25, 25, 10, 5, 5 },
            { 10, 10, 20, 30, 30, 20, 10, 10 },
            { 50, 50, 50, 50, 50, 50, 50, 50 },
            { 60, 60, 60, 60, 60, 60, 60, 60 }
        };

        private static readonly int[,] kral =
        {
            { 900, -50, 10, 10, 10, 10, -50, 900 },
            { -50, 0, 5, 5, 5, 5, 0, -50 },
            { 10, 5, 0, 0, 0, 0, 5, 10 },
            { 10, 5, 0, -10, -10, 0, 5, 10 },
            { 10, 5, 0, -10, -10, 0, 5, 10 },
            { 10, 5, 0, 0, 0, 0, 5, 10 },
            { -50, 0, 5, 5, 5, 5, 0, -50 },
            { 900, -50, 10, 10, 10, 10, -50, 900 }
        };

        private static readonly int[,] knight =
        {
            { -50, -40, -30, -30, -30, -30, -40, -50 },
            { -40, -20, 0, 0, 0, 0, -20, -40 },
            { -30, 0, 10, 15, 15, 10, 0, -30 },
            { -30, 5, 15, 20, 20, 15, 5, -30 },
            { -30, 0, 15, 20, 20, 15, 0, -30 },
            { -30, 5, 10, 15, 15, 10, 5, -30 },
            { -40, -20, 0, 5, 5, 0, -20, -40 },
            {-50, -40, -30, -30, -30, -30, -40, -50 }
        };

        private static readonly int[,] bishop =
        {
            { -20, -10, -10, -10, -10, -10, -10, -20 },
            { -10, 5, 0, 0, 0, 0, 5, -10 },
            { -10, 10, 10, 10, 10, 10, 10, -10 },
            { -10, 0, 10, 10, 10, 10, 0, -10 },
            { -10, 5, 5, 10, 10, 5, 5, -10 },
            { -10, 0, 5, 10, 10, 5, 0, -10 },
            { -10, 0, 0, 0, 0, 0, 0, -10 },
            { -20, -10, -10, -10, -10, -10, -10, -20 }
        };

        private static readonly int[,] rook =
        {
            { 0, 0, 0, 5, 5, 0, 0, 0 },
            { -5, 0, 0, 0, 0, 0, 0, -5 },
            { -5, 0, 0, 0, 0, 0, 0, -5 },
            { -5, 0, 0, 0, 0, 0, 0, -5 },
            { -5, 0, 0, 0, 0, 0, 0, -5 },
            { -5, 0, 0, 0, 0, 0, 0, -5 },
            { 5, 10, 10, 10, 10, 10, 10, 5 },
            { 0, 0, 0, 5, 5, 0, 0, 0 }
        };
        private static readonly int[,] kralovna =
        {
            { -20, -10, -10, -5, -5, -10, -10, -20 },
            { -10, 0, 0, 0, 0, 0, 0, -10 },
            { -10, 0, 5, 5, 5, 5, 0, -10 },
            { -5, 0, 5, 5, 5, 5, 0, -5 },
            { 0, 0, 5, 5, 5, 5, 0, -5 },
            { -10, 5, 5, 5, 5, 5, 0, -10 },
            { -10, 0, 5, 0, 0, 0, 0, -10 },
            { -20, -10, -10, -5, -5, -10, -10, -20 }
        };


        public int evaluate(Button[,] hraciPole, bool bilyNaRade)
        {
            int skore = 0;
            foreach (Button b in hraciPole)
            {
                if (b.Tag == null) continue;

                string figurka = b.Tag.ToString();
                int[] pozice = realizacePohybu.hledacSouradnic(b, hraciPole);
                int figurkaValue = 0;
                int poziceValue = 0;

                switch (figurka.ToLower())
                {
                    case "p":
                        figurkaValue = hodnota["p"];
                        poziceValue = bilyNaRade ? pawnWhite[pozice[0], pozice[1]] : pawnBlack[pozice[0], pozice[1]];
                        break;
                    case "r":
                        figurkaValue = hodnota["r"];
                        poziceValue = rook[pozice[0], pozice[1]];
                        break;
                    case "n":
                        figurkaValue = hodnota["n"];
                        poziceValue = knight[pozice[0], pozice[1]];
                        break;
                    case "b":
                        figurkaValue = hodnota["b"];
                        poziceValue = bishop[pozice[0], pozice[1]];
                        break;
                    case "q":
                        figurkaValue = hodnota["q"];
                        poziceValue = kralovna[pozice[0], pozice[1]];
                        break;
                    case "k":
                        figurkaValue = hodnota["k"];
                        poziceValue = kral[pozice[0], pozice[1]];
                        break;
                }

                if (Char.IsUpper(figurka[0]))
                {
                    skore += figurkaValue + poziceValue;
                }
                else
                {
                    skore -= figurkaValue + poziceValue;
                }
            }
            return skore;
        }

        public int evalPiece(string ID)
        {
            return hodnota[ID.ToLower()];
        }

    }
}
