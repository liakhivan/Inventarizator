using System;

namespace InventarizatorLI.Model
{
    public class StatElement<T>
    {
        public int Id { get; set; }
        public int IdElrment { get; set; }
        public virtual T Element { get; set; }
        public string TypeEvent { get; set; }
        public double Weight { get; set; }
        public DateTime Date { get; set; }

        public StatElement()
        {

        }
    }
}
