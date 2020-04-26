using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Cfg;
using CSF.Specifications;
using CSF.Reflection;
using NHibernate.Mapping.ByCode;

namespace CSF.NHibernate
{
    public class AssemblyMappingsProvider : IAddsMappings
    {
        readonly Assembly assembly;

        public void AddMappings(Configuration config)
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(GetMappingTypes());
            var mappings = mapper.CompileMappingForAllExplicitlyAddedEntities();
            config.AddMapping(mappings);
        }

        IEnumerable<Type> GetMappingTypes()
        {
            return assembly.GetExportedTypes()
                .Where(new IsConcreteClassSpecification())
                .Where(x => x.Namespace.EndsWith("Mappings", StringComparison.InvariantCulture))
                .ToList();
        }

        public AssemblyMappingsProvider(Assembly assembly)
        {
            this.assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }
    }
}
