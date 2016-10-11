using System;

namespace Application
{
    public static class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run((MainClass.MainWindow = new MainWindow()));
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = e.ExceptionObject as Exception;

            if (exception != null)
            {
                Microsoft.NetEnterpriseServers.ExceptionMessageBox emb = new Microsoft.NetEnterpriseServers.ExceptionMessageBox(
                       exception,
                       Microsoft.NetEnterpriseServers.ExceptionMessageBoxButtons.OK,
                       Microsoft.NetEnterpriseServers.ExceptionMessageBoxSymbol.Error);

                emb.Caption = "Internal application error. Sorry for the inconvenience.";

                emb.Show(MainClass.MainWindow);
            }
            else
            {
                Microsoft.NetEnterpriseServers.ExceptionMessageBox emb = new Microsoft.NetEnterpriseServers.ExceptionMessageBox(
                       "Unknown application error. Sorry for the inconvenience.",
                       "Unknown object was thrown.",
                       Microsoft.NetEnterpriseServers.ExceptionMessageBoxButtons.OK);

                emb.Show(MainClass.MainWindow);
            }
        }

        private static MainWindow MainWindow { get; set; }
    }
}
