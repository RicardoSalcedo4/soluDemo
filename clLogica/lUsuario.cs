using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using clEntidad;
using clDato;
using System.Data.Common;
using clEntidad;
namespace clLogica
{
    public class lUsuario:eUsuario
    {
         dUsuario idUsuario;
         DbConnection icnn_bd;

         void lUsuario(Database pdb_conexion)
         {
             idUsuario = new dUsuario();
             idUsuario.idb_conexion = pdb_conexion;             
         }

        /// <summary>
        /// Validar usuario y clave 
        /// </summary>
        /// <param name="pEmpresa">Codigo de la empresa</param>
        /// <param name="pNombreUsuario">Nombre del usuario</param>
        /// <param name="pClaveUsuario">Clave del usuario</param>
        /// <returns>[0]: 1=OK ó -1=Error ; [1]: Mensaje </returns>
        public String[] ValidarUsuarioClave(Int32 pEmpresa, String pNombreUsuario, String pClaveUsuario)
        {
            String[] lValor = new String[2];
            String[] lsReturn ;
            List<eUsuario> lListUsuario ;
            lValor[0] = "-1";
            lValor[1] = "";

            if (pEmpresa <= 0)
            {
                    lValor[0] = "-1";
                    lValor[1] = "Debe ingresar la Empresa";
                    return lValor;
            }
            if (pNombreUsuario.Trim().Length == 0)
            {
                    lValor[0] = "-1";
                    lValor[1] = "Debe ingresar el nombre del usuario";
                    return lValor;
            }
            if (pClaveUsuario.Trim().Length == 0)
            {
                    lValor[0] = "-1";
                    lValor[1] = "Debe ingresar la clave del usuario";
                    return lValor;
            }

            try
            {
                    lsReturn = idUsuario.ListarDatosUsuario(pEmpresa, pNombreUsuario, out lListUsuario);
                    if (lsReturn[0] != "1")
                    {
                        lValor[0] = "-1";
                        lValor[1] = lsReturn[1];
                        return lValor;
                    }

                    if (lListUsuario.Count == 0)
                    {
                        lValor[0] = "-1";
                        lValor[1] = "Usuario no existe";
                        return lValor;
                    }

                    if (lListUsuario.Count > 1)
                    {
                        lValor[0] = "-1";
                        lValor[1] = "Hay mas de un usuario";
                        return lValor;
                    }

                    if (lListUsuario[0].Clave != pClaveUsuario)
                    {
                        lValor[0] = "-1";
                        lValor[1] = "Clave invalida";
                        return lValor;
                    }

                    
                    lValor[0] = "1";
                    lValor[1] = "OK";
            }
            catch (Exception ex)
            {
                    lValor[0] = "-1";
                    lValor[1] ="lUsuario.ValidarDatosUsuario: " +  ex.Message;
                    return lValor;
            }

  
            return lValor;
        }

        /// <summary>
        /// Insertar datos del usuario
        /// </summary>
        /// <returns>[0]: 1=OK ó -1=Error ; [1]: Mensaje</returns>
        public String[] Insertar()
        {
            DbTransaction ltrans1 =null;
            Int32 li_CodigoUsuario;
            String[] lValor = new String[2];
            String[] lResult = new String[2];
            lValor[0] = "-1";
            lValor[1] = "";

            //Validar datos
            if (base.CodEmpresa <= 0)
            {
                lValor[0] = "-1";
                lValor[1] = "Debe ingresar la Empresa";
                return lValor;
            }
            if (base.NombreUsuario.Trim().Length == 0)
            {
                lValor[0] = "-1";
                lValor[1] = "Debe ingresar el nombre del usuario";
                return lValor;
            }
            if (base.NombreCompleto.Trim().Length == 0)
            {
                lValor[0] = "-1";
                lValor[1] = "Debe ingresar el nombre del completo del usuario";
                return lValor;
            }
            if (base.Clave.Trim().Length == 0)
            {
                lValor[0] = "-1";
                lValor[1] = "Debe ingresar la clave del usuario";
                return lValor;
            }
            if (base.FechaRegistro <= DateTime.Parse("01/01/1901"))
            {
                lValor[0] = "-1";
                lValor[1] = "Debe ingresar la fecha de registro";
                return lValor;
            }      

            
            //Grabar Datos
            try
            {
                icnn_bd = idUsuario.idb_conexion.CreateConnection();
                icnn_bd.Open();
                ltrans1 = icnn_bd.BeginTransaction();

                lResult = idUsuario.InsertarUsuario(base.CodEmpresa, base.NombreUsuario, base.NombreCompleto, base.FechaRegistro, ltrans1);
                if (lResult[0] != "1")
                {
                    vi_Commit_Rollback(ltrans1, false, icnn_bd);    
                    lValor[0] = "-1";
                    lValor[1] = lResult[1];
                    return lValor;
                }
                li_CodigoUsuario = Int32.Parse(lResult[1]);

                foreach (eSede RegSede in base.ItemSede)
                {
                    lResult = idUsuario.InsertarSedeUsuario(base.CodEmpresa, li_CodigoUsuario, RegSede.Codigo, ltrans1);
                    if (lResult[0] != "1")
                    {
                        vi_Commit_Rollback(ltrans1, false, icnn_bd);
                        lValor[0] = "-1";
                        lValor[1] = lResult[1];
                        return lValor;
                    }
                }

                vi_Commit_Rollback(ltrans1, true, icnn_bd);
                lValor[0] = "1";
                lValor[1] = li_CodigoUsuario.ToString();
                return lValor;
            }
            catch (Exception ex)
            {
                vi_Commit_Rollback(ltrans1, false, icnn_bd);
                lValor[0] = "-1";
                lValor[1] = "lUsuario.Insertar: " + ex.Message;
                return lValor;
            
            }
           
        }

        void vi_Commit_Rollback(DbTransaction ptrans_conexion1,Boolean pCommit, DbConnection pcnn_bd)
        {
            if (ptrans_conexion1 != null)
            {
                if (pCommit)
                {
                    ptrans_conexion1.Commit();
                }
                else
                {
                    ptrans_conexion1.Rollback();
                }
                    
                ptrans_conexion1.Dispose();
                ptrans_conexion1 = null;
            }

            if (pcnn_bd != null)
            {
                 pcnn_bd.Close();
                 pcnn_bd.Dispose();
                 pcnn_bd = null;
            }
           
            
        }

    }
}
