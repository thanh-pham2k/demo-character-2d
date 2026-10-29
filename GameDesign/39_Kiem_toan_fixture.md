# Kiểm toán khả thi fixture — v0.1

Kiểm tra bằng đọc và tính tay trên `10`, `14`, `29`, `30`, `34`, `35`, `37`, `38`. Không chạy simulation/test, không chứng minh toàn game. Những nguồn chưa có data được đánh `chưa chứng minh`, không gán điểm đạt.

## Trace A — NMT đúng giờ và nộp đủ

Giả sử player đã enrolled, met Long, D9 07:00 ở ROOM, E=75, wallet=30M; route và xe theo `34`; các event khác không chen ngang.

- Game nhanh 15 phút: 07:15, E=72, memory Long chỉ sau completion.
- Đi ROOM→BUS 10 phút: 07:25; E travel **chưa xác định**, wallet vẫn 30M.
- Chờ 07:40, chọn board: wallet=27M sau capture, tới H6 07:45, CLASS 07:50. Vào lớp trước start chỉ là waiting; ở lại và chọn học lúc 08:00 → on_time.
- D15 ở ROOM 18:00, hoàn thành hai milestone assignment 90 phút, E−14 theo `12`; submit 5 phút đến 19:35, status on_time, assignmentPoints 100. Không suy E cuối vì sleep/ăn/travel nhiều ngày không được author.
- D64 final 1/3 đúng → finalPoints 33; courseScore round(0.7×33+0.3×100)=53. Nếu assessment, submission, eligibility và save đều hợp lệ thì NMT pass và +3 credit một lần. Ứng xử background timer chưa author → không thể xác nhận runtime assessment sẵn sàng.

## Trace B — lỡ xe và thi lại

Giả sử fixture D9, player đã chọn xe 07:40 trước 07:00; ba lượt game hoàn thành 07:45, E−9. Xe đóng lúc 07:40 khi player còn ROOM → `missed_bus_id=BUS_0740`; Long không tự biết việc lỡ xe. Tới trạm 07:55; trợ giúp 30 phút tới H6 08:25; đi CLASS 5 phút tới 08:30 → late nếu player chọn học. Vé 3M không bị trừ nếu không lên xe; cost energy của travel/support chưa được author.

Giả sử assignmentPoints cùng attempt =100, D64 final 0/3 → score 30, failed. Đăng ký thi lại D71 08:00–18:00, assessment D78, trả lời 1/3 → finalPoints 33, score 53, pass +3 credit một lần. Điều kiện: registration hoàn tất, dữ liệu đề/timer policy hợp lệ, cùng assignment snapshot, save thành công. Nếu assignment unresolved, score vẫn unresolved. Điều này chỉ chứng minh phép tính và thứ tự giả định, không chứng minh có đủ tiền, E, route hay lịch xuyên năm.

## Các xung đột/thiếu dữ liệu cần sửa

| ID | Quan sát | Hệ quả |
|---|---|---|
| B01 | Mốc BUS_0740 selection trước 07:00 chưa có entry discovery/UX cụ thể | Fixture cần nêu trip knownAt và giao diện chọn; không khẳng định cờ missed được tạo tự nhiên |
| B02 | Route ROOM→BUS chỉ có 10 phút, chưa có energy, route trở về, đăng ký môn | Không tính ngân sách E/time thực tế |
| B03 | Timer assessment chưa có policy background/accessibility | Không thể triển khai đánh giá an toàn hoặc test save/resume |
| B04 | `PROGRAM_IT_A` có 24 môn, 23 chưa có assessment/calendar | Không chứng minh 72 credit hoặc graduation Y4/5/6 |
| B05 | C_INTERN campus và C_CAP có W labels nhưng chưa có ngày/assessor | Không chứng minh recovery và thời gian overlap |
| B06 | Postgrad fixture có A/C, chưa có policy sau C2 | Player có thể hết nội dung ứng tuyển, không chứng minh first job |
| B07 | C_NMT `14` bảo toàn point attempt; Y1 resolver wording `course_unresolved` khi final failed | Failed đã resolved là khác unresolved; khi chưa đăng ký retake nên giữ failed |
| B08 | Rounding `round()` chưa quy định tie/IEEE | Khác kết quả giữa nền tảng nếu .5; cần quy tắc số nguyên |

## Sửa định nghĩa cần thiết

- Dùng `round_half_up` cho điểm không âm, tính bằng số nguyên: `finalPoints=floor((100*correct+1.5)/3)` có thể tránh float bằng `floor((200*correct+3)/6)`; `score=floor((7*finalPoints+3*assignmentPoints+5)/10)`. Cần đồng bộ các file trước khi dùng production.
- Failed course vẫn là `failed` cho tới retake đăng ký; `reexam_pending` là trạng thái attempt/entry bổ sung, không đổi course thành unresolved. Chỉ `unresolved` khi cần input chưa có. Điều chỉnh `14` và `30` để thống nhất.

## Gate

Không có bằng chứng replayability, công bằng, content density hay hoàn thành requirements. Các trace là review thủ công một môn, không thay thế simulation/QA toàn campaign hoặc playtest người dùng.
