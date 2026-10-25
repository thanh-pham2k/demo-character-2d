using System;
using System.Collections.Generic;

namespace ProductionChapters.Year6
{
    // Executable toy network, not a claim to run external commands or contact real devices.
    // The player changes configuration; regressions exercise that configuration.
    public sealed class Year6Workbench
    {
        readonly Dictionary<string,string> edges=new Dictionary<string,string>();
        public bool Tls,PrivateAdmin,Parser,InputCheck,BoundedRetry,Alerts,Token,Permissions,SecretOutsideSource,NullGuard,DisconnectedState,CancelOldRequest;
        public int Pages {get;private set;}
        public bool HeadingStyles,Contents,FigureSources;
        public void Apply(string op)
        {
            switch(op)
            {
                case "Sensor → switch":edges["sensor"]="switch";break;
                case "Switch → diagnostic":edges["switch"]="diagnostic";break;
                case "Diagnostic → dashboard":edges["diagnostic"]="dashboard";break;
                case "TLS :443":Tls=true;break;
                case "SSH :22 quản trị":PrivateAdmin=true;break;
                case "Parse telemetry":Parser=true;break;
                case "Kiểm tra đầu vào":case "Validate payload":InputCheck=true;break;
                case "Timeout và retry có giới hạn":BoundedRetry=true;break;
                case "Phát cảnh báo":Alerts=true;break;
                case "Xác thực token":Token=true;break;
                case "Kiểm tra quyền":Permissions=true;break;
                case "Đưa secret khỏi source":SecretOutsideSource=true;break;
                case "Kiểm tra snapshot null":NullGuard=true;break;
                case "Trả trạng thái disconnected":DisconnectedState=true;break;
                case "Hủy request cũ an toàn":CancelOldRequest=true;break;
                case "Áp heading styles":HeadingStyles=true;break;
                case "Cập nhật mục lục":Contents=HeadingStyles;break;
                case "Đánh số hình và nguồn":FigureSources=true;break;
                case "Đánh số đủ 120 trang":Pages=120;break;
            }
        }
        public bool ValidateGraph()
        {
            foreach(var origin in new[]{"sensor","switch","diagnostic"})
            {
                var seen=new HashSet<string>();string node=origin;
                while(node!="dashboard")
                {
                    if(!seen.Add(node) || !edges.TryGetValue(node,out node))return false;
                }
            }
            return Tls && PrivateAdmin;
        }
        public int Request(bool validToken,bool allowed,bool validPayload)
        {
            if(Token && !validToken)return 401;
            if(Permissions && !allowed)return 403;
            if(InputCheck && !validPayload)return 400;
            return 200;
        }
        public string ReadSnapshot(bool connected)
        {
            if(connected)return Parser?"telemetry":"parse-error";
            return NullGuard && DisconnectedState && CancelOldRequest?"disconnected":"NullReference:DeviceSnapshot.Read";
        }
        public bool ProjectTests()=>Parser && InputCheck && BoundedRetry && Alerts;
        public bool SecurityTests()=>SecretOutsideSource && Request(false,true,true)==401 && Request(true,false,true)==403 && Request(true,true,false)==400 && Request(true,true,true)==200;
        public bool FinalTests()=>ProjectTests() && SecurityTests() && ReadSnapshot(false)=="disconnected" && ReadSnapshot(true)=="telemetry";
        public bool ReportValid()=>HeadingStyles && Contents && FigureSources && Pages==120;
        public void Restore(IProductionCampaignView progress)
        {
            // Reconstruct local work only from durable event receipts. No second wallet/session/save.
            foreach(var e in progress.ExportEvents())
            {
                if(e.assistance || e.mission<37 || e.mission>45)continue;
                var a=Year6Activities.Find(e.step);if(a==null)continue;
                foreach(var op in a.Required)Apply(op);
            }
        }
        public bool Validate(string step,out string error)
        {
            bool valid=true;
            switch(step)
            {
                case "assemble_network_design":case "validate_design":valid=ValidateGraph();break;
                case "implement_modules":case "run_project_tests":case "run_first_demo":valid=ProjectTests();break;
                case "patch_security":case "pass_security_tests":case "pass_demo2":valid=SecurityTests();break;
                case "format_report":case "print_report":case "bind_thesis":valid=ReportValid();break;
                case "reproduce_crash":valid=ReadSnapshot(false).StartsWith("NullReference",StringComparison.Ordinal);break;
                case "patch_final_bug":case "pass_final_tests":case "submit_project":valid=FinalTests();break;
            }
            error=valid?"":"Mô hình kiểm thử chưa đạt; tập dượt lại thao tác hiện tại.";return valid;
        }
    }
}
