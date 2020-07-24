using System.ServiceModel;



[ServiceContract(CallbackContract = typeof(IChatRespuesta),
                             SessionMode = SessionMode.Required)]
public interface IChat
{
    [OperationContract(IsInitiating = true)]
    bool Conectar(Cliente c);

    [OperationContract(IsOneWay = true)]
    void Enviar(Mensaje m);

    [OperationContract(IsOneWay = true)]
    void EstaEscribiendo(Cliente c);

    [OperationContract(IsOneWay = true)]
    void EnviarArchivo(FileMessage fMgs);
    
    [OperationContract(IsOneWay = true, IsTerminating = true)]
    void Desconectar(Cliente c);
  
}
