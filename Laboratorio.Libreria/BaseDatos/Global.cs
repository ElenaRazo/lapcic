using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio.Libreria.BaseDatos
{
    public class Global
    {
        public object Conexion { get; set; }

        public string CadenaConexion { get; set; }

        public bool EsReferencia { get; set; }
    }

    public class Parametros
    {
        bool usaTipo = false;

        public Parametros()
        {

        }

        public Parametros(string Nombre, object Valor)
        {
            this.Nombre = Nombre;
            this.Valor = Valor;
        }

        public Parametros(string Nombre, object Valor, int Tipo)
        {
            this.Nombre = Nombre;
            this.Valor = Valor;
            this.Tipo = Tipo;
            usaTipo = true;
        }

        public string Nombre { get; set; }

        public object Valor { get; set; }

        public int Tipo { get; set; }

        public bool UsaTipo
        {
            get
            {
                return usaTipo;
            }
        }
    }
}
