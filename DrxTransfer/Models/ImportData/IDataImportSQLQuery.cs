using System;
using System.Collections.Generic;
using System.Text;

namespace DrxTransfer.Models.ImportData
{
    /// <summary>
    /// Справочник SQLQuery - SQL-запрос.
    /// </summary>
    public class IDataImportSQLQuery
    {
        public int Id { get; set; }                                         // ИД.
        public string SQLstring { get; set; }                               // SQL-запрос.
        public string Status { get; set; }                                  // Состояние.
        public string Name { get; set; }                                    // Наименование.
        public string Type { get; set; }                                    // Тип сущности.
        public string SQLParameter { get; set; }                            // Параметр.
        public IDataImportStringConnection ConnectionDB { get; set; }       // Подключение к БД.
        public IDataImportEntityParameterBase EntityParameter { get; set; } // Соответствие заполняемых параметров сущности.
        public IDataImportSQLQuery MainSQLQuery { get; set; }               // Главный SQL-запрос.
    }
}
