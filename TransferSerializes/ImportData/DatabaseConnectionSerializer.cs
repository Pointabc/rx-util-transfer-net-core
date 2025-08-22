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
    class DatabaseConnectionSerializer : SungeroSerializer
    {
        public DatabaseConnectionSerializer() : base()
        {
            this.EntityName = "DatabaseConnection";
            this.EntityType = "IDataImportStringConnection";
        }

        public override void Import(object jsonObject)
        {
            var databaseConnection = (jsonObject as JObject).ToObject<IDataImportStringConnection>();
            var databaseConnectionName = databaseConnection.Name;
            var activeDatabaseConnection = IntegrationServiceClient.GetEntitiesWithFilter<IDataImportStringConnection>(x => x.Name == databaseConnectionName);

            if (activeDatabaseConnection != null)
            {
                Logger.Info(string.Format("Тип сущности {0} будет обновлен.", databaseConnectionName));
                databaseConnection = activeDatabaseConnection.FirstOrDefault();
            }

            var newEntityType = activeDatabaseConnection != null 
                ? databaseConnection 
                : IntegrationServiceClient.CreateEntity<IDataImportStringConnection>(databaseConnection);

            if (activeDatabaseConnection == null)
                Logger.Info(string.Format("Создан Тип сущности {0}", databaseConnectionName));
        }

        protected override IEnumerable<dynamic> Export()
        {
            return IntegrationServiceClient.Instance.For<IDataImportStringConnection>()
                .FindEntriesAsync()
                .Result;
        }
    }
}
