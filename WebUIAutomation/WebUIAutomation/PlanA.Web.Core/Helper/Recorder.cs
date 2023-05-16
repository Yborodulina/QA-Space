using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using SharpAvi.Output;

namespace PlanA.Web.Core.Helper;

public class Recorder : IDisposable
{
    #region Fields

    private readonly AviWriter _writer;
    private readonly RecorderParams _params;
    private readonly IAviVideoStream _videoStream;
    private readonly Thread _screenThread;
    private readonly ManualResetEvent _stopThread = new ManualResetEvent(false);

    #endregion

    public Recorder(RecorderParams @params)
    {
        _params = @params;

        _writer = @params.CreateAviWriter();

        _videoStream = @params.CreateVideoStream(_writer);

        _videoStream.Name = "Capture";

        _screenThread = new Thread(RecordScreen)
        {
            Name = nameof(Recorder) + ".RecordScreen",
            IsBackground = true
        };

        _screenThread.Start();
    }

    public void Dispose()
    {
        _stopThread.Set();
        _screenThread.Join();

        _writer.Close();

        _stopThread.Dispose();
    }

    public string FilePath()
    {
        return Path.Combine("Video", _params.FeatureName, _params.FileName);
    }

    private void RecordScreen()
    {
        var frameInterval = TimeSpan.FromSeconds(1 / (double)_writer.FramesPerSecond);
        var buffer = new byte[_params.Width * _params.Height * 4];
        Task videoWriteTask = null;
        var timeTillNextFrame = TimeSpan.Zero;

        while (!_stopThread.WaitOne(timeTillNextFrame))
        {
            var timestamp = DateTime.Now;

            Screenshot(buffer);

            videoWriteTask?.Wait();

            videoWriteTask = _videoStream.WriteFrameAsync(true, buffer, 0, buffer.Length);

            timeTillNextFrame = timestamp + frameInterval - DateTime.Now;
            if (timeTillNextFrame < TimeSpan.Zero)
                timeTillNextFrame = TimeSpan.Zero;
        }

        videoWriteTask?.Wait();
    }

    private void Screenshot(byte[] buffer)
    {
        using var bmp = new Bitmap(_params.Width, _params.Height);
        using var g = Graphics.FromImage(bmp);
        g.CopyFromScreen(Point.Empty, Point.Empty, new Size(_params.Width, _params.Height), CopyPixelOperation.SourceCopy);

        g.Flush();

        var bits = bmp.LockBits(new Rectangle(0, 0, _params.Width, _params.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
        Marshal.Copy(bits.Scan0, buffer, 0, buffer.Length);
        bmp.UnlockBits(bits);
    }
}