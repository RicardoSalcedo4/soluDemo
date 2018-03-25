using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
namespace clDato
{
    public class dUsuario
    {
        Int32 codigo;

        public Int32 Codigo
        {
            get { return codigo; }         
        }


        public Database idb_conexion;       
        public Int32 CodEmpresa { get; set; }
        public String NombreUsuario { get; set; }
        public String NombreCompleto { get; set; }
        public String Clave { get; set; }
        public DateTime FechaRegistro { get; set; }
        public Int32 EstadoActivo { get; set; }



            
       
        /// <summary>
        /// Listar datos del usuario
        /// </summary>
        /// <param name="pEmpresa">Codigo de la empresa</param>
        /// <param name="pNombreUsuario">Nombre del usuario</param>
        /// <returns>[0]: 1=OK ó -1=Error ; [1]: Mensaje</returns>
        public String[] Listar(Int32 pEmpresa, Int32 pCodigoUsuario)
        {
            String[] lValor = new String[2];                   
            lValor[0] = "-1";
            lValor[1] = "";
           
            try
            {
                if (idb_conexion.GetType().Name == "OracleDatabase")
                {
                    using (DbCommand cmd = idb_conexion.GetStoredProcCommand("DEMO.USP_USUARIO_LISTAR"))
                    {
                        idb_conexion.AddInParameter(cmd, "XX_EMPRESA", DbType.Int32, pEmpresa);
                        idb_conexion.AddInParameter(cmd, "XX_USUARIO", DbType.Int32, pCodigoUsuario);
                        cmd.Parameters.Add(dConexion.CreaParametroCursor("RC1"));
                        using (IDataReader ldr = idb_conexion.ExecuteReader(cmd))
                        {
                            while (ldr.Read())
                            {
                                codigo = Int32.Parse(ldr["CODIGO"].ToString());
                                CodEmpresa = Int32.Parse(ldr["COD_EMPRESA"].ToString());
                                EstadoActivo = Int32.Parse(ldr["ESTADO_ACTIVO"].ToString());
                                FechaRegistro = DateTime.Parse(ldr["FECHA_REGISTRO"].ToString());
                                NombreCompleto = ldr["NOMBRE_COMPLETO"].ToString();
                                NombreUsuario = ldr["NOMBRE_USUARIO"].ToString();
                                Clave = ldr["CLAVE"].ToString();

                                lValor[0] = "1";
                                lValor[1] = "OK";
                                return lValor;    
                            }
                        }
                    }
                }
                else
                {
                    lValor[0] = "-1";
                    lValor[1] = "Base de datos sin definir: " + idb_conexion.GetType().Name;                   
                    return lValor;
                }

                lValor[0] = "1";
                lValor[1] = "OK";                
                return lValor;     
            }
            catch(Exception ex) 
            {
                lValor[0] = "-1";
                lValor[1] = "dUsuario.ListarDatosUsuario: " + ex.Message;
                return lValor;
            }             
        }

        /// <summary>
        /// Insertar los datos del usuario       
        /// </summary>
        /// <param name="pEmpresa">Codigo de la empresa</param>
        /// <param name="pNombreUsuario">Nombre del usuario</param>
        /// <param name="pNombreCompleto">Nombre completo</param>
        /// <param name="pFecha">Fecha de ingreso</param>
        /// <returns>[0]: 1=OK ó -1=Error ; [1]: Mensaje</returns>       
        public String[] Insertar( DbTransaction ptrans_conexion)
        {
            String[] lValor = new String[2];
            String lExeValor, lExeMensaje;
            lValor[0] = "-1";
            lValor[1] = "";

            try
            {
                    if (idb_conexion.GetType().Name == "OracleDatabase")
                    {
                            using (DbCommand cmd = idb_conexion.GetStoredProcCommand("DEMO.SP_USUARIO_INSERTAR"))
                            {
                                idb_conexion.AddInParameter(cmd, "XX_EMPRESA", DbType.Int32, CodEmpresa);
                                idb_conexion.AddInParameter(cmd, "XX_NOMBRE_USUARIO", DbType.String, NombreUsuario);
                                idb_conexion.AddInParameter(cmd, "XX_CLAVE", DbType.String, Clave);
                                idb_conexion.AddInParameter(cmd, "XX_NOMBRE_COMPLETO", DbType.Int32, NombreCompleto);
                                idb_conexion.AddInParameter(cmd, "XX_FECHA", DbType.Date, FechaRegistro);
                                idb_conexion.AddInParameter(cmd, "XX_ACTIVO", DbType.Int32, EstadoActivo);

                                idb_conexion.AddOutParameter(cmd, "YY_VALOR", DbType.String, 2);
                                idb_conexion.AddOutParameter(cmd, "YY_MENSAJE", DbType.String, 4000);

                                idb_conexion.ExecuteNonQuery(cmd, ptrans_conexion);

                                lExeValor = idb_conexion.GetParameterValue(cmd, "YY_VALOR").ToString();
                                lExeMensaje = idb_conexion.GetParameterValue(cmd, "YY_MENSAJE").ToString();

                                if (lExeMensaje != "1")
                                {
                                     lValor[0] = "-1";
                                     lValor[1] = lExeMensaje;
                                     return lValor;                    
                                }

                                lValor[0] = "1";
                                lValor[1] = lExeMensaje;
                                return lValor;   

                            }
                    }
                   else
                   {
                       lValor[0] = "-1";
                       lValor[1] = "Base de datos sin definir: " + idb_conexion.GetType().Name;
                       return lValor;
                   }

           }
           catch (Exception ex)
           {
               lValor[0] = "-1";
               lValor[1] = ex.Message;
               return lValor;  
           }                   

         
        }

        /// <summary>
        /// Listar usuarios x nombre de usuario y clave
        /// </summary>
        /// <param name="pEmpresa">Codigo de la empresa</param>
        /// <param name="pNombreUsuario">Nombre del usuario</param>       
        /// <param name="pDataTable">Tabla de usuarios consultados</param>
        /// <returns>[0]: 1=OK ó -1=Error ; [1]: Mensaje</returns>
        public String[] Listar_xNombreUsuarioxClave(Int32 pEmpresa, String pNombreUsuario, out DataTable pDataTable)
        {
            String[] lValor = new String[2];
            pDataTable = new DataTable();
            lValor[0] = "-1";
            lValor[1] = "";

            try
            {
                if (idb_conexion.GetType().Name == "OracleDatabase")
                {
                    using (DbCommand cmd = idb_conexion.GetStoredProcCommand("DEMO.USP_USUARIO_LISTXNOMUSU_XCLAVE"))
                    {
                        idb_conexion.AddInParameter(cmd, "XX_EMPRESA", DbType.Int32, pEmpresa);
                        idb_conexion.AddInParameter(cmd, "XX_USUARIO", DbType.String, pNombreUsuario);                       

                        cmd.Parameters.Add(dConexion.CreaParametroCursor("RC1"));
                        using (IDataReader ldr = idb_conexion.ExecuteReader(cmd))
                        {
                            if (ldr.Read())
                            {
                                pDataTable.Load(ldr);
                                lValor[0] = "1";
                                lValor[1] = "OK";                                
                                return lValor;
                            }
                        }
                    }
                }
                else
                {
                    lValor[0] = "-1";
                    lValor[1] = "Base de datos sin definir: " + idb_conexion.GetType().Name;
                    return lValor;
                }

                lValor[0] = "1";
                lValor[1] = "OK";
                return lValor;
            }
            catch (Exception ex)
            {
                lValor[0] = "-1";
                lValor[1] = "dUsuario.ListarDatosUsuario: " + ex.Message;
                return lValor;
            }
        }
        
    }
}
