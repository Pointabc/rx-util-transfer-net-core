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
    class NavigationParameterSerializer : SungeroSerializer
    {
        public NavigationParameterSerializer() : base()
        {
            this.EntityName = "NavigationParameter";
            this.EntityType = "IImportDataNavigationParameter";
        }

        // TODO Реализовать импорт
        public override void Import(object jsonObject)
        {
            var navigationParameter = (jsonObject as JObject).ToObject<IImportDataNavigationParameter>();
            var tmpNavigationParameter = navigationParameter;

            var navigationParameterName = navigationParameter.Name;
            var activeNavigationParameter = IntegrationServiceClient
                .GetEntitiesWithFilter<IImportDataNavigationParameter>(x => x.Name == navigationParameterName);
            if (activeNavigationParameter != null)
            {
                Logger.Info(string.Format("Справочник Соответствие заполняемых параметров свойства-ссылки {0} будет обновлен.", navigationParameterName));
                navigationParameter = activeNavigationParameter.FirstOrDefault();
            }

            var entityTypeName = navigationParameter.EntityType?.Name;
            if (!string.IsNullOrEmpty(entityTypeName))
            {
                var entityType = IntegrationServiceClient
                    .GetEntitiesWithFilter<IDataImportDatabookType>(x => x.Name == navigationParameter.EntityType.Name && x.EntityTypeGuid == navigationParameter.EntityType.EntityTypeGuid)
                    .FirstOrDefault();
                navigationParameter.EntityType = entityType;
            }

            // Для теста.
            /*if (entityTypeName == "Представители")
            {
                int a = 8;
            }*/

            var availableParameters = tmpNavigationParameter.Parameters;
            navigationParameter.Parameters = null;
            var newNavigationParameter = activeNavigationParameter != null 
                ? navigationParameter
                : IntegrationServiceClient.CreateEntity<IImportDataNavigationParameter>(navigationParameter);
            
            if (activeNavigationParameter == null)
                Logger.Info(string.Format("Создан Тип сущности {0}", navigationParameter.Name));

            CollectionHelper.CellectionItemsClear("IImportDataNavigationParameter", newNavigationParameter.Id.ToString(), "Parameters");

            // TODO Понять как заполнять коллекцию
            foreach (var parameter in availableParameters)
            {
                IDataImportDatabookType entityType = parameter.EntityType;
                if (parameter.EntityType != null)
                {
                    entityType = IntegrationServiceClient
                        .GetEntitiesWithFilter<IDataImportDatabookType>(x => x.Name == parameter.EntityType.Name && x.EntityTypeGuid == parameter.EntityType.EntityTypeGuid)
                        .FirstOrDefault();
                }

                IntegrationServiceClient.Instance.For<IImportDataNavigationParameter>()
                    .Key(newNavigationParameter)
                    .NavigateTo(x => x.Parameters)
                    .Set(
                    new { 
                        Id = parameter.Id,
                        PropertyName =  parameter.PropertyName,
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
                        NavigationParameter = parameter.NavigationParameter,
                        IsKey = parameter.IsKey,
                        DefaultValue = parameter.DefaultValue
                    })
                    .InsertEntryAsync()
                    .Wait();
            }
        }

        protected override IEnumerable<dynamic> Export()
        {
            return IntegrationServiceClient.Instance.For<IImportDataNavigationParameter>()
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
