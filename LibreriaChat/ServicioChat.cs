using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;

[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     UseSynchronizationContext = false)]
public class ServicioChat : IChat
{
    Dictionary<Cliente, IChatRespuesta> Clientes =
            new Dictionary<Cliente, IChatRespuesta>();

    List<Cliente> ListaClientes = new List<Cliente>();

    string rcvFilesPath = @"C:/WCF_Received_Files/";

    public IChatRespuesta ActualRespuesta
    {
        get
        {
            return OperationContext.Current.
                   GetCallbackChannel<IChatRespuesta>();

        }
    }

    object ObjetoBloqueo = new object();

    //Metodo para verificar si un Usuario esta activo en el servicio
    private bool BuscarCliente(string Usuario)
    {
        foreach (Cliente c in Clientes.Keys)
        {
            if (c.Usuario == Usuario)
            {
                return true;
            }
        }
        return false;
    }


    #region IChat Miembros

    //Implementación método Conectar
    public bool Conectar(Cliente c)
    {
        if (!Clientes.ContainsValue(ActualRespuesta) &&
            !BuscarCliente(c.Usuario))
        {
            //bloqueo de exclusión mutua
            lock (ObjetoBloqueo)
            {
                Clientes.Add(c, ActualRespuesta);
                ListaClientes.Add(c);

                foreach (Cliente cl in Clientes.Keys)
                {
                    IChatRespuesta Respuesta = Clientes[cl];
                    try
                    {
                        Respuesta.RefrescarClientes(ListaClientes);
                        Respuesta.Unirse(c);
                    }
                    catch
                    {
                        Clientes.Remove(cl);
                        return false;
                    }

                }

            }
            return true;
        }
        return false;
    }

    public void Enviar(Mensaje m)
    {
        //bloqueo de exclusión mutua
        lock (ObjetoBloqueo)
        {
            foreach (IChatRespuesta r in Clientes.Values)
            {
                r.Recibir(m);
            }
        }
    }

    public void EnviarArchivo(FileMessage fileMsg)
    {
        foreach (Cliente rcvr in Clientes.Values)
            {
                Mensaje ms = new Mensaje();
                ms.Remitente = fileMsg.Remitente;
                ms.Contenido = "Enviando archivo: " + fileMsg.NombreArchivo;

                IChatRespuesta rcvCall = Clientes[rcvr];
                rcvCall.RecibirArchivo(fileMsg);
            
                foreach (Cliente Remitente in Clientes.Values)
                {
                    if (Remitente.Usuario == fileMsg.Remitente)
                    {
                        IChatRespuesta sndrCall = Clientes[Remitente];
                        sndrCall.RecibirArchivo(fileMsg);
                    }
                }
            }         
    }
       
    public void EstaEscribiendo(Cliente c)
    {
        //bloqueo de exclusión mutua
        lock (ObjetoBloqueo)
        {
            foreach (IChatRespuesta Respuesta in Clientes.Values)
            {
                Respuesta.EstaEscribiendoRespuesta(c);
            }
        }
    }


    public void Desconectar(Cliente cd)
    {
        foreach (Cliente c in Clientes.Keys)
        {
            if (c.Usuario == cd.Usuario)
            {
                //bloqueo de exclusión mutua
                lock (ObjetoBloqueo)
                {
                    this.Clientes.Remove(c);
                    this.ListaClientes.Remove(c);
                    foreach (IChatRespuesta Respuesta in Clientes.Values)
                    {
                        Respuesta.RefrescarClientes(this.ListaClientes);
                        Respuesta.Dejar(cd);
                    }
                }
                return;
            }
        }
    }

    public event EventHandler ConexionCompletada;

    #endregion

    void IChat.EnviarArchivo(FileMessage fMgs)
    {
     
    }
  
}