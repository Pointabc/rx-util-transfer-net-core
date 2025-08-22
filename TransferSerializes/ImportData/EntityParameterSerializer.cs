using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using DrxTransfer;
using DrxTransfer.IntegrationServicesClient;
using DrxTransfer.Models.ImportData;
using ImportData.IntegrationServicesClient.Models.ImportData;
using Newtonsoft.Json.Linq;

namespace TransferSerializes.ImportData
{
    [Export(typeof(SungeroSerializer))]
    class EntityParameterSerializer : SungeroSerializer
    {
        public EntityParameterSerializer() : base()
        {
            this.EntityName = "EntityParameter";
            this.EntityType = "IDataImportEntityParameter";
        }

        public override void Import(object jsonObject)
        {
            var entityParameter = (jsonObject as JObject).ToObject<IDataImportEntityParameter>();
            var tmpEntityParameter = entityParameter;

            var entityParameterName = entityParameter.Name;
            var activeEntityParameter = IntegrationServiceClient
                .GetEntitiesWithFilter<IDataImportEntityParameter>(x => x.Name == entityParameterName);
            if (activeEntityParameter != null)
            {
                Logger.Info(string.Format("Справочник Соответствие заполняемых параметров сущности {0} будет обновлен.", entityParameterName));
                entityParameter = activeEntityParameter.FirstOrDefault();
            }

            var entityTypeName = entityParameter.EntityType?.Name;
            if (!string.IsNullOrEmpty(entityTypeName))
            {
                var entityType = IntegrationServiceClient
                    .GetEntitiesWithFilter<IDataImportDatabookType>(x => x.Name == entityParameter.EntityType.Name && x.EntityTypeGuid == entityParameter.EntityType.EntityTypeGuid)
                    .FirstOrDefault();
                entityParameter.EntityType = entityType;
            }

            var availableParameters = tmpEntityParameter.EntityParameterParameters;
            //entityParameter.Parameters = null;
            entityParameter.Parameters = new List<ICollectionParameter>();
            entityParameter.EntityParameterParameters = new List<ICollectionEntityParameterParameter>();
            var newEntityParameter = activeEntityParameter != null
                ? entityParameter
                : IntegrationServiceClient.CreateEntity<IDataImportEntityParameter>(entityParameter);

            if (activeEntityParameter == null)
                Logger.Info(string.Format("Создан справочник Соответствие заполняемых параметров сущности {0}", entityParameter.Name));

            CollectionHelper.CellectionItemsClear("IDataImportEntityParameter", newEntityParameter.Id.ToString(), "EntityParameterParameters");

            foreach (var parameter in availableParameters)
            {
                IDataImportDatabookType entityType = parameter.EntityType;
                if (parameter.EntityType != null)
                {
                    entityType = IntegrationServiceClient
                        .GetEntitiesWithFilter<IDataImportDatabookType>(x => x.Name == parameter.EntityType.Name && x.EntityTypeGuid == parameter.EntityType.EntityTypeGuid)
                        .FirstOrDefault();
                }

                IImportDataNavigationParameter navigationParameter = parameter.NavigationParameter;
                if (navigationParameter != null)
                {
                    navigationParameter = IntegrationServiceClient
                        .GetEntitiesWithFilter<IImportDataNavigationParameter>(x => x.Name == parameter.NavigationParameter.Name)
                        .FirstOrDefault();
                }

                IDataImportChildEntityParameter childEntityParameter = parameter.ChildEntityParameter;
                if (childEntityParameter != null)
                {
                    childEntityParameter = IntegrationServiceClient
                        .GetEntitiesWithFilter<IDataImportChildEntityParameter>(x => x.Name == parameter.ChildEntityParameter.Name)
                        .FirstOrDefault();
                }

                IntegrationServiceClient.Instance.For<IDataImportEntityParameter>()
                    .Key(newEntityParameter)
                    .NavigateTo(x => x.EntityParameterParameters)
                    .Set(
                    new
                    {
                        Id = parameter.Id,
                        PropertyName = parameter.PropertyName,
                        PropertyTypeGuid = parameter.PropertyTypeGuid,
                        LocalizedPropertyName = parameter.LocalizedPropertyName,
                        PropertyType = parameter.PropertyType,
                        IsRequired = parameter.IsRequired,
                        ExcelColumn = parameter.ExcelColumn,
                        SQLColumn = parameter.SQLColumn,
                        FillOption = parameter.FillOption,
                        EntityType = entityType,
                        DefaultValueId = parameter.DefaultValueId,
                        DefaultValueType = parameter.DefaultValueType,
                        Priority = parameter.Priority,
                        NavigationParameter = navigationParameter,
                        IsKey = parameter.IsKey,
                        DefaultValue = parameter.DefaultValue,
                        ChildEntityParameter = childEntityParameter
                    })
                    .InsertEntryAsync()
                    .Wait();
            }
        }

        protected override IEnumerable<dynamic> Export()
        {
            return IntegrationServiceClient.Instance.For<IDataImportEntityParameter>()
                .Expand(c => c.EntityType)
                .Expand(c => c.EntityParameterParameters.Select( c =>
                new
                {
                    c.Id,
                    c.PropertyName,
                    c.PropertyTypeGuid,
                    c.LocalizedPropertyName,
                    c.PropertyType,
                    c.IsRequired,
                    c.ExcelColumn,
                    c.SQLColumn,
                    c.FillOption,
                    c.EntityType,
                    c.IsKey,
                    c.DefaultValue,
                    c.NavigationParameter,
                    c.Priority,
                    c.DefaultValueId,
                    c.DefaultValueType,
                    c.ChildEntityParameter
                }
                ))
                .FindEntriesAsync()
                .Result;
        }
    }
}
