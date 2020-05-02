using System;
using System.Reflection;
using CSF.Reflection;

namespace CSF.NHibernate
{
    /// <summary>
    /// An implementation of a service which gets a reference to the assembly containing
    /// the Mono built-in SQLite ADO driver.
    /// </summary>
    public class MonoSQLiteAssemblyFinder : IGetsMonoSQLiteAssembly
    {
        readonly IDetectsMono monoDetector;

        static readonly string[] AssemblyNames = {
            "Mono.Data.Sqlite, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756",
            "Mono.Data.Sqlite, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756",
        };

        /// <summary>
        /// Gets the assembly containing the Mono SQLite ADO driver.
        /// </summary>
        /// <returns>The assembly which contains the Mono SQLite ADO driver.</returns>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoSQLiteAssemblyFinder"/> class.
        /// </summary>
        /// <param name="monoDetector">An optional service which detects the Mono runtime.  If not provided then a default detector will be used.</param>
        public MonoSQLiteAssemblyFinder(IDetectsMono monoDetector = null)
        {
            this.monoDetector = monoDetector ?? new MonoRuntimeDetector();
        }
    }
}
