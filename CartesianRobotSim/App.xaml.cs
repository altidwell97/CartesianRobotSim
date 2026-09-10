using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using CartesianRobotSim.ViewModel;
using CartesianRobotSim.Services;
using CartesianRobotSim.Commands;

namespace CartesianRobotSim
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider? _serviceProvider;

        public App()
        {
            // Global exception handlers so startup errors are visible
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
            TaskScheduler.UnobservedTaskException += OnTaskSchedulerUnobserved;
            // App constructor called
        }

        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                Services.Logging.StartupLogger.LogException(e.Exception);
                try { MessageBox.Show("Unhandled UI exception:\n" + e.Exception.ToString(), "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Error); } catch { }
            }
            catch { }
            e.Handled = true;
        }

        private void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var ex = e.ExceptionObject as Exception;
                Services.Logging.StartupLogger.Log("Domain Unhandled Exception: " + (ex?.ToString() ?? e.ExceptionObject?.ToString()));
                try { Dispatcher?.Invoke(() => MessageBox.Show("Unhandled domain exception:\n" + (ex?.ToString() ?? e.ExceptionObject?.ToString()), "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Error)); } catch { }
            }
            catch { }
        }

        private void OnTaskSchedulerUnobserved(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            try
            {
                Services.Logging.StartupLogger.Log("UnobservedTaskException: " + e.Exception.ToString());
                try { Dispatcher?.Invoke(() => MessageBox.Show("Unobserved task exception:\n" + e.Exception.ToString(), "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Error)); } catch { }
            }
            catch { }
            e.SetObserved();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // OnStartup entered
            try
            {
                var services = new ServiceCollection();

                // ViewModels
                services.AddSingleton<RobotEnvironmentViewModel>();

                // Services
                // App cancellation service for cooperative shutdown
                services.AddSingleton<CartesianRobotSim.Services.AppCancellation.IAppCancellationService, CartesianRobotSim.Services.AppCancellation.AppCancellationService>();

                // Services - let DI construct implementations so constructor dependencies (like cancellation) are injected
                services.AddSingleton<CartesianRobotSim.Services.Move.IMoveService, CartesianRobotSim.Services.Move.MoveService>();
                // Circle service
                services.AddSingleton<CartesianRobotSim.Services.Circle.ICircleService, CartesianRobotSim.Services.Circle.CircleService>();

                // Path storage service and memorized paths store
                services.AddSingleton<CartesianRobotSim.Services.TextInteractionServices.PathStorage.IPathStorageService, CartesianRobotSim.Services.TextInteractionServices.PathStorage.PathStorageService>();
                services.AddSingleton<Model.MemorizedPaths>();
                services.AddSingleton<Stores.MemorizedPathsStore>();

                // Commands and VMs that depend on services
                services.AddSingleton<MoveToVertexCommand>();
                services.AddSingleton<CircleAxisCommand>();
                services.AddSingleton<CommandControlsViewModel>();
                services.AddSingleton<ManualControlViewModel>();
                services.AddSingleton<AddVertexCommand>();
                services.AddSingleton<AddPathCommand>();
                services.AddSingleton<RemoveVertexCommand>();
                services.AddSingleton<RemovePathCommand>();
                services.AddSingleton<PositionListEntryViewModel>();
                services.AddSingleton<MemorizedPositionsViewModel>();

                _serviceProvider = services.BuildServiceProvider();

                // Legacy JSON migration removed: txt-only path storage is enforced.

                // Ensure memorized paths are loaded before viewmodels read them
                try
                {
                    var store = _serviceProvider.GetRequiredService<Stores.MemorizedPathsStore>();

                    try
                    {
                        // Run Load on the thread-pool to avoid deadlocks from sync-blocking an async implementation
                        System.Threading.Tasks.Task.Run(async () => await store.Load()).GetAwaiter().GetResult();
                    }
                    catch (Exception ex)
                    {
                        try { Services.Logging.StartupLogger.Log("MemorizedPathsStore.Load threw exception: " + ex.ToString()); } catch { }
                        try { MessageBox.Show("Failed to load memorized paths:\n" + ex.ToString(), "Startup Error", MessageBoxButton.OK, MessageBoxImage.Warning); } catch { }
                    }
                }
                catch (Exception ex)
                {
                    try { Services.Logging.StartupLogger.Log("Failed to resolve MemorizedPathsStore: " + ex.ToString()); } catch { }
                    try { MessageBox.Show("Failed to resolve memorized paths store:\n" + ex.ToString(), "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error); } catch { }
                }

                // Expose instances to XAML as application resources for view binding
                Resources["RobotEnvironmentViewModel"] = _serviceProvider.GetRequiredService<RobotEnvironmentViewModel>();
                Resources["CommandControlsViewModel"] = _serviceProvider.GetRequiredService<CommandControlsViewModel>();
                Resources["ManualControlViewModel"] = _serviceProvider.GetRequiredService<ManualControlViewModel>();
                Resources["PositionListEntryViewModel"] = _serviceProvider.GetRequiredService<PositionListEntryViewModel>();
                Resources["MemorizedPositionsViewModel"] = _serviceProvider.GetRequiredService<MemorizedPositionsViewModel>();

                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unhandled exception in App.OnStartup: " + ex.ToString());
                try
                {
                    MessageBox.Show("Application failed to start:\n" + ex.ToString(), "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch { }
                // Ensure application exits cleanly
                try { Shutdown(); } catch { }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                // Signal application cancellation to cooperative services
                try { _serviceProvider?.GetService<CartesianRobotSim.Services.AppCancellation.IAppCancellationService>()?.Cancel(); } catch { }

                // Dispose service provider to clean up singletons that implement IDisposable
                try { (_serviceProvider as System.IDisposable)?.Dispose(); } catch { }
            }
            catch { }

            base.OnExit(e);
        }
    }

}
