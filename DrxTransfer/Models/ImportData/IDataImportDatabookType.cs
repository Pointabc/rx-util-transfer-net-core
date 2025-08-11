using DrxTransfer.IntegrationServicesClient;
using DrxTransfer.Models.ImportData;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImportData.IntegrationServicesClient.Models.ImportData
{
    /// <summary>
    /// Справочник EntityType - Типы сущностей.
    /// </summary>
    public class IDataImportDatabookType
    {
        public int Id { get; set; }                                         // ИД.
        public string Name { get; set; }                                    // Наименование.
        public string Type { get; set; }                                    // Тип сущности.
        public bool IsAbstract { get; set; }                                // Абстрактный.
        public string EntityTypeGuid { get; set; }                          // Идентификатор типа.
        public string AuthenticationMethod { get; set; }                    // Способ авторизации.
        public IDataImportDatabookType MainEntityType { get; set; }         // Главная сущность.
        public List<ICollectionAncestorGuid> AncestorGuids { get; set; }    // Guid'ы предков.
        public override string ToString()
        {
            return Name;
        }
    }
}
