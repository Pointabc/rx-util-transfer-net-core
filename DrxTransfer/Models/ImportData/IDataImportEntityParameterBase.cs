using ImportData.IntegrationServicesClient.Models.ImportData;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrxTransfer.Models.ImportData
{
    public class IDataImportEntityParameterBase
    {
        public int Id { get; set; }                                         // ИД.
        public string Status { get; set; }                                  // Состояние.
        public string Name { get; set; }                                    // Наименование.
        public string Type { get; set; }                                    // Тип (Документ; Справочник).
        public IDataImportDatabookType EntityType { get; set; }             // Тип сущности.
        public List<ICollectionParameter> Parameters { get; set; }
        public string DataSource { get; set; }                              // Тип (ExcelFile; SQLQuery).
        public bool? DisableValidation { get; set; }                        // Отключить проверку обязательности свойств.
        public bool? DisableImportDataLog { get; set; }                     // Отключить логирование Импорта данных.
        public string ExtSystemId { get; set; }                             // Код внешней системы.
        public string DuplicateHandling { get; set; }                       // Правило обработки дубликатов (None; CreateNew; TakeFirst).
        public string SheetName { get; set; }                               // Имя листа.
        public override string ToString()
        {
            return Name;
        }
    }
}
