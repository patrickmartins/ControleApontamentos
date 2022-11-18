using System;
using System.Xml.Serialization;

namespace CA.Core.Entidades.Tfs
{
    [XmlType("TimeSheetEntry")]
    public class Apontamento
    {
        [XmlAttribute("CreatedDate")]
        public DateTime DataCriacao { get; set; }

        [XmlAttribute("ApropriadoChannel")]
        public bool SincronizadoChannel { get; set; }

        [XmlAttribute("CreatedBy")]
        public string Usuario { get; set; } = string.Empty;

        [XmlAttribute("TimeSheetDate")]
        public string DataApontamento { get; set; } = string.Empty;

        [XmlAttribute("Minutes")]
        public string TempoApontamento { get; set; } = string.Empty;

        [XmlAttribute("Comments")]
        public string Comentario { get; set; } = string.Empty;
    }
}