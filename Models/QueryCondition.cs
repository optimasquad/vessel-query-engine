namespace VesselQueryEngine.Models;

/// <summary>
/// Represents a parsed query condition
/// </summary>
public class QueryCondition
{
    public string FieldName { get; set; } = string.Empty;
    public ComparisonOperator Operator { get; set; }
    public object Value { get; set; } = null!;
    public bool IsNumeric { get; set; }
}

/// <summary>
/// Supported comparison operators
/// </summary>
public enum ComparisonOperator
{
    Equals,
    LessThan,
    GreaterThan
}

/// <summary>
/// Represents a parsed query with multiple conditions
/// </summary>
public class ParsedQuery
{
    public List<QueryCondition> Conditions { get; set; } = new();
    public bool IsValid { get; set; } = true;
    public string? ErrorMessage { get; set; }
}
