using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace NotificationService
{
    public class Common
    {
        [DllImport("Iphlpapi.dll")]
        static extern int SendARP(Int32 DestIP, Int32 SrcIP, ref Int64 MacAddr, ref Int32 PhyAddrLen);
        /// <summary>  
        /// 获取MAC地址  
        /// </summary>  
        /// <returns></returns>  
        public static string GetMac()
        {

            StringBuilder strReturn = new StringBuilder();
            try
            {
                System.Net.IPHostEntry Tempaddr = (System.Net.IPHostEntry)Dns.GetHostByName(Dns.GetHostName());
                System.Net.IPAddress[] TempAd = Tempaddr.AddressList;
                Int32 remote = (int)TempAd[0].Address;
                Int64 macinfo = new Int64();
                Int32 length = 6;
                SendARP(remote, 0, ref macinfo, ref length);

                string temp = System.Convert.ToString(macinfo, 16).PadLeft(12, '0').ToUpper();

                int x = 12;
                for (int i = 0; i < 6; i++)
                {
                    if (i == 5) { strReturn.Append(temp.Substring(x - 2, 2)); }
                    else { strReturn.Append(temp.Substring(x - 2, 2) + "-"); }
                    x -= 2;
                }

                return strReturn.ToString();
            }
            catch
            {
                return "";
            }

        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public static List<string> GetIpAddress()
        {
            try
            {
                string hostname = Dns.GetHostName();
                List<string> listIP = new List<string>();
                IPHostEntry IpEntry = Dns.GetHostEntry(hostname);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        listIP.Add(IpEntry.AddressList[i].ToString());
                    }
                }
                return listIP;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="md5str">要加密的字符</param>
        /// <returns>返回MD5</returns>
        public static string EncryptMD5(string md5str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(md5str));
            string bytestr = null;
            for (int i = 0; i < result.Length; i++)
            {
                bytestr += result[i];
            }
            return bytestr;
        }


        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception)
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray(), 0, mStream.ToArray().Length);
            }
            catch
            {
                return decryptString;
            }
        }


        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log">一行日志的内容,\t分割</param>
        public static void WriteLog(List<string> log)
        {
            try
            {
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName) + "/logs/";

                DirectoryInfo d = Directory.CreateDirectory(path);
                FileStream fs = new FileStream(path + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", System.IO.FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                string logStr = DateTime.Now.ToString();
                foreach (var item in log)
                {
                    logStr += "\t" + item;
                }

                sw.WriteLine(logStr);
                sw.Close();
                fs.Close();
            }
            catch
            {
            }
        }

        public static T DeserializeJSON<T>(string json) where T : class, new()
        {
            return JsonConvert.DeserializeObject<T>(json);
        }


        public static string SerializeJSON(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
