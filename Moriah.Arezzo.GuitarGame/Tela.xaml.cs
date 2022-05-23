using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.IO;
using System.Windows.Threading;
using Moriah.Arezzo.GuitarGame.Services;

namespace Moriah.Arezzo.GuitarGame
{
    public partial class Tela : UserControl
    {
        #region Variáveis

        string Evento = "";
        //GerenciadorPremio GereciadorPremio;
        MidiaManager GerenciadorMidia;
        //SequenciaPremiacao Sequencia = new SequenciaPremiacao();


        Key[] teclasControle = new Key[] { Key.D1, Key.NumPad1 };

        Key[] teclasLog = new Key[] { Key.D2, Key.NumPad2};
        Key[] teclasBtn1 = new Key[] { Key.D3, Key.A, Key.NumPad3 };
        Key[] teclasBtn2 = new Key[] { Key.D2, Key.S, Key.NumPad2 };
        Key[] teclasBtn3 = new Key[] { Key.D4, Key.D, Key.NumPad4 };
        Key[] teclasBtn4 = new Key[] { Key.D7, Key.J, Key.NumPad7 };
        Key[] teclasBtn5 = new Key[] { Key.D8, Key.K, Key.NumPad8 };
        Key[] teclasBtn6 = new Key[] { Key.D9, Key.L, Key.NumPad9 };
        Boolean btnControleTravado = false;
        BotaoPedal btn1, btn2, btn3, btn4, btn5, btn6;
        List<BotaoPedal> botoes = new List<BotaoPedal>();
        Linhas linhas;
        List<NotaControl> notas = new List<NotaControl>();
        PercentualAcerto percentual = new PercentualAcerto();
        

        //PremioDTO PremioAtual = null;
        #endregion

        #region Propriedades
        private FileStream VideoFundo { get; set; }
        #endregion

        public Tela(int tela, string evento)
        {
            Evento = evento;
            //GereciadorPremio = new GerenciadorPremio(tela, evento);
            InitializeComponent();
            GerenciadorMidia = new MidiaManager(Video);
            AddComponentes();

        }

        #region Métodos da Classe
        // Adicionar componentes de tela dinamicamente
        private void AddComponentes()
        {
            #region Botões
            btn1 = new BotaoPedal(teclasBtn1);
            btn2 = new BotaoPedal(teclasBtn2);
            btn3 = new BotaoPedal(teclasBtn3);
            btn4 = new BotaoPedal(teclasBtn4);
            btn5 = new BotaoPedal(teclasBtn5);
            btn6 = new BotaoPedal(teclasBtn6);

            GridBotoes.Children.Add(ReturnUC(btn1, 5, 125));
            GridBotoes.Children.Add(ReturnUC(btn2, 5, 265));
            GridBotoes.Children.Add(ReturnUC(btn3, 5, 410));
            GridBotoes.Children.Add(ReturnUC(btn4, 5, 555));
            GridBotoes.Children.Add(ReturnUC(btn5, 5, 700));
            GridBotoes.Children.Add(ReturnUC(btn6, 5, 840));

            botoes.AddRange(new BotaoPedal[] { btn1, btn2, btn3, btn4, btn5, btn6 });

            GridPercentual.Children.Add(percentual);

            txtControle.Visibility = Visibility.Collapsed;

            #endregion

            #region Linhas

            linhas = new Linhas(GridNotas, btn1, btn2, btn3, btn4, btn5, btn6);

            #endregion

            #region Eventos
            this.Unloaded += Tela_Unloaded;
            this.KeyDown += Botao_KeyDown;
            this.KeyUp += Botao_KeyUp;
            #endregion

        }


        #endregion

        #region Métodos para Jogadas

        void Atencao()
        {
            new Thread(new ThreadStart(_Atencao)).Start();
        }

        void _Atencao()
        {
            Video.AbaixarVolume();
            Dispatcher.BeginInvoke(() => Video.Pause());
            Dispatcher.BeginInvoke(() => linhas.Atencao());
            percentual.TextosVisibility(Visibility.Collapsed);
        }

        public void IniciarJogada()
        {
            new Thread(new ThreadStart(_IniciarJogada)).Start();
        }


        private void Controle(List<IControle> objetos)
        {
            new Thread(new ParameterizedThreadStart(_Controle)).Start(objetos); 
        }
        
        private void _Controle(object _uiItens)
        {
            List<IControle> uiItens = (List<IControle>)_uiItens;

            while (true)
            {
                foreach (var item in uiItens)
                {
                    txtControle.Dispatcher.BeginInvoke(() => txtControle.Text = item.Controle());
                }
                
                Thread.Sleep(100);
            }
        }

        void _IniciarJogada()
        {
            lock (this)
            {
                GerenciadorMidia.StopAll();
                GerenciadorMidia.NextVideo();
                linhas.ZerarPontuacao();
                Dispatcher.BeginInvoke(() => linhas.Status = eStatusGame.Jogando);

                Dispatcher.BeginInvoke(() => Video.Play());
                
                GerenciadorMidia.NextAudio();
                GerenciadorMidia.PlayAudioDaVez();

                Thread.Sleep(750); // tempo para começar a cair as notinhas (bolinhas)
                //Controle();

                Dispatcher.BeginInvoke(() => linhas.PlayGame(220)); // play com o BPM da música...

                Thread.Sleep(GerenciadorMidia.AudioDaVez.TempoJogada);

                Dispatcher.BeginInvoke(() => linhas.Status = eStatusGame.Resultado);

                Thread.Sleep(2000);


                var percentualAcerto = Math.Round(linhas.PercentualAcerto);

                if (percentualAcerto > 100) percentualAcerto = 100;

                percentual.Acertou(percentualAcerto);

                if (percentualAcerto > 99.5)
                {
                    GerenciadorMidia.TocarSirene();
                }
                else if(percentualAcerto > 80)
                {
                    GerenciadorMidia.TocarSoloGuitarra();
                }
                else
                {
                    GerenciadorMidia.TocarTenteOutraVez();
                }

               
            }
        }

        private void LiberaGame()
        {
            linhas.LiberaGame();
            foreach (var botao in botoes)
            {
                botao.Apaga();
            }

            Atencao();

        }


        //private void AnimaBotoes()
        //{

        //    Thread.Sleep(250);
        //    while (TempoSorteio)
        //    {
        //        foreach (var botao in botoes)
        //        {
        //            if (!TempoSorteio) break;
        //            Dispatcher.BeginInvoke(() => botao.AcaoLuz.Begin());
        //            Thread.Sleep(150);
        //        }

        //        for (int i = 4; i > 0 ; i--)
        //        {
        //            if (!TempoSorteio) break;
        //            Dispatcher.BeginInvoke(() => botoes[i].AcaoLuz.Begin());
        //            Thread.Sleep(150);
        //        }
        //    }

        //    if (Evento.Equals("FV22") || Evento.Equals("PA30"))
        //    {
        //        botoes[0].Acende();
        //        botoes[1].Acende();
        //    }
        //    else if (Evento.Equals("FV23") || Evento.Equals("PA31"))
        //    {
        //        botoes[2].Acende();
        //        botoes[3].Acende();
        //    }
        //    else if (Evento.Equals("FV24") )
        //    {
        //        botoes[4].Acende();
        //        botoes[5].Acende();
        //    }

        //    linhas.Resultado();
        //    Frase.Play(ePremios.Guitarra);
        //}



        #endregion

        #region Eventos

        void Botao_KeyUp(object sender, KeyEventArgs e)
        {
            if (teclasControle.Contains(e.Key)) { btnControleTravado = false; }
            if (teclasBtn1.Contains(e.Key)) { btn1.Pressionado(false); }
            if (teclasBtn2.Contains(e.Key)) { btn2.Pressionado(false); }
            if (teclasBtn3.Contains(e.Key)) { btn3.Pressionado(false); }
            if (teclasBtn4.Contains(e.Key)) { btn4.Pressionado(false); }
            if (teclasBtn5.Contains(e.Key)) { btn5.Pressionado(false); }
            if (teclasBtn6.Contains(e.Key)) { btn6.Pressionado(false); }
        }

        void Botao_KeyDown(object sender, KeyEventArgs e)
        {
            if (teclasControle.Contains(e.Key) && !btnControleTravado)
            {
                btnControleTravado = true;

                if (linhas.Status == eStatusGame.StandBy)
                {
                    IniciarJogada();
                }
                else if (linhas.Status == eStatusGame.Atencao)
                {
                    PrepararJogada();

                }
                else if (linhas.Status == eStatusGame.Resultado)
                {
                    LiberaGame();
                }
            }

            if (teclasLog.Contains(e.Key)) { txtControle.Dispatcher.BeginInvoke(() => txtControle.Visibility = txtControle.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible); }

            if (teclasBtn1.Contains(e.Key) && !btn1.Travado) { btn1.Pressionado(true); linhas.Shot(1); }
            if (teclasBtn2.Contains(e.Key) && !btn2.Travado) { btn2.Pressionado(true); linhas.Shot(2); }
            if (teclasBtn3.Contains(e.Key) && !btn3.Travado) { btn3.Pressionado(true); linhas.Shot(3); }
            if (teclasBtn4.Contains(e.Key) && !btn4.Travado) { btn4.Pressionado(true); linhas.Shot(4); }
            if (teclasBtn5.Contains(e.Key) && !btn5.Travado) { btn5.Pressionado(true); linhas.Shot(5); }
            if (teclasBtn6.Contains(e.Key) && !btn6.Travado) { btn6.Pressionado(true); linhas.Shot(6); }
        }

        void PrepararJogada()
        {
            linhas.Status = eStatusGame.StandBy;



        }

        void Tela_Unloaded(object sender, RoutedEventArgs e)
        {
            VideoFundo.Close();
            GerenciadorMidia.FecharArquivos();
        }
        #endregion

        #region Controle de Mídia


        private void Video_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            MediaElement video = (MediaElement)sender;

            if (video.CurrentState == MediaElementState.Paused && video.NaturalDuration.Equals(video.Position))
            {
                video.Position = TimeSpan.Zero;
                video.Play();
            }
        }

        #endregion

        #region Métodos de UserControl

        private T ReturnUC<T>(T controle, int Top, int Left) where T : UserControl
        {
            controle.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            controle.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            controle.Margin = new Thickness() { Top = Top, Left = Left };
            return controle;
        }

        #endregion
    }
}
