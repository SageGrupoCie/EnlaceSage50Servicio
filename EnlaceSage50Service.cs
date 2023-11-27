using oSage50Connect;
using sage._50;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EnlaceSage50Servicio
{
    public partial class EnlaceSage50Service : ServiceBase
    {
        private oSage50 obj;
        public EventLog Log { get; set; }
        public ServiceHost serviceHost = null;
        private string instancia;
        private string usuarioSQL;
        private string contrasena;
        private string guidEmp;
        private string comunes="";
        private string empresa = "";
        public EnlaceSage50Service()
        {
            InitializeComponent();
        }

        private void CargarConfiguraciones()
        {
            string directorioEjecutable = @"C:\GRUPOCIE\";
            // Ruta del archivo de texto
            string filePath = Path.Combine(directorioEjecutable, "ConfiguracionesServicio.config");

            // Verificar si el archivo existe
            if (File.Exists(filePath))
            {
                // Leer todas las líneas del archivo
                string[] lines = File.ReadAllLines(filePath);

                // Expresión regular para encontrar el patrón [CADENA]valor
                Regex regex = new Regex(@"\[(.*?)\](.*?)");

                // Iterar sobre cada línea
                foreach (string line in lines)
                {
                    // Coincidir con el patrón en la línea
                    Match match = regex.Match(line);

                    // Verificar si hay coincidencias
                    if (match.Success)
                    {
                        // Obtener la cadena y el valor
                        string cadena = match.Groups[1].Value;
                        string valor = line.Substring(match.Index + match.Length).Trim();
                        if (cadena == "INSTANCIA") { instancia = valor.Trim(); }
                        else if (cadena == "USUARIO") { usuarioSQL = valor.Trim(); }
                        else if (cadena == "CONTRASENA") { contrasena = valor.Trim(); }
                        else if (cadena == "EMPRESA") { empresa = valor.Trim(); }
                        else if (cadena == "COMUNES") { comunes = "COMU000" + valor.Trim(); }
                    }
                }
            }
            if (comunes != "")
            {
                string connectionString = "Data Source=" + instancia + ";Initial Catalog=GRUPOCIE;User ID=" + usuarioSQL + ";Password=" + contrasena;


                string empgestion = "";
                // Crear y abrir la conexión
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Definir la consulta SELECT
                    string query = "select RUTA from [" + comunes + "].dbo.ejercici WHERE [ANY] = YEAR(GETDATE())";
                    Log.WriteEntry(query, EventLogEntryType.Information);

                    // Crear y configurar el comando SQL
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Ejecutar la consulta y obtener el resultado
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Verificar si hay filas
                            if (reader.HasRows)
                            {
                                // Iterar a través de las filas y obtener el valor
                                if (reader.Read())
                                {
                                    empgestion = reader["RUTA"].ToString().Trim();
                                }
                            }
                            else
                            {
                                //
                            }
                        }
                    }
                }


                if (empgestion != "")
                {
                    // Crear y abrir la conexión
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Definir la consulta SELECT
                        string query = "select GUID_ID from [" + empgestion + "].dbo.empresa WHERE [CODIGO] = '" + empresa + "'";
                        Log.WriteEntry(query, EventLogEntryType.Information);

                        // Crear y configurar el comando SQL
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Ejecutar la consulta y obtener el resultado
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Verificar si hay filas
                                if (reader.HasRows)
                                {
                                    // Iterar a través de las filas y obtener el valor
                                    if (reader.Read())
                                    {
                                        guidEmp = reader["GUID_ID"].ToString().Trim();
                                    }
                                }
                                else
                                {
                                    //
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void OnStart(string[] args)
        {




            bool conct;
            Log = new EventLog();
            if (!EventLog.SourceExists("EnlaceSage50Service"))
                EventLog.CreateEventSource("EnlaceSage50Service", "");
            Log.Source = "EnlaceSage50Service";
            Log.WriteEntry("Conectando servicio Sage 50 Enlace", EventLogEntryType.Information);
            
            string ruta = @"C:\Sage50\Sage50Term";
            try
            {
                CargarConfiguraciones();


                //if (main_s50.Connect(ruta, user, pass))
                //if (sage._50.main_s50.Connect("192.168.120.8" + @"\" + "SQLSAGE50", "Sage50", "ew#211218", "COMU0003", "01", false))  
                //if (main_s50._main_Sage_50_External_Entry(ruta, "COMU0003", "01"))

                //conct = main_s50.Connect(ruta, user, pass);
                //Log.WriteEntry("Estado de la conexión 1 ->" + conct, EventLogEntryType.Information);
                //conct = sage._50.main_s50.Connect(ruta,"SUPERVISOR", "Albir7275","01",false);
                //conct = main_s50._main_Sage_50_External_Entry(ruta);
                Log.WriteEntry("Estado de la conexión(esp) ->" + instancia +","+ usuarioSQL + "," + contrasena + "," + comunes + "," + guidEmp, EventLogEntryType.Information);
                //conct = sage._50.main_s50.Connect(instancia, usuarioSQL, contrasena, comunes, guidEmp, false);
                //conct = main_s50._main_Sage_50_External_Entry(ruta,comunes,"01");
                //conct = main_s50.Connect(ruta, "SUPERVISOR", "Albir7275", "01",true);
                obj = new oSage50();
                obj.init(tsTerminal: ruta, "SUPERVISOR", "Albir7275","01",false);
                conct = obj.conectar();

                //Log.WriteEntry("inicio " + sage.ew.db.DB.Conexion, EventLogEntryType.Warning);
                //Log.WriteEntry("Estado de la conexión 2 ->" + conct, EventLogEntryType.Information);
                /*
                conct = main_s50._main_Sage_50_External_Entry(ruta, "COMU0003", "01");
                Log.WriteEntry("Estado de la conexión 3 ->" + conct, EventLogEntryType.Information);
                conct = main_s50._main_Sage_50_External_Entry(ruta);
                Log.WriteEntry("Estado de la conexión 4 ->" + conct, EventLogEntryType.Information);
                */
                if (conct)
                {
                    Log.WriteEntry("Servicio de Sage 50 conectado con éxito", EventLogEntryType.Information);
                }
                else 
                {
                    Log.WriteEntry("Servicio de Sage 50 no conectado", EventLogEntryType.Error);
                }
            }
            catch (Exception ex)
            {
                Log.WriteEntry("El servicio de Sage 50 no ha podido conectarse: " + ex.Message, EventLogEntryType.Error);
            }
            
            if (serviceHost != null)
                serviceHost.Close();

            serviceHost = new ServiceHost(typeof(EnlaceSage50));
            try
            {
                serviceHost.Open();
            }
            catch (Exception ex)
            {
                Log.WriteEntry("Error al realizar el contrato de servicio. " + ex.Message, EventLogEntryType.Error);
            }


        }


        protected override void OnStop()
        {
            obj.desconectar();
            Log.WriteEntry("Deteniendo servicio", EventLogEntryType.Information);
            //isRunning = false;
            // Cerrar WCF
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }

        
    }
}
