// See https://aka.ms/new-console-template for more information

using AForge.Video.DirectShow;
using Google.Protobuf;
using Grpc.Net.Client;
using Stream;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Google.Protobuf.WellKnownTypes;

var count = 0;
//var call = client.GetStreamBytes();

//await Task.Delay(TimeSpan.FromSeconds(2));

var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
Console.WriteLine(videoDevices.Count);
var videoSource = new VideoCaptureDevice(videoDevices[1].MonikerString);
videoSource.NewFrame += async (sender, eventArgs) =>
{
    var bmp = new Bitmap(eventArgs.Frame, 800, 600);
    try
    {
        using var ms = new MemoryStream();
        eventArgs.Frame.Save(ms, ImageFormat.Jpeg);
        var bytes = ms.ToArray();
        //udpClient.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.Parse("192.168.0.75"), 4100));
        using var channel = GrpcChannel.ForAddress("http://localhost:5002");
        var client = new Streamer.StreamerClient(channel);
        client.SayHello(new CounterRequest{ Data = ByteString.CopyFrom(bytes)});
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
};
videoSource.Start();

//await call.RequestStream.CompleteAsync();

Console.WriteLine("Hello, World!");