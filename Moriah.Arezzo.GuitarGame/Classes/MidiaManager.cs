using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows;
using System.IO;

namespace Moriah.Arezzo.GuitarGame
{
    public class MidiaManager
    {
        List<string> pathVideos = new List<string> { @"\Midia\videoZZ.mov", @"\Midia\videoZZ2.mp4", @"\Midia\videoZZ3.mp4" };

        public Audio AudioDaVez { get; private set; }

        private MediaElement VideoElement { get; set; }

        int indiceAudio = 0;
        int indiceVideo = 0;
        
        Audio Audio1 = new Audio(28000, "Audio1.mp3", eTipoAudio.Jogada); //30000
        Audio Audio2 = new Audio(26500, "Audio2.mp3", eTipoAudio.Jogada); //28000
        Audio AudioSirene = new Audio(21000, "Sirene.mp3", eTipoAudio.Sirene); // Quando acertar 100%
        Audio AudioTenteOutraVez = new Audio(6000, "TenteOutraVez.mp3", eTipoAudio.TenteOutraVez); // Quando acertar menos de 50% 
        Audio AudioSoloGuitarra = new Audio(11500, "SorteioGuitarra.mp3"); // Quando acertar mais de 90%
        List<Audio> Audios = new List<Audio>();


        public void TocarSirene()
        {
            AudioSirene.Play();
        }

        public void TocarSoloGuitarra()
        {
            AudioSoloGuitarra.Play();
        }

        public void TocarTenteOutraVez()
        {
            AudioTenteOutraVez.Play();
        }

        public MidiaManager(MediaElement videoElement)
        {
            VideoElement = videoElement;
            Audios.Add(Audio1);
            Audios.Add(Audio2);
            Audios.Add(AudioSirene);
            Audios.Add(AudioTenteOutraVez);
            Audios.Add(AudioSoloGuitarra);
            ConfigurarVideo(0);
        }

        public void NextVideo()
        {
            int totalVideos = pathVideos.Count;

            if (indiceVideo == totalVideos - 1)
                indiceVideo = 0;
            else
                indiceVideo = indiceVideo + 1;


            ConfigurarVideo(indiceVideo);
        }

        private void ConfigurarVideo(int position)
        {
            //string path = System.IO.Path.GetDirectoryName(Application.Current.Host.Source.LocalPath) + pathVideos[position];
            string path = @"D:\Moriah - Clientes\Arezzo\Jogo\Moriah.Arezzo.GuitarGame\Bin\Debug" + pathVideos[position];
            var VideoFundo = new FileStream(path, FileMode.Open);
            VideoElement.Dispatcher.BeginInvoke( () => VideoElement.SetSource(VideoFundo));
            VideoElement.StopNow();
            VideoElement.PlayNow();
            
        }

        public void PlayAudioDaVez()
        {
            AudioDaVez.Play();
        }

        public void ResetAll()
        {
            lock (this)
            {
                foreach (var item in Audios)
                {
                    item.Reset();
                }
            }
        }

        public List<Audio> GetAudio()
        {
            return Audios.Where(x => x.TipoAudio == eTipoAudio.Jogada).ToList();
        }

        public void GetRandomAudio()
        {
            var audio = new Random().Next(0, 1) == 0 ? Audio1 : Audio2 ;

            audio.Midia.Dispatcher.BeginInvoke( () => audio.Midia.Position = TimeSpan.Zero);
            audio.Midia.Dispatcher.BeginInvoke( () => audio.Midia.Volume = 1);

            AudioDaVez = audio;
        }

        public void NextAudio()
        {
            var audio = indiceAudio == 0 ? Audio2 : Audio1;

            indiceAudio++;

            if (indiceAudio == 2) indiceAudio = 0;

            AudioDaVez = audio;
        }


        public void FecharArquivos()
        {
            foreach (var file in Audios)
                file.FecharArquivo();       
        }

        public void StopAll()
        {
            foreach (var audio in Audios)
                audio.Midia.StopNow();
        }
    }
}
