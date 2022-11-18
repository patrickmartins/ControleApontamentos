using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace CA.Core.Entidades.Tfs
{
    [XmlType("ArrayOfTimeSheetEntry")]
    public class ListaApontamentos
    {
        [XmlElement("TimeSheetEntry")]
        public List<Apontamento> Apontamentos { get; set; }

        public ListaApontamentos()
        {
            Apontamentos = new List<Apontamento>();
        }

        public TimeSpan ObterTempoTotalApontado()
        {
            return new TimeSpan(Apontamentos.Select(c => TimeSpan.Parse(c.TempoApontamento)).Sum(c => c.Ticks));
        }
    }
}