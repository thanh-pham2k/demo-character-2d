# T-001 — Hoàn thiện flow gameplay Năm 1

## TLDR

Hoàn thiện campaign Năm 1 trong `Room404_MVP` theo **12 mốc gameplay**: 10 mốc chính hiện có, giữ Day 29 là ngày học, thêm Day 28 ôn giữa kỳ và Day 30 thi giữa kỳ. Mỗi ngày có routine, lịch và lựa chọn cụ thể; time-skip chỉ xử lý event đã author, preview hậu quả, cho hủy trước commit và lưu state đúng một lần.

**Đang tiếp tục triển khai và đối chiếu flow; chưa nghiệm thu thủ công.** Lỗi trong ảnh nhận phòng đã được xử lý tại reference scene, scene builder và khởi tạo quầy; các bước bị gộp đã được tách trong code. Đối chiếu từng mốc và giới hạn kiểm chứng: [báo cáo T-001](../../YEAR_ONE_T001_IMPLEMENTATION.md). Hướng dẫn: [checkpoint F8](../../MANUAL_CHECKPOINTS.md). Các ô DoD dưới đây chưa nghiệm thu; source review không chứng minh gameplay/UI/animation đã đúng.

## Sơ đồ ASCII

```text
[D0] DAY 0 • 14:20 — YEAR1_ARRIVAL
  │
  ▼
Tới KTX → tới quầy → nộp thủ tục → nhận chìa khóa/phòng 404
  │
  ▼
Đi tới phòng → mở cửa → bước vào → nhìn phòng → chọn giường
  │
  ├─ TẦNG TRÊN ──► tới thang → leo lên → kiểm tra chỗ nằm ──┐
  │                                                       │
  └─ TẦNG DƯỚI ──► đi tới giường → nằm thử, không leo ──────┤
                                                          │
  ◄───────────────────────────────────────────────────────┘
  │
  ▼
Ghi lựa chọn giường → đặt balo → dọn đồ → làm quen roommate
  │
  ▼
Xuống căn tin → xem món/giá → chọn trong ngân sách → ăn bữa đầu
  │
  ▼
Về phòng 404
  │
  ▼
[D0-FREE] Thời gian tự do
  ├─ Nói chuyện → phản ứng roommate → trở lại [D0-FREE]
  ├─ Xem phòng / vật dụng → trở lại [D0-FREE]
  ├─ Nghỉ → quay lại [D0-FREE]
  └─ Lên giường (tầng trên: leo thang)
       ├─ Chỉ nằm nghỉ → trở lại [D0-FREE]
       └─ Ngủ → preview Day 1 • 06:40
            ├─ Hủy → trở lại [D0-FREE]
            └─ Xác nhận → xử lý hậu quả trên snapshot
                 ├─ Save lỗi → giữ mốc/state nguồn → [D0-FREE]
                 └─ Save thành công → công bố state mới
  │
  ▼
[D1] DAY 1 • 06:40 — HK1_FIRST_CLASSES
  │
  ▼
Báo thức → thức dậy → ra giường (giường trên: xuống thang)
  │
  ▼
Đi WC → đánh răng → rửa mặt → thay đồ → mở laptop/xem lịch
  │
  ▼
Xác nhận 08:00 GT1 / 10:00 VL1 → lấy đúng sách → đeo balo
  │
  ▼
Ra hành lang → xuống cầu thang → ra KTX → đi trạm bus
  │
  ▼
Lên bus → tới H6 → tìm lớp → vào giảng đường
  │
  ▼
08:00–10:00 • GIẢI TÍCH 1
  │ Ngồi vào chỗ → điểm danh → nghe/ghi chép → hoạt động học
  │ Ghi attendance và nội dung đã học; kết thúc tiết
  ▼
Chuyển tới lớp tiếp theo
  │
  ▼
10:00–12:00 • VẬT LÝ 1
  │ Ngồi vào chỗ → điểm danh → nghe/ghi chép → hoạt động học
  ▼
Tan học → rời lớp → đi trạm → bus về KTX → lên phòng → cất balo
  │
  ▼
[D1-FREE] Chọn hoạt động sau giờ học
  ├─ Làm bài → chọn môn → làm/nộp → cập nhật assignment/topics ─┐
  ├─ Laptop / nói chuyện / nghỉ → thời gian và story phản ứng ─┤
  │                                                          │
  │  ◄──────────────────── tiếp tục chọn hoạt động ────────────┘
  └─ Lên giường → ngủ → preview Day 2 • 06:40 + việc bị lỡ
       ├─ Hủy / save lỗi → ở [D1-FREE]
       └─ Commit/save thành công → summary/checkpoint
  │
  ▼
[D2] DAY 2 • 06:40 — HK1_GAMING_CHOICE
  │
  ▼
Thức dậy → ra giường → WC/đánh răng/rửa mặt → thay đồ
  │
  ▼
Xem lịch: 08:00–10:00 NMT / 10:00–12:00 GT1
  │
  ▼
Long rủ chơi TRƯỚC GIỜ HỌC → xem trade-off thời gian/attendance
  │
  ├─ ĐI HỌC ──► lấy sách → đeo balo → ra KTX → bus/H6 ───────┐
  │                                                         │
  └─ CHƠI ──► mở laptop → chọn game → chơi với Long           │
                    │                                       │
                    ▼                                       │
              [D2-GAME] Giờ tiếp tục chạy                     │
                    │                                       │
                    ▼                                       │
              Cảnh báo lớp T-30 → T-15 → T-5                 │
                    │ Hiện giờ lớp và thời gian nên xuất phát│
                    ▼                                       │
              Đi học hay chơi tiếp?                         │
                ├─ Chơi tiếp → [D2-GAME]                     │
                └─ Đi học → dừng game → sách/balo → bus/H6 ──┤
                                                            │
  ◄─────────────────────────────────────────────────────────┘
  │
  ▼
[D2-CLASS] Xử lý từng buổi THEO GIỜ THỰC TẾ ĐẾN LỚP
  │  Buổi 1: 08:00–10:00 NMT; buổi 2: 10:00–12:00 GT1
  ├─ Tới kịp / còn trong tiết
  │    └─ Ngồi → điểm danh đúng giờ/trễ → hoạt động học
  │         → ghi attendance và phần đã học ─────────────────┐
  └─ Đã hết khung giờ                                       │
       └─ Ghi vắng cho buổi đã khai báo, không tự cấp bài học ┤
                                                            │
  ◄─────────────────────────────────────────────────────────┘
  │ Còn buổi hôm nay? → Có: trở lại [D2-CLASS] cho buổi kế
  │                   Không / hết giờ học
  ▼
Về KTX (hoặc vẫn ở KTX nếu bỏ cả buổi) → cất balo → buổi tối
  │
  ▼
Long rủ tiếp → xem thời gian còn lại và việc học chưa làm
  │
  ├─ HỌC SỚM
  │    └─ Mở tài liệu → chọn môn/topic → làm bài
  │         → feedback → completedTopics tăng / gap giảm ────┐
  │                                                         │
  └─ CHƠI VỚI LONG                                          │
       └─ Mở laptop → chọn game → chơi/trò chuyện             │
            → thời gian trôi + relationship/event được ghi   │
            → phần chưa học vẫn cần bù ──────────────────────┤
                                                            │
  ◄─────────────────────────────────────────────────────────┘
  │
  ▼
Ghi attendance + assignment + topics + sự kiện với Long
  │
  ▼
Lên giường → ngủ → PREVIEW tới Day 28
  │ Hiện source/target/duration, deadline và event đã khai báo
  │ Khoảng trống được montage; không tự sinh lớp/vắng ngoài lịch
  ├─ Hủy / save lỗi → tiếp tục hoạt động Day 2
  └─ Commit/save thành công → summary/checkpoint
  │
  ▼
[D28] DAY 28 • 07:00 — HK1_MIDTERM_PREP
  │
  ▼
Thức dậy → ra giường → vệ sinh → thay đồ → mở midterm dashboard
  │
  ▼
Xem GT1 / VL1 / NMT: attendance, topics đã học và topics còn yếu
  │
  ├─ Đã học sớm → ít gap hơn → khối lượng ôn nhẹ hơn ─────────┐
  └─ Đã chơi nhiều / chưa học → nhiều gap → cần thêm thời gian┤
                                                            │
  ◄─────────────────────────────────────────────────────────┘
  │
  ▼
[D28-STUDY] Chọn môn → chọn topic → xem thời lượng → study activity
  │
  ▼
Feedback
  ├─ Topic đã ổn → ghi completedTopics / giảm phần yếu ───────┐
  └─ Topic còn yếu → ghi weakTopics / chỉ rõ cần ôn gì ───────┤
                                                            │
  ◄─────────────────────────────────────────────────────────┘
  │
  ▼
Còn thời gian và muốn ôn tiếp?
  ├─ Có → trở lại [D28-STUDY]; hồi phục kiến thức tốn thời gian
  └─ Không → lên giường → ngủ → preview Day 29
       ├─ Hủy / save lỗi → tiếp tục Day 28
       └─ Commit/save thành công → summary/checkpoint
  │
  ▼
[D29] DAY 29 • 06:40 — HK1_MIDTERM_CLASSES (GIỮ HAI BUỔI HỌC HIỆN CÓ)
  │
  ▼
Thức dậy → vệ sinh → thay đồ → xem lịch → sách/balo → bus/H6
  │
  ▼
08:00–10:00 • VL1 → điểm danh đúng giờ/trễ/vắng → học nếu có mặt
  │
  ▼
10:00–12:00 • NMT → điểm danh đúng giờ/trễ/vắng → học nếu có mặt
  │
  ▼
Tan học → bus về KTX → cất balo → xem phần chưa nắm / nghỉ
  │
  ▼
Lên giường → ngủ → preview Day 30 + kỳ thi/việc bị lỡ
  ├─ Hủy / save lỗi → tiếp tục Day 29
  └─ Commit/save thành công → summary/checkpoint
  │
  ▼
[D30] DAY 30 • 06:30 — HK1_MIDTERM_EXAM
  │
  ▼
Thức dậy → vệ sinh → thay đồ → xem lịch thi/đồ cần mang
  │
  ▼
Đeo balo → tới H6 → vào phòng thi
  │ GT1 08:00–09:00 / VL1 10:00–11:00 / NMT 13:00–14:00
  │
  ▼
GT1 / VL1 / NMT → kiểm tra mức chuẩn bị → làm và nộp bài giữa kỳ
  ├─ Chuẩn bị tốt → câu hỏi ở mức phù hợp hơn ────────────────┐
  └─ Chuẩn bị thiếu → khó hơn; vẫn ghi kết quả người chơi làm ┤
                                                            │
  ◄─────────────────────────────────────────────────────────┘
  │
  ▼
Midterm result theo topic: STRONG / NEUTRAL / WEAK
  │ Lưu weakTopics + feedback để ưu tiên ôn cuối kỳ
  │ Không cấp tín chỉ hoặc quyết định PASS/FAIL course ở đây
  ▼
Về 404 → nghỉ/xem feedback → lên giường → ngủ → preview Day 92
  │ Chỉ xử lý active deadline / exam window / declared event
  │ Không tự tạo lớp hay vắng trong khoảng Day 30–92
  ├─ Hủy / save lỗi → ở Day 30
  └─ Commit/save thành công → montage/summary/checkpoint
  │
  ▼
[D92] DAY 92 • 07:00 — HK1_PREPARATION
  │
  ▼
Thức dậy → vệ sinh/thay đồ → mở final-week dashboard
  │ Attendance | Assignment | WeakTopics | Deadline | Eligibility
  ▼
[D92-PRIORITY] Xem việc nguy hiểm nhất và thời gian còn tới 22:00
  │
  ├─ LÀM BÀI → chọn môn → làm/nộp → assignment cập nhật ──────┐
  ├─ HỌC BÙ → xem tác dụng/chi phí → hoạt động recovery      │
  │             → history riêng; không xóa vắng/FAIL ─────────┤
  └─ ÔN → chọn topic yếu → study activity → feedback ─────────┤
                                                            │
  ◄─────────────────────────────────────────────────────────┘
  │ Mỗi lựa chọn dùng thời gian; không đủ làm hết thì ưu tiên
  ├─ Còn thời gian → trở lại [D92-PRIORITY]
  └─ 22:00 / chủ động kết thúc → kiểm tra deadline
       ├─ Đã nộp → giữ trạng thái bài đã hoàn thành ───────────┐
       └─ Chưa nộp → overdue + hậu quả/eligibility rõ ─────────┤
                                                             │
  ◄──────────────────────────────────────────────────────────┘
  │
  ▼
Ngủ → preview Day 99: giờ đích, duration, deadline/exam bị lỡ
  ├─ Hủy / save lỗi → ở Day 92
  └─ Commit/save thành công → montage/summary/checkpoint
  │
  ▼
[D99] DAY 99 • 06:30 — HK1_FINAL_EXAMS
  │
  ▼
Thức dậy → ra giường → WC/đánh răng/rửa mặt → thay đồ
  │
  ▼
Exam dashboard: GT1 / VL1 / NMT eligible? → chuẩn bị sách/giấy bút
  │
  ▼
Đeo balo → rời KTX → bus/H6 → tới phòng thi
  │
  ▼
[E1-NEXT] Môn kế theo lịch: 08:00 GT1 → 10:00 VL1 → 13:00 NMT
  │
  ├─ Chưa tới giờ → chờ tới khung thi → trở lại [E1-NEXT]
  │
  ├─ Không đủ điều kiện → lưu FAIL + reason ──────────────────┐
  ├─ Hết khung thi mà chưa dự → lưu bỏ thi + reason ──────────┤
  │                                                         │
  └─ Đủ điều kiện và trong khung giờ → vào bài thi            │
       │                                                    │
       ▼                                                    │
     [E1-QUESTION] 3 câu / 20 giây mỗi câu                    │
       │ World clock tạm dừng; exam timer vẫn chạy           │
       ├─ Load → restore câu/đáp án/thời gian → [E1-QUESTION] │
       ├─ Trả lời/hết giờ → câu tiếp theo nếu còn             │
       ├─ Thoát → xác nhận nộp                               │
       │    └─ Hủy xác nhận → [E1-QUESTION]                  │
       └─ Nộp / hết câu → câu chưa trả lời tính sai           │
            │                                               │
            ▼                                               │
          SUBMIT ONCE                                       │
            ├─ 2–3 đúng → PASS ──────────────────────────────┤
            └─ 0–1 đúng → FAIL + reason ──────────────────────┤
                                                             │
  ◄──────────────────────────────────────────────────────────┘
  │ Còn môn thi? → Có: [E1-NEXT]; không cộng/lặp kết quả cũ
  │                Không
  ▼
Tín chỉ HK1 = tổng credits môn PASS → recap PASS/FAIL/reason/nợ
  │
  ▼
Rời trường → bus về KTX → về 404 → cất balo → xem kết quả/nghỉ
  │
  ▼
Ngủ → preview Day 113 → hủy/save lỗi: ở Day 99
  │ Commit/save thành công → montage khoảng nghỉ/summary/checkpoint
  ▼
[D113] DAY 113 • 06:40 — HK2_START
  │
  ▼
Thức dậy → ra giường → vệ sinh → thay đồ → xem lịch HK2
  │
  ▼
Lấy sách GT2/VL2 → đeo balo → hành lang/cầu thang → ra KTX
  │
  ▼
Tới trạm → xem departure countdown tới bus 07:40
  ├─ KỊP XE → lên chuyến chính ───────────────────────────────┐
  └─ LỠ XE → thấy hậu quả → xem recovery                     │
       ├─ Chờ chuyến sau → xe ghé → lên xe ──────────────────┤
       └─ Phương án khác chỉ khi đã được author → thực hiện ─┤
                                                            │
  ◄─────────────────────────────────────────────────────────┘
  │
  ▼
Tới H6 → điểm danh theo giờ thực tế; trễ không tự thành đúng giờ
  │
  ▼
08:00–10:00 GT2 → hoạt động học nếu có mặt / ghi vắng nếu bỏ
  │
  ▼
10:00–12:00 VL2 → hoạt động học nếu có mặt / ghi vắng nếu bỏ
  │
  ▼
Tan học → bus về KTX → về phòng → cất balo
  │
  ▼
[D113-FREE] Làm bài / học / nói chuyện / game / nghỉ
  ├─ Tiếp tục hoạt động → thời gian và state cập nhật → [D113-FREE]
  └─ Ngủ → preview Day 155
       ├─ Hủy / save lỗi → [D113-FREE]
       └─ Commit/save thành công → montage/summary/checkpoint
  │
  ▼
[D155] DAY 155 • 06:40 — HK2_DORM_LIFE
  │
  ▼
Thức dậy → xuống giường → WC/đánh răng/rửa mặt → thay đồ
  │
  ▼
Xem lịch → lấy sách → đeo balo → ra KTX → bus → H6
  │
  ▼
08:00–10:00 VL2 → điểm danh/học
  │
  ▼
10:00–12:00 Lập trình → điểm danh/học
  │
  ▼
Tan học → bus về KTX → cất balo → buổi tối ở 404
  │
  ▼
[D155-EVENING] Chọn hoạt động
  ├─ HỌC → bài tập/topics/prep + thời gian ───────────────────┐
  ├─ GAME → phiên chơi + thời gian ───────────────────────────┤
  └─ LONG → chơi/trò chuyện cùng → relationship/story event ──┤
                                                             │
  ◄──────────────────────────────────────────────────────────┘
  │ Còn muốn hoạt động? → [D155-EVENING]
  │ Khuya / Long muốn ngủ
  ▼
Đèn còn sáng?
  ├─ Có → hội thoại/va chạm → chọn phản ứng
  │        → tự đứng dậy → tới công tắc → tắt đèn ────────────┐
  └─ Không ──────────────────────────────────────────────────┤
                                                             │
  ◄──────────────────────────────────────────────────────────┘
  │
  ▼
Tới đúng giường → leo nếu tầng trên → người chơi/Long nằm nghỉ
  ├─ Chưa ngủ → có thể trở lại hoạt động theo state phòng
  └─ Chọn ngủ → preview Day 204
       ├─ Hủy / save lỗi → ở Day 155
       └─ Commit/save thành công → montage/summary/checkpoint
  │
  ▼
[D204] DAY 204 • 07:00 — HK2_PREPARATION
  │
  ▼
Thức dậy → vệ sinh/thay đồ → mở final-week dashboard
  │ GT2 / VL2 / Lập trình: attendance, assignment, weak topics, eligibility
  │ Giữ hai buổi của course graph hiện có:
  │ 08:00–10:00 GT2 buổi 2 / 10:00–12:00 Lập trình buổi 2
  ├─ Đi học → sách/balo → bus/H6 → học → bus về/cất balo ──┐
  └─ Ưu tiên việc khác → hết khung học ghi vắng ────────────┤
                                                           │
  ◄────────────────────────────────────────────────────────┘
  │
  ▼
[D204-PRIORITY] Chọn thứ tự dùng thời gian còn lại
  ├─ Bài tập thiếu → làm/nộp trước 22:00 ─────────────────────┐
  ├─ Học bù → xem chi phí/tác dụng → recovery history ────────┤
  └─ Ôn → chọn môn/topic → hoạt động → feedback ──────────────┤
                                                             │
  ◄──────────────────────────────────────────────────────────┘
  │ Còn thời gian → [D204-PRIORITY]
  │ 22:00 / chủ động kết thúc
  ▼
Deadline check → đã nộp: SAFE / chưa nộp: OVERDUE + hậu quả
  │
  ▼
Chuẩn bị đồ thi → lên giường → ngủ → preview Day 211
  ├─ Hủy / save lỗi → ở Day 204
  └─ Commit/save thành công → montage/summary/checkpoint
  │
  ▼
[D211] DAY 211 • 06:30 — HK2_FINAL_EXAMS
  │
  ▼
Thức dậy → vệ sinh → thay đồ → exam dashboard → chuẩn bị giấy bút
  │
  ▼
Đeo balo → ra KTX → bus → H6 → phòng thi
  │
  ▼
[E2-NEXT] Môn kế theo lịch: 08:00 GT2 → 10:00 VL2 → 13:00 Lập trình
  │
  ├─ Chưa tới giờ → chờ tới khung thi → trở lại [E2-NEXT]
  ├─ Không eligible → FAIL + reason ──────────────────────────┐
  ├─ Hết khung giờ chưa dự → bỏ thi + reason ─────────────────┤
  └─ Eligible và đúng khung → [E2-QUESTION]                   │
       │                                                    │
       ▼                                                    │
     3 câu × 20 giây/câu; world clock pause, exam timer chạy  │
       ├─ Load → restore câu/đáp án/thời gian → [E2-QUESTION] │
       ├─ Trả lời/hết giờ → câu tiếp theo                     │
       ├─ Thoát → confirm nộp; hủy → [E2-QUESTION]           │
       └─ Nộp / hết câu → unanswered sai → SUBMIT ONCE        │
            ├─ 2–3 đúng → PASS ──────────────────────────────┤
            └─ 0–1 đúng → FAIL + reason ──────────────────────┤
                                                             │
  ◄──────────────────────────────────────────────────────────┘
  │ Còn môn? → [E2-NEXT]; mỗi kết quả/tín chỉ chỉ ghi một lần
  │ Không
  ▼
YEAR 1 RECAP: credits = tổng môn PASS; PASS/FAIL/reason/môn nợ
  │
  ▼
Rời H6 → bus về KTX → về 404 → cất balo → chuẩn bị ăn lẩu
  │
  ▼
Lấy bếp → chuẩn bị nguyên liệu → sắp bàn/chỗ ngồi → gọi bạn
  │
  ▼
ĂN LẨU — vẫn được tham gia khi còn môn nợ
  ├─ 32/32 → biến thể thoại theo kết quả tốt ─────────────────┐
  ├─ <32 → biến thể thoại có môn nợ ──────────────────────────┤
  └─ Nợ nhiều → biến thể thoại theo state được author ─────────┤
                                                              │
  ◄───────────────────────────────────────────────────────────┘
  │
  ▼
Tổng kết một năm → tới giường → nằm nghỉ / chọn ngủ
  ├─ Nằm nghỉ → vẫn ở phòng, chưa YEAR1_COMPLETE
  └─ Ngủ → preview đích YEAR 2 + ngày/giờ/summary
       ├─ Hủy → ở Day 211 sau lẩu
       └─ Xác nhận → finalize Year 1 trên snapshot
            │ Giữ credits, PASS/FAIL/reasons, history và event quan hệ
            ▼
          Save snapshot
            ├─ Lỗi → giữ mốc/state nguồn, cho thử lại
            └─ Thành công → YEAR1_COMPLETE một lần
                                  │
                                  ▼
                         DAY 212 • 07:20 — BÀN GIAO STATE YEAR 2
```

## Definition of Done

- [ ] Giữ phạm vi `Room404_MVP`/YearOne v2 và đồng bộ plan, milestone calendar, course schedule, UI, state, save và scene. Danh sách phải có đủ 12 mốc: Day 0 Arrival; Day 1 first classes; Day 2 gaming choice; Day 28 midterm prep; Day 29 existing classes; Day 30 midterm exam; Day 92 HK1 prep; Day 99 HK1 exams; Day 113 HK2 start; Day 155 dorm life; Day 204 HK2 prep; Day 211 HK2 exams.
- [ ] Day 0 triển khai chuỗi quầy KTX → làm thủ tục/nhận chìa khóa/phòng → tới 404/mở cửa → nhìn phòng → chọn giường → đặt balo/dọn đồ/làm quen roommate → căn tin/chọn món/ăn → về phòng/free time → lên giường. Chọn giường trên thì leo thang và kiểm tra chỗ nằm; chọn giường dưới thì đi tới và nằm thử, không leo thang.
- [ ] Day 1 và routine ngày học có thứ tự thao tác: báo thức/thức dậy → ra giường → WC/đánh răng/rửa mặt → thay đồ → xem laptop/lịch → lấy sách đúng môn → đeo balo → hành lang/cầu thang/cổng KTX/trạm → bus/H6 → vào lớp/điểm danh/học → tan học/bus về → cất balo → làm bài/laptop/nói chuyện/nghỉ → ngủ. Chốt giờ bắt đầu 06:40–06:50, bus 07:40 và travel time đủ để tới lớp 08:00.
- [ ] Day 2 thể hiện **hai choice riêng**: (1) trước lớp Long rủ chơi game — đi học hoặc chơi tiếp, có cảnh báo T-30/T-15/T-5 và attendance đúng giờ/trễ/vắng; (2) buổi tối — học sớm hoặc chơi tiếp với Long, đổi thời gian học lấy knowledge/prep hoặc relationship. Không gộp hai choice thành một hoặc biến lịch học sáng thành lựa chọn buổi tối.
- [ ] Day 28 có dashboard theo GT1/VL1/NMT; chọn môn/topic, chạy study activity có feedback, lưu `completedTopics`/`weakTopics`. Học sớm giảm gap và thời gian ôn; chơi nhiều làm tăng lượng cần bù (mốc tham khảo trong plan: 1–2 so với 2–4 activities); vẫn cho recovery bằng cách dành thêm thời gian.
- [ ] Day 29 giữ lịch lớp hiện có: Vật Lý 1 08:00–10:00, Nhập môn Tin 10:00–12:00; điểm danh, hoạt động học và về 404. Day 30 mới tổ chức thi giữa kỳ cho GT1/VL1/NMT; kết quả tạo strong/neutral/weak topics và ảnh hưởng prep cuối kỳ, **không** trực tiếp PASS/FAIL môn hoặc cấp tín chỉ.
- [ ] Day 92 và Day 204 có dashboard attendance/assignment/deadline/weak topics/eligibility. Trong quỹ thời gian giới hạn, người chơi chọn làm bài, học bù hoặc ôn; deadline 22:00, overdue, eligibility và chi phí recovery hiển thị rõ. Không biến dashboard thành checklist bắt buộc hoàn thành tất cả.
- [ ] Day 99 và Day 211 có routine thi, dashboard điều kiện dự thi, lịch môn và exam flow: 3 câu, 20 giây/câu, world clock pause, timer thi chạy, nộp sớm tính câu bỏ trống là sai, thoát có confirm, load giữa thi khôi phục câu trả lời/thời gian, submit một lần. Eligible + 2–3 đúng = PASS; 0–1 đúng = FAIL; không đủ điều kiện/bỏ thi có reason. Tín chỉ chỉ cộng một lần từ môn PASS.
- [ ] Day 113 bắt đầu đủ sớm để routine trước bus 07:40; nhánh kịp/lỡ xe có hậu quả và recovery được trình bày. Day 155 có lớp Vật Lý 2/Lập trình, về 404, lựa chọn học/game/bạn bè, hội thoại Long/roommate, phản ứng khi đèn còn sáng, tự tắt đèn và đi ngủ bằng thao tác.
- [ ] Day 211 tổng kết credit/PASS/FAIL/môn nợ; người chơi về 404, chuẩn bị bếp/nguyên liệu/bàn, gọi bạn, ăn lẩu, nhận biến thể story theo kết quả rồi ngủ. Lẩu không bị khóa khi chưa đủ 32 tín chỉ. Sau commit cuối năm, Year 2 snapshot giữ credits, course results/reasons và history.
- [ ] Time-skip preview nêu source/target, ngày/giờ/địa điểm, duration tính từ hai mốc, deadlines/exam windows/declared events bị lỡ, hậu quả và recovery. Hủy giữ nguyên state; commit apply/save một lần; save lỗi ở lại milestone nguồn. Trong khoảng skip chỉ xử lý deadline, exam window và event đã khai báo; **không tự tạo lớp hoặc vắng học**. Montage/summary/checkpoint đưa người chơi tới đúng mốc đích.
- [ ] F8 có checkpoint với save sandbox riêng tại Day 0 chọn giường, trước hai choice Day 2, Day 28/30, Day 92, trước xe Day 113, Day 99/211 và trước lẩu. Restore phải dọn phiên cũ và khôi phục vị trí, pose, UI, giờ, state; mỗi checkpoint ghi **checkpoint → thao tác → kết quả mong đợi**.
- [ ] Chốt và đồng bộ timestamp đích, gồm giờ bắt đầu ngày học và giờ bàn giao Year 2; không để diagram, calendar data và transition preview đưa ra giờ khác nhau. Giữ course graph, credit target, recovery canon và ending hiện hành; đổi canon thì cập nhật baseline trước.
