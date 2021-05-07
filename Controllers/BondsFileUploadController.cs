using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class BondsFileUploadController : ControllerBase
    {
        public BondsFileUploadController(ILogger<BondsFileUploadController> logger)
        {
            Logger = logger;
        }

        public ILogger<BondsFileUploadController> Logger { get; }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] IFormFile bondsTemplate)
        {
            Logger.LogInformation("File Bulk upload request for bonds");
            if (bondsTemplate == null)
            {
                Logger.LogWarning("File Please upload a valid file... No file was uploaded");
                return BadRequest();
            }
            if (!bondsTemplate.FileName.ToLower().EndsWith(".xlsx"))
            {
                Logger.LogWarning("File must be an excel file with an extension of .xlsx");
                ModelState.AddModelError("fileFormat", "A valid .xlsx file should be uploaded");
                return BadRequest(ModelState);
            }
            List<BondsFileUploadResponse> uploadResponse = new List<BondsFileUploadResponse>();
            using (MemoryStream ms = new MemoryStream())
            {
                await bondsTemplate.CopyToAsync(ms);
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage(ms))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null) 
                    {
                        Logger.LogWarning("no worksheet in the uploaded file");
                        ModelState.AddModelError("fileFormat", "no worksheet in the uploaded file");
                        return BadRequest(ModelState);
                    }
                    if (worksheet.Dimension == null)
                    {
                        Logger.LogWarning("Worksheet is empty");

                        ModelState.AddModelError("fileFormat", "The worksheet cannot be empty");
                        return BadRequest(ModelState);
                    }
                    int colCount = worksheet.Dimension.End.Column;  //get Column Count
                    int rowCount = worksheet.Dimension.End.Row;
                    //if (colCount != 9) return BadRequest();
                    if (colCount != 10 || rowCount < 2)
                    {
                        Logger.LogWarning("the worksheet contain more column than expected");

                        ModelState.AddModelError("fileFormat", "Check that the data in your worksheet is sufficient");
                        return BadRequest(ModelState);
                    }

                    BondsFileUploadResponse dto = null;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            dto = new BondsFileUploadResponse
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
                            Logger.LogError(ex, "unable to read through the excel check the file");
                            return StatusCode(500);
                        }
                    }
                }
            }
            if (uploadResponse == null)
            {
                Logger.LogWarning("the worksheet is empty");
                ModelState.AddModelError("fileFormat", "The workSheet is empty");
                return BadRequest(ModelState);
            }
            return Ok(uploadResponse);
        }
    }
}
