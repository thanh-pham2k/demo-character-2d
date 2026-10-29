# Timer assessment và tiếp cận — đề xuất v0.1

Áp dụng cho các assessment thực thi trong game đơn người chơi; chưa triển khai, chưa được người chơi kiểm tra. Không phải cơ chế chống gian lận, không dùng đồng hồ hệ điều hành làm nguồn đáng tin về deadline campaign. `02` quy định world clock dừng trong assessment; tài liệu này làm rõ timer câu hỏi.

## Profile

Trước khi start, player thấy số câu, thời lượng mặc định, chế độ tạm dừng và tác dụng nộp sớm. Chọn một trong ba profile: `standard` 1× (NMT 20 giây/câu; A1 30 giây/câu), `extended` 2×, `untimed` không giới hạn. Điểm, credit, eligibility, đáp án, các achievement nội bộ và cơ hội gameplay không bị phạt vì profile. Profile khóa cho attempt đã bắt đầu; lần sau có thể chọn lại. Quick resolve không mặc định bật; chưa có rubric thì không cung cấp.

## State và timer

`AssessmentAttempt`: id, version, question IDs/order, answers, currentIndex, profile, `remainingMs` (null nếu untimed), `state=running|paused|submitted`, `pauseReason`, `lastPersistedRemainingMs`, questionStart token, submissionId. Khi đang foreground và state running, timer dùng monotonic elapsed của phiên; khi app ẩn, mất focus, mở menu pause/exit-confirm hoặc không hoạt động, chuyển `paused` và lưu `remainingMs`. World clock vẫn pause suốt attempt. Không trừ thời gian player vắng mặt, không cho replay câu đã auto-advance.

Khi hiện lại, UI xác nhận “Tiếp tục” trước khi timer chạy; không tự chạy trong lúc player đọc overlay. Lưu `currentIndex`, answers và remaining sau mỗi input, pause, question transition và theo chu kỳ phù hợp; save/restore là cơ chế đề xuất, chưa chọn storage. Nếu persistence thất bại, dừng timer, báo lỗi và cho retry; không chuyển sang câu tiếp hay trừ điểm khi chưa lưu được. Đóng tab/crash: nạp lần lưu hợp lệ gần nhất và pause; có thể mất phần tương tác sau lần lưu cuối, UI phải báo nếu có thể xác định. Không khẳng định atomic persistence trước khi có triển khai.

## Boundary và xử lý input

Tại cùng mốc `remainingMs=0`, input được coi kịp nếu event input đã được hàng đợi ghi nhận với timestamp monotonic ≤ deadline và chưa commit transition; sau deadline không tính. Resolve một lần theo attemptId + questionIndex + transitionToken. Nếu hết thời gian: câu chưa chọn được ghi `unanswered`, chuyển câu sau; câu cuối submit một lần. Nếu đã chọn trước hạn, giữ lựa chọn. Manual submit lưu và chấm unanswered sai; exit-confirm có `continue` (tiếp tục với timer đã pause) và `submit` (xác nhận nộp). Không có nút reset/retry cùng attempt, trừ recovery được course author.

Untimed không tự chuyển câu theo thời gian; player chọn Next hoặc Submit. Giới hạn thời gian không áp dụng cho việc đọc hướng dẫn trước start. Không tiết lộ đáp án đúng trước submit; hint theo evidence của assessment definition. Feedback sau submit không tự sửa topic evidence.

## Accessibility

Bàn phím có thể chọn đáp án, chuyển câu, pause và submit; focus và thứ tự đọc rõ, không phụ thuộc chuột. Đồng hồ dùng chữ/số và thanh, không chỉ màu; cảnh báo thời gian còn lại không che câu hỏi. Không phát âm thanh gây bất lợi hoặc chỉ dẫn chỉ bằng âm thanh. Hỗ trợ screen reader mô tả câu, lựa chọn, trạng thái và thời gian nhưng không đọc liên tục gây cản trở. Văn bản có thể phóng, không cắt lựa chọn; từng màn hình phải được người dùng kiểm tra.

## Chống xung đột

- Tất cả policy trên chỉ là đề xuất; file `14`, `30`, `33` vẫn là fixture, không tự có runtime. Hệ thống phải kiểm tra đủ question data, rubric, profile, timer và persistence implementation; thiếu thì `blocked_data`, không chấm giả.
- `standard` và `extended` thay đổi thời gian thực, không thay đổi world clock hay kết quả credit. Timer dừng khi pause hợp lệ để nhất quán với nhu cầu tiếp cận; không làm đồng hồ của bus/lớp tiến khi bài thi mở.
- Một attempt chỉ có một `submissionId`; save lỗi không tạo kết quả partial được công bố. After reload, đã submitted thì chỉ đọc kết quả, không cho submit lần nữa.

## QA cần chạy

Input tại deadline ±1 ms; pause trước lúc hết giờ; background 10 phút; mất focus trong exit-confirm; crash trước/sau checkpoint; save lỗi khi auto-advance; load câu cuối; double submit; standard/extended/untimed cùng đáp án tạo cùng điểm; world clock không tiến; screen-reader/keyboard/zoom. Đây là kịch bản yêu cầu, chưa chạy, chưa chứng minh policy hoạt động.
