using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pdsr.Core.Extensions;

namespace Pdsr.Data.Extensions;

public static class EFDataMapperExtensions
{
    private const string _relationalColumnNameAnnotation = "Relational:ColumnName";

    /// <summary>
    /// configures an Entity's property to hold SubjectId from database, having 36 characters and fixed length.
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
    /// <param name="applyToForeignKeys">Applies to Foreign Keys if set to true</param>
    /// <param name="applyToIndexes">Applies to Indexes if set to true.</param>
    /// <returns></returns>
    public static IMutableModel ApplySnakeCasingToEntities(this IMutableModel model, string? schema = null, bool applyToTables = true, bool applyToProperties = true, bool applyToPrimaryKeys = true, bool applyToForeignKeys = true, bool applyToIndexes = true)
    {
        foreach (var entity in model.GetEntityTypes())
        {
            if (applyToTables) entity.ApplySnakeCasingToTables(schema);
            if (applyToProperties) entity.ApplySnakeCasingToColumns();
            if (applyToPrimaryKeys) entity.ApplySnakeCasingToPrimaryKeys();
            if (applyToForeignKeys) entity.ApplySnakeCasingToForeignKeys();
            if (applyToIndexes) entity.ApplySnakeCasingToIndexes();
        }
        return model;
    }

    private static void ApplySnakeCasingToTables(this IMutableEntityType types, string? schema = null)
    {
        // if (!Debugger.IsAttached) Debugger.Launch();
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
            var annotationName = GetAnnotationName(property);
            if (annotationName is not null)
            {
                property.SetColumnName(annotationName);
            }
            else
            {
                property.SetColumnName(property.Name.ToSnakeCase());
            }
        }
    }

    private static void ApplySnakeCasingToPrimaryKeys(this IMutableEntityType entity)
    {
        // Ignore JSON mapped entities
        if (entity.IsMappedToJson()) return;

        foreach (var key in entity.GetKeys())
        {
            var keyName = GetAnnotationName(key);
            if (keyName is null)
            {
                keyName = key.GetName()?.ToSnakeCase();
                if (keyName is null)
                {
                    throw new NullReferenceException(nameof(keyName));
                }
            }

            key.SetName(keyName);
        }
    }

    private static void ApplySnakeCasingToForeignKeys(this IMutableEntityType entity)
    {
        foreach (var foreignKey in entity.GetForeignKeys())
        {
            var foreignKeyName = GetAnnotationName(foreignKey);
            if (foreignKeyName is null)
            {
                foreignKeyName = foreignKey.GetConstraintName()?.ToSnakeCase();
                if (foreignKeyName is null)
                {
                    throw new NullReferenceException(foreignKeyName);
                }
            }
            foreignKey.SetConstraintName(foreignKeyName);
        }
    }

    private static void ApplySnakeCasingToIndexes(this IMutableEntityType entity)
    {
        foreach (var index in entity.GetIndexes())
        {
            var indexKeyName = GetAnnotationName(index);
            if (indexKeyName is null)
            {
                indexKeyName = index.GetDatabaseName()?.ToSnakeCase();
                if (indexKeyName is null)
                {
                    throw new NullReferenceException(nameof(indexKeyName));
                }
            }

            index.SetDatabaseName(indexKeyName);
        }
    }

    #region Utils
    private static string? GetAnnotationName(IMutableAnnotatable annotatableObject)
    {
        var annotation = annotatableObject.FindAnnotation(_relationalColumnNameAnnotation);
        if (annotation is not null)
        {
            var annotationName = annotation.Value?.ToString() ?? throw new InvalidOperationException("Cannot get Annotation for object.");
            return annotationName;
        }

        return null;
    }
    #endregion
}
