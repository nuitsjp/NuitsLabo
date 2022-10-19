using System.Data;

public class CountableSqlDataReader : IDataReader, IObservable<int>
{
    private readonly IDataReader _dataReader;
    private int _rowNumber;
    private IObserver<int> _observer = new NullObserver();

    public CountableSqlDataReader(IDataReader dataReader)
    {
        _dataReader = dataReader;
    }

    public bool GetBoolean(int i) => _dataReader.GetBoolean(i);

    public byte GetByte(int i)
    {
        throw new NotImplementedException();
    }

    public long GetBytes(int i, long fieldOffset, byte[]? buffer, int bufferoffset, int length)
    {
        throw new NotImplementedException();
    }

    public char GetChar(int i)
    {
        throw new NotImplementedException();
    }

    public long GetChars(int i, long fieldoffset, char[]? buffer, int bufferoffset, int length)
    {
        throw new NotImplementedException();
    }

    public IDataReader GetData(int i)
    {
        throw new NotImplementedException();
    }

    public string GetDataTypeName(int i)
    {
        throw new NotImplementedException();
    }

    public DateTime GetDateTime(int i)
    {
        throw new NotImplementedException();
    }

    public decimal GetDecimal(int i)
    {
        throw new NotImplementedException();
    }

    public double GetDouble(int i)
    {
        throw new NotImplementedException();
    }

    public Type GetFieldType(int i)
    {
        throw new NotImplementedException();
    }

    public float GetFloat(int i)
    {
        throw new NotImplementedException();
    }

    public Guid GetGuid(int i)
    {
        throw new NotImplementedException();
    }

    public short GetInt16(int i)
    {
        throw new NotImplementedException();
    }

    public int GetInt32(int i)
    {
        throw new NotImplementedException();
    }

    public long GetInt64(int i)
    {
        throw new NotImplementedException();
    }

    public string GetName(int i)
    {
        throw new NotImplementedException();
    }

    public int GetOrdinal(string name)
    {
        throw new NotImplementedException();
    }

    public string GetString(int i) => _dataReader.GetString(i);

    public object GetValue(int i) => _dataReader.GetValue(i);

    public int GetValues(object[] values)
    {
        throw new NotImplementedException();
    }

    public bool IsDBNull(int i)
    {
        throw new NotImplementedException();
    }

    public int FieldCount => _dataReader.FieldCount;

    public object this[int i] => throw new NotImplementedException();

    public object this[string name] => throw new NotImplementedException();

    public void Dispose() => _dataReader.Dispose();

    public void Close() => _dataReader.Close();

    public DataTable? GetSchemaTable() => _dataReader.GetSchemaTable();

    public bool NextResult() => _dataReader.NextResult();

    public bool Read()
    {
        try
        {
            if (_dataReader.Read())
            {
                _observer.OnNext(++_rowNumber);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            _observer.OnError(e);
            throw;
        }
    }

    public int Depth => _dataReader.Depth;
    public bool IsClosed => _dataReader.IsClosed;
    public int RecordsAffected => _dataReader.RecordsAffected;
    public IDisposable Subscribe(IObserver<int> observer)
    {
        _observer = observer;
        return this;
    }

    private class NullObserver : IObserver<int>
    {
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(int value)
        {
        }
    }
}