using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace clEntidad
{
    public class eUsuario
    {

        Int32 codigo;

        public Int32 Codigo
        {
            get { return codigo; }
        }

        public Int32 CodEmpresa { get; set; }
        public String NombreUsuario{get;set;}
        public String NombreCompleto{get;set;}
        public String Clave{get;set;}
        public DateTime FechaRegistro{get;set;}
        public Int32 EstadoActivo{get;set;}      
        

    }





}
