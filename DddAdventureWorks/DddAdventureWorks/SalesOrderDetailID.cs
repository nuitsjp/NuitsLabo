namespace DddAdventureWorks
{
    public struct SalesOrderDetailID
    {
        public SalesOrderDetailID(int value)
        {
            Value = value;
        }

        internal int Value { get; }
    }
}