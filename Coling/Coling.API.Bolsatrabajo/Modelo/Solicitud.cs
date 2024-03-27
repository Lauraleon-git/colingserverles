using Coling.Shared;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Bolsatrabajo.Modelo
{
   public class Solicitud : ISolicitud
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int? idAfiliado { get ; set; }
        public string? idOferta { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public DateTime FechaPostulacion { get; set; }
        public double PretencionSalarial { get; set; }
        public string? AcercaDe { get; set; }
        public string? Curriculum { get; set; }
        public string? Estado { get; set; } = null;



    }
}
