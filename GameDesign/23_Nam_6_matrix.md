# Năm 6 — cơ hội cuối và lời chào v0.1

Theme: khép lại, trách nhiệm, chấp nhận; câu hỏi: “Tôi có thể kết thúc hành trình này thế nào?”. Chỉ mở khi Y5 transition commit vì thiếu yêu cầu thật; trạng thái thiếu dữ liệu không tự thành thiếu điều kiện. W01–W12 là thứ tự thiết kế, không phải calendar đã author. Nội dung Năm 6 không chỉ là thi lại: môi trường, các nhân vật và cách player giúp người mới phản ánh thời gian đã trôi.

| Mốc / seed | Học tập | Long | Minh / Hạnh | An / Nam | Career | Location | Lựa chọn | Outcome / callback |
|---|---|---|---|---|---|---|---|---|
| W01 `Y6_AUDIT` | Liệt kê đúng requirement thiếu | Có lịch riêng | Hạnh theo giờ tư vấn | — | Evidence trước giữ | H6, ROOM | Xem kế hoạch / ưu tiên một mục | Không xóa credit; thiếu data hiển thị unresolved |
| W02 `Y6_RECOVERY_SLOT` | Đăng ký slot cuối nếu calendar author | — | Giảng viên theo lịch | — | — | H6 | Đăng ký / xem trợ giúp / trì hoãn | Hiện chi phí và deadline; không tạo slot giả |
| W03 `Y6_JUNIOR` | Giúp sinh viên mới theo khả năng | — | S05 nếu đã gặp hoặc student khác | — | Coaching evidence nếu author | LIBRARY | Giúp / chỉ tài liệu / từ chối | Memory chỉ sau completion; không khóa học tập |
| W04 `Y6_PROJECT_REPAIR` | Sửa capstone nếu còn thiếu | — | Hạnh nếu assigned | Nam nếu available | Artifact revision | PROJECT | Làm / đổi scope nếu được phép / hỏi | Không ghi đè submission quá khứ |
| W05 `Y6_WORK_BALANCE` | Lịch học lại cạnh tranh ca làm | — | — | Coworker theo contract | Ca hoặc tự học | WORK, LIBRARY | Làm / học / nghỉ | Cần cost và recovery thực; không auto nhận việc |
| W06 `Y6_RECONCILE` | Deadline rõ trước event | Có conflict mở thì cơ hội đối thoại | Minh theo state | An tùy state | — | ROOM/CANTEEN | Nói chuyện / hẹn / bỏ qua | Không tự đóng conflict hay mặc định hàn gắn |
| W07 `Y6_GOODBYE_ROOM` | — | Callback nếu gặp và biết | — | — | — | ROOM | Xem kỷ niệm / rời đi | Chỉ memory có thật; fallback nếu chưa gặp Long |
| W08 `Y6_GOODBYE_CAMPUS` | Nhớ buổi học đã có | — | Hạnh/Minh tùy lịch | — | — | H6, LIBRARY | Ghé / không ghé | Không ép memory của event bị lỡ |
| W09 `Y6_EXAM_DEFENSE` | Assessment theo enrollment và rubric | — | Hội đồng theo lịch | — | Evidence theo artifact | CLASS, H6 | Thi / bảo vệ / hoãn nếu cho phép | Resolver ghi pass/fail/missed riêng |
| W10 `Y6_FINAL_AUDIT` | Kiểm tra toàn bộ requirements | — | — | — | Snapshot | H6 | Xem lý do / recovery nếu còn window | Missing khác unresolved; không auto graduate |
| W11 `Y6_CEREMONY_PREP` | Chỉ khi eligible | Mời nếu đã gặp và available | Tương tự | Tương tự | — | H6 | Chuẩn bị / mời / tham gia giản lược | NPC có quyền từ chối hoặc vắng; graduation không phụ thuộc khách |
| W12 `Y6_RESOLVER` | Xác nhận graduation hoặc nhánh chưa tốt nghiệp | Quan hệ/conflict giữ | Course records giữ | Memories giữ | Evidence/applications giữ | — | Preview / cancel / commit | Đủ → job search; thiếu → outcome chưa tốt nghiệp được author; unresolved → giữ state |

## Đặc tả 22 mục

1–2 Theme/câu hỏi ở trên. 3 Calendar là seed; cần mốc thật/giờ/route. 4 Academic: course thiếu, retake, capstone và deadline theo snapshot; không giả định chỉ còn một requirement. 5 Hệ: graduation audit và callback, tái dùng time/academic/relationship. 6 Locations: địa điểm đã biết; khi KTX không còn quyền ở cần điều kiện cụ thể và điểm thay thế. 7 Activities: học, review, thi, giúp student, gặp bạn, preparation.

8 NPC chính: Long/Minh/Hạnh/An/Nam theo state và calendar. 9 Phụ: S05, giám khảo, nhân viên hỗ trợ; cần hồ sơ/event riêng. 10 Major seeds: audit, recovery, final exam, resolver. 11 Medium: reconciliation, goodbye, ceremony preparation. 12 Small: bảng tin/trao đổi ngắn có trigger và tác động. 13 Milestones academic: registration, task/assessment, final audit. 14 Relationship: cơ hội liên lạc/hòa giải, không ép mọi conflict đóng. 15 Career: evidence từ công việc/project, không tự cấp offer.

16 Fail: missed cuối kỳ, chưa đủ credit, đồ án chưa đạt; 17 recovery: chỉ dùng các lịch và rule author trước hạn, công bố cost. 18–19 Unlock/gating: Y6 và assessment theo state thực; memory callback theo actor knowledge, không khóa tốt nghiệp vì thiếu người dự lễ. 20 Resolver: eligible + confirm → graduation; missing + policy author → nhánh chưa tốt nghiệp; unresolved → không phân loại. 21 Memories chỉ từ các interaction đã commit; fallback không đòi memory. 22 Carry: course attempts, credit ledger, memories, relationship, evidence, wallet/E, active applications và unresolved requirements.

## Quyết định còn thiếu

Sau W12 nếu chưa đủ điều kiện, fixture đề xuất ending chưa tốt nghiệp trong `24_Nam_6_su_kien_mau.md`; policy sản phẩm, nội dung cuối và cơ hội ngoài campaign chưa được duyệt; không được cho rằng đã tốt nghiệp hay đã có việc. Chưa có program configuration/credit graph, calendar, đề thi, rubric, roster ceremony hoặc điều kiện chỗ ở. Chưa mô phỏng, playtest hoặc chứng minh mọi thiếu hụt có đường phục hồi.
