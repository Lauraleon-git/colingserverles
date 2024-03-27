using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    internal class OfertaLaboral
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("idInstitucion"), BsonRepresentation(BsonType.String)]
        public string IdInstitucion { get; set; } = null!;

        [BsonElement("fechaOferta"), BsonRepresentation(BsonType.DateTime)]
        public DateTime FechaOferta { get; set; }

        [BsonElement("fechaLimite"), BsonRepresentation(BsonType.DateTime)]
        public DateTime FechaLimite { get; set; }

        [BsonElement("descripcion"), BsonRepresentation(BsonType.String)]
        public string Descripcion { get; set; } = null!;

        [BsonElement("tituloCargo"), BsonRepresentation(BsonType.String)]
        public string? TituloCargo { get; set; }
        [BsonElement("tipoContrato"), BsonRepresentation(BsonType.String)]
        public string TipoContrato { get; set; } = null!;
        [BsonElement("tipoTrabajo"), BsonRepresentation(BsonType.String)]
        public string TipoTrabajo { get; set; } = null!;
        [BsonElement("area"), BsonRepresentation(BsonType.String)]
        public string Area { get; set; } = null!;

        [BsonElement("aniosExperiencia"), BsonRepresentation(BsonType.Int32)]
        public int AniosExperiencia { get; set; }

        [BsonElement("caracteristicas")]
        public List<Caracteristica>? Caracteristicas { get; set; }

        [BsonElement("estado"), BsonRepresentation(BsonType.String)]
        public string Estado { get; set; } = null!;
    }
}
