using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace cleveBatteryBalance
{
    class BatteryCell : List<IndexedValue>
    {
        public IndexedValue GetSmallestValueIndex
        {
            get
            {
                var smallestValue = int.MaxValue;
                var currentIndex = -1;
                for (int i = 0; i < base.Count; i++)
                {
                    var currentVal = this[i].Value;
                    if (currentVal < smallestValue)
                    {
                        smallestValue = currentVal;
                        currentIndex = this[i].Index;
                    }
                }

                return new IndexedValue(currentIndex, smallestValue);
            }
        }

        public IndexedValue GetLargestValueIndex
        {
            get
            {
                var largestValue = -1;
                var currentIndex = -1;
                for (int i = 0; i < base.Count; i++)
                {
                    var currentVal = this[i].Value;
                    if (currentVal > largestValue)
                    {
                        largestValue = currentVal;
                        currentIndex = this[i].Index;
                    }
                }

                return new IndexedValue(currentIndex, largestValue);
            }
        }

        public int TotalValue { get; set; } //should encapsulate the value should only be allowed to be set internally
        
        public BatteryCell()
        {
            TotalValue = 0;
        }

       public new void Add(IndexedValue item)
        {
            base.Add(item);

            TotalValue += item.Value;
        }
        public new void Add(int index, string item)
        {
            var intVal = 0;
            if (!int.TryParse(item, out intVal)) throw new ArgumentException("values must be able to be converted to int values");

            base.Add(new IndexedValue(index, intVal));

            TotalValue += intVal;
        }

        public bool RemoveWhereIndex(int index)
        {
            foreach (var item in this)
            {
                if(item.Index == index)
                {
                    base.Remove(item);
                    return true;
                }
            }

            return false;
        }

        public IndexedValue GetWhereIndex(int index)
        {
            foreach (var item in this)
            {
                if (item.Index == index)
                {
                    return item;
                }
            }

            throw new ArgumentOutOfRangeException("Index value is not found");
        }

        public new void Remove(IndexedValue item)
        {
            base.Remove(item);

            TotalValue -= item.Value;
        }

        public new void RemoveAt(int index)
        {
            var value = base[index];
            base.RemoveAt(index);

            TotalValue -= value.Value;
        }
    }
}
