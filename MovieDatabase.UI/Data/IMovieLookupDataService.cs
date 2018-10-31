using System.Collections.Generic;
using System.Threading.Tasks;
using MovieDatabase.Model;

namespace MovieDatabase.UI.Data
{
    public interface IMovieLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetMovieLookupAsync();
    }
}