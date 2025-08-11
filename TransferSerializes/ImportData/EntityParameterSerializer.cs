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

        // TODO Реализовать импорт
        public override void Import(object jsonObject)
        {
            /*var entityType = (jsonObject as JObject).ToObject<IDataImportDatabookType>();

            var entityTypeName = entityType.Name;
            var activeEntityType = IntegrationServiceClient.GetEntitiesWithFilter<IDataImportDatabookType>(r => r.Name == entityTypeName);
            if (activeEntityType != null)
            {
                Logger.Info(string.Format("Тип сущности {0} уже существует", entityTypeName));
                return;
            }

            var documentType = IntegrationServiceClient
              .GetEntitiesWithFilter<IDataImportDatabookType>(t => t.Name == documentKind.DocumentType.Name)
              .FirstOrDefault();
            //entityType.DocumentType = documentType;

            var availableAncestorGuids = entityType.AncestorGuids;
            entityType.AncestorGuids = null;
            var newEntityType = IntegrationServiceClient.CreateEntity<IDataImportDatabookType>(entityType);

            CollectionHelper.CellectionItemsClear("IEntityType", newEntityType.Id.ToString(), "AncestorGuids");

            // TODO Понять как заполнять коллекцию
            foreach (var availableAncestorGuid in availableAncestorGuids)
            {
                //var availableGuid = IntegrationServiceClient.GetEntitiesWithFilter<IEntityTypeAncestorGuid>(a => a.Guid == availableAncestorGuid.Guid).FirstOrDefault();
                IntegrationServiceClient.Instance.For<IDataImportDatabookType>().Key(newEntityType)
                  .NavigateTo(x => x.AncestorGuids).Set(new { Guid = "" }).InsertEntryAsync().Wait();
            }*/
        }

        protected override IEnumerable<dynamic> Export()
        {
            return IntegrationServiceClient.Instance.For<IDataImportEntityParameter>()
                //.Expand(c => new { c.Parameters, c.EntityType })
                .Expand(c => c.EntityType)
                .Expand(c => c.Parameters.Select( c =>
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
