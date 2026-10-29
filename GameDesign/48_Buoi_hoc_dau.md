# Buổi học đầu — event fixture v0.1

Fixture Năm 1 D8 (calendar `34`), giả lập. Không phải lịch trường thật, không thay dữ liệu của 23 môn còn lại. Event D9 trong `10` độc lập và không sinh `first_class`.

## Điều kiện và lịch

`C_NMT_FIRST`: entry canonical `CLASS_NMT_FIRST`, course C_NMT enrolled, LOC_CLASS, `[D8 08:00,D8 10:00)`. Player biết lớp và giờ từ calendar ngay sau enrollment; discovery route phải được author trước runtime. Fixture bổ sung `BUS_D8_0740` khởi hành 07:40 tại BUS, giá 3M, 5 phút tới H6; ROOM→BUS 10 phút, H6→CLASS 5 phút, theo `34`. Không tự tạo xe khác hay điều kiện recovery nếu chưa có entry.

Hạnh assigned cho tiết, available `[08:00,10:00)` trong fixture; Minh có roster/presence cục bộ theo `61`, không tự gán player met Minh. Gặp giảng viên, bạn học là interaction riêng; player ngồi học không mặc định quan hệ thay đổi.

## EV_Y1_FIRST_CLASS

Category scheduled academic, một instance theo `(courseAttemptId,entryId)`. Trước 08:00, player tại lớp có thể chọn `wait`: 0 phút, state `waiting_for_class`; nếu rời trước start thì clear. Tại 08:00, nếu vẫn có mặt, panel chọn `attend` hoặc `leave`; đồng hồ pause khi chọn.

- `attend` từ trước hoặc đúng 08:00: commit attendance `on_time`, bắt đầu session học tới `10:00` hoặc dừng sớm theo lựa chọn. `attend` sau 08:00 nhưng trước 10:00: `late`, session chỉ chạy thời gian còn lại. UI hiển thị thời gian, cost và deadline đã biết trước confirm. Fixture session đầy đủ 120 phút, E−10; nếu late, tổng E được tính theo thời lượng còn lại tỉ lệ làm tròn xuống và cumulative rule `29`; effect được tính theo các checkpoint bên dưới.
- `leave`: không điểm danh, player chọn hoạt động khác; nếu quay lại trước end vẫn có thể attend late. Tại 10:00 nếu chưa commit attendance → `absent` một lần, reason `not_attended`; không auto gán lý do xe.
- Session có hai checkpoint D8 09:00 và 10:00: nếu player tham dự liên tục từ thời điểm trước checkpoint đó ít nhất 30 phút, checkpoint tạo `exposure` cho `T_BASIC`, không tự cộng skill/credit. Nếu đã nghỉ trước checkpoint thì không nhận. Hiệu ứng này chỉ là narrative exposure; định nghĩa `topic exposure` hiện chưa có chuyển đổi evidence, do đó không thay đổi evidence số cho tới khi rule được author.
- Memory `first_class` được tạo một lần khi `attend` commit; outcome `on_time|late`, actor player; Hạnh/Minh không tự thêm vào memory actors. Player có thể rời sớm, memory vẫn ghi tham dự vì đã điểm danh, không tự ghi học đủ.

## Hạnh và Minh

Nếu late và Hạnh có mặt khi attend: “Em vào chỗ nhé, lát nghỉ xem lại phần đầu.” Hạnh chỉ biết player vào muộn; không biết vì sao. Player có thể kể “Em lỡ xe” nếu `missed_bus_id` tồn tại, commit `hanh_knows_bus`; từ chối kể không ảnh hưởng attendance. Đây là dialogue không tốn thời gian và không tự đổi trust.

Player có thể chủ động dừng tiết trước 10:00 để chào Minh theo `61`: bắt đầu đúng 09:55, mất 5 phút E−1; chỉ completion ghi `met_minh` và `MEM_MEET_MINH`. Có cơ hội thay thế ở thư viện D36; không auto gặp tại Y2.

## Điều kiện biên / recovery

D8 07:50 vào lớp, chọn wait, ở lại tới 08:00 và chọn attend → on_time. Chọn wait rồi rời lúc 07:59 → không điểm danh; quay lại 08:10 và attend → late. Không attend tới 10:00 → absent, không sinh `first_class`. Đến lớp 09:45, attend → late nhưng không đủ 30 phút tới checkpoint 10:00, không nhận exposure.

Fixture `49_D8_phuong_an_di_hoc.md` bổ sung chuyến 07:50, đi bộ và hỗ trợ D8; không copy ngầm D9. Các nhánh vẫn chưa được kiểm chứng trong runtime. Player vắng vẫn có cơ hội tiếp tục ngày, ghi absent, nhưng nội dung học bù cụ thể chưa author.

## Chưa kiểm chứng

Chưa có event runner, topic exposure schema, attendance/course effect đầy đủ, roster toàn năm và route/energy, policy gián đoạn session tại 0 E hoặc runtime recovery D8. Các case chỉ tính tay, chưa chạy test/simulation/playtest.
