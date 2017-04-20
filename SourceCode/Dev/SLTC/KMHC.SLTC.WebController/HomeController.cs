using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using System.Web.Mvc;
using KMHC.SLTC.Business.Entity.Model;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using KMHC.SLTC.Business.Interface;
using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Security;
using Newtonsoft.Json;
using System.Text;
using KMHC.SLTC.Business.Entity.EC;
using KMHC.SLTC.Business.Entity.EC.Filter;
using KMHC.SLTC.Business.Entity.Filter.NCI;
using KMHC.SLTC.Business.Entity.NCI;
using KMHC.SLTC.Business.Entity;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace KMHC.SLTC.WebController
{
    public class HomeController : Controller
    {
        [AntiForgeryAuthorizationd]
        public ActionResult Index(string userId)
        {
            var userInfo = new NCI_User();
            userInfo.UserName = SecurityHelper.CurrentPrincipal.UserName;
            userInfo.UserId = SecurityHelper.CurrentPrincipal.UserId;
            userInfo.OrgType = SecurityHelper.CurrentPrincipal.OrgType;
            userInfo.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            TempData["OrgType"] = userInfo.OrgType;  
            ViewBag.User = userInfo;

            //ViewBag.Notifications = NotificationList(userInfo.UserId.ToString());
            //ViewBag.Messages = MessageList(userInfo.UserId.ToString());
            //ViewBag.Tasks = TaskList(userInfo.UserId.ToString());
            return View("index");
        }

        public ActionResult Logout()
        {
            IAuthenticationService authenticationService = IOCContainer.Instance.Resolve<IAuthenticationService>();
            authenticationService.SignOut();
            TempData["OrgType"] = TempData["OrgType"] == null ? 2 : TempData["OrgType"];
            return RedirectToAction(((int)TempData["OrgType"]).ToEnumDescriptionString(typeof(OrgType)), "Login");
        }

        public ActionResult DashBoard()
        {

			//BuildJSON(GetDashboardData_In(SecurityHelper.CurrentPrincipal.OrgId), "dataInJSON.js");
			BuildJSON(GetDashboardData_In(SecurityHelper.CurrentPrincipal.OrgId), "dataInJSON.js");

            BuildJSON(GetDashboardData_Out(SecurityHelper.CurrentPrincipal.OrgId), "dataOutJSON.js");

            BuildJSON_Bed(GetDashboard_Bed(SecurityHelper.CurrentPrincipal.OrgId), "dataBedJSON.js");

            BuildJSON(GetDashboardData_BedSore(SecurityHelper.CurrentPrincipal.OrgId), "dataBedSoreJSON.js");

            return View();
        }
		
        public ActionResult MainForm()
        {
            

            return View();
        }

        public ActionResult NavigationForm()
        {
            if (Array.Exists(SecurityHelper.CurrentPrincipal.SysType, e => e.Equals("ADMIN")))
            {
                ViewBag.IsADMIN="true";
            }
            else 
            {
                if (Array.Exists(SecurityHelper.CurrentPrincipal.SysType, e => e.Equals("LC")))
                {
                    ViewBag.IsLC = "true";
                }
                if (Array.Exists(SecurityHelper.CurrentPrincipal.SysType, e => e.Equals("DC")))
                {
                    ViewBag.IsDC = "true";
                }
            }
         
            return View();
        }

        public List<Notification> NotificationList(string userID)
        {
            var result = new List<Notification>();
            for (int i = 0; i < 7; i++)
            {
                result.Add(new Notification(){Content = "这是第"+i+"提示",Title = "消息",TimeRange = i+"分钟前"});
            }
            return result;
        }

        public List<Message> MessageList(string userID)
        {
            var result = new List<Message>();
            for (int i = 0; i < 3; i++)
            {
                result.Add(new Message()
                {
                    Content = "这是第" + i + "消息",
                    FromName = "何尚文",
                    TimeRange = i + "分钟前",
                    FromUrl = "/Images/avatar2.jpg"
                });
            }
            return result;
        }

        public List<Task> TaskList(string userID)
        {
            var result = new List<Task>();
            for (int i = 0; i < 7; i++)
            {
                result.Add(new Task() { Name = "这是第" + i + "任务", Progress = (i*10),TaskID = i.ToString()});
            }
            return result;
        }

        /// <summary>生成验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult getCheckCode()
        {
            int number;
            char code;
            string checkCode = String.Empty;

            System.Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));
                checkCode += code.ToString();
            }
            //保存进Session
            Session["CheckCode"] = checkCode;

            byte[] bytes = CreateValidateGraphic(checkCode);

            return File(bytes, @"image/jpeg");
        }


        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="containsPage"></param>
        /// <param name="validateNum"></param>
        public static byte[] CreateValidateGraphic(string checkCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(checkCode.Length * 11.5), 22);
            Graphics g = Graphics.FromImage(image); 
            try
            {
                //生成随机生成器    
                Random random = new Random();
                //清空图片背景色     
                g.Clear(Color.White);
                //画图片的干扰线    
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width); int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                    Color.Blue, Color.DarkRed, 1.2f, true); g.DrawString(checkCode, font, brush, 3, 2);
                //画图片的前景干扰点    
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width); int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线  
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据     
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片   

                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        public bool userCheck(string name, string pwd,string orgId, ref NCI_User user) {
            IOrganizationManageService service = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            BaseRequest<NCI_UserFilter> request = new BaseRequest<NCI_UserFilter>();
            request.Data.Account = name;
            request.Data.Password = pwd;
            request.Data.OrgId = orgId;
            var userList = service.QueryUser(request);
            if( userList.Data.Count > 0)
            {
                user = userList.Data[0];
            }
            return userList.Data.Count > 0;
        }

        //public List<User> getList()
        //{
        //    return new List<User>()
        //    {
        //        new User() {UserId = 1,Name="admin",PASSWORD = "123",Organization="1"},
        //        new User() {UserId = 2,Name="user",PASSWORD = "123",Organization="2"},
        //    };
        //}


        /// <summary>
        /// 生成Dashboard数据
        /// </summary>
        /// <returns></returns>
        private string  BuildJSON(DataTable dt,string jsonName) {
            string rst = "";
            string filePath = Server.MapPath("/Dashboard/" + jsonName);
            string jsonText = DataTableToJson(dt);//JsonConvert.SerializeObject(GetDashboardData());
                //using (StreamWriter sw = new System.IO.StreamWriter(filePath,false))
                //{
                try
                {
                    System.IO.File.WriteAllText(filePath, jsonText);
                }
                catch (Exception ex)
                {

                }
            //}
            return rst;
        }
        private string BuildJSON_Bed(DataTable dt, string jsonName)
        {
            string rst = "";
            string filePath = Server.MapPath("/Dashboard/" + jsonName);
            string jsonText = DataTableToJson1(dt);//JsonConvert.SerializeObject(GetDashboardData());
            //using (StreamWriter sw = new System.IO.StreamWriter(filePath,false))
            //{
            try
            {
                System.IO.File.WriteAllText(filePath, jsonText);
            }
            catch (Exception ex)
            {

            }
            //}
            return rst;
        }
        /// <summary>
        /// 獲取當前入住人數統計
        /// </summary>
        /// <returns></returns>
        private DataTable GetDashboardData_In(string orgId)
        {
            ISocialWorkerManageService service = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();

            DataTable dt = service.QueryInData(orgId);

            return dt;
        }
        /// <summary>
        /// 獲取當前結案人數統計
        /// </summary>
        /// <returns></returns>
        private DataTable GetDashboardData_Out(string orgId)
        {
            ISocialWorkerManageService service = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();

            DataTable dt = service.QueryOutData(orgId);

            return dt;
        }
        /// <summary>
        /// 統計床位使用數
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public DataTable GetDashboard_Bed(string orgId)
        {
            ISocialWorkerManageService service = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();

            DataTable dt = service.QueryBedData(orgId);

            return dt;
        }
        /// <summary>
        /// 獲取當前壓瘡人數統計
        /// </summary>
        /// <returns></returns>
        private DataTable GetDashboardData_BedSore(string orgId)
        {
            ISocialWorkerManageService service = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();

            DataTable dt = service.QueryBedSoreData(orgId);

            return dt;
        }
        private string DataTableToJson(DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("[");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append( dt.Rows[i][j].ToString());
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("]");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    Json.Append("[");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append(0);
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("]");
                    if (i < (3 - 1))
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]");
            return Json.ToString();
        }
        private string DataTableToJson1(DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                   // Json.Append("[");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append(dt.Rows[i][j].ToString());
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                   // Json.Append("]");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    Json.Append("[");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append(0);
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("]");
                    if (i < (3 - 1))
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]");
            return Json.ToString();
        }
    }

    /// <summary>
    ///   枚举辅助类  
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>  
        /// 扩展方法：根据枚举值得到相应的枚举定义字符串  
        /// </summary>  
        /// <param name="value"></param>  
        /// <param name="enumType"></param>  
        /// <returns></returns>  
        public static String ToEnumString(this int value, Type enumType)
        {
            NameValueCollection nvc = GetEnumStringFromEnumValue(enumType);
            return nvc[value.ToString()];
        }

        /// <summary>  
        /// 根据枚举类型得到其所有的 值 与 枚举定义字符串 的集合  
        /// </summary>  
        /// <param name="enumType"></param>  
        /// <returns></returns>  
        public static NameValueCollection GetEnumStringFromEnumValue(Type enumType)
        {
            NameValueCollection nvc = new NameValueCollection();
            Type typeDescription = typeof(DescriptionAttribute);
            System.Reflection.FieldInfo[] fields = enumType.GetFields();
            string strText = string.Empty;
            string strValue = string.Empty;
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    nvc.Add(strValue, field.Name);
                }
            }
            return nvc;
        }

        /// <summary>  
        /// 扩展方法：根据枚举值得到属性Description中的描述, 如果没有定义此属性则返回空串  
        /// </summary>  
        /// <param name="value"></param>  
        /// <param name="enumType"></param>  
        /// <returns></returns>  
        public static String ToEnumDescriptionString(this int value, Type enumType)
        {
            NameValueCollection nvc = GetNVCFromEnumValue(enumType);
            return nvc[value.ToString()];
        }

        /// <summary>  
        /// 根据枚举类型得到其所有的 值 与 枚举定义Description属性 的集合  
        /// </summary>  
        /// <param name="enumType"></param>  
        /// <returns></returns>  
        public static NameValueCollection GetNVCFromEnumValue(Type enumType)
        {
            NameValueCollection nvc = new NameValueCollection();
            Type typeDescription = typeof(DescriptionAttribute);
            System.Reflection.FieldInfo[] fields = enumType.GetFields();
            string strText = string.Empty;
            string strValue = string.Empty;
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        DescriptionAttribute aa = (DescriptionAttribute)arr[0];
                        strText = aa.Description;
                    }
                    else
                    {
                        strText = "";
                    }
                    nvc.Add(strValue, strText);
                }
            }
            return nvc;
        }
    }
}
