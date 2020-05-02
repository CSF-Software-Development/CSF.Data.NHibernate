using System.Reflection;

namespace CSF.NHibernate
{
    /// <summary>
    /// An object which can get the assembly which contains the Mono built-in SQLite ADO driver.
    /// </summary>
    public interface IGetsMonoSQLiteAssembly
    {
        /// <summary>
        /// Gets the assembly containing the Mono SQLite ADO driver.
        /// </summary>
        /// <returns>The assembly which contains the Mono SQLite ADO driver.</returns>
        Assembly GetMonoSQLiteAssembly();
    }
}
