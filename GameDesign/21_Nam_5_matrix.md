# Năm 5 — phục hồi và góc nhìn mới v0.1

Theme: so sánh, phục hồi, trưởng thành. Câu hỏi: “Nếu đi chậm hơn bạn bè thì sao?”. Chỉ mở khi Y4 resolver đã lưu transition sang Y5 vì điều kiện tốt nghiệp chưa đạt; chưa đủ dữ liệu không được suy ra thất bại. Mốc W01–W12 là đề xuất, không đại diện một calendar đã author.

## Ma trận

| Mốc / seed | Học tập | Long | Minh / Hạnh | An / Nam | Career | Location | Quyết định | Outcome / callback |
|---|---|---|---|---|---|---|---|---|
| W01 `Y5_AUDIT` | Phân biệt course nợ, capstone thiếu, credit thiếu | Nếu gặp: hỏi theo thông tin biết | Hạnh theo office hours | — | Evidence giữ nguyên | H6, ROOM | Xem recovery, sắp lịch | Không reset kết quả đã pass; thiếu calendar → unresolved |
| W02 `Y5_NEW_STUDENT` | Có thể giúp sinh viên mới tìm lớp | — | — | S05 hỏi đường | — | H6 | Giúp / chỉ bảng hướng dẫn / từ chối | Mất thời gian khác nhau; memory nếu tương tác; không bắt buộc |
| W03 `Y5_RETAKE` | Slot học lại cho course cụ thể | — | Hạnh nếu được phân công | — | — | CLASS, LIBRARY | Đăng ký / trì hoãn | Fee, deadline, kết quả theo course data; không tạo slot giả |
| W04 `Y5_CAPSTONE_REPAIR` | Khôi phục milestone đồ án | — | Mentor theo hợp đồng | Nam có thể review | Artifact revision | PROJECT | Sửa scope / thêm milestone / đổi phương án nếu được phép | Giữ snapshot cũ; bản mới có parentEvidenceId |
| W05 `Y5_WORK_OR_STUDY` | Học lại hoặc deadline | Có lịch riêng | — | — | Ca làm nếu có hợp đồng | CAFE, WORK | Ca / học / nghỉ | Time, tiền, E; không mặc định đi làm |
| W06 `Y5_MENTOR_JUNIOR` | Chia sẻ topic đã nắm | — | S05 theo lịch | — | Evidence hướng dẫn nếu author | LIBRARY | Giúp nhóm / chỉ tài liệu / không nhận | Memory và thời gian; không tự cấp skill |
| W07 `Y5_COMPARE` | Xem tiến độ thực | Long có thể gặp | Minh có lịch riêng | An tùy quan hệ | — | CANTEEN, ROOM | Nói về áp lực / đổi chủ đề / để sau | NPC không biết điểm nếu chưa kể; không ép hòa giải |
| W08 `Y5_CAREER_PIVOT` | Học prerequisite mới | — | — | Nam nếu đã gặp và rảnh | Xem JD/skill graph | CAREER, PROJECT | Tiếp hướng cũ / học skill khác | Không xóa evidence trước, không auto offer |
| W09 `Y5_REVIEW` | Kiểm tra nợ và deadline | — | Hạnh chỉ nếu được phân công | — | Project revision | H6, PROJECT | Ưu tiên nhiệm vụ còn thiếu | Nếu chưa có rubric/calendar → unresolved |
| W10 `Y5_EXAM_DEFENSE` | Thi/bảo vệ nếu enrolled và eligible | — | Hội đồng theo lịch | — | Evidence từ kết quả thực | CLASS, H6 | Làm bài / hoãn nếu rule có | Pass/fail/missed theo definition; không tự cấp credit |
| W11 `Y5_FRIENDS` | — | Gặp nếu available | Minh tùy state | An tùy state | — | ROOM, CANTEEN | Gặp / hẹn lại / không tham gia | Callback chỉ dựa vào shared memory và thông tin NPC biết |
| W12 `Y5_RESOLVER` | Kiểm tra graduation requirements | Giữ relationship/conflict | Giữ course outcomes | Giữ memory | Giữ evidence/applications | H6 | Graduate nếu đủ / preview Y6 nếu thiếu | Atomic save; thiếu data → unresolved, không giả cấp bằng |

## Đặc tả năm theo 22 mục

1–2 Theme/câu hỏi như trên. 3 Calendar: chỉ có seed, cần ngày, window và time-skip. 4 Academic: retake và capstone revision phụ thuộc reason thực trong snapshot; mỗi requirement thiếu phải map tới cơ hội có cost, deadline và availability. 5 Hệ mới: recovery planning và mentor junior trên hệ hiện có, không cần phát minh thêm chỉ số. 6 Location: những nơi đã biết cùng lớp học/office hour có lịch; 7 activity: học lại, project revision, hỗ trợ sinh viên mới, ca làm, nghỉ, gặp bạn.

8 NPC chính: Long, Minh, Hạnh, An, Nam tùy state; không tự giả định họ tốt nghiệp hay làm việc. 9 NPC phụ: S05 sinh viên mới và nhân sự course/work. 10 Major seeds: audit, recovery, comparison, resolver. 11 Medium: giúp sinh viên, career pivot, gặp bạn. 12 Small: tin bảng, thoại tại H6 chỉ khi author. 13 Academic: registration, retake, defense. 14 Relationship: liên hệ có điều kiện, conflict có thể còn mở. 15 Career: evidence sửa project, tìm hướng khác; không tự cấp job.

16 Fail: lỡ lịch học lại, bài chưa đạt, thiếu credit, portfolio không hoàn thành; không làm lại cả năm khi có một lỗi. 17 Recovery: slot tiếp theo đã author, đổi scope nếu rule cho phép, tư vấn chương trình; data trống hiện unresolved. 18–19 Gate: Y5 chỉ khi chuyển hợp lệ; không require gặp bạn hoặc mentoring. 20 Resolver: đủ mọi predicate và player confirm → graduation; chưa đủ + có calendar → Y6; thiếu data → unresolved. 21 Memory: chỉ ghi interaction đã commit; giúp sinh viên có thể callback khi tốt nghiệp nếu actor thực biết. 22 Carry: credits, course attempts, knowledge, evidence revisions, applications, wallet/E, quan hệ/conflict, flags/memories, lịch.

## Chưa đạt

Chưa có program configuration, số môn/credit, lịch học lại, dữ liệu capstone, phí, thông tin việc làm hoặc event records. Không thể chứng minh mọi thiếu hụt có thể phục hồi trước năm 6, hay nhịp chơi công bằng. Chưa mô phỏng hoặc playtest.
