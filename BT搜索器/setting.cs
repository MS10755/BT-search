using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT搜索器
{
    
    class setting
    {
        private static string SettingFilePath { get; } = Application.StartupPath + "/settings.ini";
        [DllImport("kernel32")]
        private static extern int WritePrivateProfileString(string secation,string key ,string val ,string filepath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string secation,string key,string def ,StringBuilder retval,int size,string filepath);

        public static bool WtrieSetting(string secation,string key,string val) {
            int res = WritePrivateProfileString(secation,key,val, SettingFilePath);
            if (res == 0)
            {
                return false;
            }
            else {
                return true;
            }
        }

        public static String ReadSetting(string secation,string key) {
            StringBuilder stringBuilder = new StringBuilder(1024);
            int res = GetPrivateProfileString(secation,key,"",stringBuilder,1024,SettingFilePath);
            if (res==0)
            {
                return "";
            }
            else{
                return stringBuilder.ToString();
            }
        
        }
    }
}
