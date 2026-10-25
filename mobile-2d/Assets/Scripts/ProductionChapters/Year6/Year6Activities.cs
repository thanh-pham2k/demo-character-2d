using System;
using System.Collections.Generic;

namespace ProductionChapters.Year6
{
    // Every sequence represents domain work, not a generic mission confirmation.
    // A wrong operation resets the local activity, never rewinds durable campaign events.
    public sealed class Year6Activity
    {
        public readonly string Step, Prompt;
        public readonly string[] Operations, Required;
        public readonly float Seconds;
        public Year6Activity(string step,string prompt,string[] operations,string[] required,float seconds=0)
        {Step=step;Prompt=prompt;Operations=operations;Required=required;Seconds=seconds;}
    }
    public sealed class Year6Attempt
    {
        public int Position {get;private set;}
        public int Confidence {get;private set;}=3;
        public bool Passed {get;private set;}
        public string Feedback {get;private set;}="";
        public void Reset(){Position=0;Confidence=3;Passed=false;Feedback="";}
        public bool Choose(Year6Activity activity,string operation)
        {
            if(activity==null || Passed)return false;
            if(Array.IndexOf(activity.Operations,operation)<0)return false;
            if(operation!=activity.Required[Position])
            {
                Position=0;Confidence=Math.Max(0,Confidence-1);
                Feedback=Confidence==0?"Dừng tại checkpoint. Chọn tập dượt rồi thử lại.":"Chưa đúng: xem lại quy trình và bắt đầu lại thao tác này.";
                return false;
            }
            if(Confidence==0)return false;
            Position++;Passed=Position==activity.Required.Length;
            Feedback=Passed?"Thao tác đạt. Đang lưu checkpoint…":"Đúng. Tiếp tục bước tiếp theo.";
            return Passed;
        }
    }
    public static class Year6Activities
    {
        static string[] Split(string value)=>value.Split('|');
        static Year6Activity A(string id,string prompt,string required,string distractors="",float seconds=0)
        {
            var expected=Split(required);var options=new List<string>(expected);
            if(distractors.Length>0)options.AddRange(Split(distractors));
            // Stable shuffle keeps the correct sequence from being a top-to-bottom checklist.
            for(int i=0;i<options.Count;i++){int j=(i*7+3)%options.Count;var t=options[i];options[i]=options[j];options[j]=t;}
            return new Year6Activity(id,prompt,options.ToArray(),expected,seconds);
        }
        public static readonly Year6Activity[] All={
            A("meet_advisor","GVHD: Em muốn giải quyết lỗi gì? Mở lịch rồi gặp thầy đúng hẹn.","Mở lịch hẹn|Chào GVHD|Trình bày lỗi thiết bị mạng","Bỏ hẹn"),
            A("choose_network_topic","Chọn một đề tài xuyên suốt: tự động hóa và chẩn đoán lỗi thiết bị mạng.","Tự động hóa thiết bị mạng|Chẩn đoán lỗi và cảnh báo","Game online|Nhận diện khuôn mặt"),
            A("register_topic","Điền hồ sơ: mục tiêu → phạm vi → người hướng dẫn.","Mục tiêu chẩn đoán|Phạm vi mạng thử nghiệm|Gắn GVHD|Gửi đăng ký","Đổi đề tài"),
            A("receive_research_checklist","Thầy giao checklist nghiên cứu trước khi viết code.","Đọc yêu cầu|Ghi nguồn tài liệu|Ghi sơ đồ và kiểm thử","Bỏ checklist"),
            A("search_references","Thư viện: chọn tài liệu có tác giả, nguồn chính thức và phiên bản.","Tài liệu giao thức chính thức|Ghi tác giả và phiên bản|Lưu trích dẫn","Copy bài không nguồn"),
            A("assemble_network_design","Ghép graph: sensor → switch → diagnostic service → dashboard. Cổng dịch vụ TLS 443; SSH 22 chỉ quản trị.","Sensor → switch|Switch → diagnostic|Diagnostic → dashboard|TLS :443|SSH :22 quản trị","Dashboard → sensor|Telnet :23 công khai"),
            A("validate_design","Kiểm tra graph/cổng: không vòng lặp, mọi node tới dashboard, port quản trị tách biệt.","Kiểm tra đường đi|Kiểm tra không có vòng|Chặn cổng quản trị công khai|Ping và TLS handshake","Mở mọi cổng"),
            A("write_backlog","Tách backlog có phụ thuộc: thu thập → chẩn đoán → cảnh báo → UI.","Thu thập telemetry|Bộ chẩn đoán|Cảnh báo|Dashboard","UI trước dữ liệu"),
            A("implement_modules","Sửa module: parse telemetry rồi xử lý timeout và phát hiện lỗi.","Parse telemetry|Kiểm tra đầu vào|Timeout và retry có giới hạn|Phát cảnh báo","Retry vô hạn"),
            A("run_project_tests","Chạy test với ca bình thường, thiết bị mất kết nối và dữ liệu sai.","Test thiết bị online|Test mất kết nối|Test payload sai|Đối chiếu kết quả","Bỏ test lỗi"),
            A("run_first_demo","Demo đầu: bật thiết bị, gây lỗi dây mạng, xem cảnh báo và khôi phục.","Bật mô hình|Rút dây mô phỏng|Quan sát cảnh báo|Cắm lại và phục hồi","Chỉ mở slide"),
            A("book_demo1","Chọn lịch demo sau khi có bản chạy và test log.","Đính kèm test log|Chọn lịch GVHD|Gửi lịch demo 1","Gửi bản chưa test"),
            A("present_demo1","Trước GVHD: chạy mô hình thật, ngắt thiết bị để chứng minh chẩn đoán.","Kết nối mô hình|Chạy telemetry|Mô phỏng mất mạng|Mở log chẩn đoán","Chỉ đọc slide"),
            A("record_security_feedback","GVHD phát hiện API chưa xác thực và input chưa kiểm tra. Ghi đủ hai vấn đề.","Ghi thiếu xác thực|Ghi input không tin cậy|Thêm acceptance tests","Ẩn cảnh báo"),
            A("patch_security","Sửa API: kiểm tra danh tính, quyền, dữ liệu và giấu secret.","Xác thực token|Kiểm tra quyền|Validate payload|Đưa secret khỏi source","Hardcode mật khẩu|Tắt kiểm tra TLS"),
            A("pass_security_tests","Regression: từ chối truy cập sai, payload lỗi; yêu cầu hợp lệ vẫn chạy.","Token sai → 401|Sai quyền → 403|Payload lỗi → 400|Yêu cầu hợp lệ → 200","Mọi request → 200"),
            A("book_demo2","Gửi lịch demo 2 kèm bằng chứng vá lỗi.","Đính kèm diff bảo mật|Đính kèm regression log|Đặt demo 2","Gửi log demo cũ"),
            A("pass_demo2","GVHD thử cả request sai và đúng trước khi duyệt.","Chạy request sai|Chứng minh bị từ chối|Chạy request đúng|GVHD duyệt demo 2","Bỏ test quyền"),
            A("format_report","Báo cáo 120 trang: style tiêu đề, mục lục, hình, trích dẫn, số trang.","Áp heading styles|Cập nhật mục lục|Đánh số hình và nguồn|Đánh số đủ 120 trang|Xuất PDF","Gõ mục lục bằng tay"),
            A("print_report","Tiệm in: kiểm preview 120 trang, lề gáy và thứ tự trước khi in.","Mở PDF preview|Kiểm 120 trang|Chọn lề gáy|In đủ bộ","In thiếu phụ lục"),
            A("bind_thesis","Xếp ruột báo cáo đúng chiều rồi đóng bìa.","Xếp trang 1–120|Thêm bìa đề tài mạng|Đóng gáy|Kiểm trang không rơi","Trộn thứ tự trang"),
            A("review_submission_deadline","Lịch: bắt đầu kiểm thử lúc 22:45; cổng nộp đóng 23:45.","Đọc hạn 23:45|Ghi lịch kiểm thử 22:45|Chuẩn bị file và backup","Để qua nửa đêm"),
            A("reproduce_crash","22:45. Tái hiện crash khi thiết bị ngắt kết nối giữa lúc đọc telemetry.","Chạy bản release|Ngắt thiết bị giữa lượt đọc|Thu crash trace","Khởi động lại rồi bỏ qua"),
            A("read_crash_log","Log: NullReference tại DeviceSnapshot.Read sau disconnect. Xác định nguyên nhân.","Mở stack trace|Tìm DeviceSnapshot.Read|Đối chiếu disconnect|Xác định snapshot null","Đổ lỗi máy in"),
            A("patch_final_bug","Vá đúng lỗi null khi disconnect, không che mọi exception.","Kiểm tra snapshot null|Trả trạng thái disconnected|Hủy request cũ an toàn","Catch rồi bỏ qua mọi lỗi"),
            A("pass_final_tests","Test lại bản vá trên cùng ca crash và các ca cũ.","Chạy disconnect regression|Chạy reconnect|Chạy security suite|Build bản nộp","Chỉ test happy path"),
            A("submit_project","23:44:32. Chọn đúng bản đã test, checksum và báo cáo đóng bìa trước hạn 23:45.","Chọn release đã test|Đối chiếu checksum|Đính kèm báo cáo|Upload và nhận receipt","Upload bản demo 1"),
            A("save_receipt_backup","Giữ receipt, checksum và bản nộp ở hai nơi.","Tải receipt|Lưu release cùng checksum|Sao lưu USB|Đối chiếu backup","Xóa bản gốc"),
            A("receive_defense_schedule","Lịch hội đồng gồm GVHD và bốn giám khảo; chuẩn bị phần trình bày.","Đọc ngày và phòng|Ghi hội đồng 5 người|Chuẩn bị slide và bản dự phòng","Bỏ lịch"),
            A("change_to_shirt","Ngày bảo vệ: lấy sơ mi sạch và mang đủ báo cáo, laptop, bản dự phòng.","Mặc sơ mi|Mang báo cáo|Mang laptop và USB","Mặc áo ngủ"),
            A("connect_projector","Cắm máy chiếu, chọn đầu vào và kiểm tra slide trước khi bắt đầu.","Cắm cáp|Chọn đúng input|Kiểm slide lên màn|Kiểm bản demo offline","Rút cáp"),
            A("present_thesis","Trình bày theo mạch vấn đề → thiết kế → demo → giới hạn.","Nêu vấn đề|Giải thích graph|Demo gây lỗi và phục hồi|Nêu giới hạn","Giấu giới hạn"),
            A("pass_defense_qa","Hội đồng 5 người hỏi lần lượt. Sai làm giảm tự tin; hết tự tin được tập dượt lại tại checkpoint.","GVHD: một đề tài xuyên suốt|Chủ tịch: chẩn đoán từ telemetry|Giám khảo A: timeout có giới hạn|Giám khảo B: token và phân quyền|Giám khảo C: disconnect regression","Retry vô hạn|Mở mọi cổng|Không cần test|Đổi đề tài"),
            A("receive_grade_a","Chỉ sau phản biện đạt: nghe hội đồng công bố và nhận biên bản A, đủ 150 tín chỉ.","Nghe nhận xét hội đồng|Đọc biên bản A|Nhận kết quả","Tự chấm A"),
            A("call_mother","Mẹ ơi, con bảo vệ đạt rồi. Hẹn gia đình ngày nhận bằng.","Gọi mẹ|Kể kết quả bảo vệ|Hẹn ngày lễ|Kết thúc cuộc gọi","Tắt máy giữa cuộc gọi"),
            A("pack_own_belongings","Chỉ dọn góc Lươn. Giữ nguyên đồ của ba người mới.","Laptop của Lươn → balo|Sách của Lươn → thùng|Tua vít của Lươn → balo|Chăn của Lươn → túi","Chăn người mới 1 → thùng|Sách người mới 2 → thùng|Đồ người mới 3 → balo"),
            A("turn_off_light","Đồ riêng đã gói; chạm công tắc trước khi rời phòng.","Tới công tắc|Tắt đèn","Bật đèn"),
            A("close_room_door","Nhìn lại 404 rồi khép cửa, vẫn giữ nguyên góc ba người mới.","Nhìn lại phòng|Ra hành lang|Khép cửa","Dọn giường người mới"),
            A("return_key","Tới quầy quản lý, trả chìa khóa 404 và nhận biên nhận.","Tới quầy quản lý|Trao khóa 404|Nhận biên nhận","Giữ chìa khóa"),
            A("receive_ceremony_invitation","Thư mời A5 chuyển câu chuyện đến ngày lễ; chưa kết thúc game.","Mở thư mời A5|Đọc ngày lễ|Chuẩn bị áo tốt nghiệp","Bỏ qua ngày lễ"),
            A("receive_degree","Sảnh A5: tới lượt tên mình, bước lên sân khấu và nhận bằng.","Nghe gọi tên|Bước lên sân khấu|Nhận bằng|Chào và xuống sân khấu","Nhận bằng trước lượt"),
            A("meet_family","Tìm ba mẹ ở sảnh, trao bó hoa và chụp ảnh cùng gia đình.","Gặp mẹ|Gặp cha|Trao hoa|Chụp ảnh gia đình","Rời sảnh một mình"),
            A("read_dam_return_message","Điện thoại: Đạm đã về. Anh em hẹn gặp sau lễ.","Mở tin Đạm|Đọc đã về Việt Nam|Trả lời điểm hẹn","Xóa tin chưa đọc"),
            A("reunite_friends","Gặp đủ Đạm, Long, Tộ sau nhiều năm.","Chào Đạm|Chào Long|Chào Tộ|Hẹn Bình Đông","Bỏ cuộc hẹn"),
            A("travel_binh_dong","Đổi đồ sau lễ và đi cùng nhóm tới Bình Đông.","Cất áo tốt nghiệp|Đi cùng anh em|Tới Bình Đông","Ở lại A5"),
            A("link_shoulders","Đứng đúng thứ tự Lươn–Đạm–Long–Tộ, ghép tay lên vai rồi đi cùng nhau.","Lươn cạnh Đạm|Đạm cạnh Long|Long cạnh Tộ|Khoác vai|Đi cùng anh em","Đi một mình"),
            A("finish_credits","Phòng 404 — Những năm tháng sinh viên\nCảm ơn gia đình, thầy cô và anh em.\nMột câu chuyện hư cấu về tình bạn và trưởng thành.","Xem credits", "",18)
        };
        public static Year6Activity Find(string id)=>Array.Find(All,a=>a.Step==id);
        // Authored simulation time derived from committed events; reload cannot reset it.
        public static int SubmissionSeconds(IProductionCampaignView progress)
        {
            int elapsed=0;
            foreach(var e in progress.ExportEvents())if(e.mission==42 && !e.assistance)
            {
                switch(e.step){case "reproduce_crash":elapsed=900;break;case "read_crash_log":elapsed=1800;break;case "patch_final_bug":elapsed=3000;break;case "pass_final_tests":elapsed=3572;break;case "submit_project":elapsed=3572;break;}
            }
            return elapsed;
        }
    }
}
