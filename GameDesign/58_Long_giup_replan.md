# Long nhờ giúp giữa buổi làm bài — fixture thiết kế v0.1

Fixture này author một tình huống cho `57`, không chứng minh hệ thống đã chạy hoặc nội dung toàn năm đã đủ. Các con số là giả định để kiểm toán, cần playtest.

## EV_Y1_D22_LONG_HELP

- **Loại:** NPC invitation, một lần theo occurrence; năm 1, D22 [18:30,18:40), ROOM; priority 50 sau event bắt buộc cùng mốc. Calendar occurrence `D22_LONG_HELP`, chỉ được biết khi Long gặp player; không hiện trước trên kế hoạch. Long có mặt tại ROOM [18:30,19:00] riêng trong fixture này; lịch ngoài khoảng đó chưa được xác định.
- **Trigger:** đến 18:30 hoặc player vào ROOM trong cửa sổ; `met_long=true`, player và Long tại ROOM, Long available, chưa resolve occurrence. Không cần assignment đang chạy. Nếu thiếu dữ liệu availability thì `blocked_data`, không mặc định Long có mặt. Có thể offer sau khi xung đột priority cao kết thúc nếu vẫn trong cửa sổ và predicate còn đúng. Mỗi occurrence chỉ hiện một lần; không gặp hoặc hết cửa sổ thì expired, không trừ quan hệ.
- **Mục tiêu:** quyết định có dành nửa giờ giúp Long chuẩn bị buổi thuyết trình nhóm hay giữ thời gian cho việc của mình. Long cần rà thứ tự slide và tập nói, không mặc định biết deadline hay trạng thái bài của player.
- **Thoại mở:** Long: “Cậu rảnh xem giúp tớ thứ tự slide không? Tớ muốn tập nói một lượt trước khi gửi nhóm.” Nếu player đang làm bài, player thấy riêng tiến độ và giờ nộp đã biết; Long không nhắc đến deadline nếu chưa được báo.

| Choice | Preview và điều kiện | Kết quả |
|---|---|---|
| Giúp ngay | 30 phút, E −3, 0 tiền; cả hai ở ROOM, Long rảnh đến hết lượt và E ≥3. Tạm ngắt activity được hỗ trợ resume, hoặc kết thúc nó theo policy riêng trước khi start. | Khi hoàn thành, trust Long +2 giới hạn 100; tạo memory `MEM_LONG_SLIDES_D22`, `actors=[PLAYER,LONG]`, `knownBy=[PLAYER,LONG]`, outcome `helped`, timestamp completion. Không tự thay đổi bài tập, knowledge, career hay conflict. |
| Không lúc này | 0 phút; Long: “Ừ, để tớ thử tự sắp xếp.” | Resolve `declined`, không đổi trust, không memory. Player có thể tiếp tục run bị ngắt nếu còn hợp lệ. Không tự tạo lời hẹn hay event khác. |
| Xem lại kế hoạch | 0 phút, quay lại cùng decision; không resolve invitation. | Xem lịch và deadline đã biết; không đổi tài nguyên. Muốn rời decision phải chọn một trong hai lựa chọn trên. |

Do availability trong fixture kết thúc lúc 19:00, muốn giúp đủ 30 phút phải bắt đầu đúng 18:30; nếu event được offer muộn hơn thì không còn đủ lịch và nút giúp bị khóa. Khi Long không còn available hoặc E không đủ, nút giúp bị khóa và nói rõ lý do đã biết; từ chối vẫn khả dụng. Action giúp không bị ràng buộc với assignment: có thể giúp ngay cả khi player đang rảnh. Dừng help trước phút 30 giữ thời gian và E đã tiêu theo `29` (`floor(3×elapsed/30)`), không tạo memory/trust; occurrence kết thúc `interrupted`, Long tiếp tục việc riêng. Không có lời hứa retry trong fixture. Assignment đã ngắt vẫn có thể resume theo điều kiện dưới đây.

## Hợp đồng ngắt và khôi phục

Trong fixture này mỗi milestone ASG_01 là một activityRun cho phép `interrupted → running` với cùng runId. Sau khi player chọn giúp, run assignment được lưu thành interrupted, không chiếm slot running; help có runId riêng. Chỉ một run `running` tại một thời điểm. Resume thủ công tại ROOM/LIBRARY khi record chưa nộp/missed, giữ `elapsed` và `energyApplied`; không thu phí lần nữa hoặc hoàn thành milestone trước khi đủ 45 phút. Nếu không thể resume vì nộp partial hoặc hết deadline thì hiển thị lý do và đóng run, giữ thời gian/E đã dùng. Hành vi này là đề xuất bổ sung cho riêng ASG_01; không suy rộng cho mọi activity.

Save gồm run assignment interrupted, run help nếu đang chạy, event instance/choice, scheduler cursor và effect keys theo `43`; commit completion help atomically, chống lặp trust và memory. Save lỗi giữ snapshot trước transaction, retry không nhân hiệu ứng. Giúp xong không tự resume hoặc chạy kế hoạch: player xác nhận lại; plan đã lưu đánh dấu stale khi mốc kết thúc không còn đúng.

## Trace tính tay có điều kiện

Giả sử player đã gặp Long, ROOM, E ≥17, ASG_01 overdue chưa làm milestone nào và chọn giúp ngay:

| Mốc | Kết quả |
|---|---|
| 18:00–18:30 | Milestone 1 chạy 30/45 phút; đã trừ floor(7×30/45)=4 E; event pause. |
| 18:30–19:00 | Help hoàn tất, −3 E; trust +2 và memory được commit. |
| 19:00–19:15 | Player xác nhận resume cùng runId; trừ thêm 3 E, milestone 1 hoàn tất. |
| 19:15–20:00 | Milestone 2 −7 E, hoàn tất. |
| 20:00–20:05 | Submit 5 phút, 0 E; bản đủ milestone được nhận late trong cửa sổ D22. Tổng chi phí 17 E, không tính các hoạt động khác. |

Đây là một lịch khả thi theo giả định, không bảo đảm mọi state hồi phục. Từ chối rồi tiếp tục ngay: nộp 19:35, tổng −14 E. Nếu player học thêm 60 phút sau khi nộp, phải có thêm 10 E và đủ điều kiện topic; kết thúc 21:05 trong trace giúp. Deadline và kiểu nộp theo `12`.

## Follow-up, miss và QA

Callback được author trong lựa chọn thoại `EV_Y1_HOTPOT` tại `13` và `44`: chỉ khi bữa lẩu thực sự diễn ra, Long có mặt và biết memory; conflict đèn mở được ưu tiên hơn nên có thể không phát thoại slide. Không giúp/không gặp không tạo callback giả. Bữa lẩu vẫn phụ thuộc dữ liệu lịch, mời khách và chuẩn bị chưa hoàn chỉnh; callback không được coi là bảo đảm xuất hiện. Deadline assignment không thay đổi; bài có thể nộp partial hoặc missed và theo recovery `12`.

Ca cần kiểm tra khi có runtime: vào ROOM đúng 18:30/18:40; event trùng deadline; Long vắng/availability unknown; E=2 hoặc E=3; decline sau pause; giúp khi không làm assignment; ngắt help ở phút 10; resume assignment lặp; nộp partial trước resume; load trước/sau completion; save lỗi; event đã resolve không xuất hiện lại; UI không tiết lộ deadline cho Long. Chưa chạy simulation, test hoặc playtest.
