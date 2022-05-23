using System.Threading;

namespace System.Windows.Controls
{
    public static class MidiaExtensions
    {
        #region Controls


        public static void PlayNow(this MediaElement midia)
        {
            lock (midia)
            {
                midia.VolumeFull();
                midia.PositionZero();
                Thread.Sleep(10);
                midia.Dispatcher.BeginInvoke(() => midia.Play());
            }
        }

        public static void VolumeFull(this MediaElement midia)
        {
            lock(midia)
            {
                midia.Dispatcher.BeginInvoke(() => midia.Volume = 1);
            }
        }

        public static void StopNow(this MediaElement midia)
        {
            lock (midia)
            {
                PositionZero(midia);
                midia.Dispatcher.BeginInvoke(() => midia.Stop());
            }
        }

        public static void PositionZero(this MediaElement midia)
        {
            lock (midia)
            {
                midia.Dispatcher.BeginInvoke(() => midia.Position = TimeSpan.Zero);
            }
        }

        #endregion

        #region Abaixar Volume

        public static void AbaixarVolume(this MediaElement _midia)
        {
            new Thread(new ParameterizedThreadStart(_AbaixarVolume)).Start(_midia);
        }

        private static void _AbaixarVolume(Object _midia)
        {

            MediaElement midia = (MediaElement)_midia;

            double volumeAtual = 2;
            midia.Dispatcher.BeginInvoke(() => volumeAtual = midia.Volume * 10);

            while (volumeAtual == 2)
            {
                Thread.Sleep(10);
            }

            for (double i = volumeAtual; i >= 0; i--)
            {
                double volume = i / 10;
                midia.Dispatcher.BeginInvoke(() => midia.Volume = volume);
                Thread.Sleep(250);
            }

        }

        #endregion

        #region Aumentar Volume

        public static void AumentarVolume(this MediaElement _midia)
        {
            new Thread(new ParameterizedThreadStart(_AumentarVolume)).Start(_midia);
        }

        private static void _AumentarVolume(Object _midia)
        {

            MediaElement midia = (MediaElement)_midia;

            double volumeAtual = 2;
            midia.Dispatcher.BeginInvoke(() => volumeAtual = midia.Volume * 10);

            while (volumeAtual == 2)
            {
                Thread.Sleep(10);
            }

            for (double i = volumeAtual; i <= 10; i++)
            {
                double volume = i / 10;
                midia.Dispatcher.BeginInvoke(() => midia.Volume = volume);
                Thread.Sleep(250);
            }

        }

        #endregion

    }
}
