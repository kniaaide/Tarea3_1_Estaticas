using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tarea3_1_Estaticas
{
    partial class MainForm
    {
        /// <summary>
        /// Construye la interfaz (equivalente al código generado por el diseñador).
        /// Visual Studio usa este archivo al abrir MainForm.cs en "Ver diseñador".
        /// </summary>
        private void InitializeComponent()
        {
            Text = "Unidad 3 · Tarea 3.1 — Pilas y Colas Estáticas";
            ClientSize = new Size(980, 640);
            MinimumSize = new Size(820, 560);
            StartPosition = FormStartPosition.CenterScreen;

            TabControl tabs = new TabControl { Dock = DockStyle.Fill };

            // ---------- PILA ESTÁTICA ----------
            tabs.TabPages.Add(new TabEstructura(
                "Pila estática (LIFO)", "Push", "Pop", "Peek",
                cap => _pila = new PilaEstatica<string>(cap),
                v => _pila.Push(v),
                () => _pila.Pop(),
                () => _pila.Peek(),
                () => _pila.IsEmpty(),
                () => _pila.IsFull(),
                () => _pila.ObtenerArreglo(),
                () => new[] { ("top", _pila.Top) },
                () => $"Count={_pila.Count}/{_pila.Capacidad}   top={_pila.Top}"));

            // ---------- COLA SIMPLE ESTÁTICA ----------
            tabs.TabPages.Add(new TabEstructura(
                "Cola simple estática (FIFO)", "Enqueue", "Dequeue", "Peek",
                cap => _colaSimple = new ColaSimpleEstatica<string>(cap),
                v => _colaSimple.Enqueue(v),
                () => _colaSimple.Dequeue(),
                () => _colaSimple.Peek(),
                () => _colaSimple.IsEmpty(),
                () => _colaSimple.IsFull(),
                () => _colaSimple.ObtenerArreglo(),
                () => _colaSimple.IsEmpty()
                        ? new (string, int)[0]
                        : new[] { ("front", _colaSimple.IndiceFront), ("rear", _colaSimple.IndiceRear) },
                () => $"Count={_colaSimple.Count}/{_colaSimple.Capacidad}   front={_colaSimple.IndiceFront}   rear={_colaSimple.IndiceRear}   celdas desperdiciadas={_colaSimple.EspacioDesperdiciado}"));

            // ---------- COLA CIRCULAR ESTÁTICA ----------
            tabs.TabPages.Add(new TabEstructura(
                "Cola circular estática", "Enqueue", "Dequeue", "Peek",
                cap => _colaCircular = new ColaCircularEstatica<string>(cap),
                v => _colaCircular.Enqueue(v),
                () => _colaCircular.Dequeue(),
                () => _colaCircular.Peek(),
                () => _colaCircular.IsEmpty(),
                () => _colaCircular.IsFull(),
                () => _colaCircular.ObtenerArreglo(),
                () => _colaCircular.IsEmpty()
                        ? new (string, int)[0]
                        : new[] { ("front", _colaCircular.IndiceFront), ("rear", _colaCircular.IndiceRear) },
                () => $"Count={_colaCircular.Count}/{_colaCircular.Capacidad}   front={_colaCircular.IndiceFront}   rear={_colaCircular.IndiceRear}"));

            Controls.Add(tabs);
        
        }
    }
}
