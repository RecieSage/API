using API.Models;
using API.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Backend
{
    /// <summary>
    /// Managemanet Instance of Database Updates
    /// </summary>
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
            if (this.DoesDbScriptTableExist() && this.GetNotExecutedScripts().Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Update()
        {
            //Get all scripts that have not been executed

            //Execute all scripts

            //Update DbScript table
        }

        private List<string> GetNotExecutedScripts()
        {
            using var db = new CookingDataContext();

            List<DbScript> executedScripts = db.DbScripts.Where(script => script.Success == true).ToList();
            List<string> allScriptIds = this.GetScriptNames();

            return MissingScriptsCompare(executedScripts, allScriptIds);
        }

        private List<string> GetScriptNames()
        {
            List<string> scripts = new List<string>();

            string[] filePaths = Directory.GetFiles("DatabaseUpdateScripts", "*.sql");

            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                scripts.Add(fileName);
            }

            return scripts;
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

        private static List<string> MissingScriptsCompare(List<DbScript> executedScripts, List<string> allScripts)
        {
            List<string> missingScripts = new List<string>();

            foreach (string script in allScripts)
            {
                if (!executedScripts.Any(executedScript => script.Equals(executedScript.ScriptId)))
                {
                    missingScripts.Add(script);
                }
            }

            return missingScripts;
        }
    }
}
