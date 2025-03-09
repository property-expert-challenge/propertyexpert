namespace WebApi.Models.Enums;

/// <summary>
/// Represents the different types of property repair and maintenance services that can be classified.
/// </summary>
public enum Classification
{
    /// <summary>
    /// Services related to detecting and fixing water leaks.
    /// </summary>
    WaterLeakDetection,

    /// <summary>
    /// Services for replacing damaged or worn roofing tiles.
    /// </summary>
    RoofingTileReplacement,

    /// <summary>
    /// Services for repairing walls damaged by fire.
    /// </summary>
    FireDamagedWallRepair,

    /// <summary>
    /// Services for repairing or replacing broken doors.
    /// </summary>
    BrokenDoorRepair,

    /// <summary>
    /// Services for waterproofing basement areas.
    /// </summary>
    BasementWaterproofing
}