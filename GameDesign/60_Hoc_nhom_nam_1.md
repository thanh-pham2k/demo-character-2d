# Minh và buổi học nhóm — fixture Năm 1 v0.1

Đặc tả ví dụ D50, chưa phải lịch toàn campaign hoặc bằng chứng chạy. Thiết kế dùng `A_GROUP_STUDY` trong `08`, topics và E trong `12`, thứ tự scheduler `02`, save `43`.

## EV_Y1_GROUP_STUDY — Một câu hỏi, hai cách nghĩ

- **Loại:** medium, NPC/academic, một instance occurrence `GROUP_STUDY`; D50 [18:00,20:00), LIBRARY. Thư viện mở riêng cho fixture từ 17:30 đến 20:00. Minh có mặt và rảnh LIBRARY [18:00,20:00) trong fixture; không suy ra lịch ngoài khoảng này. Entry chỉ biết khi player gặp Minh tại thư viện, không lộ trên calendar trước đó. Không tự di chuyển NPC giữa scene theo lịch này ở production: cần presence/route riêng.
- **Trigger:** player vào LIBRARY trong cửa sổ, `met_minh=true`, C_NMT enrolled, Minh hiện diện và available; priority 40; chưa resolve instance. Nếu availability unknown → `blocked_data`; false → không offer. Chưa gặp, không học C_NMT hoặc vắng thư viện không trừ quan hệ; không có memory. Khi player đang làm activity interruptible, pause theo `02`, player tự chọn tiếp tục hoặc thay kế hoạch; activity không hỗ trợ resume áp dụng policy riêng.
- **Mục tiêu:** chọn chủ đề ôn với Minh hoặc dùng thời gian theo kế hoạch riêng. Minh muốn kiểm tra lập luận điều kiện trước kỳ cuối; player không tự biết điểm thi của Minh.
- **Thoại mở:** Minh: “Mình đang ôn điều kiện. Cậu muốn cùng giải vài ví dụ không? Mình ở đây đến tám giờ.”

| Lựa chọn | Thời gian và điều kiện hiển thị trước | Outcome |
|---|---|---|
| `together` | Chọn T_BASIC hoặc T_CONTROL đã mở; 60 phút, E −10, 0 M; cả hai hiện diện/available đến lúc kết thúc, E ≥10, hoàn tất ≤20:00. | Completion cộng +10 evidence topic đã chọn, clamp 100; trust Minh +2, clamp 100; `MEM_Y1_STUDY_MINH` ghi actor PLAYER/MINH, topic, thời điểm, `knownBy=[PLAYER,MINH]`. Không sửa assignment hoặc điểm thi. |
| `alone` | 0 phút để kết thúc lời mời; player có thể chọn `A_LIBRARY_STUDY` qua panel riêng. | Outcome `declined_alone`, không tự chạy học, không trừ trust hoặc tạo memory. |
| `not_now` | 0 phút. | Outcome `declined`, không đổi điểm và không tạo memory. |
| `inspect` | 0 phút; xem course card và lịch đã biết. | Quay lại cùng decision, không resolve hay trôi giờ. |

Nếu chủ đề chưa đủ prerequisite, UI nêu prerequisite đã biết và khóa chọn chủ đề, không khóa `alone`/`not_now`. Nếu còn dưới 60 phút hoặc Minh không có mặt cả buổi, khóa `together` và nêu giờ; không tự đổi thành học một mình. Tại 19:00 có thể bắt đầu và hoàn tất đúng 20:00; scheduler commit completion trước khi đóng availability cùng mốc theo `02`, sau đó không được start. Đủ E được kiểm tra khi start, không trừ trước. Học nhóm không hứa điểm cao hơn học một mình; giá trị bổ sung là ký ức chung và khả năng kết nối sau này, còn thời gian và địa điểm là đánh đổi.

## Activity và tính nguyên tử

`A_GROUP_STUDY` trong occurrence này là 60 phút liên tục, E tiêu lũy kế `floor(10×elapsed/60)`. Nếu player dừng hoặc Minh rời trước completion, activity kết thúc `interrupted`, giữ E/thời gian đã tiêu, không evidence/trust/memory; occurrence resolved interrupted, không tự lặp invitation. Player vẫn có thể tự học qua activity riêng nếu còn giờ. Trường hợp đang làm activity khác: chỉ được có một running run; nếu run trước cho resume thì lưu riêng runId/elapsed/energyApplied theo `43`, không tự resume sau nhóm.

Completion ghi evidence, trust, memory và `resolved=completed` trong một transaction với key `(instanceId,completion-v1)`. Save lỗi giữ snapshot trước completion; retry cùng key không cộng hai lần. Không tạo memory tại invitation, start, hoặc khi Minh rời sớm; không ghi feedback midterm vào memory trừ khi player thực sự chia sẻ qua event khác.

## Follow-up và bỏ lỡ

Trong `EV_Y2_GROUP_FORM` (`16`), nếu Minh là partner thực sự được tạo và Minh biết `MEM_Y1_STUDY_MINH`, tại lúc project/team đã commit Minh nói một lần: “Lần ôn ở thư viện, mình thích cách cậu giải thích từng bước. Mình thử chia việc rõ từ đầu nhé.” Nếu không có memory, thoại mặc định: “Mình chia việc rõ từ đầu nhé.” Lời này không tự thay trust, scope hay cam kết contribution. Không mở hoặc bảo đảm Minh nhận lời mời Y2; nếu chọn solo, S04, hoặc Minh bận thì không phát. Không đọc điểm thi hay lý do riêng của player.

Hết cửa sổ chưa offer → expired; Minh đi học việc riêng, không phạt. `not_now`/`alone` không chặn cơ hội project Y2. Chưa gặp Minh: có thể gặp theo cơ hội D8 hoặc D36 trong `61`; fixture học nhóm không tự đặt `met_minh=true`.

## Trace và QA đề xuất

Ví dụ D50 18:30, E=50, T_CONTROL evidence 20 và T_BASIC ≥20, cả hai có mặt: chọn học nhóm → 19:30, E=40, T_CONTROL=30, trust +2, một memory chung. Đến 19:01: không đủ 60 phút trước đóng cửa; từ chối và cân nhắc activity khác. Nếu dừng lúc 30 phút, E đã trừ 5, không cộng evidence/memory. Những con số này tính tay, không phải kết quả test.

Cần kiểm tra: player chưa gặp Minh, Minh vắng hoặc unknown, topic locked, E=9/10, evidence=95, start đúng 19:00 và 19:01, interruption, availability thay đổi, run đã pause, save lỗi/retry, solo/S04 Y2 và callback knowledge. Cần dữ liệu di chuyển và xác minh khả năng tới các cơ hội gặp Minh trước D50; chưa chạy simulation hoặc playtest.
