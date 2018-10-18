using Autofac;
using StatSnapShotter.Interfaces;
using Serilog;

namespace StatSnapShotter
{
    public class Program
    {     
        static void Main(string[] args)
        {
            string environment = "";
            if (args.Length != 0)
            {
                if (args[0] != null)
                {
                    environment = args[0];
                }
            }

            var container = ContainerConfig.Configure(environment);

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IApplication>();
                var totalWrites = app.Run();
                Log.Information("SeqSnapShot ended.  {writes}", totalWrites);
            }
        }
    }   
}