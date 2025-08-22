using DrxTransfer.IntegrationServicesClient;
using ImportData.IntegrationServicesClient.Models.ImportData;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace DrxTransfer.Models.ImportData
{
    /// <summary>
    /// Справочник EntityParameter - Соответствие заполняемых параметров сущности.
    /// </summary>
    public class IDataImportEntityParameter : IDataImportEntityParameterBase
    {
        public List<ICollectionEntityParameterParameter> EntityParameterParameters { get; set; }
        public string Action { get; set; }                                  // Действие.
        public string HistoricalField { get; set; }                         // Историческое поле.
    }
}
