# Năm 1 v2 — 10 milestone và hậu quả học tập

## Phạm vi

Áp dụng cho scene đang dùng `Room404_MVP`. Mở rộng `YearOneProgress`, `YearOneSchedule`, controller và store hiện có. Không thay pipeline truyện riêng `Production_Opening`, không sinh ảnh mới và không triển khai học lại Năm 2.

## Timeline và dữ liệu

`YearOneCalendar.cs` khai báo mười milestone, ngày lịch tuyệt đối, học kỳ/tuần, lịch học và lịch thi. `milestoneId` và `absoluteMinutes` là state lưu thật; `day`/`clockMinutes` chỉ là accessor tương thích, không có đồng hồ lưu thứ hai.

| Milestone | Ngày lịch |
| --- | ---: |
| YEAR1_ARRIVAL | 0 |
| HK1_FIRST_CLASSES | 1 |
| HK1_GAMING_CHOICE | 2 |
| HK1_MIDTERM | 29 |
| HK1_PREPARATION | 92 |
| HK1_FINAL_EXAMS | 99 |
| HK2_START | 113 |
| HK2_DORM_LIFE | 155 |
| HK2_PREPARATION | 204 |
| HK2_FINAL_EXAMS | 211 |

Mỗi HK: Giải Tích 6 TC, Vật Lý 5 TC, Tin/Lập trình 5 TC. Đây là quy đổi trò chơi. Mỗi môn có hai buổi, bài tập bắt buộc, ôn tập và kỳ thi ba câu. Đúng 2/3 là PASS; vắng tối đa một buổi và bài tập nộp trước hạn là điều kiện dự thi. Ôn tập không khóa quyền thi.

Deadline: ngày lịch 92 lúc 22:00 cho HK1, ngày lịch 204 lúc 22:00 cho HK2. Thi theo ba khung 08–09h, 10–11h, 13–14h. Mỗi câu có 20 giây thực; thế giới tạm dừng trong bài thi. Nộp sớm tính câu chưa trả lời là sai. Save lưu câu trả lời, chỉ số câu và thời gian còn lại.

## Nguyên tắc progression

- Điểm danh khi ngồi trong lớp; chờ nếu sớm. Tới hết phút 15 là đúng giờ; sau đó là trễ; hết tiết chưa điểm danh là vắng.
- Buổi đã điểm danh không trở thành vắng vì còn đang làm câu hỏi.
- Học bù tạo lịch sử khắc phục riêng, không xóa vắng hoặc đổi FAIL thành PASS.
- Môn chỉ thành nợ sau kết quả FAIL. Hết khung thi chưa tham gia sẽ ghi bỏ thi/không đủ điều kiện.
- Tín chỉ được tính từ kết quả PASS duy nhất theo course ID; tổng kết không cấp thưởng.
- Ngủ không yêu cầu hoàn thành bài, đạt môn, đi xe về hoặc chơi cùng Long. Lên giường chỉ nằm nghỉ; nút Ngủ mở preview rồi mới xử lý hậu quả.
- Preview không thay state. Commit chuyển ngày dùng bản sao, chỉ công bố sau khi save thành công; lỗi lưu giữ nguyên milestone cũ.
- Skip xử lý lịch/thi/deadline đã khai báo trong khoảng chuyển, không sinh buổi hoặc thành tích ngoài màn hình.
- Ngày cuối: ngủ bỏ qua thi sẽ xử lý phần còn lại tới cuối ngày và vẫn ở milestone cuối để dự lẩu. Sau lẩu, lần ngủ tiếp theo bàn giao Năm 2; không thêm ngày gameplay thứ 11.
- Ăn lẩu không đòi 32 TC. Snapshot Năm 2 giữ kết quả/TC HK1–HK2, PASS, FAIL, lý do và toàn bộ history. UI hiển thị môn nợ; không có học lại ở bản này.

## Gameplay và UI

- Chơi game ẩn Windows, giữ phiên đang chạy. Trong phòng có nút thoát và mở lại Windows. Mở Windows tạm dừng; Về phòng tiếp tục phiên; Tắt máy kết thúc phiên.
- Sinh hoạt, đi bộ, chờ/đi xe và học bù x1 phút/giây; chơi game x4. Không cộng giờ khi mua vé hoặc về phòng. Hoàn thành tiết học đưa giờ tới cuối tiết nếu cần.
- HUD hiện mốc học kỳ, thứ/ngày, giờ, hoạt động, tiết tiếp theo, giờ bắt đầu–kết thúc, countdown và ước tính xuất phát theo marker/tốc độ/animation xe hiện có.
- Chuyến đầu ngày 7 dùng mốc calendar 07:40; countdown cũ là accessor phút game, không có timer lưu độc lập. Giờ nên xuất phát xét cả giờ xe và giờ học.
- Nhắc 30/15/5 phút được ghi theo session; qua nhiều mốc chỉ hiện mức khẩn cấp nhất. Card không xếp chồng; cập nhật cảnh báo khẩn hơn và giữ thêm 5 giây, trừ khi người chơi chủ động đóng.
- Lịch học có Hôm nay/Học kỳ, tín chỉ thực, kết quả môn, lịch buổi, điểm danh và tình trạng bài tập. Thi tại lớp; UI học ở bàn không cấp kết quả thi.
- Ánh sáng nội suy theo giờ. Menu bàn cũ được ẩn khi phiên chơi/học bù đang hiển thị trong phòng.
- Ngày 8 Long tham gia ghế còn lại; mốc 20 giây không cưỡng chế dừng. Sau 23h có hội thoại; tắt đèn và ngủ vẫn là lựa chọn. Dùng lại animation hai người chơi/ngủ.

## Save và tiền xe

`year-one-v2.json` + `.bak`, ghi qua file tạm và thay thế nguyên tử. Save v1 không được chuyển đổi/xóa/ghi đè. Menu hướng dẫn chơi mới nếu chỉ có v1. Phục hồi safe location, không tự mở lại game; bài thi dở được khôi phục.

Hỗ trợ đầu năm v2: 120.000đ; sau cơm số dư là 130.000đ. Tuyến chuẩn 16 vé × 7.000đ còn 18.000đ. Vé được commit trước khi chạy xe; lỗi lưu không cho xuất phát. Người chơi có thể đi thêm vòng nếu đủ tiền, mỗi lượt thật có lịch sử vé riêng; không ghi nhận vé giả khi ở nhà.

## Kiểm tra và giới hạn bằng chứng

- `YearOneV2Checks`: kiểm tra mô hình, biên điểm danh, ba tuyến 0/26/32 TC, ngân sách 16 vé, preview/commit, deadline, exam resume, backup, save v1 và tính hợp lệ của toàn bộ checkpoint.
- `YearOneV2PlayChecks`: các lượt Play Mode tách save để kiểm tra Windows/typing/x1–x4, ngủ/cancel/skip, bài thi dở, lớp học thật, ăn lẩu và Năm 2 có nợ, lỗi ghi file, xe chuyến sau và Long. Log Error/Exception/Assert làm lượt kiểm tra thất bại.
- Kết quả và ảnh: `mobile-2d/PlayModeInteractionReports/year-one-v2/`.
- Các suite lịch cũ sáu ngày không còn là tiêu chí của progression v2. Không lấy kết quả cũ 117 checks làm bằng chứng cho bản mới.

Đọc `MANUAL_CHECKPOINTS.md` để tự kiểm tra trong Unity. Kết quả mô hình không thay thế kết quả Play Mode hoặc trải nghiệm thủ công.

## File triển khai chính

- `Assets/Scripts/YearOneCalendar.cs`, `YearOneSchedule.cs`: timeline, lịch học, môn và tín chỉ.
- `YearOneProgress.cs`, `YearOneComputerTime.cs`: trạng thái, hậu quả, thi, tín chỉ và snapshot Năm 2.
- `YearOneCampaignController.cs`, `YearOneCheckpointStore.cs`: transaction và save v2.
- `YearOneCampaignFlow.cs`, `YearOneCampaignUI.cs`, `YearOneMilestoneController.cs`: lớp học, ngủ, skip, tổng kết, lẩu và chuyển năm.
- `YearOneRuntimeUI.cs`, `YearOneReminders.cs`, `ComputerDesktopUI.cs`, `DeskActionModal.cs`: HUD, nhắc lịch, Windows và phiên chơi trong phòng.
- `CampusCommuteController.cs`, `YearOneTravelPresentation.cs`, `PlayerMovement.cs`, `PlayerInteractionDetector.cs`, `RoommateActivityController.cs`: thời gian di chuyển, khóa input và tương tác hiện có.
- `Assets/Editor/DeveloperCheckpointCatalog.cs`, `DeveloperCheckpointWindow.cs`: checkpoint F8 tách save.
- `Assets/Editor/YearOneV2Checks.cs`, `YearOneV2PlayChecks.cs`: kiểm tra mô hình và tương tác Play Mode.

Các tên file không ghi thư mục ở trên thuộc `mobile-2d/Assets/Scripts/`.

## Kết quả kiểm tra

- Unity 6000.6.7f1: compile thành công.
- 431 kiểm tra mô hình: PASS, gồm ba tuyến toàn năm 0/26/32 TC.
- 8 luồng Play Mode thực: PASS (708 assertions); sau sửa countdown, chạy lại riêng luồng nhắc lịch: PASS (7 assertions).
- Đã xem ảnh Windows, lịch HK, chơi trong phòng, nhắc học, đêm, hai người ngủ và bàn giao Năm 2.
- Báo cáo: `mobile-2d/PlayModeInteractionReports/year-one-v2/validation-summary.json`. Hash nguồn/scene được đối chiếu với bản chạy kiểm tra.
- Chưa chạy standalone player build hoặc chơi thủ công toàn bộ 10 ngày.
