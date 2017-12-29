using System;
using System.Management;
using System.IO;

namespace TrialAppVerify
{
    class Program
    {
        static void Main(string[] args)
        {
            string strMacAddress = GetMacAddress();
            if (strMacAddress == strRightMacAddress)
            {
                Console.WriteLine("Mac address is right");
                DecodeFile();
            }
            else
            {
                Console.WriteLine("Mac address is not right");
                EncodeFile();
            }
        }

        public const string strRightMacAddress = "60:45:CB:29:77:97";//正确的MAC地址

        //获取本机MAC地址
        static public string GetMacAddress()
        {
            try
            {
                //获取网卡硬件地址
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
        }

        static public void EncodeFile()
        {
            File.Move("TrialApp.exe", "TrialApp.exenotverified");
        }

        static public void DecodeFile()
        {
            if (File.Exists("TrialApp.exenotverified"))
            {
                File.Move("TrialApp.exenotverified", "TrialApp.exe");
            }
        }
    }
}
