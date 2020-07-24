using System;
using System.Runtime.Serialization;
using System.ServiceModel;

 [DataContract]
   public class FileMessage
    {
        [DataMember]
        public DateTime Fecha;

        [DataMember]
        public String Remitente;

        [DataMember]
        public String Contenido;

        [DataMember]
        public string NombreArchivo;

        [DataMember]
        public byte[] Datos;
    }