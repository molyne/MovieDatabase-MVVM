using MovieDatabase.Model;

namespace MovieDatabase.UI.Data.Repositories
{
    public interface IMovieRepository : IGenericRepository<Movie>
    {
        void RemoveActor(Actor selectedActorModel);
    }
}