using šachys.Properties;
using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace šachys
{
    internal class ChessKomp
    {
        detekceSachu detekceSachu = new detekceSachu();
        realizacePohybu realizacePohybu = new realizacePohybu();
        evaluaceFigurek evaluaceFigurek = new evaluaceFigurek();
        int[] minuleSouradniceGlobal;
        int[] aktualniSouradniceGlobal;
        int[] enpeasantGlobal = new int[2];
        string vybranaFigurkaGlobal;
        Button vybranaFigurkaBtnGlobal;
        string sebranaFigurkaGlobal;
        bool povyseni = false;
        public Button[,] AI(Button[,] hraciPole, int[,] stavHracihoPole, bool bilyNaRade)
        {
            povyseni = false;
            Random rand = new Random();
            bool vybirani = true;
            List<Button> platneFigurky = new List<Button>();
            foreach (var item in hraciPole)
            {
                if (item.Tag != null)
                {
                    if ((Char.IsLower(item.Tag.ToString()[0]) && !bilyNaRade) || (!Char.IsLower(item.Tag.ToString()[0]) && bilyNaRade))
                    {
                        platneFigurky.Add(item);
                    }
                }
            }

            while (vybirani && platneFigurky.Count > 0)
            {
                Button vybranaFigurka = platneFigurky[rand.Next(platneFigurky.Count)];
                List<int> platneTahy = detekceSachu.tahyProtiSachuMatu(stavHracihoPole, hraciPole, bilyNaRade, vybranaFigurka);

                if (platneTahy.Count > 1)
                {
                    List<int[]> captureMoves = new List<int[]>();
                    List<int[]> sachMoves = new List<int[]>();
                    List<int[]> otherMoves = new List<int[]>();

                    for (int i = 0; i < platneTahy.Count; i += 2)
                    {
                        int[] aktualniSouradniceTemp = { platneTahy[i], platneTahy[i + 1] };
                        if (hraciPole[aktualniSouradniceTemp[0], aktualniSouradniceTemp[1]].BackgroundImage != null)
                        {
                            captureMoves.Add(aktualniSouradniceTemp);
                        }
                        else if (detekceSachu.testTahuProtiSachuMatu(stavHracihoPole, hraciPole, !bilyNaRade, vybranaFigurka, aktualniSouradniceTemp))
                        {
                            sachMoves.Add(aktualniSouradniceTemp);
                        }
                        else
                        {
                            otherMoves.Add(aktualniSouradniceTemp);
                        }
                    }

                    int[] selectedMove;
                    if (captureMoves.Count > 0)
                    {
                        selectedMove = captureMoves[rand.Next(captureMoves.Count)];
                    }
                    else if (sachMoves.Count > 0)
                    {
                        selectedMove = sachMoves[rand.Next(captureMoves.Count)];
                    }
                    else
                    {
                        selectedMove = otherMoves[rand.Next(otherMoves.Count)];
                    }

                    int[] minuleSouradnice = realizacePohybu.hledacSouradnic(vybranaFigurka, hraciPole);
                    int[] aktualniSouradnice = selectedMove;

                    minuleSouradniceGlobal = minuleSouradnice;
                    aktualniSouradniceGlobal = aktualniSouradnice;
                    vybranaFigurkaGlobal = vybranaFigurka.Tag.ToString();
                    vybranaFigurkaBtnGlobal = hraciPole[aktualniSouradnice[0], aktualniSouradnice[1]];
                    if (hraciPole[aktualniSouradnice[0], aktualniSouradnice[1]].Tag != null)
                    {
                        sebranaFigurkaGlobal = hraciPole[aktualniSouradnice[0], aktualniSouradnice[1]].Tag.ToString();
                    }

                    hraciPole = realizacePohybu.polozeniFigurek(vybranaFigurka.Tag.ToString(), minuleSouradnice, aktualniSouradnice, hraciPole);

                    if (hraciPole[aktualniSouradnice[0], aktualniSouradnice[1]].Tag.ToString().ToLower() == "p" && (aktualniSouradnice[0] == 0 || aktualniSouradnice[0] == 7))
                    {
                        hraciPole[aktualniSouradnice[0], aktualniSouradnice[1]].Tag = bilyNaRade ? "Q" : "q";
                        hraciPole[aktualniSouradnice[0], aktualniSouradnice[1]].BackgroundImage = bilyNaRade ? šachys.Properties.Resources.QueenW : šachys.Properties.Resources.QueenB;
                        povyseni = true;
                    }
                    else if (vybranaFigurkaGlobal.ToString() == "p" && Math.Abs(aktualniSouradnice[0] - minuleSouradnice[0]) == 2)
                    {
                        enpeasantGlobal = aktualniSouradnice;
                    }

                    vybirani = false;
                }
                else
                {
                    platneFigurky.Remove(vybranaFigurka);
                }
            }

            return hraciPole;
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
    }
}
