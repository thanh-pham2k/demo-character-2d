# Hệ thống thời gian và sự kiện — đề xuất v0.1

Đây là thiết kế thử nghiệm, chưa cân bằng hoặc triển khai. Mục tiêu: thời gian hữu hạn tạo trade-off rõ ràng và cho phép sửa kế hoạch khi xuất hiện thông tin mới, không gây phạt ẩn.

## State và lịch

- `now`: phút nguyên từ đầu campaign; `day`, giờ địa phương được suy ra theo calendar campaign, không dùng đồng hồ máy.
- `calendarEntry`: `id`, loại (class, transport, deadline, exam, invitation, story), `[start,end)`, địa điểm, điều kiện, `knownAt`, trạng thái `scheduled | active | completed | missed | cancelled`, kết quả. Lịch chỉ hiển thị những entry player đã biết.
- `activityRun`: `id`, lựa chọn, start, plannedEnd, elapsed, trạng thái `running | interrupted | committed | cancelled`, reservation, kết quả từng phần. Không cho hai hoạt động chiếm thời gian chạy đồng thời.
- `eventInstance`: `instanceId`, `eventId`, trigger time, priority, điều kiện, deadline, trạng thái `eligible | offered | resolved | expired`; instanceId dùng để chống xử lý lặp khi save/load.

## Quy tắc thời gian

Thời gian chỉ tiến khi hoạt động hoặc travel xác nhận. Xem map, inventory, lịch, dialogue choice và menu pause đều không tiêu tốn thời gian. Không có deadline chạy theo thời gian thực trong menu; bộ đếm thi là ngoại lệ, được công bố ở UI và lưu riêng.

Khi xác nhận hoạt động, hiển thị bắt đầu, thời lượng, kết thúc, chi phí biết trước và những lịch đã biết có khả năng xung đột. Di chuyển dùng thời gian giữa hai location; cùng phòng có thể miễn phí. Nếu đến trạm trước hoặc đúng thời điểm xe rời, player có một lượt quyết định boarding tại mốc đó khi xe/cửa sẵn sàng; chỉ sau khi lượt quyết định kết thúc và bước đóng cửa đã commit mới xác định lỡ chuyến đã chọn. Giờ xe và thời gian di chuyển phải được author cho từng tuyến, không hard-code mặc định toàn campaign.

Một hoạt động từ t0 đến t1 xử lý các mốc trong khoảng **(t0,t1]** theo thời gian, rồi theo loại: (1) hoàn tất hoạt động tại mốc, (2) mở lượt quyết định boarding nếu có xe sắp đóng cửa và player đủ điều kiện, (3) sau khi lượt quyết định đóng, xử lý deadline/đóng cửa, (4) khởi tạo và cập nhật event, (5) thông báo, (6) ghi nhận missed khi hết cửa sổ. Quy tắc boarding là ngoại lệ có thời điểm dừng rõ, không áp dụng chung cho mọi deadline. Tại cùng timestamp, ví dụ nộp bài hoàn tất đúng deadline được tính đúng hạn; đến lớp đúng lúc bắt đầu là đúng giờ; đến đúng lúc kết thúc không thể điểm danh. Những quy ước này phải thống nhất trên UI, save và event.

Nếu event cần quyết định trước t1: tạm dừng activityRun tại event time, lưu elapsed và reservation; player chọn tiếp tục, dừng, hoặc thay kế hoạch. Dừng không hoàn lại thời gian đã trôi; hiệu quả từng phần chỉ áp dụng khi activity định nghĩa checkpoint, mọi chi phí chưa tiêu thụ được nhả. Event trùng thời điểm: bắt buộc/deadline trước optional, sau đó theo `priority` giảm dần rồi `eventId` tăng dần; event không hợp lệ sau lựa chọn trước đó bị bỏ và ghi lý do. Không tự chọn thay player cho event đòi hỏi lựa chọn.

## Time skip và save

Preview skip cho thấy nguồn/đích, thời lượng, mọi deadline, exam và scheduled event đã author có nguy cơ bị lỡ; thông tin chưa biết không tiết lộ. Có cancel không thay đổi state. Commit tạo snapshot dự kiến, xử lý event đã khai báo đúng thứ tự và ghi save atomically một lần; save lỗi giữ nguyên snapshot nguồn. Không tự sinh lịch học hoặc điểm danh trong khoảng trống. Sau khi commit, recap ghi kết quả, không cấp lại reward khi load lại.

Save ghi clock, activityRun, event instances, lịch, reservation, nguồn RNG, phiên bản schema. Load tại giữa event đưa về cùng choice đã biết; idempotency key gồm instanceId và outcome version. Kiểm tra: hủy skip không đổi hash; nộp đúng deadline được tính đúng hạn; hai event trùng giờ có thứ tự ổn định; load trước/sau commit không nhân đôi reward; save lỗi không nhảy thời gian. Đây là các ca QA đề xuất, chưa chạy.

## Sự kiện

Định nghĩa gồm ID, loại, actor, location, thời gian, trigger, preconditions, cờ bắt buộc/cấm, priority, cooldown, choice, cost, effect, memory, follow-up, miss, recovery, presentation và localized text. Eligibility phải kiểm tra năm, thời gian, vị trí, actor availability, điều kiện và cooldown; không có eligible event thì không ép random. Pool có seed lưu trong save, chỉ dùng random để chọn trong nhóm cùng priority sau khi lọc; event bắt buộc không random rơi mất.

Invitation bị bỏ lỡ có thể thay đổi availability hoặc thoại nhưng không tự giảm quan hệ nếu chưa có rule author. Event chỉ tạo ký ức khi outcome của nó đã commit. Accessibility: timeline hiển thị bằng chữ, không chỉ màu; menu nêu giá trị chắc chắn và hậu quả đã biết.

## Giao điểm hệ khác và rủi ro

Thời gian tác động attendance, stamina, chi phí đi lại, lịch NPC và deadline; academic và relationship không tự tăng chỉ do clock tiến. Đề xuất playtest các phiên 10–15 phút; đo số lựa chọn thực, số hoạt động có thể bỏ qua và số lỗi dự đoán trước khi chốt tốc độ. Cần author route, calendar, luật interruption theo hoạt động và thử các boundary trước production.
