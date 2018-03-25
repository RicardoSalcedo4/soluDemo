using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using clEntidad;
namespace clDato
{
    public class dUsuario
    {
        public Database idb_conexion;              
       
        /// <summary>
        /// Listar datos del usuario
        /// </summary>
        /// <param name="pEmpresa">Codigo de la empresa</param>
        /// <param name="pNombreUsuario">Nombre del usuario</param>
        /// <returns>[0]: 1=OK ó -1=Error ; [1]: Mensaje</returns>
        public String[] ListarDatosUsuario(Int32 pEmpresa,String pNombreUsuario, out List<eUsuario> pList_Usuario)
        {
            String[] lValor = new String[2];
            List<eUsuario> lList_Usuario = new List<eUsuario>();
            eUsuario lUsuario = null;
            lValor[0] = "-1";
            lValor[1] = "";
            pList_Usuario = null;
            try
            {
                if (idb_conexion.GetType().Name == "OracleDatabase")
                {
                    using (DbCommand cmd = idb_conexion.GetStoredProcCommand("DEMO.USP_USUARIO_LISTAR"))
                    {
                        idb_conexion.AddInParameter(cmd, "XX_EMPRESA", DbType.Int32, pEmpresa);
                        idb_conexion.AddInParameter(cmd, "XX_NUMUSU", DbType.String, pNombreUsuario);
                        cmd.Parameters.Add(dConexion.CreaParametroCursor("RC1"));
                        using (IDataReader ldr = idb_conexion.ExecuteReader(cmd))
                        {
                            while (ldr.Read())
                            {
                                lUsuario = new eUsuario();
                                lUsuario.Codigo = Int32.Parse(ldr["CODIGO"].ToString());
                                lUsuario.CodEmpresa = Int32.Parse(ldr["COD_EMPRESA"].ToString());
                                lUsuario.EstadoActivo = Int32.Parse(ldr["ESTADO_ACTIVO"].ToString());
                                lUsuario.FechaRegistro = DateTime.Parse(ldr["FECHA_REGISTRO"].ToString());
                                lUsuario.NombreCompleto = ldr["NOMBRE_COMPLETO"].ToString();
                                lUsuario.NombreUsuario = ldr["NOMBRE_USUARIO"].ToString();
                                lUsuario.Clave = ldr["CLAVE"].ToString();
                                lList_Usuario.Add(lUsuario);
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
                pList_Usuario = lList_Usuario;
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
        public String[] InsertarUsuario(Int32 pEmpresa, String pNombreUsuario, String pNombreCompleto, DateTime pFecha, DbTransaction ptrans_conexion)
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
                                idb_conexion.AddInParameter(cmd, "XX_EMPRESA", DbType.Int32, pEmpresa);
                                idb_conexion.AddInParameter(cmd, "XX_NOMBRE_USUARIO", DbType.String, pNombreUsuario);
                                idb_conexion.AddInParameter(cmd, "XX_NOMBRE_COMPLETO", DbType.Int32, pNombreCompleto);
                                idb_conexion.AddInParameter(cmd, "XX_FECHA", DbType.Date, pFecha);
               
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
        /// Insertar Sede del usuario
        /// </summary>
        /// <param name="pEmpresa">Codigo de la empresa</param>
        /// <param name="pUsuario">Codigo del usuario</param>
        /// <param name="pSede">Codigo de la sede</param>
        /// <returns>[0]: 1=OK ó -1=Error ; [1]: Mensaje</returns>
        public String[] InsertarSedeUsuario(Int32 pEmpresa, Int32 pUsuario, Int32 pSede, DbTransaction ptrans_conexion)
        {
            String[] lValor = new String[2];
            String lExeValor, lExeMensaje;
            lValor[0] = "-1";
            lValor[1] = "";

            try
            {
                if (idb_conexion.GetType().Name == "OracleDatabase")
                {
                    using (DbCommand cmd = idb_conexion.GetStoredProcCommand(is_oracle_esquema + ".SP_USUARIO_SEDE_INSERTAR"))
                    {
                        idb_conexion.AddInParameter(cmd, "XX_EMPRESA", DbType.Int32, pEmpresa);
                        idb_conexion.AddInParameter(cmd, "XX_USUARIO", DbType.Int32, pUsuario);
                        idb_conexion.AddInParameter(cmd, "XX_SEDE", DbType.Int32, pSede);                       

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


        
    }
}
