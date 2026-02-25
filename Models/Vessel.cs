using System.Text.Json.Serialization;

namespace VesselQueryEngine.Models;

/// <summary>
/// Represents a vessel record from the database
/// </summary>
public class Vessel
{
    [JsonPropertyName("X01_CVN")]
    public long? X01_CVN { get; set; }
    
    [JsonPropertyName("Z30_BEN_OWNER_ID")]
    public long? Z30_BEN_OWNER_ID { get; set; }
    
    [JsonPropertyName("P59_VESSEL_TYPE_SHORT")]
    public string? P59_VESSEL_TYPE_SHORT { get; set; }
    
    [JsonPropertyName("L91_HULL_TYPE")]
    public string? L91_HULL_TYPE { get; set; }
    
    [JsonPropertyName("Z01_CURRENT_NAME")]
    public string? Z01_CURRENT_NAME { get; set; }
    
    [JsonPropertyName("Z20_SPEC_VALUE")]
    public double? Z20_SPEC_VALUE { get; set; }
    
    [JsonPropertyName("Z21_SPEC_UNIT")]
    public string? Z21_SPEC_UNIT { get; set; }
    
    [JsonPropertyName("A04_DWT_tonnes")]
    public double? A04_DWT_tonnes { get; set; }
    
    [JsonPropertyName("A05_GT")]
    public double? A05_GT { get; set; }
    
    [JsonPropertyName("FLAG")]
    public string? FLAG { get; set; }
    
    [JsonPropertyName("A12_YEAR_BUILT")]
    public int? A12_YEAR_BUILT { get; set; }
    
    [JsonPropertyName("A13_MONTH_BUILT")]
    public int? A13_MONTH_BUILT { get; set; }
    
    [JsonPropertyName("BUILDER")]
    public string? BUILDER { get; set; }
    
    [JsonPropertyName("BUILDER_FULL")]
    public string? BUILDER_FULL { get; set; }
    
    [JsonPropertyName("Z70_MAJOR_GROUP_ID")]
    public long? Z70_MAJOR_GROUP_ID { get; set; }
    
    [JsonPropertyName("GROUP_OWNER")]
    public string? GROUP_OWNER { get; set; }
    
    [JsonPropertyName("Z94_BUILDER_ID")]
    public long? Z94_BUILDER_ID { get; set; }
    
    [JsonPropertyName("ZS0_SHIP_TYPE")]
    public string? ZS0_SHIP_TYPE { get; set; }
    
    [JsonPropertyName("Z11_MAIN_STATUS")]
    public string? Z11_MAIN_STATUS { get; set; }
    
    [JsonPropertyName("BUILDER_COUNTRY_ID")]
    public long? BUILDER_COUNTRY_ID { get; set; }
    
    [JsonPropertyName("Z80_VESSEL_AGE")]
    public double? Z80_VESSEL_AGE { get; set; }
    
    [JsonPropertyName("OWNER_REAL_NATIONALITY_ID")]
    public long? OWNER_REAL_NATIONALITY_ID { get; set; }
    
    [JsonPropertyName("BEN_OWNER")]
    public string? BEN_OWNER { get; set; }
    
    [JsonPropertyName("BEN_OWNER_FULL")]
    public string? BEN_OWNER_FULL { get; set; }
    
    [JsonPropertyName("ZH9_DEMOLITION_DATE")]
    public string? ZH9_DEMOLITION_DATE { get; set; }
    
    [JsonPropertyName("Z02_EXNAME")]
    public string? Z02_EXNAME { get; set; }
    
    [JsonPropertyName("Z06_CGT")]
    public double? Z06_CGT { get; set; }
    
    [JsonPropertyName("Z05_DATE_BUILT")]
    public string? Z05_DATE_BUILT { get; set; }
    
    [JsonPropertyName("Z03_FLAG_CODE")]
    public string? Z03_FLAG_CODE { get; set; }
    
    [JsonPropertyName("BUILDER_COUNTRY")]
    public string? BUILDER_COUNTRY { get; set; }
    
    [JsonPropertyName("EF01105_ENGINE_1_DESIGNER_COMPANY")]
    public string? EF01105_ENGINE_1_DESIGNER_COMPANY { get; set; }
    
    [JsonPropertyName("Z19_CLASS")]
    public string? Z19_CLASS { get; set; }
    
    [JsonPropertyName("Z15_TYPE_OF_CHANGE_DATE")]
    public string? Z15_TYPE_OF_CHANGE_DATE { get; set; }
    
    [JsonPropertyName("ZY0_WORLD_FLEET_TYPE")]
    public string? ZY0_WORLD_FLEET_TYPE { get; set; }
    
    [JsonPropertyName("P36_VESSEL_TYPE")]
    public string? P36_VESSEL_TYPE { get; set; }
    
    [JsonPropertyName("Z22_SIZE")]
    public double? Z22_SIZE { get; set; }
    
    [JsonPropertyName("Z23_SIZE_UNIT")]
    public string? Z23_SIZE_UNIT { get; set; }
    
    [JsonPropertyName("A06_LOA_m")]
    public double? A06_LOA_m { get; set; }
    
    [JsonPropertyName("A08_DRAFT_m")]
    public double? A08_DRAFT_m { get; set; }
    
    [JsonPropertyName("D22_SPEED_knots")]
    public double? D22_SPEED_knots { get; set; }
    
    [JsonPropertyName("D10_MAIN_FUEL_CONSUMPTION_tpd")]
    public double? D10_MAIN_FUEL_CONSUMPTION_tpd { get; set; }
    
    [JsonPropertyName("A07_BREADTH_m")]
    public double? A07_BREADTH_m { get; set; }
    
    [JsonPropertyName("Z14_TYPE_OF_CHANGE")]
    public string? Z14_TYPE_OF_CHANGE { get; set; }
    
    [JsonPropertyName("ZV2_OLD_SHORT_TYPE")]
    public string? ZV2_OLD_SHORT_TYPE { get; set; }
    
    [JsonPropertyName("ZV3_CURRENT_SHORT_TYPE")]
    public string? ZV3_CURRENT_SHORT_TYPE { get; set; }
    
    [JsonPropertyName("ZV1_DATE_CONVERSION_BUILT")]
    public string? ZV1_DATE_CONVERSION_BUILT { get; set; }
    
    [JsonPropertyName("Z83_BEST_TOC_DATE")]
    public string? Z83_BEST_TOC_DATE { get; set; }
    
    [JsonPropertyName("P19_BUILDER_STATUS")]
    public string? P19_BUILDER_STATUS { get; set; }
    
    [JsonPropertyName("A14_HULL_NUMBER")]
    public string? A14_HULL_NUMBER { get; set; }
    
    [JsonPropertyName("L93_STATUS")]
    public string? L93_STATUS { get; set; }
    
    [JsonPropertyName("E22_CONTRACT_DATE")]
    public string? E22_CONTRACT_DATE { get; set; }
    
    [JsonPropertyName("E28_NEWBUILD_PRICE")]
    public double? E28_NEWBUILD_PRICE { get; set; }
    
    [JsonPropertyName("E29_NEWBUILD_CURRENCY_IND")]
    public string? E29_NEWBUILD_CURRENCY_IND { get; set; }
    
    [JsonPropertyName("Z04_GT_ESTIMATED")]
    public double? Z04_GT_ESTIMATED { get; set; }
    
    [JsonPropertyName("ZR1_BUILT")]
    public string? ZR1_BUILT { get; set; }
    
    [JsonPropertyName("Z92_BUILDER_GROUP_ID")]
    public long? Z92_BUILDER_GROUP_ID { get; set; }
    
    [JsonPropertyName("BUILDER_GROUP")]
    public string? BUILDER_GROUP { get; set; }
    
    [JsonPropertyName("VESSEL_DESCRIPTION")]
    public string? VESSEL_DESCRIPTION { get; set; }
    
    [JsonPropertyName("X03_IMO_NUMBER")]
    public long? X03_IMO_NUMBER { get; set; }
    
    [JsonPropertyName("Z13_STATUS_CODE")]
    public int? Z13_STATUS_CODE { get; set; }
    
    [JsonPropertyName("Z2T_GLOBAL_MAIN_STATUS")]
    public int? Z2T_GLOBAL_MAIN_STATUS { get; set; }
    
    [JsonPropertyName("Z2S_GLOBAL_FLEET_TYPE")]
    public int? Z2S_GLOBAL_FLEET_TYPE { get; set; }
    
    [JsonPropertyName("Z50_MANAGER_ID")]
    public long? Z50_MANAGER_ID { get; set; }
}
