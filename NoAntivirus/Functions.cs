#pragma warning disable CA1416 // Validate platform compatibility

using System;
using System.IO;
using System.Management;
using System.Text.Json;
using CommandDotNet;
using NoAntivirus.Models;

namespace NoAntivirus
{
    public class Functions
    {
        private static readonly string BackupPath = Path.Combine(Environment.CurrentDirectory, "backup.json");

        [Command(Description = "On the NoAntivirus, we will backup your anti-virus programs.")]
        public bool On(bool backup = true)
        {
            var programs = new AntivirusPrograms();

            // ReSharper disable once InvertIf
            if (backup)
            {
                var programCollection = programs.GetAll();
                var json = JsonSerializer.Serialize(programCollection);

                File.WriteAllText(BackupPath, json);
            }

            return
                programs.RemoveAll(it =>
                    it.GetPropertyValue("displayName").ToString() != "Windows Defender"
                )
                &&
                programs.Add(new AntivirusProgram() { Guid = new Guid(), Name = "NoAntivirus" });
        }

        [Command(Description = "Off the NoAntivirus, if your have back up, we will restore it.")]
        public bool Off()
        {
            var programs = new AntivirusPrograms();

            if (!File.Exists(BackupPath))
            {
                programs.RemoveAll(it => 
                    it.GetPropertyValue("displayName").ToString() == "NoAntivirus"
                );
            }

            var json = File.ReadAllText(BackupPath);
            var collection = JsonSerializer.Deserialize<ManagementObjectCollection>(json);

            return programs.Add(collection);
        }
    }
}
