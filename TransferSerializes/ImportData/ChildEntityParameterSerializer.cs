using DrxTransfer;
using DrxTransfer.Models.ImportData;
using ImportData.IntegrationServicesClient.Models.ImportData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

namespace TransferSerializes.ImportData
{
    [Export(typeof(SungeroSerializer))]
    class ChildEntityParameterSerializer : SungeroSerializer
    {
        public ChildEntityParameterSerializer() : base()
        {
            this.EntityName = "ChildEntityParameter";
            this.EntityType = "IDataImportChildEntityParameter";
        }

        public override void Import(object jsonObject)
        {
            var childEntityParameter = (jsonObject as JObject).ToObject<IDataImportChildEntityParameter>();
            var tmpChildEntityParameter = childEntityParameter;

            var childEntityParameterName = childEntityParameter.Name;
            var activeChildEntityParameter = IntegrationServiceClient
                .GetEntitiesWithFilter<IDataImportChildEntityParameter>(x => x.Name == childEntityParameterName);
            if (activeChildEntityParameter != null)
            {
                Logger.Info(string.Format("Справочник Соответствие заполняемых параметров строки свойства-коллекции {0} будет обновлен.", childEntityParameterName));
                childEntityParameter = activeChildEntityParameter.FirstOrDefault();
            }

            var entityTypeName = childEntityParameter.EntityType?.Name;
            if (!string.IsNullOrEmpty(entityTypeName))
            {
                var entityType = IntegrationServiceClient
                    .GetEntitiesWithFilter<IDataImportDatabookType>(x => x.Name == childEntityParameter.EntityType.Name && x.EntityTypeGuid == childEntityParameter.EntityType.EntityTypeGuid)
                    .FirstOrDefault();
                childEntityParameter.EntityType = entityType;
            }

            var availableParameters = tmpChildEntityParameter.Parameters;
            childEntityParameter.Parameters = null;
            var newChildEntityParameter = activeChildEntityParameter != null
                ? childEntityParameter
                : IntegrationServiceClient.CreateEntity<IDataImportChildEntityParameter>(childEntityParameter);

            if (activeChildEntityParameter == null)
                Logger.Info(string.Format("Создан справочник Соответствие заполняемых параметров строки свойства-коллекции {0}", childEntityParameter.Name));

            CollectionHelper.CellectionItemsClear("IDataImportChildEntityParameter", newChildEntityParameter.Id.ToString(), "Parameters");

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

                IntegrationServiceClient.Instance.For<IDataImportChildEntityParameter>()
                    .Key(newChildEntityParameter)
                    .NavigateTo(x => x.Parameters)
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
                        DefaultValue = parameter.DefaultValue
                    })
                    .InsertEntryAsync()
                    .Wait();
            }
        }

        protected override IEnumerable<dynamic> Export()
        {
            return IntegrationServiceClient.Instance.For<IDataImportChildEntityParameter>()
                .Expand(c => c.EntityType)
                .Expand(c => c.Parameters.Select(c =>
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
                    c.DefaultValueType
                }
                ))
                .FindEntriesAsync()
                .Result;
        }
    }
}
