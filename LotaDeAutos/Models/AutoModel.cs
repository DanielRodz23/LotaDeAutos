using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotaDeAutos.Models
{
    public class AutoModel
    {
        public int Id { get; set; }
        public string Marca { get; set; } = null!;
        public string Modelo { get; set; } = null!;
        public string Version { get; set; } = null!;
        public short Anio { get; set; }
        public decimal Precio { get; set; }
        public int Kilometraje { get; set; }
        public string Motor { get; set; } = null!;
        public string Transmicion { get; set; } = null!;
        public string Carroceria { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
    }
}
