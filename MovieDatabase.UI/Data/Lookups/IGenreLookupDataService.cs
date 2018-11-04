using MovieDatabase.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieDatabase.UI.Data.Lookups
{
    public interface IGenreLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetGenreLookupAsync();
    }
}