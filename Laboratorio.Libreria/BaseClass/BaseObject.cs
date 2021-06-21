using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laboratorio.Libreria.BaseDatos;

namespace Laboratorio.Libreria.BaseClass
{
    public class BaseObject : IDisposable
    {
        protected IBaseDatos conexion;

        public BaseObject(string ConnectionString)
        {
            conexion = Factorizador.CrearBaseDatos(Factorizador.DBMotor.SQLSERVER, ConnectionString);
        }

        public BaseObject(IBaseDatos Conexion)
        {
            conexion = Factorizador.CrearBaseDatos(Factorizador.DBMotor.SQLSERVER, Conexion.ObtenerConexion());
            this.conexion.EstablecerEsreferencia(true);
        }

        public void Dispose()
        {
            if (conexion != null && !this.conexion.ObtenerEsreferencia())
            {
                conexion.Dispose();
            }
        }
    }
}
