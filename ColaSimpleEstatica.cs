using System;

namespace Tarea3_1_Estaticas
{
    /// <summary>
    /// COLA SIMPLE ESTÁTICA (FIFO: First In, First Out).
    /// Arreglo de tamaño fijo con dos índices: front (salida) y rear (entrada).
    ///
    /// Convención: front = 0 y rear = -1 → cola vacía (front &gt; rear).
    ///             rear = Capacidad - 1   → cola "llena" (aunque queden huecos al inicio).
    ///
    /// LIMITACIÓN DIDÁCTICA: al hacer Dequeue, front avanza y las posiciones anteriores
    /// quedan inutilizables hasta que la cola se vacía por completo (entonces se reinician
    /// los índices). Esto se mide con la propiedad EspacioDesperdiciado y es la razón de
    /// ser de la COLA CIRCULAR.
    /// Complejidad: todas las operaciones son O(1).
    /// </summary>
    public class ColaSimpleEstatica<T>
    {
        private readonly T[] _datos;
        private int _front;
        private int _rear;

        public int Capacidad => _datos.Length;
        public int IndiceFront => _front;
        public int IndiceRear => _rear;
        public int Count => IsEmpty() ? 0 : _rear - _front + 1;

        /// <summary>Celdas que quedaron antes de front y no se pueden reutilizar.</summary>
        public int EspacioDesperdiciado => IsEmpty() ? 0 : _front;

        public ColaSimpleEstatica(int capacidad)
        {
            if (capacidad <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacidad), "La capacidad debe ser mayor que 0.");
            _datos = new T[capacidad];
            _front = 0;
            _rear = -1;
        }

        public bool IsEmpty() => _front > _rear;

        public bool IsFull() => _rear == _datos.Length - 1;

        /// <summary>Agrega un elemento al final de la cola.</summary>
        public void Enqueue(T elemento)
        {
            if (IsFull())
            {
                string detalle = _front > 0
                    ? $" (hay {_front} celda(s) libres al inicio que una cola simple no puede reutilizar)"
                    : string.Empty;
                throw new InvalidOperationException("Cola llena: rear llegó al último índice" + detalle + ".");
            }
            _rear++;
            _datos[_rear] = elemento;
        }

        /// <summary>Quita y devuelve el elemento del frente.</summary>
        public T Dequeue()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Subdesbordamiento (underflow): la cola está vacía.");
            T valor = _datos[_front];
            _datos[_front] = default(T);
            _front++;

            // Si ya no quedan elementos, se reinician los índices para recuperar toda la capacidad.
            if (_front > _rear)
            {
                _front = 0;
                _rear = -1;
            }
            return valor;
        }

        /// <summary>Devuelve el elemento del frente sin eliminarlo.</summary>
        public T Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException("La cola está vacía: no hay elemento al frente.");
            return _datos[_front];
        }

        public T[] ObtenerArreglo()
        {
            T[] copia = new T[_datos.Length];
            for (int i = 0; i < _datos.Length; i++)
                copia[i] = _datos[i];
            return copia;
        }
    }
}
