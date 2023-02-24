using System.Text;
using System.Threading;
using System.Diagnostics;
using System;

namespace Api.OCR;

public class Base64Utils
{
  public String ExecBase64Script(String Base64String, String BlobURL)
  {
    var process = new Process();
    
    try {

      var processStartInfo = new ProcessStartInfo()
      {
          WindowStyle = ProcessWindowStyle.Hidden,
          FileName = $"extract-text.sh",
          Arguments = $" -b " + Base64String + " -u " + BlobURL,
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          UseShellExecute = false
      };

      process.StartInfo = processStartInfo;
      process.Start();
      var output = process.StandardOutput.ReadToEnd();
      
      return output;
    }
    catch (Exception ex)
    {
      var errorOutput = process.StandardError.ReadToEnd();
      return errorOutput + "; ex:" + ex;
    }
  }
}

