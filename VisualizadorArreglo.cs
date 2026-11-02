using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Tarea3_1_Estaticas
{
    /// <summary>
    /// Control propio que dibuja un arreglo como una fila de celdas indexadas y
    /// coloca flechas (▼) con los nombres de los punteros (top, front, rear).
    /// </summary>
    public class VisualizadorArreglo : Panel
    {
        private string[] _slots = new string[0];
        private (string Etiqueta, int Indice)[] _marcadores = new (string, int)[0];

        public VisualizadorArreglo()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Color.White;
        }

        public void Mostrar(string[] slots, (string Etiqueta, int Indice)[] marcadores)
        {
            _slots = slots ?? new string[0];
            _marcadores = marcadores ?? new (string, int)[0];
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int n = _slots.Length;
            if (n == 0) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int margen = 20;
            int ancho = Math.Max(24, Math.Min(80, (Width - 2 * margen) / n));
            int alto = 52;
            int y = Height / 2 - alto / 2 + 10;

            using (Font fuente = new Font("Segoe UI", 10f))
            using (Font negrita = new Font("Segoe UI", 9f, FontStyle.Bold))
            using (StringFormat centro = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                // 1) Celdas del arreglo
                for (int i = 0; i < n; i++)
                {
                    Rectangle celda = new Rectangle(margen + i * ancho, y, ancho, alto);
                    bool ocupada = _slots[i] != null;

                    using (SolidBrush relleno = new SolidBrush(ocupada ? Color.FromArgb(173, 216, 230) : Color.WhiteSmoke))
                        g.FillRectangle(relleno, celda);
                    g.DrawRectangle(Pens.Black, celda);

                    if (ocupada)
                        g.DrawString(_slots[i], fuente, Brushes.Black, celda, centro);

                    Rectangle rotuloIndice = new Rectangle(celda.X, celda.Bottom + 4, celda.Width, 20);
                    g.DrawString(i.ToString(), fuente, Brushes.Gray, rotuloIndice, centro);
                }

                // 2) Punteros (si dos coinciden en la misma celda se unen: "front/rear")
                Dictionary<int, string> grupos = new Dictionary<int, string>();
                foreach (var m in _marcadores)
                {
                    if (m.Indice < 0 || m.Indice >= n) continue;
                    grupos[m.Indice] = grupos.ContainsKey(m.Indice) ? grupos[m.Indice] + "/" + m.Etiqueta : m.Etiqueta;
                }
                foreach (KeyValuePair<int, string> par in grupos)
                {
                    int x = margen + par.Key * ancho;
                    g.DrawString(par.Value, negrita, Brushes.Firebrick, new Rectangle(x - 10, y - 48, ancho + 20, 20), centro);
                    g.DrawString("▼", negrita, Brushes.Firebrick, new Rectangle(x, y - 28, ancho, 20), centro);
                }
            }
        }
    }
}
