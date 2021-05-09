#pragma warning disable CA1416 // Validate platform compatibility

using System;
using System.IO;
using System.Management;
using System.Text.Json;
using NoAntivirus.Models;

namespace NoAntivirus
{
    public class Functions
    {
        private static readonly string BackupPath = Path.Combine(Environment.CurrentDirectory, "backup.json");

        public static bool OpenNoAntivirus(bool backup)
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

        public static bool RestoreAntivirus()
        {
            if (!File.Exists(BackupPath)) return false;

            var json = File.ReadAllText(BackupPath);
            var collection = JsonSerializer.Deserialize<ManagementObjectCollection>(json);

        }
    }
}
