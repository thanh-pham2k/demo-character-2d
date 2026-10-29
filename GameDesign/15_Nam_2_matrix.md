# Năm 2 — content matrix v0.1

**Trạng thái:** cấu trúc nội dung đề xuất, chưa có event database hoặc calendar runnable. Theme khám phá, quan hệ và tự lập; câu hỏi “Tôi thích cuộc sống nào?”. W01–W12 là nhãn sắp xếp, không đồng nghĩa mọi ngày đều playable. Lịch và con số trong các event cần author sau; UI không tuyên bố đã mở cơ hội khi chưa đủ điều kiện.

## Ma trận

| Tuần / seed | Học tập | Long | Minh / Hạnh | An / Nam | Career | Location | Lựa chọn | Kết quả / callback |
|---|---|---|---|---|---|---|---|---|
| W01 `Y2_RETURN` | Xem khóa và môn nợ thực có | Phản hồi theo quan hệ/memory | Minh chỉ nếu đã gặp | Chưa tự gặp | Evidence Y1 giữ | ROOM, H6 | Học bù / xem cơ hội mới | Môn nợ không bị xóa; không gán ký ức không có |
| W01 `Y2_RETAKE_PLAN` | Xem lịch học lại nếu có nợ | Có thể bận | Hạnh tư vấn theo lịch | — | — | H6, LIBRARY | Chọn slot học lại hoặc chưa đăng ký | Chỉ mở nếu calendar có slot; chưa có data thì nêu unresolved |
| W02 `Y2_CLUB_FAIR` | Lịch học đang chạy | Có kế hoạch riêng | — | An/Nam có thể ở fair | Biết kỹ năng liên quan | CLUB | coding / thể thao / nhạc / tình nguyện / không tham gia | Cửa sổ và thời gian thay đổi; không tự khóa nghề |
| W03 `Y2_CLUB_FIRST` | Cân bằng bài tập | Có thể tham gia CLB khác | — | Gặp An hoặc Nam theo lựa chọn | Cơ hội project | CLUB | Nhận nhiệm vụ / quan sát / nghỉ | Memory và evidence chỉ khi hoàn thành; missed invitation trung tính |
| W03 `Y2_WORK_NOTICE` | Lịch học hiển thị | — | — | — | Biết công việc, chưa có hợp đồng | CAFE, WORK | Xem ca / không ứng tuyển | Vẫn có đường tự học nếu không làm |
| W04 `Y2_FIRST_SHIFT` | Bài tập cạnh tranh thời gian | Rủ ăn tùy lịch | — | An có lời mời nếu đã gặp | Tiền, kinh nghiệm tùy ca | CAFE | Nhận ca / ưu tiên học | Thu nhập sau completion; có thể lỡ lời mời |
| W05 `Y2_MEET_AN` | — | Có lịch riêng | — | An có sự kiện mở, có đường gặp ngoài CLB | — | CLUB hoặc CANTEEN | Trò chuyện / để sau | Không tự tạo tình bạn hoặc romance |
| W06 `Y2_GROUP_ASSIGN` | Môn có project | — | Minh nếu đã gặp, hoặc S04 thay thế | — | Project evidence | PROJECT | Nhóm với Minh / nhóm khác / tự làm nếu course cho phép | Team composition và scope; prerequisite môn phải author |
| W07 `Y2_TEAM_CONFLICT` | Deadline project | — | Minh/S04 theo team | — | Chất lượng artifact | PROJECT | Chia lại việc / tự làm thêm / báo giảng viên | Time cost, trust/conflict và quality theo event |
| W08 `Y2_TEAM_REVIEW` | Feedback sau nộp | — | Hạnh biết bài nộp theo quyền | Nam chỉ khi được mời | Evidence completed/partial | PROJECT | Nhận feedback / trì hoãn | Không xóa tiến độ sau nộp; callback có điều kiện |
| W09 `Y2_SHIFT_VS_EXAM` | Lịch thi đã biết | Rủ chơi tùy lịch | — | An có thể mời hoạt động | Thu nhập hoặc ôn tập | ROOM, CAFE | Nhận ca / ôn / nghỉ | Time & E, không ngầm biến lựa chọn thành fail |
| W10 `Y2_FINAL` | Thi từng môn và course resolver | Theo lịch riêng | Theo lớp/office hour | — | Evidence học phần | CLASS | Thi / nộp sớm / bỏ lỡ | Điểm/credit theo definition từng môn; recovery đã author |
| W11 `Y2_RECONNECT` | Xem kết quả | Có thể sắp xếp gặp sau khi bận | Minh theo availability | An theo memory thật | Có thể trao đổi định hướng | CANTEEN / ROOM | Gặp / hẹn lại / để sau | Relationship và memory theo outcome; không ép hòa giải |
| W12 `Y2_RESOLVER` | Chốt snapshot/preview Y3 | Giữ memory/conflict | Giữ team outcome | Giữ trạng thái gặp | Evidence, project, income ledger | — | Hủy / xác nhận chuyển năm | Atomically save, không cấp lại credit |

## Các mục đặc tả năm

- Calendar: các mốc seed ở bảng; chưa có timestamp thực, holiday hoặc số tuần playable. Academic: cần course catalog, registration, final, retake và milestone từng môn. Hệ mới: club, part-time, money opportunities và group project, tái dùng time/energy/relationship.
- Location mới: CLUB, CAFE, PROJECT, WORK khi biết; phải có route và lịch mở. Activity: fair, ca làm, họp nhóm, project, tự học và nghỉ trong `08_Dia_diem_va_hoat_dong.md`.
- NPC chính: Long, Minh, An, Nam, Hạnh theo điều kiện; S04/S06 và nhân viên ca làm là secondary. Không gặp NPC không ngăn kết thúc năm.
- Major seeds: return, fair, project conflict, shift/exam, final; medium: ca đầu, gặp An, review, reconnect; small: bảng tin hoặc lời mời với window. Chưa có event records đầy đủ, không dùng phân loại seed làm bằng chứng đạt chỉ tiêu số lượng.
- Academic milestones: đăng ký môn, bài nhóm, thi, resolver; relationship milestones: gặp/giữ liên lạc, conflict, hàn gắn có điều kiện; career milestones: evidence project/ca làm, không tự cấp internship.
- Failure: trượt môn, lỡ ca, project partial, lỡ lời mời. Recovery: học lại nếu calendar có, sửa scope trước deadline, ca sau nếu có, nói chuyện lại khi NPC available. Mỗi recovery cần event record, chi phí và deadline; không mặc định chắc chắn tồn tại.
- Unlock/gating: hội CLB khi đã biết và window active; có thể chọn không vào CLB; ứng tuyển việc cần hợp đồng/lịch; project nhóm phụ thuộc môn và team. Một lời mời hết hạn không khóa mọi cơ hội sau.
- Resolver/carry: snapshot `YearSnapshot`; giữ course record, memory, relationship/conflict, tiền, energy, skill/evidence, project và lịch event; không cấp lại credits hay tự gán career. Recap chỉ nêu điều đã xảy ra và không định nghĩa danh tính player bằng điểm duy nhất.

## Chưa đạt production

Chưa có lịch, số liệu, hồ sơ CLB/job, rubric project, lựa chọn chi tiết hoặc nhánh dialogue trong event DB. Chưa có đầy đủ recovery cho môn nợ Y1; chưa mô phỏng được fairness tài chính, xung đột lịch hoặc save/restore. Các hướng romance là tùy chọn và chưa author consent/event đầy đủ.
