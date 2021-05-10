namespace DddAdventureWorks
{
    public readonly struct SalesOrderID
    {
        public SalesOrderID(int value)
        {
            Value = value;
        }

        internal int Value { get; }
    }
}