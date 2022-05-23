using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Linq;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Moriah.Arezzo.GuitarGame
{
	public partial class BotaoPedal : UserControl
	{
		Key[] _tecla;
        Boolean travado = false;

        public Boolean Travado
        {
            get { return travado; }
            private set { travado = value; }
        }
        
		
		public BotaoPedal(Key[] tecla)
		{
			// Required to initialize variables
			InitializeComponent();
			_tecla = tecla;
            imgLuz.Visibility = System.Windows.Visibility.Collapsed;
            
		}

        public void Pressionado(bool pressionado)
        {
            travado = pressionado;
            ImgBtnPepsi.Visibility = pressionado ? Visibility.Visible : Visibility.Collapsed;
        }

        public void Fire()
        {
            imgLuz.Visibility = System.Windows.Visibility.Visible;
            Dispatcher.BeginInvoke(() => AcaoLuz.Begin());
        }

        public void Acende()
        {
            Dispatcher.BeginInvoke(() => imgLuzSorteado.Visibility = System.Windows.Visibility.Visible);
        }

        public void Apaga()
        {
            Dispatcher.BeginInvoke(() => imgLuzSorteado.Visibility = System.Windows.Visibility.Collapsed);
        }


	}
}