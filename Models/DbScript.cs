using System;
using System.Collections.Generic;

namespace API.Models;

/// <summary>
/// Represents a database script entry
/// </summary>
public partial class DbScript
{
    /// <summary>
    /// The Id if the sqipt entry
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The Id of the script file
    /// </summary>
    public string ScriptId { get; set; } = null!;

    /// <summary>
    /// states if the script was executed successfully
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The output of the script if an error occured
    /// </summary>
    public string? Output { get; set; }

    /// <summary>
    /// The date of the last execution
    /// </summary>
    public DateTime LastExecution { get; set; }
}
