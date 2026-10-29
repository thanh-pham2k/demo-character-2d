# Chuẩn bị thi cuối C_NMT — fixture D57 v0.1

Đặc tả cho một môn hư cấu, nối `12`, `14` và calendar `34`. Điểm số, lịch và chi phí cần cân bằng; không đại diện quy định trường thật. Thiết kế chưa được triển khai hoặc playtest.

## Công bố lịch và thông tin

`FINAL_NOTICE_NMT` D57 18:00, course attempt enrolled: công bố lịch final D64 [08:00,09:00), LOC_CLASS; card cho biết final 70%, ASG_01 30%, pass ≥50, midterm chỉ feedback và chỉ cần enrolled để đủ eligibility theo `14`. Ghi knownAt tại thời điểm công bố, không yêu cầu mở panel, không tự di chuyển player. Attendance unresolved hiển thị unknown và không mặc định absent; missing assignment hoặc missing rubric hiển thị unresolved, không suy diễn 0. Nếu course definition thiếu thì panel đánh dấu `blocked_data` và không hứa eligibility. Entry `FINAL_PREP` D57 [18:00,22:00) tại ROOM/LIBRARY là cơ hội mở panel, không bắt buộc.

## EV_Y1_FINAL_PREP — Có bốn giờ, ôn gì?

- **Loại:** academic, priority 50, một occurrence; trigger khi player chủ động mở course card ở ROOM/LIBRARY trong window và enrolled. Không auto pause activity đang chạy hoặc tạo decision cưỡng bức. Minh/Long không bắt buộc và không tự có mặt.
- **Mục tiêu:** chọn giữa ôn một chủ đề để có hint, nghỉ để hồi E hoặc giữ thời gian cho việc khác. ASG_01 đã hết cửa nộp D22, D57 không cho làm lại bản nộp trong cùng attempt; card cho biết record thật và đường thi lại chỉ khi đã được công bố, không hứa sẽ được thi lại khi chưa có lịch.
- Panel 0 phút hiện topic evidence/nhãn, prerequisite, final timing/weights và các lịch đã biết; không hiển thị đáp án hay đề thi chưa công bố. Player được đóng panel và mở lại đến trước 22:00. Không tự đặt outcome hoặc memory khi xem card.

| Lựa chọn | Preview | Hoàn tất |
|---|---|---|
| `study_basic` | A_STUDY 60 phút, −10 E, 0 M; topic T_BASIC mở và E ≥10. | +10 evidence T_BASIC, tối đa 100. |
| `study_control` | Như trên; T_CONTROL chỉ học khi T_BASIC ≥20. | +10 evidence T_CONTROL, tối đa 100. |
| `rest` | 30 phút, +15 E lúc hoàn tất, tối đa 100; chỉ nơi được phép nghỉ theo activity definition. | Không đổi knowledge/điểm. |
| `leave` | 0 phút. | Đóng panel, không thay state học tập. |

Mỗi lượt được player xác nhận riêng; nếu cùng location và activity cho phép, có thể làm nhiều lượt trong window. Kết thúc đúng 22:00 được nhận completion theo `02`; không start qua mốc đối với activity được offer bởi event này. Hoạt động học cùng ID có thể tồn tại ngoài event theo definition riêng, không mặc định bị cấm sau 22:00. Không offer `rest` ở LIBRARY nếu definition nơi nghỉ chưa author; UI ghi rõ. Không offer hỏi Hạnh khi chưa có lịch/presence thực tế.

`A_STUDY` tiêu E lũy kế floor(10×elapsed/60), chỉ cộng evidence khi hoàn tất; dừng giữa lượt giữ elapsed/cost, không cộng evidence. Nghỉ bị ngắt trước completion không cộng E. Một run chỉ có một completion transaction, key `(activityRunId,completion-v1)`, chống nhân evidence/E khi retry; save/load tuân thủ `43`. Event đóng lúc 22:00 không xóa evidence hoặc thay đổi exam eligibility.

## Tác động có thể giải thích

Khi topic evidence ≥30 tại lúc mở hint, mỗi topic có tối đa một hint theo `14`; evidence không tự thành đáp án hay điểm thi. Ví dụ T_BASIC=20, T_CONTROL=20, E=40: player chọn ôn control 18:00–19:00, E=30, control=30; sau đó nghỉ 19:00–19:30 tại ROOM, E=45; ôn basic 19:30–20:30, E=35, basic=30. Đây là khả năng mở hint cho cả hai topic, không bảo đảm pass. Chọn làm việc khác thay vì ôn vẫn được dự thi nếu enrolled.

Nếu bỏ lỡ final: `missed_final` và finalPoints=0 chỉ khi đầy đủ course definition `14`; assignment resolved 100 cho courseScore=30 nên failed, không game over. Thi lại `30` cần đăng ký trong lịch và có điều kiện riêng; khi thiếu data giữ unresolved. Event chuẩn bị không tự đăng ký hay hứa recovery, và không tự kể kết quả cho NPC.

## Kiểm tra cần thực hiện

Cần kiểm tra course chưa enrolled, notice khi player ngoài phòng, evidence 19/20/29/30/95, prerequisite, E=9/10/100, interruption, start 21:00/21:01, clock đúng 22:00, unresolved assignment, missed final, save lỗi/retry, nghỉ ở thư viện không có definition, chưa có Hạnh. Trace chỉ tính tay; chưa có runtime, simulation hoặc playtest. Tuyến D64 là fixture tại `63`; các môn còn lại và route từ mọi location vẫn phải author.
