using System;
using System.Reflection;
using CSF.Reflection;

namespace CSF.NHibernate
{
    public class MonoSQLiteAssemblyFinder : IGetsMonoSQLiteAssembly
    {
        readonly IDetectsMono monoDetector;

        static readonly string[] AssemblyNames = {
            "Mono.Data.Sqlite, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756",
            "Mono.Data.Sqlite, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756",
        };

        public Assembly GetMonoSQLiteAssembly()
        {
            if (!monoDetector.IsExecutingWithMono())
                throw new InvalidOperationException($"Invalid usage of {nameof(GetMonoSQLiteAssembly)}.  This method must not be used when not executing with the Mono runtime.");

            foreach (var name in AssemblyNames)
            {
                try
                {
                    var assembly = Assembly.Load(name);
                    if (assembly != null) return assembly;
                }
                catch (Exception) { /* Intentional no-op, if the assembly can't be loaded, continue the loop */ }
            }

            throw new InvalidOperationException("Cannot load the Mono SQLite assembly 'Mono.Data.Sqlite'.");
        }

        public MonoSQLiteAssemblyFinder(IDetectsMono monoDetector = null)
        {
            monoDetector = monoDetector ?? new MonoRuntimeDetector();
        }
    }
}
