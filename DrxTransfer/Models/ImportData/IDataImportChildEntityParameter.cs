using System;
using System.Collections.Generic;
using System.Text;

namespace DrxTransfer.Models.ImportData
{
    /// <summary>
    /// Справочник Соответствие заполняемых параметров строки свойства-коллекции
    /// </summary>
    public class IDataImportChildEntityParameter : IDataImportEntityParameterBase
    {
        public string Separator { get; set; }               // Разделитель.
        public int EntityParameterId { get; set; }          // ИД соответствия заполняемых параметров сущности.
        public bool IsClearChildEntity { get; set; }        // Очистить коллекцию.
        public string VersionAction { get; set; }           // Действие.
        public bool IsDeleteEmptyEntries { get; set; }      // Удалять пустые записи.
    }
}
