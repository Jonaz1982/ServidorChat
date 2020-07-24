using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//using LibreriaChat;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Runtime.Serialization;

namespace ServidorChat
{

    public partial class Principal : Window
    {
        ServiceHost Servidor;

        public Principal()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, RoutedEventArgs e)
        {
            btnIniciar.IsEnabled = false;

            //Definir la dirección base para todas las conexiones
            Uri DireccionTCP = new Uri("net.tcp://" +
                txtIP.Text + ":" +
                txtPuerto.Text + "/ServicioChat/");

            Uri DireccionHTTP = new Uri("http://" +
                txtIP.Text + ":" +
                (int.Parse(txtPuerto.Text) + 1).ToString() +
                "/ServicioChat/");

            Uri[] DireccionesBase = { DireccionTCP, DireccionHTTP };

            //Instancia del servicio
            Servidor = new ServiceHost(
                   typeof(ServicioChat), DireccionesBase);
            

            NetTcpBinding tcpEnlace =
               new NetTcpBinding(SecurityMode.None, true);
            
            //Habilitar la transferencia de archivos de 64 MB
            tcpEnlace.MaxBufferPoolSize = (int)67108864;
            tcpEnlace.MaxBufferSize = 67108864;
            tcpEnlace.MaxReceivedMessageSize = (int)67108864;
            tcpEnlace.TransferMode = TransferMode.Buffered;
            tcpEnlace.ReaderQuotas.MaxArrayLength = 67108864;
            tcpEnlace.ReaderQuotas.MaxBytesPerRead = 67108864;
            tcpEnlace.ReaderQuotas.MaxStringContentLength = 67108864;

            tcpEnlace.MaxConnections = 100;


            //Configurar el número máximo de conexiones al servicio
            //lo cual incide en el comportamiento del mismo
            ServiceThrottlingBehavior controlServicio;
            controlServicio =
             Servidor.Description.Behaviors.Find<ServiceThrottlingBehavior>();
            if (controlServicio == null)
            {
                controlServicio = new ServiceThrottlingBehavior();
                controlServicio.MaxConcurrentCalls = 100;
                controlServicio.MaxConcurrentSessions = 100;
                Servidor.Description.Behaviors.Add(controlServicio);
            }

            //Configurar confiabilidad y durabilidad de la sesión
            tcpEnlace.ReceiveTimeout = new TimeSpan(20, 0, 0);
            tcpEnlace.ReliableSession.Enabled = true;
            tcpEnlace.ReliableSession.InactivityTimeout =
                                       new TimeSpan(20, 0, 10);

            Servidor.AddServiceEndpoint(typeof(IChat),
                                    tcpEnlace, "tcp");


            //Habilitar la publicación de metadatos del servicio
            ServiceMetadataBehavior mBehave =
                           new ServiceMetadataBehavior();
            Servidor.Description.Behaviors.Add(mBehave);

            Servidor.AddServiceEndpoint(typeof(IMetadataExchange),
                MetadataExchangeBindings.CreateMexTcpBinding(),
                "net.tcp://" + txtIP.Text.ToString() + ":" +
                (int.Parse(txtPuerto.Text.ToString()) - 1).ToString() +
                "/ServicioChat/mex");
           // net.tcp://localhost:7996//ServicioChat/mex
            try
            {
                //Iniciar el servicio
                Servidor.Open();
            }
            catch (Exception ex)
            {
                //No se pudo iniciar el servicio
                lblEstado.Text = ex.Message.ToString();
                btnIniciar.IsEnabled = true;
            }
            finally
            {
                //Mostrar estado del servicio si se pudo abrir
                if (Servidor.State == CommunicationState.Opened)
                {
                    lblEstado.Text = "Abierto";
                    btnDetener.IsEnabled = true;
                }
            }
        }

        //Detener el servicio si esta abierto
        private void btnDetener_Click(object sender, RoutedEventArgs e)
        {
            if (Servidor != null)
            {
                try
                {
                    Servidor.Close();
                }
                catch (Exception ex)
                {
                    lblEstado.Text= ex.Message.ToString();
                }
                finally
                {
                    if (Servidor.State == CommunicationState.Closed)
                    {
                        lblEstado.Text = "Cerrado";
                        btnIniciar.IsEnabled = true;
                        btnDetener.IsEnabled = false;
                    }
                }
            }
        }


    }
}
