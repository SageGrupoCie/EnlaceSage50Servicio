
namespace EnlaceSage50Servicio
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.ServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            this.ServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            // 
            // ServiceInstaller
            // 
            this.ServiceInstaller.Description = "Servicio de enlace con Sage 50";
            this.ServiceInstaller.DisplayName = "Servicio Sage50 Enlace";
            this.ServiceInstaller.ServiceName = "Servicio Sage50 Enlace";
            this.ServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ServiceProcessInstaller
            // 
            this.ServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ServiceProcessInstaller.Password = null;
            this.ServiceProcessInstaller.Username = null;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ServiceInstaller,
            this.ServiceProcessInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceInstaller ServiceInstaller;
        private System.ServiceProcess.ServiceProcessInstaller ServiceProcessInstaller;
    }
}