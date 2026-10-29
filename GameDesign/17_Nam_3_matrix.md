# Năm 3 — content matrix v0.1

Theme: định hướng, năng lực và tham vọng; câu hỏi: “Tôi muốn thử trở thành ai?”. Đây là bản authoring, không phải calendar/event database đã hoàn thiện. `W01–W12` là thứ tự đề xuất, không tự tạo lịch hay ngày học. Snapshot Y2 mang theo course records, debt, skills, evidence, quan hệ, conflict, memory, tiền và các dự án còn hiệu lực.

| Mốc / seed | Học tập | Long | Minh / Hạnh | An / Nam | Career | Location | Lựa chọn / đánh đổi | Outcome / callback |
|---|---|---|---|---|---|---|---|---|
| W01 `Y3_RETURN` | Xem điều kiện môn và nợ | Có lịch riêng | Lời thoại theo memory | NPC chưa gặp không tự biết | Xem evidence | H6, ROOM | Đăng ký môn / xem recovery | Nợ không biến mất; entry chỉ có khi calendar author |
| W02 `Y3_COURSE_PATH` | Môn nâng cao, prerequisite rõ | — | Hạnh office hour | — | Mở topic/skill dự kiến | CLASS, LIBRARY | Chọn môn trong lịch | Môn khác có thể chồng giờ; không khóa nghề chỉ vì một môn |
| W03 `Y3_MENTOR` | Trao đổi chủ đề đã học | — | Hạnh có thể hướng dẫn | Nam chỉ khi available | Biết project/office hour | H6, PROJECT | Hỏi advice / hỏi project / để sau | Lead không đảm bảo internship; đường self-study tồn tại nếu author |
| W04 `Y3_PROJECT_PROPOSAL` | Project có scope, deadline | Long có thể góp ý nếu rảnh | Minh tùy team | Nam review tùy lịch | Project evidence | PROJECT, ROOM | Backend / data / chọn scope khác | Không gắn career cố định; thiếu prerequisite có lý do |
| W05 `Y3_HACKATHON` | Ôn giữa kỳ có lịch | Rủ đi cùng nếu event cho phép | Minh có team riêng | An/Nam tùy gặp | Demo, kỹ năng, networking | PROJECT | Hackathon / ôn / ca làm / nghỉ | Thời gian hữu hạn; không tham gia không khóa mọi career |
| W06 `Y3_HACK_REVIEW` | Feedback theo course | Có hoặc không dựa vào event | Minh phản hồi nếu đã gặp | Nam review nếu đồng ý | Artifact completed/partial | PROJECT | Sửa artifact / giữ nguyên | Memory chỉ nếu tham dự, không tự tính thắng |
| W07 `Y3_FRIENDS_DIVERGE` | Deadline học tập | Long có kế hoạch riêng | Minh bận hồ sơ | An có hoạt động riêng | — | ROOM, CANTEEN | Gặp / hẹn lại / bỏ qua | Không ép relationship; callback dựa trên việc đã biết |
| W08 `Y3_CV` | Course evidence | — | Minh có thể review | Nam khi available | CV snapshot | CAREER | Chọn evidence thật, review hoặc tự nộp | Không biến bài tập chưa hoàn thành thành thành tích |
| W09 `Y3_FAIR` | Lịch môn hiển thị | — | — | Recruiter S07 theo lịch | Tìm JD, yêu cầu và deadline | CAREER | Thăm fair / học / làm thêm | Biết opportunity, chưa phải offer |
| W10 `Y3_APPLICATION` | — | — | Mentor nếu đã gặp | Nam không mặc định referral | Application, test, feedback | ROOM, CAREER | Nộp / cải thiện CV / bỏ qua | Rejection có lý do và đường nộp tiếp; không guarantee fail |
| W11 `Y3_FINAL` | Thi, final và môn nợ theo data | — | Hạnh theo lịch | — | Kết quả môn, evidence | CLASS | Ôn / thi / recovery | Course resolver chỉ chạy với dữ liệu đủ |
| W12 `Y3_RESOLVER` | Course snapshot | Lịch và conflict giữ | Team outcomes giữ | Memory giữ | Projects, applications, skill graph | — | Xem preview / cancel / commit | Sang Y4 nếu commit; không tự grant offer |

## Đủ 22 mục ở mức khung

1–2 Theme/câu hỏi như đầu tài liệu. 3 Calendar chưa chốt timestamp. 4 Academic: advanced course/prerequisite/final/recovery cần data. 5 Hệ mới: portfolio, CV snapshot, mentor, JD và application. 6 Location: CAREER và PROJECT khi biết; route chưa author. 7 Activity: A_RESEARCH, A_PERSONAL_PROJECT, A_HACKATHON, A_CV_REVIEW, A_APPLY và học/việc/nghỉ.

8 Long, Minh, An, Nam, Hạnh có calendar và knowledge riêng. 9 S07 recruiter, S04 classmate; chưa có bản ghi chi tiết. 10 Major seeds: project, hackathon, CV/application, final; 11 medium: review, mentor, bạn bè; 12 small: bảng tin/nhắn tin chỉ khi có nội dung thật; chưa đạt content density. 13 Academic milestones: đăng ký, midterm, final; 14 relationship: mentor và bạn bè có thể gần hoặc xa; 15 career: artifact → CV → tìm JD → nộp, có đường không tham gia hackathon.

16 Failure: missed deadline, project partial, rejection, trượt môn; 17 recovery: scope giảm/hoàn thiện sau, review CV/nộp công ty khác, học lại nếu calendar có. 18–19 Gating: prerequisite data và thời gian, không ép một nghề; mất cửa sổ không xóa skill. 20 Resolver dựa trên dữ liệu từng môn và save atomically. 21 Memory chỉ ghi sự kiện thật. 22 Carry-over toàn snapshot và application state.

## Gate chưa đạt

Chưa có course list, 6–8 career paths, skills và company data, calendar, đối thoại, project rubric, cost/recurring income và các event records. Chưa mô phỏng đường không CLB, đổi nghề, nhiều lần rejection, nợ môn hoặc đủ điều kiện Y4. Không gọi matrix này là nội dung hoàn chỉnh.
