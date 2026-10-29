# Bữa lẩu cuối Năm 1 — fixture event v0.2

Bản này cụ thể hóa `EV_Y1_HOTPOT` trong `13` cho fixture D71; chi phí và lịch là giả định thiết kế, chưa cân bằng, chưa có runtime. Lẩu là một cơ hội hội tụ ký ức, không phải điều kiện tốt nghiệp hay sự kiện tự bắt buộc.

## Calendar và thông tin

`HOTPOT_PLAN`: D71 [18:00,22:00), ROOM, một occurrence/campaign; priority 40. Khi vào ROOM trong cửa sổ và `room_entered=true`, player nhìn thấy tờ ghi chú chung về bữa cuối năm, biết sự kiện và giá/điều kiện fixture dưới đây; player không cần từng gặp Long. Ghi nhận `knownAt=now`, không lộ lịch hoặc chi tiết trước đó. Nếu vắng ROOM cả cửa sổ: expired, không trừ quan hệ, năm vẫn tiếp tục. 22:00 xử lý activity hoàn thành đúng mốc trước khi đóng cửa sổ (`02`); không cho bắt đầu thao tác không thể hoàn tất trước mốc.

`LONG_HOTPOT_AVAIL`: D71 [18:00,22:00), ROOM nếu `met_long`; `MINH_HOTPOT_AVAIL`: D71 [19:00,21:30), ROOM nếu `met_minh`. Đây chỉ là availability cho fixture, không mặc định actor xuất hiện hay đã nhận lời. Tại ROOM, player có thể mời từng người 0 phút, chỉ khi actor đã gặp và đang available. Long: “Nếu cậu chuẩn bị thì tớ ăn cùng nhé.” Minh: “Mình có thể ở đến chín rưỡi.” Nhận lời lưu guest confirmation, không tăng trust, không tạo memory. Actor chỉ là attendee khi được mời và có mặt lúc bắt đầu ăn; không tự dịch chuyển Minh từ nơi khác. Cần mô hình actor presence/route thực tế trước production; fixture giả định actor available đang ở ROOM khi player mở lựa chọn mời. Player có thể bỏ qua hoàn toàn mời khách.

## Điều kiện, đạo cụ và lựa chọn

Fixture cung cấp `POT_SHARED` và `COOKER_SHARED` dùng chung trong ROOM, không thuộc inventory player, không mua/tiêu hao. Chúng tồn tại chỉ với content version fixture này. Chọn một trong các nguồn nguyên liệu:

| Phương án | Bắt đầu và preview | Commit |
|---|---|---|
| `prepare_standard` | ROOM, bếp/nồi sẵn; reserve 12 M, 20 phút, E −2; nêu bữa 60 phút +5 E và guest đã xác nhận. | Hết 20 phút capture 12 M, `prepared=standard`; ingredients dùng ngay, không sinh inventory dùng lại. |
| `prepare_support` | ROOM; không cần đủ tiền, gói hỗ trợ KTX đã author cho fixture, 0 M, 20 phút, E −2; không gán người trả thay. | `prepared=support`; cùng lượng bữa và guest capacity. |
| `decline` | 0 phút khi chưa chuẩn bị; thông báo sẽ bỏ dịp này. | Resolve declined, không đổi chỉ số, không sinh memory. |
| `later` | 0 phút, đóng panel; có thể mở lại trước khi hết cửa sổ. | Không tạo memory, không trừ chi phí. |

Chuẩn bị cần E ≥2; nếu không đủ, hướng dẫn player nghỉ theo activity đã biết, không tự cộng E. Nếu bị ngắt: tiêu E theo elapsed `floor(2×elapsed/20)`, nhả reservation chưa capture; không tạo `prepared`. Khi chuẩn bị xong, tiền và thời gian không hoàn lại nếu player không ăn; có thể bắt đầu ăn sau đó trong cùng cửa sổ. Đồ chuẩn bị hết hiệu lực khi occurrence đóng; không suy ra inventory/đồ thừa.

Sau prepare, `join_meal` 60 phút, E +5 khi hoàn thành (giới hạn 100), 0 M, cần ROOM, `prepared` và thời điểm kết thúc ≤22:00. Player có thể ăn một mình hoặc với attendee đã xác nhận và có mặt trong toàn lượt. Không có giới hạn khách trong fixture vì tối đa hai khách trên; không hứa có NPC khác. UI liệt kê khách đủ điều kiện và giờ kết thúc, có thể bỏ khách mà không trừ trust. Nếu khách không thể ở tới khi lượt ăn kết thúc, họ không được chọn làm attendee, nhưng player vẫn ăn một mình hoặc với người khác. Nếu availability thay đổi giữa lượt, dừng activity, giữ thời gian đã trôi, không cộng E hoặc memory; player quyết định có bắt đầu lại với attendee còn đủ điều kiện, nếu đủ thời gian. Không thu phí lại và không tự hoàn lại chi phí prepare. Nếu chuẩn bị xong không kịp ăn, hiển thị rõ và không cho bắt đầu.

## Kết quả, thoại, dữ liệu

Completion atomically consume `prepared`, cộng E, ghi một memory `MEM_Y1_HOTPOT` với `actors=[PLAYER,...attendees]`, `knownBy` giống actors, outcome `standard|support`, timestamp completion và event instance ID. Không tự cộng trust hay đổi conflict, academic, career; không gọi một bữa solo là bữa chung. Callback Long trong `44` được chọn tại completion nếu Long là attendee có mặt đến mốc hoàn tất; chọn một dòng theo ưu tiên, không tự giải quyết conflict. Completion đúng 22:00 được xử lý trước khi availability đóng, nên callback vẫn đủ điều kiện; lưu nội dung đã chọn trong cùng transaction và hiển thị sau commit, kể cả khi giờ đã sang mốc rời. Minh chưa có dialogue callback được author: nếu Minh là attendee có mặt đến completion, có thể nói “Hết năm đầu rồi nhỉ. Ăn lúc còn nóng đi.” không nhắc điểm hay việc riêng chưa được chia sẻ. Khi Long và Minh cùng tham dự, cả hai có thể có một lượt thoại, không dùng dialogue của người vắng.

Outcome sau completion `joined`; hết window chưa ăn `expired_prepared` hoặc `expired_unprepared`; bỏ trước prepare `declined`. Không ghi memory cho prepare/đã mời, không giả định biết kết quả thi hay Long đã tốt nghiệp. Các outcome giữ year resolver khả dụng khi các điều kiện riêng của resolver được đáp ứng. Không tự chạy hoặc skip sang năm 2.

## Lưu và ranh giới

`instanceId=(campaignId,HOTPOT_PLAN,version)`, lưu trạng thái event, `knownAt`, guest confirmations, prepare run, `prepared`, reservation, meal run và attendee snapshot. Start prepare và complete prepare là transaction riêng theo `29`,`43`; meal completion là transaction riêng có idempotency key `(instanceId,meal-complete-v1)`. Retry cùng command trả outcome đã commit; nếu save lỗi, giữ snapshot trước transaction và không hiển thị success. Với meal interrupt, persist elapsed và kết quả dừng atomically; chỉ one running activity, không tự resume. Check availability lại lúc chọn guest, lúc start và mọi mốc lịch trong activity; không dùng lời mời thay cho availability.

## Trace và kiểm tra cần thực hiện

Giả sử D71 18:00 ROOM, E=50, wallet available=15 M, đã gặp Long, Long hiện diện, không có Minh: mời Long → prepare standard tới 18:20, E=48, wallet=3 M → meal tới 19:20, E=53 → một memory có player và Long, một callback hợp lệ theo `44`. Chọn support thay thế giữ 15 M. Người chưa gặp Long vẫn có thể chuẩn bị và ăn một mình. Tại 21:00 đã prepare, một bữa có thể hoàn thành đúng 22:00; tại 21:01 không được start. Chuẩn bị từ 20:41 tới 21:01 cũng không còn đủ 60 phút: UI phải cảnh báo trước khi xác nhận prepare.

QA cần kiểm tra: actor chưa gặp/vắng/availability unknown, mời trước và sau ca Minh, Long conflict mở, memory không có hoặc không biết, wallet=11/12 M và reservation khác, E=1/2/100, ngắt prepare/meal, actor rời 21:30, bữa solo, 22:00 boundary, save lỗi và retry, chuẩn bị rồi hết hạn, year resolver và cả hai loại nguyên liệu. Traces trên là tính tay; chưa simulation, test hoặc playtest. Cần quyết định route NPC và dữ liệu bữa hỗ trợ ngoài fixture trước khi gọi event production-ready.
