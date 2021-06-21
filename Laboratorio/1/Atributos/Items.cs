using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TribunalAdministrativo.Usuarios.Atributos;
using TribunalAdministrativo.Usuarios.Descriptores.Atributos;

///<summary>
///Espacio de nombre clasificador de objetos base para la interacción general de la solución Tribunal Administrativo.
///</summary>
///<remarks>
///</remarks>
namespace TribunalAdministrativo.Medios.Items
{
    ///<summary>
    ///Objeto minimo común de Tribunal Administrativo, proporciona un identificador único, pudiendo pertenecer a un contenedor organizador principal.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class ArchivoBase
    {
        ///<summary>
        ///Atributo de tipo numérico largo (int64), de cualidad unica para un archivo resguardado en el aplicativo TribunalAdministrativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public long IdArchivo { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, referencia del nombre original del archivo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string NombreOrigen { get; set; }
        ///<summary>
        ///Atributo de tipo entero enumerado (int), referencia al tipo de medio y si contiene mas elementos dentro de el.
        ///</summary>
        ///<remarks>
        ///Las carpetas son medios respaldados, ejemplo un video de audiencia que contiene una "n" cantidad de archivos, no confundir con el contenedor Folder
        ///</remarks>
        public TipoArchivo TipoArchivo { get; set; }
    }

    ///<summary>
    ///Objeto completo común de Tribunal Administrativo, proporciona un identificador único, pudiendo pertenecer a un contenedor organizador principal.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class Archivo : ArchivoBase
    {

        public string Sumario { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, referencia del nombre con que esta resguardado el archivo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string NombreCodigo { get; set; }

        public bool Asegurado { get; set; }

        public OficinaBase Oficina { get; set; }

        ///<summary>
        ///Atributo de tipo object, referencia al usuario que realizo la carga del archico al Tribunal Administrativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public UsuarioBase Usuario { get; set; }

        ///<summary>
        ///Atributo de tipo booleano, referencia si el archivo puede ser accesible de forma publica sin autenticación.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public bool Publico { get; set; }

        ///<summary>
        ///Atributo de tipo Fecha/Tiempo, referencia la fecha en que fue resguardado el archivo en el Tribunal Administrativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string Ruta { get; set; }
        public string URL { get; set; }
        public bool Disponible { get; set; }
    }

    ///<summary>
    ///Objeto completo común de Tribunal Administrativo, proporciona un identificador único, pudiendo pertenecer a un contenedor organizador principal.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class Directorio : Archivo
    {
        public long IdDirectorio { get; set; }

        public string Referencias { get; set; }

        public TipoDirectorio TipoDirectorio { get; set; }

        public MarcadorDirectorio MarcadorDirectorio { get; set; }

        public bool Raiz { get; set; }

        public bool Fisico { get; set; }
    }

    ///<summary>
    ///Objeto completo común de Tribunal Administrativo, proporciona un identificador único, pudiendo pertenecer a un contenedor organizador principal.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class Medio : Archivo
    {
        public long IdMedio { get; set; }

        ///<summary>
        ///Atributo de tipo objeto, referencia al tipo de archivo que esta resguardado (Carpeta,docx, pdf, xls).
        ///</summary>
        ///<remarks>
        ///Los tipos de medios deben ser autorizados para garantizar la homogeneidad de los archivos.
        ///</remarks>
        public TipoMedio TipoMedio { get; set; }

        ///<summary>
        ///Atributo de tipo cadena, referencia al contenido del archivo, siempre y cuando sea de tipo Carpeta, se expresa en formato XML.
        ///</summary>
        ///<remarks>
        ///
        ///</remarks>
        public string Esquema { get; set; }
        ///<summary>
        ///Atributo de tipo numérico largo (int64), referencia el tamaño del total archivo resguardado.
        ///</summary>
        ///<remarks>
        ///En el caso de los de tipo 2 [Carpeta] representa el tamaño total de los archivos contenientes 
        ///</remarks>
        public long TamanioBytes { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, referencia la digestión de los bytes precisos del archivo, se muestra en formato MD5.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Suma { get; set; }

        public bool EnDirectorio { get; set; }
    }

    ///<summary>
    ///Objeto clasificador de tipos para el Tribunal Administrativo, proporciona una clasificación de los archivos que puede manejar el aplicativo.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class TipoMedio
    {
        ///<summary>
        ///Atributo de tipo numérico (int32), de cualidad unica para un tipo de medio a manejar en el aplicativo TribunalAdministrativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public int IdTipoMedio { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, referencia el nombre del tipo de medio a manejar en el aplicativo TribunalAdministrativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Nombre { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, referencia a Multipurpose Internet Mail Extensions, para el control de respuestas a los tipos registrados.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Mime { get; set; }

        public string Extension { get; set; }
        ///<summary>
        ///Atributo de tipo cadena, referencia la descripción del tipo que utilizará el aplicativo.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public string Descripcion { get; set; }
        ///<summary>
        ///Atributo de tipo booleano, referencia al si el Tipo Archivo esta disponible.
        ///</summary>
        ///<remarks>
        ///</remarks>
        public bool Disponible { get; set; }
    }

    public class TipoDirectorio
    {
        public int IdTipoDirectorio { get; set; }

        public string Descripcion { get; set; }

        public bool Disponible { get; set; }
    }

    public class MarcadorDirectorio
    {
        public int IdMarcadorDirectorio { get; set; }

        public string Descripcion { get; set; }

        public string Color { get; set; }

        public string Icono { get; set; }

        public bool Disponible { get; set; }
    }

    ///<summary>
    ///Enumeracion clasificador de tipos medio para el Tribunal Administrativo, proporciona una clasificación de los medios y como lo interpreta el aplicativo.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public enum TipoArchivo
    {
        ARCHIVO = 0,
        MEDIO = 1,
        DIRECTORIO = 2
    }

}
