using ImportData.IntegrationServicesClient.Models.ImportData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DrxTransfer.Models.ImportData
{
    /// <summary>
    /// Коллекция Параметры.
    /// </summary>
    public class ICollectionParameter
    {
        public int Id { get; set; }                                                // ИД.
        public string PropertyName { get; set; }                                    // Наименование свойства.
        public string PropertyTypeGuid { get; set; }                                // Guid типа свойства.
        public string LocalizedPropertyName { get; set; }                           // Локализованное имя свойства.
        public string PropertyType { get; set; }                                    // Тип свойства.
        public bool? IsRequired { get; set; }                                       // Обязательное свойство.
        public string ExcelColumn { get; set; }                                     // Excel столбец.
        public string SQLColumn { get; set; }                                       // SQL столбец.
        public string FillOption { get; set; }                                      // Перечисление (Ссылка внешней системы; По сущности; Заполнение 1:1; Путь к файлу; Base64; Бинарные данные; Коллекция).
        public IDataImportDatabookType EntityType { get; set; }                     // Объект.
        public string DefaultValue { get; set; }                                    // Значение по умолчанию.
        public IImportDataNavigationParameter NavigationParameter { get; set; }     // Ссылка параметр.
        public int Priority { get; set; }                                           // Приоритет.
        public int DefaultValueId { get; set; }                                     // ИД значения по умолчанию.
        public string DefaultValueType { get; set; }                                // Тип значения по умолчанию.
        public bool? IsKey { get; set; }                                            // Ключевое поле.
        public IDataImportChildEntityParameter ChildEntityParameter { get; set; }   // Дочернее Соответствие заполняемых параметров строки свойства-коллекции
    }
}
