using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tarea3_1_Estaticas
{
    /// <summary>
    /// Pestaña reutilizable que construye TODA la interfaz de una estructura estática
    /// (barra de controles, visor del arreglo, estado y bitácora) y se comunica con la
    /// estructura real mediante delegados. Así las tres estructuras comparten una sola UI.
    /// </summary>
    public class TabEstructura : TabPage
    {
        private readonly Action<int> _crear;
        private readonly Action<string> _insertar;
        private readonly Func<string> _extraer;
        private readonly Func<string> _consultar;
        private readonly Func<bool> _vacia;
        private readonly Func<bool> _llena;
        private readonly Func<string[]> _arreglo;
        private readonly Func<(string Etiqueta, int Indice)[]> _marcadores;
        private readonly Func<string> _info;
        private readonly string _nombreInsertar, _nombreExtraer, _nombreConsultar;

        private readonly NumericUpDown numCapacidad = new NumericUpDown { Minimum = 1, Maximum = 12, Value = 5, Width = 55 };
        private readonly TextBox txtValor = new TextBox { Width = 110 };
        private readonly Label lblEstado = new Label
        {
            Dock = DockStyle.Top, Height = 34, Padding = new Padding(8, 8, 0, 0),
            Font = new Font("Consolas", 9.5f), BackColor = Color.FromArgb(245, 245, 245)
        };
        private readonly ListBox lstBitacora = new ListBox { Dock = DockStyle.Bottom, Height = 150, IntegralHeight = false };
        private readonly VisualizadorArreglo visor = new VisualizadorArreglo { Dock = DockStyle.Fill };

        public TabEstructura(
            string titulo, string nombreInsertar, string nombreExtraer, string nombreConsultar,
            Action<int> crear, Action<string> insertar, Func<string> extraer, Func<string> consultar,
            Func<bool> vacia, Func<bool> llena, Func<string[]> arreglo,
            Func<(string Etiqueta, int Indice)[]> marcadores, Func<string> info)
        {
            Text = titulo;
            _nombreInsertar = nombreInsertar; _nombreExtraer = nombreExtraer; _nombreConsultar = nombreConsultar;
            _crear = crear; _insertar = insertar; _extraer = extraer; _consultar = consultar;
            _vacia = vacia; _llena = llena; _arreglo = arreglo; _marcadores = marcadores; _info = info;

            // ---- Barra de controles ----
            Button btnCrear = Boton("Crear / reiniciar");
            Button btnInsertar = Boton(nombreInsertar + "(valor)");
            Button btnExtraer = Boton(nombreExtraer + "()");
            Button btnConsultar = Boton(nombreConsultar + "()");
            Button btnVacia = Boton("IsEmpty()");
            Button btnLlena = Boton("IsFull()");

            FlowLayoutPanel barra = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 80, Padding = new Padding(6), WrapContents = true };
            barra.Controls.AddRange(new Control[]
            {
                Etiqueta("Capacidad:"), numCapacidad, btnCrear,
                Etiqueta("Valor:"), txtValor,
                btnInsertar, btnExtraer, btnConsultar, btnVacia, btnLlena
            });

            // ---- Eventos ----
            btnCrear.Click += (s, e) => Crear();
            btnInsertar.Click += (s, e) => Insertar();
            txtValor.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) { Insertar(); e.SuppressKeyPress = true; }
            };
            btnExtraer.Click += (s, e) => Ejecutar(() => Registrar($"{_nombreExtraer}() → \"{_extraer()}\""));
            btnConsultar.Click += (s, e) => Ejecutar(() => Registrar($"{_nombreConsultar}() → \"{_consultar()}\""));
            btnVacia.Click += (s, e) => Ejecutar(() => Registrar($"IsEmpty() → {_vacia()}"));
            btnLlena.Click += (s, e) => Ejecutar(() => Registrar($"IsFull() → {_llena()}"));

            // Orden de acople: el último en agregarse queda más arriba/afuera.
            Controls.Add(visor);
            Controls.Add(lstBitacora);
            Controls.Add(lblEstado);
            Controls.Add(barra);

            Crear();
        }

        private static Button Boton(string texto) =>
            new Button { Text = texto, AutoSize = true, Margin = new Padding(4, 3, 4, 3), Padding = new Padding(6, 2, 6, 2) };

        private static Label Etiqueta(string texto) =>
            new Label { Text = texto, AutoSize = true, Margin = new Padding(8, 9, 2, 0) };

        private void Crear()
        {
            _crear((int)numCapacidad.Value);
            Registrar($"Estructura creada con capacidad {numCapacidad.Value}.");
            Actualizar();
        }

        private void Insertar()
        {
            string valor = txtValor.Text.Trim();
            if (valor.Length == 0) { Registrar("⚠ Escribe un valor antes de insertar."); return; }

            Ejecutar(() =>
            {
                _insertar(valor);
                Registrar($"{_nombreInsertar}(\"{valor}\") ✔");
                txtValor.Clear();
            });
            txtValor.Focus();
        }

        /// <summary>Ejecuta una operación capturando los errores de desbordamiento/subdesbordamiento.</summary>
        private void Ejecutar(Action operacion)
        {
            try { operacion(); }
            catch (InvalidOperationException ex) { Registrar("⚠ " + ex.Message); }
            Actualizar();
        }

        private void Actualizar()
        {
            visor.Mostrar(_arreglo(), _marcadores());
            lblEstado.Text = $"{_info()}   |   IsEmpty={_vacia()}   IsFull={_llena()}";
        }

        private void Registrar(string mensaje) =>
            lstBitacora.Items.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {mensaje}");
    }
}
