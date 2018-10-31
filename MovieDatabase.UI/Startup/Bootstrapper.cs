using Autofac;
using MovieDatabase.DataAccess;
using MovieDatabase.UI.Data;
using MovieDatabase.UI.ViewModel;
using Prism.Events;

namespace MovieDatabase.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<MovieDatabaseDbContext>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();

            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<MovieDetailViewModel>().As<IMovieDetailViewModel>();

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<MovieDataService>().As<IMovieDataService>();

            return builder.Build();

        }
    }
}
