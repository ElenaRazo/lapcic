using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio.Libreria.BaseDatos
{
    public static class Factorizador
    {
        public static IBaseDatos CrearBaseDatos(DBMotor Motor, string CadenaConexion)
        {
            IBaseDatos nuevoMotor;
            switch (Motor)
            {
                case DBMotor.SQLSERVER:
                    nuevoMotor = new Motor.SqlServer();
                    nuevoMotor.Conectar(CadenaConexion);
                    break;
                default:
                    nuevoMotor = new Motor.SqlServer();
                    nuevoMotor.Conectar(CadenaConexion);
                    break;
            }
            return nuevoMotor;
        }
        public static IBaseDatos CrearBaseDatos(DBMotor Motor, object Conexion)
        {
            IBaseDatos nuevoMotor;
            switch (Motor)
            {
                case DBMotor.SQLSERVER:
                    nuevoMotor = new Motor.SqlServer();
                    nuevoMotor.EstablecerConexion(Conexion);
                    break;
                default:
                    nuevoMotor = new Motor.SqlServer();
                    nuevoMotor.EstablecerConexion(Conexion);
                    break;
            }
            return nuevoMotor;
        }
        public enum DBMotor {
            SQLSERVER=1,
            PGSQL=2
        }

    }
}
