using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    class ScaleBrandSeriesModel
    {
        public List<ScaleSeries> GetByBrandID(int brandId) {
            List<ScaleSeries> list = null;
            string condition = ScaleSeriesEnum.brand_id.ToString() + " = " + brandId;
            String sql = MyHelper.DbBaseHelper.getSelectSql(DataTabeName.scale_series.ToString(),null,condition);
            list = MyHelper.DbBaseHelper.DataTableToEntitys<ScaleSeries>(DatabaseOPtionHelper.GetInstance().select(sql));
            return list;
        }

        public List<ScaleSeries> GetAll(){
            List<ScaleSeries> list = null;
            String sql = MyHelper.DbBaseHelper.getSelectSql(DataTabeName.scale_series.ToString());
            list = MyHelper.DbBaseHelper.DataTableToEntitys<ScaleSeries>(DatabaseOPtionHelper.GetInstance().select(sql));
            return list;
         }
    }
}
