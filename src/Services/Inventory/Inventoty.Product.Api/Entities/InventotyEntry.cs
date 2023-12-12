using Contract.Domain;
using Infrastructures.Extentions;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Enums.Inventory;

namespace Inventoty.Product.Api.Entities
{
    [BsonCollection("InventotyEntries")]
    public class InventotyEntry : MongoEntity
    {
        public InventotyEntry()
        {
            DocumentType = EDocumentType.Purchase;
            DocumentNo = Guid.NewGuid().ToString();
            ExternalDocmentNo = Guid.NewGuid().ToString();
        }
        public InventotyEntry(string id) => (Id) = id;
        [BsonElement("documentType")]
        public EDocumentType DocumentType { get; set; }
        [BsonElement("documentNo")]
        public string DocumentNo { get; set; }
        [BsonElement("itemNo")]
        public string ItemNo { get; set; }
        [BsonElement("quantity")]
        public int Quantity { get; set; }
        [BsonElement("externalDocmentNo")]
        public string ExternalDocmentNo { get; set; }
    }
}
