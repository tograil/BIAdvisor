using System.Data;

namespace BIAdvisor.BL
{
    public interface IUserMethods
    {
        DataRow GetUser(string username);
    }
}