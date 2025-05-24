using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace šachys
{
    internal class ChessKomp
    {
        [SupportedOSPlatform("windows6.1")]
        detekceSachu detekceSachu = new detekceSachu();
        realizacePohybu realizacePohybu = new realizacePohybu();
        evaluaceFigurek evaluaceFigurek = new evaluaceFigurek();
        TridaFigurky TridaFigurky = new TridaFigurky();
        int[] minuleSouradniceGlobal;
        int[] aktualniSouradniceGlobal;
        int[] enpeasantGlobal = new int[2];
        string vybranaFigurkaGlobal;
        Button vybranaFigurkaBtnGlobal;
        string sebranaFigurkaGlobal;
        double testScore = 0;
        bool povyseni = false;
        List<TAH> platneTahy;
        private Dictionary<string, double> transpositionTable = new Dictionary<string, double>();

        int illitarion = 0;

        public Button[,] AI(Button[,] board, int[,] stavHracihoPole, bool bilyNaRade)
        {
            int maxDepth = 15;
            Button vybranaFigurka = null;
            int[] MoveTo = null;
            double bestScore = bilyNaRade ? double.NegativeInfinity : double.PositiveInfinity;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            platneTahy = srovnatTahy(board, stavHracihoPole, bilyNaRade);
            for (int depth = 1; depth <= maxDepth; depth++)
            {
                if (stopwatch.ElapsedMilliseconds >= 5000)
                {
                    break;
                }
                foreach (TAH move in platneTahy)
                {
                    int[] from = move.From;
                    int[] To = move.To;
                    var originalBg = board[To[0], To[1]].BackgroundImage;
                    var originalTag = board[To[0], To[1]].Tag;
                    var figurkaTag = board[from[0], from[1]].Tag.ToString();
                    polozeniFigurek(figurkaTag, from, To, board);
                    int[,] tempStavPole = updateStavHracihoPole(board);

                    double score = Minimax(board, tempStavPole, depth, !bilyNaRade, double.NegativeInfinity, double.PositiveInfinity);

                    polozeniFigurek(figurkaTag, To, from, board);
                    board[To[0], To[1]].BackgroundImage = originalBg;
                    board[To[0], To[1]].Tag = originalTag;

                    if ((bilyNaRade && score > bestScore) || (!bilyNaRade && score < bestScore))
                    {
                        bestScore = score;
                        MoveTo = To;
                        vybranaFigurka = board[from[0], from[1]];
                    }
                }
            }

            testScore = bestScore;
            if ((MoveTo[0] == 7 || MoveTo[0] == 0) && vybranaFigurka.Tag.ToString().ToLower() == "p")
            {
                return polozeniFigurek(bilyNaRade ? "Q" : "q", realizacePohybu.hledacSouradnic(vybranaFigurka, board), MoveTo, board);
            }

            return polozeniFigurek(vybranaFigurka.Tag.ToString(), realizacePohybu.hledacSouradnic(vybranaFigurka, board), MoveTo, board);
        }

        private double Minimax(Button[,] board, int[,] stavHracihoPole, int depth, bool bilyNaRade, double alpha, double beta)
        {
            string boardHash = GetBoardHash(board, bilyNaRade);
            if (transpositionTable.ContainsKey(boardHash))
            {
                return transpositionTable[boardHash];
            }

            if (depth == 0)
            {
                double evaluation = captureMinMax(board, stavHracihoPole, bilyNaRade, alpha, beta);
                transpositionTable[boardHash] = evaluation;
                return evaluation;
            }

            platneTahy = srovnatTahy(board, stavHracihoPole, bilyNaRade);
            if (platneTahy.Count <= 0) return bilyNaRade ? double.NegativeInfinity : double.PositiveInfinity;
            if (jeKralSach(board, !bilyNaRade, platneTahy))
            {
                depth++;
            }

            foreach (TAH move in platneTahy)
            {
                int[] From = move.From;
                int[] To = move.To;
                var originalBg = board[To[0], To[1]].BackgroundImage;
                var originalTag = board[To[0], To[1]].Tag;
                var playedPieceTag = board[From[0], From[1]].Tag.ToString();

                polozeniFigurek(playedPieceTag, From, To, board);
                int[,] tempStavPole = updateStavHracihoPole(board);

                double score = Minimax(board, tempStavPole, depth - 1, !bilyNaRade, -beta, -alpha);

                polozeniFigurek(playedPieceTag, To, From, board);
                board[To[0], To[1]].BackgroundImage = originalBg;
                board[To[0], To[1]].Tag = originalTag;

                if (score >= beta)
                {
                    return beta;
                }

                alpha = Math.Max(alpha, score);
            }
            transpositionTable[boardHash] = alpha;
            return alpha;
        }

        private double captureMinMax(Button[,] board, int[,] stavHracihoPole, bool bilyNaRade, double alpha, double beta)
        {
            double score = evaluaceFigurek.evaluate(board, bilyNaRade, stavHracihoPole);
            if (score >= beta)
            {
                return beta;
            }
            alpha = Math.Max(alpha, score);


            List<TAH> tahy = GeneraceTahuAI(stavHracihoPole, board, bilyNaRade, true);
            illitarion = tahy.Count;
            if (tahy.Count <= 0) { return alpha; }
            foreach (TAH move in tahy)
            {
                int[] From = move.From;
                int[] To = move.To;
                var originalBg = board[To[0], To[1]].BackgroundImage;
                var originalTag = board[To[0], To[1]].Tag;
                var playedPieceTag = board[From[0], From[1]].Tag.ToString();

                polozeniFigurek(playedPieceTag, From, To, board);

                score = captureMinMax(board, stavHracihoPole, !bilyNaRade, -beta, -alpha);

                polozeniFigurek(playedPieceTag, To, From, board);
                board[To[0], To[1]].BackgroundImage = originalBg;
                board[To[0], To[1]].Tag = originalTag;

                if (score >= beta)
                {
                    return beta;
                }

                alpha = Math.Max(alpha, score);
            }
            return alpha;
        }

        private string GetBoardHash(Button[,] board, bool bilyNaRade)
        {
            var sb = new System.Text.StringBuilder(65);

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    sb.Append(board[y, x].Tag?.ToString() ?? " ");
                }
            }

            sb.Append(bilyNaRade ? 'W' : 'B');
            return sb.ToString();
        }

        List<TAH> sebraniList;
        List<TAH> sach;
        List<TAH> povyseniList;
        List<TAH> ostatni;
        List<TAH> srovnatTahy(Button[,] hraciPole, int[,] stavPole, bool bilyNaRade)
        {
            List<TAH> tahy = GeneraceTahuAI(stavPole, hraciPole, bilyNaRade, false);
            if (tahy.Count <= 0) { return tahy; }

            sebraniList = new List<TAH>();
            sach = new List<TAH>();
            povyseniList = new List<TAH>();
            ostatni = new List<TAH>();

            foreach (TAH move in tahy)
            {
                int[] souradnice = move.To;
                string vybranaFigurka = hraciPole[move.From[0], move.From[1]].Tag.ToString();
                string captureFiurka = hraciPole[souradnice[0], souradnice[1]].Tag?.ToString();

                if (detekceSachu.testTahuProtiSachuMatu(stavPole, hraciPole, !bilyNaRade, hraciPole[move.From[0], move.From[1]], souradnice))
                {
                    sach.Add(move);
                }
                else if (captureFiurka != null && ((Char.IsUpper(captureFiurka[0]) && !bilyNaRade) || (Char.IsLower(captureFiurka[0]) && bilyNaRade)))
                {
                    sebraniList.Add(move);
                }
                else if ((souradnice[0] == 0 || souradnice[0] == 7) && vybranaFigurka.ToLower() == "p")
                {
                    povyseniList.Add(move);
                }
                else
                {
                    ostatni.Add(move);
                }
            }

            tahy.Clear();
            tahy.AddRange(sebraniList);
            tahy.AddRange(sach);
            tahy.AddRange(povyseniList);
            tahy.AddRange(ostatni);

            return tahy;
        }

        private List<TAH> GeneraceTahuAI(int[,] stavPole, Button[,] hraciPole, bool bilyNaRade, bool captureMoves)
        {
            List<TAH> moves = new List<TAH>();

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Button b = hraciPole[y, x];
                    if (b.Tag == null) continue;

                    string figurkaTag = b.Tag.ToString();
                    if (((bilyNaRade && char.IsUpper(figurkaTag[0])) || (!bilyNaRade && char.IsLower(figurkaTag[0]))) && !captureMoves)
                    {
                        List<int> tahy = detekceSachu.tahyProtiSachuMatu(stavPole, hraciPole, bilyNaRade, b);
                        if (tahy.Count <= 0) continue;

                        int[] souradniceFigurky = [y, x];
                        for (int i = 0; i < tahy.Count; i += 2)
                        {
                            moves.Add(new TAH(souradniceFigurky, new int[] { tahy[i], tahy[i + 1] }));
                        }
                    }

                    else if (captureMoves)
                    {
                        List<int> tahy = detekceSachu.tahyProtiSachuMatu(stavPole, hraciPole, bilyNaRade, b);
                        if (tahy.Count <= 0) continue;

                        int[] souradniceFigurky = [y, x];
                        for (int i = 0; i < tahy.Count; i += 2)
                        {
                            string figSebrat = hraciPole[tahy[i], tahy[i + 1]].Tag?.ToString();
                            if (figSebrat != null)
                            {
                                if ((!bilyNaRade && char.IsUpper(figSebrat[0])) || (bilyNaRade && char.IsLower(figSebrat[0])))
                                {
                                    moves.Add(new TAH(souradniceFigurky, new int[] { tahy[i], tahy[i + 1] }));
                                }
                            }
                        }
                    }
                }
            }

            return moves;
        }


        private bool jeKralSach(Button[,] hraciPole, bool bilyNaRade, List<TAH> moves)
        {
            int[] poziceKrale = detekceSachu.hledacPoziceKrale(hraciPole, bilyNaRade);
            if (poziceKrale == null) { return true; }
            foreach (TAH t in moves)
            {
                if (t.To[0] == poziceKrale[0] && t.To[1] == poziceKrale[1])
                {
                    return true;
                }
            }
            return false;
        }

        private Button[,] polozeniFigurek(string kodfigurky, int[] From, int[] To, Button[,] hraciPole)
        {
            hraciPole[From[0], From[1]].BackgroundImage = null;
            hraciPole[From[0], From[1]].Tag = null;
            hraciPole[To[0], To[1]].BackgroundImage = TridaFigurky.rozpoznavaniFigurek(kodfigurky);
            hraciPole[To[0], To[1]].Tag = kodfigurky;
            return hraciPole;
        }

        private int[,] updateStavHracihoPole(Button[,] hraciPole)
        {
            int[,] newStavHracihoPole = new int[8, 8];
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (hraciPole[y, x].BackgroundImage != null)
                    {
                        newStavHracihoPole[y, x] = 1;
                    }
                    else
                    {
                        newStavHracihoPole[y, x] = 0;
                    }
                }
            }
            return newStavHracihoPole;
        }

        public int[] vratitAktualniPole()
        {
            return aktualniSouradniceGlobal;
        }
        public int[] vratitMinulePole()
        {
            return minuleSouradniceGlobal;
        }
        public string vratitFigurku()
        {
            return vybranaFigurkaGlobal;
        }
        public Button vratitBtn()
        {
            return vybranaFigurkaBtnGlobal;
        }
        public string vratitSebranaFigurka()
        {
            return sebranaFigurkaGlobal;
        }
        public bool vratitPovysenibool()
        {
            return povyseni;
        }
        public int[] vratitEnpeasant()
        {
            return enpeasantGlobal;
        }

        public double vratitScore()
        {
            return testScore;
        }

        public int illitirationNum()
        {
            return illitarion;
        }
    }
}
