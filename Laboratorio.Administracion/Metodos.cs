using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Laboratorio.Administracion;

using System.Security.Cryptography;
using NReco.VideoConverter;
using Laboratorio.Libreria.BaseDatos;
using Newtonsoft.Json;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Laboratorio.Administracion
{
    public class Metodos : Libreria.BaseClass.BaseObject
    {
        public Metodos(string ConnectionString) : base(ConnectionString)
        {

        }

        public Metodos(IBaseDatos Conexion) : base(Conexion)
        {

        }
        public void EnviarPublicacion(int Tipo, string mensaje, string Token, string Cuerpo)
        {
            var post = new Administracion.Herramientas.Mensajeria();
            var msj = new Administracion.MovilMensajeria()
            {
                notificacion = new Notificacion()
                {
                    title = mensaje,
                    body = "",
                    content_available = true
                },
                priority = "high",
                notification = new notification()
                {
                    body = "",
                    title = mensaje,
                    content_available = true
                },
                data = new Data()
                {
                    Tipo = Tipo,
                    Mensaje = mensaje,
                    Cuerpo = Cuerpo

                },
                to = Token
            };
            post.PostMensajeAsync(msj);
        }
        public Usuario SeguridadUsuario(string Nick, string Password, int Laboratorio)
        {
            Usuario respuesta = null;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Nick", Nick));
                prmtrs.Add(new SqlParameter("@Password", Password));
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.empleadoSeguridad_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        respuesta = ConstruirUsuario(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool ChekinUsuario(long IdUsuario, int Laboratorio, string maquina, string ip)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@IdUsuario", IdUsuario));
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                prmtrs.Add(new SqlParameter("@maquina", maquina));
                prmtrs.Add(new SqlParameter("@ip", ip));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.empleadoSeguridadChekIn_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        respuesta = true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public List<string> ConsultarCorreos()
        {
            List<string> respuesta = new List<string>();
            try
            {
                List<Object> prmtrs = new List<object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.correos_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        if (Resultado["correo"].ToString().Contains("@"))
                            respuesta.Add(Resultado["correo"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public Usuario SeguridadPaciente(string Nick, string Password, string Token)
        {
            Usuario respuesta = null;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Nick", Nick));
                prmtrs.Add(new SqlParameter("@Password", MD5Hash(Password)));
                if (Token != "")
                    prmtrs.Add(new SqlParameter("@Token", Token));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.pacienteSeguridad_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        respuesta = ConstruirUsuario(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public Usuario SeguridadMedico(string Nick, string Password, string Token)
        {
            Usuario respuesta = null;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Nick", Nick));
                prmtrs.Add(new SqlParameter("@Password", MD5Hash(Password)));
                if (Token != "")
                    prmtrs.Add(new SqlParameter("@Token", Token));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.medicoSeguridad_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        respuesta = ConstruirUsuario(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public Usuario SeguridadOrganizacion(string Nick, string Password, string Token)
        {
            Usuario respuesta = null;
            try
            {
                List<Object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Nick", Nick));
                prmtrs.Add(new SqlParameter("@Password", MD5Hash(Password)));
                if (Token != "")
                    prmtrs.Add(new SqlParameter("@Token", Token));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.OrganizacionSeguridad_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        respuesta = ConstruirUsuario(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        private string MD5Hash(string text)
        {

            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        public List<TipoDeposito> ConsultarTipoDepositoColeccion()
        {
            List<TipoDeposito> tipos = new List<TipoDeposito>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.tipoDeposito_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirTipoDeposito(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Movimiento> ConsultarMovimientosSolicitud(string solicitud)
        {
            List<Movimiento> tipos = new List<Movimiento>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Solicitud", solicitud));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitudMovimiento_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirMovimiento(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Puesto> ConsultarPuestoColeccion()
        {
            List<Puesto> tipos = new List<Puesto>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Puesto_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirPuesto(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        private TipoDeposito ConstruirTipoDeposito(DataTableReader Resultado)
        {
            return new TipoDeposito()
            {
                Id = int.Parse(Resultado["idTipoDeposito"].ToString()),
                Nombre = Resultado["nombreTipoDeposito"].ToString(),
                Entrega = Resultado["entregaTipoDeposito"].ToString() == "1",
                Indicaciones = Resultado["indicacionesTipoDeposito"].ToString(),
                Existencia = double.Parse(Resultado["existenciaTipoDeposito"].ToString())
            };
        }
        private Puesto ConstruirPuesto(DataTableReader Resultado)
        {
            return new Puesto()
            {
                Id = int.Parse(Resultado["idPuesto"].ToString()),
                Nombre = Resultado["nombrePuesto"].ToString()
            };
        }
        private TipoMovimiento ConstruirTipoMovimiento(DataTableReader Resultado)
        {
            return new TipoMovimiento()
            {
                Id = int.Parse(Resultado["idTipoMovimiento"].ToString()),
                Nombre = Resultado["Movimiento"].ToString()
            };
        }
        private Movimiento ConstruirMovimiento(DataTableReader Resultado)
        {
            return new Movimiento()
            {
                Id = int.Parse(Resultado["idMovimiento"].ToString()),
                Usuario = ConstruirUsuario(Resultado),
                Fecha = DateTime.Parse(Resultado["Fecha"].ToString()),
                Observaciones = Resultado["Observaciones"].ToString(),
                TipoMovimiento = ConstruirTipoMovimiento(Resultado)
            };
        }
        private Ciudad ConstruirCiudad(DataTableReader Resultado)
        {
            return new Ciudad()
            {
                Id = int.Parse(Resultado["idCiudad"].ToString()),
                Nombre = Resultado["nombreCiudad"].ToString(),
                Estado = ConstruirEstado(Resultado)
            };
        }
        private Pago ConstruirPago(DataTableReader Resultado)
        {
           var pago = new Pago()
            {
                Id = long.Parse(Resultado["idPagoSolicitud"].ToString()),
                ClaveSolicitud = Resultado["ClaveSolicitud"].ToString(),
                Monto = double.Parse(Resultado["montoPago"].ToString()),
                ClavePago = Resultado["clavePago"].ToString(),
                Observacion = Resultado["observacionPago"].ToString(),
                Fecha = DateTime.Parse(Resultado["fechaPago"].ToString())
            };
            try
            {
                pago.Adicional = Resultado["adicionalPago"].ToString();
            }
            catch (Exception error) { };
            return pago;
        }
        private Pago ConstruirPagoGabinete(DataTableReader Resultado)
        {
            return new Pago()
            {
                Id = long.Parse(Resultado["IdEstudioGabinetePago"].ToString()),
                Monto = double.Parse(Resultado["MontoPago"].ToString()),
                ClavePago = Resultado["ClavePago"].ToString(),
                Fecha = DateTime.Parse(Resultado["FechaPago"].ToString())
            };
        }
        private DepositoBusqueda ConstruirDeposito(DataTableReader Resultado)
        {
            var deposito = new DepositoBusqueda()
            {
                Nombre = Resultado["nombre"].ToString(),
                Cantidad = int.Parse(Resultado["cantidad"].ToString()),
                NumeroMuestras = int.Parse(Resultado["numero"].ToString())
            };
            deposito.Total = deposito.Cantidad * deposito.NumeroMuestras;
            return deposito;
        }
        private Colonia ConstruirColonia(DataTableReader Resultado)
        {
            return new Colonia()
            {
                Id = int.Parse(Resultado["idColonia"].ToString()),
                Nombre = Resultado["nombreColonia"].ToString()
            };
        }
        public List<TipoMuestra> ConsultarTipoMuestraColeccion()
        {
            List<TipoMuestra> tipos = new List<TipoMuestra>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.tipoMuestra_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirTipoMuestra(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }

        private TipoMuestra ConstruirTipoMuestra(DataTableReader Resultado)
        {
            return new TipoMuestra()
            {
                Id = int.Parse(Resultado["idTipoMuestra"].ToString()),
                Nombre = Resultado["nombreTipoMuestra"].ToString(),
                Indicaciones = Resultado["indicacionesTipoMuestra"].ToString()
            };
        }
        private Muestra ConstruirMuestra(DataTableReader Resultado)
        {
            return new Muestra()
            {
                Id = int.Parse(Resultado["idEstudioMuestra"].ToString()),
                Nombre = Resultado["Nombre"].ToString()
            };
        }
        private MuestraImpresion ConstruirMuestraImpresion(DataTableReader Resultado)
        {
            return new MuestraImpresion()
            {
                Muestra = Resultado["NombreMuestra"].ToString(),
                Edad = Resultado["Edad"].ToString(),
                Descripcion = Resultado["Abreviatura"].ToString(),
                TipoDeposito = Resultado["TipoDeposito"].ToString(),
                Solicitud = Resultado["Solicitud"].ToString(),
                Paciente = Resultado["Paciente"].ToString(),
                Numero = int.Parse(Resultado["NumeroMuestras"].ToString()),
                EsPerfil = int.Parse(Resultado["EsPerfil"].ToString())
            };
        }

        public List<TipoPago> ConsultarTipoPagoColeccion()
        {
            List<TipoPago> tipos = new List<TipoPago>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.tipoPago_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirTipoPago(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<EstudioGabinete> ConsultarEstudioGabineteColeccion()
        {
            List<EstudioGabinete> tipos = new List<EstudioGabinete>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioGabinete_get", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        tipos.Add(ConstruirEstudioGabinete(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<EstudioGabinete> ConsultarEstudioRadiologiaMedicoColeccion()
        {
            List<EstudioGabinete> tipos = new List<EstudioGabinete>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioGabineteMedico_get", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        tipos.Add(ConstruirEstudioGabinete(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<EstudioGabinete> ConsultarEstudioRadiologiaTecnicoColeccion()
        {
            List<EstudioGabinete> tipos = new List<EstudioGabinete>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioGabineteTecnico_get", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        tipos.Add(ConstruirEstudioGabinete(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<EstudioGabinete> ConsultarEstudioGabinetePaciente(long Paciente)
        {
            List<EstudioGabinete> tipos = new List<EstudioGabinete>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Paciente", Paciente));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioGabinetePaciente_get", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        tipos.Add(ConstruirEstudioGabinete(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<EstudioGabinete> ConsultarEstudioGabineteMedico(long Medico)
        {
            List<EstudioGabinete> tipos = new List<EstudioGabinete>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Medico", Medico));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioGabineteExpedienteMedico_get", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        tipos.Add(ConstruirEstudioGabinete(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<EstudioGabinete> ConsultarEstudioGabineteOrganizacion(long Organizacion)
        {
            List<EstudioGabinete> tipos = new List<EstudioGabinete>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Organizacion", Organizacion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioGabineteExpedienteOrganizacion_get", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        tipos.Add(ConstruirEstudioGabinete(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Estudio> ConsultarEstudioNombre(string Nombre)
        {
            List<Estudio> tipos = new List<Estudio>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioNombre_get", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        tipos.Add(ConstruirEstudio(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public EstudioGabinete ConsultarEstudioGabineteId(int Id)
        {
            EstudioGabinete estudio = new EstudioGabinete();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioGabineteId_get", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        estudio = ConstruirEstudioGabinete(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return estudio;
        }
        public List<EstudioGabinete> ConsultarEstudioUltrasonidoMedicoColeccion()
        {
            List<EstudioGabinete> tipos = new List<EstudioGabinete>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioUltrasonidoMedico_get", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        tipos.Add(ConstruirEstudioGabinete(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<EstudioGabinete> ConsultarUltrasonidoMedicoBusqueda(string texto)
        {
            List<EstudioGabinete> tipos = new List<EstudioGabinete>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@texto", texto));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioUltrasonidoMedicoBusqueda_get", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        tipos.Add(ConstruirEstudioGabinete(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<EstudioGabinete> ConsultarRadiologiaMedicoBusqueda(string texto)
        {
            List<EstudioGabinete> tipos = new List<EstudioGabinete>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@texto", texto));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioRadiologiaMedicoBusqueda_get", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        tipos.Add(ConstruirEstudioGabinete(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<EstudioGabinete> ConsultarGabineteBusqueda(string texto)
        {
            List<EstudioGabinete> tipos = new List<EstudioGabinete>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@texto", texto));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioGabineteBusqueda_get", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        tipos.Add(ConstruirEstudioGabinete(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<EstudioGabinete> ConsultarEstudioGabinetePendientesResultado()
        {
            List<EstudioGabinete> tipos = new List<EstudioGabinete>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioGabinetePendientesResultado_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirEstudioGabinete(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<EstudioGabinete> ConsultarEstudioGabineteSolicitados()
        {
            List<EstudioGabinete> tipos = new List<EstudioGabinete>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioGabineteSolicitados_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirEstudioGabinete(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }

        private TipoPago ConstruirTipoPago(DataTableReader Resultado)
        {
            return new TipoPago()
            {
                Id = int.Parse(Resultado["idTipoPago"].ToString()),
                Nombre = Resultado["nombreTipoPago"].ToString(),
                Clave = Resultado["claveTipoPago"].ToString()
            };
        }
        private Elemento ConstruirElemento(DataTableReader Resultado)
        {
            return new Elemento()
            {
                Id = int.Parse(Resultado["idElemento"].ToString()),
                Nombre = Resultado["nombreElemento"].ToString(),
                Orden = int.Parse(Resultado["ordenElemento"].ToString())
            };
        }
        private EstudioGabinete ConstruirEstudioGabinete(DataTableReader Resultado)
        {

            var estudio = new EstudioGabinete()
            {
                Id = int.Parse(Resultado["idEstudio"].ToString()),
                Observaciones = Resultado["observacionesEstudio"].ToString(),
                EstatusSolicitud = (EstatusSolicitudGabinete)int.Parse(Resultado["estatusEstudio"].ToString()),
                Resultado = Resultado["resultadoEstudio"].ToString(),
                Activo = bool.Parse(Resultado["activoEstudio"].ToString()),
                Interpretacion = bool.Parse(Resultado["InterpretacionEstudio"].ToString()),
                TipoEstudio = (TipoEstudioGabinete)int.Parse(Resultado["tipoEstudioGabinete"].ToString()),
                //TipoSolicitud = ConstruirEstudio(Resultado),
                Usuario = ConstruirUsuario(Resultado),
                Paciente = new Usuario() {
                    Id = int.Parse(Resultado["idPaciente"].ToString()),
                    Edad = Resultado["edadPaciente"].ToString() != "" ? int.Parse(Resultado["edadPaciente"].ToString()) : 0,
                    Nombre = Resultado["nombrePaciente"].ToString(),
                    Paterno = Resultado["paternoPaciente"].ToString(),
                    Materno = Resultado["maternoPaciente"].ToString(),
                    FechaNacimiento = Resultado["fechaNacimientoPaciente"].ToString() != "" ? DateTime.Parse(Resultado["fechaNacimientoPaciente"].ToString()) : new DateTime(),
                    Curp = Resultado["curpPaciente"].ToString(),
                    Email = Resultado["emailPaciente"].ToString(),
                    Genero = Resultado["generoPaciente"].ToString(),
                    Celular = Resultado["celularPaciente"].ToString(),
                    NumeroSeguroSocial = Resultado["nssPaciente"].ToString(),
                    Telefono = Resultado["telefonoPaciente"].ToString(),
                    Nick = Resultado["emailPaciente"].ToString(),
                    Direccion = Resultado["direccionPaciente"].ToString(),
                    TipoUsuario = TipoUsuario.PACIENTE
                },
                Organizacion = ConstruirOrganizacion(Resultado),
                Medico = ConstruirMedico(Resultado),
                Fecha = DateTime.Parse(Resultado["fechaSolicitud"].ToString()),
                FechaEnvio = Resultado["FechaEnvioEstudio"].ToString() != "" ? DateTime.Parse(Resultado["FechaEnvioEstudio"].ToString()) : new DateTime(),
                FechaResultado = Resultado["FechaResultadoEstudio"].ToString() != "" ? DateTime.Parse(Resultado["FechaResultadoEstudio"].ToString()) : new DateTime(),
                ObservacionesTecnico = Resultado["ObservacionesTecnicoEstudio"].ToString()

            };
            try
            {
                estudio.Usuario.Password = Resultado["pass"].ToString() == "4297f44b13955235245b2497399d7a93" ? "123123" : "";
            }
            catch (Exception error) { }
            try
            {
                estudio.Folio = Resultado["FolioEstudio"].ToString();
            }
            catch (Exception error) { }
            try
            {
                estudio.ClaveSolicitud = Resultado["claveEstudioGabinete"].ToString();
            }
            catch (Exception error) { }
            try
            {
                estudio.Pagos = ConsultarPagoGabinete(estudio.Id);
            }
            catch (Exception error) { };
            try
            {
                estudio.MedicoInterpretacion = ConstruirMedicoInterpretacion(Resultado);
            }
            catch (Exception error) { };
            try
            {
                estudio.Factura = int.Parse(Resultado["facturaSolicitud"].ToString());
                estudio.DatosFactura = Resultado["datosFacturaSolicitud"].ToString();
            }
            catch (Exception error) { };
            estudio.Estudios = ConsultarEstudioGabinete(estudio.Id);
            estudio.Paciente.NombreCompleto = estudio.Paciente.Nombre + " " + estudio.Paciente.Paterno + " " + estudio.Paciente.Materno;
            estudio.Adjunto = Resultado["AdjuntosEstudio"].ToString() != "" ? Resultado["AdjuntosEstudio"].ToString().Trim(' ').Split(',').ToList() : null;
            if (estudio.Adjunto != null)
            {
                for (int i = 0; i < estudio.Adjunto.Count(); i++)
                {
                    estudio.Adjunto[i] = @"/Directorio/" + estudio.Adjunto[i].Trim();
                }
            }

            if (estudio.EstatusSolicitud == EstatusSolicitudGabinete._SOLICITADO || estudio.EstatusSolicitud == EstatusSolicitudGabinete._RESULTADO || estudio.EstatusSolicitud == EstatusSolicitudGabinete._ENPROCESO)
            {
                estudio.PorCubrir = 0;
                //estudio.Total = estudio.TipoSolicitud.PrecioBase;
                estudio.Pagado = true;
            }
            return estudio;
        }
        private Solicitud ConstruirSolicitudPagos(DataTableReader Resultado)
        {

            var estudio = new Solicitud()
            {
                Id = int.Parse(Resultado["idSolicitud"].ToString()),
                // Observaciones = Resultado["observacionesSolicitud"].ToString(),
                EstatusSolicitud = (EstatusSolicitud)int.Parse(Resultado["estatusSolicitud"].ToString()),
                Usuario = ConstruirUsuario(Resultado),
                SubTotal = Resultado["subtotalSolicitud"].ToString() != "" ? Double.Parse(Resultado["subtotalSolicitud"].ToString()) : 0,
                Total = Resultado["totalSolicitud"].ToString() != "" ? Double.Parse(Resultado["totalSolicitud"].ToString()) : 0,
                PorCubrir = Resultado["porcubrirSolicitud"].ToString() != "" ? Double.Parse(Resultado["porcubrirSolicitud"].ToString()) : 0,
                Debe = Resultado["debeSolicitud"].ToString() != "" ? Double.Parse(Resultado["debeSolicitud"].ToString()) : 0,
                Descuento = Resultado["descuentoSolicitud"].ToString() != "" ? Double.Parse(Resultado["descuentoSolicitud"].ToString()) : 0,
                Acuenta = Resultado["acuentaSolicitud"].ToString() != "" ? Double.Parse(Resultado["acuentaSolicitud"].ToString()) : 0,
                ClaveSolicitud = Resultado["ClaveSolicitud"].ToString(),
                Consecutivo = int.Parse(Resultado["consecutivoSolicitud"].ToString()),
                Folio = Resultado["folioSolicitud"].ToString(),
                Observaciones = Resultado["observacionesSolicitud"].ToString(),
                Edad = Resultado["ededSolicitud"].ToString(),
                Empresa = ConstruirOrganizacion(Resultado),
                TipoUrgencia = Resultado["TipoUrgenciaSolicitud"].ToString() != "" ? (TipoUrgencia)int.Parse(Resultado["TipoUrgenciaSolicitud"].ToString()) : TipoUrgencia.NORMAL,
                ObservacionUrgencia = Resultado["ObservacionUrgenciaSolicitud"].ToString(),
                Pagado = bool.Parse(Resultado["pagadoSolicitud"].ToString()),
                Urgencia = bool.Parse(Resultado["urgenciaSolicitud"].ToString()),
                CostoUrgencia = Double.Parse(Resultado["costourgenciaSolicitud"].ToString()),
                Paciente = new Usuario()
                {
                    Id = int.Parse(Resultado["idPaciente"].ToString()),
                    Edad = Resultado["edadPaciente"].ToString() != "" ? int.Parse(Resultado["edadPaciente"].ToString()) : 0,
                    Nombre = Resultado["nombrePaciente"].ToString(),
                    Paterno = Resultado["paternoPaciente"].ToString(),
                    Materno = Resultado["maternoPaciente"].ToString(),
                    FechaNacimiento = Resultado["fechaNacimientoPaciente"].ToString() != "" ? DateTime.Parse(Resultado["fechaNacimientoPaciente"].ToString()) : new DateTime(),
                    Curp = Resultado["curpPaciente"].ToString(),
                    Email = Resultado["emailPaciente"].ToString(),
                    Genero = Resultado["generoPaciente"].ToString(),
                    Celular = Resultado["celularPaciente"].ToString(),
                    NumeroSeguroSocial = Resultado["nssPaciente"].ToString(),
                    Telefono = Resultado["telefonoPaciente"].ToString(),
                    Nick = Resultado["emailPaciente"].ToString(),
                    Direccion = Resultado["direccionPaciente"].ToString(),
                    TipoUsuario = TipoUsuario.PACIENTE
                },
                Medico = ConstruirMedico(Resultado),
                Fecha = DateTime.Parse(Resultado["fechaSolicitud"].ToString()),
                Laboratorio = ConstruirLaboratorio(Resultado),
                FechaEntrega = Resultado["FechaEntregaSolicitud"].ToString() != "" ? DateTime.Parse(Resultado["FechaEntregaSolicitud"].ToString()) : new DateTime(),
            };
            try
            {
                estudio.Paciente.Colonia = new Colonia
                {
                    Id = int.Parse(Resultado["idColonia"].ToString()),
                    Nombre = Resultado["nombreColonia"].ToString()
                };
                estudio.Paciente.Ciudad = new Ciudad()
                {
                    Id = int.Parse(Resultado["idCiudad"].ToString()),
                    Nombre = Resultado["nombreCiudad"].ToString(),
                    Estado = new Estado()
                    {
                        Id = int.Parse(Resultado["idEstado"].ToString()),
                        Nombre = Resultado["nombreEstado"].ToString()
                    }
                };
            }
            catch (Exception error) { }
            try
            {
                estudio.Factura = int.Parse(Resultado["facturaSolicitud"].ToString());
                estudio.DatosFactura = Resultado["datosFacturaSolicitud"].ToString();
            }
            catch (Exception error) { };
            estudio.Pagos = ConsultarPagoSolicitud(estudio.ClaveSolicitud);
            estudio.Paciente.NombreCompleto = estudio.Paciente.Nombre + " " + estudio.Paciente.Paterno + " " + estudio.Paciente.Materno;
            return estudio;
        }
        private Solicitud ConstruirSolicitud(DataTableReader Resultado, bool completo)
        {

            var estudio = new Solicitud()
            {
                Id = int.Parse(Resultado["idSolicitud"].ToString()),
                // Observaciones = Resultado["observacionesSolicitud"].ToString(),
                EstatusSolicitud = (EstatusSolicitud)int.Parse(Resultado["estatusSolicitud"].ToString()),
                Usuario = ConstruirUsuario(Resultado),
                SubTotal = Resultado["subtotalSolicitud"].ToString() != "" ? Double.Parse(Resultado["subtotalSolicitud"].ToString()) : 0,
                Total = Resultado["totalSolicitud"].ToString() != "" ? Double.Parse(Resultado["totalSolicitud"].ToString()) : 0,
                PorCubrir = Resultado["porcubrirSolicitud"].ToString() != "" ? Double.Parse(Resultado["porcubrirSolicitud"].ToString()) : 0,
                Debe = Resultado["debeSolicitud"].ToString() != "" ? Double.Parse(Resultado["debeSolicitud"].ToString()) : 0,
                Descuento = Resultado["descuentoSolicitud"].ToString() != "" ? Double.Parse(Resultado["descuentoSolicitud"].ToString()) : 0,
                Acuenta = Resultado["acuentaSolicitud"].ToString() != "" ? Double.Parse(Resultado["acuentaSolicitud"].ToString()) : 0,
                ClaveSolicitud = Resultado["ClaveSolicitud"].ToString(),
                Consecutivo = int.Parse(Resultado["consecutivoSolicitud"].ToString()),
                Folio = Resultado["folioSolicitud"].ToString(),
                Observaciones = Resultado["observacionesSolicitud"].ToString(),
                Edad = Resultado["ededSolicitud"].ToString(),
                Empresa = ConstruirOrganizacion(Resultado),
                TipoUrgencia = Resultado["TipoUrgenciaSolicitud"].ToString() != "" ? (TipoUrgencia)int.Parse(Resultado["TipoUrgenciaSolicitud"].ToString()) : TipoUrgencia.NORMAL,
                ObservacionUrgencia = Resultado["ObservacionUrgenciaSolicitud"].ToString(),
                Pagado = bool.Parse(Resultado["pagadoSolicitud"].ToString()),
                Urgencia = bool.Parse(Resultado["urgenciaSolicitud"].ToString()),
                CostoUrgencia = Double.Parse(Resultado["costourgenciaSolicitud"].ToString()),
                Paciente = new Usuario()
                {
                    Id = int.Parse(Resultado["idPaciente"].ToString()),
                    Edad = Resultado["edadPaciente"].ToString() != "" ? int.Parse(Resultado["edadPaciente"].ToString()) : 0,
                    Nombre = Resultado["nombrePaciente"].ToString(),
                    Paterno = Resultado["paternoPaciente"].ToString(),
                    Materno = Resultado["maternoPaciente"].ToString(),
                    FechaNacimiento = Resultado["fechaNacimientoPaciente"].ToString() != "" ? DateTime.Parse(Resultado["fechaNacimientoPaciente"].ToString()) : new DateTime(),
                    Curp = Resultado["curpPaciente"].ToString(),
                    Email = Resultado["emailPaciente"].ToString(),
                    Genero = Resultado["generoPaciente"].ToString(),
                    Celular = Resultado["celularPaciente"].ToString(),
                    NumeroSeguroSocial = Resultado["nssPaciente"].ToString(),
                    Telefono = Resultado["telefonoPaciente"].ToString(),
                    Nick = Resultado["emailPaciente"].ToString(),
                    Direccion = Resultado["direccionPaciente"].ToString(),
                    TipoUsuario = TipoUsuario.PACIENTE
                },
                Medico = ConstruirMedico(Resultado),
                Fecha = DateTime.Parse(Resultado["fechaSolicitud"].ToString()),
                Laboratorio = ConstruirLaboratorio(Resultado),
                FechaEntrega = Resultado["FechaEntregaSolicitud"].ToString() != "" ? DateTime.Parse(Resultado["FechaEntregaSolicitud"].ToString()) : new DateTime(),
            };
            try
            {
                estudio.Paciente.Colonia = new Colonia
                {
                    Id = int.Parse(Resultado["idColonia"].ToString()),
                    Nombre = Resultado["nombreColonia"].ToString()
                };
                estudio.Paciente.Ciudad = new Ciudad()
                {
                    Id = int.Parse(Resultado["idCiudad"].ToString()),
                    Nombre = Resultado["nombreCiudad"].ToString(),
                    Estado = new Estado()
                    {
                        Id = int.Parse(Resultado["idEstado"].ToString()),
                        Nombre = Resultado["nombreEstado"].ToString()
                    }
                };
            }
            catch (Exception error) { }
            if (completo)
            {
                estudio.Estudios = ConsultarEstudioSolicitud(estudio.ClaveSolicitud);
                estudio.Pagos = ConsultarPagoSolicitud(estudio.ClaveSolicitud);
            }
            try
            {
                estudio.Factura = int.Parse(Resultado["facturaSolicitud"].ToString());
                estudio.DatosFactura = Resultado["datosFacturaSolicitud"].ToString();
            }
            catch (Exception error) { };
            estudio.Paciente.NombreCompleto = estudio.Paciente.Nombre + " " + estudio.Paciente.Paterno + " " + estudio.Paciente.Materno;
            return estudio;
        }
        private Cotizacion ConstruirCotizacion(DataTableReader Resultado, bool completo)
        {

            var estudio = new Cotizacion()
            {
                Usuario = ConstruirUsuario(Resultado),
                ClaveCotizacion = Resultado["ClaveCotizacion"].ToString(),
                Consecutivo = int.Parse(Resultado["consecutivo"].ToString()),
                Nombre = Resultado["nombre"].ToString(),
                Paterno = Resultado["paterno"].ToString(),
                Materno = Resultado["materno"].ToString(),
                Fecha = DateTime.Parse(Resultado["fecha"].ToString()),
                Laboratorio = ConstruirLaboratorio(Resultado),

            };
            if (completo)
            {
                estudio.Estudios = ConsultarEstudioCotizacion(estudio.ClaveCotizacion);
            }
            return estudio;
        }

        public List<Departamento> ConsultarDepartamentoColeccion()
        {
            List<Departamento> tipos = new List<Departamento>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Departamento_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirDepartamento(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        private Departamento ConstruirDepartamento(DataTableReader Resultado)
        {
            return new Departamento()
            {
                Id = int.Parse(Resultado["idDepartamento"].ToString()),
                Nombre = Resultado["nombreDepartamento"].ToString()
            };
        }
        public List<Usuario> ConsultarUsuariosColeccion()
        {
            List<Usuario> tipos = new List<Usuario>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Usuario_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirUsuario(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Usuario> ConsultarUsuariosCumpleanos()
        {
            List<Usuario> tipos = new List<Usuario>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.UsuarioCumpleanos_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirUsuario(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Usuario> ConsultarPacientesNombre(string Nombre)
        {
            List<Usuario> tipos = new List<Usuario>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.UsuarioNombre_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirUsuario(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Usuario> ConsultarPacientesBusqueda(string Nombre)
        {
            List<Usuario> tipos = new List<Usuario>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.UsuarioBusqueda_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirUsuario(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Usuario> ConsultarMedicosNombre(string Nombre)
        {
            List<Usuario> tipos = new List<Usuario>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.MedicoNombre_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirUsuario(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public Usuario ConsultarMedico(long Id)
        {
            Usuario tipos = new Usuario();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.MedicoId_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos = ConstruirUsuario(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Usuario> ConsultarMedicosColeccion()
        {
            List<Usuario> tipos = new List<Usuario>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Medico_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirUsuario(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public Usuario ConsultarUsuario(long Id)
        {
            Usuario tipos = new Usuario();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.UsuarioId_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos = ConstruirUsuario(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Organizacion> ConsultarOrganizacionesColeccion()
        {
            List<Organizacion> tipos = new List<Organizacion>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Organizacion_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirOrganizacion(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Insumo> ConsultarInsumosColeccion()
        {
            List<Insumo> tipos = new List<Insumo>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Insumo_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirInsumo(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Insumo> ConsultarInsumosBusqueda(DateTime Inicio, DateTime Fin)
        {
            List<Insumo> tipos = new List<Insumo>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Inicio", Inicio));
                prmtrs.Add(new SqlParameter("@Fin", Fin));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.InsumoBusqueda_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirInsumo(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Insumo> ConsultarInsumosBusquedaLaboratorio(DateTime Inicio, DateTime Fin, int Laboratorio)
        {
            List<Insumo> tipos = new List<Insumo>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Inicio", Inicio));
                prmtrs.Add(new SqlParameter("@Fin", Fin));
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.InsumoBusquedaLaboratorio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirInsumo(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Insumo> ConsultarInsumosLaboratorio(int Laboratorio)
        {
            List<Insumo> tipos = new List<Insumo>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.InsumoLaboratorio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirInsumo(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Laboratorio> ConsultarLaboratorioColeccion()
        {
            List<Laboratorio> tipos = new List<Laboratorio>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Laboratorio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirLaboratorio(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public Laboratorio ConsultarLaboratorioId(int Id)
        {
            Laboratorio tipos = new Laboratorio();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.LaboratorioId_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos = ConstruirLaboratorio(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        private Usuario ConstruirUsuario(DataTableReader Resultado)
        {
            var user = new Usuario()
            {
                Id = int.Parse(Resultado["idUsuario"].ToString()),
                Nombre = Resultado["nombreUsuario"].ToString(),
                Paterno = Resultado["paternoUsuario"].ToString(),
                Materno = Resultado["maternoUsuario"].ToString(),
                Curp = Resultado["curpUsuario"].ToString(),
                Email = Resultado["emailUsuario"].ToString(),
                Genero = Resultado["generoUsuario"].ToString(),
                Celular = Resultado["celularUsuario"].ToString(),
                NumeroSeguroSocial = Resultado["nssUsuario"].ToString(),
                Telefono = Resultado["telefonoUsuario"].ToString(),

                Direccion = Resultado["direccionUsuario"].ToString(),
                TipoUsuario = (TipoUsuario)int.Parse(Resultado["tipoUsuario"].ToString())
            };
            try { user.Nick = Resultado["nickUsuario"].ToString(); } catch (Exception error) { };
            user.Foto = Resultado["fotoUsuario"].ToString();
            try
            {
                if (Resultado["idCiudad"].ToString() != "")
                    user.Ciudad = ConstruirCiudad(Resultado);
                if (Resultado["idColonia"].ToString() != "")
                    user.Colonia = ConstruirColonia(Resultado);
            }
            catch (Exception error) { }
            try
            {
                user.Edad = int.Parse(Resultado["edadUsuario"].ToString());
            }
            catch (Exception error) { };
            try
            {
                user.FechaNacimiento = Resultado["fechaUsuario"].ToString() != "" ? DateTime.Parse(Resultado["fechaUsuario"].ToString()) : new DateTime();
            }
            catch (Exception error) { };
            try
            {
                user.LaboratorioCompleto = ConstruirLaboratorio(Resultado);
            }
            catch (Exception error) { };
            try
            {
                if (Resultado["idPuesto"].ToString() != "")
                    user.Puesto = new Puesto()
                    {
                        Id = int.Parse(Resultado["idPuesto"].ToString()),
                        Nombre = Resultado["nombrePuesto"].ToString(),
                    };
            }
            catch (Exception error) { }
            try
            {
                if (Resultado["escolaridad"].ToString() != "")
                    user.Profesion = (Profesion)int.Parse(Resultado["escolaridad"].ToString());
                if (Resultado["civil"].ToString() != "")
                    user.Civil = (EstadoCivil)int.Parse(Resultado["civil"].ToString());
            }
            catch (Exception error) { }
            try
            {
                user.Password = Resultado["pass"].ToString() == "4297f44b13955235245b2497399d7a93" ? "123123" : "";
            }
            catch (Exception error) { }
            user.NombreCompleto = user.Nombre + " " + user.Paterno + " " + user.Materno;
            return user;
        }
        private Usuario ConstruirMedico(DataTableReader Resultado)
        {
            var user = new Usuario()
            {
                Id = int.Parse(Resultado["idMedico"].ToString()),
                Foto = "",
                Nombre = Resultado["nombreMedico"].ToString(),
                Paterno = Resultado["paternoMedico"].ToString(),
                Materno = Resultado["maternoMedico"].ToString(),
                Curp = Resultado["curpMedico"].ToString(),
                Email = Resultado["emailMedico"].ToString(),
                Genero = Resultado["generoMedico"].ToString(),
                Celular = Resultado["celularMedico"].ToString(),
                NumeroSeguroSocial = Resultado["nssMedico"].ToString(),
                Telefono = Resultado["telefonoMedico"].ToString(),
                Nick = Resultado["emailMedico"].ToString(),
                Direccion = Resultado["direccionMedico"].ToString(),
                TipoUsuario = TipoUsuario.MEDICO
            };
            user.NombreCompleto = user.Nombre + " " + user.Paterno + " " + user.Materno;
            return user;
        }
        private Usuario ConstruirMedicoInterpretacion(DataTableReader Resultado)
        {
            var user = new Usuario()
            {
                Id = int.Parse(Resultado["idMedicoInterpretacion"].ToString()),
                Foto = "",
                Nombre = Resultado["nombreMedicoInterpretacion"].ToString(),
                Paterno = Resultado["paternoMedicoInterpretacion"].ToString(),
                Materno = Resultado["maternoMedicoInterpretacion"].ToString()
            };
            try
            {
                user.Direccion = Resultado["firmaMedicoInterpretacion"].ToString();
            }
            catch (Exception error) { };
            user.NombreCompleto = user.Nombre + " " + user.Paterno + " " + user.Materno;
            return user;
        }
        private Organizacion ConstruirOrganizacion(DataTableReader Resultado)
        {
            return new Organizacion()
            {
                Id = int.Parse(Resultado["idOrganizacion"].ToString()),
                Nombre = Resultado["nombreOrganizacion"].ToString(),
                Direccion = Resultado["direccionOrganizacion"].ToString(),
                Telefono1 = Resultado["Telefono1Organizacion"].ToString(),
                Telefono2 = Resultado["Telefono2Organizacion"].ToString(),
                Colonia = Resultado["coloniaOrganizacion"].ToString(),
                Ciudad = Resultado["ciudadOrganizacion"].ToString(),
                Precios = bool.Parse(Resultado["preciosOrganizacion"].ToString()),
                Pagos = (Resultado["pagosOrganizacion"].ToString() == "True" || Resultado["pagosOrganizacion"].ToString() == "1") ? true :false
            };
        }
        private Laboratorio ConstruirLaboratorio(DataTableReader Resultado)
        {
            return new Laboratorio()
            {
                Id = int.Parse(Resultado["idLaboratorio"].ToString()),
                Nombre = Resultado["nombreLaboratorio"].ToString(),
                Direccion = Resultado["direccionLaboratorio"].ToString(),
                Telefono = Resultado["TelefonoLaboratorio"].ToString(),
                Iniciales = Resultado["inicialesEstudioLaboratorio"].ToString(),
                Logotipo = Resultado["logotipoLaboratorio"].ToString(),
                Responsable = Resultado["responsableLaboratorio"].ToString(),
                Slogan = Resultado["sloganLaboratorio"].ToString()
            };
        }
        private Insumo ConstruirInsumo(DataTableReader Resultado)
        {
            return new Insumo()
            {
                Id = int.Parse(Resultado["id"].ToString()),
                TipoDepoosito = ConstruirTipoDeposito(Resultado),
                TotalPiezas = int.Parse(Resultado["totalPiezas"].ToString()),
                Cantidad = int.Parse(Resultado["cantidad"].ToString()),
                CodigoBarras = Resultado["codigoBarras"].ToString(),
                FechaCompra = DateTime.Parse(Resultado["fechaCompra"].ToString()),
                Costo = Resultado["costo"].ToString(),
                Utilizados = int.Parse(Resultado["utilizados"].ToString()),
                Unidad = Resultado["unidad"].ToString(),
                Proveedor = ConstruirProveedor(Resultado)
            };
        }
        private Proveedor ConstruirProveedor(DataTableReader Resultado)
        {
            return new Proveedor()
            {
                Id = int.Parse(Resultado["idProveedor"].ToString()),
                Nombre = Resultado["nombreProveedor"].ToString()
            };
        }
        public List<TipoCaptura> ConsultarTipoCapturaColeccion()
        {
            List<TipoCaptura> tipos = new List<TipoCaptura>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.tipoCaptura_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirTipoCaptura(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Proveedor> ConsultarProveedoresColeccion()
        {
            List<Proveedor> tipos = new List<Proveedor>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.proveedor_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirProveedor(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        private TipoCaptura ConstruirTipoCaptura(DataTableReader Resultado)
        {
            return new TipoCaptura()
            {
                Id = int.Parse(Resultado["idTipoCaptura"].ToString()),
                Descripcion = Resultado["descripcionTipoCaptura"].ToString()
            };
        }
        public List<Estudio> ConsultarEstudioColeccion()
        {
            List<Estudio> tipos = new List<Estudio>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Estudio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirEstudio(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Estudio> ConsultarEstudios()
        {
            List<Estudio> tipos = new List<Estudio>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Estudios_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirEstudio(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Estudio> ConsultarEstudioPerfil(string ClaveEstudio)
        {
            List<Estudio> tipos = new List<Estudio>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClaveEstudio", ClaveEstudio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioPerfil_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirEstudio(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Estudio> ConsultarEstudioDepartamento(int Departamento)
        {
            List<Estudio> tipos = new List<Estudio>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Departamento", Departamento));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioDepartamento_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirEstudio(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Estudio> ConsultarEstudioDepartamentoGabinete()
        {
            List<Estudio> tipos = new List<Estudio>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioDepartamentoGabinete_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirEstudio(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Estudio> ConsultarPerfilesColeccion()
        {
            List<Estudio> tipos = new List<Estudio>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.perfiles_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirEstudio(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<EstudioResultado> ConsultarEstudioSolicitud(string ClaveSolicitud)
        {
            List<EstudioResultado> tipos = new List<EstudioResultado>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClaveSolicitud", ClaveSolicitud));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioSolicitud_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirEstudioResultado(Resultado, ClaveSolicitud));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<EstudioResultado> ConsultarEstudioSolicitud2(string ClaveSolicitud)
        {
            List<EstudioResultado> tipos = new List<EstudioResultado>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClaveSolicitud", ClaveSolicitud));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioSolicitudDetalle_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirEstudioResultado2(Resultado, ClaveSolicitud));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<EstudioResultado> ConsultarEstudioSolicitud3(string ClaveSolicitud)
        {
            List<EstudioResultado> tipos = new List<EstudioResultado>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClaveSolicitud", ClaveSolicitud));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioSolicitudDetalle_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirEstudioResultado3(Resultado, ClaveSolicitud));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<EstudioGabineteDetalle> ConsultarEstudioGabinete(long Id)
        {
            List<EstudioGabineteDetalle> tipos = new List<EstudioGabineteDetalle>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudiosGabinete_get", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        tipos.Add(ConstruirEstudioGabineteDetalle(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<EstudioResultado> ConsultarEstudioCotizacion(string ClaveCotizacion)
        {
            List<EstudioResultado> tipos = new List<EstudioResultado>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClaveCotizacion", ClaveCotizacion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioCotizacion_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirEstudioResultadoCotizacion(Resultado, ClaveCotizacion));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Estudio> ConsultarEstudioGabinete()
        {
            List<Estudio> tipos = new List<Estudio>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioGabineteColeccion_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirEstudio(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Solicitud> ConsultarSolicitudLaboratorio(int Laboratorio)
        {
            List<Solicitud> tipos = new List<Solicitud>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitudLaboratorio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirSolicitud(Resultado, false));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Solicitud> ConsultarSolicitudLaboratorioBsuqueda(int Laboratorio, string texto)
        {
            List<Solicitud> tipos = new List<Solicitud>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                prmtrs.Add(new SqlParameter("@Texto", texto));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitudLaboratorioBusqueda_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirSolicitud(Resultado, false));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Muestra> ConsultarMuestrasEstudio(string ClaveEstudio)
        {
            List<Muestra> tipos = new List<Muestra>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClaveEstudio", ClaveEstudio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudioMuestras_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirMuestra(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<MuestraImpresion> ConsultarMuestrasSolicitud(string Clave)
        {
            List<MuestraImpresion> tipos = new List<MuestraImpresion>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClaveSolicitud", Clave));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudiomuestrasSolicitud_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        var muestra = ConstruirMuestraImpresion(Resultado);
                        muestra.Modificar = true;
                        if (tipos.Count > 0)
                        {
                            if (muestra.Muestra != "")
                            {
                                tipos.Add(muestra);
                            }
                            else
                            {
                                if ((tipos.Last().TipoDeposito == muestra.TipoDeposito))
                                {
                                    tipos.Last().Descripcion = tipos.Last().Descripcion + ", " + muestra.Descripcion;
                                    if (muestra.Numero > 1)
                                    {
                                        for (var i = 0; i < muestra.Numero; i++)
                                        {
                                            tipos.Add(tipos.Last());
                                        }
                                    }
                                }
                                else
                                {
                                    tipos.Add(muestra);
                                }
                            }

                        }
                        else {
                                if (muestra.Numero > 1)
                                {
                                    for (var i = 0; i < muestra.Numero; i++)
                                    {
                                        tipos.Add(muestra);
                                    }
                                }
                                else
                                {
                                    tipos.Add(muestra);
                                }
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Cotizacion> ConsultarCotizacionLaboratorio(int Laboratorio)
        {
            List<Cotizacion> tipos = new List<Cotizacion>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.cotizacionLaboratorio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirCotizacion(Resultado, false));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Solicitud> ConsultarSolicitudPeriodo(int Laboratorio, int Mes, int Anio)
        {
            List<Solicitud> tipos = new List<Solicitud>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                prmtrs.Add(new SqlParameter("@Mes", Mes));
                prmtrs.Add(new SqlParameter("@Anio", Anio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitudLaboratorioPeriodo_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirSolicitud(Resultado, false));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Solicitud> ConsultarSolicitudEstadistica(int Laboratorio, int Mes, int Anio, int Colonia, int Ciudad, int Estado)
        {
            List<Solicitud> tipos = new List<Solicitud>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                prmtrs.Add(new SqlParameter("@Mes", Mes));
                prmtrs.Add(new SqlParameter("@Anio", Anio));
                prmtrs.Add(new SqlParameter("@Colonia", Colonia));
                prmtrs.Add(new SqlParameter("@Ciudad", Ciudad));
                prmtrs.Add(new SqlParameter("@Estado", Estado));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitudLaboratorioEstadistica_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirSolicitud(Resultado, false));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public DataTable  ConsultarSolicitudEstudios(string Estudio, int Laboratorio, int Mes, int Anio, int Colonia, int Ciudad, int Estado)
        {
            DataTable datos = new DataTable();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Estudio", Estudio));
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                prmtrs.Add(new SqlParameter("@Mes", Mes));
                prmtrs.Add(new SqlParameter("@Anio", Anio));
                prmtrs.Add(new SqlParameter("@Colonia", Colonia));
                prmtrs.Add(new SqlParameter("@Ciudad", Ciudad));
                prmtrs.Add(new SqlParameter("@Estado", Estado));
                using (var Resultado = base.conexion.EjecutarDataStoreProcedure("dbo.solicitudLaboratorioEstudios_get", prmtrs))
                {
                    datos = Resultado;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return datos;
        }
        public List<Pago> ConsultarPagosPeriodo(int Laboratorio, int Mes, int Anio)
        {
            List<Pago> tipos = new List<Pago>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                prmtrs.Add(new SqlParameter("@Mes", Mes));
                prmtrs.Add(new SqlParameter("@Anio", Anio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.pagosLaboratorioPeriodo_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirPago(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<DepositoBusqueda> ConsultarDepositosPeriodo(int Laboratorio, int Mes, int Anio)
        {
            List<DepositoBusqueda> tipos = new List<DepositoBusqueda>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                prmtrs.Add(new SqlParameter("@Mes", Mes));
                prmtrs.Add(new SqlParameter("@Anio", Anio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.depositosLaboratorioPeriodo_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirDeposito(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Solicitud> ConsultarSolicitudPaciente(int Paciente)
        {
            List<Solicitud> tipos = new List<Solicitud>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Paciente", Paciente));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitudPaciente_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirSolicitud(Resultado, false));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Solicitud> ConsultarSolicitudMedico(int Medico)
        {
            List<Solicitud> tipos = new List<Solicitud>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Medico", Medico));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitudMedico_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirSolicitud(Resultado, false));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Solicitud> ConsultarSolicitudOrganizacion(int Organizacion)
        {
            List<Solicitud> tipos = new List<Solicitud>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Organizacion", Organizacion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitudOrganizacion_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirSolicitud(Resultado, false));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public bool ActualizarPassUsuario(long IdUsuario, string Pass)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", IdUsuario));
                prmtrs.Add(new SqlParameter("@Pass", MD5Hash(Pass)));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.UsuarioPass_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool ActualizarPassPaciente(long Id, string Pass)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                prmtrs.Add(new SqlParameter("@Pass", MD5Hash(Pass)));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.PacientePass_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool ActualizarPassMedico(long Id, string Pass)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                prmtrs.Add(new SqlParameter("@Pass", MD5Hash(Pass)));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.MedicoPass_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool ActualizarPassOrganizacion(long Id, string Pass)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                prmtrs.Add(new SqlParameter("@Pass", MD5Hash(Pass)));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.OrganizacionPass_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool ActualizarFotoPaciente(long Id, string foto)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                prmtrs.Add(new SqlParameter("@foto", foto));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.PacienteFoto_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool ActualizarFotoMedico(long Id, string foto)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                prmtrs.Add(new SqlParameter("@foto", foto));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.MedicoFoto_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool ActualizarFotoOrganizacion(long Id, string foto)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                prmtrs.Add(new SqlParameter("@foto", foto));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.OrganizacionFoto_upd", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarEstudioSolicitud(long Id, int Usuario)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@EstudioSolicitud_Id", Id));
                prmtrs.Add(new SqlParameter("@Usuario", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudiosolicitud_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarEstudioPerfil(long Id, int Usuario)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.perfildetalle_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarSolicitudGabinete(long Id, int Usuario)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Solicitud_Id", Id));
                prmtrs.Add(new SqlParameter("@Usuario", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitudGabinete_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarSolicitud(long Id, long IdUsuario)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Solicitud_Id", Id));
                prmtrs.Add(new SqlParameter("@Usuario", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitud_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarEstudio(long Id, long IdUsuario)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Estudio_Id", Id));
                prmtrs.Add(new SqlParameter("@Usuario", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Estudio_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarPaciente(long Id, long IdUsuario)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Paciente_Id", Id));
                prmtrs.Add(new SqlParameter("@Usuario", IdUsuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.paciente_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarPago(long Id, long IdUsuario, string Observaciones)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@IdPagoSolicitud_Id", Id));
                prmtrs.Add(new SqlParameter("@Usuario", IdUsuario));
                prmtrs.Add(new SqlParameter("@Observaciones", Observaciones));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Pago_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarEmpresa(long Id)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Empresa_Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.empresa_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarUsuario(long Id)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Usuario_Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.usuario_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarMedico(long Id)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Medico_Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Medico_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarTipoMuestra(long Id)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@TipoMuestra_Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.TipoMuestra_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarTipoDeposito(long Id)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@TipoDeposito_Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.TipoDeposito_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EliminarDepartamento(long Id)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Departamento_Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Departamento_del", prmtrs))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public List<Solicitud> ConsultarSolicitudPacientePeriodo(int Paciente, int Anio)
        {
            List<Solicitud> tipos = new List<Solicitud>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Paciente", Paciente));
                prmtrs.Add(new SqlParameter("@Anio", Anio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitudPacientePeriodo_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirSolicitud(Resultado, false));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Solicitud> ConsultarSolicitudPacienteBusqueda(int Paciente, string Nombre)
        {
            List<Solicitud> tipos = new List<Solicitud>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Paciente", Paciente));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitudPacienteBusqueda_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirSolicitud(Resultado, false));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<ResultadoSolicitud> ConsultarResutladoSolicitud(string Solicitud)
        {
            List<ResultadoSolicitud> tipos = new List<ResultadoSolicitud>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Solicitud", Solicitud));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.resultadosolicitud_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirResultadoSolicitud(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public ResultadoSolicitud ConsultarResultadoComponenteSolicitud(int Componente, string estudio, string Solicitud)
        {
            ResultadoSolicitud tipos = new ResultadoSolicitud();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Solicitud", Solicitud));
                prmtrs.Add(new SqlParameter("@Componente", Componente));
                prmtrs.Add(new SqlParameter("@Estudio", estudio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.resultadocomponentesolicitud_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos = ConstruirResultadoSolicitud(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public Solicitud ConsultarSolicitud(long Id)
        {
            Solicitud tipos = new Solicitud();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitud_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos = ConstruirSolicitud(Resultado, true);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public Solicitud ConsultarSolicitudPagos(long Id)
        {
            Solicitud tipos = new Solicitud();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitud_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos = ConstruirSolicitudPagos(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public Solicitud ConsultarSolicitud(long Id, bool completo)
        {
            Solicitud tipos = new Solicitud();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitud_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos = ConstruirSolicitud(Resultado, completo);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public Cotizacion ConsultarCotizacion(string Clave)
        {
            Cotizacion tipos = new Cotizacion();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClaveCotizacion", Clave));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Cotizacion_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos = ConstruirCotizacion(Resultado, true);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public Estudio ConsultarEstudio(int Id)
        {
            Estudio estudio = new Estudio();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@IdEstudio", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioId_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        estudio = ConstruirEstudio(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return estudio;
        }
        public List<Componente> ConsultarComponentesEstudio(string Clave)
        {
            List<Componente> tipos = new List<Componente>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Clave", Clave));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.ComponenteEstudio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirComponente(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Componente> ConsultarComponentesEstudioResultado(string Clave, string Solicitud)
        {
            List<Componente> tipos = new List<Componente>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Clave", Clave));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.ComponenteEstudio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirComponenteResultado(Resultado, Solicitud));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Componente> ConsultarComponentesEstudioResultadoPerfil(string Clave, string Solicitud)
        {
            List<Componente> tipos = new List<Componente>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Clave", Clave));
                prmtrs.Add(new SqlParameter("@ClaveSolicitud", Solicitud));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.ComponenteEstudioPerfil_get", prmtrs))
                {

                    while (Resultado.Read())
                    {
                        var z = ConstruirComponenteResultado(Resultado, Solicitud);

                        tipos.Add(z);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Componente> ConsultarComponentesEstudioPerfil(string Clave, string ClaveSolicitud)
        {
            List<Componente> tipos = new List<Componente>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Clave", Clave));
                prmtrs.Add(new SqlParameter("@ClaveSolicitud", ClaveSolicitud));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.ComponenteEstudioPerfil_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirComponente(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Pago> ConsultarPagoSolicitud(string ClaveSolicitud)
        {
            List<Pago> tipos = new List<Pago>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Clave", ClaveSolicitud));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitudPago_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirPago(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Pago> ConsultarPagoGabinete(long Id)
        {
            List<Pago> tipos = new List<Pago>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@EstudioGabinete_Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.EstudioGabinetePago_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirPagoGabinete(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public List<Elemento> ConsultarElementosComponente(int Id)
        {
            List<Elemento> tipos = new List<Elemento>();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.elementosComponente_get", prmtrs))
                {
                    tipos.Add(new Elemento() { Id = 0, Nombre = "", Orden = 0 });
                    while (Resultado.Read())
                    {
                        tipos.Add(ConstruirElemento(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public Componente ConsultarComponente(int Id)
        {
            Componente tipos = new Componente();
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Componente_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        tipos = ConstruirComponente(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return tipos;
        }
        public bool CrearEstudio(int? Muestra, int? Deposito, int? Departamento, string Nombre, string Clave, string Abreviatura, string Indicaciones, string Precio, string Unidad, string Urgencia, string Volumen, string Numero, int PermiteDescuento, int Perfil)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@IdDepartamento", Departamento));
                prmtrs.Add(new SqlParameter("@IdDeposito", Deposito));
                prmtrs.Add(new SqlParameter("@IdMuestra", Muestra));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Clave", Clave));
                prmtrs.Add(new SqlParameter("@Abreviatura", Abreviatura));
                prmtrs.Add(new SqlParameter("@Indicaciones", Indicaciones));
                prmtrs.Add(new SqlParameter("@Precio", float.Parse(Precio)));
                prmtrs.Add(new SqlParameter("@UnidadMuestra", Unidad));
                prmtrs.Add(new SqlParameter("@Urgencia", Urgencia));
                prmtrs.Add(new SqlParameter("@Volumen", float.Parse(Volumen != null ? Volumen : "0")));
                prmtrs.Add(new SqlParameter("@Numero", float.Parse(Numero != null ? Numero : "0")));
                prmtrs.Add(new SqlParameter("@PermiteDescuento", PermiteDescuento));
                prmtrs.Add(new SqlParameter("@Perfil", Perfil));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudio_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearComponenteEstudio(int TipoCaptura, string Nombre, string Abreviatura, string Unidad, int Orden, string ClaveEstudio)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                
                prmtrs.Add(new SqlParameter("@Unidad", Unidad));
                prmtrs.Add(new SqlParameter("@Abreviatura", Abreviatura));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Orden", Orden));
                prmtrs.Add(new SqlParameter("@ClaveEstudio", ClaveEstudio));
                prmtrs.Add(new SqlParameter("@TipoCaptura", TipoCaptura));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudioComponente_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool ActualizarComponente(Componente Componente, int tipo)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                switch (tipo)
                {
                    case 1:

                        prmtrs.Add(new SqlParameter("@MING", Componente.MinG));
                        prmtrs.Add(new SqlParameter("@MAXG", Componente.MaxG));
                        prmtrs.Add(new SqlParameter("@MINH", Componente.MinH));
                        prmtrs.Add(new SqlParameter("@MAXH", Componente.MaxH));
                        prmtrs.Add(new SqlParameter("@MINM", Componente.MinM));
                        prmtrs.Add(new SqlParameter("@MAXM", Componente.MaxM));
                        prmtrs.Add(new SqlParameter("@MINN", Componente.MinN));
                        prmtrs.Add(new SqlParameter("@MAXN", Componente.MaxN));
                        prmtrs.Add(new SqlParameter("@MINRN", Componente.MinRN));
                        prmtrs.Add(new SqlParameter("@MAXRN", Componente.MaxRN));
                        prmtrs.Add(new SqlParameter("@TextoNormalGeneral", Componente.TextoNormalGeneral));
                        prmtrs.Add(new SqlParameter("@TextoNormalHombres", Componente.TextoNormalHombres));
                        prmtrs.Add(new SqlParameter("@TextoNormalMujeres", Componente.TextoNormalMujeres));
                        prmtrs.Add(new SqlParameter("@TextoNormalNiños", Componente.TextoNormalNiños));
                        prmtrs.Add(new SqlParameter("@TextoNormalRecienNacido", Componente.TextoNormalRecienNacido));
                        break;
                    case 4: //elementos
                        break;
                    case 3: //personalizado
                        prmtrs.Add(new SqlParameter("@TextoNormalGeneral", Componente.TextoNormalGeneral));
                        break;
                    default:
                        break;
                }
                prmtrs.Add(new SqlParameter("@Componente", Componente.Id));
                prmtrs.Add(new SqlParameter("@Abreviatura", Componente.Abreviatura));
                prmtrs.Add(new SqlParameter("@Nombre", Componente.Nombre));
                prmtrs.Add(new SqlParameter("@Unidad", Componente.Unidad));
                prmtrs.Add(new SqlParameter("@Orden", Componente.Orden));
                prmtrs.Add(new SqlParameter("@Indicacion", Componente.Indicaciones));
                prmtrs.Add(new SqlParameter("@UsarValoresGenerales", Componente.UsarValoresGenerales));
                prmtrs.Add(new SqlParameter("@PermiteDesfase", Componente.PermiteDesfaseValores));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.actualizarComponente_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearElementoComponente(string Descripcion, int Componente, int Orden)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();

                prmtrs.Add(new SqlParameter("@Componente", Componente));
                prmtrs.Add(new SqlParameter("@Descripcion", Descripcion));
                prmtrs.Add(new SqlParameter("@Orden", Orden));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.elementoComponente_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public string CambiarEstatus(string Clave, bool Pagado, int Estatus, int Usuario)
        {
            string respuesta = "";
            try
            {
                List<Object> prmtrs = new List<Object>();
                string token = "";
                long res = 0;
                prmtrs.Add(new SqlParameter("@Estatus", Estatus));
                prmtrs.Add(new SqlParameter("@Usuario", Usuario));
                prmtrs.Add(new SqlParameter("@Clave", Clave));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitudEstatus_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        res = long.Parse(Resultado["Id"].ToString());
                        token = Resultado["Token"].ToString();
                        if (res != 0)
                            respuesta = Resultado["Email"].ToString();
                    }

                    if (Pagado && Estatus == 4) {
                        EnviarPublicacion(2, "Se han capturado los resultados de su solicitud " + Clave, token, JsonConvert.SerializeObject(res));
                    }

                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public long CambiarUsuario(int Laboratorio, int Usuario, int Rol)
        {
            long respuesta = 0;
            try
            {

                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                prmtrs.Add(new SqlParameter("@Usuario", Usuario));
                prmtrs.Add(new SqlParameter("@Rol", Rol));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.cambiarUsuario_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        respuesta = long.Parse(Resultado["Identificador"].ToString());
                    }
                    
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearPago(string ClaveSolicitud, string Tipo, double Monto, double Descuento, double CostoUrgencias, int Usuario, string Adicional)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();

                prmtrs.Add(new SqlParameter("@ClaveSolicitud", ClaveSolicitud));
                prmtrs.Add(new SqlParameter("@Tipo", Tipo));
                prmtrs.Add(new SqlParameter("@Monto", Monto));
                prmtrs.Add(new SqlParameter("@Descuento", Descuento));
                prmtrs.Add(new SqlParameter("@CostoUrgencias", CostoUrgencias));
                prmtrs.Add(new SqlParameter("@Usuario", Usuario));
                prmtrs.Add(new SqlParameter("@Adicional", Adicional));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.pago_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearPagoGabinete(long Estudio, string Tipo, double Monto, int Usuario)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();

                prmtrs.Add(new SqlParameter("@EstudioGabinete_Id", Estudio));
                prmtrs.Add(new SqlParameter("@TipoPago_Id", Tipo));
                prmtrs.Add(new SqlParameter("@Monto", Monto));
                prmtrs.Add(new SqlParameter("@Usuario_Id", Usuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudioGabinetePago_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearUsuario(string Nombre, string Paterno, string Materno, string Direccion, string Telefono, string Celular, string Email, int Laboratorio, int Puesto, int Genero)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Direccion", Direccion));
                prmtrs.Add(new SqlParameter("@Telefono", Telefono));
                prmtrs.Add(new SqlParameter("@Celular", Celular));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Paterno", Paterno));
                prmtrs.Add(new SqlParameter("@Materno", Materno));
                prmtrs.Add(new SqlParameter("@Email", Email));
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                prmtrs.Add(new SqlParameter("@Puesto", Puesto));
                prmtrs.Add(new SqlParameter("@Genero", Genero));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Usuario_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearMedico(string Nombre, string Paterno, string Materno, string Direccion, string Telefono, string Celular, string Email,int Genero)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Direccion", Direccion));
                prmtrs.Add(new SqlParameter("@Telefono", Telefono));
                prmtrs.Add(new SqlParameter("@Celular", Celular));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Paterno", Paterno));
                prmtrs.Add(new SqlParameter("@Materno", Materno));
                prmtrs.Add(new SqlParameter("@Email", Email));
                prmtrs.Add(new SqlParameter("@Genero", Genero));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Medico_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool ActualizarMedico(long Id, string Nombre, string Paterno, string Materno, string Direccion, string Telefono, string Celular, string Email, int Genero)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                prmtrs.Add(new SqlParameter("@Direccion", Direccion));
                prmtrs.Add(new SqlParameter("@Telefono", Telefono));
                prmtrs.Add(new SqlParameter("@Celular", Celular));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Paterno", Paterno));
                prmtrs.Add(new SqlParameter("@Materno", Materno));
                prmtrs.Add(new SqlParameter("@Email", Email));
                prmtrs.Add(new SqlParameter("@Genero", Genero));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Medico_upd", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearPaciente(string Nombre, string Paterno, string Materno, string Direccion, string Telefono, string Celular, string NSS, string RFC, string Email, int Ciudad, int Colonia, int Escolaridad, int EstadoCivil, int Edad, DateTime Fecha, string Genero, int Organizacion)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Direccion", Direccion));
                prmtrs.Add(new SqlParameter("@Telefono", Telefono));
                prmtrs.Add(new SqlParameter("@Celular", Celular));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Paterno", Paterno));
                prmtrs.Add(new SqlParameter("@Materno", Materno));
                prmtrs.Add(new SqlParameter("@NSS", NSS));
                prmtrs.Add(new SqlParameter("@RFC",RFC));
                prmtrs.Add(new SqlParameter("@Email", Email));
                prmtrs.Add(new SqlParameter("@Ciudad", Ciudad));
                prmtrs.Add(new SqlParameter("@Colonia",Colonia));
                if (Fecha.Year > 1)
                    prmtrs.Add(new SqlParameter("@FechaNacimiento", Fecha));
                prmtrs.Add(new SqlParameter("@Edad", Edad));
                prmtrs.Add(new SqlParameter("@Escolaridad", Escolaridad));
                prmtrs.Add(new SqlParameter("@EstadoCivil", EstadoCivil));
                prmtrs.Add(new SqlParameter("@Genero", Genero));
                prmtrs.Add(new SqlParameter("@Organizacion", Organizacion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.paciente_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool ActualizarPaciente(long Id,string Nombre, string Paterno, string Materno, string Direccion, string Telefono, string Celular, string NSS, string RFC, string Email, int Ciudad, int Colonia, int Escolaridad, int EstadoCivil, int Edad, DateTime Fecha, string Genero, int Organizacion)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                prmtrs.Add(new SqlParameter("@Direccion", Direccion));
                prmtrs.Add(new SqlParameter("@Telefono", Telefono));
                prmtrs.Add(new SqlParameter("@Celular", Celular));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Paterno", Paterno));
                prmtrs.Add(new SqlParameter("@Materno", Materno));
                prmtrs.Add(new SqlParameter("@NSS", NSS));
                prmtrs.Add(new SqlParameter("@RFC", RFC));
                prmtrs.Add(new SqlParameter("@Email", Email));
                prmtrs.Add(new SqlParameter("@Ciudad", Ciudad));
                prmtrs.Add(new SqlParameter("@Colonia", Colonia));
                if (Fecha.Year > 1)
                    prmtrs.Add(new SqlParameter("@FechaNacimiento", Fecha));
                prmtrs.Add(new SqlParameter("@Edad", Edad));
                prmtrs.Add(new SqlParameter("@Escolaridad", Escolaridad));
                prmtrs.Add(new SqlParameter("@EstadoCivil", EstadoCivil));
                prmtrs.Add(new SqlParameter("@Genero", Genero));
                prmtrs.Add(new SqlParameter("@Organizacion", Organizacion));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.paciente_upd", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EnviarEstudioGabinete(int Id, string Observaciones, string Imagenes, int Usuario)
        {
            bool respuesta = false;
            try
            {
                var ruta = ConfigurationManager.ConnectionStrings["Directorio"].ToString().Replace("\\", @"\");
                var nuevas = "";
                var Adjuntos1 = Imagenes.ToString() != "" ? Imagenes.ToString().Trim(' ').Split(',').ToList() : null;

                if (Imagenes.Contains(","))
                {

                    for (int i = 0; i < Adjuntos1.Count(); i++)
                    {
                        var adj = Adjuntos1[i].ToString().Trim(' ');
                        var ext = adj.Trim().ToLower().Split('.');
                        for (int e = 0; e < ext.Count(); e++)
                        {
                            var resultado = Guid.NewGuid() + "." + ext[e].ToString();
                            if (ext[e] == "avi")
                            {
                                var name = AviToMp4(ruta + @"\Temp\" + Adjuntos1[i], ext[e].ToString());
                                if (name != "")
                                {
                                    Imagenes = Imagenes.Replace(Adjuntos1[i].ToString(), name);
                                }                               
                            }
                            else if (ext[e] == "tif")
                            {
                                var name = TifftoPng(ruta + @"\Temp\" + Adjuntos1[i], ext[e].ToString());
                                if (name != "")
                                {
                                    Imagenes = Imagenes.Replace(Adjuntos1[i].ToString(), name);
                                }
                            }
                            else if (ext[e] == "txt" || ext[e] == "dcm" || ext[e] == "dcm30" || ext[e] == "rar" || ext[e] == "zip" || ext[e] == "jpg" || ext[e] == "png" || ext[e] == "mp4" || ext[e] == "pdf"  || ext[e] == "jpeg" || ext[e] == "doc" || ext[e] == "docx")
                            {

                                Imagenes = Imagenes.Replace(Adjuntos1[i].ToString(), @"/Imagenes/" + DateTime.Now.Year.ToString() + @"/" + DateTime.Now.Month.ToString() + @"/" + resultado);
                                String path = ConfigurationManager.ConnectionStrings["Directorio"].ToString();
                                var dir = VerificarRutaActual(path);
                                File.Move(ruta + @"\Temp\" + Adjuntos1[i], dir + @"\" + resultado);
                                //@"/Imagenes/" + DateTime.Now.Year.ToString() + @"/" + DateTime.Now.Month.ToString() + @"/" + resultado
                            }
                        }
                    }
                }
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                prmtrs.Add(new SqlParameter("@Observaciones", Observaciones));
                prmtrs.Add(new SqlParameter("@adjuntos", Imagenes));
                prmtrs.Add(new SqlParameter("@Usuario", Usuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudioGabineteEnvio_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearResultadoEstudio(int Id, string Observaciones, int Usuario)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                prmtrs.Add(new SqlParameter("@Observaciones", Observaciones));
                prmtrs.Add(new SqlParameter("@Usuario", Usuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudioGabineteResultado_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearResultadoEstudio2(int Id, string Observaciones, int Usuario,string Imagenes)
        {
            bool respuesta = false;
            var ruta = ConfigurationManager.ConnectionStrings["Directorio"].ToString().Replace("\\", @"\");
                var Adjuntos1 = Imagenes.ToString() != "" ? Imagenes.ToString().Trim(' ').Split(',').ToList() : null;

                if (Imagenes.Contains(","))
                {

                    for (int i = 0; i < Adjuntos1.Count(); i++)
                    {
                        var adj = Adjuntos1[i].ToString().Trim(' ');
                        var ext = adj.Trim().ToLower().Split('.');
                        for (int e = 0; e < ext.Count(); e++)
                        {
                            var resultado = Guid.NewGuid() + "." + ext[e].ToString();
                            if (ext[e] == "avi")
                            {
                                var name = AviToMp4(ruta + @"\Temp\" + Adjuntos1[i], ext[e].ToString());
                                if (name != "")
                                {
                                    Imagenes = Imagenes.Replace(Adjuntos1[i].ToString(), name);
                                }                               
                            }
                            else if (ext[e] == "tif")
                            {
                                var name = TifftoPng(ruta + @"\Temp\" + Adjuntos1[i], ext[e].ToString());
                                if (name != "")
                                {
                                    Imagenes = Imagenes.Replace(Adjuntos1[i].ToString(), name);
                                }
                            }
                            else if (ext[e] == "txt" || ext[e] == "dcm" || ext[e] == "dcm30" || ext[e] == "rar" || ext[e] == "zip" || ext[e] == "jpg" || ext[e] == "png" || ext[e] == "mp4" || ext[e] == "pdf"  || ext[e] == "jpeg" || ext[e] == "doc" || ext[e] == "docx")
                            {

                                Imagenes = Imagenes.Replace(Adjuntos1[i].ToString(), @"/Imagenes/" + DateTime.Now.Year.ToString() + @"/" + DateTime.Now.Month.ToString() + @"/" + resultado);
                                String path = ConfigurationManager.ConnectionStrings["Directorio"].ToString();
                                var dir = VerificarRutaActual(path);
                                File.Move(ruta + @"\Temp\" + Adjuntos1[i], dir + @"\" + resultado);
                            }
                        }
                    }
                }
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                prmtrs.Add(new SqlParameter("@Observaciones", Observaciones));
                prmtrs.Add(new SqlParameter("@Usuario", Usuario));
                prmtrs.Add(new SqlParameter("@adjuntos", Imagenes));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudioGabineteResultado_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearResultadosEstudioSolicitud(List<ResultadoSolicitud> Resultados)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                foreach (var r in Resultados)
                {
                    prmtrs.Add(new SqlParameter("@NombreEstudio", r.NombreEstudio));
                    prmtrs.Add(new SqlParameter("@Capturado", r.Capturado));
                    prmtrs.Add(new SqlParameter("@EstudioComponente_Id", r.EstudioComponente));
                    prmtrs.Add(new SqlParameter("@ClaveEstudio", r.ClaveEstudio));
                    prmtrs.Add(new SqlParameter("@ClaveEstudioMain", r.ClaveEstudioMain));
                    prmtrs.Add(new SqlParameter("@ClaveSolicitud", r.ClaveSolicitud));
                    prmtrs.Add(new SqlParameter("@FueraRango", r.FueraRango));
                    prmtrs.Add(new SqlParameter("@Imprimir", r.Imprimir));
                    prmtrs.Add(new SqlParameter("@LineasImpresion", r.LineasImpresion));
                    prmtrs.Add(new SqlParameter("@MAX",  float.Parse(r.MAX)));
                    prmtrs.Add(new SqlParameter("@MIN", float.Parse(r.MIN)));    
                    prmtrs.Add(new SqlParameter("@Negrita", r.Negrita));
                    prmtrs.Add(new SqlParameter("@Referencia", r.Referencia));
                    prmtrs.Add(new SqlParameter("@Resultado", r.Resultado));
                    prmtrs.Add(new SqlParameter("@Ruta", r.Ruta));
                    prmtrs.Add(new SqlParameter("@Normal", r.Normal));
                    prmtrs.Add(new SqlParameter("@TipoCaptura", r.TipoCaptura));
                    prmtrs.Add(new SqlParameter("@Titulo", r.Titulo));
                    prmtrs.Add(new SqlParameter("@Unidad", r.Unidad));
                    prmtrs.Add(new SqlParameter("@Observaciones", r.Observaciones));
                    using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudiosolicitudResultado_set", prmtrs))
                    {
                        while (Resultado.Read())
                        {

                            var res = Resultado["Id"].ToString();
                            if (res != "0")
                                respuesta = true;
                        }
                        respuesta = true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public string TifftoPng(string Original, string nombre)
        {
            var name = Guid.NewGuid();
            var resultado = name + ".png";
            try
            {
                String path = ConfigurationManager.ConnectionStrings["Directorio"].ToString();
                var dir = VerificarRutaActual(path);
                System.Drawing.Bitmap.FromFile(Original)
               .Save(dir + @"\" + resultado, System.Drawing.Imaging.ImageFormat.Png);
               
                resultado = @"/Imagenes/" + DateTime.Now.Year.ToString() + @"/" + DateTime.Now.Month.ToString() + @"/" + resultado;
                File.Move(Original, dir + @"\" + name + "." + nombre);

            }
            catch (Exception error)
            {
                return resultado != "" ? resultado : "";
            }
            return resultado;
        }
        public string AviToMp4(string Original, string nombre)
        {
            var name = Guid.NewGuid();
            var resultado = name + ".mp4";
            try
            {
                String path = ConfigurationManager.ConnectionStrings["Directorio"].ToString();
                var dir = VerificarRutaActual(path);
                var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                ffMpeg.ConvertMedia(Original, dir + @"\" + resultado, Format.mp4);
                resultado = @"/Imagenes/" + DateTime.Now.Year.ToString() + @"/" + DateTime.Now.Month.ToString() + @"/" + resultado;
                File.Move(Original, dir + @"\" + name + "." + nombre);

            }
            catch (Exception error)
            {
                return "";
            }
            return resultado;
        }
        public string VerificarRutaActual(string path)
        {
            var dir = path + @"\Imagenes\" + DateTime.Now.Year.ToString() + @"\" + DateTime.Now.Month.ToString();
            if (!File.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }
        public bool CrearDepartamento(string Nombre)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.departamento_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearDeposito(string Nombre, int Existencia, string Indicaciones)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Existencia", Existencia));
                prmtrs.Add(new SqlParameter("@Indicaciones", Indicaciones));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.tipodeposito_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearInsumo(int Cantidad, int Proveedor, int TipoDeposito, int TotalCompra, int TotalPiezas, string Tipo, DateTime Fecha, int Laboratorio)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Cantidad", Cantidad));
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                prmtrs.Add(new SqlParameter("@Proveedor", Proveedor));
                prmtrs.Add(new SqlParameter("@TipoDeposito", TipoDeposito));
                prmtrs.Add(new SqlParameter("@TotalCompra", TotalCompra));
                prmtrs.Add(new SqlParameter("@TotalPiezas", TotalPiezas));
                prmtrs.Add(new SqlParameter("@Fecha", Fecha));
                prmtrs.Add(new SqlParameter("@Tipo", Tipo));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.insumo_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EnviarInsumo(int Cantidad, int Proveedor, int TipoDeposito, int TotalPiezas, string Tipo, DateTime Fecha, int Laboratorio, int LaboratorioEnvia)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Cantidad", Cantidad));
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                prmtrs.Add(new SqlParameter("@Proveedor", Proveedor));
                prmtrs.Add(new SqlParameter("@TipoDeposito", TipoDeposito));
                prmtrs.Add(new SqlParameter("@TotalPiezas", TotalPiezas));
                prmtrs.Add(new SqlParameter("@Fecha", Fecha));
                prmtrs.Add(new SqlParameter("@Tipo", Tipo));
                prmtrs.Add(new SqlParameter("@LaboratorioEnvia", LaboratorioEnvia));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.insumoEnvio_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearMuestra(string Nombre, string Indicaciones)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Indicaciones", Indicaciones));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.tipomuestra_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearEstudioMuestra(string Nombre, string ClaveEstudio)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Clave", ClaveEstudio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudiomuestra_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool ActualizarGeneralComponente(int Id, string General)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                prmtrs.Add(new SqlParameter("@General", General));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudiogeneralcomponente_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool ActualizarComponenteLista(int Id, string Nombre, string Orden)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Orden", Orden));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.elementocomponente_upd", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EiminarComponenteLista(int Id)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.elementocomponente_del", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool EiminarComponente(int Id)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.componente_del", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearLaboratorio(string Nombre, string Direccion, string Telefono, string Iniciales, string Responsable)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Direccion", Direccion));
                prmtrs.Add(new SqlParameter("@Telefono", Telefono));
                prmtrs.Add(new SqlParameter("@Iniciales", Iniciales));
                prmtrs.Add(new SqlParameter("@Responsable", Responsable));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.laboratorio_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearEmpresa(string Nombre, string Direccion, string Colonia, string Ciudad, string Telefono, string Telefono2, int ImprimirRecibo, int PagoDefault)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Direccion", Direccion));
                prmtrs.Add(new SqlParameter("@Colonia", Colonia));
                prmtrs.Add(new SqlParameter("@Ciudad", Ciudad));
                prmtrs.Add(new SqlParameter("@Telefono", Telefono));
                prmtrs.Add(new SqlParameter("@Telefono2", Telefono2));
                prmtrs.Add(new SqlParameter("@ImprimirRecibo", ImprimirRecibo));
                prmtrs.Add(new SqlParameter("@PagoDefault", PagoDefault));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Empresa_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool ActualizarEmpresa(int IdEmpresa, string Nombre, string Direccion, string Colonia, string Ciudad, string Telefono, string Telefono2, int ImprimirRecibo, int PagoDefault)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Id", IdEmpresa));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Empresa_Id", IdEmpresa));
                prmtrs.Add(new SqlParameter("@Direccion", Direccion));
                prmtrs.Add(new SqlParameter("@Colonia", Colonia));
                prmtrs.Add(new SqlParameter("@Ciudad", Ciudad));
                prmtrs.Add(new SqlParameter("@Telefono", Telefono));
                prmtrs.Add(new SqlParameter("@Telefono2", Telefono2));
                prmtrs.Add(new SqlParameter("@ImprimirRecibo", ImprimirRecibo));
                prmtrs.Add(new SqlParameter("@PagoDefault", PagoDefault));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Empresa_upd", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearTipoPago(string Nombre, string Clave)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Clave", Clave));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.tipopago_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearProveedor(string Nombre)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Proveedor_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool CrearEstudioGabinete(int Medico, int TipoEstudio, int Paciente, int Organizacion, int Usuario, int TipoSolicitud, string Observaciones, string PaseMedico, List<int> Estudios, bool Interpretacion, int Factura, string DatosFactura)
        {
            bool respuesta = false;
            try
            {
                long resultado = 0;
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@IdMedico", Medico));
                prmtrs.Add(new SqlParameter("@IdPaciente", Paciente));
                prmtrs.Add(new SqlParameter("@IdUsuario", Usuario));
                prmtrs.Add(new SqlParameter("@TipoSolicitud", TipoSolicitud));
                prmtrs.Add(new SqlParameter("@Observaciones", Observaciones));
                prmtrs.Add(new SqlParameter("@PaseMedico", PaseMedico));
                prmtrs.Add(new SqlParameter("@TipoEstudio", TipoEstudio));
                prmtrs.Add(new SqlParameter("@Interpretacion", Interpretacion));
                prmtrs.Add(new SqlParameter("@Organizacion", Organizacion));
                prmtrs.Add(new SqlParameter("@Factura", Factura));
                prmtrs.Add(new SqlParameter("@DatosFactura", DatosFactura));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudioGabinete_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        resultado = long.Parse(Resultado["Id"].ToString());
                        if (resultado != 0)
                        {
                            respuesta = true;
                            for (int i = 0; i < Estudios.Count; i++)
                            {
                                AsignarEstudioGabinete(Estudios[i], Usuario, resultado);
                            }
                        }
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public long CrearSolicitud(int Medico, int Paciente,int Empresa, int Usuario, string PaseMedico, string Observaciones, List<string> Estudio, int Laboratorio, int Urgencia, string ObservacionesUrgencia, int Factura, string DatosFactura)
        {
            long resultado = 0;
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@IdMedico", Medico));
                prmtrs.Add(new SqlParameter("@IdPaciente", Paciente));
                prmtrs.Add(new SqlParameter("@IdUsuario", Usuario));
                prmtrs.Add(new SqlParameter("@Observaciones", Observaciones));
                prmtrs.Add(new SqlParameter("@PaseMedico", PaseMedico));
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                prmtrs.Add(new SqlParameter("@Organizacion", Empresa));
                prmtrs.Add(new SqlParameter("@TipoUrgencia", Urgencia));
                prmtrs.Add(new SqlParameter("@ObservacionesUrgencia", ObservacionesUrgencia));
                prmtrs.Add(new SqlParameter("@Factura", Factura));
                prmtrs.Add(new SqlParameter("@DatosFactura", DatosFactura));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitud_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        resultado =long.Parse(Resultado["Id"].ToString());
                        var clave = Resultado["Clave"].ToString();
                        if (resultado != 0)
                        {
                            respuesta = true;
                            for (int i = 0; i < Estudio.Count; i++) {
                                AsignarEstudioSolicitud(clave, Estudio[i], Usuario, i + 1);
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return resultado;
        }
        public long ActualizarSolicitud(long Solicitud, int Medico, int Paciente, int Empresa, int Usuario, string PaseMedico, string Observaciones, int Laboratorio, int Urgencia, string ObservacionesUrgencia, int Factura, string DatosFactura)
        {
            long resultado = 0;
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@IdSolicitud", Solicitud));
                prmtrs.Add(new SqlParameter("@IdMedico", Medico));
                prmtrs.Add(new SqlParameter("@IdPaciente", Paciente));
                prmtrs.Add(new SqlParameter("@Observaciones", Observaciones));
                prmtrs.Add(new SqlParameter("@PaseMedico", PaseMedico));
                prmtrs.Add(new SqlParameter("@Organizacion", Empresa));
                prmtrs.Add(new SqlParameter("@TipoUrgencia", Urgencia));
                prmtrs.Add(new SqlParameter("@ObservacionesUrgencia", ObservacionesUrgencia));

                prmtrs.Add(new SqlParameter("@Factura", Factura));
                prmtrs.Add(new SqlParameter("@DatosFactura", DatosFactura));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitud_upd", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        resultado = Solicitud;
                        var clave = Resultado["Clave"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return resultado;
        }
        public long ActualizarSolicitudGabinete(long Solicitud, int Medico, int Paciente, int Empresa, int Usuario, string PaseMedico, string Observaciones, int Factura, string DatosFactura)
        {
            long resultado = 0;
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@IdSolicitud", Solicitud));
                prmtrs.Add(new SqlParameter("@IdMedico", Medico));
                prmtrs.Add(new SqlParameter("@IdPaciente", Paciente));
                prmtrs.Add(new SqlParameter("@Observaciones", Observaciones));
                prmtrs.Add(new SqlParameter("@PaseMedico", PaseMedico));
                prmtrs.Add(new SqlParameter("@Organizacion", Empresa));
                prmtrs.Add(new SqlParameter("@Factura", Factura));
                prmtrs.Add(new SqlParameter("@DatosFactura", DatosFactura));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.solicitudgabinete_upd", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        resultado = long.Parse(Resultado["Id"].ToString());
                        var clave = Resultado["Clave"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return resultado;
        }
        public string CrearCotizacion(string Nombre, string Paterno, string Materno, int Usuario, List<string> Estudio, int Laboratorio)
        {
            string resultado = "";
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Paterno", Paterno));
                prmtrs.Add(new SqlParameter("@Materno", Materno));
                prmtrs.Add(new SqlParameter("@Usuario", Usuario));
                prmtrs.Add(new SqlParameter("@Laboratorio", Laboratorio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.Cotizacion_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        resultado = Resultado["Clave"].ToString();
                        if (resultado != "")
                        {
                            respuesta = true;
                            for (int i = 0; i < Estudio.Count; i++)
                            {
                                AsignarEstudioCotizacion(resultado, Estudio[i], Usuario, i + 1);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return resultado;
        }
        public bool AsignarEstudioSolicitud(string Solicitud, string Estudio, int Usuario, int Orden)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClaveSolicitud", Solicitud));
                prmtrs.Add(new SqlParameter("@ClaveEstudio", Estudio));
                prmtrs.Add(new SqlParameter("@Usuario", Usuario));
                prmtrs.Add(new SqlParameter("@Orden", Orden));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudiosolicitud_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool AsignarEstudioGabinete(int Estudio, int Usuario, long IdEstudioGabinete)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@EstudioGabinete_Id", IdEstudioGabinete));
                prmtrs.Add(new SqlParameter("@Estudio_Id", Estudio));
                prmtrs.Add(new SqlParameter("@Usuario", Usuario));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudiosgabinete_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool AsignarEstudioCotizacion(string Cotizacion, string Estudio, int Usuario, int Orden)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClaveCotizacion",  Cotizacion));
                prmtrs.Add(new SqlParameter("@ClaveEstudio", Estudio));
                prmtrs.Add(new SqlParameter("@Orden", Orden));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudiocotizacion_set", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool ActualizarProveedor(int Id, string Nombre)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Proveedor_Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.proveedor_upd", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool ActualizarEstudio(int Estudio, int Muestra, int Deposito, int Departamento, string Nombre, string Clave, string Abreviatura, string Indicaciones, string Precio, string Unidad, string Urgencia, string Volumen, string Numero, int PermiteDescuento, int Perfil)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@IdEstudio", Estudio));
                prmtrs.Add(new SqlParameter("@IdDepartamento", Departamento));
                prmtrs.Add(new SqlParameter("@IdDeposito", Deposito));
                prmtrs.Add(new SqlParameter("@IdMuestra", Muestra));
                prmtrs.Add(new SqlParameter("@Nombre", Nombre));
                prmtrs.Add(new SqlParameter("@Clave", Clave));
                prmtrs.Add(new SqlParameter("@Abreviatura", Abreviatura));
                prmtrs.Add(new SqlParameter("@Indicaciones", Indicaciones));
                prmtrs.Add(new SqlParameter("@Precio", float.Parse(Precio)));
                prmtrs.Add(new SqlParameter("@Unidad", Unidad));
                prmtrs.Add(new SqlParameter("@Urgencia", Urgencia));
                prmtrs.Add(new SqlParameter("@Volumen", float.Parse(Volumen)));
                prmtrs.Add(new SqlParameter("@Numero", float.Parse(Numero)));
                prmtrs.Add(new SqlParameter("@PermiteDescuento", PermiteDescuento));
                prmtrs.Add(new SqlParameter("@Perfil", Perfil));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudio_upd", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        public bool AgregarEstudioPerfil(string ClavePerfil, string ClaveEstudio)
        {
            bool respuesta = false;
            try
            {
                List<Object> prmtrs = new List<Object>();
                prmtrs.Add(new SqlParameter("@ClaveEstudio", ClaveEstudio));
                prmtrs.Add(new SqlParameter("@ClavePerfil", ClavePerfil));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estudioperfil_set", prmtrs))
                {
                    while (Resultado.Read())
                    {

                        var res = Resultado["Id"].ToString();
                        if (res != "0")
                            respuesta = true;
                    }
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return respuesta;
        }
        private Estudio ConstruirEstudio(DataTableReader Resultado)
        {
            return new Estudio()
            {
                Id = int.Parse(Resultado["idEstudio"].ToString()),
                Nombre = Resultado["nombreEstudio"].ToString(),
                Abreviatura = Resultado["abreviaturaEstudio"].ToString(),
                ClaveEstudio = Resultado["claveEstudio"].ToString(),
                Indicaciones = Resultado["indicacionesEstudio"].ToString(),
                NumeroMuestras = Resultado["numeroMuestrasEstudio"].ToString() != "" ?  int.Parse(Resultado["numeroMuestrasEstudio"].ToString()) : 0,
                HojaNueva = Resultado["hojaNuevaEstudio"].ToString() != "" ? bool.Parse(Resultado["hojaNuevaEstudio"].ToString()) : false,
                PermiteDescuento = Resultado["permiteDescuentoEstudio"].ToString() != "" ? bool.Parse(Resultado["permiteDescuentoEstudio"].ToString()) : false,
                Perfil = Resultado["perfilEstudio"].ToString() != "" ?  bool.Parse(Resultado["perfilEstudio"].ToString()) : false,
                TiempoProceso = Resultado["tiempoProcesoEstudio"].ToString() != "" ? double.Parse(Resultado["tiempoProcesoEstudio"].ToString()) : 0.0,
                PrecioBase = Resultado["precioBaseEstudio"].ToString() != "" ? double.Parse(Resultado["precioBaseEstudio"].ToString()) : 0.0,
                UnidadProceso = Resultado["unidadProcesoEstudio"].ToString(),
                Urgencia = Resultado["urgenciaEstudio"].ToString() != "" ? int.Parse(Resultado["urgenciaEstudio"].ToString()) : 0,
                Volumen = Resultado["volumenEstudio"].ToString() != "" ? double.Parse(Resultado["volumenEstudio"].ToString()) : 0.0,
                UnidadMuestra = Resultado["unidadMuestraEstudio"].ToString(),
                 TipoDeposito = Resultado["idTipoDeposito"].ToString() != "0" ? ConstruirTipoDeposito(Resultado) : null,
                 TipoMuestra = Resultado["idTipoMuestra"].ToString() != "0" ? ConstruirTipoMuestra(Resultado) : null,
                 Departamento = Resultado["idDepartamento"].ToString() != "0" ?  ConstruirDepartamento(Resultado) : null,
                 
            };
        }
        private EstudioGabineteDetalle ConstruirEstudioGabineteDetalle(DataTableReader Resultado)
        {
            var estudio = new EstudioGabineteDetalle()
            {
                IdDetalle = long.Parse(Resultado["idEstudioGabineteDetalle"].ToString()),
                Id = int.Parse(Resultado["idEstudio"].ToString()),
                Nombre = Resultado["nombreEstudio"].ToString(),
                Abreviatura = Resultado["abreviaturaEstudio"].ToString(),
                ClaveEstudio = Resultado["claveEstudio"].ToString(),
                Indicaciones = Resultado["indicacionesEstudio"].ToString(),
                NumeroMuestras = Resultado["numeroMuestrasEstudio"].ToString() != "" ? int.Parse(Resultado["numeroMuestrasEstudio"].ToString()) : 0,
                HojaNueva = Resultado["hojaNuevaEstudio"].ToString() != "" ? bool.Parse(Resultado["hojaNuevaEstudio"].ToString()) : false,
                PermiteDescuento = Resultado["permiteDescuentoEstudio"].ToString() != "" ? bool.Parse(Resultado["permiteDescuentoEstudio"].ToString()) : false,
                Perfil = Resultado["perfilEstudio"].ToString() != "" ? bool.Parse(Resultado["perfilEstudio"].ToString()) : false,
                TiempoProceso = Resultado["tiempoProcesoEstudio"].ToString() != "" ? double.Parse(Resultado["tiempoProcesoEstudio"].ToString()) : 0.0,
                PrecioBase = Resultado["precioBaseEstudio"].ToString() != "" ? double.Parse(Resultado["precioBaseEstudio"].ToString()) : 0.0,
                UnidadProceso = Resultado["unidadProcesoEstudio"].ToString(),
                Urgencia = Resultado["urgenciaEstudio"].ToString() != "" ? int.Parse(Resultado["urgenciaEstudio"].ToString()) : 0,
                Volumen = Resultado["volumenEstudio"].ToString() != "" ? double.Parse(Resultado["volumenEstudio"].ToString()) : 0.0,
                UnidadMuestra = Resultado["unidadMuestraEstudio"].ToString(),
                TipoDeposito = Resultado["idTipoDeposito"].ToString() != "0" ? ConstruirTipoDeposito(Resultado) : null,
                TipoMuestra = Resultado["idTipoMuestra"].ToString() != "0" ? ConstruirTipoMuestra(Resultado) : null,
                Departamento = Resultado["idDepartamento"].ToString() != "0" ? ConstruirDepartamento(Resultado) : null,
                Observaciones = Resultado["observacionesEstudio"].ToString(),
                Adjunto = Resultado["AdjuntosEstudio"].ToString() != "" ? Resultado["AdjuntosEstudio"].ToString().Trim(' ').Split(',').ToList() : null,
                Resultado = Resultado["resultadoEstudio"].ToString(),
                FechaEnvio = Resultado["FechaEnvioEstudio"].ToString() != "" ? DateTime.Parse(Resultado["FechaEnvioEstudio"].ToString()) : new DateTime(),
                FechaResultado = Resultado["FechaResultadoEstudio"].ToString() != "" ? DateTime.Parse(Resultado["FechaResultadoEstudio"].ToString()) : new DateTime(),
                ObservacionesTecnico = Resultado["ObservacionesTecnicoEstudio"].ToString()
            };
            try {
                estudio.MedicoInterpretacion = ConstruirMedicoInterpretacion(Resultado);
            } catch (Exception error) { };
            return estudio;
        }
        private EstudioResultado ConstruirEstudioResultado(DataTableReader Resultado, string ClaveSolicitud)
        {
            var estudio= new EstudioResultado()
            {
                Id = int.Parse(Resultado["idEstudio"].ToString()),
                Nombre = Resultado["nombreEstudio"].ToString(),
                Abreviatura = Resultado["abreviaturaEstudio"].ToString(),
                ClaveEstudio = Resultado["claveEstudio"].ToString(),
                Indicaciones = Resultado["indicacionesEstudio"].ToString(),
                NumeroMuestras = Resultado["numeroMuestrasEstudio"].ToString() != "" ? int.Parse(Resultado["numeroMuestrasEstudio"].ToString()) : 0,
                HojaNueva = Resultado["hojaNuevaEstudio"].ToString() != "" ? bool.Parse(Resultado["hojaNuevaEstudio"].ToString()) : false,
                PermiteDescuento = Resultado["permiteDescuentoEstudio"].ToString() != "" ? bool.Parse(Resultado["permiteDescuentoEstudio"].ToString()) : false,
                Perfil = Resultado["perfilEstudio"].ToString() != "" ? bool.Parse(Resultado["perfilEstudio"].ToString()) : false,
                TiempoProceso = Resultado["tiempoProcesoEstudio"].ToString() != "" ? double.Parse(Resultado["tiempoProcesoEstudio"].ToString()) : 0.0,
                PrecioBase = Resultado["precioBaseEstudio"].ToString() != "" ? double.Parse(Resultado["precioBaseEstudio"].ToString()) : 0.0,
                UnidadProceso = Resultado["unidadProcesoEstudio"].ToString(),
                Urgencia = Resultado["urgenciaEstudio"].ToString() != "" ? int.Parse(Resultado["urgenciaEstudio"].ToString()) : 0,
                Volumen = Resultado["volumenEstudio"].ToString() != "" ? double.Parse(Resultado["volumenEstudio"].ToString()) : 0.0,
                UnidadMuestra = Resultado["unidadMuestraEstudio"].ToString(),
                TipoDeposito = Resultado["idTipoDeposito"].ToString() != "0" ? ConstruirTipoDeposito(Resultado) : null,
                TipoMuestra = Resultado["idTipoMuestra"].ToString() != "0" ? ConstruirTipoMuestra(Resultado) : null,
                Departamento = ConstruirDepartamento(Resultado),
                Genero = Resultado["generoPaciente"].ToString()
            };
            if (Resultado["edadPaciente"].ToString() != "") {
                estudio.Edad = int.Parse(Resultado["edadPaciente"].ToString());
            }
            else if (Resultado["fechaNacimientoPaciente"].ToString() != "") {
                estudio.Edad = DateTime.Now.Year - DateTime.Parse(Resultado["fechaNacimientoPaciente"].ToString()).Year;
            }
            if (estudio.Perfil == true) {
                estudio.Componentes = ConsultarComponentesEstudioPerfil(estudio.ClaveEstudio, ClaveSolicitud);
            }
            else{
                estudio.Componentes = ConsultarComponentesEstudio(estudio.ClaveEstudio);
            }
            try
            {
                estudio.IdDetalle = int.Parse(Resultado["idSolicitudDetalle"].ToString());
            }
            catch (Exception error) {
            };
            return estudio;
        }
        private EstudioResultado ConstruirEstudioResultado3(DataTableReader Resultado, string ClaveSolicitud)
        {
            var estudio = new EstudioResultado()
            {
                Id = int.Parse(Resultado["idEstudio"].ToString()),
                Nombre = Resultado["nombreEstudio"].ToString(),
                Abreviatura = Resultado["abreviaturaEstudio"].ToString(),
                ClaveEstudio = Resultado["claveEstudio"].ToString(),
                Indicaciones = Resultado["indicacionesEstudio"].ToString(),
                NumeroMuestras = Resultado["numeroMuestrasEstudio"].ToString() != "" ? int.Parse(Resultado["numeroMuestrasEstudio"].ToString()) : 0,
                HojaNueva = Resultado["hojaNuevaEstudio"].ToString() != "" ? bool.Parse(Resultado["hojaNuevaEstudio"].ToString()) : false,
                PermiteDescuento = Resultado["permiteDescuentoEstudio"].ToString() != "" ? bool.Parse(Resultado["permiteDescuentoEstudio"].ToString()) : false,
                Perfil = Resultado["perfilEstudio"].ToString() != "" ? bool.Parse(Resultado["perfilEstudio"].ToString()) : false,
                TiempoProceso = Resultado["tiempoProcesoEstudio"].ToString() != "" ? double.Parse(Resultado["tiempoProcesoEstudio"].ToString()) : 0.0,
                PrecioBase = Resultado["precioBaseEstudio"].ToString() != "" ? double.Parse(Resultado["precioBaseEstudio"].ToString()) : 0.0,
                UnidadProceso = Resultado["unidadProcesoEstudio"].ToString(),
                Urgencia = Resultado["urgenciaEstudio"].ToString() != "" ? int.Parse(Resultado["urgenciaEstudio"].ToString()) : 0,
                Volumen = Resultado["volumenEstudio"].ToString() != "" ? double.Parse(Resultado["volumenEstudio"].ToString()) : 0.0,
                UnidadMuestra = Resultado["unidadMuestraEstudio"].ToString(),
                TipoDeposito = Resultado["idTipoDeposito"].ToString() != "0" ? ConstruirTipoDeposito(Resultado) : null,
                TipoMuestra = Resultado["idTipoMuestra"].ToString() != "0" ? ConstruirTipoMuestra(Resultado) : null,
                Departamento = ConstruirDepartamento(Resultado),
                Genero = Resultado["generoPaciente"].ToString()
            };
            if (Resultado["edadPaciente"].ToString() != "")
            {
                estudio.Edad = int.Parse(Resultado["edadPaciente"].ToString());
            }
            else if (Resultado["fechaNacimientoPaciente"].ToString() != "")
            {
                estudio.Edad = DateTime.Now.Year - DateTime.Parse(Resultado["fechaNacimientoPaciente"].ToString()).Year;
            }
            if (estudio.Perfil == true)
            {
                estudio.Componentes = ConsultarComponentesEstudioPerfil(estudio.ClaveEstudio, ClaveSolicitud);
            }
            else
            {
                estudio.Componentes = ConsultarComponentesEstudio(estudio.ClaveEstudio);
            }
            try
            {
                estudio.IdDetalle = int.Parse(Resultado["idSolicitudDetalle"].ToString());
            }
            catch (Exception error)
            {
            };
            return estudio;
        }
        private EstudioResultado ConstruirEstudioResultado2(DataTableReader Resultado, string ClaveSolicitud)
        {
            var estudio = new EstudioResultado()
            {
                Id = int.Parse(Resultado["idEstudio"].ToString()),
                Nombre = Resultado["nombreEstudio"].ToString(),
                Abreviatura = Resultado["abreviaturaEstudio"].ToString(),
                ClaveEstudio = Resultado["claveEstudio"].ToString(),
                Indicaciones = Resultado["indicacionesEstudio"].ToString(),
                NumeroMuestras = Resultado["numeroMuestrasEstudio"].ToString() != "" ? int.Parse(Resultado["numeroMuestrasEstudio"].ToString()) : 0,
                HojaNueva = Resultado["hojaNuevaEstudio"].ToString() != "" ? bool.Parse(Resultado["hojaNuevaEstudio"].ToString()) : false,
                PermiteDescuento = Resultado["permiteDescuentoEstudio"].ToString() != "" ? bool.Parse(Resultado["permiteDescuentoEstudio"].ToString()) : false,
                Perfil = Resultado["perfilEstudio"].ToString() != "" ? bool.Parse(Resultado["perfilEstudio"].ToString()) : false,
                TiempoProceso = Resultado["tiempoProcesoEstudio"].ToString() != "" ? double.Parse(Resultado["tiempoProcesoEstudio"].ToString()) : 0.0,
                PrecioBase = Resultado["precioBaseEstudio"].ToString() != "" ? double.Parse(Resultado["precioBaseEstudio"].ToString()) : 0.0,
                UnidadProceso = Resultado["unidadProcesoEstudio"].ToString(),
                Urgencia = Resultado["urgenciaEstudio"].ToString() != "" ? int.Parse(Resultado["urgenciaEstudio"].ToString()) : 0,
                Volumen = Resultado["volumenEstudio"].ToString() != "" ? double.Parse(Resultado["volumenEstudio"].ToString()) : 0.0,
                UnidadMuestra = Resultado["unidadMuestraEstudio"].ToString(),
                TipoDeposito = Resultado["idTipoDeposito"].ToString() != "0" ? ConstruirTipoDeposito(Resultado) : null,
                TipoMuestra = Resultado["idTipoMuestra"].ToString() != "0" ? ConstruirTipoMuestra(Resultado) : null,
                Departamento = ConstruirDepartamento(Resultado),
                Genero = Resultado["generoPaciente"].ToString()
            };
            if (Resultado["edadPaciente"].ToString() != "")
            {
                estudio.Edad = int.Parse(Resultado["edadPaciente"].ToString());
            }
            else if (Resultado["fechaNacimientoPaciente"].ToString() != "")
            {
                estudio.Edad = DateTime.Now.Year - DateTime.Parse(Resultado["fechaNacimientoPaciente"].ToString()).Year;
            }
            try
            {
                estudio.IdDetalle = int.Parse(Resultado["idSolicitudDetalle"].ToString());
            }
            catch (Exception error)
            {
            };
            return estudio;
        }
        private EstudioResultado ConstruirEstudioResultadoCotizacion(DataTableReader Resultado, string ClaveCotizacion)
        {
            var estudio = new EstudioResultado()
            {
                Id = int.Parse(Resultado["idEstudio"].ToString()),
                Nombre = Resultado["nombreEstudio"].ToString(),
                Abreviatura = Resultado["abreviaturaEstudio"].ToString(),
                ClaveEstudio = Resultado["claveEstudio"].ToString(),
                Indicaciones = Resultado["indicacionesEstudio"].ToString(),
                NumeroMuestras = Resultado["numeroMuestrasEstudio"].ToString() != "" ? int.Parse(Resultado["numeroMuestrasEstudio"].ToString()) : 0,
                HojaNueva = Resultado["hojaNuevaEstudio"].ToString() != "" ? bool.Parse(Resultado["hojaNuevaEstudio"].ToString()) : false,
                PermiteDescuento = Resultado["permiteDescuentoEstudio"].ToString() != "" ? bool.Parse(Resultado["permiteDescuentoEstudio"].ToString()) : false,
                Perfil = Resultado["perfilEstudio"].ToString() != "" ? bool.Parse(Resultado["perfilEstudio"].ToString()) : false,
                TiempoProceso = Resultado["tiempoProcesoEstudio"].ToString() != "" ? double.Parse(Resultado["tiempoProcesoEstudio"].ToString()) : 0.0,
                PrecioBase = Resultado["precioBaseEstudio"].ToString() != "" ? double.Parse(Resultado["precioBaseEstudio"].ToString()) : 0.0,
                UnidadProceso = Resultado["unidadProcesoEstudio"].ToString(),
                Urgencia = Resultado["urgenciaEstudio"].ToString() != "" ? int.Parse(Resultado["urgenciaEstudio"].ToString()) : 0,
                Volumen = Resultado["volumenEstudio"].ToString() != "" ? double.Parse(Resultado["volumenEstudio"].ToString()) : 0.0,
                UnidadMuestra = Resultado["unidadMuestraEstudio"].ToString(),
                TipoDeposito = Resultado["idTipoDeposito"].ToString() != "0" ? ConstruirTipoDeposito(Resultado) : null,
                TipoMuestra = Resultado["idTipoMuestra"].ToString() != "0" ? ConstruirTipoMuestra(Resultado) : null,
                Departamento = ConstruirDepartamento(Resultado),
            };

            return estudio;
        }
        private ResultadoSolicitud ConstruirResultadoSolicitud(DataTableReader Resultado)
        {
            var estudio = new ResultadoSolicitud()
            {
                ClaveEstudio = Resultado["ClaveEstudio"].ToString(),
                ClaveSolicitud = Resultado["claveSolicitud"].ToString(),
                FueraRango = Resultado["FueraRango"].ToString() != "" ? bool.Parse(Resultado["FueraRango"].ToString()) : false,
                ClaveEstudioMain = Resultado["ClaveEstudioMain"].ToString(),
                MAX = Resultado["MAX"].ToString(),
                MIN = Resultado["MIN"].ToString(),
                NombreEstudio  = Resultado["NombreEstudio"].ToString(),
                Normal = Resultado["Normal"].ToString(),
                Resultado = Resultado["Resultado"].ToString(),
                Imprimir = Resultado["Imprimir"].ToString() != "" ? bool.Parse(Resultado["Imprimir"].ToString()) : false,
                Negrita = Resultado["Negrita"].ToString() != "" ? bool.Parse(Resultado["Negrita"].ToString()) : false,
                Referencia = Resultado["Referencia"].ToString(),
                Titulo = Resultado["Titulo"].ToString(),
                Unidad = Resultado["Unidad"].ToString(),
                Observaciones = Resultado["Observaciones"].ToString(),
                TipoCaptura = Resultado["TipoCaptura"].ToString(),
                LineasImpresion = Resultado["LineasImpresion"].ToString(),
                Capturado = Resultado["Capturado"].ToString(),
                Ruta = Resultado["Ruta"].ToString(),
            };
            try { estudio.IdResultado = long.Parse(Resultado["Id"].ToString()); } catch (Exception error) { };
            try { estudio.EstudioComponente = int.Parse(Resultado["EstudioComponente_Id"].ToString()); } catch (Exception error) { };
            return estudio;
        }
        private Componente ConstruirComponente(DataTableReader Resultado)
        {
            var comp= new Componente()
            {
                Id = int.Parse(Resultado["idEstudioComponente"].ToString()),
                Nombre = Resultado["nombre"].ToString(),
                Abreviatura = Resultado["abreviatura"].ToString(),
                ClaveEstudio = Resultado["claveEstudio"].ToString(),
                Indicaciones = Resultado["indicacion"].ToString(),
                MinG = Resultado["MinG"].ToString() != "" ? Double.Parse(Resultado["MinG"].ToString()) : 0,
                MaxG = Resultado["MaxG"].ToString() != "" ? Double.Parse(Resultado["MaxG"].ToString()) : 0,
                MinH = Resultado["MinH"].ToString() != "" ? Double.Parse(Resultado["MinH"].ToString()) : 0,
                MaxH = Resultado["MaxH"].ToString() != "" ? Double.Parse(Resultado["MaxH"].ToString()) : 0,
                MinM = Resultado["MinM"].ToString() != "" ? Double.Parse(Resultado["MinM"].ToString()) : 0,
                MaxM = Resultado["MaxM"].ToString() != "" ? Double.Parse(Resultado["MaxM"].ToString()) : 0,
                MinN = Resultado["MinN"].ToString() != "" ? Double.Parse(Resultado["MinN"].ToString()) : 0,
                MaxN = Resultado["MaxN"].ToString() != "" ? Double.Parse(Resultado["MaxN"].ToString()) : 0,
                MinRN = Resultado["MinRN"].ToString() != "" ? Double.Parse(Resultado["MinRN"].ToString()) : 0,
                MaxRN = Resultado["MaxRN"].ToString() != "" ? Double.Parse(Resultado["MaxRN"].ToString()) : 0,
                UsarValoresGenerales = Resultado["UsarValoresGenerales"].ToString() != "" ? bool.Parse(Resultado["UsarValoresGenerales"].ToString()) : false,
                PermiteDesfaseValores = Resultado["PermiteDesfaseValores"].ToString() != "" ? bool.Parse(Resultado["PermiteDesfaseValores"].ToString()) : false,
                Unidad = Resultado["unidad"].ToString(),
                Orden = Resultado["Orden"].ToString(),
                TextoNormalGeneral = Resultado["TextoNormalGeneral"].ToString(),
                TextoNormalHombres = Resultado["TextoNormalHombres"].ToString(),
                TextoNormalMujeres = Resultado["TextoNormalMujeres"].ToString(),
                TextoNormalNiños = Resultado["TextoNormalNiños"].ToString(),
                TextoNormalRecienNacido = Resultado["TextoNormalRecienNacido"].ToString(),
                TipoCaptura = Resultado["idtipocaptura"].ToString() != "0" ? ConstruirTipoCaptura(Resultado) : null
            };

            try
            {
                comp.Elementos = ConsultarElementosComponente(comp.Id);
            }
            catch (Exception error) { }
            return comp;
        }
        private Componente ConstruirComponenteResultado(DataTableReader Resultado, string solicitud)
        {
            
            var comp = new Componente()
            {
                Id = int.Parse(Resultado["idEstudioComponente"].ToString()),
                Nombre = Resultado["Nombre"].ToString(),
                Abreviatura = Resultado["Abreviatura"].ToString(),
                ClaveEstudio = Resultado["claveEstudio"].ToString(),
                Indicaciones = Resultado["indicacion"].ToString(),
                MinG = Resultado["MinG"].ToString() != "" ? Double.Parse(Resultado["MinG"].ToString()) : 0,
                MaxG = Resultado["MaxG"].ToString() != "" ? Double.Parse(Resultado["MaxG"].ToString()) : 0,
                MinH = Resultado["MinH"].ToString() != "" ? Double.Parse(Resultado["MinH"].ToString()) : 0,
                MaxH = Resultado["MaxH"].ToString() != "" ? Double.Parse(Resultado["MaxH"].ToString()) : 0,
                MinM = Resultado["MinM"].ToString() != "" ? Double.Parse(Resultado["MinM"].ToString()) : 0,
                MaxM = Resultado["MaxM"].ToString() != "" ? Double.Parse(Resultado["MaxM"].ToString()) : 0,
                MinN = Resultado["MinN"].ToString() != "" ? Double.Parse(Resultado["MinN"].ToString()) : 0,
                MaxN = Resultado["MaxN"].ToString() != "" ? Double.Parse(Resultado["MaxN"].ToString()) : 0,
                MinRN = Resultado["MinRN"].ToString() != "" ? Double.Parse(Resultado["MinRN"].ToString()) : 0,
                MaxRN = Resultado["MaxRN"].ToString() != "" ? Double.Parse(Resultado["MaxRN"].ToString()) : 0,
                UsarValoresGenerales = Resultado["UsarValoresGenerales"].ToString() != "" ? bool.Parse(Resultado["UsarValoresGenerales"].ToString()) : false,
                PermiteDesfaseValores = Resultado["PermiteDesfaseValores"].ToString() != "" ? bool.Parse(Resultado["PermiteDesfaseValores"].ToString()) : false,
                Unidad = Resultado["unidad"].ToString(),
                Orden = Resultado["Orden"].ToString(),
                TextoNormalGeneral = Resultado["TextoNormalGeneral"].ToString(),
                TextoNormalHombres = Resultado["TextoNormalHombres"].ToString(),
                TextoNormalMujeres = Resultado["TextoNormalMujeres"].ToString(),
                TextoNormalNiños = Resultado["TextoNormalNiños"].ToString(),
                TextoNormalRecienNacido = Resultado["TextoNormalRecienNacido"].ToString(),
                TipoCaptura = Resultado["idtipocaptura"].ToString() != "0" ? ConstruirTipoCaptura(Resultado) : null
            };
            try { comp.N = int.Parse(Resultado["N"].ToString()); } catch (Exception error) { };
            try
            {
                if (comp.TipoCaptura.Id == 4)
                    comp.Elementos = ConsultarElementosComponente(comp.Id);
            }
            catch (Exception error) { }
            try
            {
                comp.Resultado = ConsultarResultadoComponenteSolicitud(comp.Id,comp.ClaveEstudio, solicitud);
            }
            catch (Exception error) { }
            return comp;
        }
        public Estado ConsultarEstado(int IdEstado)
        {
            Estado estado = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Estado_Id", IdEstado));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estado_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        estado = ConstruirEstado(Resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return estado;
        }
        public List<Estado> ConsultarEstadoPais(int IdPais)
        {
            List<Estado> estado = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Pais_Id", IdPais));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estadoPais_get", prmtrs))
                {
                    estado = new List<Estado>();
                    while (Resultado.Read())
                    {
                        estado.Add(ConstruirEstado(Resultado));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return estado;
        }
        public List<Estado> ConsultarEstadoColeccion()
        {
            List<Estado> estados = null;
            try
            {
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.estadoColeccion_get", null))
                {
                    estados = new List<Estado>();
                    while (Resultado.Read())
                    {
                        var estado = ConstruirEstado(Resultado);
                        estados.Add(estado);
                    }

                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return estados;
        }
        private Estado ConstruirEstado(DataTableReader Resultado)
        {
            return new Estado()
            {
                Id = int.Parse(Resultado["idEstado"].ToString()),
                Nombre = Resultado["nombreEstado"].ToString(),
                //Pais = new Pais()
                //{
                //    Id = int.Parse(Resultado["idPais"].ToString()),
                //    Nombre = Resultado["nombrePais"].ToString(),
                //    Abreviatura = Resultado["abreviaturaPais"].ToString(),
                //}
            };
        }

        private Estado ConstruirEstadoBase(DataTableReader Resultado)
        {
            return new Estado()
            {
                Id = int.Parse(Resultado["idEstado"].ToString()),
                Nombre = Resultado["nombreEstado"].ToString()
            };
        }
        public List<Colonia> ConsultarColoniaMunicipioColeccion(int Id)
        {
            List<Colonia> colonias = new List<Colonia>();
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Municipio_Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.coloniaColeccionPorMunicipio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        Colonia registro = new Colonia()
                        {
                            Id = int.Parse(Resultado["idColonia"].ToString()),
                            Nombre = Resultado["nombreColonia"].ToString(),
                            CodigoPostal = Resultado["codigoPostalColonia"].ToString(),

                        };
                        colonias.Add(registro);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return colonias;
        }
        public List<Colonia> ConsultarColoniaEstadoColeccion(int Id)
        {
            List<Colonia> colonias = new List<Colonia>();
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Estado_Id", Id));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.coloniaColeccionPorEstado_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        Colonia registro = new Colonia()
                        {
                            Id = int.Parse(Resultado["idColonia"].ToString()),
                            Nombre = Resultado["nombreColonia"].ToString(),
                            CodigoPostal = Resultado["codigoPostalColonia"].ToString(),

                        };
                        colonias.Add(registro);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return colonias;
        }

        //Municipio

        public Colonia ConsultarColonia(int IdColonia)
        {
            Colonia Colonia = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Colonia_Id", IdColonia));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.colonia_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        Colonia = ConstruirColonia(Resultado);

                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return Colonia;
        }
        public Municipio ConsultarMunicipio(int IdMunicipio)
        {
            Municipio municipio = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@IdCiudad", IdMunicipio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.municipio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        municipio = ConstruirMunicipio(Resultado);

                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return municipio;
        }
        public Municipio ConsultarMunicipioBase(int IdMunicipio)
        {
            Municipio municipio = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@IdCiudad", IdMunicipio));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.municipio_get", prmtrs))
                {
                    while (Resultado.Read())
                    {
                        municipio = ConstruirMunicipioBase(Resultado);

                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return municipio;
        }
        public List<Municipio> ConsultarMunicipioEstado(int IdEstado)
        {
            List<Municipio> municipios = null;
            try
            {
                List<object> prmtrs = new List<object>();
                prmtrs.Add(new SqlParameter("@Estado_Id", IdEstado));
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.ciudadEstado_get", prmtrs))
                {
                    municipios = new List<Municipio>();
                    while (Resultado.Read())
                    {
                        var municipio = ConstruirMunicipio(Resultado);
                        municipios.Add(municipio);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return municipios;
        }
        public List<Municipio> ConsultarMunicipioColeccion()
        {
            List<Municipio> municipios = null;
            try
            {
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.ciudadColeccion_get", null))
                {
                    municipios = new List<Municipio>();
                    while (Resultado.Read())
                    {
                        var municipio = ConstruirMunicipio(Resultado);
                        municipios.Add(municipio);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return municipios;
        }
        private Municipio ConstruirMunicipio(DataTableReader Resultado)
        {
            return new Municipio()
            {
                Id = int.Parse(Resultado["idCiudad"].ToString()),
                Nombre = Resultado["nombreCiudad"].ToString(),
                Clave = int.Parse(Resultado["claveCiudad"].ToString()),
                Estado = ConstruirEstado(Resultado)
            };
        }

        private Municipio ConstruirMunicipioBase(DataTableReader Resultado)
        {
            return new Municipio()
            {
                Id = int.Parse(Resultado["idCiudad"].ToString()),
                Nombre = Resultado["nombreCiudad"].ToString(),
                Clave = int.Parse(Resultado["claveCiudad"].ToString()),
                Estado = ConstruirEstadoBase(Resultado)
            };
        }
        public List<Acceso> ConsultarAccesos()
        {
            List<Acceso> accesos = null;
            try
            {
                using (var Resultado = base.conexion.EjecutarReaderStoreProcedure("dbo.accesos_get", null))
                {
                    accesos = new List<Acceso>();
                    while (Resultado.Read())
                    {
                        var acceso = ConstruirAcceso(Resultado);
                        accesos.Add(acceso);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
            return accesos;
        }
        private Acceso ConstruirAcceso(DataTableReader Resultado)
        {
            return new Acceso()
            {
                Id = int.Parse(Resultado["IdChekin"].ToString()),
                IdUsuario = int.Parse(Resultado["IdUsuario"].ToString()),
                IdLaboratorio = int.Parse(Resultado["IdLaboratorio"].ToString()),
                NombreUsuario = Resultado["NombreUsuario"].ToString(),
                NombreLaboratorio = Resultado["NombreLaboratorio"].ToString(),
                Maquina = Resultado["Maquina"].ToString(),
                Ip = Resultado["Ip"].ToString(),
                Fecha = DateTime.Parse(Resultado["Fecha"].ToString())
            };
        }
    }
}


