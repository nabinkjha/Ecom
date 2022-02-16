using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System;
using System.Linq;

namespace ECom.API
{
    public static class EdmModelBuilder
    {

        public static IEdmModel Build(string version)
        {
            switch (version)
            {
                case "v1": return BuildV1Model();
                case "v2": return BuildV2Model();
            }

            throw new NotSupportedException($"The input version '{version}' is not supported!");
        }
        private static IEdmModel BuildV1Model()
        {
            var builder = CreateIEdmModel("ECom.Core", "ECom.Core.Entities");
            return builder.GetEdmModel();
        }
        private static IEdmModel BuildV2Model()
        {
            var builder = CreateIEdmModel("ECom.Core", "ECom.Core.Entities");// Use namespace of new version of the entity
            return builder.GetEdmModel();
        }

        private static ODataConventionModelBuilder CreateIEdmModel(string assemblyName, string entityNamespace)
        {
            var builder = new ODataConventionModelBuilder
            {
                Namespace = "ECom.OData",
                ContainerName = "EComContainer"
            };
            builder.EnableLowerCamelCase();

            foreach (Type item in GetTypesInNamespace(System.Reflection.Assembly.Load(assemblyName), entityNamespace))
            {
                if (item.GetProperty("Id") == null)
                    continue;
                EntityTypeConfiguration entityType = builder.AddEntityType(item);
                entityType.HasKey(item.GetProperty("Id"));
                builder.AddEntitySet(item.Name, entityType);//item name must be same as Controller name as it will be used as part of our URL path by OData
            }

            return builder;
        }
        private static Type[] GetTypesInNamespace(System.Reflection.Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes()
                .Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                .ToArray();
        }
    }
}