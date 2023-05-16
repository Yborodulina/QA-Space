using System.IO;
using PlanA.Web.Core.Core.WebDriver;
using SharpAvi;
using SharpAvi.Codecs;
using SharpAvi.Output;

namespace PlanA.Web.Core.Helper;

public class RecorderParams
{
    public int Height { get; }
    public int Width { get; }

    public readonly string FileName;
    public readonly string FeatureName;
    private readonly int _framesPerSecond;
    private readonly int _quality;
    private readonly FourCC _codec;

    public RecorderParams(string featureName, string filename, int frameRate, FourCC encoder, int quality)
    {
        FileName = filename;
        FeatureName = featureName;
        _framesPerSecond = frameRate;
        _codec = encoder;
        _quality = quality;
        Height = Driver.Instance.Manage().Window.Size.Height;
        Width = Driver.Instance.Manage().Window.Size.Width;
    }

    public AviWriter CreateAviWriter()
    {
        return new AviWriter(Path.Combine(FileHelper.CreateFolder("Video", FeatureName), FileName))
        {
            FramesPerSecond = _framesPerSecond,
            EmitIndex1 = true
        };
    }

    public IAviVideoStream CreateVideoStream(AviWriter writer)
    {
        if (_codec == CodecIds.Uncompressed)
            return writer.AddUncompressedVideoStream(Width, Height);
        if (_codec == CodecIds.X264)
            return writer.AddEncodingVideoStream(new UncompressedVideoEncoder(Width, Height));
        return writer.AddMpeg4VcmVideoStream(Width, Height, (double)writer.FramesPerSecond,
            quality: _quality,
            codec: _codec,
            forceSingleThreadedAccess: true);
    }
}