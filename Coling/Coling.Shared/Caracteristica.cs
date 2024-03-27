using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class Caracteristica
    {
        [BsonElement("nombre"), BsonRepresentation(BsonType.String)]
        public string? nombre { get; set; }
    }
}
