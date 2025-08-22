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
    class EntityTypeSerializer : SungeroSerializer
    {
        public EntityTypeSerializer() : base()
        {
            this.EntityName = "EntityType";
            this.EntityType = "IDataImportDatabookType";
        }

        public override void Import(object jsonObject)
        {
            var entityType = (jsonObject as JObject).ToObject<IDataImportDatabookType>();
            var tmpEntityType = entityType;

            var entityTypeName = entityType.Name;
            var activeEntityType = IntegrationServiceClient.GetEntitiesWithFilter<IDataImportDatabookType>(x => x.Name == entityTypeName && x.EntityTypeGuid == entityType.EntityTypeGuid);
            if (activeEntityType != null)
            {
                Logger.Info(string.Format("Тип сущности {0} будет обновлен.", entityTypeName));
                entityType = activeEntityType.FirstOrDefault();
            }
            
            var mainEntityTypeName = entityType.MainEntityType?.Name;

            if (!string.IsNullOrWhiteSpace(mainEntityTypeName))
            {
                // Если не найдена сущность вылетает исключение и сущность не создается.
                IEnumerable<IDataImportDatabookType> mainEntityType = Enumerable.Empty<IDataImportDatabookType>();
                try
                {
                    var entityTypeFinded = IntegrationServiceClient
                        .GetEntitiesWithFilter<IDataImportDatabookType>(x => x.Name == mainEntityTypeName && x.EntityTypeGuid == entityType.MainEntityType.EntityTypeGuid)
                        .FirstOrDefault();
                    entityType.MainEntityType = entityTypeFinded;
                }
                catch
                {
                    var newMainEntityType = IntegrationServiceClient.CreateEntity<IDataImportDatabookType>(entityType.MainEntityType);

                    Logger.Info(string.Format("Создан главный справочник Тип сущности {0}", newMainEntityType.Name));
                    entityType.MainEntityType = newMainEntityType;
                }
            }

            var availableAncestorGuids = tmpEntityType.AncestorGuids;
            entityType.AncestorGuids = null;
            var newEntityType = activeEntityType != null ? entityType : IntegrationServiceClient.CreateEntity<IDataImportDatabookType>(entityType);
            
            if (activeEntityType == null)
                Logger.Info(string.Format("Создан Тип сущности {0}", entityTypeName));

            CollectionHelper.CellectionItemsClear("IDataImportDatabookType", newEntityType.Id.ToString(), "AncestorGuids");

            foreach (var availableAncestorGuid in availableAncestorGuids)
            {
                IntegrationServiceClient.Instance.For<IDataImportDatabookType>()
                    .Key(newEntityType)
                    .NavigateTo(x => x.AncestorGuids)
                    .Set(new { id = availableAncestorGuid.Id, Guid = availableAncestorGuid.Guid })
                    .InsertEntryAsync()
                    .Wait();
            }
        }

        protected override IEnumerable<dynamic> Export()
        {
            return IntegrationServiceClient.Instance.For<IDataImportDatabookType>()
                .Expand(c => c.MainEntityType)
                .Expand(c => c.AncestorGuids)
                .FindEntriesAsync()
                .Result;
        }
    }
}
