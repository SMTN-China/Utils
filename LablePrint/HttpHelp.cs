using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LablePrint
{
    public class HttpHelp
    {

        public HttpHelp()
        {

        }

        AuthenticateResultModel _token;

        AuthenticateResultModel Token
        {
            get
            {
                if (_token != null && _token.ExpTime > DateTime.Now)
                {
                    return _token;
                }
                else
                {
                    return Login();
                }
            }
        }

        string BaseUrl
        {
            get { return ConfigurationManager.AppSettings["baseUrl"]; }
        }

        public AuthenticateResultModel Login()
        {
            
                var uid = ConfigurationManager.AppSettings["uid"];
                var pwd = ConfigurationManager.AppSettings["pwd"];

                var res = RequsetAbp<AuthenticateResultModel>(RequsetMethod.POST, "/api/TokenAuth/Authenticate", new AuthenticateModel()
                {
                    UserNameOrEmailAddress = uid,
                    Password = pwd,
                    RememberClient = true
                }, new List<HttpHeader>()
                {
                    new HttpHeader()
                    {
                        Key = "Abp.TenantId",
                        Value = ConfigurationManager.AppSettings["tenantId"]
                    }
                }, true);

                if (res.Success)
                {
                    res.Result.ExpTime = DateTime.Now.AddSeconds(res.Result.ExpireInSeconds - 5);

                    _token = res.Result;

                    return res.Result;
                }

                return null;
           
        }

        public List<Tenant> GetTenantByKeyName(string keyName)
        {
            return RequsetAbp<List<Tenant>>(RequsetMethod.GET, "/api/services/app/Account/GetTenantByKeyName?keyName=" + keyName, null, null, true).Result;
        }
        public T Post<T>(string url, object data) where T : class, new()
        {
            return Requset<T>(RequsetMethod.POST, url, data, null, false);
        }

        public ReresultDto<T> PostAbp<T>(string url, object data) where T : class, new()
        {
            return RequsetAbp<T>(RequsetMethod.POST, url, data, null, false);
        }


        public T Get<T>(string url, object data) where T : class, new()
        {
            return Requset<T>(RequsetMethod.GET, url, data, null, false);
        }

        public ReresultDto<T> GetAbp<T>(string url, object data) where T : class, new()
        {
            return RequsetAbp<T>(RequsetMethod.GET, url, data, null, false);
        }

        public ReresultDto<T> RequsetAbp<T>(RequsetMethod method, string url, object data, List<HttpHeader> headers, bool noToken)
        {
            return Requset<ReresultDto<T>>(method, url, data, headers, noToken);
        }

        public T Requset<T>(RequsetMethod method, string url, object data, List<HttpHeader> headers, bool noToken) where T : class, new()
        {

            Encoding encoding = Encoding.UTF8;

            // 如果直接精确到地址不用加服务器基础地址
            if (url.ToLower().Contains("https://") || url.Contains("http://"))
            {

            }
            else
            {
                url = BaseUrl + url;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = method.ToString();

            // 添加token
            if (!noToken)
            {
                request.Headers.Add("Authorization", "Bearer " + Token.AccessToken);
            }

            // 添加自定义头
            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            // 统一这一种格式数据
            request.ContentType = "application/json;charset=utf-8";

            // 有数据正文，发送数据正文
            if (data != null)
            {
                var jsonData = Common.SerializeJSON(data);
                byte[] requsetData;
                requsetData = Encoding.UTF8.GetBytes(jsonData);
                request.ContentLength = requsetData.Length;

                using (Stream reStream = request.GetRequestStream())
                {
                    reStream.Write(requsetData, 0, requsetData.Length);
                }
            }


            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {

                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    var resSrt = reader.ReadToEnd();

                    return Common.DeserializeJSON<T>(resSrt);
                }
            }
        }



        public void PostAsync<T>(string url, object data, Action<T> onResponse) where T : class, new()
        {
            RequsetAsync<T>(RequsetMethod.POST, url, data, null, false, onResponse);
        }

        public void PostAbpAsync<T>(string url, object data, Action<ReresultDto<T>> onResponse) where T : class, new()
        {
            RequsetAbpAsync<T>(RequsetMethod.POST, url, data, null, false, onResponse);
        }


        public void GetAsync<T>(string url, object data, Action<T> onResponse) where T : class, new()
        {
            RequsetAsync<T>(RequsetMethod.GET, url, data, null, false, onResponse);
        }

        public void GetAbpAsync<T>(string url, object data, Action<ReresultDto<T>> onResponse) where T : class, new()
        {
            RequsetAbpAsync<T>(RequsetMethod.GET, url, data, null, false, onResponse);
        }

        public void RequsetAbpAsync<T>(RequsetMethod method, string url, object data, List<HttpHeader> headers, bool noToken, Action<ReresultDto<T>> onResponse) where T : class, new()
        {
            RequsetAsync<ReresultDto<T>>(method, url, data, headers, noToken, onResponse);
        }

        public void RequsetAsync<T>(RequsetMethod method, string url, object data, List<HttpHeader> headers, bool noToken, Action<T> onResponse) where T : class, new()
        {


            Encoding encoding = Encoding.UTF8;

            if (url.ToLower().Contains("https://") || url.Contains("http://"))
            {

            }
            else
            {
                url = BaseUrl + url;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseUrl + url);

            // 如果直接精确到地址不用加服务器基础地址
            if (url.ToLower().Contains("https://") || url.Contains("http://"))
            {
                request = (HttpWebRequest)WebRequest.Create(url);
            }

            request.Method = method.ToString();

            // 添加token
            if (!noToken)
            {
                request.Headers.Add("Authorization", "Bearer " + Token.AccessToken);
            }

            // 添加自定义头
            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            // 统一这一种格式数据
            request.ContentType = "application/json;charset=utf-8";




            request.BeginGetRequestStream(new AsyncCallback((result) =>
            {
                HttpWebRequest req = (HttpWebRequest)result.AsyncState;

                // 有数据正文，发送数据正文
                if (data != null)
                {
                    var jsonData = Common.SerializeJSON(data);
                    Stream postStream = req.EndGetRequestStream(result);
                    byte[] requsetData;
                    requsetData = Encoding.UTF8.GetBytes(jsonData);
                    postStream.Write(requsetData, 0, requsetData.Length);
                    postStream.Close();
                }

                req.BeginGetResponse(new AsyncCallback(responseResult =>
                {
                    HttpWebRequest req1 = (HttpWebRequest)responseResult.AsyncState;

                    if (responseResult.IsCompleted)
                    {
                        var webResponse = req1.EndGetResponse(responseResult);
                        using (var stream = webResponse.GetResponseStream())
                        {
                            using (var read = new StreamReader(stream))
                            {
                                if (onResponse != null)
                                {
                                    onResponse.Invoke(Common.DeserializeJSON<T>(read.ReadToEnd()));
                                }
                            }
                        }
                    }
                }), req);
            }), request);

        }



    }


    public enum RequsetMethod
    {
        GET = 0, POST, DELETE, PUT
    }

    public class ReresultDto<T>
    {
        public T Result { get; set; }

        public string TargetUrl { get; set; }

        public bool Success { get; set; }

        public Error Error { get; set; }

        public bool UnAuthorizedRequest { get; set; }

        public bool __abp { get; set; }

    }

    public class Error
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public string Details { get; set; }

        public string ValidationErrors { get; set; }
    }


    public class Tenant
    {
        public string Name { get; set; }

        public string TenantId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class AuthenticateResultModel
    {
        public string AccessToken { get; set; }
        public string EncryptedAccessToken { get; set; }
        public int ExpireInSeconds { get; set; }
        public DateTime ExpTime { get; set; }
    }

    public class AuthenticateModel
    {
        public string UserNameOrEmailAddress { get; set; }
        public string Password { get; set; }
        public bool RememberClient { get; set; }
    }

    public class HttpHeader
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class PagedResultDto<T>
    {
        public int TotalCount { get; set; }
        public List<T> Items { get; set; }
    }

    public class PagedResultRequestMESDto
    {
        public int SkipCount { get; set; }
        public int MaxResultCount { get; set; }


        public List<RequestMESDto> RequestMESDtos { get; set; }

        public string SortName { get; set; }

        public bool Desc { get; set; }

    }

    public class RequestMESDto
    {
        public string PropertyName { get; set; }

        public Operation Operation { get; set; }

        public object QueryValue { get; set; }

        public LinkOperation LinkOperation { get; set; }

        public List<RequestMESDto> RequestMESDtos { get; set; }
    }

    public enum LinkOperation
    {
        And = 0,
        Or
    }

    /// <summary>
    /// 操作符
    /// </summary>
    public enum Operation
    {
        Equal = 0,
        NotEqual,
        Contains,
        StartsWith,
        EndsWith,
        NotContains,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        Empry,
        NotEmpty,
        Null,
        NotNull,
        RegEx
    }

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
            //return null;
            return JsonConvert.DeserializeObject<T>(json);
        }


        public static string SerializeJSON(object obj)
        {
            //return null;
            return JsonConvert.SerializeObject(obj);
        }
    }
}
