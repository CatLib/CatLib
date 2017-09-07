/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System.IO;
using System.Net;
using System.Text;

namespace CatLib.Tests
{
    /// <summary>
    /// http帮助器
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// 以get的方式请求服务器
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="ret">请求结果</param>
        /// <returns>http状态码</returns>
        public static HttpStatusCode Get(string url ,out string ret)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "get";
            request.ContentType = "text/html;charset=utf-8";

            var response = (HttpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            var responseReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            ret = responseReader.ReadToEnd();
            responseReader.Close();
            responseReader.Close();

            return response.StatusCode;
        }
    }
}
