using LablePrint.Dtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Windows.Forms;

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
            for (int i = 0; i < printReelDto.ReelQty; i++)
            {
                var reelOutLife = HttpHelp.RequsetAbp<PrintReelDto>(RequsetMethod.POST, "/api/services/app/PrintReel/GetNewPrintReel", printReelDto, null, false).Result;
                try
                {

                    string Path = Application.StartupPath + "\\" + ConfigurationManager.AppSettings["printTemp"];
                    Main.btFormat = Main.btApp.Formats.Open(@Path, true, "");
                    Main.btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                    Main.btApp.Visible = false;
                    Main.btFormat.PrintSetup.NumberSerializedLabels = 1;
                    Main.btFormat.SetNamedSubStringValue("PartNoId", reelOutLife.PartNoId);
                    Main.btFormat.SetNamedSubStringValue("Qty", reelOutLife.Qty.ToString());
                    Main.btFormat.SetNamedSubStringValue("DateCode", reelOutLife.DateCode);
                    Main.btFormat.SetNamedSubStringValue("Id", reelOutLife.Id);
                    Main.btFormat.SetNamedSubStringValue("LotCode", reelOutLife.LotCode);
                    Main.btFormat.SetNamedSubStringValue("Supplier", reelOutLife.Supplier);
                    Main.btFormat.SetNamedSubStringValue("Info", reelOutLife.Info);
                    Main.btFormat.SetNamedSubStringValue("Name", reelOutLife.Name);
                    Main.btFormat.PrintOut(false, false);
                    Main.btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
    }
}
