using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Moriah.Arezzo.GuitarGame
{
    public class Audio : IControle
    {
        public int TempoJogada { get; private set; }
        public MediaElement Midia { get; set; }
        private FileStream AudioFile { get; set; }
        public eTipoAudio TipoAudio { get; private set; }
        string _nomeAudio = "";

        public Audio(int tempoJogada, string nomeAudio, eTipoAudio tipoAudio = eTipoAudio.Jogada)
        {
            TempoJogada = tempoJogada;
            TipoAudio = tipoAudio;
            _nomeAudio = nomeAudio;
            string caminho = Path.GetDirectoryName(Application.Current.Host.Source.LocalPath) + @"\Audios\" + nomeAudio;
            AudioFile = new FileStream(caminho, FileMode.Open);
            Midia = new MediaElement();
            Midia.AutoPlay = false;
            Midia.Volume = 1;
            Midia.SetSource(AudioFile);
        }

        public void FecharArquivo()
        {
            AudioFile.Close();
        }

        public void Reset()
        {
            Midia.PositionZero();
        }

        public void Play()
        {
            Midia.PlayNow();            
        }

        public string Controle()
        {
            StringBuilder sb = new StringBuilder();

            string volume = "", position = "";
            Midia.Dispatcher.BeginInvoke(() => volume = Midia.Volume.ToString());
            Midia.Dispatcher.BeginInvoke(() => position = Midia.Position.ToString());

            sb.AppendLine("*** Áudio ***");
            sb.AppendLine($"Nome Audio: {_nomeAudio}");
            sb.AppendLine($"Volume: {volume}");
            sb.AppendLine($"Position: {position}");

            return sb.ToString();
        }

    }
}
