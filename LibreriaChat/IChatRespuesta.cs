using System.ServiceModel;
using System.Collections.Generic;


public interface IChatRespuesta
{
    [OperationContract(IsOneWay = true)]
    void RefrescarClientes(List<Cliente> Clientes);

    [OperationContract(IsOneWay = true)]
    void Recibir(Mensaje m);

    [OperationContract(IsOneWay = true)]
    void EstaEscribiendoRespuesta(Cliente c);

    [OperationContract(IsOneWay = true)]
    void Respuesta(Mensaje msg);

    [OperationContract(IsOneWay = true)]
    void RecibirArchivo(FileMessage fMsg);

    [OperationContract(IsOneWay = true)]
    void Unirse(Cliente c);

    [OperationContract(IsOneWay = true)]
    void Dejar(Cliente c);


    
}