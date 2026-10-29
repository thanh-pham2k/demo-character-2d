# Calendar Năm 1 — fixture C_NMT v0.1

Chỉ là lịch thử để đối chiếu những event đã author, không phải calendar hoàn chỉnh của 24 môn hay lịch runtime. Thời gian dùng ngày campaign nguyên bắt đầu D1 00:00; `Wn = D(7n−6)..D(7n)`. Ví dụ W02 = D8–D14. Cùng mốc xử lý theo `02` và exception boarding `29`. Cửa sổ sự kiện `[start,end)` trừ khi ghi rõ completion đúng end được chấp nhận. Mốc không có entry không tự tạo lớp, lương hay NPC.

## Entries

| ID | Ngày / thời gian | Location | Điều kiện / tác dụng |
|---|---|---|---|
| RECEPTION_1 | D1 14:00–18:00 | LOBBY | nhận phòng theo `13`; hoàn tất trước/đúng 18:00, ca dự phòng D2 06:00–07:00 |
| MEAL_INVITE | D1 17:00–20:00 | ROOM→CANTEEN | Long đã gặp/available; theo `13`, không bắt buộc |
| CLASS_NMT_FIRST | D8 08:00–10:00 | CLASS | C_NMT enrolled; first class, attendance theo `48` |
| LONG_INVITE | D9 07:00–07:20 | ROOM | met Long, available; `EV_Y1_LONG_INVITE` |
| BUS_0740 / BUS_0750 | D9 rời 07:40 / 07:50 | BUS→H6 | boarding decision theo `29`; 3M, 5 phút trên xe |
| CLASS_NMT_D2 | D9 08:00–10:00 | CLASS | cùng `C_NMT_D2` trong `10`; 08:00 on-time, 10:00 absent |
| LONG_EVENING | D9 18:00–20:00 | ROOM | Long available; callback nếu có memory, không tạo memory nếu chỉ hiện thoại |
| ASG_OPEN / DUE | D15 08:00 / D15 22:00 | ROOM/CLASS | ASG_01 mở/đến hạn; completion submit đúng 22:00 hợp lệ |
| ASG_LATE | D22 08:00–22:00 | ROOM/CLASS | late window; completion đúng 22:00 được nhận trước close |
| MIDTERM_PREP | D22 18:00–22:00 | ROOM/LIBRARY | cùng ngày recovery; panel theo `12` |
| D22_LONG_HELP | D22 18:30–18:40 | ROOM | invitation có điều kiện; Long available đến 19:00; xem `58` |
| MIDTERM_NMT | D29 08:00–09:00 | CLASS | bài giữa kỳ theo `14`; chưa có background policy |
| REVIEW_NMT | D36 17:00–20:00 | LIBRARY | feedback hoặc missed; hỏi Hạnh chỉ khi có lịch |
| MINH_LIBRARY_INTRO | D36 17:00–18:00 | LIBRARY | cơ hội gặp Minh nếu chưa gặp; xem `61` |
| LIGHT_CONFLICT | D43 22:00–23:00 | ROOM | Long met, có mặt, đèn bật; `12` |
| GROUP_STUDY | D50 18:00–20:00 | LIBRARY | Minh hiện diện theo fixture khi đã gặp; xem event và giới hạn tại `60` |
| FINAL_NOTICE_NMT / FINAL_PREP | D57 18:00 / 18:00–22:00 | ROOM/LIBRARY | công bố lịch thi; lựa chọn chuẩn bị theo `62` |
| FINAL_NMT | D64 08:00–09:00 | CLASS | final theo `14`, tuyến và lựa chọn lỡ xe theo `63`; phải chủ động start trước 09:00 |
| REEXAM_REG | D71 08:00–18:00 | H6 | chỉ final fail/missed và chưa pass; theo `30` |
| HOTPOT_PLAN | D71 18:00–22:00 | ROOM | theo `59`, tùy chọn; D71 18:00 registration close trước event; Long 18:00–22:00, Minh 19:00–21:30 khi đã gặp |
| REEXAM_NMT | D78 08:00–09:00 | CLASS | registered và eligible; theo `30` |
| YEAR_RESOLVER | D84 18:00 | ROOM/H6 | preview và commit sau các entry đã author; không tự pass môn khác |

## Route fixture và ví dụ tính tay

ROOM→BUS 10 phút, BUS→H6 5 phút sau departure, H6→CLASS 5 phút. D9 game 07:00–07:15 → trạm 07:25 → xe 07:40 → lớp 07:50; player phải vẫn ở lớp và chọn học lúc 08:00 để on-time. Ba lượt 07:00–07:45 → trạm 07:55; nếu chọn hỗ trợ trong fixture 30 phút → H6 08:25 → CLASS 08:30, được late nếu chọn học. Đây là tính tay, không có simulator hay runtime.

D22 player ở ROOM 18:00, ASG_01 còn hai milestone: làm 90 phút tới 19:30, nộp 5 phút tới 19:35; sau đó học một topic 60 phút tới 20:35. Có thể chọn ngược thứ tự và vẫn kịp trong fixture nếu đủ E và ở cùng location. Việc có một lịch khả thi không chứng minh mọi state có thể phục hồi; player ở LIBRARY phải có route duration trước khi tính.

## Gaps và xung đột

- NMT có 2 attendance entry, nhưng calendar trên chưa có registration đầy đủ D1–D8, các route WC/cầu thang (giờ xe D8 đã có fixture `49`) hay phí sinh hoạt. Các môn khác chưa có entry.
- ASG recovery mở D22 thay cho các ngày sau D15 theo fixture, không tự chạy daily. Thi cuối/thi lại cần background timer/accessibility policy trước khi có runtime.
- Reception dự phòng D2 và first class D8 thuộc fixture calendar này; không gộp với fixture D2 của `10`, trong đó D2 là **ngày thứ hai của chuỗi sự kiện sáng**, được map sang D9 ở đây. Các tài liệu cũ chỉ là ví dụ độc lập.
- Chưa có actual timestamps cho năm 2–6, NPC availability, budget đầy đủ, lịch retake mọi môn hoặc migration. Không chứng minh đủ 72 credits hay không có soft-lock.
