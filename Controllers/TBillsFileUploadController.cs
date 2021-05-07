using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using DigitalProductAPI.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace DigitalProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TBillsFileUploadController : ControllerBase
    {
        public TBillsFileUploadController(ILogger<TBillsFileUploadController> logger)
        {
            Logger = logger;
        }

        public ILogger<TBillsFileUploadController> Logger { get; }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] IFormFile tBillsTemplate)
        {
            Logger.LogInformation("File Bulk upload request for Treasury bills");

            if (tBillsTemplate == null)
            {
                Logger.LogWarning("Unpload template is null");
                return BadRequest();
            }
            if (!tBillsTemplate.FileName.ToLower().EndsWith(".xlsx"))
            {
                Logger.LogWarning("File is not an excel document with an extension of .xlsx");
                ModelState.AddModelError("fileFormat", "A valid .xlsx file should be uploaded");
                return BadRequest(ModelState);
            }
            List<TBillsFileUploadResponse> uploadResponse = new List<TBillsFileUploadResponse>();
            using (MemoryStream ms = new MemoryStream())
            {
                await tBillsTemplate.CopyToAsync(ms);
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage(ms))
                {                    
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        Logger.LogWarning("File does not contain a valid worksheet");
                        ModelState.AddModelError("fileFormat", "File does not contain a valid worksheet");
                        return BadRequest(ModelState);
                    }
                    if (worksheet.Dimension == null)
                    {
                        Logger.LogWarning("The worksheet cannot be empty");
                        ModelState.AddModelError("fileFormat", "The worksheet cannot be empty");
                        return BadRequest(ModelState);
                    }
                    int colCount = worksheet.Dimension.End.Column;  //get Column Count
                    int rowCount = worksheet.Dimension.End.Row;
                    //if (colCount != 9) return BadRequest();
                    if (colCount != 10 || rowCount < 2)
                    {
                        Logger.LogWarning("Check that the data in your worksheet is sufficient");
                        ModelState.AddModelError("fileFormat", "Check that the data in your worksheet is sufficient");
                        return BadRequest(ModelState);
                    }
                    
                    TBillsFileUploadResponse dto = null;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            dto = new TBillsFileUploadResponse
                            {
                                Id = row - 1,
                                SecurityID = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                MaturityValue = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                MaturityDate = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                Rate = worksheet.Cells[row, 4].Value.ToString().Trim(),
                                BidPrice = worksheet.Cells[row, 5].Value.ToString().Trim(),
                                Currency = worksheet.Cells[row, 6].Value.ToString().Trim(),
                                AvailableVolume = worksheet.Cells[row, 7].Value.ToString().Trim(),
                                OfferPrice = worksheet.Cells[row, 8].Value.ToString().Trim(),
                                Consideration = worksheet.Cells[row, 9].Value.ToString().Trim(),
                                FaceValue = worksheet.Cells[row, 10].Value.ToString().Trim(),
                            };
                            uploadResponse.Add(dto);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex, $"Unable to pass the excel file. check row please check the file");
                            //Logger.LogError(e, $"Unable to parse item in row {row}");
                        }
                    }
                }
            }
            if (uploadResponse == null)
            {
                Logger.LogWarning("The workSheet is empty");
                ModelState.AddModelError("fileFormat", "The workSheet is empty");
                return BadRequest(ModelState);
            }
            return Ok(uploadResponse);
        }
    }
}
