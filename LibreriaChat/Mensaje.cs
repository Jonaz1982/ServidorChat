using System;
using System.Runtime.Serialization;


[DataContract]
public class Mensaje
{

    [DataMember]
    public DateTime Fecha;

    [DataMember]
    public String Remitente;

    [DataMember]
    public String Contenido;

    [DataMember]
    public byte[] Emoticon;

    [DataMember]
    public byte[] Imagen;

    [DataMember]
    public String Enviado;
}

