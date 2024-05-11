global using NAudio.Wave;

public class LoopStream : WaveStream
{
    readonly WaveStream _sourceStream;
 
    public bool EnableLooping { get; set; }
    
    public LoopStream(WaveStream sourceStream, bool enableLooping = true)
    {
        this._sourceStream = sourceStream;
        this.EnableLooping = enableLooping;
    }
 
    public override WaveFormat WaveFormat => _sourceStream.WaveFormat;

    public override long Length => _sourceStream.Length;

    public override long Position
    {
        get => _sourceStream.Position;
        set => _sourceStream.Position = value;
    }
 
    public override int Read(byte[] buffer, int offset, int count)
    {
        int totalBytesRead = 0;
 
        while (totalBytesRead < count)
        {
            int bytesRead = _sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
            if (bytesRead == 0)
            {
                if (_sourceStream.Position == 0 || !EnableLooping)
                {
                    // something wrong with the source stream
                    break;
                }
                // loop
                _sourceStream.Position = 0;
            }
            totalBytesRead += bytesRead;
        }
        return totalBytesRead;
    }
}