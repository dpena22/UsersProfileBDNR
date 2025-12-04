using MongoDB.Bson;

public static class BsonExtensions
{
    public static BsonDocument Doc(this BsonValue v)
        => v != null && v.IsBsonDocument ? v.AsBsonDocument : new BsonDocument();

    public static BsonArray Arr(this BsonValue v)
        => v != null && v.IsBsonArray ? v.AsBsonArray : new BsonArray();

    public static object ToPlainObject(this BsonDocument doc)
        => BsonTypeMapper.MapToDotNetValue(doc);
}
