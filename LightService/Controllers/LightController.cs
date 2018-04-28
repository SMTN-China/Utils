using LightService.Dtos;
using LightService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace LightService.Controllers
{
    public class LightController : ApiController
    {

        public LYLightService LYHelper { get; set; }

        /// <summary>
        /// 单灯或者多灯控制
        /// </summary>
        /// <param name="storageLight">灯控制合集</param>
        [HttpPost]
        public void LightOrder(List<StorageLight> storageLight)
        {
            // 获取立于货架集合
            var shelfLY = storageLight.Where(l => l.MainBoardId.ToString().Length < 4);

            // 货架亮灯
            if (shelfLY.Count() > 0)
            {

                LYHelper.LightOrder(shelfLY.ToList());
            }

        }

        /// <summary>
        /// 灯塔控制
        /// </summary>
        /// <param name="houseLights">灯塔指令合集</param>
        [HttpPost]
        public void HouseOrder(List<HouseLight> houseLights)
        {
            // 获取立于货架集合
            var shelfLY = houseLights.Where(l => l.MainBoardId.ToString().Length < 4);

            // 货架亮灯
            if (shelfLY.Count() > 0)
            {

                LYHelper.HouseOrder(shelfLY.ToList());
            }

        }

        /// <summary>
        /// 多灯控制
        /// </summary>
        /// <param name="allLightOrders">多灯指令</param>
        [HttpPost]
        public void AllLightOrder(List<AllLight> allLightOrders)
        {

            // 获取立于货架集合
            var shelfLY = allLightOrders.Where(l => l.MainBoardId.ToString().Length < 4);

            // 货架亮灯
            if (shelfLY.Count() > 0)
            {
                LYHelper.AllLightOrder(shelfLY.ToList());
            }
        }
    }
}
