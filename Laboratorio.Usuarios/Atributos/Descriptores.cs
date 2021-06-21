using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laboratorio.Usuarios.Descriptores.Atributos;

namespace Laboratorio.Usuarios.Descriptores.Atributos
{
    public class Colonia
    {
        ///<summary>
        ///Atributo de tipo numérico (int32), de cualidad unica para un contenedor de colonia
        ///</summary>
        ///<remarks>
        ///</remarks>
        public int Id { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe el nombre de la colonia en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Nombre { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe el codigo postal de la colonia en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string CodigoPostal { get; set; }
    }
    public class Pais
    {
        ///<summary>
        ///Atributo de tipo numérico (int32), de cualidad unica para un contenedor de Pais
        ///</summary>
        ///<remarks>
        ///</remarks>
        public int Id { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe el nombre del Pais en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Nombre { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe la abreviatura del Pais en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Abreviatura { get; set; }
    }
    ///<summary>
    ///Objeto completo de un contenedor de Estados, proporciona un identificador único, referenciando los estados de México.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class Estado
    {
        ///<summary>
        ///Atributo de tipo numérico (int32), de cualidad unica para un contenedor de Estados
        ///</summary>
        ///<remarks>
        ///</remarks>
        public int Id { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe el nombre del Estado del pais en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Nombre { get; set; }
        ///<summary>
        ///Atributo de tipo pais, que describe el pais del Estado en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public Pais Pais { get; set; }
    }

    ///<summary>
    ///Objeto completo de un contenedor de  Municipos, proporciona un identificador único, referenciando los municipios de un Estado.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class Municipio
    {
        ///<summary>
        ///Atributo de tipo numérico (int32), de cualidad unica para un contenedor de Municipios en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public int Id { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe el nombre del Municipio del Estado seleccionado en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Nombre { get; set; }
        ///<summary>
        ///Atributo de tipo entero, que describe el identificador de la ciudad del estado en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public int Clave { get; set; }
        ///<summary>
        ///Atributo de tipo entero, que describe el identificador de la ciudad del estado en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public Estado Estado { get; set; }
    }

    ///<summary>
    ///Objeto completo de un contenedor de Empresas, proporciona un identificador único, referenciando a la empresa.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class Empresa : EmpresaBase
    {
        
        ///<summary>
        ///Atributo de tipo cadena, que describe la razon social de la empresa.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string RazonSocial { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe el codigo postal de la empresa.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Codigo { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe la colonia de la empresa.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Colonia { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe la dirección una empresa.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Direccion { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe la dirección una empresa.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public Municipio Municipio { get; set; }
    }

    public class EmpresaBase
    {

        ///<summary>
        ///Atributo de tipo numérico (int32), de cualidad unica para un contenedor de Empresa en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public int Id { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe el nombre de la Empresa en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Nombre { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe Registro Federal de Contribuyentes de la empresa.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string RFC { get; set; }
        ///<summary>
        ///Atributo de tipo booleano, referencia al si la empresa esta disponible.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public bool Disponible { get; set; }
    }

    ///<summary>
    ///Objeto completo de un contenedor de Departamentos, proporciona un identificador único, referenciando al departamento.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class Division : DivisionBase
    {
        ///<summary>
        ///Atributo de tipo objeto, que describe a la empresa a la que pertenece la division.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public EmpresaBase Empresa { get; set; }
        ///<summary>
        ///Atributo de tipo ClasificacionDivision, que describe una clasificacion division.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public ClasificacionDivision ClasificacionDivision { get; set; }
    }
    public class DivisionBase
    {
        ///<summary>
        ///Atributo de tipo numérico (int32), de cualidad unica para un contenedor de Departamento en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public int Id { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe el nombre del Departamento en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Nombre { get; set; }
        ///<summary>
        ///Atributo de tipo objeto, que describe a la oficina Administrador del Departamento.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public bool Disponible { get; set; }
    }
    ///<summary>
    ///Objeto completo de un contenedor de Coordinacion, proporciona un identificador único, referenciando a la Coordinacion.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class Seccion
    {
        ///<summary>
        ///Atributo de tipo numérico (int32), de cualidad unica para un contenedor de Seccion en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public int Id { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe el nombre de la Seccion en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Nombre { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe una Seccion.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Descripcion { get; set; }
        ///<summary>
        ///Atributo de tipo booleano, referencia al si la Seccion esta disponible.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public DivisionBase Division { get; set; }
        ///<summary>
        ///Atributo de tipo booleano, referencia al si la Seccion esta disponible.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public bool Disponible { get; set; }
    }
    ///<summary>
    ///Objeto minimo de un contenedor de Seccion, proporciona un identificador único.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class SeccionBase
    {
        ///<summary>
        ///Atributo de tipo numérico (int32), de cualidad unica para un contenedor de Seccion en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public int Id { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe el nombre de la Seccion en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Nombre { get; set; }
    }
    ///<summary>
    ///Objeto minimo de un contenedor de tipo Oficina, proporciona un identificador único.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class OficinaBase
    {
        ///<summary>
        ///Atributo de tipo numérico (int32), de cualidad unica para un contenedor de Oficina en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public int Id { get; set; }
        ///<summary>
        ///Atributo de tipo numérico (long).
        ///</summary>
        ///<remarks>
        ///</remarks>
        public long IdArchivo { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe el nombre de la Oficina en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Nombre { get; set; }
        ///<summary>
        ///Atributo de tipo objeto, que el municipio al que pertenece la oficina.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public Municipio Municipio { get; set; }

        ///<summary>
        ///Atributo de tipo cadena, que describe una Oficina.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Descripcion { get; set; }
    }
    ///<summary>
    ///Objeto minimo de un contenedor de tipo Oficina, proporciona un identificador único.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class OficinaBasica
    {
        ///<summary>
        ///Atributo de tipo numérico (int32), de cualidad unica para un contenedor de Oficina en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public int Id { get; set; }
        ///<summary>
        ///Atributo de tipo numérico (long).
        ///</summary>
        ///<remarks>
        ///</remarks>
        public long IdArchivo { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe el nombre de la Oficina en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Nombre { get; set; }
        ///<summary>
        ///Atributo de tipo objeto, que el municipio al que pertenece la oficina.
        ///</summary>
        ///<remarks>
        ///</remarks>
        //public Municipio Municipio { get; set; }

        ///<summary>
        ///Atributo de tipo cadena, que describe una Oficina.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Descripcion { get; set; }
    }

    ///<summary>
    ///Objeto de un contenedor de tipo Oficina, proporciona un identificador único.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class Oficina : OficinaBase
    {
        ///<summary>
        ///Atributo de tipo objeto, que describe tipo de oficina.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public TipoOficina TipoOficina { get; set; }
        ///<summary>
        ///Atributo de tipo objeto, que indica la zona al que pertenece la oficina.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public Seccion Seccion { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe la direccion de la oficina.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Direccion { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe la direccion de la oficina.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Clave { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe el codigo postal de la oficina.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Codigo { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe la colonia de la oficina.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Colonia { get; set; }
        ///<summary>
        ///Atributo de tipo booleano, referencia al si la oficina esta disponible.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public bool Disponible { get; set; }
  

    }
    ///<summary>
    ///Objeto de un contenedor de tipoOficina, proporciona un identificador único.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class TipoOficina
    {
        ///<summary>
        ///Atributo de tipo numérico (int32), de cualidad unica para un contenedor de Tipo Oficina en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public int Id { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe el nombre del Tipo de Oficina en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Nombre { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe al Tipo de oficina.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Descripcion { get; set; }
        ///<summary>
        ///Atributo de tipo booleano, referencia al si el  tipo de oficina esta disponible.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public bool Disponible { get; set; }
    }
    ///<summary>
    ///Objeto de un contenedor de clasificacion de divisiones, proporciona un identificador único.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class ClasificacionDivision
    {
        ///<summary>
        ///Atributo de tipo numérico (int32), de cualidad unica para un contenedor de ClasificacionDivision en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public int Id { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe el nombre de la clasificacion de la division en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Nombre { get; set; }
        ///<summary>
        ///Atributo de tipo string, referencia si la clasificacion de la division esta disponible.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public bool Disponible { get; set; }
    }
    ///<summary>
    ///Objeto de un contenedor de salas, proporciona un identificador único.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class ClasificacionTelefono
    {
        ///<summary>
        ///Atributo de tipo numérico (int32), de cualidad unica para un contenedor de ClasificacionTelefono en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public int IdClasificacionTelefono { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, que describe el nombre de la sala en el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Nombre { get; set; }
        ///<summary>
        ///Atributo de tipo string, referencia al si la clasificación de telefono esta disponible.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public bool Disponible { get; set; }
    }
    public class OficinaAutoridad : OficinaBase
    {
        public Usuarios.Atributos.UsuarioBasico Magistrado { get; set; }
        public Usuarios.Atributos.UsuarioBasico Secretario { get; set; }

    }
}
