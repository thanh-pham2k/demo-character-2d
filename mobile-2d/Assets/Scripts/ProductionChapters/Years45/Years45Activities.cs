using System;
using System.Collections.Generic;

namespace ProductionChapters.Years45
{
    // Explicit authored inputs. No fallback completion for unknown events.
    public sealed class Years45Activity
    {
        public readonly int Mission;
        public readonly string Step, Prompt;
        public readonly string[] Inputs;
        public readonly bool Hold, Camera;
        public Years45Activity(int mission,string step,string prompt,string[] inputs,bool hold=false,bool camera=false)
        {Mission=mission;Step=step;Prompt=prompt;Inputs=inputs;Hold=hold;Camera=camera;}
        public bool Accept(int index,string input)=>index>=0 && index<Inputs.Length && Inputs[index]==input;
    }
    public static class Years45Activities
    {
        static readonly Dictionary<string,Years45Activity> all=new Dictionary<string,Years45Activity>();
        static void Add(int m,string s,string p,params string[] inputs){all.Add(m+":"+s,new Years45Activity(m,s,p,inputs));}
        public static IEnumerable<Years45Activity> All=>all.Values;
        public static Years45Activity Find(int m,string step){all.TryGetValue(m+":"+step,out var a);return a;}
        static Years45Activities()
        {
            Add(24,"take_book","Đạm: Mang cuốn sách mạng máy tính tới lab HCMUS giúp tao nhé.","Lấy sách mạng máy tính");
            Add(24,"travel_hcmus","Chọn điểm đến đúng trên bản đồ.","Lab HCMUS");
            Add(24,"deliver_book","Đạm đang chờ bên bàn nghiên cứu.","Đưa sách cho Đạm");
            Add(24,"listen_lab_talk","Nhóm lab đang tìm cách phát hiện thiết bị mạng hỏng. Nghe đủ ba ý.","Đọc log thiết bị","Đối chiếu sơ đồ mạng","Tự động hóa chẩn đoán");
            Add(24,"return_room","Long nhắn: Về xem CV giúp tao với.","Về phòng 404");
            Add(25,"edit_cv","CV Long: tiêu đề lệch, mục kinh nghiệm nằm cuối, ngày tháng chưa đồng nhất.","Căn trái tiêu đề","Đưa kinh nghiệm lên trước","Chuẩn hóa ngày tháng");
            Add(25,"export_cv_pdf","Xuất bản CV đã chỉnh để giữ định dạng khi gửi.","PDF","Long_CV.pdf");
            Add(25,"send_cv","Soạn thư ứng tuyển của Long. Chọn người nhận và đính kèm bản xuất.","tuyendung@congty.example","Long_CV.pdf","Gửi hồ sơ ứng tuyển");
            Add(25,"receive_job_email","Vài ngày sau — hộp thư Long có thư mới.","Mở thư mời nhận việc","Đọc ngày bắt đầu: thứ Hai");
            Add(26,"see_to_to_work","06:15 — Tộ mặc đồ đi làm, cầm cặp ra cửa.","Chúc Tộ ca sáng tốt lành");
            Add(26,"read_friend_schedules","Lịch tuần: Long làm giờ hành chính, Tộ làm ca, Đạm ở lab. Phòng vắng dần.","Long: 08:00–17:00","Tộ: 06:00–14:00","Đạm: lịch lab");
            Add(26,"receive_eligibility_notice","Thông báo khóa: mở đợt xét điều kiện đồ án. Đây là mốc cân bằng hư cấu trong game.","Đọc thông báo xét đồ án");
            Add(27,"apply_thesis","Nộp một hồ sơ xét điều kiện. Kết quả sẽ giữ nguyên tới khi có điểm mới.","Chọn đợt xét hiện tại","Nộp hồ sơ xét điều kiện");
            Add(27,"read_rejection","118/136 tín chỉ: thiếu 18. CTDL chưa đạt, HĐH bị khóa. GT2 đã đạt, không thi lại.","Đọc thiếu 18 tín chỉ","Đọc tiên quyết CTDL → HĐH");
            Add(27,"receive_photo_invite","Long: Mai chụp cho tụi tao tấm ảnh ở ĐHQG nhé?", "Nhắn: Mai tao tới");
            Add(28,"meet_graduate_friends","Sân ĐHQG — Long, Tộ, Đạm vừa xong lễ riêng. Lươn cầm máy ảnh.","Gọi ba bạn đứng cạnh nhau");
            all.Add("28:take_group_photo",new Years45Activity(28,"take_group_photo","Căn đủ ba bạn trong khung; lấy nét rồi bấm chụp.",new[]{"Lấy nét ba bạn","Chụp ảnh"},camera:true));
            Add(28,"answer_parents","Phụ huynh: Còn con khi nào tốt nghiệp?", "Dạ con đang trả các môn còn nợ, rồi làm đồ án");
            Add(28,"return_room","Cả nhóm trở về KTX. Long báo sẽ dọn đầu kỳ 9.","Về phòng 404");
            Add(29,"carry_longs_boxes","Đầu kỳ 9 — chuyển thùng sách của Long từ giường tới cửa.","Nhấc thùng Long","Đặt thùng ở cửa");
            Add(29,"say_goodbye_long","Long: Có gì cần cứ gọi tao. Ráng lên nhé.","Chào Long, cảm ơn những năm ở chung");
            Add(29,"long_leaves","Long mang thùng ra hành lang.","Tiễn Long qua cửa");
            Add(29,"freshman1_moves_in","Người mới 1 tới. Chỉ giao slot Long vừa để trống.","Giao giường cũ của Long","Đặt đồ người mới 1");
            Add(30,"receive_to_gift","Một ngày khác — Tộ đưa gói thịt khô trước khi chuyển trọ.","Nhận gói thịt khô");
            Add(30,"say_goodbye_to","Tộ: Nhớ ăn uống cho đàng hoàng đó.","Ôm chào Tộ");
            Add(30,"to_leaves","Tộ xách hành lý tới cửa.","Tiễn Tộ chuyển trọ");
            Add(30,"freshman2_moves_in","Người mới 2 nhận đúng slot Tộ, giữ nguyên đồ người mới 1.","Giao giường cũ của Tộ","Đặt đồ người mới 2");
            Add(31,"hear_dams_plan","Đạm: Tao xong thủ tục KTX rồi. Còn ở thành phố một thời gian, chưa bay đâu.","Nghe ngày rời KTX","Hẹn còn gặp trước chuyến bay");
            Add(31,"say_goodbye_dam","Đạm: Mày cứ học, tụi tao vẫn ở đây.","Chào Đạm ở cửa");
            Add(31,"dam_leaves_dorm","Đạm rời KTX, chưa rời Việt Nam.","Tiễn Đạm khỏi phòng");
            Add(31,"freshman3_moves_in","Người mới 3 nhận slot Đạm. Cả ba người mới có giường và đồ riêng.","Giao giường cũ của Đạm","Đặt đồ người mới 3");
            Add(32,"select_uninstall","Đêm — laptop vẫn còn game. Chọn ứng dụng cần gỡ.","Game","Gỡ cài đặt");
            all.Add("32:hold_uninstall",new Years45Activity(32,"hold_uninstall","Giữ nút Gỡ game liên tục 2 giây. Nhả tay hoặc Hủy sẽ xóa tiến trình giữ.",new[]{"Gỡ game"},hold:true));
            Add(32,"set_alarm","Đặt báo thức cho ngày học mới.","06","30","Lưu báo thức 06:30");
            Add(32,"write_recovery_plan","Kế hoạch trả nợ: CTDL trước, đủ tiên quyết mới học HĐH. GT2 đã đạt.","Kỳ 9: CTDL","Kỳ 10: HĐH");
            Add(33,"wake_immediately","06:30 — báo thức reo. Hôm nay dậy ngay.","Tắt báo thức và ngồi dậy","Rời giường");
            Add(33,"morning_wc","Chuẩn bị buổi sáng trước giờ học.","Đánh răng","Rửa mặt");
            Add(33,"arrive_class_on_time","06:40 — tới H6, lớp bắt đầu 06:45.","Vào lớp trước 06:45","Điểm danh");
            Add(33,"study_days","Ba ngày đại diện — ôn bài, làm bài tập, nghỉ hồi sức. Không grind lặp vô hạn.","Ngày 1: ôn CTDL","Ngày 2: làm bài tập","Ăn và nghỉ","Ngày 3: sửa bài sai");
            Add(33,"hear_knock","01:00 — đang ôn bài, nghe ba tiếng gõ cửa.","Gấp vở và nghe tiếng gọi");
            Add(34,"open_for_friends","01:00 — Long, Tộ, Đạm đứng ngoài, mang đồ ăn. Đạm chưa bay.","Mở cửa đón ba bạn");
            Add(34,"receive_supplies","Bạn mang đồ ăn và thức uống cho buổi học khuya.","Nhận đồ ăn","Đặt nước cạnh bàn");
            Add(34,"share_hotpot","Ngồi ăn cùng nhau. Đồ ba người mới vẫn giữ tại từng giường.","Ngồi bên nồi lẩu","Mời cả nhóm ăn","Ăn và trò chuyện");
            Add(34,"receive_study_notes","Đạm đưa tài liệu; Long và Tộ nhắc nghỉ ngơi.","Nhận tài liệu CTDL","Cất vào góc học");
            Add(34,"say_goodbye_visitors","Ba bạn ra về sau lần tiếp tế.","Tiễn ba bạn ra cửa");
            Add(34,"read_flight_message","Sau buổi thăm — Đạm: Tao sắp bay. Nhớ báo tin khi mày qua môn.","Mở tin nhắn chuyến bay","Nhắn chúc Đạm thượng lộ bình an");
            Add(35,"revise_data_structures","Kỳ 9 — ôn CTDL. Chọn thứ tự duyệt cây nhị phân giữa (inorder).", "Trái","Gốc","Phải");
            Add(35,"pass_data_structures","Bài thi CTDL kỳ 9: cấu trúc nào lấy phần tử vào trước ra trước?", "Hàng đợi FIFO","Nộp bài CTDL");
            Add(35,"enroll_operating_systems","Kết quả CTDL đã đạt. Chuyển kỳ 10, mở lớp HĐH.","Xem CTDL đã đạt","Đăng ký HĐH kỳ 10","Học lập lịch và đồng bộ");
            Add(35,"pass_operating_systems","Thi HĐH kỳ 10: công cụ nào bảo vệ vùng găng để tránh race condition?", "Mutex","Nộp bài HĐH");
            Add(35,"read_debt_cleared","Portal: CTDL đạt kỳ 9, HĐH đạt kỳ 10. GT2 và các môn trước giữ nguyên.","Xem CTDL đạt","Xem HĐH đạt","Xem không còn nợ tiên quyết");
            Add(36,"open_eligibility_portal","Trở về 404 — kết quả mới đã đồng bộ, mở đợt xét lại.","Mở portal xét điều kiện");
            Add(36,"confirm_136_credits","Đối chiếu: đủ 136/150; GT2, VL1, KTLT, CTDL, HĐH đều đạt. Đủ điều kiện đồ án, chưa tốt nghiệp.","Kiểm tra 5 môn đã đạt","Xác nhận 136 tín chỉ");
            Add(36,"receive_advisor_appointment","Lịch hẹn GVHD đầu năm 6 và form đề tài đã mở.","Nhận lịch hẹn GVHD","Lưu form đăng ký đề tài");
        }
        public static string Occupant(IProductionCampaignView p,int slot)
        {
            string[] old={"Long","Tộ","Đạm"};string[] left={"long","to","dam"};
            if(slot<0||slot>2)throw new ArgumentOutOfRangeException(nameof(slot));
            return p.Has("room:freshman"+(slot+1))?"Người mới "+(slot+1):p.Has("room:"+left[slot]+"_left")?"Trống":old[slot];
        }
        public static bool Gate(IProductionCampaignView p,int mission,string step,out string error)
        {
            error="";
            if(p.CurrentMission!=mission || p.CurrentStep==null || p.CurrentStep.id!=step){error="Nhiệm vụ đã thay đổi.";return false;}
            if(Find(mission,step)==null){error="Event chưa được author.";return false;}
            if(mission==35 && step=="enroll_operating_systems" && !p.Has("passed:CTDL")){error="Cần đạt CTDL trước HĐH.";return false;}
            if(mission==36 && step=="confirm_136_credits")
                foreach(string course in new[]{"GT2","VL1","KTLT","CTDL","HDH"})
                    if(!p.Has("passed:"+course)){error="Chưa đạt "+course;return false;}
            return true;
        }
    }
}
