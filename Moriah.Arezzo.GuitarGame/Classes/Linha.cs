using System;
using System.Collections.Generic;
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
    public class Linha<T> : IControle where T : NotaControl, new()
    {
        const int quantNotas = 12;
        BotaoPedal Botao;
        Grid GridNotas;
        List<T> notas = new List<T>();
        public int NotasIniciadas { get; private set; } = 0;
        public int Acertos { get; private set; } = 0;

        public void ZerarPontuacao()
        {
            NotasIniciadas = 0;
            Acertos = 0;
        }

        public Linha(Grid gridNotas, BotaoPedal btn, string nomeLinha)
        {
            GridNotas = gridNotas;
            Botao = btn;

            for (int i = 0; i < quantNotas; i++)
            {
                T item = new T();
                notas.Add(item);
                GridNotas.Children.Add(item);
            }
        }

        public void Shot()
        {
            for (int i = 0; i < notas.Count; i++)
            {
                var item = notas[i];

                if (item.Status == eStatusNota.Iniciado)
                {
                    if (item.Shot())
                    {
                        Acertos++;
                        Botao.Fire();
                    }
                }
                if (item.Status == eStatusNota.Finalizado)
                {
                    item.Reset();
                }
            }
        }

        public void PlayNota()
        {
            for (int i = 0; i < notas.Count; i++)
            {
                var item = notas[i];

                if (item.Status == eStatusNota.Criado || item.AcaoStatus == ClockState.Stopped)
                {
                    item.Start();
                    NotasIniciadas++;
                    break;
                }
                else if (item.Status == eStatusNota.Finalizado)
                {
                    item.Reset();
                }
            }
        }

        public string Controle()
        {
            return "";
        }
    }
}
