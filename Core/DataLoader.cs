using System.Text.Json;
using System.Text.RegularExpressions;
using VesselQueryEngine.Models;

namespace VesselQueryEngine.Core;

/// <summary>
/// Responsible for loading vessel data from JSON files
/// </summary>
public class DataLoader
{
    /// <summary>
    /// Loads vessel data from a JSON file
    /// Handles JavaScript-style variable assignment format (var vessels = [...])
    /// </summary>
    public List<Vessel> LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Data file not found: {filePath}");
        }

        string content = File.ReadAllText(filePath);
        
        // Remove JavaScript variable assignment if present
        content = ExtractJsonArray(content);
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        try
        {
            var vessels = JsonSerializer.Deserialize<List<Vessel>>(content, options);
            return vessels ?? new List<Vessel>();
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException($"Failed to parse JSON data: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Extracts the JSON array from JavaScript-style content
    /// </summary>
    private string ExtractJsonArray(string content)
    {
        // Remove JavaScript comments
        content = Regex.Replace(content, @"/\*.*?\*/", "", RegexOptions.Singleline);
        content = Regex.Replace(content, @"//.*?$", "", RegexOptions.Multiline);
        
        // Handle "var varName = [" format - find start of array
        var startMatch = Regex.Match(content, @"var\s+\w+\s*=\s*\[", RegexOptions.Singleline);
        if (startMatch.Success)
        {
            // Get everything from the opening bracket
            int startIndex = startMatch.Index + startMatch.Length - 1; // Position of '['
            content = content.Substring(startIndex);
        }
        
        // Trim whitespace
        content = content.Trim();
        
        // If it starts with '[' but doesn't end with ']', add the closing bracket
        if (content.StartsWith("[") && !content.TrimEnd().EndsWith("]"))
        {
            content = content.TrimEnd();
            // Remove trailing comma if present
            if (content.EndsWith(","))
            {
                content = content.Substring(0, content.Length - 1);
            }
            // Handle incomplete object - try to close it
            if (content.EndsWith("}"))
            {
                content += "]";
            }
            else
            {
                // Try to find the last complete object and close the array
                int lastBrace = content.LastIndexOf('}');
                if (lastBrace > 0)
                {
                    content = content.Substring(0, lastBrace + 1) + "]";
                }
            }
        }
        
        return content;
    }
}
