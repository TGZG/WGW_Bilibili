using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public class TcpClient {
        public object IDUpdateLock = new object();
        public Action OnConnect;
        public Action<ITcpClientBase> OnDisconnect;
        public Action<string> OnSend;
        public Action<string, TouchSocket.Sockets.TcpClient> OnReceive;
        public Dictionary<string, Action<Dictionary<string, string>>> OnRead = new();
        public string IP = "127.0.0.1:7789";
        public TouchSocket.Sockets.TcpClient Client = new();
        public int ID = 0;
        public bool IsConnected => Client.Online;
        public ConcurrentDictionary<string, Action<Dictionary<string, string>>> Success = new();
        public TcpClient() {

        }
        public TcpClient(string X) {
            IP = X;
        }
        public TcpClient(string X, string Y) {
            IP = X;
            OnConnect += () => {
                Send(new() { { "标题", "_版本检测" }, { "版本", Y } }, t => {
                    if (t["版本正确"] == "错误") {
                        Client.Close();
                    }
                });
            };
        }
        public void Stop() {
            Client.Close();
        }
        public bool Start() {
            Client.Connected = (client, e) => OnConnect?.Invoke();
            Client.Disconnected = (client, e) => OnDisconnect?.Invoke(client);
            Client.Received = (client, byteBlock, requestInfo) => {
                OnReceive?.Invoke(byteBlock.ToString(), client);
                var A = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len).JsonDeserialize<Dictionary<string, string>>();
                OnAppUpdate(() => {
                    if (A.ContainsKey("_ID")) {
                        if (!Success.ContainsKey(A["_ID"])) Print(A.JsonSerialize(false));
                        Success[A["_ID"]](A);
                        Success.Remove(t => t.Key == A["_ID"]);
                    } else if (OnRead.ContainsKey(A["标题"])) {
                        OnRead[A["标题"]](A);
                    }
                });
            };
            Client.Setup(new TouchSocketConfig()
                .SetRemoteIPHost(new IPHost(IP))
                .UsePlugin()
                //.ConfigurePlugins(a => a.UseReconnection(5, true, 1000))
                .SetBufferLength(1024 * 64)
                .SetDataHandlingAdapter(() => new FixedHeaderPackageAdapter() { FixedHeaderType = FixedHeaderType.Int }));
            try {
                Client.Connect();
            } catch (Exception e) {
                Print($"TCP连接失败：{e.Message}");
                return false;
            }
            return true;
        }
        public async Task<string> GetPing() {
            var 服务器发送时间 = await SendAsync(new() { { "标题", "_GetPing" } });
            var 当前时间 = DateTime.Now;
            return (当前时间 - 服务器发送时间["_服务器发送时间"].JsonDeserialize<DateTime>(false)).TotalMilliseconds.ToString() + "ms";
        }
        public async Task<string> Get丢包() {
            return "123";
        }
        public async Task<string> Get传输速度() {
            return "123";
        }
        public void Send(Dictionary<string, string> X, Action<Dictionary<string, string>> Y = null) {
            try {
                lock (IDUpdateLock) {
                    X["_ID"] = ID++.ToString();
                }
                if (Y != null) Success[X["_ID"]] = Y;
                var A = X.JsonSerialize(false);
                Client.Send(A);
                OnSend?.Invoke(A);
            } catch (Exception e) {
                PrintWarning($"TCP发送失败:{e.Message}");
                Success.Remove(t => t.Key == X["_ID"]);
            }
        }
        public async Task<Dictionary<string, string>> SendAsync(Dictionary<string, string> X) {
            var tcs = new TaskCompletionSource<Dictionary<string, string>>();//创建一个等待事件

            Send(X, result => {
                tcs.SetResult(result);
                //服务器回复后，将等待事件设置为完成状态
            });
            //返回服务器消息，等待结束。
            return await tcs.Task;
        }
    }
    public class TcpServer {
        public int Port;
        public Dictionary<string, Func<Dictionary<string, string>, SocketClient, Dictionary<string, string>>> OnRead = new();
        public Action<string, SocketClient> OnReceive;
        public Action<SocketClient> OnConnect;
        public Action<SocketClient> OnDisconnect;
        public string Version;
        public TcpService Server = new TcpService();
        public TcpServer(int port) {
            Port = port;
        }
        public TcpServer(int port, string Y) {
            Port = port;
            Version = Y;
        }
        public void Start() {
            Server.Connected = (client, e) => {
                OnConnect?.Invoke(client);
            };
            Server.Disconnected = (client, e) => {
                OnDisconnect?.Invoke(client);
            };
            Server.Received = (client, byteBlock, requestInfo) => {
                OnReceive?.Invoke(byteBlock.ToString(), client);
                var A = byteBlock.ToString().JsonDeserialize<Dictionary<string, string>>();
                var B = OnRead[A["标题"]](A, client);
                if (B != null) {
                    B["_ID"] = A["_ID"];
                    client.Send(B.JsonSerialize(false));//将收到的信息直接返回给发送方
                }
            };
            OnRead["_版本检测"] = (t, c) => {
                if (t["版本"] == Version) {
                    return new Dictionary<string, string> { { "版本正确", "正确" } };
                } else {
                    return new Dictionary<string, string> { { "版本正确", "错误" } };
                }
            };
            OnRead["测试信息"] = (t, c) => {
                return new() { { "返回", $"您发来的消息是 {t["内容"]}" } };
            };
            Server.Setup(new TouchSocketConfig()
                .SetListenIPHosts(new IPHost[] { new IPHost(Port) })
                .SetDataHandlingAdapter(() => new FixedHeaderPackageAdapter() { FixedHeaderType = FixedHeaderType.Int }))
            .Start();
        }
        public void AllSend(Dictionary<string, string> X) {
            foreach (var i in Server.GetClients()) {
                i.Send(X.JsonSerialize(false));
            }
        }
    }
}
