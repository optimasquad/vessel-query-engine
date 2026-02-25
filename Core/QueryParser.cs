using System.Text.RegularExpressions;
using VesselQueryEngine.Models;

namespace VesselQueryEngine.Core;

/// <summary>
/// Parses user queries into structured QueryCondition objects
/// Supports: WHERE field = value, WHERE field > value, WHERE field < value
/// AND logic for combining conditions
/// </summary>
public class QueryParser
{
    private static readonly Regex WherePattern = new(
        @"^\s*WHERE\s+(.+)\s*$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);
    
    private static readonly Regex ConditionPattern = new(
        @"([\w_]+)\s*([=<>])\s*('([^']*)'|([\d.\-]+))",
        RegexOptions.Compiled);
    
    private static readonly Regex AndSplitPattern = new(
        @"\s+AND\s+",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    /// <summary>
    /// Parses a query string into a ParsedQuery object
    /// </summary>
    public ParsedQuery Parse(string query)
    {
        var result = new ParsedQuery();

        if (string.IsNullOrWhiteSpace(query))
        {
            result.IsValid = false;
            result.ErrorMessage = "Query cannot be empty";
            return result;
        }

        // Extract the WHERE clause content
        var whereMatch = WherePattern.Match(query.Trim());
        if (!whereMatch.Success)
        {
            result.IsValid = false;
            result.ErrorMessage = "Query must start with WHERE clause. Example: WHERE Z13_STATUS_CODE = 4";
            return result;
        }

        string conditionsText = whereMatch.Groups[1].Value.Trim();
        
        // Split by AND
        var conditionParts = AndSplitPattern.Split(conditionsText);
        
        foreach (var part in conditionParts)
        {
            var condition = ParseCondition(part.Trim());
            if (condition == null)
            {
                result.IsValid = false;
                result.ErrorMessage = $"Invalid condition format: '{part.Trim()}'. Expected format: field_name = value or field_name < value or field_name > value";
                return result;
            }
            result.Conditions.Add(condition);
        }

        if (result.Conditions.Count == 0)
        {
            result.IsValid = false;
            result.ErrorMessage = "No valid conditions found in query";
        }

        return result;
    }

    /// <summary>
    /// Parses a single condition string
    /// </summary>
    private QueryCondition? ParseCondition(string conditionText)
    {
        var match = ConditionPattern.Match(conditionText);
        if (!match.Success)
        {
            return null;
        }

        var condition = new QueryCondition
        {
            FieldName = match.Groups[1].Value
        };

        // Parse operator
        condition.Operator = match.Groups[2].Value switch
        {
            "=" => ComparisonOperator.Equals,
            "<" => ComparisonOperator.LessThan,
            ">" => ComparisonOperator.GreaterThan,
            _ => throw new InvalidOperationException($"Unknown operator: {match.Groups[2].Value}")
        };

        // Parse value - check if it's a quoted string or a number
        if (match.Groups[4].Success && match.Groups[4].Length > 0)
        {
            // Quoted string value
            condition.Value = match.Groups[4].Value;
            condition.IsNumeric = false;
        }
        else if (match.Groups[5].Success)
        {
            // Numeric value
            string numStr = match.Groups[5].Value;
            if (double.TryParse(numStr, out double numValue))
            {
                condition.Value = numValue;
                condition.IsNumeric = true;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        return condition;
    }

    /// <summary>
    /// Gets a list of supported query examples
    /// </summary>
    public static string GetQueryHelp()
    {
        return @"
╔══════════════════════════════════════════════════════════════════════════════╗
║                           QUERY SYNTAX HELP                                   ║
╠══════════════════════════════════════════════════════════════════════════════╣
║  Supported Operators:                                                         ║
║    =  (equals)                                                                ║
║    <  (less than)                                                             ║
║    >  (greater than)                                                          ║
║                                                                               ║
║  Combining Conditions:                                                        ║
║    Use AND to combine multiple conditions                                     ║
║                                                                               ║
║  Examples:                                                                    ║
║    WHERE Z13_STATUS_CODE = 4                                                  ║
║    WHERE BUILDER_GROUP = 'Guoyu Logistics'                                    ║
║    WHERE A12_YEAR_BUILT > 2010                                                ║
║    WHERE A04_DWT_tonnes < 50000                                               ║
║    WHERE Z13_STATUS_CODE = 4 AND BUILDER_GROUP = 'Guoyu Logistics'            ║
║    WHERE A12_YEAR_BUILT > 2010 AND FLAG = 'Singapore'                         ║
║                                                                               ║
║  Available Fields:                                                            ║
║    Z13_STATUS_CODE, BUILDER_GROUP, A12_YEAR_BUILT, FLAG, BUILDER,             ║
║    A04_DWT_tonnes, A05_GT, Z01_CURRENT_NAME, BUILDER_COUNTRY, etc.            ║
╚══════════════════════════════════════════════════════════════════════════════╝";
    }
}
