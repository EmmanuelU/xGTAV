using System;
using System.IO;
namespace xGTAV.IO
{
    public interface ISerializable : IDisposable
    {
        byte[] Serialize();
        bool UnSerialize(IOReader reader);
    }
}
