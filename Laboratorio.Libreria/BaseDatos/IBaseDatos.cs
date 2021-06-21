using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio.Libreria.BaseDatos
{
    public interface IBaseDatos : IDisposable
    {
        void Conectar();

        void Conectar(string CadenaConexion);

        void Conectar(object Conexion);

        object ObtenerConexion();

        void EjecutarNonQuery(string Query, List<object> Parametros);

        DataTable EjecutarQuery(string Query, List<object> Parametros);

        DataTableReader EjecutarReaderQuery(string Query, List<object> Parametros);

        DataSet EjecutarColeccionQuery(string Query, List<object> Parametros);

        DataTable EjecutarDataStoreProcedure(string Query, List<object> Parametros);

        void EjecutarStoreProcedure(string Query, List<object> Parametros);

        DataTableReader EjecutarReaderStoreProcedure(string Query, List<object> Parametros);

        DataSet EjecutarColeccionStoreProcedure(string Query, List<object> Parametros);

        void EstablecerConexion(object Conexion);

        void EstablecerCadenaConexion(string CadenaConexion);

        bool ObtenerEsreferencia();

        void EstablecerEsreferencia(bool Valor);

        void Desconectar();

    }
}
