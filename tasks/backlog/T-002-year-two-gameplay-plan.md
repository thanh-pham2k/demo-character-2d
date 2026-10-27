# T-002 — Hoàn thiện flow gameplay Năm 2

## TLDR

Author và hoàn thiện Năm 2 từ bàn giao Năm 1 đến đăng ký HK3, các ngày học/chơi đại diện, lựa chọn thức khuya/báo thức, kiểm tra KTX, deadline C++, thi cuối kỳ, portal học vụ và cuộc gọi mẹ mở Năm 3. Mô tả bằng một graph liên tục với routine, hậu quả và recovery tại từng sự kiện. Phạm vi narrative M09–M16 dựa trên [production baseline](../../story/PRODUCTION_FLOW_BASELINE.md), [transition plan](../../story/GAMEPLAY_TRANSITIONS_PLAN.md) và [manuscript](../../story/MASTER_GAME_FLOW.md); module kỹ thuật có thể khác ranh giới năm truyện.

## Sơ đồ ASCII

```text
[Y2-ENTRY] NHẬN BÀN GIAO TỪ NĂM 1
  │ Credits, PASS/FAIL/reasons, môn nợ, history, lựa chọn giường và quan hệ
  ▼
Khôi phục Lươn tại 404 → xem recap năm trước và mục tiêu HK3
  │ Không xóa môn đã PASS hoặc tự cấp lại tín chỉ
  ▼
Author mốc ngày đăng ký → preview source/target/duration/việc sẽ bị lỡ
  ├─ Hủy / save lỗi → giữ mốc nguồn
  └─ Commit/save thành công → thẻ HK3 + summary + checkpoint
  │
  ▼
[M09] NGÀY ĐĂNG KÝ • 08:59 — PHÒNG 404
  │
  ▼
Thức dậy → ra giường (tầng trên: xuống thang) → vệ sinh → thay đồ
  │ Routine phải bắt đầu trước 08:59; giờ thức dậy cần author
  ▼
Tới bàn → ngồi đúng ghế → mở laptop → mở portal → xem giờ mở cổng
  │
  ▼
[REG-REFRESH] Chủ động F5 / Refresh
  ├─ Cổng chưa mở → feedback → chờ hoặc thử lại [REG-REFRESH]
  └─ Cổng đã mở → tải danh sách lớp và trạng thái chỗ
  │
  ▼
Xem môn/lớp/giờ học và lớp đã hết chỗ
  │ GT2-0900 hết chỗ theo prototype hiện có
  │ GT2: T2/06:45; VL1: T4/06:45; KTLT: T6/13:00
  ▼
Chọn lớp → xem conflict/điều kiện đăng ký
  ├─ Chưa đủ / không hợp lệ → lý do → chỉnh lựa chọn
  ├─ Đóng trước chốt → bỏ draft, không tự đăng ký
  └─ Chốt → lưu lịch → mở lại lịch để xác nhận các tiết 06:45
  │
  ▼
Điều chỉnh giờ báo thức/kế hoạch sáng → đứng dậy → free time ở 404
  │
  ▼
Ngủ/chuyển chặng → preview tới tuần học đầu HK3
  │ Montage lớp buổi sớm, đường đi, thời gian với bạn, việc học còn lại
  │ Chỉ phản ánh event đã author; không tự tạo attendance hay bài đã làm
  ├─ Hủy / save lỗi → giữ ngày đăng ký
  └─ Commit/save thành công → trả quyền tại ngày học đại diện
  │
  ▼
[Y2-CLASS] NGÀY HỌC ĐẠI DIỆN HK3 — NGÀY/GIỜ CẦN AUTHOR
  │
  ▼
Thức dậy → xuống giường → WC/đánh răng/rửa mặt → thay đồ
  │
  ▼
Xem lịch/giờ xuất phát → lấy sách → đeo balo → ra hành lang/cầu thang
  │
  ▼
Ra KTX → tới trạm → xe tới H6 → tìm lớp → ngồi/điểm danh
  ├─ Đến trong khung → đúng giờ/trễ theo timestamp thực tế → hoạt động học
  └─ Hết khung → vắng cho buổi đã khai báo, không tự cấp phần đã học
  │
  ▼
Tan học → xe về KTX → về 404 → cất balo → xem bài/deadline/knowledge gaps
  ├─ Làm bài / ôn → dùng thời gian, ghi phần thực sự hoàn thành
  ├─ Nói chuyện / chơi với Long → dùng thời gian, phản ứng quan hệ
  └─ Nghỉ → tiếp tục sinh hoạt tại 404
  │
  ▼
Chuyển tới tối M10 theo ngày đã author → preview → commit/save
  │
  ▼
[M10] MỘT TỐI • 23:40 — LONG RỦ CHƠI THÊM
  │ Sáng mai có tiết GT2 lúc 06:45; hiện giờ học, thời gian ngủ còn lại
  ▼
Đang ở bàn/laptop → Long rủ thêm trận → chọn YES / NO
  ├─ NO: KẾT THÚC PHIÊN CHƠI
  │    └─ Tắt game → đứng dậy → tắt đèn → tới đúng giường
  │         → leo nếu tầng trên → chọn ngủ lúc 23:40
  │         → giữ hậu quả nghỉ sớm/khả năng tỉnh táo tốt hơn ──────────┐
  │                                                                │
  └─ YES: CHƠI TIẾP                                                 │
       └─ Tiếp tục chơi với Long → time-lapse rõ ràng                 │
            23:40 → 01:17 → 03:05 → 04:32                            │
            → phòng bừa/rác, mệt và thiếu ngủ                        │
            → kết thúc game → đứng dậy → tới giường → ngủ ───────────┤
                                                                     │
  ◄──────────────────────────────────────────────────────────────────┘
  │ Preview/commit cùng đêm; giữ nhánh lựa chọn, không đổi ngày tùy ý
  ▼
[M11] SÁNG HÔM SAU • 06:30 — BÁO THỨC
  │
  ▼
Chuông reo → trình bày mức mệt theo đêm trước → dậy / snooze
  ├─ DẬY
  │    └─ Tắt báo thức → ngồi dậy → xuống giường/thang
  │         → WC/đánh răng/rửa mặt → thay đồ → sách/balo
  │         → rời KTX → di chuyển tới H6
  │         → tính arrival theo thời gian routine + travel
  │         → điểm danh đúng giờ/trễ/vắng thật, không teleport ───────┐
  │                                                                │
  └─ SNOOZE                                                        │
       └─ Tắt chuông → ngủ tiếp → time-lapse tới 11:47               │
            → thức dậy muộn → phòng vắng, bạn đã đi học              │
            → lịch hiện tiết đã qua → ghi attendance tương ứng      │
            → vệ sinh/thay đồ → xem việc còn có thể làm ─────────────┤
                                                                     │
  ◄──────────────────────────────────────────────────────────────────┘
  │ Nhánh đêm và báo thức đều tồn tại trong history
  ▼
[XỬ LÝ BUỔI HỌC BỊ ẢNH HƯỞNG]
  ├─ Còn thời gian tới lớp → đi học/phần học còn lại
  ├─ Có gap kiến thức → ôn/học bù theo rule hiện hành, mất free time
  └─ Bỏ mặc → giữ gap và rủi ro; không xóa lịch sử vắng
  │ Báo trước chi phí và tác dụng recovery; chưa cấp F cả môn tại đây
  ▼
Về 404 → xem recap: choice đêm qua → giờ dậy → attendance/kiến thức
  │
  ▼
Ngủ/chuyển thời gian → preview M12 là TỐI CỦA NGÀY KHÁC
  │ Hiện lịch kiểm tra KTX và bối cảnh nấu lẩu
  ├─ Hủy / save lỗi → giữ mốc nguồn
  └─ Commit/save thành công → summary/checkpoint tại 404
  │
  ▼
[M12] MỘT ĐÊM KHÁC — NẤU LẨU / KIỂM TRA KTX
  │
  ▼
Ở phòng cùng bạn → chuẩn bị bếp/đồ ăn → cắm điện → nấu
  │
  ▼
Nghe tiếng chìa khóa → báo nguy cơ → countdown 10 giây gameplay
  │
  ▼
[STEALTH] Thao tác đúng thứ tự với prop trong phòng
  │ 1. Tới ổ điện → rút phích
  │ 2. Tới nồi → bê/cất vào tủ
  │ 3. Tới giường → leo nếu tầng trên → giả ngủ/trùm chăn
  ├─ Hoàn thành trước hết giờ → bảo vệ kiểm tra → thoát cảnh
  ├─ Sai thứ tự / chưa tới prop → feedback, chưa ghi thành công
  └─ Hết giờ → cảnh cáo/feedback → recovery hoặc retry checkpoint
       │ Retry xử lý phần chưa commit; không hoàn tác event bền vững
       └─ Trở lại [STEALTH] theo checkpoint hợp lệ
  │
  ▼
Kiểm tra kết thúc → khôi phục quyền điều khiển, pose và trạng thái bếp
  │
  ▼
Xem tin nhắc C++ → preview sang NGÀY HÔM SAU / ngày deadline đã author
  │ Không gộp đêm kiểm tra phòng với hạn nộp nếu timestamp khác nhau
  ├─ Hủy / save lỗi → giữ mốc nguồn
  └─ Commit/save thành công → summary/checkpoint
  │
  ▼
[M13] NGÀY DEADLINE C++ • HẠN 23:59:59
  │
  ▼
Routine phù hợp giờ bắt đầu → xem yêu cầu, bài đã làm và thời gian còn lại
  │
  ▼
Tới bàn → ngồi → mở laptop/project bài tập mô phỏng → xem lỗi
  │
  ▼
[CPP] Compile/chạy mô phỏng → đọc thông báo/log → tìm vị trí lỗi
  ├─ Chọn sai vị trí/patch sai → feedback → trở lại [CPP], giờ vẫn trôi
  └─ Tìm đúng → sửa điều kiện/đoạn code cần thiết
  │
  ▼
Chạy các case đã author: rỗng / một phần tử / nhiều phần tử / biên
  ├─ Còn lỗi → đọc feedback → sửa lại [CPP]
  └─ Case đạt → xem kết quả → chọn nộp
  │
  ▼
Submit trước 23:59:59?
  ├─ Có → nhận receipt/timestamp thật → lưu nộp một lần
  │         23:59:41 là ví dụ, không ép mọi người chơi nộp đúng giây đó
  └─ Không → báo trễ và hậu quả/retry theo checkpoint đã author
       └─ Không tự cấp receipt, điểm hoặc đổi ví
  │
  ▼
Kết thúc bài tập → điểm thành phần/feedback theo dữ liệu đã chốt
  │ Điểm luyện tập và kết quả học kỳ phải phân biệt rõ
  ▼
Tin lịch thi → người chơi xem attendance, assignment, topics và eligibility
  ├─ Còn cơ hội → ưu tiên ôn/học bù, mất thời gian hoặc hoạt động khác
  └─ Không tiếp tục ôn → chấp nhận mức chuẩn bị hiện tại
  │
  ▼
Ngủ/chuyển thời gian → preview tới kỳ thi HK3/HK4 được author
  │ Tách các học kỳ/ngày thi, không biến cả năm thành cùng một đêm
  │ Montage chỉ tổng hợp hoạt động/sự kiện đã khai báo
  ├─ Hủy / save lỗi → ở mốc nguồn
  └─ Commit/save thành công → summary/checkpoint trước thi
  │
  ▼
[M14] NGÀY THI CUỐI KỲ — NGÀY/GIỜ TỪNG MÔN CẦN AUTHOR
  │
  ▼
Thức dậy → xuống giường → vệ sinh → thay đồ → xem lịch/eligibility
  │
  ▼
Chuẩn bị giấy bút/sách theo quy định trong game → balo → KTX/trạm/H6
  │
  ▼
[EXAM-NEXT] Tới đúng phòng/khung thi theo môn đã author
  ├─ Chưa tới giờ → chờ/ôn phần còn yếu → [EXAM-NEXT]
  ├─ Không đủ điều kiện / bỏ khung thi → ghi reason theo rule được chốt
  └─ Đủ điều kiện → ngồi vào chỗ → nhận đề → bắt đầu bài
       │
       ▼
     Câu hỏi theo phần đã học/mức chuẩn bị
       ├─ Trả lời → feedback/kết quả thật, không ép tiến độ 20%
       ├─ Nộp sớm → xác nhận và xử lý phần chưa làm theo rule
       ├─ Hủy/thoát/load → checkpoint hoặc restore attempt có quy tắc rõ
       └─ Nộp → lưu kết quả attempt một lần
  │
  ▼
Còn kỳ thi được author? → Có: [EXAM-NEXT]; không: kết thúc chặng thi
  │
  ▼
Rời H6 → bus về KTX → cất balo → nhận ngày portal công bố kết quả
  │
  ▼
Ngủ/chuyển thời gian → preview vài ngày tới mốc công bố điểm
  ├─ Hủy / save lỗi → giữ mốc nguồn
  └─ Commit/save thành công → montage/summary/checkpoint
  │
  ▼
[M15] PORTAL CÔNG BỐ ĐIỂM — HẬU QUẢ HỌC VỤ
  │
  ▼
Thức dậy/sinh hoạt theo giờ mốc → tới bàn → ngồi → mở portal
  │
  ▼
Recap nguyên nhân học vụ từ event thật:
  attendance / assignment / topics / bài thi / nhánh đêm và báo thức
  │
  ▼
Xem bảng điểm Năm 2 theo canon hiện hành:
  GT2: F | VL1: F | KTLT: F → cảnh báo học vụ và chậm tiến độ
  │ Điểm attempt và kết quả học kỳ được phân biệt trên UI
  │ Chỉ được commit sau khi rule canon–state và mapping môn Năm 1 đã chốt
  ▼
Xem graph môn: KTLT → CTDL → HĐH
  │ KTLT chưa PASS → CTDL chưa mở; CTDL chưa PASS → HĐH chưa mở
  │ Không ghi rằng mọi môn đều bị khóa; giữ môn đã PASS hợp lệ
  ▼
Xem credits/môn nợ/reasons và hóa đơn học lại
  │ Không tự trừ tiền hoặc invent học phí chỉ vì mở hóa đơn
  ▼
Đứng dậy → trở lại sinh hoạt ở 404 → điện thoại báo mẹ gọi
  │
  ▼
[M16] CUỘC GỌI MẸ — CẦU NỐI CUỐI NĂM 2 / ĐẦU NĂM 3
  │ Narrative nằm sau bảng điểm/hóa đơn; có thể reuse module Year3
  ▼
Nhìn điện thoại → bắt máy → nghe lời hỏi thăm → chọn trả lời
  ├─ Nói thật / xin hỗ trợ → phản ứng riêng của mẹ ────────────┐
  └─ Giấu khó khăn / nói mình ổn → phản ứng riêng ─────────────┤
                                                              │
  ◄───────────────────────────────────────────────────────────┘
  │ Choice để lại story/history; không tự cấp tiền ngoài data
  ▼
Kết thúc cuộc gọi → phản ứng Lươn tại 404 → xem mục tiêu tự lo học phí
  │
  ▼
Preview mốc NGÀY SAU có khách cần sửa máy
  │ Không spawn khách giữa cuộc gọi, không coi học phí đã đóng
  ├─ Hủy / save lỗi → giữ state sau cuộc gọi
  └─ Commit/save thành công → snapshot/checkpoint bàn giao
  │
  ▼
NĂM 3 / M17 — KHÁCH MANG LAPTOP ĐẦU TIÊN
  Credits + PASS/FAIL/reasons + debt + prerequisites + money/history
  + relationship/story events + lịch và mục tiêu tiếp theo
```

## Definition of Done

- [ ] Chốt ranh giới Năm 2 narrative M09–M16 và bridge sang M17; dùng M09 registration hiện có, module Year2 M10–M15 và module Year3 cho M16 khi phù hợp, tránh duplicate controller/session/wallet/save. Nguồn handoff là mô tả source chưa được nghiệm thu scene.
- [ ] Author milestone IDs, học kỳ/tuần/ngày tương đối và timestamp nguồn/đích cho từng chặng; giữ các giờ canon 08:59, lớp 06:45, choice 23:40, nhánh thức tới 04:32, báo thức 06:30, snooze 11:47 và hạn C++ 23:59:59. Trường hợp đổi giờ cần cập nhật baseline/transition data trước. Không invent ngày tuyệt đối khi chưa có lịch.
- [ ] Đối chiếu snapshot T-001 với course IDs Năm 2: YearOne v2 hiện có GT2/VL1 có thể đã PASS, trong khi production Năm 2 author GT2/VL1/KTLT F. Chốt mapping/điều chỉnh baseline và tổng credits trước khi implement (mốc cuối Năm 2 của baseline hiện là 50/150); không xóa PASS, cấp lại credits hoặc biến môn đã đạt thành nợ âm thầm.
- [ ] M09 có routine trước 08:59, interaction laptop/portal/F5, giờ mở cổng, lớp hết chỗ, chọn lịch, validation, draft/cancel/confirm và load lịch đã chốt. GT2/VL1/KTLT đăng ký theo điều kiện hợp lệ; giờ học 06:45 được báo trước choice đêm.
- [ ] M10 có YES chơi tới 04:32 và NO thực sự kết thúc game/ngủ sớm; ghi hậu quả energy/room/story theo nhánh, không đánh đồng cả hai. M11 lưu riêng wake/snooze; nếu routine + travel sau báo thức 06:30 không kịp lớp 06:45, phải ghi arrival/trễ thật hoặc author lại lịch, không gán on-time chỉ vì bấm wake. Đối chiếu handoff Year2 đang có text 06:50 với transition plan snooze 11:47.
- [ ] Các ngày học có routine giường trên/dưới, vệ sinh, thay đồ, xem lịch, sách/balo, ra KTX/bus/H6, điểm danh/hoạt động học, bus về/cất balo. Recovery hiển thị chi phí và chỉ sửa phần còn có thể khắc phục, giữ lịch sử vắng và choice.
- [ ] M12 có lịch kiểm tra được báo trước, bếp/điện/tủ/giường và 10 giây thao tác đúng thứ tự; sai/timeout/cancel/pause/reload có kết quả rõ; retry không đảo event đã commit. M13 có mô phỏng compile/log/find/patch/test/submit, feedback sai, timestamp/receipt thật và late recovery; không thực thi code tùy ý hay ép submit mẫu 23:59:41.
- [ ] M14 có lịch/eligibility theo môn, đề dựa trên kiến thức, câu trả lời/điểm attempt thật và policy pause/cancel/load/retry; không ép người chơi chỉ đạt 20%. Chốt quan hệ giữa kết quả attempt, thành phần và ba F canon M15 trước khi author bảng điểm; các nhánh tốt phải có tác động nhìn thấy và không hứa thay đổi outcome ngoài baseline.
- [ ] M15 hiển thị recap nhân–quả, điểm học kỳ/môn nợ/credits và khóa đúng KTLT→CTDL→HĐH. M16 có hai phản ứng mẹ theo lựa chọn và hậu quả story; không tự cấp/trừ tiền ngoài ledger. Hóa đơn, cuộc gọi và khách M17 có thứ tự ngày/giờ đúng.
- [ ] Mọi sleep/skip/travel có source/target/duration, preview việc bị lỡ và hậu quả, hủy trước commit, apply/save một lần, lỗi lưu giữ mốc nguồn, montage/summary/safe restore. Chỉ xử lý lớp, deadline, exam window và event thực sự khai báo; không tự tạo lớp/vắng trong khoảng trống. Save tách theo runtime hoặc migration rõ, không trộn YearOne v2 với production session.
- [ ] Có F8 checkpoint sandbox tại registration, trước YES/NO, trước wake/snooze, trước stealth/timeout, trước deadline C++, trước thi, portal và cuộc gọi mẹ. Reset phiên cũ/coroutine/reservation/modal trước restore đúng state, vị trí, pose, UI và giờ. Tài liệu ghi **checkpoint → thao tác → kết quả mong đợi** cho từng nhánh; người dùng kiểm tra thủ công theo quy định repo.
- [ ] Gate vertical slice/playtest T-007 vẫn áp dụng trước khi mở rộng production; đồng bộ task với timestamp T-006 và tích hợp T-004. Ưu tiên tái sử dụng art/pose hiện có; mọi nhu cầu art mới phải có inventory và kế hoạch batch riêng.
