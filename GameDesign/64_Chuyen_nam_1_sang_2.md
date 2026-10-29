# Chuyển từ Năm 1 sang Năm 2 — contract fixture v0.1

Áp dụng D84 `YEAR_RESOLVER` của `34`; mục tiêu là chuyển trạng thái trung thực, không chứng minh lịch hay nội dung Năm 2 hoàn chỉnh. Định nghĩa số liệu là giả lập, chưa triển khai và chưa playtest.

## Điểm vào và preview

D84 18:00, tại ROOM hoặc H6, player chủ động mở panel chuyển năm. Trước mốc không offer; sau mốc panel vẫn khả dụng, không tự chuyển. UI pause clock khi đọc và xác nhận. Y2 fixture bắt đầu D85 08:00, mapping ROOM/H6 và card mở đầu tại `65`; nếu `now` đã qua D85 08:00, destination là `now`, không lùi thời gian, calendar Y2 phải được author trước khi apply. Nếu các entry Y2 hay chính sách những entry bị lỡ chưa có, khóa confirm với `blocked_data`, vẫn cho đóng panel và tiếp tục. Không tự sinh điểm danh hoặc lương trong khoảng skip.

Panel hiển thị source/destination, kết quả từng course đã biết và trạng thái `passed|failed|enrolled/unresolved`, credit ledger, event/deadline đã biết trong khoảng skip, assignment snapshot, reexam registered/missed/completed, tiền và E hiện có, conflict đang mở, ký ức có thể được player xem. Không tiết lộ lịch chưa biết hay thông tin NPC riêng. Chưa đủ data tính credit/course thì hiển thị `unresolved` kèm lý do; không đổi thành fail hay pass. Bước preview không trừ tài nguyên, không quyết định thay player và giữ source revision.

## Lựa chọn

| Lệnh | Effect |
|---|---|
| `cancel` | Đóng preview, không đổi clock hoặc state. |
| `confirm` | Revalidate revision, content version, destination, tất cả occurrence bắt buộc và skip policies đã author; nếu hợp lệ thì tính candidate, xử lý các mốc theo `02`, persist atomically và mới công bố Y2. |
| `inspect_course` | Xem course card 0 phút; quay về preview, không tự đăng ký học lại. |

Nếu preview stale do activity/event khác, yêu cầu refresh và xem lại; không âm thầm commit snapshot cũ. Nếu event bắt buộc có nhánh missed đã author thì preview kết quả sẽ xảy ra; thiếu miss policy hoặc calendar nguồn/đích là `blocked_data`. Optional event chưa biết không được lộ trong preview nhưng vẫn xử lý theo policy đã author nếu có; không random nội dung không có định nghĩa. Player không cần tham gia lẩu hoặc đạt ngưỡng quan hệ để chuyển năm.

## Snapshot và ledger

Candidate gồm `now`, location hợp lệ tại destination theo policy scene transition đã author, active/interrupted runs và quyết định pending (cần resolve trước khi skip hoặc có explicit skip policy), course attempts, attendance, assignment submissions, assessments, course status, credit ledger, topic evidence, energy, wallet/reservations, inventory, NPC relationship/conflict/knowledge, memories, project, events/occurrence ledger, calendar, RNG, contentVersion theo `43`. Không reset E/tiền hoặc tự di chuyển đến ROOM. Nếu chưa xác định vị trí đích hợp lệ, khóa confirm; không tự teleport.

Course NMT đã pass giữ `passed` và 3 credit đã ghi một lần; failed giữ `failed` và evidence/assignment/attempt. Retake registered chưa diễn ra giữ pending nếu còn tương lai; nếu skip qua mốc thi thì chỉ ghi missed khi policy `30` đã author và dữ liệu đầy đủ. Retake missed giữ failed của attempt trước, không cấp credit. Missing assessment/assignment vẫn unresolved; carry vào Y2 kèm nguyên nhân, không coi là fail. Nợ môn chỉ có slot phục hồi nếu calendar Y2 thực sự có; nếu chưa, UI thể hiện chưa có lịch chứ không hứa cơ hội hay chuyển state tự động.

## Transaction và ví dụ

`transitionId=(campaignId,Y1_TO_Y2,version)`, `expectedRevision`, source/destination và calendar contentVersion; candidate xử lý ledger theo `02` trước persist. Save lỗi giữ snapshot trước lệnh, không publish Y2; retry cùng command ID trả kết quả đã commit nếu transaction trước đã thành công. Một transitionId chỉ commit một lần, không duplicate credit/memory hoặc tái xử lý occurrence. Location, clock và snapshot chỉ thay đổi cùng commit.

Ví dụ A: player D84 18:00 ROOM, NMT passed, 3 credit, E=40, wallet=12 M, Long conflict open; dữ liệu destination/skip đầy đủ → confirm tạo Y2 D85 08:00 với cùng các giá trị và conflict, trừ khi một occurrence đã author trong skip thay đổi chúng. Ví dụ B: NMT failed sau miss retake → Y2 giữ failed, không có 3 credit, không giả vờ đã đăng ký học lại. Ví dụ C: final data thiếu → unresolved; nếu policy cho phép mang unresolved và destination đầy đủ thì chuyển kèm nhãn, nếu policy cần đánh giá thì khóa và nêu data thiếu. Không tự chọn giữa hai policy khi content chưa author.

## Kiểm tra cần thực hiện

Cancel; stale preview; pending decision và interrupted run; occurrence trước/đúng/sau destination; actor vắng; 3 credit không cấp hai lần; failed vs unresolved; retake registered rồi missed; destination chưa có scene; save lỗi trước persist và retry sau persist; không lộ event chưa biết; accessibility bằng bàn phím. Tất cả là ca QA đề xuất, chưa chạy. Mới có policy destination ROOM/H6 và entry mở đầu Y2 tại `65`; chưa có calendar Y2 đầy đủ, nên đây chưa là đường chuyển năm được chứng minh chạy được.
