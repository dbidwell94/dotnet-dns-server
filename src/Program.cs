global using System.Globalization;
// using System.Net.Sockets;
// using DotnetDNS.Answer;

// namespace DotnetDNS;

// public class Program
// {
//     public static async Task Main(string[] args)
//     {
//         UdpClient client = new UdpClient(2021);
//         while (true)
//         {
//             var result = await client.ReceiveAsync();

//             var request = new DNSRequest(result.Buffer);

//             RCode resultCode = RCode.NoError;

//             List<DNSAnswer> answers = new List<DNSAnswer>();
//             foreach (var query in request.Queries)
//             {
//                 if ((query.QType != QType.A && query.QType != QType.AAAA) || query.QClass != QClass.Internet)
//                 {
//                     resultCode = RCode.NotImplemented;
//                 }
//                 DNSAnswerData? answerData = null;

//                 switch (query.QType)
//                 {
//                     case QType.A:
//                         answerData = new AAnswerData(System.Net.IPAddress.Parse("192.168.1.1"));
//                         break;

//                     case QType.AAAA:
//                         answerData = new AAAAAnswerData(System.Net.IPAddress.Parse("2001:0db8:ac10:fe01::"));
//                         break;
//                 }
//                 if (answerData == null)
//                 {
//                     continue;
//                 }
//                 answers.Add(new DNSAnswer(String.Join('.', query.Labels), answerData));
//             }

//             var response = new DNSResponse(request, answers.ToArray(), resultCode);

//             await client.SendAsync(response.Data, response.Data.Length, result.RemoteEndPoint);
//         }
//     }
// }