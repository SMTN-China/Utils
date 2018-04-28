using Hangfire;
using NotificationService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace NotificationService.Controllers
{
    /// <summary>
    /// 任务管理
    /// </summary>
    public class TaskController : ApiController
    {
        public NLog.ILogger Logger { get; set; }

        public ErpSyncService ErpSyncService { get; set; }

        public OutLifeReelMailService OutLifeReelMailService { get; set; }

        public void TestTask()
        {
            RecurringJob.AddOrUpdate("TestTask", () => Test(), Hangfire.Cron.Minutely());
        }

        public void Test()
        {
            Logger.Info("任务测试");
        }

        /// <summary>
        /// 同步物料信息
        /// </summary>
        /// <param name="pn">可选输入料号,不输入则同步最近一周的新料号</param>
        [HttpPost]
        public void SyncMaterial(string[] pn)
        {
            ErpSyncService.SyncMaterial(pn);

        }
        /// <summary>
        /// 同步备料单信息
        /// </summary>
        /// <param name="readyMBill">可选输入备料单号,不输入则同步最近两天的新料号</param>
        [HttpPost]
        public void SyncReadyMBill(string[] readyMBill)
        {
            ErpSyncService.SyncReadyMBill(readyMBill);

        }



        /// <summary>
        /// 开启自动同步
        /// </summary>
        [HttpPost]
        public void StartAutoSync()
        {
            ErpSyncService.StartSyncTask();
        }

        static int i;
        /// <summary>
        /// 关闭自动同步
        /// </summary>
        [HttpPost]
        public void StopAutoSync()
        {
            ErpSyncService.StopSyncTask();
        }

        /// <summary>
        /// 开启超期物料计算
        /// </summary>
        [HttpPost]
        public void StartIqcOutLifeReel()
        {
            OutLifeReelMailService.StartMailTask();
        }

        /// <summary>
        /// 关闭超期物料计算
        /// </summary>
        [HttpPost]
        public void StopIqcOutLifeReel()
        {
            OutLifeReelMailService.StopMailTask();
        }

    }
}
