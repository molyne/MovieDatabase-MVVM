using Autofac;
using MovieDatabase.DataAccess;
using MovieDatabase.UI.Data.Lookups;
using MovieDatabase.UI.Data.Repositories;
using MovieDatabase.UI.View.Services;
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

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<MovieDetailViewModel>().As<IMovieDetailViewModel>();

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<MovieRepository>().As<IMovieRepository>();

            return builder.Build();

        }
    }
}
