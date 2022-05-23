using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Moriah.Arezzo.GuitarGame
{
    public enum eStatusGame
    {
        StandBy, // Jogada já terminou
        Atencao, // Jogada irá começar
        Jogando, // Jogada está acontecendo
        Resultado, // Resultado na Tela do Brinde
        ResultadoGuitarra // Resultado Guitarra (ação diferente das demais, 2º sorteio!

    }
}
