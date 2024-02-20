using Microsoft.EntityFrameworkCore;

namespace Pdsr.Data;

/// <summary>
/// Represents database context model mapping
/// </summary>
public interface IMappingConfiguration
{
    /// <summary>
    /// Apply the mapping configuration
    /// </summary>
    /// <param name="modelBuilder">Model builder to apply config to</param>
    void ApplyConfiguration(ModelBuilder modelBuilder);
}
