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
using System.IO;
using System.Management;
using Microsoft.Win32;

namespace HardwareSoftwareMonitor
{
    class Szoftver
    { 
        public string Nev { set; get; }
        public string Verzio { set; get; }
        public string Keszito { set; get; }
        public string LetoltesHelye { set; get; }
        public string LetoltesDatuma { set; get; }
        public string Update { set; get; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Szoftver> szoftver = new List<Szoftver>();
        public MainWindow()
        {
            InitializeComponent();
            CPU();
            RAM();
            Alaplap();
            GPU();
            Meghajto();
            Szoftverinfok();
        }
        public void CPU()
        {
            try
            {
                ManagementObjectSearcher cpu = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                foreach (var a in cpu.Get())
                {
                    CNev.Content = a["Name"];
                    CMag.Content = $"{a["NumberOfCores"]} mag";
                    CSzal.Content = $"{a["ThreadCount"]} szál";
                    COrajel.Content = $"{a["MaxClockSpeed"]} GHz órajel";
                }
            }
            catch(Exception)
            {
                return;
            }
        }
        public void RAM()
        {
            try
            {
                ManagementObjectSearcher ram = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory");
                foreach (var b in ram.Get())
                {
                    RNev.Content = b["Name"];
                    RKapacitas.Content = b["Capacity"];
                    RTipus.Content = b["MemoryType"];
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        public void Alaplap()
        {
            try
            {
                ManagementObjectSearcher alaplap = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Baseboard");
                foreach (var c in alaplap.Get())
                {
                    AGyarto.Content = c["Manufacturer"];
                    AModel.Content = c["Model"];
                    ANev.Content = c["Product"];
                    ASorozatszam.Content = c["SerialNumber"];
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        public void Meghajto()
        {
            try
            {
                ManagementObjectSearcher meghajto = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");
                foreach (var d in meghajto.Get())
                {
                    MNev.Content = d["Name"];
                    MGyarto.Content = d["Manufacturer"];
                    MParticio.Content = $"{d["Partitions"]} db partíció";
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        public void GPU()
        {
            try
            {
                ManagementObjectSearcher gpu = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Baseboard");
                foreach (var e in gpu.Get())
                {
                    GNev.Content = e["Name"];
                    GRam.Content = e["AdapterRAM"];
                    GDriver.Content = $"{e["DriverVersion"]}-s verziójú driver";
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        public void Szoftverinfok()
        {
            string f = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey g = Registry.LocalMachine.OpenSubKey(f))
            {
                foreach (string h in g.GetSubKeyNames())
                {
                    using (RegistryKey i = g.OpenSubKey(h))
                    {
                        if (i.GetValue("DisplayName") != null)
                        {
                            szoftver.Add(new Szoftver
                            {
                                Nev = (string)i.GetValue("DisplayName"),
                                Verzio = (string)i.GetValue("DisplayVersion"),
                                LetoltesHelye = (string)i.GetValue("InstallLocation"),
                                LetoltesDatuma = (string)i.GetValue("InstallDate"),
                                Keszito = (string)i.GetValue("Publisher"),
                                //Update = (string)i.GetValue("Update"),
                            });
                        }
                    }
                }
            }
            szoftverinformaciok.ItemsSource = szoftver;
        }
    }
}
