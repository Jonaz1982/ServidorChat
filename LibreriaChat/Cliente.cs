using System;
using System.Runtime.Serialization;

[DataContract]
public class Cliente
{

    [DataMember]
    public int Id;

    [DataMember]
    public String Usuario;

    [DataMember]
    public byte[] Foto;

    [DataMember]
    public String Pais;

    [DataMember]
    public byte[] Bandera;



}