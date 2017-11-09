using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace Coldairarrow.Util.Wcf
{
    /// <summary>
    /// Wcf客户端工厂
    /// </summary>
    public class WcfClientFactory
    {
        #region 构造函数

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static WcfClientFactory()
        {
            _defaultBinding = GetDefaultWSHttpBinding();
        }

        #endregion

        #region 外部接口

        /// <summary>
        /// 创建客户端(使用默认协议)
        /// </summary>
        /// <typeparam name="T">服务接口</typeparam>
        /// <param name="getUrl">服务地址</param>
        /// <returns></returns>
        public static T CreateClientByUrl<T>(string getUrl)
        {
            return CreateClientByUrl<T>(getUrl, "wshttpbinding");
        }

        /// <summary>
        /// 创建客户端
        /// </summary>
        /// <typeparam name="T">服务接口</typeparam>
        /// <param name="getUrl">服务地址</param>
        /// <param name="bindingType">协议类型</param>
        /// <returns></returns>
        public static T CreateClientByUrl<T>(string getUrl, string bindingType)
        {
            if (string.IsNullOrEmpty(getUrl))
                throw new Exception("服务地址不能为Null或者空");
            EndpointAddress address = new EndpointAddress(getUrl);
            Binding binding = CreateBinding(bindingType);
            ChannelFactory<T> factory = new ChannelFactory<T>(binding, address);
            return factory.CreateChannel();
        }

        #endregion

        #region 私有成员

        private static Binding _defaultBinding { get; }
        private static Binding CreateBinding(string binding)
        {
            Binding bindingInstance = null;
            if (binding.ToLower() == "basichttpbinding")
            {
                BasicHttpBinding ws = new BasicHttpBinding();
                ws.MaxBufferSize = 2147483647;
                ws.MaxBufferPoolSize = 2147483647;
                ws.MaxReceivedMessageSize = 2147483647;
                ws.ReaderQuotas.MaxStringContentLength = 2147483647;
                ws.CloseTimeout = new TimeSpan(0, 30, 0);
                ws.OpenTimeout = new TimeSpan(0, 30, 0);
                ws.ReceiveTimeout = new TimeSpan(0, 30, 0);
                ws.SendTimeout = new TimeSpan(0, 30, 0);

                bindingInstance = ws;
            }
            else if (binding.ToLower() == "nettcpbinding")
            {
                NetTcpBinding ws = new NetTcpBinding();
                ws.MaxReceivedMessageSize = 65535000;
                ws.Security.Mode = SecurityMode.None;
                bindingInstance = ws;
            }
            else if (binding.ToLower() == "wshttpbinding")
            {
                bindingInstance = _defaultBinding;
            }

            return bindingInstance;
        }
        private static WSHttpBinding GetDefaultWSHttpBinding()
        {
            //设置默认绑定
            WSHttpBinding defaultWSHttpBinding = new WSHttpBinding
            {
                CloseTimeout = TimeSpan.Parse("00:01:00"),
                OpenTimeout = TimeSpan.Parse("00:01:00"),
                ReceiveTimeout = TimeSpan.Parse("00:10:00"),
                SendTimeout = TimeSpan.Parse("00:01:00"),
                BypassProxyOnLocal = false,
                TransactionFlow = false,
                HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                MaxBufferPoolSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                MessageEncoding = WSMessageEncoding.Text,
                TextEncoding = Encoding.UTF8,
                UseDefaultWebProxy = true
            };

            //数据长度
            defaultWSHttpBinding.ReaderQuotas.MaxDepth = int.MaxValue;
            defaultWSHttpBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            defaultWSHttpBinding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            defaultWSHttpBinding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            defaultWSHttpBinding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;

            defaultWSHttpBinding.ReliableSession.Ordered = true;
            defaultWSHttpBinding.ReliableSession.InactivityTimeout = TimeSpan.Parse("00:10:00");
            defaultWSHttpBinding.ReliableSession.Enabled = false;

            //安全协议,默认不起用
            defaultWSHttpBinding.Security.Mode = SecurityMode.None;
            defaultWSHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            defaultWSHttpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            defaultWSHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.None;

            return defaultWSHttpBinding;
        }

        #endregion
    }
}
