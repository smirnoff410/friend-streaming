// See https://aka.ms/new-console-template for more information

using System.Diagnostics;


Console.WriteLine("Input WebCam name.");
var webCamName = Console.ReadLine();
Console.WriteLine("Input destination url.");
var destinationUrl = Console.ReadLine();

var commandArgs = new List<string>
{
    "-f dshow -i",
    $"video=\"{webCamName}\"",
    "-f mpegts -codec:v mpeg1video -b:v 1000k -bf 0",
    $"udp://{destinationUrl}"
};

var argumentsString = string.Join(" ", commandArgs);
var ffmpeg     = "D:\\Programs\\ffmpeg\\ffmpeg-2022-05-23-git-6076dbcb55-essentials_build\\bin\\ffmpeg.exe";
var procStartInfo = new ProcessStartInfo(ffmpeg)
{
    RedirectStandardOutput = true,
    UseShellExecute = false,
    CreateNoWindow = true,
    WorkingDirectory = Environment.CurrentDirectory,
    Arguments = argumentsString
};

var process = new Process { StartInfo = procStartInfo };
process.Start();
var processOutput = await process.StandardOutput.ReadToEndAsync();
Console.WriteLine(processOutput);
process.WaitForExit();

Console.WriteLine("Hello, World!");