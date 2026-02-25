using System.Reflection;
using VesselQueryEngine.Models;

namespace VesselQueryEngine.Core;

/// <summary>
/// Executes parsed queries against the in-memory vessel data
/// </summary>
public class QueryEngine
{
    private readonly List<Vessel> _vessels;
    private readonly Dictionary<string, PropertyInfo> _propertyCache;

    public QueryEngine(List<Vessel> vessels)
    {
        _vessels = vessels ?? throw new ArgumentNullException(nameof(vessels));
        
        // Cache property info for faster lookups
        _propertyCache = typeof(Vessel)
            .GetProperties()
            .ToDictionary(
                p => p.Name.ToUpperInvariant(),
                p => p,
                StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Executes a parsed query and returns matching vessels
    /// </summary>
    public QueryResult Execute(ParsedQuery query)
    {
        var result = new QueryResult
        {
            Query = query,
            ExecutedAt = DateTime.UtcNow
        };

        if (!query.IsValid)
        {
            result.Success = false;
            result.ErrorMessage = query.ErrorMessage;
            return result;
        }

        try
        {
            // Validate all field names exist
            foreach (var condition in query.Conditions)
            {
                if (!_propertyCache.ContainsKey(condition.FieldName.ToUpperInvariant()))
                {
                    result.Success = false;
                    result.ErrorMessage = $"Unknown field: '{condition.FieldName}'. Available fields: {string.Join(", ", _propertyCache.Keys.Take(10))}...";
                    return result;
                }
            }

            // Filter vessels based on all conditions (AND logic)
            var matchingVessels = _vessels.Where(vessel => 
                query.Conditions.All(condition => EvaluateCondition(vessel, condition))
            ).ToList();

            result.Success = true;
            result.Results = matchingVessels;
            result.TotalRecords = _vessels.Count;
            result.MatchingRecords = matchingVessels.Count;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Query execution error: {ex.Message}";
        }

        return result;
    }

    /// <summary>
    /// Evaluates a single condition against a vessel
    /// </summary>
    private bool EvaluateCondition(Vessel vessel, QueryCondition condition)
    {
        var property = _propertyCache[condition.FieldName.ToUpperInvariant()];
        var propertyValue = property.GetValue(vessel);

        // Handle null values
        if (propertyValue == null)
        {
            return false;
        }

        // Numeric comparison
        if (condition.IsNumeric)
        {
            double vesselValue;
            if (propertyValue is double d)
                vesselValue = d;
            else if (propertyValue is int i)
                vesselValue = i;
            else if (propertyValue is long l)
                vesselValue = l;
            else if (propertyValue is float f)
                vesselValue = f;
            else if (double.TryParse(propertyValue.ToString(), out double parsed))
                vesselValue = parsed;
            else
                return false;

            double conditionValue = Convert.ToDouble(condition.Value);

            return condition.Operator switch
            {
                ComparisonOperator.Equals => Math.Abs(vesselValue - conditionValue) < 0.0001,
                ComparisonOperator.LessThan => vesselValue < conditionValue,
                ComparisonOperator.GreaterThan => vesselValue > conditionValue,
                _ => false
            };
        }
        else
        {
            // String comparison (case-insensitive for equals)
            string vesselValueStr = propertyValue.ToString() ?? string.Empty;
            string conditionValueStr = condition.Value.ToString() ?? string.Empty;

            return condition.Operator switch
            {
                ComparisonOperator.Equals => string.Equals(vesselValueStr, conditionValueStr, StringComparison.OrdinalIgnoreCase),
                ComparisonOperator.LessThan => string.Compare(vesselValueStr, conditionValueStr, StringComparison.OrdinalIgnoreCase) < 0,
                ComparisonOperator.GreaterThan => string.Compare(vesselValueStr, conditionValueStr, StringComparison.OrdinalIgnoreCase) > 0,
                _ => false
            };
        }
    }

    /// <summary>
    /// Gets the total number of vessels in the database
    /// </summary>
    public int GetTotalCount() => _vessels.Count;

    /// <summary>
    /// Gets all available field names
    /// </summary>
    public IEnumerable<string> GetAvailableFields() => _propertyCache.Keys;
}

/// <summary>
/// Represents the result of a query execution
/// </summary>
public class QueryResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public ParsedQuery Query { get; set; } = null!;
    public List<Vessel> Results { get; set; } = new();
    public int TotalRecords { get; set; }
    public int MatchingRecords { get; set; }
    public DateTime ExecutedAt { get; set; }
}
