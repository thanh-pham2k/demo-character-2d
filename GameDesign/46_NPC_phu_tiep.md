# NPC phụ — bổ sung bốn event fixture v0.1

Đề xuất hư cấu, chưa có runtime, calendar hoặc playtest. Gắn ID S04–S08 trong `07`; NPC chỉ biết sự kiện họ tham gia/chứng kiến/được kể. Không cộng relationship hay memory ngoài effects ghi dưới đây. Cost chỉ là fixture; dùng transaction/idempotency tại `43`.

## S04 — Bạn cùng lớp: phân chia nhiệm vụ

`EV_S04_PROJECT`: medium, Y2 W07 `[10:00,17:00)`, LOC_PROJECT; required project team gồm S04 và player, deadline còn hiệu lực, S04 available, một milestone player hoàn tất và partner chưa đóng góp, chưa resolve event. Không trigger cho team Minh hay solo. Mục tiêu: chia scope trước deadline, không gán S04 lười vì không biết lý do.

S04: “Tớ bị trùng lịch, chưa làm được phần này. Mình chia lại cho kịp không?”
- `redistribute`: 30 phút E−5; nếu S04 xác nhận lịch W08 10:00, tạo task reservation. Tới mốc, chỉ commit một milestone khi S04 available và task thực hoàn tất; nếu bận, gửi notice, không tự cấp.
- `do_extra`: 90 phút E−15, player hoàn thành một milestone nếu đủ thời gian. S04 không tự biết player làm thêm khi vắng.
- `discuss_later`: 0 phút, không đổi scope. Nếu hết cửa sổ, project giữ deadline/tiến độ, không tự sinh conflict.

Player có thể chọn lời cáo buộc riêng khi event cho phép: ghi `conflict_s04_project=open`, resentment +3 một lần; trao đổi lịch bình thường không bị phạt. Memory chỉ sinh sau dialogue commit. Chưa có lịch S04 và rubric project chính thức.

## S06 — Trưởng CLB: phiên đầu

`EV_S06_WELCOME`: medium, Y2 W03 `[18:00,20:00)`, LOC_CLUB; player chọn coding club ở fair, S06 available. Nếu không tham gia, không trigger; tự học và các nghề vẫn giữ lối riêng.

S06: “Phiên đầu mình xem một project nhỏ. Cậu muốn làm hay chỉ quan sát?”
- `participate`: 60 phút E−10, khi hoàn thành +10 S_BASIC theo skill ledger nếu session được author; tạo memory `club_first` với actor thật. Không cộng thêm C_NMT topic hay credit.
- `observe`: 30 phút E−4, biết lịch sinh hoạt tiếp nếu calendar author; không tự tăng skill, không tự gặp Nam.
- `leave`: 0 phút, không hủy membership; nếu miss phiên, không trừ trust hay tự rút khỏi CLB.

Nhiều phiên cần dữ liệu riêng; event này không author cho CLB thể thao/nhạc/tình nguyện. Save/reload sau completion không cộng skill hai lần. Nếu S06 vắng, event unavailable, không tự phát lời thoại.

## S07 — Người tuyển dụng: đọc điều kiện tuyển

`EV_S07_FAIR`: medium, Y3 W09 `[09:00,17:00)`, LOC_CAREER, S07 available, hội nghề active. Player chưa có JD nào vẫn có thể hỏi thông tin.

S07: “Bên mình xem kỹ project ứng viên đã thực sự làm. Đây là yêu cầu của đợt này.”
- `read_jd`: 10 phút E−1, khi hoàn tất ghi `JD_A known` nếu công ty có data hợp lệ; UI hiển thị deadline và yêu cầu công khai. Không nộp CV, không tạo invitation.
- `ask_scope`: 15 phút E−2, S07 giải thích full/partial theo rubric JD đã author; nếu rubric thiếu, nói chưa có thông tin, không tự kết luận.
- `leave`: 0 phút, JD chỉ biết nếu đã hoàn thành read hoặc từ nguồn hợp lệ khác.

S07 không biết private memory, không cam kết offer. Hết fair, board có thể cho biết JD nếu calendar board author; không tự mở JD ngoài window. Các câu test/interview dùng `33`, không đổi snapshot đã nộp.

## S08 — Đồng nghiệp thực tập: nhờ review

`EV_S08_REVIEW`: conditional, Y4 W05 `[14:00,17:00)`, LOC_WORK, active external internship contract, task artifact tồn tại, S08 available. Không xuất hiện trong campus practical hoặc khi chưa có contract.

S08: “Đưa tớ xem đoạn cậu đang mắc nhé. Mình có thể kiểm tra cùng.”
- `ask_review`: 30 phút E−4; khi hoàn tất tạo `peer_feedback` gắn artifact version, không tự đóng BUG_A hoặc tăng điểm course.
- `investigate_alone`: 60 phút E−10 theo `EV_Y4_BUG`; chỉ mở nếu bug active, tạo note sau completion.
- `later`: 0 phút, không phạt quan hệ; S08 hết ca thì review không có sẵn, player có thể tự làm nếu event author.

S08 biết feedback và bug nếu cùng xem, không biết điểm học/câu chuyện Long. Review không tự thành mentor assessment C_INTERN. Chưa author dialogue khi review bị ngắt, lịch S08 hoặc rubric chất lượng.

## QA / giới hạn

Chưa có ngày tuyệt đối, actor calendars, project rubric, dữ liệu CLB khác, route hoặc schema runtime. Cần kiểm tra S04 ≠ Minh, solo không trigger, skill ledger một lần, fair chưa có JD, S08 không có contract, save/retry và private knowledge. Đây là bốn event riêng, chưa thay thế hồ sơ và arc đầy đủ cho từng NPC; chưa mô phỏng/playtest.
