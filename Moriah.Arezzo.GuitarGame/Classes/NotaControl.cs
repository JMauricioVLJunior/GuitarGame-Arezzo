using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Moriah.Arezzo.GuitarGame
{
    public class NotaControl : UserControl
    {
        string posicao = "";
        eStatusNota status = eStatusNota.Criado;
        Storyboard acao;

        ClockState acaoStatus;

        public ClockState AcaoStatus
        {
            get {
                Dispatcher.BeginInvoke(() => acaoStatus = Acao.GetCurrentState());    
                return acaoStatus;             
            }
            set { acaoStatus = value; }
        }

        public Storyboard Acao
        {
            get { return acao; }
            private set { acao = value; }
        }

        Grid GridObj;
        Boolean Acertou = false;

        public eStatusNota Status { get { return status; } }

        public NotaControl() : base()
        {
            Dispatcher.BeginInvoke(() => GridObj.Visibility = System.Windows.Visibility.Collapsed);
        }

        public void Config(Storyboard story, Grid gridObj)
        {
            Acao = story;
            GridObj = gridObj;
            Acao.Completed += acao_Completed;  
        }

        void acao_Completed(object sender, EventArgs e)
        {
            if (!Acertou)
            {
                status = eStatusNota.Finalizado;
                Reset();
            }

        }

        public void Start()
        {
            status = eStatusNota.Iniciado;
            Dispatcher.BeginInvoke(() => GridObj.Visibility = System.Windows.Visibility.Visible);
            Dispatcher.BeginInvoke(() => Acao.Begin());
            Dispatcher.BeginInvoke(() => GridObj.Visibility = System.Windows.Visibility.Visible);            
        }

        public bool Shot()
        {
            TimeSpan time = Acao.GetCurrentTime();

            if (time > new TimeSpan(0, 0, 0, 1, 280) && time < new TimeSpan(0, 0, 0, 1, 600))
            {
                Acertou = true;
                Reset();
                return true;
            }

            return false;
        }

        public void Reset()
        {
            Dispatcher.BeginInvoke(() => Acao.Stop());            
            status = eStatusNota.Criado;
            Acertou = false;
            Dispatcher.BeginInvoke(() => GridObj.Visibility = System.Windows.Visibility.Collapsed);
        }

        public string Controle()
        {
            return "";
        }
    }
}
