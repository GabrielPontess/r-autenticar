using RAutenticar;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAutenticar.Models
{
    public class DadosValidacao
    {
        [PrimaryKey][AutoIncrement]
        public long Id { get; set; }
        public Atendente Atendente { get; set; }
        public String DispositivoCaptura { get; set; }
        public DateTime DataCaptura { get; set; }
        public DateTime HoraCaptura { get; set; }

    }
}
