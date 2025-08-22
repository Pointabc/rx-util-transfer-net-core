using System;
using System.Collections.Generic;
using System.Text;

namespace DrxTransfer.Models.ImportData
{
    /// <summary>
    /// Коллекция Параметры в справочнике EntityParameter.
    /// </summary>
    public class ICollectionEntityParameterParameter : ICollectionParameter
    {
        public IDataImportChildEntityParameter ChildEntityParameter { get; set; }   // Дочернее Соответствие заполняемых параметров строки свойства-коллекции
    }
}
