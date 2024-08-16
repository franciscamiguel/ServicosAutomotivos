using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiçosAutomotivos
{
    public class Servico
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Custo { get; set; }
        public TimeSpan TempoEstimado { get; set; }
    }
}
