using ImportData.IntegrationServicesClient.Models.ImportData;
using System;
using System.Collections.Generic;

namespace DrxTransfer.Models.ImportData
{
    /// <summary>
    /// Справочник DatabaseConnection - Подлючение к БД.
    /// </summary>
    public class IDataImportStringConnection
    {
        public int Id { get; set; }                     // ИД.
        public string Name { get; set; }                // Наименование.
        public string Status { get; set; }              // Состояние.
        public string Note { get; set; }                // Примечание.
        public string UserId { get; set; }              // Имя для входа.
        public string Password { get; set; }            // Пароль.
        public string Server { get; set; }              // Имя сервера.
        public string Database { get; set; }            // База данных.
        public string DBMS { get; set; }                // СУБД.
        public string Port { get; set; }                // Порт.
        public string PasswordIsVisible { get; set; }   // Пароль.
    }
}
