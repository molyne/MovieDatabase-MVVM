using MovieDatabase.DataAccess;
using MovieDatabase.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.UI.Data.Lookups
{
    public class LookupDataService : IMovieLookupDataService, IGenreLookupDataService
    {
        private readonly MovieDatabaseDbContext _contextCreator;

        public LookupDataService(MovieDatabaseDbContext contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetMovieLookupAsync()
        {
            using (var context = _contextCreator)
            {
                return await context.Movies.AsNoTracking()
                      .Select(movie =>
                          new LookupItem
                          {
                              Id = movie.Id,
                              DisplayMember = movie.Title
                          })
                      .ToListAsync();
            }

        }
        public async Task<IEnumerable<LookupItem>> GetGenreLookupAsync()
        {
            using (var context = _contextCreator)
            {
                return await context.Genres.AsNoTracking()
                    .Select(genre =>
                       new LookupItem
                       {
                           Id = genre.Id,
                           DisplayMember = genre.Name
                       })
                    .ToListAsync();
            }
        }
    }
}
