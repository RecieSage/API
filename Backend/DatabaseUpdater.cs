using API.Models;
using API.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Backend
{
    public class DatabaseUpdater
    {

        /// <summary>
        /// Checks if an update is available by verifying if the database
        /// has the 'DB_Scripts' table and if there are new update scripts
        /// that haven't been executed on the database.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if an update is available, otherwise <see langword="false"/>.
        /// </returns>
        public bool IsUpdateAvailable()
        {
            this.DoesDbScriptTableExist();

            return false;
        }

        public void Update()
        {
            //Get all scripts that have not been executed

            //Execute all scripts

            //Update DbScript table
        }

        private List<DbScript> GetMissingScripts()
        {
            return new List<DbScript>();
        }

        private bool DoesDbScriptTableExist()
        {
            using var db = new CookingDataContext();
            db.Database.OpenConnection();
            bool result = db.Database.GetDbConnection().GetSchema("Tables").Rows.OfType<DataRow>().Any(row => this.TabeleRowValidation(row));
            db.Database.CloseConnection();

            return result;
        }

        private bool TabeleRowValidation(DataRow row)
        {
            string? rowName = row["TABLE_NAME"]?.ToString();

            if (rowName == null)
            {
                return false;
            }

            if (rowName.Equals("DB_Scripts", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;

        }
    }
}
