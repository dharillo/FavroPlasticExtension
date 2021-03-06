using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;

namespace FavroPlasticExtensionInstaller
{
    class Program
    {
        const string favroExtensionDLL = "FavroPlasticExtension.dll";
        const string customExtensionsFilename = @"customextensions.conf";
        static string customExtensionsFilePath = @"C:\Program Files\PlasticSCM5\client\" + customExtensionsFilename;

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

        static void configureCustomExtensions()
        {
            var workingDir = Directory.GetCurrentDirectory();
            var favroExtensionConfig = $"Favro={workingDir}\\{favroExtensionDLL}";
            string currentExtensionConfig;
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
        static void configurePlasticClient()
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


        static void removeExtensionLineFromConfigFile(string configFile)
        {
            string currentExtensionConfig;
            using (FileStream fs = File.Open(configFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                currentExtensionConfig = findExtensionLine(fs, favroExtensionDLL);
            }
            if (currentExtensionConfig.Length > 0)
            {
                File.WriteAllLines(configFile, File.ReadLines(configFile).Where(l => !l.Contains(currentExtensionConfig)).ToList());
            }
        }

        static void removeCustomExtensionsConfiguration()
        {
            removeExtensionLineFromConfigFile(customExtensionsFilePath);
        }

        static void removePlasticClientConfiguration()
        {
            var roamingAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var plasticClientConfPath = Path.Combine(Directory.GetParent(roamingAppData).FullName, @"Local\plastic4\client.conf");
            removeExtensionLineFromConfigFile(plasticClientConfPath);
        }

        static void install()
        {
            configureCustomExtensions();
            configurePlasticClient();

            // modificar el uninstall string que se almacena en el registro para poder desinstalar el plugin correctamente de la carpeta de Plastic
            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            Microsoft.Win32.RegistryKey uninstallKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(registryKey);
            if (uninstallKey != null)
            {
                var appName = Assembly.GetCallingAssembly().GetName().Name;
                foreach (String a in uninstallKey.GetSubKeyNames())
                {
                    Microsoft.Win32.RegistryKey subkey = uninstallKey.OpenSubKey(a, true);
                    // Found the Uninstall key for this app.
                    if (subkey.GetValue("DisplayName").Equals(appName))
                    {
                        string uninstallString = subkey.GetValue("UninstallString").ToString();

                        // Wrap uninstall string with my own command
                        var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                        string newUninstallString = $"cmd /c \"{exeName} --uninstall & {uninstallString}\"";
                        subkey.SetValue("UninstallString", newUninstallString);
                        subkey.Close();
                    }
                }
            }
        }

        static void uninstall()
        {
            removeCustomExtensionsConfiguration();
            removePlasticClientConfiguration();
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
            else
            {
                string registryKey = @"SOFTWARE\Classes\plastic\DefaultIcon";
                Microsoft.Win32.RegistryKey PlasticKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(registryKey);
                if (PlasticKey != null)
                {
                    customExtensionsFilePath = Path.Combine(Directory.GetParent(PlasticKey.GetValue("").ToString()).FullName, customExtensionsFilename);
                }

                if (args.Count() >= 1 && args[0] == "--uninstall")
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
}
