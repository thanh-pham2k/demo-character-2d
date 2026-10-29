# Progression, mở khóa và đóng cơ hội — v0.1

Thiết kế hệ thống, chưa phải danh mục nội dung đầy đủ. IDs là đề xuất; threshold cụ thể phải có trong dữ liệu author. Không coi quan hệ, ngành nghề hay sự kiện ví dụ là đã được triển khai.

## Kiểu cạnh và đánh giá

Một node có `id`, `kind` (skill, topic, activity, location, event, evidence, application, milestone), conditions dạng AST `all/any/not/predicate`, khoảng hiệu lực `[start,end)`, kết quả, recovery links. Cạnh prerequisite chỉ mở khi predicate đúng; cạnh opportunity diễn tả cơ hội được offer chứ không tự cấp outcome; cạnh expiry kết thúc một cửa sổ chứ không xóa skill hay memory.

State: `unknown` (chưa biết), `known_locked` (biết nhưng chưa đủ điều kiện), `available`, `completed`, `expired` hoặc `unavailable_reason`. Đây là trạng thái hiển thị, không thay cho event ledger. Mỗi node phải mô tả ít nhất một nguồn vào và một downstream use; nếu cửa sổ là one-shot phải có phương án khác cho mục tiêu bắt buộc. UI chỉ tiết lộ điều kiện player đã biết; không dùng unknown để che giấu deadline đã công bố.

## Graph toàn campaign

```text
Y1_FIRST_CLASS → topics cơ sở → course evidence ───────────┐
      │                                                    │
      ├→ Y1_LONG_GAME → memory + quan hệ → callback Long    │
      └→ hoạt động tự học / hỏi bạn / học nhóm ────────────┤
                                                           ▼
Y2_CLUB hoặc Y2_PROJECT hoặc tự học → evidence kỹ năng → Y3_PROJECT
                                                           │
Y3_MENTOR hoặc tự ứng tuyển ────────────────────────────────┤
                                                           ▼
Y4_INTERNSHIP / CAPSTONE → portfolio + course records → graduation resolver
                                                           │
                            graduate ──────────────────────┤
                                                           ▼
                      job market → application → offer → accept → first job
```

Đường đi ví dụ, không phải mọi mũi tên đều là prerequisite bắt buộc. Cổng graduation dựa vào dữ liệu chương trình; việc không gặp Long hoặc không tham gia CLB không phải điều kiện tốt nghiệp. Career có thể mở qua các nguồn evidence khác nhau; cuối năm 4 không đủ yêu cầu thì chuyển nhánh năm 5, năm 5 chưa đủ thì năm 6.

## Ví dụ gate có thể author

| ID | Điều kiện mở | Trạng thái/đầu ra | Nếu bỏ lỡ hoặc từ chối | Callback |
|---|---|---|---|---|
| `OPP_Y1_LONG_GAME` | Đã gặp Long; Long rảnh trong cửa sổ sáng | Đề nghị chơi, player tự chọn | Hết cửa sổ → expired, không tạo memory chơi | `CB_LONG_GAME` chỉ khi đã chơi và Long biết |
| `OPP_Y2_CLUB_FAIR` | Calendar hội CLB active; player ở địa điểm | Chọn thăm, đăng ký hoặc bỏ qua | Event hết hạn; học và project cá nhân vẫn khả dụng nếu được author | Tùy CLB và event tham gia thật |
| `OPP_Y3_BACKEND_PROJECT` | API và database đạt ngưỡng trong definition | Hiển thị project và cost | Có thể học bù hoặc chọn project khác | Evidence project khi hoàn thành |
| `OPP_Y4_BACKEND_INTERNSHIP` | Trong application window; requirements của công ty thỏa ALL/ANY | Cho nộp CV, chưa đảm bảo offer | Công ty khác hoặc đợt khác nếu calendar author | Phản hồi dựa vào application record |
| `GATE_GRADUATE` | Credits, môn và capstone theo cấu hình | Cho xác nhận graduate | Liệt kê thiếu hụt + cửa sổ recovery | Recap chỉ dùng state đã commit |
| `GATE_FIRST_JOB` | Graduation committed AND offer accepted | Scene ngày đầu đi làm | Tìm tiếp, không tự tạo offer | Job và role dựa trên offer đã nhận |

## Tính độc quyền và replay

Lịch là nguồn độc quyền tự nhiên: hoạt động chồng thời gian không thể làm cùng lúc; UI cho biết lịch đã biết. Lời mời chỉ hết khi tới deadline hoặc nhân vật rút lời mời theo event; bỏ lỡ không xóa NPC khỏi game. Chọn một CLB không tự cấm CLB khác trừ khi calendar hoặc rule explicit; có thể có dự án chồng thời gian và các lời mời mutually exclusive được công bố.

Khi thiếu một prerequisite, trả về mã và nguồn phục hồi thay vì chỉ `false`. Khi không có entry phù hợp cho mục tiêu bắt buộc, đánh dấu lỗi thiết kế, không phát sinh content ngẫu nhiên. Những cổng không có đường thay thế, chưa xác định thời gian hoặc chi phí phải được báo là chưa hoàn thiện.

## QA đề xuất

Duyệt graph tìm node mồ côi, chu trình ALL không có seed, gate bắt buộc không có recovery, ID không tồn tại, event mà callback không thể được sinh. Mô phỏng hai đường: không CLB → tự học → project → internship; CLB → mentor → project → offer. So sánh cổng đã mở, lịch, evidence và ending; đây mới là kịch bản cần chạy, chưa phải kết quả kiểm chứng.
