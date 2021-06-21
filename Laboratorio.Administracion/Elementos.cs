using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio.Administracion
{
    public class Notificacion
    {
        public string body { get; set; }

        public string title { get; set; }
        public bool content_available { get; set; }
        public string text { get; set; }

    }
    public class Etiqueta {
        public string clave { get; set; }
        public long solicitud { get; set; }
    }
    public class Chekin
    {
        public string huella { get; set; }
        public long idusuario { get; set; }
        public int idlaboratorio { get; set; }
        public string maquina { get; set; }
        public string ip { get; set; }
    }
    public class Data
    {
        public int Tipo { get; set; }

        public string Mensaje { get; set; }
        public string Cuerpo { get; set; }

    }
    public class MovilMensajeria
    {
        public Notificacion notificacion { get; set; }
        public Data data { get; set; }
        public string priority { get; set; }

        public string Mensaje { get; set; }
        public notification notification { get; set; }
        public string to { get; set; }

    }
    public class notification
    {
        public bool content_available { get; set; }
        public string body { get; set; }
        public string title { get; set; }
    }
    public enum TipoUrgencia {
        NORMAL =0,
        DIAS =1,
        MAÑANA =2,
        HOY = 3,
        URGENCIA = 4
    }
    public class Solicitud {
        public TipoUrgencia TipoUrgencia { get; set; }
        public string ObservacionUrgencia { get; set; }
        public int Factura { get; set; }
        public string DatosFactura { get; set; }
        public long Id { get; set; }
        public bool Pagado { get; set; }
        public int Consecutivo { get; set; }
        public string Edad { get; set; }
        public Organizacion Empresa { get; set; }
        public bool Urgencia { get; set; }
        public Double CostoUrgencia { get; set; }
        public string Observaciones { get; set; }
        public string Folio { get; set; }
        public string ClaveSolicitud { get; set; }
        public Usuario Medico { get; set; }
        public Laboratorio Laboratorio { get; set; }
        public Usuario Usuario { get; set; }
        public Usuario Paciente { get; set; }
        public List<EstudioResultado> Estudios { get; set; }
        public Double Total { get; set; }
        public Double SubTotal { get; set; }
        public Double Acuenta { get; set; }
        public Double PorCubrir { get; set; }
        public Double Debe { get; set; }
        public Double Descuento { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaEntrega { get; set; }
        public EstatusSolicitud EstatusSolicitud { get; set; }
        public List<Pago> Pagos { get; set; }
    }
    public class Cotizacion {
        public string ClaveCotizacion { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public DateTime Fecha { get; set; }
        public int Consecutivo { get; set; }
        public Usuario Usuario { get; set; }
        public Laboratorio Laboratorio { get; set; }
        public List<EstudioResultado> Estudios { get; set; }
       
    }
    public class MovilRespuesta
    {
        public string Metodo { get; set; }

        public string Parametros { get; set; }

        public object Respuesta { get; set; }

        public ResponseFlag ResponseFlag { get; set; }

        public string Traza { get; set; }

    }
    public enum ResponseFlag
    {
        OK = 1,
        FAIL = 2,
        TIMEOUT = 3,
        SESION_INVALIDA = 4,
        EXPEDIENTE_NOENCONTRADO = 5
    }
    public class MovilLogin
    {
        public string Usuario { get; set; }
        public int Id { get; set; }
        public string Password { get; set; }
    }
    public class MovilContrasena
    {
        public int Identificador { get; set; }
        public string Pass { get; set; }
    }
    public class MovilDetalle
    {
        public int Id { get; set; }
        public int Anio { get; set; }
        public string ClaveSolicitud { get; set; }
        public string Nombre { get; set; }
    }
    public class MovilCotizacion
    {
        public string Estudios { get; set; }
        public string Email { get; set; }
        public string Materno { get; set; }
        public string Paterno { get; set; }
        public string Nombre { get; set; }
    }
    public class MovilLoginToken
    {
        public string Usuario { get; set; }

        public string Password { get; set; }
        public string Token { get; set; }
    }
    public class Pago {
        public long Id { get; set; }
        public string ClaveSolicitud { get; set; }
        public string ClavePago { get; set; }
        public string Adicional { get; set; }
        public string Observacion { get; set; }
        public DateTime Fecha { get; set; }
        public Double Monto { get; set; }
    }
    public class DepositoBusqueda
    {
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public int NumeroMuestras { get; set; }
        public int Total { get; set; }
    }
    public class EstudioGabinete : Solicitud {
        public Usuario MedicoInterpretacion { get; set; }
        public string Observaciones { get; set; }
        public string Resultado { get; set; }
        public Organizacion Organizacion { get; set; }
        public List<string> Adjunto { get; set; }
        public List<EstudioGabineteDetalle> Estudios { get; set; }
        public TipoEstudioGabinete TipoEstudio { get; set; }
        public bool Interpretacion { get; set; }
        public Estudio TipoSolicitud { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime FechaResultado { get; set; }
        public string ObservacionesTecnico { get; set; }
        public EstatusSolicitudGabinete EstatusSolicitud { get; set; }
        public bool Activo { get; set; }
    }
    public class EstudioSolicitud : Solicitud
    {
        public EstatusSolicitud EstatusSolicitud { get; set; }
        public bool Activo { get; set; }
    }
    public enum TipoEstudioGabinete {
        SINIDENTIFICAR=0,
        RADIOLOGIA=1,
        ULTRASONIDO=2,
        AMBOS=3
    }
    public enum EstatusSolicitudGabinete
    {
        SOLICITADO = 1,
        _SOLICITADO = 2, //pagado
        _ENPROCESO = 3, //pagado
        ENPROCESO = 4,
        _RESULTADO = 5, //pagado
        RESULTADO = 6
    }
    public enum EstatusSolicitud
    {
        /* SOLICITADOSINPAGAR = 1,
         SOLICITADOPAGADO = 2,
         ENPROCESOPAGADO = 3,
         ENPROCESO = 4,
         RESULTADOPAGADO = 5,
         RESULTADO = 6,
         FINALIZADOSINCAPTURA = 7,
         ENTREGADOALCLIENTE =8,
         RESULTADOSPARCIALES = 9*/
        SOLICITADO = 1,
        ENPROCESO = 2,
        ESTUDIOFINALIZADOSINCAPTURADERESULTADOS  = 3,
        RESULTADOCAPTURADO = 4,
        ENTREGADOALCLIENTE = 5,
        RESULTADOSPARCIALES = 6

    }
    public enum TipoSolicitud {
        RADIOLOGIA = 1,
        ULTRASONIDO = 2,
        ULTRASONIDO2 = 3,
    }
    public class Detalle {
        public List<Componente> Componente { get; set; }
        public List<Elemento> Elementos { get; set; }
    }
    public class Estadistica {

    }
    public class Laboratorio {
        public string Responsable { get; set; }
        public Usuario UsuarioResponsable { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Iniciales { get; set; }
        public int Id { get; set; }
        public string Slogan { get; set; }
        public string Logotipo { get; set; }
    }
    public class Elemento {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Orden { get; set; }
    }
    public class Componente {
        public int N { get; set; }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ClaveEstudio { get; set; }
        public string Abreviatura { get; set; }
        public string Unidad { get; set; }
        public string Orden { get; set; }
        public double MinG { get; set; }
        public double MaxG { get; set; }
        public double MinH { get; set; }
        public double MaxH { get; set; }
        public double MinM { get; set; }
        public double MaxM { get; set; }
        public double MinN { get; set; }
        public double MaxN { get; set; }
        public double MinRN { get; set; }
        public double MaxRN { get; set; }
        public string TextoNormalGeneral { get; set; }
        public string TextoNormalHombres { get; set; }
        public string TextoNormalMujeres { get; set; }
        public string TextoNormalNiños { get; set; }
        public string TextoNormalRecienNacido { get; set; }
        public bool UsarValoresGenerales { get; set; }
        public bool PermiteDesfaseValores { get; set; }
        public string Indicaciones { get; set; }
        public TipoCaptura TipoCaptura { get; set; }
        public List<Elemento> Elementos { get; set; }
        public ResultadoSolicitud Resultado { get; set; }
    }
    public class ComponenteResultado: Componente{

    }
    public class EstudioGabineteDetalle : Estudio {
        public long IdDetalle { get; set; }
        public string Observaciones { get; set; }
        public string Resultado { get; set; }
        public List<string> Adjunto { get; set; }
        public Estudio TipoSolicitud { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime FechaResultado { get; set; }
        public string ObservacionesTecnico { get; set; }
        public Usuario MedicoInterpretacion { get; set; }
    }
    public class Estudio
    {
        public int Id { get; set; }
        public Departamento Departamento { get; set; }
        public string ClaveEstudio { get; set; }
        public TipoDeposito TipoDeposito { get; set; }
        public TipoMuestra TipoMuestra { get; set; }
        public string Abreviatura { get; set; }
        public string Nombre { get; set; }
        public double PrecioBase { get; set; }
        public int Urgencia { get; set; }
        public bool PermiteDescuento { get; set; }
        public double TiempoProceso { get; set; }
        public string Indicaciones { get; set; }
        public string UnidadProceso { get; set; }
        public bool Perfil { get; set; }
        public int NumeroMuestras { get; set; }
        public double Volumen { get; set; }
        public string UnidadMuestra { get; set; }
        public bool HojaNueva { get; set; }
    }
    public class ResultadoSolicitud {
        public long IdResultado { get; set; }
        public string NombreEstudio { get; set; }
        public string Observaciones { get; set; }
        public int EstudioComponente { get; set; }
        public string TipoCaptura { get; set; }
        public string ClaveSolicitud { get; set; }
        public string ClaveEstudio { get; set; }
        public string ClaveEstudioMain { get; set; }
        public string Resultado { get; set; }
        public string Capturado { get; set; }
        public string Titulo { get; set; }
        public string Referencia { get; set; }
        public string LineasImpresion { get; set; }
        public string Unidad { get; set; }
        public string Normal { get; set; }
        public bool Imprimir { get; set; }
        public bool FueraRango { get; set; }
        public string MAX { get; set; }
        public string MIN { get; set; }
        public string Ruta { get; set; }
        public bool Negrita { get; set; }
    }
    public class EstudioResultado : Estudio {
        public long IdDetalle { get; set; }
        public int Edad { get; set; }
        public string Genero { get; set; }
        public List<Componente> Componentes { get; set; }
    }
    public class TipoDeposito
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Entrega { get; set; }
        public string Indicaciones { get; set; }
        public double Existencia { get; set; }
    }
    public class Movimiento
    {
        public long Id { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }
        public TipoMovimiento TipoMovimiento { get; set;}
    }
public class TipoMovimiento
{
    public int Id { get; set; }
    public string Nombre { get; set; }
}
public class TipoMuestra
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Indicaciones { get; set; }
    }
    public class Muestra
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
    public class MuestraImpresion
    {
        public int Id { get; set; }
        public string Muestra { get; set; }
        public string Paciente { get; set; }
        public string TipoDeposito { get; set; }
        public string Edad { get; set; }
        public string Solicitud { get; set; }
        public string Descripcion { get; set; }
        public bool Modificar { get; set; }
        public int EsPerfil { get; set; }
        public int Numero { get; set; }
    }
    public class TipoPago
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Clave { get; set; }
    }
    public class Organizacion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Colonia { get; set; }
        public string Ciudad { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public bool Precios { get; set; }
        public bool Pagos { get; set; }
    }
    public class Insumo
    {
        public int Id { get; set; }
        public Proveedor Proveedor { get; set; }
        public TipoDeposito TipoDepoosito { get; set; }
        public int Cantidad { get; set; }
        public string Unidad { get; set; }
        public int TotalPiezas { get; set; }
        public string Costo { get; set; }
        public string CodigoBarras { get; set; }
        public DateTime FechaCompra { get; set; }
        public bool Activo { get; set; }
        public int Utilizados { get; set; }
    }
    public class Proveedor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
    public class Departamento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
    public class Ciudad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public Estado Estado { get; set; }
    }
    public class Estado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
    public class Municipio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Clave { get; set; }
        public Estado Estado { get; set; }
    }
    public class Colonia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string CodigoPostal { get; set; }
    }
    public class TipoCaptura
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Rfc { get; set; }
        public string Materno { get; set; }
        public string Curp { get; set; }
        public string Genero { get; set; }
        public int Edad { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaAlta { get; set; }
        public string Foto { get; set; }
        public string Nick { get; set; }
        public string NumeroSeguroSocial { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }

        public string Direccion { get; set; }
        public Colonia Colonia { get; set; }
        public Ciudad Ciudad { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        public string Observacion { get; set; }

        public TipoUsuario TipoUsuario { get; set; }
        public string Token { get; set; }
        public int Laboratorio { get; set; }
        public Laboratorio LaboratorioCompleto { get; set; }
        public Profesion Profesion { get; set; }
        public EstadoCivil Civil { get; set; }
        public Puesto Puesto { get; set; }
    }

    public class Puesto
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
    }
    public enum TipoUsuario
    {
        PACIENTE = 1,
        USUARIO = 2,
        MEDICO = 3
    }
    public enum EstadoCivil
    {
        Soltero = 1,
        Casado = 2
    }
     public enum Profesion
    {
        NoEspeficado = 0,
        Empleo1 = 1,
        Empleo2 = 2,
        Empleo3 = 3,
        Empleo4 = 4,
        Empleo5 = 5,
        Empleo6 = 6,
        Empleo7 = 7,
    }
    public class Acceso
    {
        public long Id { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreLaboratorio { get; set; }
        public long  IdUsuario { get; set; }
        public long IdLaboratorio { get; set; }
        public string Maquina { get; set; }
        public string Ip { get; set; }
        public DateTime Fecha { get; set; }
    }
}

