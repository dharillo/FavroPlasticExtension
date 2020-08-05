using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FavroPlasticExtensionInstaller
{
    class Program
    {
        static string findExtensionLine(FileStream fs, string extension)
        {
            using (StreamReader sr = new StreamReader(fs))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(extension))
                    {
                        return line.Trim();
                    }
                }
            }
            return "";
        }

        static void configureCustomExtensions(string favroExtensionDLL)
        {
            var workingDir = Directory.GetCurrentDirectory();
            var favroExtensionConfig = $"Favro={workingDir}\\{favroExtensionDLL}";
            string currentExtensionConfig;
            var customExtensionsFilePath = @"C:\Program Files\PlasticSCM5\client\customextensions.conf";
            using (FileStream fs = File.Open(customExtensionsFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                currentExtensionConfig = findExtensionLine(fs, favroExtensionDLL);
            }
            if (currentExtensionConfig.Length == 0)
            {
                File.AppendAllText(customExtensionsFilePath, favroExtensionConfig);
            }
            else if (currentExtensionConfig != favroExtensionConfig)
            {
                File.WriteAllText(customExtensionsFilePath, File.ReadAllText(customExtensionsFilePath).Replace(currentExtensionConfig, favroExtensionConfig));
            }
        }

        static void configurePlasticClient(string favroExtensionDLL)
        {
            var workingDir = Directory.GetCurrentDirectory();
            var favroExtensionConfig = $"<Extension AssemblyFile=\"{workingDir}\\{favroExtensionDLL}\" />";
            var roamingAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var plasticClientConfPath = Path.Combine(Directory.GetParent(roamingAppData).FullName, @"Local\plastic4\client.conf");
            var currentExtensionConfig = "";
            using (FileStream fs = File.Open(plasticClientConfPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                currentExtensionConfig = findExtensionLine(fs, favroExtensionDLL);
            }
            if (currentExtensionConfig.Length > 0 && currentExtensionConfig != favroExtensionConfig)
            {
                File.WriteAllText(plasticClientConfPath, File.ReadAllText(plasticClientConfPath).Replace(currentExtensionConfig, favroExtensionConfig));
            }
        }

        static void install()
        {
            var workingDir = Directory.GetCurrentDirectory();
            var favroExtensionDLL = "FavroPlasticExtension.dll";

            configureCustomExtensions(favroExtensionDLL);
            configurePlasticClient(favroExtensionDLL);
            
            // TODO: 3. modificar el uninstall string que se almacena en el registro para poder desinstalar el plugin correctamente de la carpeta de Plastic
            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            Microsoft.Win32.RegistryKey uninstallKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(registryKey);
            if (uninstallKey != null)
            {
                foreach (String a in uninstallKey.GetSubKeyNames())
                {
                    Microsoft.Win32.RegistryKey subkey = uninstallKey.OpenSubKey(a, true);
                    // Found the Uninstall key for this app.
                    if (subkey.GetValue("DisplayName").Equals("AppDisplayName"))
                    {
                        string uninstallString = subkey.GetValue("UninstallString").ToString();

                        var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;


                        // Wrap uninstall string with my own command
                        // In this case a reg delete command to remove a reg key.
                        // TODO: llamar a la aplicacion FavroPlasticExtensionInstaller incluyendo el working directory con el argumento --uninstall
                        /*string newUninstallString = "cmd /c \"" + uninstallString +
                            " & reg delete HKEY_CURRENT_USER\\SOFTWARE\\CLASSES\\mykeyv" +
                            MYAPP_VERSION + " /f\"";
                        subkey.SetValue("UninstallString", newUninstallString);
                        subkey.Close();*/
                    }
                }
            }
        }

        static void uninstall()
        {
            // TODO: borra la configuracion del fichero de customextensions
            // TODO: borra la configuracion de cliente de plastic (C:\Users\username\AppData\Local\plastic4\client.conf)
        }

        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        static void Main(string[] args)
        {
            if (IsAdministrator() == false)
            {
                // Restart program and run as admin
                var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                startInfo.Verb = "runas";
                startInfo.Arguments = String.Join(" ", args);
                System.Diagnostics.Process.Start(startInfo);
            }
            else if (args.Count() >= 2 && args[1] == "--uninstall")
            {
                uninstall();
            }
            else
            {
                install();
            }
        }
    }
}
