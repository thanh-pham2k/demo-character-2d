# Năm 1 — chuỗi sự kiện buổi sáng v0.1

Đây là lát cắt authoring, không phải toàn bộ event database. Giờ, tiền, hiệu ứng và text là dữ liệu giả lập để làm rõ nhánh; chưa qua mô phỏng hoặc playtest. Áp dụng quy tắc boundary/save trong `02_He_thong_thoi_gian_va_su_kien.md` và relationship trong `03_Hoc_tap_va_quan_he.md`.

## Fixture chung

Ngày D2, 07:00, LOC_ROOM; player energy 75, tiền 30 M, Long đã gặp và available, lịch học/course `C_NMT` đã biết, player đã đăng ký. Tuyến KTX→trạm mất 10 phút, xe đầu đóng cửa và rời 07:40, chuyến sau 07:50; xe đến H6 sau 5 phút, H6→lớp 5 phút; giá 3 M. Tiết học `[08:00,10:00)`. Player có thể đi tới trạm và chờ; không tự động bắt xe nếu chưa chọn. Đây là fixture thử, chưa phải calendar campaign đầy đủ.

Quy ước trùng mốc: lên xe đúng giờ rời được nếu đã ở trạm, cửa đang mở và chọn boarding trước bước đóng cửa; tới sau thời điểm đó là lỡ. Cửa lớp đóng ở 10:00; tới 08:00 là đúng giờ, sau 08:00 và trước 10:00 là muộn. Một lượt game 15 phút; chơi ngay 07:00 → 07:15 → tới trạm 07:25 → kịp xe đầu. Hai lượt liên tiếp đến 07:30, tới trạm 07:40 có thể kịp nếu người chơi chọn lên đúng mốc; ba lượt đến 07:45, tới trạm 07:55, lỡ cả hai chuyến đã nêu. Không tự tạo chuyến thứ ba: fixture này cần author recovery khác, xem `EV_Y1_BUS_RECOVERY`.

### EV_Y1_LONG_INVITE — Một trận trước giờ học?

- Category: major NPC; năm 1, W02/D2, window `[07:00,07:20)`; LOC_ROOM; actor Long.
- Trigger: khi player ở phòng và cửa sổ active; required `met_long=true`, Long available; forbidden `invite_resolved=true`. Nếu Long không available hoặc player không vào phòng, invitation hết hạn trung tính; vẫn có thể đi học. Một instance/ngày, priority 50.
- Goal/conflict: đến lớp đồng thời cân nhắc lời rủ. Trước choice hiện giờ học, chuyến xe đã biết và thời lượng 15 phút/lượt; thời gian đi bộ dự kiến, giá vé, energy dự kiến. Không hứa chắc giờ đến khi player có thể thay lựa chọn.
- A `leave`: không tốn phút, `invite_resolved=leave`; Long: “Ừ, lát gặp.” Không cộng/trừ quan hệ, không tạo memory đã chơi. Player tiếp tục tự do.
- B `play_one`: khởi chạy activity 15 phút, cost E −3 phân bổ theo thời gian; khi hoàn tất tạo `memory_long_game_d2`, actors Lươn/Long, outcome `played=1`, trust Long +2 một lần, `game_rounds=1`, event `EV_Y1_LONG_REPLAN` eligible. Dừng trước hoàn tất không cấp memory hoặc trust, thời gian và E đã trôi không hoàn.
- C `not_now`: không tốn phút, `invite_resolved=declined`; không trừ trust chỉ vì từ chối. Mở các hoạt động khác.
- Career/academic effect trực tiếp: không. Nguy cơ gián tiếp: thời gian đến trạm và lớp. Không tự đặt flag lỡ xe hoặc muộn.
- Miss/alternate: hết window hoặc Long không available → `expired`; không sinh lời thoại/memory. Có thể xem lịch và tự đi. Recovery cho player muốn tương tác Long: lời mời buổi tối nếu event và availability cho phép, không đảm bảo.
- Idempotency: commit choice theo instanceId; sau reload không mời lại và không cộng trust hai lần. Callback: `CB_LONG_EVENING` đọc memory trò chơi; không tự cho Long biết chuyện ở lớp.

### EV_Y1_LONG_REPLAN — Chơi tiếp?

- Category: conditional; sau một lượt completed, LOC_ROOM; trong window `[07:15,07:45)`, Long available; max 3 lượt trong fixture. Một instance trên mỗi số lượt, không mở lại sau `stop`.
- Goal: chọn thêm quan hệ/tương tác hay giữ thời gian. UI hiện thời điểm hiện tại, giờ xe, thời gian đi trạm và kết thúc dự kiến; khi thời gian tới lớp còn 30/15/5 phút, thông báo lịch đã biết (không ngắt dialog đang mở).
- `stop`: 0 phút; đóng chuỗi, player tự đi hoặc chọn hoạt động khác. `continue`: 15 phút, E −3; thêm 1 vào `game_rounds` khi hoàn thành, không cộng trust hoặc memory của lượt đầu lần nữa. Long không tự đi theo player.
- Miss: rời phòng, Long hết ca available, hoặc hết window → đóng cơ hội; không thêm phạt. Interrupt trước completion: ghi elapsed và energy đã chi, không tăng rounds; player có thể dừng.
- Academic/career: không trực tiếp. Memory đầu giữ nguyên, không tạo ba bản sao cho cùng phiên. Follow-up là bus/class records do travel và attendance thật tạo ra.

### EV_Y1_BUS_RECOVERY — Khi không kịp xe

- Category: conditional transport; LOC_BUS; trigger khi player tới trạm sau giờ rời của một chuyến đã chọn hoặc khi không còn chuyến trong fixture. Chỉ ghi `missed_bus_id` cho chuyến đã chọn và đã rời mà player chưa lên, kể cả nếu player chưa tới trạm; nếu chưa chọn chuyến thì chỉ ghi `no_bus_available`, không tạo memory lỡ xe. Nếu không có tiền, vẫn có lựa chọn hỗ trợ.
- A: đợi chuyến tiếp theo nếu calendar còn chuyến; chi phí chờ là thời gian đến xe, boarding mới trừ 3 M. B: đến quầy trợ giúp trạm và chọn hỗ trợ di chuyển; trong fixture, hỗ trợ miễn phí mất 30 phút tới H6, không trừ tiền, chỉ có một lần cho ngày này. C: quay lại KTX nếu muốn; không tự đánh dấu vắng cho đến khi calendar class hết cửa sổ.
- Tại 07:55, hai chuyến fixture đã hết; nếu chưa chọn chuyến nào thì không sinh `missed_bus_id`. Chọn B → 08:25 tới H6 → 08:30 tới lớp, attendance late. Nếu chờ, phải hiển thị “chưa có chuyến khác trong lịch” và disable thay vì tạo xe. Recovery khác cho campaign thật phải là calendar/data được author riêng.
- NPC/career: không tự cho Long biết. Memory `missed_bus` chỉ ghi khi player bỏ lỡ chuyến đã chọn; support không hứa điểm danh đúng giờ. Save: transaction boarding/trợ giúp có idempotency key; reload không trừ tiền hai lần.

### EV_Y1_ATTEND — Đến lớp và điểm danh

- Category: scheduled academic; location LOC_CLASS; entry `C_NMT_D2` `[08:00,10:00)`; required enrolled. Khi player tương tác trước 08:00, ghi `waiting_for_class`; chỉ ghi `on_time` lúc 08:00 nếu player vẫn ở lớp và chọn ngồi học; trong khoảng sau start và trước end ghi `late`. Nếu không tham dự khi đồng hồ đến 10:00, ghi `absent` một lần. Đã điểm danh thì không thể tiếp tục đổi trạng thái.
- Choice khi còn tiết: ngồi và học thời gian còn lại hoặc rời đi; kết quả topic do definition lớp xác định, không tự cấp nguyên vẹn nội dung cho phần đã lỡ. Nếu absent, course record nêu lý do và đường học bù được author; không tự pass hoặc fail course.
- Hạnh phản ứng khi có mặt và chứng kiến player muộn; không tự biết nguyên nhân xe. Minh chỉ xuất hiện nếu đã được xếp lịch. Player có thể tự kể Long nghe sau; khi đó mới thêm knowledge `long_knows_late`.
- Academic: ghi attendance per entry; không cấp credit. Với entry đầu tiên `CLASS_NMT_FIRST`, chỉ khi player thực sự tham dự tạo memory `first_class` với player là actor; không tạo memory này cho entry D2 hoặc trường hợp vắng. Career/relationship: không thay đổi trực tiếp. Miss: ghi absent tại end kể cả khi player đang ở khu khác; tiếp tục ngày và giữ cơ hội học bù.

### CB_LONG_EVENING — Nhắc lại chuyện sáng

- Category: callback; cửa sổ tối do calendar tác giả khai báo, LOC_ROOM, Long available. Nếu `memory_long_game_d2` tồn tại, Long có thể nhắc trận game; nếu `long_knows_late=true`, có thêm câu hỏi về việc đến muộn; nếu không, không nhắc nguyên nhân hay giờ đến lớp. Nếu không có memory, dùng thoại trung tính.
- Lựa chọn học / chơi / nghỉ là activity riêng, không tự cho lời mời lặp mãi. Miss nếu Long bận hoặc player không tới: callback expire trung tính; không xóa memory. Không cộng quan hệ khi chỉ hiển thị thoại; effect gắn với hoạt động hoàn tất hoặc choice được author.

## Các ca cần kiểm tra

1. 07:00 chơi một lượt, đi trạm → 07:25; lên 07:40, đến lớp 07:50 → on_time. Tính lại 07:40 + 5 + 5 = 07:50.
2. Ba lượt game → 07:45, tới trạm 07:55 → hỗ trợ 30 phút, đến lớp 08:30 → late, Long không biết nếu chưa kể.
3. Chưa gặp Long hoặc từ chối → không có memory trò chơi, không trừ trust.
4. Save ngay trước commit, sau commit, giữa travel và sau attendance: không nhân đôi cost/memory/credit.
5. Đến lớp 08:00 on_time; 08:01 late; 10:00 absent. Boarding đúng 07:40 phải xét theo thứ tự input/đóng cửa đã mô tả.

Đây là expected cases, chưa chạy. Còn phải author buffer input tại đúng mốc xe, scheduler cho class bắt đầu, phí và hỗ trợ; fixture không đại diện toàn campaign. Các event khác của Năm 1 và content matrix các năm tiếp theo chưa có record.
