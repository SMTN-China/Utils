using Dapper;
using Hangfire;
using NotificationService.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Services
{

    public class ErpSyncService
    {
        public IDbConnection ErpDB { get; set; }

        public HttpHelp HttpHelp { get; set; }

        public void SyncMaterial(string[] pn = null)
        {
            string sql = @"SELECT
    ltrim(rtrim(itemno)) Id,
    ltrim(rtrim(itemname)) Name,
	ltrim(rtrim(description)) Info,
    NULL Remark,
	30 ShelfLife,
	itemclass MPNHierarchy,
    itemlevel MPNLevel,
	packagemax MPQ1,
    itemtype MPNType,
	0 MSDLevel,
	0 IncomingMethod,
	'True' IsActive
FROM
    material_erp";
            if (pn == null || pn.Length < 0)
            {
                sql += " where DateDiff(dd,mtime,getdate())<=7";
            }
            else
            {
                sql += " where ltrim(rtrim(itemno)) in ('" + string.Join("','", pn) + "')";
            }

            var res = ErpDB.Query<MPNDto>(sql);


            int pageIndex = 1, pageSize = 200, tol = res.Count();

            while ((pageIndex - 1) * pageSize < tol)
            {
                var async = res.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var ress = HttpHelp.RequsetAbp<bool>(RequsetMethod.POST, "/api/services/app/MPN/BatchInsOrUpdate", async, null, false);
                pageIndex += 1;
            }
        }


        public void SyncReadyMBill(string[] readyMBill = null)
        {
            // 获取配置文件
            var config = HttpHelp.RequsetAbp<SettingValue[]>(RequsetMethod.POST, "/api/services/app/Configuration/GetAppConfig", new string[] { "defaultForCustomerMStorageId", "defaultForSelfMStorageId", "reelMoveMethodId" }, null, false).Result;

            string sql = string.Format(@"SELECT
	ltrim(rtrim(reqno)) Id,
	'True' IsActive,
	ltrim(rtrim(parentitem)) ProductId,
	ltrim(rtrim(line)) LineId,
	ltrim(rtrim(sono)) WorkBillId,
	soqty WorkQty,
	ltrim(rtrim(partno)) PartNoId,
	reqqty PartNoQty,
    CASE WHEN SUBSTRING(ltrim(rtrim(partno)),1,1)='2' THEN 36*30 ELSE 18*30 END  as ShelfLife,
	'{0}' ReelMoveMethodId,
	'{1}' ForCustomerMStorageId,
	'{2}' ForSelfMStorageId
FROM
	materialrequest_erp", config.FirstOrDefault(c => c.Name == "reelMoveMethodId").Value, config.FirstOrDefault(c => c.Name == "defaultForCustomerMStorageId").Value, config.FirstOrDefault(c => c.Name == "defaultForSelfMStorageId").Value);

            if (readyMBill == null || readyMBill.Length == 0)
            {
                sql += " where DateDiff(dd,mtime,getdate())<=2";
            }
            else
            {
                sql += " where ltrim(rtrim(reqno)) in ('" + string.Join("','", readyMBill) + "')";
            }

            var res = ErpDB.Query<RBBatchReadyMBillDto>(sql, System.Data.CommandType.Text);



            // 先同步物料信息,分批次

            SyncMaterial(res.GroupBy(r => r.PartNoId).Select(r => r.Key).Union(res.GroupBy(r => r.ProductId).Select(r => r.Key)).ToArray());

            // 分批次同步备料信息
            int pageIndex = 1, pageSize = 200, tol = res.Count();

            while ((pageIndex - 1) * pageSize < tol)
            {
                var async = res.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var ress = HttpHelp.RequsetAbp<bool>(RequsetMethod.POST, "/api/services/app/ReadyMBill/RBBatchInsOrUpdate", async, null, false);
                pageIndex += 1;
            }
        }


        public void StartSyncTask()
        {
            var config = HttpHelp.RequsetAbp<SettingValue[]>(RequsetMethod.POST, "/api/services/app/Configuration/GetAppConfig", new string[] { "asyncInterval" }, null, false).Result;
            RecurringJob.AddOrUpdate("ErpSync", () => SyncReadyMBill(null), Cron.HourInterval(int.Parse(config.FirstOrDefault(c => c.Name == "asyncInterval").Value)));
        }

        public void StopSyncTask()
        {
            RecurringJob.RemoveIfExists("ErpSync");
        }
    }
}
