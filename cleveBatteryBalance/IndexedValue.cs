namespace cleveBatteryBalance
{
    struct IndexedValue
    {
        public int Value;
        public int Index;

        public IndexedValue(int index, int val)
        {
            Index = index;
            Value = val;
        }

        public override string ToString()
        {
            return Index.ToString() + ",  " + Value.ToString();
        }
    }
}
