using System.ComponentModel;
using System.Dynamic;
using FVStorage.Entities;

namespace FVStorage;

public static class HAL
{
    public static dynamic PaginateAsDynamic(string baseUrl, int index, int count, int total)
    {
        dynamic links = new ExpandoObject();
        links.self = new { href = $"{baseUrl}" };
        if (index < total)
        {
            links.next = new { href = $"{baseUrl}?index={index + count}" };
            links.final = new { href = $"{baseUrl}?index={total - (total % count)}&count={count}" };
        }

        if (index > 0)
        {
            links.prev = new { href = $"{baseUrl}?index={index - count}" };
            links.first = new { href = $"{baseUrl}?index=0" };
        }

        return links;
    }

    public static Dictionary<string, object> PaginateAsDictionary(string baseUrl, int index, int count, int total)
    {
        var links = new Dictionary<string, object>();
        links.Add("self", new { href = $"{baseUrl}" });
        if (index < total)
        {
            links["next"] = new { href = $"{baseUrl}?index={index + count}" };
            links["final"] = new { href = $"{baseUrl}?index={total - (total % count)}&count={count}" };
        }

        if (index > 0)
        {
            links["prev"] = new { href = $"{baseUrl}?index={index - count}" };
            links["first"] = new { href = $"{baseUrl}?index=0" };
        }

        return links;
    }

    public static dynamic ToResourceSupply(this Supply supply)
    {
        var resource = supply.ToDynamic();
        resource._links = new
        {
            self = new
            {
                href = $"/api/supplies/{supply.Id}"
            },
            product = new
            {
                href = $"/api/products/{supply.ProductCode}"
            }
        };
        return resource;
    }
    
    public static dynamic ToResourceProduct(this Product product)
    {
        var resource = product.ToDynamic();
        resource._links = new
        {
            self = new
            {
                href = $"/api/products/{product.Code}"
            },
            supplier = new
            {
                href = $"/api/suppliers/{product.SupplierCode}"
            }
        };
        return resource;
    }
    
    public static dynamic ToResourceSupplier(this Supplier supplier)
    {
        var resource = supplier.ToDynamic();
        resource._links = new
        {
            self = new
            {
                href = $"/api/products/{supplier.Code}"
            }
        };
        return resource;
    }

    public static dynamic ToDynamic(this object value)
    {
        IDictionary<string, object> result = new ExpandoObject();
        var properties = TypeDescriptor.GetProperties(value.GetType());
        foreach (PropertyDescriptor prop in properties)
        {
            if (Ignore(prop)) continue;
            result.Add(prop.Name, prop.GetValue(value));
        }

        return result;
    }

    private static bool Ignore(PropertyDescriptor prop)
    {
        return prop.Attributes.OfType<System.Text.Json.Serialization.JsonIgnoreAttribute>().Any();
    }
    
}