using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;

namespace Moriah.Arezzo.GuitarGame
{
    public class Linhas
    {
        eStatusGame _status = eStatusGame.StandBy;

        public eStatusGame Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public float AcertosTotais
        {
            get
            {
                return linha1.Acertos +
                       linha2.Acertos +
                       linha3.Acertos +
                       linha4.Acertos +
                       linha5.Acertos +
                       linha6.Acertos;
            }
        }

        public float NotasTotais
        {
            get
            {
                return linha1.NotasIniciadas +
                       linha2.NotasIniciadas +
                       linha3.NotasIniciadas +
                       linha4.NotasIniciadas +
                       linha5.NotasIniciadas +
                       linha6.NotasIniciadas;
            }
        }

        public float PercentualAcerto
        {
            get { return AcertosTotais / NotasTotais * 100; }
        }

        List<Linha<NotaControl>> linhas = new List<Linha<NotaControl>>();

        int tempoMinimo = 100;
        int tempoMusica = 130;

        Linha<Nota1> linha1;
        Linha<Nota2> linha2;
        Linha<Nota3> linha3;
        Linha<Nota4> linha4;
        Linha<Nota5> linha5;
        Linha<Nota6> linha6;



        public Linhas(Grid gridNotas, BotaoPedal btn1, BotaoPedal btn2, BotaoPedal btn3, BotaoPedal btn4, BotaoPedal btn5, BotaoPedal btn6)
        {
            linha1 = new Linha<Nota1>(gridNotas, btn1, "Linha1");
            linha2 = new Linha<Nota2>(gridNotas, btn2, "Linha2");
            linha3 = new Linha<Nota3>(gridNotas, btn3, "Linha3");
            linha4 = new Linha<Nota4>(gridNotas, btn4, "Linha4");
            linha5 = new Linha<Nota5>(gridNotas, btn5, "Linha5");
            linha6 = new Linha<Nota6>(gridNotas, btn6, "Linha6");

        }

        public void ZerarPontuacao()
        {

            linha1.ZerarPontuacao();
            linha2.ZerarPontuacao();
            linha3.ZerarPontuacao();
            linha4.ZerarPontuacao();
            linha5.ZerarPontuacao();
            linha6.ZerarPontuacao();
        }

        public void Shot(int pedal)
        {
            switch (pedal)
            {
                case 1: linha1.Shot(); break;
                case 2: linha2.Shot(); break;
                case 3: linha3.Shot(); break;
                case 4: linha4.Shot(); break;
                case 5: linha5.Shot(); break;
                case 6: linha6.Shot(); break;
                default: break;
            }
        }

        public void Atencao()
        {
            Status = eStatusGame.Atencao;

        }

        private void PlayNota(int nota)
        {
            switch (nota)
            {
                case 1: linha1.PlayNota(); break;
                case 2: linha2.PlayNota(); break;
                case 3: linha3.PlayNota(); break;
                case 4: linha4.PlayNota(); break;
                case 5: linha5.PlayNota(); break;
                case 6: linha6.PlayNota(); break;
                default: break;
            }
        }

        public void PlayGame(decimal bpm)
        {
            lock (this)
            {
                Status = eStatusGame.Jogando;

                int tempo = Convert.ToInt16(60 / bpm * 1000);

                tempoMusica = tempo < tempoMinimo ? tempoMinimo : tempo;

                new Thread(new ThreadStart(PlayNotas)).Start();
            }
        }



        private void PlayNotas()
        {
            lock (this)
            {
                while (Status == eStatusGame.Jogando)
                {
                    PlayNota(new Random().Next(1, 7));

                    Thread.Sleep(tempoMusica);
                }
            }
        }

        public void Resultado()
        {
            Status = eStatusGame.Resultado;
        }

        public void ResultadoGuitarra()
        {
            Status = eStatusGame.ResultadoGuitarra;
        }

        public void LiberaGame()
        {
            Status = eStatusGame.StandBy;
        }





    }
}
