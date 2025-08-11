using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DrxTransfer.Models.ImportData
{
    /// <summary>
    /// Справочник Соответствие заполняемых параметров свойства-ссылки.
    /// </summary>
    public class IImportDataNavigationParameter : IDataImportEntityParameterBase
    {
        public int EntityParameterId { get; set; } 
    }
}
