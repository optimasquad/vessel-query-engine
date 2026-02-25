# 🚢 Vessel Query Engine - In-Memory Database

A C# console application that implements an in-memory database with a custom query language for querying vessel data.

## Features

- **In-Memory Database**: Loads JSON vessel data into memory for fast querying
- **Custom Query Language**: Supports SQL-like WHERE clauses with AND logic
- **Multiple Output Formats**: Exports results to both HTML (styled table) and JSON
- **Interactive CLI**: User-friendly command-line interface with help and field listing
- **Robust Parsing**: Handles JavaScript-style JSON files (with `var varName = [...]` format)

## Architecture

```
VesselQueryEngine/
├── Models/
│   ├── Vessel.cs           # Data model for vessel records
│   └── QueryCondition.cs   # Query condition models
├── Core/
│   ├── DataLoader.cs       # JSON data loading and parsing
│   ├── QueryParser.cs      # Query string parsing
│   └── QueryEngine.cs      # Query execution logic
├── Output/
│   ├── HtmlExporter.cs     # HTML result generation
│   └── JsonExporter.cs     # JSON result generation
└── Program.cs              # Main entry point and CLI
```

## Requirements

- .NET 8.0 SDK or later
- A JSON data file containing vessel records

## Installation & Running

1. Clone the repository:
```bash
git clone https://github.com/optimasquad/VesselQueryEngine.git
cd VesselQueryEngine
```

2. Build the project:
```bash
dotnet build
```

3. Run the application:
```bash
# With default data file path
dotnet run

# Or specify a custom data file
dotnet run /path/to/your/data.json
```

## Query Language Syntax

### Basic Syntax
```
WHERE <field_name> <operator> <value>
```

### Supported Operators
| Operator | Description |
|----------|-------------|
| `=` | Equals (works for both numbers and strings) |
| `<` | Less than (numeric comparison) |
| `>` | Greater than (numeric comparison) |

### Combining Conditions
Use `AND` to combine multiple conditions:
```
WHERE <condition1> AND <condition2>
```

### Value Types
- **Numeric values**: Enter directly without quotes: `WHERE Z13_STATUS_CODE = 4`
- **Text values**: Enclose in single quotes: `WHERE BUILDER_GROUP = 'Guoyu Logistics'`

## Query Examples

### Simple Queries
```sql
-- Find vessels with status code 4
WHERE Z13_STATUS_CODE = 4

-- Find vessels built by a specific builder group
WHERE BUILDER_GROUP = 'Guoyu Logistics'

-- Find vessels built after 2010
WHERE A12_YEAR_BUILT > 2010

-- Find vessels with DWT less than 50000
WHERE A04_DWT_tonnes < 50000
```

### Combined Queries (AND logic)
```sql
-- Find vessels with status code 4 built by Guoyu Logistics
WHERE Z13_STATUS_CODE = 4 AND BUILDER_GROUP = 'Guoyu Logistics'

-- Find vessels built after 2010 flagged in Singapore
WHERE A12_YEAR_BUILT > 2010 AND FLAG = 'Singapore'

-- Find bulk carriers with high DWT
WHERE P59_VESSEL_TYPE_SHORT = 'Bulk' AND A04_DWT_tonnes > 50000
```

## Interactive Commands

| Command | Description |
|---------|-------------|
| `help` | Show query syntax help |
| `fields` | List all available field names |
| `clear` | Clear the console screen |
| `exit` or `quit` | Exit the application |

## Output Formats

### HTML Output
- Styled responsive table
- Dark theme with gradient background
- Query metadata and results summary
- Viewable in any web browser

### JSON Output
```json
{
  "Metadata": {
    "ExecutedAt": "2026-02-25T13:47:34.222Z",
    "Success": true,
    "TotalRecords": 22,
    "MatchingRecords": 8,
    "Conditions": [
      {
        "Field": "Z13_STATUS_CODE",
        "Operator": "Equals",
        "Value": "4"
      }
    ]
  },
  "Results": [
    // Array of matching vessel records
  ]
}
```

## Available Fields

The database supports the following vessel fields:

| Category | Fields |
|----------|--------|
| **Identification** | X01_CVN, X03_IMO_NUMBER, Z01_CURRENT_NAME, Z02_EXNAME |
| **Classification** | P59_VESSEL_TYPE_SHORT, P36_VESSEL_TYPE, ZS0_SHIP_TYPE |
| **Dimensions** | A04_DWT_TONNES, A05_GT, A06_LOA_M, A07_BREADTH_M, A08_DRAFT_M |
| **Build Info** | A12_YEAR_BUILT, A13_MONTH_BUILT, BUILDER, BUILDER_GROUP, BUILDER_COUNTRY |
| **Ownership** | BEN_OWNER, GROUP_OWNER, FLAG |
| **Status** | Z13_STATUS_CODE, Z11_MAIN_STATUS, L93_STATUS |
| **Performance** | D22_SPEED_KNOTS, D10_MAIN_FUEL_CONSUMPTION_TPD |

## Error Handling

The application provides clear error messages for:
- Invalid query syntax
- Unknown field names
- File not found errors
- JSON parsing errors

## License

MIT License - feel free to use, modify, and distribute.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
