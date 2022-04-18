using Pdsr.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pdsr.Data.Extensions;

public static class EFDataMapperExtensions
{
    /// <summary>
    /// configurs an Entity's property to hold SubjectId from database, having 36 characters and fixed length.
    /// </summary>
    /// <param name="builder">the property plan to apply</param>
    public static void IsSubjectId(this PropertyBuilder builder) => builder.HasMaxLength(36).IsRequired().IsFixedLength(true);

    /// <summary>
    /// Converts Model Entities to snake_case
    /// ie: TableName --> table_name
    /// </summary>
    /// <param name="model">The model to apply casing on</param>
    /// <param name="schema">The database schema tables exists on</param>
    /// <param name="applyToTables">Applies to tables if set to true</param>
    /// <param name="applyToProperties">Applies to Properties (columns) if set to true</param>
    /// <param name="applyToPrimaryKeys">Applies to Primary Keys if set to true</param>
    /// <param name="applyToForiegnKeys">Applies to Foreiegn Keys if set to true</param>
    /// <param name="applyToIndexes">Applies to Indexes if set to true.</param>
    /// <returns></returns>
    public static IMutableModel ApplySnakeCasingToEntities(this IMutableModel model, string? schema = null, bool applyToTables = true, bool applyToProperties = true, bool applyToPrimaryKeys = true, bool applyToForiegnKeys = true, bool applyToIndexes = true)
    {
        foreach (var entity in model.GetEntityTypes())
        {
            if (applyToTables) entity.ApplySnakeCasingToTables(schema);
            if (applyToProperties) entity.ApplySnakeCasingToColumns();
            if (applyToPrimaryKeys) entity.ApplySnakeCasingToPrimaryKeys();
            if (applyToForiegnKeys) entity.ApplySnakeCasingToForiegnKeys();
            if (applyToIndexes) entity.ApplySnakeCasingToIndexes();
        }
        return model;
    }

    private static void ApplySnakeCasingToTables(this IMutableEntityType types, string? schema = null)
    {
        var tableName = types.GetTableName();

        var properties = types.GetProperties();
        foreach (var property in properties)
        {
            if (tableName is not null)
                property.GetColumnName(StoreObjectIdentifier.Table(tableName, schema));
        }

        var snakeCasedName = types.GetTableName()?.ToSnakeCase();
        if (snakeCasedName is null)
        {
            throw new NullReferenceException(nameof(snakeCasedName));
        }

        types.SetTableName(snakeCasedName);
    }

    private static void ApplySnakeCasingToColumns(this IMutableEntityType entity)
    {
        foreach (var property in entity.GetProperties())
        {
            property.SetColumnName(property.Name.ToSnakeCase());
        }
    }

    private static void ApplySnakeCasingToPrimaryKeys(this IMutableEntityType entity)
    {
        foreach (var key in entity.GetKeys())
        {
            var snakeCasedName = key.GetName()?.ToSnakeCase();
            if (snakeCasedName is null)
            {
                throw new NullReferenceException(nameof(snakeCasedName));
            }

            key.SetName(snakeCasedName);
        }
    }
    private static void ApplySnakeCasingToForiegnKeys(this IMutableEntityType entity)
    {
        foreach (var foriegnKey in entity.GetForeignKeys())
        {
            var snakeCasedConstaints = foriegnKey.GetConstraintName()?.ToSnakeCase();
            if (snakeCasedConstaints is null)
            {
                throw new NullReferenceException(snakeCasedConstaints);
            }
            foriegnKey.SetConstraintName(snakeCasedConstaints);
        }
    }

    private static void ApplySnakeCasingToIndexes(this IMutableEntityType entity)
    {
        foreach (var index in entity.GetIndexes())
        {
            var snakeCasedIndex = index.GetDatabaseName()?.ToSnakeCase();
            if (snakeCasedIndex is null)
            {
                throw new NullReferenceException(nameof(snakeCasedIndex));
            }

            index.SetDatabaseName(snakeCasedIndex);
        }
    }
}
