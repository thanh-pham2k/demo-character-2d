using System;
using System.Collections.Generic;

namespace ProductionChapters.Year3
{
    // A sequence is deliberately local until the campaign step commits. Cancel/load
    // discards scratch work, never rewards or course facts.
    public sealed class Year3Activity
    {
        public string Prompt;
        public string[] Actions;
        public string[] Distractors;
        public Year3Activity(string prompt,string actions,string wrong="")
        { Prompt=prompt;Actions=actions.Split('|');Distractors=wrong.Length==0?Array.Empty<string>():wrong.Split('|'); }
    }
    public static class Year3Activities
    {
        static readonly Dictionary<string,Year3Activity> activities=new Dictionary<string,Year3Activity> {
            {"finish_call",new Year3Activity("Mẹ: Cứ từng việc một. Nhớ ăn và ngủ; mẹ vẫn nghe con.","Cảm ơn mẹ|Cúp máy")},
            {"receive_first_job",new Year3Activity("Khách phòng bên nhắn: Máy không khởi động được, em xem giúp nhé?","Đọc tin nhắn|Hẹn mang máy sang")},
            {"receive_laptop",new Year3Activity("Nhận máy và xác định phạm vi công việc.","Hỏi triệu chứng|Xin phép sao lưu và cài lại|Ghi nhận đồng ý","Cài ngay không hỏi")},
            {"backup_data",new Year3Activity("Sao lưu giả lập: không chạm dữ liệu thật.","Chọn Documents và Pictures|Chép sang ổ dự phòng|Đối chiếu checksum","Format ổ nguồn")},
            {"boot_usb",new Year3Activity("BIOS: chọn thiết bị boot an toàn.","Cắm USB cứu hộ|Mở boot menu|Chọn UEFI USB","Xóa phân vùng")},
            {"install_system",new Year3Activity("Backup đã được lưu; chỉ cài lên phân vùng hệ thống.","Chọn phân vùng System|Giữ phân vùng Data|Cài driver|Chạy kiểm tra khởi động","Xóa toàn bộ ổ đĩa")},
            {"handover_laptop",new Year3Activity("Bàn giao ca đầu; phí công 50.000đ chỉ ghi khi lưu thành công.","Mở lại dữ liệu mẫu|Khách kiểm tra máy|Nhận 50.000đ")},
            {"post_service_sign",new Year3Activity("Biển dịch vụ runtime: Sửa máy • hỏi trước khi cài • giữ dữ liệu.","Chọn vị trí cạnh bàn|Gắn góc trái|Gắn góc phải")},
            {"accept_next_jobs",new Year3Activity("Hai ca đại diện và lịch học lại cùng mở.","Đọc ca mạng|Đọc ca vệ sinh|Đặt lịch không trùng lớp")},
            {"complete_job_a",new Year3Activity("Ca 2: mất mạng. Thu công một lần sau bàn giao.","Xin phép và sao lưu cấu hình|Kiểm tra cáp|Đặt DHCP|Ping gateway|Khách nghiệm thu","Cài lại Windows ngay")},
            {"rest_and_eat",new Year3Activity("Dừng ca để ăn bánh mì và nghỉ.","Cất dụng cụ|Ăn bánh mì|Nghỉ xong mới nhận ca")},
            {"complete_job_b",new Year3Activity("Ca 3: máy nóng. Không tháo khi còn nguồn.","Xin phép và sao lưu|Tắt nguồn và rút sạc|Vệ sinh khe gió|Lắp lại và đo nhiệt|Bàn giao","Tháo khi đang chạy")},
            {"review_weekly_ledger",new Year3Activity("Montage nhiều tuần: sổ thu chi ròng 1.200.000đ; ba ca trên đã ghi riêng, không cộng lại.","Tuần 1: thu 400k, chi 100k|Tuần 2: thu 450k, chi 150k|Tuần 3: thu 500k, chi 200k|Tuần 4: thu 450k, chi 150k|Đối chiếu ròng 1.200.000đ")},
            {"pay_retaking_fee",new Year3Activity("Hóa đơn học lại 900.000đ từ ví production.","Đối chiếu mã sinh viên|Kiểm tra học phí 900.000đ|Nộp và nhận biên lai")},
            {"pass_programming",new Year3Activity("Kỳ 5 • KTLT: duyệt mảng n phần tử.","Chọn i < n|Chạy test n = 0|Đọc kết quả KTLT đạt","Chọn i <= n")},
            {"pass_physics",new Year3Activity("Kỳ 5 • VL1: đơn vị lực trong SI?","Chọn Newton|Đọc kết quả VL1 đạt","Chọn Joule")},
            {"enroll_data_structures",new Year3Activity("Kỳ 6 • KTLT đạt mới mở CTDL; HĐH còn khóa cho đến CTDL đạt.","Kiểm tra KTLT đạt|Chọn CTDL kỳ 6|Lưu lịch học")},
            {"receive_crush_laptop",new Year3Activity("Bạn nữ nhờ kiểm tra máy; cần đồng ý trước khi thao tác.","Hỏi triệu chứng|Xin phép scan và sao lưu|Nhận đồng ý")},
            {"scan_virus",new Year3Activity("Chỉ cách ly file bị phát hiện; không xóa tài liệu.","Cập nhật mẫu nhận diện|Quét toàn bộ|Cách ly trojan.exe","Xóa Documents")},
            {"repair_crush_laptop",new Year3Activity("Khôi phục máy sau scan.","Tạo điểm khôi phục|Tắt startup độc hại|Khởi động lại|Kiểm tra dữ liệu và trả máy")},
            {"waive_labor",new Year3Activity("Chỉ miễn công; không ghi thu nhập từ ca này.","Ghi công 0đ|Gửi biên nhận miễn công")},
            {"receive_cafe_invite",new Year3Activity("Tin nhắn: Ra quán giúp mình nhé, có một máy nữa cần xem.","Đọc địa điểm|Hẹn giờ gặp")},
            {"borrow_jacket",new Year3Activity("Long cho mượn áo; nhớ trả sau buổi gặp.","Hỏi Long mượn áo|Khoác áo")},
            {"arrive_cafe",new Year3Activity("Đến quán theo địa điểm trong tin nhắn.","Chọn quán trong tin|Tới bàn hẹn")},
            {"meet_boyfriend",new Year3Activity("Bạn nữ: Đây là bạn trai mình. Anh ấy cần sửa laptop. Lươn hơi khựng lại.","Chào hai người|Hỏi tình trạng máy")},
            {"repair_second_laptop",new Year3Activity("Ca tại quán vẫn cần quy trình đầy đủ.","Xin phép và sao lưu|Quét lỗi mạng|Sửa cấu hình DNS|Kiểm tra truy cập|Bàn giao","Reset xóa dữ liệu")},
            {"return_jacket",new Year3Activity("Về 404. Long nhận ra bạn đang buồn.","Cởi áo|Trả áo Long|Cảm ơn Long")},
            {"accept_lake_invite",new Year3Activity("Anh em: Đi Hồ Đá uống trà, không cần kể nếu chưa muốn.","Nhận lời|Hẹn cả nhóm")},
            {"sit_with_friends",new Year3Activity("Hồ Đá, cả bốn người ngồi ghế. Không có đếm giờ.","Chọn chỗ cạnh Long|Ngồi cùng Đạm và Tộ")},
            {"share_tea",new Year3Activity("Chia trà đào và bánh tráng cho đủ bốn người.","Đưa trà cho Long|Đưa trà cho Tộ|Đưa trà cho Đạm|Chia bánh tráng")},
            {"finish_conversation",new Year3Activity("Long: Buồn thì cứ buồn. Tộ: Có anh em đây. Đạm: Mai mình học tiếp.","Kể chuyện ở quán|Nghe các bạn|Cảm ơn anh em")},
            {"return_sleep",new Year3Activity("Cả nhóm về 404; sáng mai có thông báo điểm.","Về phòng|Cất đồ|Ngủ")},
            {"open_year3_results",new Year3Activity("Portal công bố kết quả cuối năm 3.","Đăng nhập portal|Mở bảng điểm năm 3")},
            {"pass_calculus2",new Year3Activity("GT2 đã đạt; không đăng ký thi trả nợ lại năm 5.","Mở dòng GT2|Đọc trạng thái đạt")},
            {"read_data_structures_fail",new Year3Activity("CTDL chưa đạt; HĐH tiếp tục khóa theo tiên quyết.","Mở dòng CTDL|Đọc chưa đạt|Xem HĐH bị khóa")},
            {"read_78_credits",new Year3Activity("Tổng tích lũy cuối năm: 78/150, chưa đủ điều kiện đồ án.","Đối chiếu 78 tín chỉ|Đóng bảng điểm")},
            {"receive_lab_request",new Year3Activity("Đạm: Mang giúp tao sách tới lab HCMUS nhé.","Đọc tin Đạm|Nhận mang sách")}
        };
        public static Year3Activity Get(string step) { return activities.TryGetValue(step,out var value)?value:null; }
        public static IEnumerable<string> Steps=>activities.Keys;
    }
}
