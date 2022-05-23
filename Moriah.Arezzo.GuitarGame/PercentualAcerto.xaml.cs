using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Moriah.Arezzo.GuitarGame
{
    public partial class PercentualAcerto : UserControl
    {
        public PercentualAcerto()
        {
            InitializeComponent();
            TextosVisibility(Visibility.Collapsed);
        }


        public void Acertou(double percentual)
        {
            PercentualVisible(Visibility.Visible);
            int perc = Convert.ToInt32(percentual);
            string msg = "";

            for (int i = 0; i <= perc; i++)
            {
                txtPercentualAcerto.Dispatcher.BeginInvoke(() => txtPercentualAcerto.Text = $"{i} %");

                Thread.Sleep(15);
            }

            if (percentual < 80)
            {
                msg = "PRECISA MELHORAR A PONTARIA!";
            }
            else if (percentual >= 80 && percentual <= 90)
            {
                msg = "BOA PONTUAÇÃO!!!";
            }
            else if (percentual > 90 && percentual < 100)
            {
                msg = "UAUUU QUE SHOWWW!!";
            }
            else
            {
                msg = "****** INVENCÍVEL *******";
            }

            txtNotificacao.Dispatcher.BeginInvoke(() => txtNotificacao.Text = msg);
            ComentarioVisible(Visibility.Visible);
        }

        public void PercentualVisible(Visibility visibility)
        {
            txtPercentualAcerto.Dispatcher.BeginInvoke(() => txtPercentualAcerto.Visibility = visibility);
        }

        public void ComentarioVisible(Visibility visibility)
        {
            txtNotificacao.Dispatcher.BeginInvoke(() => txtNotificacao.Visibility = visibility);
        }

        public void TextosVisibility(Visibility visibility)
        {
            ComentarioVisible(visibility);
            PercentualVisible(visibility);
        }


    }
}
