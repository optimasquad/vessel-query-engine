using System.Text.Json;
using System.Text.Json.Serialization;
using VesselQueryEngine.Core;
using VesselQueryEngine.Models;

namespace VesselQueryEngine.Output;

/// <summary>
/// Exports query results to JSON format
/// </summary>
public class JsonExporter
{
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonExporter()
    {
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = null,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    /// <summary>
    /// Exports query results to a JSON file
    /// </summary>
    public string Export(QueryResult result, string outputPath)
    {
        var exportData = new ExportData
        {
            Metadata = new QueryMetadata
            {
                ExecutedAt = result.ExecutedAt,
                Success = result.Success,
                TotalRecords = result.TotalRecords,
                MatchingRecords = result.MatchingRecords,
                ErrorMessage = result.ErrorMessage,
                Conditions = result.Query?.Conditions?.Select(c => new ConditionInfo
                {
                    Field = c.FieldName,
                    Operator = c.Operator.ToString(),
                    Value = c.Value?.ToString() ?? ""
                }).ToList() ?? new List<ConditionInfo>()
            },
            Results = result.Results
        };

        string json = JsonSerializer.Serialize(exportData, _jsonOptions);
        File.WriteAllText(outputPath, json);
        return outputPath;
    }

    /// <summary>
    /// Exports just the vessel data to a JSON file (without metadata)
    /// </summary>
    public string ExportDataOnly(List<Vessel> vessels, string outputPath)
    {
        string json = JsonSerializer.Serialize(vessels, _jsonOptions);
        File.WriteAllText(outputPath, json);
        return outputPath;
    }
}

public class ExportData
{
    public QueryMetadata Metadata { get; set; } = null!;
    public List<Vessel> Results { get; set; } = new();
}

public class QueryMetadata
{
    public DateTime ExecutedAt { get; set; }
    public bool Success { get; set; }
    public int TotalRecords { get; set; }
    public int MatchingRecords { get; set; }
    public string? ErrorMessage { get; set; }
    public List<ConditionInfo> Conditions { get; set; } = new();
}

public class ConditionInfo
{
    public string Field { get; set; } = string.Empty;
    public string Operator { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
