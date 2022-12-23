using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAutenticar.Models
{
    public class Atendente
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public String Cpf { get; set; }
        public byte[] Imagem { get; set; }
        public string ImagemBase64 { get; set; }
    }
}
