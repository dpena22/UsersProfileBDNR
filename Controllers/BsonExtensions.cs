using MongoDB.Bson;
using System.Text.Json;

public static class BsonExtensions
{
    public static object ToPlainObject(this BsonDocument bson)
    {
        if (bson == null) return new { };

        string json = bson.ToJson();

        return JsonSerializer.Deserialize<object>(json)!;
    }
}
