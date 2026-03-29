using System.Reflection;
using System.Text.Json;
using Bicep.Extension.Netbox;
using Bicep.Local.Extension.Types.Attributes;

namespace Bicep.Extension.Netbox.Tests;

/// <summary>
/// Validates all resource type models have correct attributes and structure.
/// Catches issues like missing [ResourceType], wrong identifiers, or unsupported types.
/// </summary>
public class ModelValidationTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    private static IEnumerable<Type> GetAllResourceTypes()
    {
        return typeof(Configuration).Assembly
            .GetTypes()
            .Where(t => t.GetCustomAttribute<ResourceTypeAttribute>() != null);
    }

    [Fact]
    public void All_ResourceTypes_Have_At_Least_One_TypeProperty()
    {
        foreach (var type in GetAllResourceTypes())
        {
            var allProps = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var hasTypeProperty = allProps.Any(p => p.GetCustomAttribute<TypePropertyAttribute>() != null);
            Assert.True(hasTypeProperty, $"{type.Name} has no properties with [TypeProperty]");
        }
    }

    [Fact]
    public void No_Nullable_Value_Types_In_Models()
    {
        var unsupportedTypes = new HashSet<Type> { typeof(int?), typeof(bool?), typeof(double?), typeof(long?) };

        foreach (var type in GetAllResourceTypes())
        {
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Assert.False(unsupportedTypes.Contains(prop.PropertyType),
                    $"{type.Name}.{prop.Name} uses unsupported nullable type {prop.PropertyType}. Use string? instead.");
            }
        }
    }

    [Fact]
    public void No_JsonPropertyName_Attributes_On_Models()
    {
        foreach (var type in GetAllResourceTypes())
        {
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var hasJsonAttr = prop.GetCustomAttributes()
                    .Any(a => a.GetType().Name == "JsonPropertyNameAttribute");
                Assert.False(hasJsonAttr,
                    $"{type.Name}.{prop.Name} has [JsonPropertyName] — remove it. Use SnakeCaseLower naming policy instead.");
            }
        }
    }

    [Fact]
    public void All_ResourceTypes_Serialize_To_Lowercase_SnakeCase_Json()
    {
        foreach (var type in GetAllResourceTypes())
        {
            var instance = Activator.CreateInstance(type);
            Assert.NotNull(instance);

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.PropertyType == typeof(string) && prop.CanWrite)
                    prop.SetValue(instance, "test");
                else if (prop.PropertyType == typeof(int) && prop.CanWrite)
                    prop.SetValue(instance, 1);
            }

            var json = JsonSerializer.Serialize(instance, type, JsonOptions);
            Assert.NotNull(json);
            Assert.NotEmpty(json);

            var doc = JsonDocument.Parse(json);
            foreach (var property in doc.RootElement.EnumerateObject())
            {
                Assert.True(property.Name == property.Name.ToLowerInvariant(),
                    $"Property '{property.Name}' in {type.Name} is not lowercase — snake_case naming policy may not be applied");
            }
        }
    }

    [Fact]
    public void At_Least_25_Resource_Types_Exist()
    {
        var count = GetAllResourceTypes().Count();
        Assert.True(count >= 25, $"Expected at least 25 resource types but found {count}");
    }
}
