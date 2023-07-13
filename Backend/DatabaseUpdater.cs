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

        /// <summary>
        ///  Attempts to execute all update scripts that haven't been executed yet.
        /// </summary>
        public void Update()
        {
            List<string> updateScripts = this.GetNotExecutedScripts();

            updateScripts.ForEach(script => this.ExecuteScript(script));
        }

        private void ExecuteScript(string scriptid)
        {
            using var db = new CookingDataContext();
            try
            {
                string filePath = $"DatabaseUpdateScripts/{scriptid}.sql";
                string scriptContent = File.ReadAllText(filePath);

                db.Database.ExecuteSqlRaw(scriptContent);

                if (db.DbScripts.Any(script => script.ScriptId.Equals(scriptid)))
                {
                    DbScript dbScript = db.DbScripts.First(script => script.ScriptId.Equals(scriptid));
                    dbScript.Success = true;
                    dbScript.LastExecution = DateTime.Now;
                    dbScript.Output = null;

                    db.DbScripts.Update(dbScript);
                }
                else
                {
                    DbScript dbScript = new DbScript
                    {
                        ScriptId = scriptid,
                        Success = true,
                        LastExecution = DateTime.Now,
                    };

                    db.DbScripts.Add(dbScript);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (db.DbScripts.Any(script => script.ScriptId.Equals(scriptid)))
                {
                    DbScript dbScript = db.DbScripts.First(script => script.ScriptId.Equals(scriptid));
                    dbScript.Success = false;
                    dbScript.LastExecution = DateTime.Now;
                    dbScript.Output = $"Message:\n{e.Message}\n\nStacktrace:\n{e.StackTrace}";

                    db.DbScripts.Update(dbScript);
                }
                else
                {
                    DbScript dbScript = new DbScript
                    {
                        ScriptId = scriptid,
                        Success = false,
                        LastExecution = DateTime.Now,
                        Output = $"Message:\n{e.Message}\n\nStacktrace:\n{e.StackTrace}",
                    };

                    db.DbScripts.Add(dbScript);
                }
            }

            db.SaveChanges();
        }

        private List<string> GetNotExecutedScripts()
        {
            using var db = new CookingDataContext();

            List<DbScript> executedScripts;
            if (this.DoesDbScriptTableExist())
            {
               executedScripts = db.DbScripts.Where(script => script.Success == true).ToList();
            }
            else
            {
                executedScripts = new List<DbScript>();
            }

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
