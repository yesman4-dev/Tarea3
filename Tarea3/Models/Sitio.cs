using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tarea3.Models
{
    internal class Sitio
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Edad { get; set; }
        public string Direccion { get; set; }
        public string Puesto { get; set; }
        public byte[] save_image { get; set; }

        public override string ToString()
        {
            return this.Nombre + " | " + this.Apellidos + " " + this.Edad + " " + this.Direccion + " " + this.Puesto + " " + save_image;
        }
    }
}
