using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace šachys
{
    internal class evaluaceFigurek
    {
        [SupportedOSPlatform("windows6.1")]
        realizacePohybu realizacePohybu = new realizacePohybu();
        detekceSachu detekceSachu = new detekceSachu();

        public const int PawnValue = 100;
        public const int KnightValue = 300;
        public const int BishopValue = 320;
        public const int RookValue = 500;
        public const int QueenValue = 900;

        private static readonly int[,] pawn =
        {
            { 0,   0,   0,   0,   0,   0,   0,   0,},
            {50,  50,  50,  50,  50,  50,  50,  50,},
            {10,  10,  20,  30,  30,  20,  10,  10,},
             {5,   5,  10,  25,  25,  10,   5,   5,},
             {0,   0,   0,  20,  20,   0,   0,   0,},
             {5,  -5, -10,   15,   15, -10,  -5,   5,},
             {5,  10,  10, -10, -10,  10,  10,   5,},
             {0,   0,   0,   0,   0,   0,   0,   0},
        };

        public static readonly int[,] PawnsEnd = {
             { 100,   100,   100,   100,   100,   100,   100,   100, },
            {90,  90,  90,  90,  90,  90,  90,  90,},
            {70,  70,  70,  70,  70,  70,  70,  70,},
            {50,  50,  50,  50,  50,  50,  50,  50,},
            {30,  30,  30,  30,  30,  30,  30,  30,},
            {20,  20,  20,  20,  20,  20,  20,  20,},
            {10,  10,  10,  10,  10,  10,  10,  10,},
             {0,   0,   0,   0,   0,   0,   0,   0},
        };

        public static readonly int[,] Rooks =  {
            {0,  0,  0,  0,  0,  0,  0,  0,},
            {5, 10, 10, 10, 10, 10, 10,  5, },
            {-5,  0,  0,  0,  0,  0,  0, -5,},
            {-5,  0,  0,  0,  0,  0,  0, -5,},
            {-5,  0,  0,  0,  0,  0,  0, -5,},
            {-5,  0,  0,  0,  0,  0,  0, -5,},
            {-5,  0,  0,  0,  0,  0,  0, -5,},
            {0,  0,  0,  5,  5,  0,  0,  0 }
        };
        public static readonly int[,] Knights = {
            {-50,-40,-30,-30,-30,-30,-40,-50,},
            {-40,-20,  0,  0,  0,  0,-20,-40,},
            {-30,  0, 10, 15, 15, 10,  0,-30,},
            {-30,  5, 15, 20, 20, 15,  5,-30,},
            {-30,  0, 15, 20, 20, 15,  0,-30,},
            {-30,  5, 10, 15, 15, 10,  5,-30,},
            {-40,-20,  0,  5,  5,  0,-20,-40,},
            {-50,-40,-30,-30,-30,-30,-40,-50,},
        };
        public static readonly int[,] Bishops =  {
            {-20,-10,-10,-10,-10,-10,-10,-20,},
            {-10,  0,  0,  0,  0,  0,  0,-10,},
            {-10,  0,  5, 10, 10,  5,  0,-10,},
            {-10,  5,  5, 10, 10,  5,  5,-10,},
            {-10,  0, 10, 10, 10, 10,  0,-10,},
            {-10, 10, 10, 10, 10, 10, 10,-10,},
            {-10,  5,  0,  0,  0,  0,  5,-10,},
            {-20,-10,-10,-10,-10,-10,-10,-20,},
        };
        public static readonly int[,] Queens =  {
            {-20,-10,-10, -5, -5,-10,-10,-20,},
            {-10,  0,  0,  0,  0,  0,  0,-10,},
            {-10,  0,  5,  5,  5,  5,  0,-10,},
            {-5,   0,  5,  5,  5,  5,  0, -5,},
            {0,    0,  5,  5,  5,  5,  0, -5, },
            {-10,  5,  5,  5,  5,  5,  0,-10,},
            {-10,  0,  5,  0,  0,  0,  0,-10,},
            {-20,-10,-10, -5, -5,-10,-10,-20},
        };
        public static readonly int[,] KingStart =
        {
            {-80, -70, -70, -70, -70, -70, -70, -80,},
            {-60, -60, -60, -60, -60, -60, -60, -60,},
            {-40, -50, -50, -60, -60, -50, -50, -40,},
            {-30, -40, -40, -50, -50, -40, -40, -30,},
            {-20, -30, -30, -40, -40, -30, -30, -20,},
            {-10, -20, -20, -20, -20, -20, -20, -10,},
            {20,  20,  -5,  -5,  -5,  -5,  20,  20,},
            {20,  30,  10,   0,   0,  10,  30,  20},
        };

        public static readonly int[,] KingEnd =
        {
            {-20, -10, -10, -10, -10, -10, -10, -20,},
            {-5,   0,   5,   5,   5,   5,   0,  -5,},
            {-10, -5,   20,  30,  30,  20,  -5, -10,},
            {-15, -10,  35,  45,  45,  35, -10, -15,},
            {-20, -15,  30,  40,  40,  30, -15, -20,},
            {-25, -20,  20,  25,  25,  20, -20, -25,},
            {-30, -25,   0,   0,   0,   0, -25, -30,},
            {-50, -30, -30, -30, -30, -30, -30, -50},
        };

        int[,] flippedPawn = druhaStrana(pawn);
        int[,] flippedPawnsEnd = druhaStrana(PawnsEnd);
        int[,] flippedRooks = druhaStrana(Rooks);
        int[,] flippedKnights = druhaStrana(Knights);
        int[,] flippedBishops = druhaStrana(Bishops);
        int[,] flippedQueens = druhaStrana(Queens);

        static int[,] druhaStrana(int[,] table)
        {
            int[,] flippedTable = new int[8, 8];

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    flippedTable[y, x] = table[7 - y, 7 - x];
                }
            }

            return flippedTable;
        }

        private static bool isEndgame(Button[,] hraciPole)
        {
            int queenCount = 0, minorPieceCount = 0;
            foreach (Button b in hraciPole)
            {
                if (b.Tag == null) continue;
                string figurka = b.Tag.ToString().ToLower();
                if (figurka == "q") queenCount++;
                if (figurka == "r" || figurka == "n" || figurka == "b") minorPieceCount++;
            }
            return queenCount <= 1 && minorPieceCount <= 3;
        }

        public double evaluate(Button[,] hraciPole, bool bilyNaRade, int[,] stavPole)
        {
            bool kralSach = detekceSachu.jeKralSach(hraciPole, !bilyNaRade, stavPole);
            bool hasValidMoves = maPlatneTahy(hraciPole, !bilyNaRade, stavPole);

            if (kralSach && !hasValidMoves)
            {
                return bilyNaRade ? double.PositiveInfinity : double.NegativeInfinity;
            }
            if (!kralSach && !hasValidMoves)
            {
                return bilyNaRade ? double.NegativeInfinity : double.PositiveInfinity;
            }

            double skore = 0;
            bool endgame = isEndgame(hraciPole);

            int[] poziceWKral = null;
            int[] poziceBKral = null;

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
                        poziceValue = endgame ? (bilyNaRade ? PawnsEnd[pozice[0], pozice[1]] : flippedPawnsEnd[pozice[0], pozice[1]]) : (bilyNaRade ? pawn[pozice[0], pozice[1]] : flippedPawn[pozice[0], pozice[1]]);
                        figurkaValue = PawnValue;

                        int drahaPromote = bilyNaRade ? pozice[0] : 7 - pozice[0];
                        poziceValue += bilyNaRade ? (7 - drahaPromote) * 5 : -((7 - drahaPromote) * 5);

                        if ((bilyNaRade && pozice[0] == 0) || (!bilyNaRade && pozice[0] == 7))
                        {
                            figurkaValue = QueenValue;
                        }
                        break;
                    case "r":
                        poziceValue = bilyNaRade ? Rooks[pozice[0], pozice[1]] : flippedRooks[pozice[0], pozice[1]];
                        figurkaValue = RookValue;
                        break;
                    case "n":
                        poziceValue = bilyNaRade ? Knights[pozice[0], pozice[1]] : flippedKnights[pozice[0], pozice[1]];
                        figurkaValue = KnightValue;
                        break;
                    case "b":
                        poziceValue = bilyNaRade ? Bishops[pozice[0], pozice[1]] : flippedBishops[pozice[0], pozice[1]];
                        figurkaValue = BishopValue;
                        break;
                    case "q":
                        poziceValue = bilyNaRade ? Queens[pozice[0], pozice[1]] : flippedQueens[pozice[0], pozice[1]];
                        figurkaValue = QueenValue;
                        break;
                    case "k":
                        poziceValue = endgame ? KingEnd[pozice[0], pozice[1]] : KingStart[pozice[0], pozice[1]];
                        if (Char.IsUpper(figurka[0]))
                        {
                            poziceWKral = pozice;
                        }
                        else
                        {
                            poziceBKral = pozice;
                        }
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

            if (endgame && poziceWKral != null && poziceBKral != null)
            {
                int drahaKral = Math.Abs(poziceWKral[0] - poziceBKral[0]) + Math.Abs(poziceWKral[1] - poziceBKral[1]);
                skore += bilyNaRade ? drahaKral * 2 : -(drahaKral * 2);
            }

            return skore;
        }


        private bool maPlatneTahy(Button[,] hraciPole, bool bilyNaRade, int[,] stavPole)
        {
            foreach (Button b in hraciPole)
            {
                if (b.Tag == null) continue;
                string figurka = b.Tag.ToString();
                if ((bilyNaRade && Char.IsUpper(figurka[0])) || (!bilyNaRade && Char.IsLower(figurka[0])))
                {
                    List<int> legalMoves = detekceSachu.tahyProtiSachuMatu(stavPole, hraciPole, bilyNaRade, b);
                    if (legalMoves.Count >= 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }





        public static int hodnotaFigurky(string figurka)
        {
            if (figurka == null) return 0;
            switch (figurka.ToLower())
            {
                case "p": return PawnValue;
                case "n": return KnightValue;
                case "b": return BishopValue;
                case "r": return RookValue;
                case "q": return QueenValue;
                case "k": return 10000;
                default: return 0;
            }
        }
    }
}