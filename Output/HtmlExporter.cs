using System.Text;
using VesselQueryEngine.Core;
using VesselQueryEngine.Models;

namespace VesselQueryEngine.Output;

/// <summary>
/// Exports query results to HTML format
/// </summary>
public class HtmlExporter
{
    /// <summary>
    /// Exports query results to an HTML file with a styled table
    /// </summary>
    public string Export(QueryResult result, string outputPath)
    {
        var html = GenerateHtml(result);
        File.WriteAllText(outputPath, html);
        return outputPath;
    }

    /// <summary>
    /// Generates HTML content for the query results
    /// </summary>
    private string GenerateHtml(QueryResult result)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html lang=\"en\">");
        sb.AppendLine("<head>");
        sb.AppendLine("    <meta charset=\"UTF-8\">");
        sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        sb.AppendLine("    <title>Vessel Query Results</title>");
        sb.AppendLine("    <style>");
        sb.AppendLine(GetCssStyles());
        sb.AppendLine("    </style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        
        // Header
        sb.AppendLine("    <div class=\"container\">");
        sb.AppendLine("        <header>");
        sb.AppendLine("            <h1>🚢 Vessel Query Engine Results</h1>");
        sb.AppendLine($"            <p class=\"subtitle\">Generated: {result.ExecutedAt:yyyy-MM-dd HH:mm:ss UTC}</p>");
        sb.AppendLine("        </header>");
        
        // Query Info
        sb.AppendLine("        <div class=\"query-info\">");
        sb.AppendLine("            <h2>Query Information</h2>");
        sb.AppendLine($"            <p><strong>Status:</strong> <span class=\"{(result.Success ? "success" : "error")}\">{(result.Success ? "Success" : "Failed")}</span></p>");
        
        if (result.Query?.Conditions != null && result.Query.Conditions.Any())
        {
            sb.AppendLine("            <p><strong>Conditions:</strong></p>");
            sb.AppendLine("            <ul>");
            foreach (var condition in result.Query.Conditions)
            {
                var opSymbol = condition.Operator switch
                {
                    ComparisonOperator.Equals => "=",
                    ComparisonOperator.LessThan => "<",
                    ComparisonOperator.GreaterThan => ">",
                    _ => "?"
                };
                var valueDisplay = condition.IsNumeric ? condition.Value.ToString() : $"'{condition.Value}'";
                sb.AppendLine($"                <li><code>{condition.FieldName} {opSymbol} {valueDisplay}</code></li>");
            }
            sb.AppendLine("            </ul>");
        }
        
        if (!result.Success && !string.IsNullOrEmpty(result.ErrorMessage))
        {
            sb.AppendLine($"            <p class=\"error\"><strong>Error:</strong> {result.ErrorMessage}</p>");
        }
        else
        {
            sb.AppendLine($"            <p><strong>Total Records:</strong> {result.TotalRecords}</p>");
            sb.AppendLine($"            <p><strong>Matching Records:</strong> {result.MatchingRecords}</p>");
        }
        sb.AppendLine("        </div>");
        
        // Results Table
        if (result.Success && result.Results.Any())
        {
            sb.AppendLine("        <div class=\"results\">");
            sb.AppendLine("            <h2>Results</h2>");
            sb.AppendLine("            <div class=\"table-container\">");
            sb.AppendLine("            <table>");
            sb.AppendLine("                <thead>");
            sb.AppendLine("                    <tr>");
            sb.AppendLine("                        <th>#</th>");
            sb.AppendLine("                        <th>Vessel Name</th>");
            sb.AppendLine("                        <th>IMO Number</th>");
            sb.AppendLine("                        <th>Type</th>");
            sb.AppendLine("                        <th>Builder Group</th>");
            sb.AppendLine("                        <th>Year Built</th>");
            sb.AppendLine("                        <th>Flag</th>");
            sb.AppendLine("                        <th>DWT (tonnes)</th>");
            sb.AppendLine("                        <th>Status Code</th>");
            sb.AppendLine("                        <th>Owner</th>");
            sb.AppendLine("                    </tr>");
            sb.AppendLine("                </thead>");
            sb.AppendLine("                <tbody>");
            
            int index = 1;
            foreach (var vessel in result.Results)
            {
                sb.AppendLine("                    <tr>");
                sb.AppendLine($"                        <td>{index++}</td>");
                sb.AppendLine($"                        <td class=\"name\">{EscapeHtml(vessel.Z01_CURRENT_NAME)}</td>");
                sb.AppendLine($"                        <td>{vessel.X03_IMO_NUMBER}</td>");
                sb.AppendLine($"                        <td>{EscapeHtml(vessel.P59_VESSEL_TYPE_SHORT)}</td>");
                sb.AppendLine($"                        <td>{EscapeHtml(vessel.BUILDER_GROUP)}</td>");
                sb.AppendLine($"                        <td>{vessel.A12_YEAR_BUILT}</td>");
                sb.AppendLine($"                        <td>{EscapeHtml(vessel.FLAG)}</td>");
                sb.AppendLine($"                        <td class=\"number\">{vessel.A04_DWT_tonnes:N0}</td>");
                sb.AppendLine($"                        <td class=\"number\">{vessel.Z13_STATUS_CODE}</td>");
                sb.AppendLine($"                        <td>{EscapeHtml(vessel.BEN_OWNER)}</td>");
                sb.AppendLine("                    </tr>");
            }
            
            sb.AppendLine("                </tbody>");
            sb.AppendLine("            </table>");
            sb.AppendLine("            </div>");
            sb.AppendLine("        </div>");
        }
        else if (result.Success)
        {
            sb.AppendLine("        <div class=\"no-results\">");
            sb.AppendLine("            <p>No vessels match the query criteria.</p>");
            sb.AppendLine("        </div>");
        }
        
        // Footer
        sb.AppendLine("        <footer>");
        sb.AppendLine("            <p>Vessel Query Engine - In-Memory Database</p>");
        sb.AppendLine("        </footer>");
        sb.AppendLine("    </div>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
        
        return sb.ToString();
    }

    private string GetCssStyles()
    {
        return @"
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
            min-height: 100vh;
            color: #e4e4e4;
        }
        .container {
            max-width: 1400px;
            margin: 0 auto;
            padding: 2rem;
        }
        header {
            text-align: center;
            margin-bottom: 2rem;
            padding: 2rem;
            background: rgba(255, 255, 255, 0.05);
            border-radius: 15px;
            backdrop-filter: blur(10px);
        }
        header h1 {
            font-size: 2.5rem;
            background: linear-gradient(90deg, #00d4ff, #00ff88);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            background-clip: text;
            margin-bottom: 0.5rem;
        }
        .subtitle {
            color: #888;
        }
        .query-info {
            background: rgba(255, 255, 255, 0.08);
            border-radius: 15px;
            padding: 1.5rem;
            margin-bottom: 2rem;
        }
        .query-info h2 {
            color: #00d4ff;
            margin-bottom: 1rem;
            font-size: 1.3rem;
        }
        .query-info p {
            margin-bottom: 0.5rem;
        }
        .query-info ul {
            margin-left: 2rem;
            margin-top: 0.5rem;
        }
        .query-info code {
            background: rgba(0, 212, 255, 0.2);
            padding: 0.2rem 0.5rem;
            border-radius: 5px;
            font-family: 'Consolas', monospace;
        }
        .success {
            color: #00ff88;
            font-weight: bold;
        }
        .error {
            color: #ff6b6b;
        }
        .results {
            background: rgba(255, 255, 255, 0.05);
            border-radius: 15px;
            padding: 1.5rem;
            margin-bottom: 2rem;
        }
        .results h2 {
            color: #00ff88;
            margin-bottom: 1rem;
            font-size: 1.3rem;
        }
        .table-container {
            overflow-x: auto;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            font-size: 0.9rem;
        }
        th, td {
            padding: 0.8rem;
            text-align: left;
            border-bottom: 1px solid rgba(255, 255, 255, 0.1);
        }
        th {
            background: rgba(0, 212, 255, 0.2);
            color: #00d4ff;
            font-weight: 600;
            position: sticky;
            top: 0;
        }
        tr:hover {
            background: rgba(255, 255, 255, 0.05);
        }
        td.name {
            font-weight: 600;
            color: #00d4ff;
        }
        td.number {
            text-align: right;
            font-family: 'Consolas', monospace;
        }
        .no-results {
            text-align: center;
            padding: 3rem;
            background: rgba(255, 255, 255, 0.05);
            border-radius: 15px;
            color: #888;
        }
        footer {
            text-align: center;
            padding: 1.5rem;
            color: #666;
            font-size: 0.9rem;
        }";
    }

    private string EscapeHtml(string? text)
    {
        if (string.IsNullOrEmpty(text))
            return "N/A";
        
        return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#39;");
    }
}
