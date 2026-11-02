using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tarea3_1_Estaticas
{
    /// <summary>
    /// Formulario principal: un TabControl con las tres estructuras estáticas.
    /// Cada pestaña se enlaza a su estructura mediante lambdas.
    /// </summary>
    public partial class MainForm : Form
    {
        private PilaEstatica<string> _pila = new PilaEstatica<string>(5);
        private ColaSimpleEstatica<string> _colaSimple = new ColaSimpleEstatica<string>(5);
        private ColaCircularEstatica<string> _colaCircular = new ColaCircularEstatica<string>(5);

        public MainForm()
        {
            InitializeComponent();
        }
    }
}
