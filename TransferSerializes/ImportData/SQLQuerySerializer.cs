using DrxTransfer;
using DrxTransfer.Models.ImportData;
using ImportData.IntegrationServicesClient.Models.ImportData;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace TransferSerializes.ImportData
{
    [Export(typeof(SungeroSerializer))]
    class SQLQuerySerializer : SungeroSerializer
    {
        public SQLQuerySerializer() : base()
        {
            this.EntityName = "SQLQuery";
            this.EntityType = "IDataImportSQLQuery";
        }

        public override void Import(object jsonObject)
        {
            var sqlQuery = (jsonObject as JObject).ToObject<IDataImportSQLQuery>();
            var sqlQueryName = sqlQuery.Name;
            var activeSQLQuery = IntegrationServiceClient.GetEntitiesWithFilter<IDataImportSQLQuery>(x => x.Name == sqlQueryName && Equals(x.EntityParameter, sqlQuery.EntityParameter));

            if (activeSQLQuery != null)
            {
                Logger.Info(string.Format("Тип сущности {0} будет обновлен.", sqlQueryName));
                sqlQuery = activeSQLQuery.FirstOrDefault();
            }

            var mainSQLQueryName = sqlQuery.MainSQLQuery?.Name;

            if (!string.IsNullOrWhiteSpace(mainSQLQueryName))
            {
                // Если не найдена сущность вылетает исключение и сущность не создается.
                IEnumerable<IDataImportSQLQuery> mainEntityType = Enumerable.Empty<IDataImportSQLQuery>();
                try
                {
                    var entityTypeFinded = IntegrationServiceClient
                        .GetEntitiesWithFilter<IDataImportSQLQuery>(x => x.Name == mainSQLQueryName)
                        .FirstOrDefault();
                    sqlQuery.MainSQLQuery = entityTypeFinded;
                }
                catch
                {
                    var newMainSQLQuery = IntegrationServiceClient.CreateEntity<IDataImportSQLQuery>(sqlQuery.MainSQLQuery);

                    Logger.Info(string.Format("Создан главный справочник Тип сущности {0}", newMainSQLQuery.Name));
                    sqlQuery.MainSQLQuery = newMainSQLQuery;
                }
            }

            var newSQLQuery = activeSQLQuery != null ? sqlQuery : IntegrationServiceClient.CreateEntity<IDataImportSQLQuery>(sqlQuery);

            if (activeSQLQuery == null)
                Logger.Info(string.Format("Создан Тип сущности {0}", sqlQueryName));
        }

        protected override IEnumerable<dynamic> Export()
        {
            return IntegrationServiceClient.Instance.For<IDataImportSQLQuery>()
                .Expand(c => c.MainSQLQuery)
                .FindEntriesAsync()
                .Result;
        }
    }
}
