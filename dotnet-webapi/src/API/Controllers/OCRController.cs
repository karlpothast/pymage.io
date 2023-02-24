using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using Api.OCR;

#nullable disable

namespace pymage.io.WebUI.Controllers;

//public class GetOCRTextController : ApiControllerBase
//{
   // [HttpGet]
   // [Route("api/GetOCRText")]
   // [EnableCors]
   // public ActionResult<string> GetOCRText()
   // {
    //  OCRText ocr1 = new OCRText();
    //  Console.WriteLine("calling bash class ");
    //  var result = ocr1.ExecPythonOCRScript();
    //  return result;
    //}
//}

public class Base64RequestBody
{
    public String base64String { get; set; }
    public String blobURL { get; set; }
}

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
[EnableCors]
public class OCRController : ControllerBase
{
  [HttpPost]
  [Route("PostBase64")]
  [EnableCors]
  public ActionResult<string> PostBase64Result(Base64RequestBody input)
  {  
    Base64Utils b64utils = new Base64Utils();
    var result = b64utils.ExecBase64Script(input.base64String, input.blobURL);
    return result;
  }
}