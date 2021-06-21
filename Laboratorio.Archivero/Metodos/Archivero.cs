using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Laboratorio.Medios.Atributos;
using Laboratorio.Libreria.BaseDatos;

namespace Laboratorio.Medios.Metodos
{
    public class Archivero : Libreria.BaseClass.BaseObject
    {
        public Archivero(string ConnectionString) : base(ConnectionString)
        {

        }

        public Archivero(IBaseDatos Conexion) : base(Conexion)
        {

        }
        public long CrearMedio(Atributos.Items.Medio Medio)
        {
            long idMedio = 0;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Archivo_Id", Medio.Archivo));
                prmtrs.Add(new SqlParameter("@Esquema", Medio.Esquema));
                prmtrs.Add(new SqlParameter("@Tamanio", Medio.Tamanio));
                prmtrs.Add(new SqlParameter("@Checksum", Medio.CheckSum));
                prmtrs.Add(new SqlParameter("@MedioTipo_Id", Medio.MedioTipo.Id));
                prmtrs.Add(new SqlParameter("@EnDirectorio", Medio.EnDirectorio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medio_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        idMedio = Resultado.GetInt64(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return idMedio;
        }
    }
}
