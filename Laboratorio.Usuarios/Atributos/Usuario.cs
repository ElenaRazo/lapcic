using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio.Usuarios.Atributos
{
    public class MovilMensajeria
    {
        public Notificacion notificacion { get; set; }
        public Data data { get; set; }

        public string Mensaje { get; set; }

        public string to { get; set; }

    }
    public class Notificacion
    {
        public string body { get; set; }

        public string title { get; set; }
        public string content_available { get; set; }
        public string text { get; set; }

    }
    public class Data
    {
        public int Tipo { get; set; }

        public string Mensaje { get; set; }
        public string Cuerpo { get; set; }

    }
    public class UsuarioBasico : UsuarioFisicaBase
    {
        public string Token { get; set; }
        public int IdUsuarioMoral { get; set; }
        public string Razon { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nick { get; set; }
        public string Foto { get; set; }        
        public Estatus Estatus { get; set; }     
        public int Convalidado { get; set; }
    }
    public class UsuarioFoto {
        public int Id { get; set; }
        public string Foto { get; set; }
        public string Nombre { get; set; }
    }
    public class UsuarioKardex : UsuarioFisicaBase
    {
        public int Id { get; set; }
        public Kardex Kardex { get; set; }
        public string Nick { get; set; }
        public string Foto { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public Estatus Estatus { get; set; }
    }
    public class Usuario
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Url { get; set; }
        public string Nick { get; set; }
        public TelefonoUsuario Telefono { get; set; }
        public string Foto { get; set; }

        public string Password { get; set; }
        
        public string Email { get; set; }
        public string Observacion { get; set; }

        public TipoUsuario TipoUsuario { get; set; }

        public ClasificacionUsuario ClasificacionUsuario { get; set; }

        public ClasificacionIdentidad ClasificacionIdentidad {get;set;}

        public string Identificacion { get; set; }
        public DomicilioUsuario Domicilio { get; set; }

        public Estatus Estatus { get; set; }    
        public string Token { get; set; }
    }
    public class UsuarioBase : Usuario{
        public UsuarioFisicaBase Fisica { get; set; }
        public UsuarioMoralBase Moral { get; set; }
    }
    public class Estatus
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }
    }
    public class TiposUsuario
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }
    }

    public class ClasificacionIdentidad
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public bool Disponible { get; set; }
    }

    public class UsuarioMoral
    {
        public int IdUsuarioMoral { get; set; }
        public Usuario Usuario { get; set; }

        public int /*bool*/ TipoMoral { get; set; }

        public string Razon { get; set; }

        public string Nombre { get; set; }

        public string Paterno { get; set; }

        public string Materno { get; set; }
        public string Observacion { get; set; }
    }
    public class UsuarioMoralBase
    {
        public int IdUsuarioMoral { get; set; }

        public TipoMoral TipoMoral { get; set; }

        public string Razon { get; set; }

        public string Nombre { get; set; }

        public string Paterno { get; set; }

        public string Materno { get; set; }
    }
    public enum TipoMoral
    {
        MORAL = 1,
        INSTITUCION = 2
    }
    public class UsuarioFisica
    {
        public int IdUsuarioFisica { get; set; }
        public string Nombre { get; set; }
        public Usuario Usuario { get; set; }
        public Kardex Kardex { get; set; }

        public string Paterno { get; set; }

        public string Materno { get; set; }

        public int Genero { get; set; }
    }

    public class UsuarioFisicaBase
    {
        public int IdUsuarioFisica { get; set; }
        public string Nombre { get; set; }

        public string Paterno { get; set; }

        public string Materno { get; set; }

        public int Genero { get; set; }
    }

    public class ClasificacionCuenta
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public bool Disponible { get; set; }
    }
    public class Acceso
    {
        public long Id { get; set; }

        public Usuario Usuario { get; set; }

        public int Plataforma { get; set; }

        public ClasificacionCuenta ClasificacionCuenta { get; set; }
        
        public int Oficina { get; set; }

        public DateTime Fecha { get; set; }
       
        public int Referencia { get; set; }

        public bool Disponible { get; set; }
    }

    public class CuentaNivel
    {
        public int Id { get; set; }

        public ClasificacionCuenta ClasificacionCuenta { get; set; }

        public bool Disponible { get; set; }
    }

    public class Puesto
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
       
        public ClasificacionCuenta ClasificacionCuenta { get; set; }
        public bool Disponible { get; set; }
    }

    public class Sesion
    {
        public long Id { get; set; }

        public Usuario Usuario { get; set; }

        public string Token { get; set; }

        public DateTime Fecha { get; set; }

        public bool Dispositivo { get; set; }

        public string Tag { get; set; }

        public string Nombre { get; set; }

        public bool Disponible { get; set; }
    }

    public class Kardex
    {
        public int Id { get; set; }

        public Puesto Puesto { get; set; }
        
        public DateTime Fecha { get; set; }
        
        public Descriptores.Atributos.OficinaBase Oficina { get; set; }

        public ClasificacionCuenta ClasificacionCuenta { get; set; }
       
        public long Archivo { get; set; }

        public List<Modulo> Modulos { get; set; }
        public Estatus Estatus { get; set; }
    }

    public class DomicilioUsuario
    {
        public int Id { get; set; }

        public string Calle { get; set; }
       

        public string Codigo { get; set; }

        public int NumeroInt { get; set; }

        public int NumeroExt { get; set; }

        public string Referencia { get; set; }
        public string Puntos { get; set; }
        public Colonia Colonia { get; set; }

        public Descriptores.Atributos.Municipio Municipio { get; set; }

        public bool Disponible { get; set; }

        public int Usuario { get; set; }
    }
    public class Colonia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
    public class TelefonoUsuario
    {
        public int Id { get; set; }
        

        public Descriptores.Atributos.ClasificacionTelefono ClasificacionTelefono { get; set; }

        public string Numero { get; set; }

        public bool Disponible { get; set; }
    }
    public class Modulo
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        public int IdKardexModulo { get; set; }

        public bool Activo { get; set; }
    }
    public class ModuloKardex
    {
        public int Id { get; set; }
        public Modulo Modulo { get; set; }
        public Kardex kardex { get; set; }
        public bool Disponible { get; set; }
    }
    public class ModuloUsuario
    {
        public int Id { get; set; }
        public Modulo Modulo { get; set; }
        public Usuario Usuario { get; set; }
        public bool Disponible { get; set; }
    }
    public enum ClasificacionUsuario {
        INTERNO = 1,
        SERVICIOS = 2,
        EXTERNO = 3
    }
    public enum TipoUsuario
    {
        FISICO = 1,
        MORAL = 2,
        INSTITUCION = 3
    }
}
