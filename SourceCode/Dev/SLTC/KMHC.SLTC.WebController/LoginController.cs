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
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.EC;
using KMHC.SLTC.Business.Entity.EC.Filter;
using KMHC.SLTC.Business.Entity.Filter.NCI;
using KMHC.SLTC.Business.Entity.NCI;

namespace KMHC.SLTC.WebController
{
    public class LoginController : Controller
    {
        private IOrganizationManageService userService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
        public ActionResult Index(string userId)
        {
            return this.Nursing("", "", "", "");
        }
        public ActionResult Nursing(string name, string pwd, string code, string orgId)
        {
            BaseRequest<ORGFilter> request = new BaseRequest<ORGFilter>();
            var response = userService.QueryOrgNsHome(request);
            ViewBag.Msg = "";
            if (response.ResultCode == 0)
            {
                ViewBag.OrgList = response.Data;
            }
            else
            {
                ViewBag.OrgList = new List<NursingHome>();
            }
            //if (Constants.ServerIdentify.ContainsKey(Computer.DiskID) && Constants.ServerIdentify[Computer.DiskID] == Computer.MacAddress)
                if (true)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (SignIn(name, pwd, orgId, OrgType.NursingHome))
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                        {
                            return Redirect(HttpUtility.UrlDecode(Request.QueryString["ReturnUrl"]));
                        }
                        return Redirect("/NCIA/HlxSbaoDesk");
                    }
                    else
                    {
                        ViewBag.Msg = "请输入正确信息";
                    }
                }
            }
            else
            {
                ViewBag.Msg = "机器未注册,请联系管理员";
            }
          
            return View();
        }

        public ActionResult Agency(string name, string pwd, string code, string orgId)
        {
            BaseRequest<ORGFilter> request = new BaseRequest<ORGFilter>();
            var response = userService.QueryOrgAgency(request);
            ViewBag.Msg = "";
            if (response.ResultCode == 0)
            {
                ViewBag.OrgList = response.Data;
            }
            else
            {
                ViewBag.OrgList = new List<Agency>();
            }
            //if (Constants.ServerIdentify.ContainsKey(Computer.DiskID) && Constants.ServerIdentify[Computer.DiskID] == Computer.MacAddress)
                if (true)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (SignIn(name, pwd, orgId, OrgType.Agency))
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                        {
                            return Redirect(HttpUtility.UrlDecode(Request.QueryString["ReturnUrl"]));
                        }
                        return Redirect("/Home/Index");
                    }
                    else
                    {
                        ViewBag.Msg = "请输入正确信息";
                    }
                }
            }
            else
            {
                ViewBag.Msg = "机器未注册,请联系管理员";
            }
      
            return View();
        }

        public ActionResult Ins(string name, string pwd, string code, string orgId)
        {
            BaseRequest<ORGFilter> request = new BaseRequest<ORGFilter>();
            var response = userService.QueryOrgInHome(request);
            ViewBag.Msg = "";
            if (response.ResultCode == 0)
            {
                ViewBag.OrgList = response.Data;
            }
            else
            {
                ViewBag.OrgList = new List<Insurance>();
            }
            if (!string.IsNullOrEmpty(name))
            {
                if (SignIn(name, pwd, orgId, OrgType.InsuranceCompany))
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                    {
                        return Redirect(HttpUtility.UrlDecode(Request.QueryString["ReturnUrl"]));
                    }
                    return Redirect("/Home/Index");
                }
                else
                {
                    ViewBag.Msg = "请输入正确信息";
                }
            }
            return View();
        }

        private bool SignIn(string name, string pwd, string orgId, OrgType orgType)
        {
            NCI_User user = null;
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(pwd) && UserCheck(name, pwd, orgId, orgType, ref user))
            {
                IAuthenticationService authenticationService = IOCContainer.Instance.Resolve<IAuthenticationService>();
                ClientUserData clientUserData = new ClientUserData()
                {
                    UserId = user.UserId,
                    UserIdInt = user.UserId,
                    Account = user.Account,
                    UserName = user.UserName,
                    OrgId = orgId,// 超级管理员可能登陆进入不同Org, 因此这里用登陆选择的ORG.
                    RoleType = user.RoleType,
                    OrgType = (int)orgType,
                    RoleId = user.RoleId,
                    LoginName = user.Account,
                    GovId=user.BelongToGovId
                };
                authenticationService.SignIn(clientUserData, true);
                return true;
            }

            return false;
        }
        private bool UserCheck(string name, string pwd, string orgId, OrgType orgType, ref NCI_User user)
        {
            //TODO 添加更丰富的返回信息,给予失败提示
            pwd = Util.Encryption(pwd);
            var userQueryResponse = userService.QueryUserByAccount(name);
            if (userQueryResponse == null || userQueryResponse.Data == null)
                return false;

            var stroedUser = userQueryResponse.Data;
            if (stroedUser.Password != pwd)
                return false;

            if (stroedUser.Status!= (int)AccountStatus.Enable)
                return false;

            var isSuperAdmin = false;

            if (stroedUser.OrgId != orgId && !(isSuperAdmin = CheckIsSuperAdmin(stroedUser)))
                return false;

            if (stroedUser.OrgId != orgId || stroedUser.OrgType != (int)orgType)
            {
                if(!(isSuperAdmin = CheckIsSuperAdmin(stroedUser)))//非超级管理员,只能登陆本机构.
                    return false;
            }

            var request = new BaseRequest<NCI_UserFilter>
            {
                Data =
                {
                    Account = name,
                    Password = pwd
                }
            };

            if (!isSuperAdmin)
                request.Data.OrgId = orgId;

            //TODO 改进逻辑,在最初就获取UserExtend对象.
            var userList = userService.QueryUserExtend(request);
            if (userList.Data.Count > 0)
            {
                user = userList.Data[0];
            }
            return userList.Data.Count > 0;
        }

        private bool CheckIsSuperAdmin(NCI_User user)
        {
            //检查是否是超级管理员,只有超级管理员可以登入所有机构.
            var userRoleResponse = userService.GetRole(user.RoleId);
            if (userRoleResponse == null || userRoleResponse.Data == null)
                return false;

            var userRole = userRoleResponse.Data;
            return userRole.RoleType == EnumRoleType.SuperAdmin.ToString();
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
    }
}
