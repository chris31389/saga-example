using MongoDB.Bson.Serialization;

namespace Invoices.Worker.Sagas;

public class OrderSagaDataClassMap : BsonClassMap<OrderSagaData>
{
    public OrderSagaDataClassMap()
    {
    }
}