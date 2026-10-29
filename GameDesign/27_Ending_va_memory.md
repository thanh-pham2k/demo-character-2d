# Ending resolver và memory — v0.1

Thiết kế đề xuất, chưa triển khai hay xác minh bằng simulation. Kết thúc phải dựa trên state đã commit, không suy luận thành tích hoặc quan hệ chưa xảy ra. Áp dụng handoff trong `25` và job search trong `26`.

## Dữ liệu đầu vào

`EndingSnapshot`: version; graduationCommit hoặc nonGraduationCommit; year; courseAttempt/creditLedger; project/evidence immutable versions; internship records; offerAcceptance và job start record; relationships (stage, trust, conflict IDs, last contact), NPC knowledge; memories (id, instanceId, actors, outcome, visibility, time); event ledger và seed. Snapshot tạo một lần khi đủ điều kiện terminal, hash source revision; nếu save thất bại giữ state nguồn.

`MemoryRecord` chỉ tạo sau event commit, không tạo bởi việc NPC nhận invitation. Mỗi memory có `visibility=player|actors|public`, metadata sự kiện, actors và knownBy; callback NPC chỉ dùng memory mà NPC biết. Có thể có các trạng thái mất liên lạc, conflict mở và trust cao cùng lúc. Để chọn recap, player có thể chọn ưu tiên trong số memory đủ điều kiện; không thể chọn thứ chưa xảy ra.

## Resolver kết thúc

1. Nếu `nonGraduationCommit` hợp lệ ở cuối Y6 → `END_CONTINUING_OUTSIDE_CAMPAIGN`; `graduated=false`, `job=null` và text ghi rõ việc học tiếp không được mô phỏng. Không sinh scene ngày đầu đi làm.
2. Nếu `graduationCommit` hợp lệ AND `acceptedOffer` hợp lệ AND `firstDayStarted` committed → `END_FIRST_JOB` với role/company theo offer. Không dùng offer expired/declined hoặc lời mời phỏng vấn làm job.
3. Nếu chỉ có graduation hoặc acceptance mà chưa tới/commit first day → `PENDING`, tiếp tục gameplay, không chốt ending.
4. Nếu thiếu dữ liệu hoặc state mâu thuẫn → `UNRESOLVED` và báo record thiếu, không tự suy diễn. Với một snapshot có cả nonGraduationCommit và graduationCommit, chặn resolve và yêu cầu xử lý lỗi data; không ưu tiên một nhánh để che lỗi.

Khi ending hợp lệ, tạo `endingCommit` một lần với snapshotId, resolverVersion, primaryEnding và các fragmentIds. Retry cùng transactionId trả lại kết quả, không nhân bản memory. Đọc màn hình recap không trôi game time hay đổi state. Đổi resolverVersion không tự thay lịch sử đã commit; migration cần quy tắc riêng.

## Thành phần recap

- **Học tập:** năm tốt nghiệp nếu có; credit từ ledger; môn đạt/trượt theo kết quả đã chốt; recovery chỉ nêu nếu có attempt sau thất bại. Không gọi người chơi “thất bại” chỉ vì học thêm năm.
- **Nghề:** `acceptedOffer.role/company` khi first day đã commit; nếu chưa, không hiện nghề như kết quả. Internship chỉ nêu nếu contract và trạng thái tương ứng tồn tại; project ghi đúng revision và quality.
- **Quan hệ:** với Long/Minh/An/Nam/Hạnh, chỉ hiện fragment khi đã gặp hoặc có reason hợp lệ. `close` cần `stage=close` và shared memories theo hệ quan hệ; conflict mở được ghi thêm, không tự biến thành đã hòa giải. Tình cảm cần flag tương hỗ đã commit, không suy từ trust.
- **Ký ức:** chọn tối đa ba theo priority event author, thời gian và preference player; mỗi ID tối đa một lần. Nếu không có memory phù hợp, dùng câu trung tính “Bạn đã đi qua một hành trình riêng” không gán sự kiện cụ thể. Nếu character không có mặt, không đặt lời thoại vào miệng họ.

## Fragment mẫu với predicate

| ID | Predicate | Nội dung tóm tắt | Fallback |
|---|---|---|---|
| F_FIRST_ROOM | `first_404` | Nhớ lần đầu bước vào phòng | Không hiện |
| F_LONG_GAME | `memory_long_game_d2` và Long biết memory | Nhắc một trận chơi cùng Long | Nếu chưa chơi, không hiện |
| F_CONFLICT_OPEN | conflict Long `open` | Một cuộc trò chuyện vẫn còn dang dở | Nếu chưa gặp hoặc conflict không tồn tại, không hiện |
| F_CONFLICT_RESOLVED | conflict Long `resolved` và resolution memory | Hai người đã tìm cách thỏa thuận | Không hiện nếu chỉ có apology chưa resolve |
| F_ACADEMIC_RECOVERY | một course failed attempt rồi passed attempt sau đó | Đã vượt qua một môn sau lần thử lại | Không hiện nếu chỉ failed |
| F_HACK | memory hackathon committed | Nhớ buổi demo/hackathon | Không tự ghi chiến thắng |
| F_INTERNSHIP | internship completed evidence | Có kinh nghiệm thực tập | Nếu chưa hoàn tất, hiện đúng partial khi author |
| F_JOB | `END_FIRST_JOB` | Nêu role/company accepted | Không hiện trong pending hoặc non-graduate |

Không ưu tiên fragment trái ngược cho cùng conflict: status open chọn F_CONFLICT_OPEN; resolved chọn F_CONFLICT_RESOLVED nếu có evidence; unknown dùng fallback. Một memory có thể làm nền cho câu khác nhưng không lặp cùng nội dung nhiều lần.

## Hai trace giả định để review

- A: graduation năm 4 + backend project full + offer A accepted + first day committed + chưa gặp Long → END_FIRST_JOB, F_JOB và academic, không tạo callback Long. Việc có offer A không chứng minh player đã tham gia CLB.
- B: graduation năm 5 + course NMT failed rồi passed + Long met với conflict resolved + offer C accepted + first day committed → END_FIRST_JOB, F_ACADEMIC_RECOVERY và F_CONFLICT_RESOLVED nếu có memory tương ứng. Không tự xác định nghề backend.
- C: năm 6 program missing và non-graduation commit → END_CONTINUING_OUTSIDE_CAMPAIGN, không có bằng/job. Nếu data program unknown → UNRESOLVED, không tự kết thúc.

Đây chỉ là trace thiết kế, chưa mô phỏng bằng code. Không tuyên bố replayability từ ba ví dụ.

## Gate chưa đạt

Cần đầy đủ memory IDs/actor knowledge, fragment database, NPC ending variants, data course/career, policy migration và content cho mọi điều kiện. Cần kiểm tra save lỗi/accept retry, pending không kết thúc, fragment conflict loại trừ, privacy và empty memory. Chưa có simulation hay playtest, chưa chứng minh mọi nhánh terminal.
