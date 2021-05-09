#pragma warning disable CA1416 // Validate platform compatibility

using System;
using System.Management;
using NoAntivirus.Models;

namespace NoAntivirus
{
    public class AntivirusPrograms
    {
        public AntivirusPrograms()
        {
            _searcher = new ManagementObjectSearcher(_wmiPath, "SELECT * FROM AntivirusProduct");
        }

        private readonly string _wmiPath = @$"\\{Environment.MachineName}\root\SecurityCenter:AntiVirusProduct";

        private readonly ManagementObjectSearcher _searcher;

        public ManagementObjectCollection GetAll()
        {
            return _searcher.Get();
        }

        public bool Add(AntivirusProgram program)
        {
            var manClass = new ManagementClass(_wmiPath);

            var obj = manClass.CreateInstance();

            if (obj == null) return false;

            obj.SetPropertyValue("displayName", program.Name);
            obj.SetPropertyValue("instanceGuid", program.Guid.ToString("B"));
            obj.SetPropertyValue("productUptoDate", true);
            obj.SetPropertyValue("onAccessScanningEnabled", true);

            obj.Put();

            return true;
        }

        public bool Add(ManagementObjectCollection collection)
        {
            throw new NotImplementedException();

            var result = true;

            foreach (var item in collection)
            {
                if (item is ManagementObject obj)
                {
                    obj.Put();
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public bool RemoveAll(Func<ManagementBaseObject, bool> providerAction)
        {
            var result = true;

            foreach (var obj in GetAll())
            {
                if (!providerAction.Invoke(obj)) continue;
                if (obj is ManagementObject objCanDel)
                {
                    objCanDel.Delete();
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
