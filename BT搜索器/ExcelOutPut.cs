using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace BT搜索器
{
    class ExcelOutPut
    {
        public static bool SaveExcel(string filename,Spider._xpath[] data) {
            HSSFWorkbook workbook=new   HSSFWorkbook();
            ISheet  sheet = workbook.CreateSheet("种子信息");

            //创建表头
            IRow header =sheet.CreateRow(0);
            for (int i = 0; i < 6; i++)
            {
                header.CreateCell(i);
            }
            header.Cells[0].SetCellValue("来源");
            header.Cells[1].SetCellValue("标题");
            header.Cells[2].SetCellValue("日期");
            header.Cells[3].SetCellValue("大小");
            header.Cells[4].SetCellValue("热度");
            header.Cells[5].SetCellValue("磁力/种子");
            
            //写入内容
            for (int i = 0; i < data.Length; i++)
            {
                IRow row = sheet.CreateRow(i+1);
                row.CreateCell(0).SetCellValue(data[i].source);
                row.CreateCell(1).SetCellValue(data[i].title);
                row.CreateCell(2).SetCellValue(data[i].date);
                row.CreateCell(3).SetCellValue(data[i].size);
                row.CreateCell(4).SetCellValue(data[i].hot);
                row.CreateCell(5).SetCellValue(data[i].link);
            }
            try
            {
                FileStream f = new FileStream(filename, FileMode.OpenOrCreate);
                workbook.Write(f);
                f.Close();
                workbook.Close();
            }
            catch (Exception)
            {

                return false;
            }
           
            return true;
        }
    }
}
