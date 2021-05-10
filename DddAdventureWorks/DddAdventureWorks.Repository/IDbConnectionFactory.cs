using System.Data;

namespace DddAdventureWorks.Repository
{
    public interface IDbConnectionFactory
    {
        IDbConnection Open();
    }
}