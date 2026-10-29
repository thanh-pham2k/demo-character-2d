# Fixture môn và đề thi — C_NMT v0.1

Dữ liệu hư cấu để kiểm tra thiết kế, không đại diện chương trình trường thực. Một môn không đủ cho toàn bộ Năm 1 hoặc điều kiện tốt nghiệp; không dùng fixture này để tính tổng credits toàn campaign. Chưa chạy mô phỏng hay playtest.

## Định nghĩa môn và topic

`C_NMT`, Nhập môn tư duy lập trình, 3 credit, prerequisite môn: không; topic `T_BASIC` (đọc biến, bắt đầu 20, không prerequisite) và `T_CONTROL` (điều kiện, bắt đầu 20; học thêm khi T_BASIC ≥20). Attendance gồm hai entry `D1_CLASS` và `D2_CLASS`, mỗi entry `[08:00,10:00)`. Fixture D1/D2 là các ngày được author, không suy ra có lớp ở mọi ngày giữa mốc. Các mốc W03–W10 là tuần, cần calendar ngày cụ thể trước runtime.

Evidence là tiến trình ôn, không thay thế đáp án player. Chọn topic trong preparation; nếu evidence ≥30, player có thể mở tối đa một gợi ý ngắn thuộc topic trong attempt, không giảm điểm; mở lại cùng hint không tạo thêm cost. Evidence <30 không bị khóa câu hỏi. Người chơi trả lời bằng kiến thức của mình hoặc quick resolve có xác suất được công bố và cần seed lưu; công thức quick resolve chưa thiết kế, do đó fixture không cho phép mode này. Evidence không tự trở thành điểm bài thi.

## Assignment `ASG_01`

Theo `12_Nam_1_hoc_va_ban_cung_phong.md`: hai milestone, mỗi milestone 45 phút; nộp 5 phút. Snapshot submission lưu `milestonesCompleted` và thời điểm. Điểm thành phần theo completed tại thời điểm nộp: 0/1/2 milestones tương ứng 0/50/100 điểm, late không phạt trong fixture; missed = 0. Không cập nhật snapshot khi player tiếp tục làm sau khi nộp. Phần evidence không tăng chỉ vì đánh dấu assignment complete; tác động evidence nếu có phải được author riêng.

## Eligibility và final fixture

Đủ điều kiện thi khi enrolled, không phụ thuộc attendance/assignment trong fixture. Hai attendance entry dùng cho câu chuyện và feedback, không làm player bị loại; trường hợp entry chưa resolved hiển thị unknown. Final W10 `[08:00,09:00)` có ba câu, 20 giây/câu; world clock pause từ lúc bắt đầu đến khi nộp, real-time timer xử lý theo policy khi app/background (chưa chốt). Người chơi có thể submit sớm; các câu chưa trả lời sai. `answerIndex` lưu per question, retry thao tác submit cùng attempt không đổi kết quả.

`finalPoints = floor((200 * correctCount + 3) / 6)` (round half up); missing/missed final được ghi trạng thái `missed`, điểm dùng cho course resolver của fixture là 0 và reason `missed_final` được giữ. Final 70%, assignment 30%, courseScore = floor((7 * finalPoints + 3 * assignmentPoints + 5) / 10) (round half up); đạt khi courseScore ≥50. Giữa kỳ chỉ feedback, không tính vào điểm hay credit. Course resolver chỉ chạy khi assignment và final đã resolved/missed, definition đầy đủ; thì set status `passed|failed` và cộng 3 credit đúng một lần khi pass. Thiếu dữ liệu = `unresolved`, không tự tính 0.

Ví dụ tính tay: assignment đủ 100, đúng 1/3 → final 33, tổng round(23.1+30)=53 → pass; assignment 0, đúng 2/3 → final 67, tổng 47 → fail; assignment đủ, bỏ thi → 30 → fail. Các ví dụ cố ý cho phép nhiều cách bù trừ nhưng chưa được cân bằng; thông báo trước cho player tỷ trọng và threshold.

## Bộ câu hỏi giữa kỳ mẫu

Câu 1 `MID_BASIC_01` (T_BASIC): “Gán x = 2 rồi x = x + 1. x bằng bao nhiêu?” A 1, B 2, **C 3**, D 4. Hint nếu evidence ≥30: “Phép gán thứ hai dùng giá trị x trước đó.”

Câu 2 `MID_CONTROL_01` (T_CONTROL): “Nếu `n = 4`, điều kiện `n > 3` có giá trị gì?” **A đúng**, B sai. Hint: “So sánh 4 với 3.”

Câu 3 `MID_CONTROL_02` (T_CONTROL): “Với `n = 1`, nhánh `if n > 2` chạy hay không?” A chạy, **B không chạy**. Hint: “1 không lớn hơn 2.”

Một câu T_BASIC, hai câu T_CONTROL. Feedback topic: 100% đúng với topic đã xuất hiện = strong; có cả đúng và sai = neutral; 0% = weak; không có câu hoặc attempt missed = unknown, không gọi weak. `midterm` không cập nhật topic evidence, không cấp credit và không quyết định pass/fail.

## Bộ câu hỏi final mẫu

Câu 1 `FIN_BASIC_01` (T_BASIC): “Gán `a = 3`, sau đó `a = a + 2`. Giá trị a?” A 3, B 4, **C 5**, D 6. Hint: “Dùng giá trị cũ của a khi tính vế phải.”

Câu 2 `FIN_CONTROL_01` (T_CONTROL): “Với `n = 2`, điều kiện `n >= 2`?” **A đúng**, B sai. Hint: “Dấu ≥ bao gồm trường hợp bằng.”

Câu 3 `FIN_CONTROL_02` (T_CONTROL): “Với `score = 5`, `if score > 5` chạy nhánh nào?” A nhánh if, **B nhánh else**. Hint: “Lớn hơn không bao gồm bằng.”

## Recovery giả lập và đường năm sau

Nếu fail hoặc missed final, calendar fixture mở `C_NMT_RETAKE` W12 `[08:00,09:00)` với một attempt final khác; đề và quy tắc thử được bổ sung trong `30_NMT_thi_lai.md`, nhưng chưa có policy background timer hoặc runtime nên vẫn chưa xác minh khả dụng. Để chuyển Năm 2 trong trường hợp này, resolver giữ kết quả `failed` đã có và record; chỉ đánh dấu `unresolved` nếu thiếu dữ liệu đánh giá; luật tiến độ năm sau và lịch thi lại cần author riêng.

## Cần xác minh

Chưa có simulation, các đề chưa trải qua review nội dung, chưa có course còn lại, tổng credit, chính sách background timer, accessibility cho giới hạn thời gian hoặc resolver liên năm. Nhánh fail/retake và tỷ trọng 70/30 chưa chứng minh công bằng hay đủ cơ hội phục hồi.

**Policy đề xuất:** xem `40_Policy_timer_va_accessibility.md`; chưa được triển khai hoặc kiểm chứng, các ghi chú “chưa chốt” ở trên phản ánh trạng thái fixture.
