# C_NMT — fixture thi lại v0.1

Đề xuất cho một môn hư cấu; không thay cho curriculum toàn game hay quy định trường thật. Bổ sung fixture C_NMT trong `14`. Dữ liệu chưa qua mô phỏng/playtest; các mốc W là nhãn, cần calendar có ngày thật.

## Hai loại cơ hội

- `REEXAM_NMT_Y1`: thi lại final một lần W12 `[08:00,09:00)` sau lần final fail hoặc missed. Giữ assignmentPoints từ cùng course attempt; final mới thay final cũ để tính lại score của attempt. Không cho thi nếu course đã passed. Phí 0 M, LOC_CLASS, enrolled + registered vào slot trước start. Nếu assignment chưa resolved thì resolver `unresolved`, không đoán điểm.
- `RET_NMT_Y5`/`RET_NMT_Y6`: học lại cả môn sau khi attempt trước failed, đăng ký/fee theo `22`/`24`. Tạo courseAttemptId mới, reset assignmentPoints mới về unresolved, giữ topic evidence và credit ledger. Cần assignment và calendar của attempt mới trước khi cho resolve; đề dưới có thể dùng cho final attempt mới nhưng không giả định điểm assignment mới từ attempt cũ.

W12 fixture: cho đăng ký thi lại tại H6 W11 `[08:00,18:00)` mất 15 phút E −2, 0 M; kết thúc đúng 18:00 được nhận. Nếu không đăng ký kịp, attempt cũ giữ kết quả; nợ/credit không bị thay đổi. Năm 5/6 chỉ dùng slot khi year transition đã commit và có calendar tương ứng. Không tự sinh cơ hội bổ sung ngoài data.

## Đề thi lại `REEXAM_NMT_QSET_A`

Ba câu; 20 giây thực/câu, world clock pause khi làm, lưu timer/answer. Evidence ≥30 của topic mở hint tương ứng, không thay đáp án hoặc tự cộng điểm.

- `R_BASIC_01` / T_BASIC: “`x = 4; x = x - 1`. x bằng?” A 2, B 4, **C 3**, D 5. Hint: “Thực hiện phép trừ với giá trị trước đó.”
- `R_CONTROL_01` / T_CONTROL: “Với `n = 3`, `n < 3` đúng hay sai?” A đúng, **B sai**. Hint: “Nhỏ hơn không bao gồm bằng.”
- `R_CONTROL_02` / T_CONTROL: “Với `score = 6`, `if score >= 5` chọn nhánh nào?” **A if**, B else. Hint: “6 lớn hơn 5.”

Nộp sớm tính câu bỏ trống sai; thời gian hết câu thì chuyển câu tiếp theo, đáp án chưa chọn là rỗng. Sau câu cuối tự submit; thoát hỏi tiếp tục hoặc nộp, không tự làm lại. Background timer và accessibility chưa được author, phải chặn đánh giá runtime nếu policy chưa có.

## Resolver fixture

`finalPoints=floor((200*correct+3)/6)`, `score=floor((7*finalPoints+3*assignmentPoints+5)/10)` (round half up), pass khi score ≥50. AssignmentPoints lấy immutable submission của **cùng attempt**; nếu missed và `14` quy định 0 thì dùng 0 với reason `missed_assignment`, không biến unresolved thành 0. Nếu pass, `C_NMT=passed`, award 3 credit một lần theo course ID; nếu fail, giữ evidence, ghi attempt result và không cấp credit. Đã pass không được nộp retake lần nữa.

Ví dụ: same-attempt assignment 100, 1/3 đúng → round(23.1+30)=53, pass; assignment 0, 2/3 đúng → round(46.9)=47, fail; assignment unresolved → unresolved dù final đã có. Kết quả là tính tay, không chứng minh cân bằng hoặc runtime.

## Kết nối, boundary và hạn chế

`REEXAM_NMT_Y1` không tự chuyển một attempt failed thành unresolved trước khi player đăng ký; sau khi registered, `reexam_pending` nằm trên attempt, Year 1 resolver preview có thể hiện pending. Cancel skip giữ state; nếu bỏ lỡ exam tại W12 thì record missed retake, attempt cũ vẫn failed và có thể sang năm sau khi policy year transition/data author. Nếu đăng ký nhưng chưa thi và skip qua window, scheduler ghi missed theo entry đã author; không tự pass/fail do skip.

Các case cần xác minh: register kết thúc 18:00; registration interrupted; submit đúng mốc; resume timer; cùng attemptId submit hai lần; assignment unresolved; đã pass không có retake; new course attempt không dùng assignment cũ; credit một lần. Chưa có chương trình/đề cho các môn còn lại, lịch Y2–Y4, data timestamp thực, policy background hoặc simulation.

**Policy đề xuất:** xem `40_Policy_timer_va_accessibility.md`; chưa được triển khai hoặc kiểm chứng, các ghi chú “chưa chốt” ở trên phản ánh trạng thái fixture.
