using LablePrint.Dtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace LablePrint.Controllers
{
    /// <summary>
    /// 标签打印
    /// </summary>
    public class PrintLableController : ApiController
    {
        public HttpHelp HttpHelp { get; set; }

       

        /// <summary>
        ///  料卷标签打印
        /// </summary>
        /// <param name="printReelDto">打印信息</param>
        [HttpPost]
        public void PrintReelLable(PrintReelDto printReelDto)
        {
            BarTender.Application btApp = new BarTender.Application();
            BarTender.Format btFormat;
            for (int i = 0; i < printReelDto.ReelQty; i++)
            {
                var reelOutLife = HttpHelp.RequsetAbp<PrintReelDto>(RequsetMethod.POST, "/api/services/app/PrintReel/GetNewPrintReel", printReelDto, null, false).Result;
                try
                {

                    string Path = System.Environment.CurrentDirectory + "\\" + ConfigurationManager.AppSettings["printTemp"];
                    btFormat = btApp.Formats.Open(@Path, true, "");
                    btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                    btApp.Visible = false;
                    btFormat.PrintSetup.NumberSerializedLabels = 1;
                    btFormat.SetNamedSubStringValue("PartNoId", reelOutLife.PartNoId);
                    btFormat.SetNamedSubStringValue("Qty", reelOutLife.Qty.ToString());
                    btFormat.SetNamedSubStringValue("DateCode", reelOutLife.DateCode);
                    btFormat.SetNamedSubStringValue("Id", reelOutLife.Id);
                    btFormat.SetNamedSubStringValue("LotCode", reelOutLife.LotCode);
                    btFormat.SetNamedSubStringValue("Supplier", reelOutLife.Supplier);
                    btFormat.SetNamedSubStringValue("Info", reelOutLife.Info);
                    btFormat.SetNamedSubStringValue("Name", reelOutLife.Name);
                    btFormat.PrintOut(false, false);
                    btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
    }
}
