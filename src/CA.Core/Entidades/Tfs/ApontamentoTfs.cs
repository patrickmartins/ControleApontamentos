using System.Xml.Serialization;

namespace CA.Core.Entidades.Tfs
{
    [XmlType("TimeSheetEntry")]
    public class ApontamentoTfs
    {

        [XmlAttribute("Id")]
        public string? Id { get; set; }

        [XmlIgnore]
        public DateTime? DataApropriacao { get; set; }

        [XmlAttribute("DataApropriacao")]
        public string DataApropriacaoString
        {
            get
            {
                return DataApropriacao.HasValue ? DataApropriacao.Value.ToString("yyyy/MM/dd HH:mm:ss.fff") : string.Empty;
            }
            set
            {
                DataApropriacao = !string.IsNullOrEmpty(value) ? DateTime.Parse(value) : null;
            }
        }

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