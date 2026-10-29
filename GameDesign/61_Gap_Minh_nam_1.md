# Gặp Minh — hai cơ hội độc lập trong Năm 1 v0.1

Fixture bổ sung roster và cơ hội gặp thay thế cho `48` và `60`; không tự đặt `met_minh` khi player chỉ dự lớp. Nội dung, thời gian và cost là giả định cần duyệt, chưa có runtime.

## Roster và presence

- D8 Minh có mặt tại CLASS trong giờ `[08:00,10:00)` với tư cách sinh viên C_NMT; chỉ rảnh trò chuyện `[09:55,10:00)` sau khi player kết thúc hoặc rời activity attend. Không cho thoại chạy song song với attend. Tại 09:55 có thể bắt đầu lời chào 5 phút và hoàn tất đúng 10:00; scheduler commit completion trước khi đóng availability cùng mốc theo `02`, D8 từ 09:56 không đủ thời gian.
- Cơ hội khác `MINH_LIBRARY_INTRO`: D36 `[17:00,18:00)`, LIBRARY, Minh hiện diện/available trong khoảng này. Thư viện mở `[17:00,20:00)` cho fixture. Không phụ thuộc có dự lớp đầu hoặc thi giữa kỳ; không tự biết player đi muộn hay điểm thi.
- Các khoảng trên chỉ author presence cục bộ, không giả lập hành trình NPC. Availability unknown trong runtime phải thành `blocked_data`, không ép là có mặt. D36 có `REVIEW_NMT` tại cùng địa điểm; chào và panel review là hai lựa chọn riêng, không có tự động học. Nếu pending decisions cùng mốc, theo priority/scheduler `02` và revalidate sau mỗi decision.

## EV_Y1_MEET_MINH — Người ngồi gần cửa sổ

Instance duy nhất theo campaign và event definition; occurrence tương ứng D8 hoặc D36. Predicate: `met_minh=false`, player cùng location với Minh, Minh available ít nhất 5 phút còn lại trong cửa sổ occurrence, chưa có run đang running. Offer khi player chọn tương tác với Minh; không tự ngắt buổi học hoặc chiếm quyền điều khiển. Nếu thiếu thời gian thì không start và hiển thị giờ Minh rời. Lời chào không phụ thuộc enrolled hay midterm, miễn có access tới địa điểm.

Minh: “Mình là Minh. Cậu cũng đang học ở H6 à?”

| Lựa chọn | Nội dung |
|---|---|
| `greet` | Preview 5 phút, E −1, 0 M. Nếu E ≥1 và cả hai ở cùng địa điểm đủ thời gian, start run; completion atomically đặt `met_minh=true`, memory `MEM_MEET_MINH` với actor PLAYER/MINH, location, timestamp, `knownBy=[PLAYER,MINH]`; trust +2 clamp 100. Minh: “Gặp lại ở lớp hoặc thư viện nhé.” Không tạo lời hẹn. |
| `later` | 0 phút; Long/Minh trust không đổi; instance vẫn chưa resolved, có thể chủ động tương tác lại trước khi hết cửa sổ. Minh: “Ừ, khi khác cũng được.” |

Không đủ E: khóa greet và giải thích; later khả dụng. Không tự cộng evidence, credit, điểm thi hoặc tiết lộ kết quả cá nhân. Nếu player ngắt hoặc Minh rời trước completion: giữ thời gian/E đã tiêu theo `floor(elapsed/5)`, không ghi gặp hoặc memory; nếu còn cơ hội trong occurrence có thể tương tác lại. D8 hết cửa sổ không đóng definition: D36 vẫn khả dụng. Hết D36 chưa gặp thì không có recovery bổ sung trong fixture; không mặc định gặp tại D50/Y2. Gặp xong tại một occurrence làm occurrence còn lại không offer.

## Idempotency và liên kết

Mỗi start lưu runId và occurrenceId; commit completion dùng key `(campaignId,EV_Y1_MEET_MINH,meet-v1)` trên cả hai occurrence để không nhân đôi trust/memory. Lưu `met_minh`, memory, actor knowledge và event state cùng một transaction theo `43`; save lỗi không publish thành công, retry không cộng hai lần. Legacy label `met_minh_first_class` ở `48` được thay bằng `MEM_MEET_MINH`; `location=CLASS` phân biệt lần gặp D8 với LIBRARY. Không suy ra memory này từ attendance.

Nếu đã greet xong, D50 có thể offer `EV_Y1_GROUP_STUDY` tại `60` khi mọi điều kiện khác đúng; Y2 có thể mời Minh khi event `16` cho phép. Những event này vẫn tùy lịch, lựa chọn và presence, không được auto chạy.

## Ví dụ và trường hợp cần kiểm tra

D8 player chủ động dừng attend đúng 09:55, E=40, chào đến 10:00: E=39, gặp Minh, chỉ một memory; D36 không offer. Nếu bỏ lỡ D8, tới LIBRARY D36 17:10 và hoàn tất chào 17:15, sau đó có thể mở review nếu còn điều kiện; D50 vẫn cần tự tới thư viện. Nếu player chưa từng gặp Minh, D50 không phát lời mời quen biết; không trừ quan hệ.

QA dự kiến: D8 09:54/09:55/09:56, đang attend, D36 17:55/17:56, E=0/1, interrupt ở phút 4, retry sau lỗi save, đã gặp ở D8, cả hai event cùng trigger, Minh vắng/unknown, bỏ lỡ cả hai. Đây là kiểm tra thiết kế bằng ví dụ, chưa chạy test hay playtest; thời gian route đến LIBRARY và policy đầy đủ cho các event khác vẫn thiếu.
