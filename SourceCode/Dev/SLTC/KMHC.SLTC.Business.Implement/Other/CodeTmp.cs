﻿using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Implement.Base
{
    public class CodeTmp
    {
        public static IList<CodeValue> CreateData()
        {
            IList<CodeValue> list = new List<CodeValue>
            {
                new CodeValue() {Name = "男", Value = "1", Parent = "Gender"},
                new CodeValue() {Name = "女", Value = "2", Parent = "Gender"},
                new CodeValue() {Name = "全日型住宿", Value = "1", Parent = "ServiceType"},
                new CodeValue() {Name = "夜间型住宿", Value = "2", Parent = "ServiceType"},
                new CodeValue() {Name = "自杀倾向", Value = "1", Parent = "WorkerTip"},
                new CodeValue() {Name = "家庭问题", Value = "2", Parent = "WorkerTip"},
                new CodeValue() {Name = "病危期", Value = "1", Parent = "NurseTip"},
                new CodeValue() {Name = "急性期", Value = "2", Parent = "NurseTip"},
                new CodeValue() {Name = "情不稳", Value = "1", Parent = "Reason"},
                new CodeValue() {Name = "眼睛失明", Value = "2", Parent = "Reason"},
                new CodeValue() {Name = "在院中", Value = "1", Parent = "Status"},
                new CodeValue() {Name = "出院", Value = "2", Parent = "Status"},
                new CodeValue() {Name = "天主教", Value = "1", Parent = "Religion"},
                new CodeValue() {Name = "基督教", Value = "2", Parent = "Religion"},
                new CodeValue() {Name = "中文", Value = "1", Parent = "Language"},
                new CodeValue() {Name = "英文", Value = "2", Parent = "Language"},
                new CodeValue() {Name = "正常", Value = "1", Parent = "Communication"},
                new CodeValue() {Name = "笔谈", Value = "2", Parent = "Communication"},
                new CodeValue() {Name = "配偶", Value = "1", Parent = "Caregiver"},
                new CodeValue() {Name = "儿女", Value = "2", Parent = "Caregiver"},
                new CodeValue() {Name = "与配偶同住", Value = "1", Parent = "Live"},
                new CodeValue() {Name = "与儿女同住", Value = "2", Parent = "Live"},
                new CodeValue() {Name = "金牛座", Value = "1", Parent = "Constellation"},
                new CodeValue() {Name = "天平座", Value = "2", Parent = "Constellation"},
                new CodeValue() {Name = "转诊", Value = "1", Parent = "Source"},
                new CodeValue() {Name = "门诊", Value = "2", Parent = "Source"},
                new CodeValue() {Name = "100", Value = "1", Parent = "Postcode"},
                new CodeValue() {Name = "101", Value = "2", Parent = "Postcode"},
                new CodeValue() {Name = "女", Value = "1", Parent = "LabelName"},
                new CodeValue() {Name = "子", Value = "2", Parent = "LabelName"},
                new CodeValue() {Name = "夫或妻", Value = "3", Parent = "LabelName"},
                new CodeValue() {Name = "兄弟姐妹", Value = "4", Parent = "LabelName"},
                new CodeValue() {Name = "其他血亲", Value = "1", Parent = "BloodRelationship"},
                new CodeValue() {Name = "朋友", Value = "2", Parent = "BloodRelationship"},
                new CodeValue() {Name = "社政人员", Value = "3", Parent = "BloodRelationship"},
                new CodeValue() {Name = "原生家庭", Value = "4", Parent = "BloodRelationship"},
                new CodeValue() {Name = "一般家属", Value = "1", Parent = "Category"},
                new CodeValue() {Name = "法定代理人", Value = "2", Parent = "Category"},
                new CodeValue() {Name = "紧急联系人", Value = "3", Parent = "Category"},
                new CodeValue() {Name = "糖尿病", Value = "1", Parent = "Condition"},
                new CodeValue() {Name = "高血压", Value = "2", Parent = "Condition"},
                new CodeValue() {Name = "存", Value = "1", Parent = "Disability"},
                new CodeValue() {Name = "残", Value = "2", Parent = "Disability"},
                new CodeValue() {Name = "工", Value = "1", Parent = "Profession"},
                new CodeValue() {Name = "公", Value = "2", Parent = "Profession"},
                new CodeValue() {Name = "商", Value = "3", Parent = "Profession"},
                new CodeValue() {Name = "自由", Value = "4", Parent = "Profession"},
                new CodeValue() {Name = "不佳", Value = "1", Parent = "EconomyStatus"},
                new CodeValue() {Name = "良好", Value = "2", Parent = "EconomyStatus"},
                new CodeValue() {Name = "自费", Value = "1", Parent = "Qualification"},
                new CodeValue() {Name = "中低收入补助", Value = "2", Parent = "Qualification"},
                new CodeValue() {Name = "疾病", Value = "1", Parent = "CheckReason"},
                new CodeValue() {Name = "养老", Value = "2", Parent = "CheckReason"},
                new CodeValue() {Name = "步行", Value = "1", Parent = "CheckMode"},
                new CodeValue() {Name = "轮椅", Value = "2", Parent = "CheckMode"},
                new CodeValue() {Name = "无", Value = "3", Parent = "FinancialStatus"},
                new CodeValue() {Name = "中低收入", Value = "4", Parent = "FinancialStatus"},
                new CodeValue() {Name = "日托型", Value = "1", Parent = "Type"},
                new CodeValue() {Name = "安置型", Value = "2", Parent = "Type"},
                new CodeValue() {Name = "无", Value = "1", Parent = "ManualBarrier"},
                new CodeValue() {Name = "植物人", Value = "2", Parent = "ManualBarrier"},
                new CodeValue() {Name = "小学", Value = "1", Parent = "Education"},
                new CodeValue() {Name = "初中", Value = "2", Parent = "Education"},
                new CodeValue() {Name = "会计", Value = "1", Parent = "Job"},
                new CodeValue() {Name = "教师", Value = "2", Parent = "Job"},
                new CodeValue() {Name = "未婚", Value = "1", Parent = "Marriage"},
                new CodeValue() {Name = "已婚", Value = "2", Parent = "Marriage"},
                new CodeValue() {Name = "A型", Value = "1", Parent = "BloodType"},
                new CodeValue() {Name = "B型", Value = "2", Parent = "BloodType"},
                new CodeValue() {Name = "AB型", Value = "3", Parent = "BloodType"},
                new CodeValue() {Name = "O型", Value = "4", Parent = "BloodType"},
                new CodeValue() {Name = "其它", Value = "5", Parent = "BloodType"},
                new CodeValue() {Name = "碎食", Value = "1", Parent = "FoodHabit"},
                new CodeValue() {Name = "软食", Value = "2", Parent = "FoodHabit"},
                new CodeValue() {Name = "糊食", Value = "3", Parent = "FoodHabit"},
                new CodeValue() {Name = "住院", Value = "1", Parent = "StopFoodReason"},
                new CodeValue() {Name = "请假", Value = "2", Parent = "StopFoodReason"},
                new CodeValue() {Name = "糖尿病", Value = "糖尿病", Parent = "ClanCaseHistory"},
                new CodeValue() {Name = "高血压", Value = "高血压", Parent = "ClanCaseHistory"},
                new CodeValue() {Name = "高血脂", Value = "高血脂", Parent = "ClanCaseHistory"},
                new CodeValue() {Name = "心理评量", Value = "1", Parent = "ArchiveType"},
                new CodeValue() {Name = "相关公文", Value = "2", Parent = "ArchiveType"},
                new CodeValue() {Name = "特别记录", Value = "3", Parent = "ArchiveType"},
                new CodeValue() {Name = "台湾双联养老集团", Value = "1", Parent = "GroupList"},
                new CodeValue() {Name = "中国红会关爱集团", Value = "2", Parent = "GroupList"},
                new CodeValue() {Name = "美联希望集团", Value = "3", Parent = "GroupList"},
                new CodeValue() {Name = "人事部门", Value = "1", Parent = "DeptList"},
                new CodeValue() {Name = "财力部门", Value = "2", Parent = "DeptList"},
                new CodeValue() {Name = "养老部门", Value = "3", Parent = "DeptList"},
                new CodeValue() {Name = "医药部门", Value = "4", Parent = "DeptList"},
                new CodeValue() {Name = "经理", Value = "1", Parent = "TitelList"},
                new CodeValue() {Name = "部长", Value = "2", Parent = "TitelList"},
                new CodeValue() {Name = "员工", Value = "3", Parent = "TitelList"},
                new CodeValue() {Name = "夕阳红老年福利院", Value = "1", Parent = "OrgList"},
                new CodeValue() {Name = "红会老年活动中心", Value = "2", Parent = "OrgList"},
                new CodeValue() {Name = "深圳老年人服务中心", Value = "3", Parent = "OrgList"},
                new CodeValue() {Name = "嘉年华老年中心", Value = "4", Parent = "OrgList"},
                new CodeValue() {Name = "否", Value = "1", Parent = "IsOrgCtrl"},
                new CodeValue() {Name = "是", Value = "2", Parent = "IsOrgCtrl"},
                new CodeValue() {Name = "正常", Value = "1", Parent = "StatusList"},
                new CodeValue() {Name = "待定", Value = "2", Parent = "StatusList"},
                new CodeValue() {Name = "失效", Value = "3", Parent = "StatusList"},
                new CodeValue() {Name = "7", Value = "7", Parent = "EvalDateGap"},
                new CodeValue() {Name = "30", Value = "30", Parent = "EvalDateGap"},
                new CodeValue() {Name = "31", Value = "31", Parent = "EvalDateGap"},
                new CodeValue() {Name = "60", Value = "60", Parent = "EvalDateGap"},
                new CodeValue() {Name = "61", Value = "61", Parent = "EvalDateGap"},
                new CodeValue() {Name = "90", Value = "90", Parent = "EvalDateGap"},
                new CodeValue() {Name = "180", Value = "180", Parent = "EvalDateGap"},
                new CodeValue() {Name = "365", Value = "365", Parent = "EvalDateGap"},
                new CodeValue() {Name = "0", Value = "0", Parent = "EvalDateGap"},
                new CodeValue() {Name = "良好", Value = "1", Parent = "HealthStatus"},
                new CodeValue() {Name = "不好", Value = "2", Parent = "HealthStatus"},
                new CodeValue() {Name = "已預立", Value = "1", Parent = "CureList"},
                new CodeValue() {Name = "未預立", Value = "2", Parent = "CureList"},
                new CodeValue() {Name = "拒絕預立", Value = "3", Parent = "CureList"},
                new CodeValue() {Name = "無", Value = "1", Parent = "CloseReason"},
                new CodeValue() {Name = "痊癒", Value = "2", Parent = "CloseReason"},
                new CodeValue() {Name = "轉院", Value = "3", Parent = "CloseReason"},
                new CodeValue() {Name = "返家", Value = "4", Parent = "CloseReason"},
                new CodeValue() {Name = "失蹤", Value = "5", Parent = "CloseReason"},
                new CodeValue() {Name = "死亡", Value = "6", Parent = "CloseReason"},
                new CodeValue() {Name = "有", Value = "1", Parent = "YesOrNo"},
                new CodeValue() {Name = "無", Value = "2", Parent = "YesOrNo"},
                new CodeValue() {Name = "土葬", Value = "1", Parent = "BodiesHandledList"},
                new CodeValue() {Name = "火葬", Value = "2", Parent = "BodiesHandledList"},
                new CodeValue() {Name = "海葬", Value = "3", Parent = "BodiesHandledList"},
                new CodeValue() {Name = "捐獻大體給醫院", Value = "4", Parent = "BodiesHandledList"},
                new CodeValue() {Name = "公墓", Value = "1", Parent = "BodiesConserveList"},
                new CodeValue() {Name = "家屬領回", Value = "2", Parent = "BodiesConserveList"},
                new CodeValue() {Name = "靈骨塔", Value = "3", Parent = "BodiesConserveList"},
                new CodeValue() {Name = "由家屬領回", Value = "1", Parent = "HeritageHandledList"},
                new CodeValue() {Name = "交友人領回", Value = "2", Parent = "HeritageHandledList"},
                new CodeValue() {Name = "喪葬委員會處理", Value = "3", Parent = "HeritageHandledList"},
                new CodeValue() {Name = "病故", Value = "1", Parent = "DieReasonList"},
                new CodeValue() {Name = "自然死亡", Value = "2", Parent = "DieReasonList"},
                new CodeValue() {Name = "家中", Value = "1", Parent = "DieAddrList"},
                new CodeValue() {Name = "醫院", Value = "2", Parent = "DieAddrList"},
                new CodeValue() {Name = "日補助", Value = "1", Parent = "SubCal"},
                new CodeValue() {Name = "月補助(30天計)", Value = "2", Parent = "SubCal"},
                new CodeValue() {Name = "月補助(31天計)", Value = "3", Parent = "SubCal"},
                new CodeValue() {Name = "月補助(實際天數計)", Value = "4", Parent = "SubCal"},
                new CodeValue() {Name = "不需設定", Value = "5", Parent = "SubCal"},
                new CodeValue() {Name = "臺北市政府", Value = "1", Parent = "SubUnit"},
                new CodeValue() {Name = "基隆市政府", Value = "2", Parent = "SubUnit"},
                new CodeValue() {Name = "嘉義市政府", Value = "3", Parent = "SubUnit"},
                new CodeValue() {Name = "新竹市政府", Value = "4", Parent = "SubUnit"},
                new CodeValue() {Name = "新北市政府", Value = "5", Parent = "SubUnit"},
                new CodeValue() {Name = "0%", Value = "1", Parent = "Ratio"},
                new CodeValue() {Name = "25%", Value = "2", Parent = "Ratio"},
                new CodeValue() {Name = "50", Value = "3", Parent = "Ratio"},
                new CodeValue() {Name = "75%", Value = "4", Parent = "Ratio"},
                new CodeValue() {Name = "100%", Value = "5", Parent = "Ratio"},
                new CodeValue() {Name = "自費", Value = "1", Parent = "SubQua"},
                new CodeValue() {Name = "中低收入戶補助", Value = "2", Parent = "SubQua"},
                new CodeValue() {Name = "低收入戶補助", Value = "3", Parent = "SubQua"},
                new CodeValue() {Name = "身心障礙補助", Value = "4", Parent = "SubQua"},

                new CodeValue() {Name = "李國民", Value = "1", Parent = "Socialworkers"},
                new CodeValue() {Name = "張學亮", Value = "2", Parent = "Socialworkers"},
                new CodeValue() {Name = "花滿樓", Value = "3", Parent = "Socialworkers"},
                new CodeValue() {Name = "苗人鳳", Value = "4", Parent = "Socialworkers"},
                //資源連結中的需求類型 楊金高
                new CodeValue() {Name = "醫療及心理復健", Value = "1", Parent = "DemandType"},
                new CodeValue() {Name = "學校教育", Value = "2", Parent = "DemandType"},
                new CodeValue() {Name = "工作協助", Value = "3", Parent = "DemandType"},
                new CodeValue() {Name = "機構照顧服務", Value = "4", Parent = "DemandType"},
                
                new CodeValue() {Name = "經濟協助", Value = "5", Parent = "DemandType"},
                new CodeValue() {Name = "居住服務", Value = "6", Parent = "DemandType"},
                new CodeValue() {Name = "輔助協助", Value = "7", Parent = "DemandType"},
                new CodeValue() {Name = "家庭支持服務", Value = "8", Parent = "DemandType"},
                new CodeValue() {Name = "權益爭取倡導服務", Value = "9", Parent = "DemandType"},
                new CodeValue() {Name = "其他", Value = "10", Parent = "DemandType"},
                //資源連結中的需求名稱 楊金高
                new CodeValue() {Name = "醫療輔具", Value = "1", Parent = "DemandName"},
                new CodeValue() {Name = "改善居家無障礙環境", Value = "2", Parent = "DemandName"},
                new CodeValue() {Name = "短期照顧服務", Value = "3", Parent = "DemandName"},
                new CodeValue() {Name = "長期照顧服務", Value = "4", Parent = "DemandName"},
                new CodeValue() {Name = "居家環境改善", Value = "5", Parent = "DemandName"},
                new CodeValue() {Name = "住院看護", Value = "6", Parent = "DemandName"},
                new CodeValue() {Name = "休閒活動", Value = "7", Parent = "DemandName"},
                new CodeValue() {Name = "交通服務", Value = "8", Parent = "DemandName"},
                new CodeValue() {Name = "加強生活處理能力", Value = "9", Parent = "DemandName"},
                new CodeValue() {Name = "關懷，心理支持", Value = "10", Parent = "DemandName"},
                new CodeValue() {Name = "改善案家關係", Value = "11", Parent = "DemandName"},
                new CodeValue() {Name = "改善人際關係", Value = "12", Parent = "DemandName"},
                new CodeValue() {Name = "宗教關懷", Value = "13", Parent = "DemandName"},
                new CodeValue() {Name = "依托服務", Value = "14", Parent = "DemandName"},
                new CodeValue() {Name = "安全服務", Value = "15", Parent = "DemandName"},
                new CodeValue() {Name = "法律咨詢", Value = "16", Parent = "DemandName"},
                new CodeValue() {Name = "權益受損申訴", Value = "17", Parent = "DemandName"},
                new CodeValue() {Name = "權益倡導", Value = "18", Parent = "DemandName"},
                new CodeValue() {Name = "其他", Value = "19", Parent = "DemandName"},
                //資源連結中的需求評估結果 楊金高
                new CodeValue() {Name = "不需要", Value = "1", Parent = "DemandEvalResult"},
                new CodeValue() {Name = "不確定", Value = "2", Parent = "DemandEvalResult"},
                new CodeValue() {Name = "需要", Value = "3", Parent = "DemandEvalResult"},
                new CodeValue() {Name = "很需要", Value = "4", Parent = "DemandEvalResult"},
                new CodeValue() {Name = "迫切需要", Value = "5", Parent = "DemandEvalResult"},
                //提供單位名稱 楊金高
                new CodeValue() {Name = "單位名稱11111111111111", Value = "1", Parent = "UnitName"},
                new CodeValue() {Name = "單位名稱22222222222222", Value = "2", Parent = "UnitName"},
                new CodeValue() {Name = "單位名稱33333333333333", Value = "3", Parent = "UnitName"},
                new CodeValue() {Name = "單位名稱44444444444444", Value = "4", Parent = "UnitName"},
                //資源連結中的連結情形 楊金高
                new CodeValue() {Name = "待連結", Value = "1", Parent = "ResourceState"},
                new CodeValue() {Name = "未能連結到資源", Value = "2", Parent = "ResourceState"},
                new CodeValue() {Name = "部分連結資源", Value = "3", Parent = "ResourceState"},
                new CodeValue() {Name = "全部連結資源", Value = "4", Parent = "ResourceState"},
                 //資源連結中的未能連結原因 楊金高
                new CodeValue() {Name = "已連結資源不需填寫", Value = "1", Parent = "LinkReason"},
                new CodeValue() {Name = "沒有資源", Value = "2", Parent = "LinkReason"},
                new CodeValue() {Name = "資源本身的限制", Value = "3", Parent = "LinkReason"},
                new CodeValue() {Name = "缺乏輔助資源", Value = "4", Parent = "LinkReason"},
                new CodeValue() {Name = "資源已被案主耗盡", Value = "5", Parent = "LinkReason"},
                new CodeValue() {Name = "案主(家)內在障礙", Value = "6", Parent = "LinkReason"},
                //資源連結時機 楊金高
                new CodeValue() {Name = "入住階段", Value = "1", Parent = "RegState"},
                new CodeValue() {Name = "出院準備", Value = "2", Parent = "RegState"},
                new CodeValue() {Name = "重大疾病住院時", Value = "3", Parent = "RegState"},
                new CodeValue() {Name = "結案後", Value = "4", Parent = "RegState"},
                //問題分類 楊金高
                new CodeValue() {Name = "行政法令查詢", Value = "1", Parent = "QuestionCategory"},
                new CodeValue() {Name = "其他", Value = "2", Parent = "QuestionCategory"},
                new CodeValue() {Name = "院民權益維護", Value = "3", Parent = "QuestionCategory"},
                new CodeValue() {Name = "院務遺失舉發", Value = "4", Parent = "QuestionCategory"},
                new CodeValue() {Name = "院務與革建議", Value = "5", Parent = "QuestionCategory"},
                //處理結果 楊金高
                new CodeValue() {Name = "已解決", Value = "1", Parent = "ProcessRlt"},
                new CodeValue() {Name = "未解決", Value = "2", Parent = "ProcessRlt"},
                new CodeValue() {Name = "處理中", Value = "3", Parent = "ProcessRlt"},
                //會議地點　楊金高
                new CodeValue() {Name = "本院會議室1", Value = "1", Parent = "MeetingAddr"},
                new CodeValue() {Name = "本院會議室2", Value = "2", Parent = "MeetingAddr"},
                new CodeValue() {Name = "本院會議室3", Value = "3", Parent = "MeetingAddr"},
                //記錄人員　楊金高
                new CodeValue() {Name = "趙子龍", Value = "1", Parent = "RecordBy"},
                new CodeValue() {Name = "楊國棟", Value = "2", Parent = "RecordBy"},
                new CodeValue() {Name = "馬國明", Value = "3", Parent = "RecordBy"},
                //會議主席　楊金高
                new CodeValue() {Name = "楊主席", Value = "1", Parent = "MeetChairman"},
                new CodeValue() {Name = "朱主席", Value = "2", Parent = "MeetChairman"},
                new CodeValue() {Name = "張跑跑", Value = "3", Parent = "MeetChairman"},
                //指導老師　楊金高
                new CodeValue() {Name = "Bella", Value = "1", Parent = "GuidTeacher"},
                new CodeValue() {Name = "Jacky", Value = "2", Parent = "GuidTeacher"},
                new CodeValue() {Name = "Dennis", Value = "3", Parent = "GuidTeacher"},
                //排泄協助　楊金高
                new CodeValue() {Name = "可自理", Value = "1", Parent = "ExcreteHelp"},
                new CodeValue() {Name = "部分協助", Value = "2", Parent = "ExcreteHelp"},
                new CodeValue() {Name = "完全協助", Value = "3", Parent = "ExcreteHelp"},//Traffic mode
                //接送方式　楊金高
                new CodeValue() {Name = "交通車", Value = "1", Parent = "TrafficMode"},
                new CodeValue() {Name = "計程車", Value = "2", Parent = "TrafficMode"},
                new CodeValue() {Name = "家屬接送", Value = "3", Parent = "TrafficMode"},//
                //上－下午活動　楊金高
                new CodeValue() {Name = "簡易體操", Value = "1", Parent = "AMActivity"},
                new CodeValue() {Name = "健口操", Value = "2", Parent = "AMActivity"},
                new CodeValue() {Name = "健康促進", Value = "3", Parent = "AMActivity"},
                new CodeValue() {Name = "益智認知", Value = "4", Parent = "AMActivity"},
                new CodeValue() {Name = "生活自立", Value = "5", Parent = "AMActivity"},
                new CodeValue() {Name = "文康休閒", Value = "6", Parent = "AMActivity"},
                new CodeValue() {Name = "醫院復健", Value = "7", Parent = "AMActivity"},
                new CodeValue() {Name = "其他", Value = "8", Parent = "AMActivity"},
                //活動參與度　楊金高
                new CodeValue() {Name = "完全參與", Value = "1", Parent = "Participate"},
                new CodeValue() {Name = "時常參與", Value = "2", Parent = "Participate"},
                new CodeValue() {Name = "偶爾參與", Value = "3", Parent = "Participate"},
                new CodeValue() {Name = "完全不參與", Value = "4", Parent = "Participate"},
                //專注力　楊金高EQUIPMENTUSE
                new CodeValue() {Name = "十分專注", Value = "1", Parent = "Concentration"},
                new CodeValue() {Name = "時常專注", Value = "2", Parent = "Concentration"},
                new CodeValue() {Name = "偶爾專注", Value = "3", Parent = "Concentration"},
                new CodeValue() {Name = "完全不專注", Value = "4", Parent = "Concentration"},
                //運動器材使用　楊金高
                new CodeValue() {Name = "腳踏車", Value = "1", Parent = "Equipmentuse"},
                new CodeValue() {Name = "電動踩踏機", Value = "2", Parent = "Equipmentuse"},
                new CodeValue() {Name = "腳底按摩器", Value = "3", Parent = "Equipmentuse"},
                new CodeValue() {Name = "手部運動機", Value = "4", Parent = "Equipmentuse"},
                new CodeValue() {Name = "其他", Value = "5", Parent = "Equipmentuse"},
                //收支别
                new CodeValue() {Name = "收入", Value = "1", Parent = "PaymentType"},
                new CodeValue() {Name = "支收", Value = "2", Parent = "PaymentType"},

                //收款方式
                new CodeValue() {Name = "现金", Value = "1", Parent = "PaymentWay"},
                new CodeValue() {Name = "支票", Value = "2", Parent = "PaymentWay"},
                new CodeValue() {Name = "汇款", Value = "3", Parent = "PaymentWay"},
                new CodeValue() {Name = "未填", Value = "4", Parent = "PaymentWay"},

                //协助输入
                new CodeValue() {Name = "一般预收款", Value = "1", Parent = "AssistInput"},
                new CodeValue() {Name = "保证金", Value = "2", Parent = "AssistInput"},
                new CodeValue() {Name = "预收款退款", Value = "3", Parent = "AssistInput"},
                
                //费用类型
                new CodeValue() {Name = "固定費用", Value = "0", Parent = "BillTypes"},
                new CodeValue() {Name = "消費費用", Value = "1", Parent = "BillTypes"},

                //支付状态
                new CodeValue() {Name = "未支付", Value = "0", Parent = "BillStatuses"},
                new CodeValue() {Name = "部分支付", Value = "1", Parent = "BillStatuses"},
                new CodeValue() {Name = "已付清", Value = "2", Parent = "BillStatuses"},

               
                 //房間類型
                new CodeValue() {Name = "普通", Value = "0", Parent = "RoomTypes"},
                new CodeValue() {Name = "高级", Value = "1", Parent = "RoomTypes"},
                new CodeValue() {Name = "豪華", Value = "2", Parent = "RoomTypes"},

                 //床位类型
                new CodeValue() {Name = "普通病床", Value = "1", Parent = "BedTypes"},
                new CodeValue() {Name = "传染病床", Value = "2", Parent = "BedTypes"},
                new CodeValue() {Name = "高干病床", Value = "3", Parent = "BedTypes"},

                 //床位类型
                new CodeValue() {Name = "普通", Value = "1", Parent = "BedKinds"},
                new CodeValue() {Name = "高級", Value = "2", Parent = "BedKinds"},
                new CodeValue() {Name = "豪華", Value = "3", Parent = "BedKinds"},
                
                //病床状态
                new CodeValue() {Name = "空床 ", Value = "E", Parent = "BedStatuses"},
                new CodeValue() {Name = "住院中", Value = "N", Parent = "BedStatuses"},
                new CodeValue() {Name = "关帐中", Value = "O", Parent = "BedStatuses"},

                //科別
                new CodeValue() {Name = "傳染科 ", Value = "001", Parent = "DeptNos"},
                new CodeValue() {Name = "高齡科", Value = "002", Parent = "DeptNos"},
                new CodeValue() {Name = "心血疾病科", Value = "003", Parent = "DeptNos"},

                //針劑種類
                new CodeValue() {Name = "H1N1流感", Value = "001", Parent = "E00"},
                new CodeValue() {Name = "肺炎疫苗", Value = "002", Parent = "E00"},
                new CodeValue() {Name = "流行性感冒疫苗", Value = "003",Parent = "E00"}
            };
            return list;
        }
    }
}