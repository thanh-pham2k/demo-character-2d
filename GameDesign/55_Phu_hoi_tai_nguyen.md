# Phục hồi tài nguyên và chống kẹt — fixture v0.1

Thiết kế giả lập, không đại diện trợ cấp/dịch vụ ngoài đời. Đây là một phương án cần playtest; không khẳng định đã giải quyết mọi soft-lock. Dùng ledger/reservation `29`, save `43`, lịch D8 `49`.

## Nghỉ miễn phí

`A_REST_PUBLIC`: tại ROOM, BUS, H6, LOBBY, LIBRARY hoặc CLASS khi có chỗ nghỉ author; 30 phút, 0M, +15 E khi hoàn tất; interrupt trước completion không cộng E, thời gian đã qua không hoàn. Có thể start E=0 vì cost E=0; không cooldown, clamp 100. UI cho thấy các deadline đã biết có thể lỡ, không hứa đi học đúng giờ. Nếu không có địa điểm/data hợp lệ thì `blocked_data`, không giả định nghỉ ở bất kỳ vị trí nào.

D8 fixture BUS có chỗ nghỉ công cộng 24h. Ví dụ E=0 lúc 07:55: nghỉ đến 08:25 → E=15; chọn support đến H6 08:55 (E−1), đi lớp đến 09:00 (E−1), E=13; có thể late nếu attend trước 10:00. Khi support đã hết window/lượt, đi bộ 40 phút + 5 phút nếu route open và đủ E. Tính tay; chưa có event runner để kiểm chứng. Chờ hết thời gian 10:00 vẫn absent, không có game over.

Giữa travel khi E đạt 0: action interruptible theo `29`, lưu vị trí `routeId+fraction`, elapsed và EApplied; fixture chỉ author `safe_stop` ở BUS/H6, chưa có intermediate rest anchor. Nếu engine chưa có safe_stop hợp lệ, travel fixture phải thiết kế không interruptible và chỉ start khi đủ E; không teleport hoặc tạo chỗ nghỉ ngẫu nhiên. Đây là boundary cần author tiếp cho từng route.

## Bữa hỗ trợ có giới hạn

`MEAL_SUPPORT_D1`: CANTEEN D1 `[17:00,20:00)`, S02 hoặc quầy tự phục vụ fixture; 0M, ăn 30 phút E+5 khi hoàn tất, một suất/player. Nếu Long đã gặp và cả hai thật sự có mặt/available thì có thể ăn chung; memory theo `13`; không giả định Long trả tiền. Khi player thiếu 5M hoặc 8M hiển thị lựa chọn này nếu còn suất, không tự chọn. Hủy trước completion release suất, không cấp E/memory; completion đánh dấu consumed cùng transaction.

Ngoài D1, support meal chưa có calendar → không tự đảm bảo miễn phí mỗi ngày. Cần author nguồn ăn/thu nhập/housing từng giai đoạn trước khi đánh giá economy. E ở đây là thước đo năng lượng gameplay, không thay cho hệ hunger hay mô phỏng sức khỏe thật.

## Phí đăng ký học lại

Voucher `RET_NMT_Y5`/`RET_NMT_Y6` theo `22`/`24`: chỉ dành cho attempt và window tương ứng; 30 phút E−2 xin voucher, 15 phút E−2 đăng ký, 0M khi redeem. Voucher trạng thái `available|reserved|redeemed`, idempotency `(courseAttemptId,voucherId)`. Hủy registration release reservation, không đổi voucher đã redeem của attempt khác; không thể chuyển voucher sang course khác. Player có đủ tiền vẫn có thể chọn voucher trong fixture, nhưng phải dùng thời gian. Nếu 45 phút không thể kết thúc trước/đúng registration close, UI không hứa kịp và hiển thị slot tiếp theo nếu calendar author.

## Bảng trạng thái và minh bạch

| Tình huống | Lựa chọn authored | Kết quả | Còn thiếu |
|---|---|---|---|
| E=0 tại trạm D8 | nghỉ công cộng 30 phút | E+15, thời gian trôi | runtime, availability, travel interrupt |
| <3M tại trạm D8 | đi bộ/support nếu mở | không trừ tiền, có thể muộn | không chứng minh mọi giờ có hỗ trợ |
| <5M tại căn tin D1 | MEAL_SUPPORT_D1 nếu còn | bữa miễn phí, E+5 | lịch các ngày khác |
| <5M học lại NMT Y5/Y6 | voucher theo window | 0M, mất 45 phút | các môn/attempt khác |
| không có income hậu tốt nghiệp | chưa có data | báo thiếu nội dung, không tự cấp tiền | economy và recovery dài hạn |

## QA cần thiết

E0 tại BUS, save/reload giữa rest, thời gian đúng deadline, interruption/voucher reservation, hai purchases cùng held balance, support suất cuối, Long vắng, route đóng. Chưa chạy test/simulation/playtest; thiết kế này không chứng minh toàn campaign không kẹt.
