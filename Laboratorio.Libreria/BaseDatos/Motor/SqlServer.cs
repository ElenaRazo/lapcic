using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Laboratorio.Libreria.BaseDatos.Motor
{
    public class SqlServer : Global, IBaseDatos
    {
        public void Conectar()
        {
            this.Conexion = new SqlConnection(this.CadenaConexion);
            ((SqlConnection)this.Conexion).Open();
        }

        public void Conectar(string CadenaConexion)
        {
            this.CadenaConexion = CadenaConexion;
            this.Conexion = new SqlConnection(CadenaConexion);
            ((SqlConnection)this.Conexion).Open();
        }

        public void Conectar(object Conexion)
        {
            this.Conexion = Conexion;
        }

        public void Desconectar()
        {
            ((SqlConnection)this.Conexion).Close();
        }

        public void Dispose()
        {
            try
            {
                ((SqlConnection)this.Conexion).Dispose();
            }
            catch { /*do nothing*/ }
        }

        public DataSet EjecutarColeccionQuery(string Query, List<object> Parametros)
        {
            throw new NotImplementedException();
        }

        public DataSet EjecutarColeccionStoreProcedure(string Query, List<object> Parametros)
        {
            throw new NotImplementedException();
        }

        public void EjecutarNonQuery(string Query, List<object> Parametros)
        {
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = ((SqlConnection)this.Conexion);
                cmd.CommandText = Query;
                if (Parametros != null)
                {
                    cmd.Parameters.AddRange(Parametros.ToArray());
                }
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable EjecutarQuery(string Query, List<object> Parametros)
        {
            DataTable _tmp = new DataTable();
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = ((SqlConnection)this.Conexion);
                cmd.CommandText = Query;
                if (Parametros != null)
                {
                    cmd.Parameters.AddRange(Parametros.ToArray());
                }
                using (var data = new SqlDataAdapter(cmd))
                {
                    data.Fill(_tmp);
                }
            }
            return _tmp;
        }

        public DataTableReader EjecutarReaderQuery(string Query, List<object> Parametros)
        {
            DataTableReader _tmp;
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = ((SqlConnection)this.Conexion);
                cmd.CommandText = Query;
                if (Parametros != null)
                {
                    foreach (var par in Parametros)
                    {
                        cmd.Parameters.AddRange(Parametros.ToArray());
                    }
                }
                using (var reader = cmd.ExecuteReader())
                {
                    using (var dt = new DataTable())
                    {
                        dt.Load(reader);
                        _tmp = dt.Clone().CreateDataReader();
                    }
                }
            }
            return _tmp;
        }

        public DataTableReader EjecutarReaderStoreProcedure(string Query, List<object> Parametros)
        {
            DataTableReader tmp;
            using (var command = ((SqlConnection)this.Conexion).CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = Query;
                if (Parametros != null)
                {
                    command.Parameters.AddRange(Parametros.ToArray());
                }
                using (var dr = command.ExecuteReader())
                {
                    using (var dt = new DataTable())
                    {
                        dt.Load(dr);
                        tmp = dt.CreateDataReader();
                    }
                }
                if (Parametros != null)
                {
                    Parametros.Clear();
                    Parametros = null;
                }
            }
            return tmp;
        }

        public DataTable EjecutarDataStoreProcedure(string Query, List<object> Parametros)
        {
            DataTable tmp;
            using (var command = ((SqlConnection)this.Conexion).CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = Query;
                if (Parametros != null)
                {
                    command.Parameters.AddRange(Parametros.ToArray());
                }
                using (var dr = command.ExecuteReader())
                {
                    using (var dt = new DataTable())
                    {
                        dt.Load(dr);
                        tmp = dt;
                    }
                }
                if (Parametros != null)
                {
                    Parametros.Clear();
                    Parametros = null;
                }
            }
            return tmp;
        }

        public void EjecutarStoreProcedure(string Query, List<object> Parametros)
        {
            using (var command = ((SqlConnection)this.Conexion).CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = Query;
                if (Parametros != null)
                {
                    command.Parameters.AddRange(Parametros.ToArray());
                }
                command.ExecuteNonQuery();
            }
        }

        public void EstablecerCadenaConexion(string CadenaConexion)
        {
            this.CadenaConexion = CadenaConexion;
        }

        public void EstablecerConexion(object Conexion)
        {
            this.Conexion = Conexion;
        }

        public object ObtenerConexion()
        {
            return Conexion;
        }

        public bool ObtenerEsreferencia() {
            return this.EsReferencia;
        }

        public void EstablecerEsreferencia(bool Valor)
        {
             this.EsReferencia= Valor;
        }
    }
}
