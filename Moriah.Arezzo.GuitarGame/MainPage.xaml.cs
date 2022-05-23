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
using System.Diagnostics;

namespace Moriah.Arezzo.GuitarGame
{

    public partial class MainPage : UserControl
    {
        Window TelaDoJogo;
        Tela Jogo;

        public MainPage()
        {
            InitializeComponent();
            this.Width = 600;
            this.Height = 355;
            this.MaxHeight = 355;
            this.MaxWidth = 600;
            this.GotFocus += MainPage_GotFocus;
            DetectarTelas();

            var app = this.Parent;

            var window = System.Windows.Application.Current.MainWindow;

            window.Height = this.Height;
            window.Width = this.Width;
            
        }

        void DetectarTelas()
        {
            DisplayManager.Displays.Clear();
            DisplayManager.LoadDisplays();
            MyDisplayList.ItemsSource = DisplayManager.Displays;
        }

        void MainPage_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TelaDoJogo != null) Jogo.Focus();
        }

        private void OpenWindow_Click(object sender, RoutedEventArgs e)
        {
            if (DisplayManager.Displays.Count > 0)
            {
                Window w = new Window();

                //w.WindowStyle = WindowStyle.None;

                w.Left = (from DisplayInfo di in DisplayManager.Displays
                          select di.MonitorArea.Left).Min();

                w.Top = (from DisplayInfo di in DisplayManager.Displays
                         select di.MonitorArea.Top).Min();

                w.Width = (from DisplayInfo di in DisplayManager.Displays select di.Width).Sum();

                w.Height = (from DisplayInfo di in DisplayManager.Displays select di.Height).Sum();



                //MediaElement video = new MediaElement();
                //video.Width = w.Width;
                //video.Height = w.Height;
                //video.Source = new Uri("http://10rem.net/pub/sl5iA/NetduinoRobot_SmallM.wmv", UriKind.Absolute);
                //video.Stretch = Stretch.UniformToFill;
                //video.AutoPlay = true;

            }
            else
            {
                MessageBox.Show("Display list not yet loaded.");
            }
        }

        private void PositionWindowOnSingleScreen(DisplayInfo screen, Window window, bool SizeToScreen = true, bool maximize = false)
        {
            window.Left = screen.MonitorArea.Left;
            window.Top = screen.MonitorArea.Top;

            if (SizeToScreen)
            {
                window.Width = screen.Width;
                window.Height = screen.Height;
            }

            if (maximize)
                window.WindowState = WindowState.Maximized;
        }

        private void AbrirJogo(object sender, RoutedEventArgs e)
        {
            
            string evento = "";            

            if (DisplayManager.Displays.Count > 0)
            {
                // find first secondary display
                DisplayInfo info = (from DisplayInfo di in DisplayManager.Displays
                                    where !di.IsPrimary
                                    select di).FirstOrDefault();

                if (TelaDoJogo != null)
                {
                    MessageBox.Show("Jogo já iniciado!", "Oooops...", MessageBoxButton.OK);
                    TelaDoJogo.Activate();
                    return;
                }
                else if (info == null)
                {
                    info = DisplayManager.Displays.FirstOrDefault();

                    //if (MessageBox.Show("Não há uma segunda tela, deseja abrir na tela principal?", "Atenção!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    //{
                    //    info = DisplayManager.Displays.FirstOrDefault();
                    //}
                    //else
                    //{
                    //    return;
                    //}
                }

                TelaDoJogo = new Window() { Title = "ZZ HUB Guitar Game" };
                TelaDoJogo.Closing += TelaDoJogo_Closing;

                TelaDoJogo.WindowStyle = WindowStyle.None;
                PositionWindowOnSingleScreen(info, TelaDoJogo, true, false);
                Jogo = new Tela(1, evento);
                Jogo.SizeChanged += Jogo_SizeChanged;
                Jogo.Width = TelaDoJogo.Width;
                Jogo.Height = TelaDoJogo.Height;
                TelaDoJogo.Content = Jogo;
                TelaDoJogo.Show();

                //Jogo.IniciarJogada();
                
                

            }
            else
            {
                MessageBox.Show("Lista de Telas não foi carregada!");
            }
        }

        void Jogo_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //((PepsiSorter)sender).Width = e.NewSize.Width;
            //((PepsiSorter)sender).Height = e.NewSize.Height;
        }

        void TelaDoJogo_Closing(object sender, System.ComponentModel.ClosingEventArgs e)
        {
            TelaDoJogo = null;
            //Jogo.FecharAplicacao();
        }

        private void MoveMainWindowToSelectedDisplay()
        {
            // see comments in blog post as to why I went with strings
            string deviceName = ((DisplayInfo)MyDisplayList.SelectedItem).MonitorName;

            DisplayInfo display = (from DisplayInfo di in DisplayManager.Displays
                                   where di.MonitorName.ToLower() == deviceName.ToLower()
                                   select di).FirstOrDefault();

            var mainWindow = Application.Current.MainWindow;

            // center on the display

            mainWindow.Left = display.MonitorArea.Left + (display.Width - mainWindow.Width) / 2;
            mainWindow.Top = display.MonitorArea.Top + (display.Height - mainWindow.Height) / 2;

        }

        private void MoveToSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyDisplayList.SelectedItem != null)
            {
                MoveMainWindowToSelectedDisplay();
            }
            else
            {
                MessageBox.Show("Por favor, selecione uma das telas na lista acima!");
            }
        }

        private void btnDetectarTelas_Click(object sender, RoutedEventArgs e)
        {
            DetectarTelas();
        }


    }
}
