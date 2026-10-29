# NPC phụ — sự kiện fixture v0.1

Các mốc và giá trị là đề xuất hư cấu. Đây là bốn event có chi tiết hơn, không phải toàn bộ hồ sơ/sự kiện của tám NPC phụ; chưa có runtime hay playtest. Dùng clock, knowledge và transaction tại `02`, `03`, `29`, `43`.

## S01 — Quản lý KTX: báo nước rò

`EV_S01_WATER`: medium, Y1 W03, LOBBY, window `[16:00,18:00)`, `water_issue_known=true`, S01 on duty. Không tự sinh sự cố khi player không biết; trigger từ bảng tin hoặc chứng kiến event đã author. S01 muốn nhận thông tin đủ để đặt lịch sửa, không biết đồ trong phòng trừ khi player nói.

S01: “Cậu thấy nước rò ở đâu? Tớ ghi để nhờ người kiểm tra.”
- `report`: 15 phút E−1; completion tạo `issue_reported`, lịch kiểm tra giả lập W04 10:00–11:00; người sửa tới chỉ nếu calendar author và lối vào hợp lệ. Không xóa issue ngay.
- `later`: 0 phút, chưa ghi report; khi hết ca, nhắc ca sau chỉ nếu calendar có.
- `inspect_first`: activity riêng chưa author; UI không cho chạy trong fixture.

Nếu player vắng giờ kiểm tra, report vẫn còn; không tự phạt quan hệ hoặc sửa thành công. Callback khi S01 biết report: “Mình đã ghi lịch kiểm tra.” Không cho S01 nói đã sửa nếu thiếu `repair_completed`. Không ảnh hưởng credit; memory `water_report` chỉ khi report hoàn tất. Chưa có record repair, nên issue vẫn unresolved.

## S02 — Nhân viên căn tin: món gần hết

`EV_S02_MEAL`: small, Y1 D1 `[17:00,20:00)`, CANTEEN, S02 available, menu fixture còn `MEAL_A` 8M và `MEAL_B` 5M. Inventory trong event snapshot, không tự refill sau save/load. “Món A còn một phần; món B thì vẫn còn.”
- `meal_a`: reserve 8M + một phần hàng trong cùng transaction, ăn 30 phút E+5 completion; capture tiền/hàng sau completion. Hủy/interruption release cả hai, không cấp E.
- `meal_b`: tương tự giá 5M, không đánh dấu đã chọn món A.
- `leave`: 0 phút; nếu không đủ tiền, chỉ hiển thị meal support nếu được author và còn window.

Long có thể cùng ăn chỉ khi đã gặp, tới CANTEEN và available; không đặt `first_meal_long` từ event một mình. Không tự phát romance/friendship hay +XP. Nếu cả hai cùng chọn phần cuối trong một state, expectedRevision giúp chỉ một reservation commit; người thua được xem lựa chọn còn lại. Callback S02 chỉ nhớ món nếu NPC biết và event memory có actor S02. Chưa author restock/route/cảnh phục vụ.

## S03 — Tài xế: xác nhận tuyến

`EV_S03_ROUTE`: small, D9 tại BUS khi xe dừng và cửa mở; `tripId` phải tồn tại, không chặn quyết định boarding ở deadline. Tài xế: “Xe này đến H6. Cửa đóng lúc 7 giờ 40.”
- `ask`: 0 phút trong màn hình thông tin trước lượt boarding; đánh dấu `route_known` nếu chưa biết, không tự mua vé.
- `board`: thực hiện transaction trong `29`, 3M; không tự tạo memory trò chuyện nếu chỉ đọc bảng.
- `skip`: đóng thông tin, vẫn phải quyết định lên/không lên.

Không cho tương tác nếu xe chưa đến hoặc đã đóng cửa; không trì hoãn departure theo thời gian thực. Không tự nói player lỡ xe trước đó. Route D9 là fixture; các chuyến khác cần data riêng.

## S05 — Sinh viên mới: tìm thư viện

`EV_S05_LIBRARY`: medium, Y5 W02 `[09:00,11:00)`, H6, S05 available, player biết LIBRARY; nếu chưa biết thư viện, NPC hỏi quầy chỉ dẫn thay, không tự cấp kiến thức tuyến.

S05: “Anh/chị biết thư viện đi hướng nào không? Em đang tìm phòng tự học.”
- `walk`: 15 phút E−2, event hoàn tất nếu cả hai còn available; actor S05 và player được ghi vào memory `helped_freshman`; chuyển vị trí chỉ khi route được author.
- `point`: 5 phút E−1, tạo memory outcome `directions`; không tự di chuyển S05.
- `decline`: 0 phút, không trừ quan hệ. `leave`: hết window expired, không tạo memory.

Nếu player có `first_class` memory, player có thể nhớ lần đầu tìm lớp; S05 không thể nhắc memory đó nếu không biết. Ở lễ tốt nghiệp, callback chỉ khi S05 được mời hoặc sự kiện có S05 thực sự tham gia; không tự gán roster. S05 có lịch riêng và có thể không xuất hiện lại.

## QA / chưa đủ

Tất cả là fixture, chưa có calendar/route/inventory runtime, restock, repair follow-up, policy support meal hay localization. Cần kiểm tra race reservation, NPC vắng, pending choice tại departure, save giữa activity và memory actor/knowledge. S04/S06/S07/S08 và các scene còn lại chưa có records đủ; không coi 8 NPC phụ đã hoàn thiện.
