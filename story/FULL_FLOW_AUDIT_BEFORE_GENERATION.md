# Rà soát toàn bộ flow trước khi generate

Ngày: 25/10/2026. Phạm vi: toàn bộ Mission 01–45, transition, hệ thống xuyên suốt và tác động asset.
Trạng thái: phân tích tĩnh source/kịch bản, đề xuất thiết kế; chưa sửa gameplay, chưa generate ảnh.
Không coi báo cáo test cũ là bằng chứng full game đã hoàn thành. Không chạy Play Mode mới cho bản rà soát này.

> Ghi chú 26/10/2026: đây là audit lịch sử. Lịch HĐH kỳ 10/M36 cuối Năm 5 trong bản này đã được thay bằng [baseline mới](PRODUCTION_FLOW_BASELINE.md): môn cuối và M36 đầu Năm 6; payoff theo [plan Năm 6](YEAR_SIX_PAYOFF_PLAN.md). Không dùng bảng cũ làm lịch author hiện hành.

## 1. Kết luận

Cốt truyện có arc rõ: tài năng/chủ quan → hậu quả → mưu sinh → chia ly → chủ động sửa sai → trưởng thành.
Nhưng kịch bản chưa phải flow sản xuất hoàn chỉnh, và runtime hiện mới là vertical slice Năm 1 nối vào demo bốn stage.
Cần chốt lại logic học vụ, lựa chọn và transition trước khi đóng danh mục ảnh mới.
16 sheet/98 ô trong REMAINING_ASSET_INVENTORY.md là dự toán cũ cho phạm vi hẹp, KHÔNG phải số lượng cuối cùng cho full game.

P0 = phải giải quyết trước khóa flow/asset; P1 = phải giải quyết trước milestone hoàn chỉnh; P2 = polish.
“Thiếu runtime” nghĩa là chưa thấy đường thực thi đầy đủ trong các controller đã kiểm tra, không chỉ thiếu sprite.
Mọi thay đổi bên dưới là đề xuất, chưa tự ghi đè master design.

## 2. Bằng chứng hiện trạng

| Nguồn | Quan sát xác nhận từ source | Ý nghĩa |
|---|---|---|
| MVPFlowController.InitStage1 | Bắt đầu ngay trong 404, xí giường → dọn rác 3 lần → bạn xuất hiện | Không có opening M01–02; M03–04 rút gọn; M05 chưa nằm trong chuỗi này |
| RoommatesEnterRoutine + extendedYearOne | Giới thiệu → chuẩn bị balo/WC/xe buýt | Bỏ bữa cơm làm quen; nhận diện nhân vật chủ yếu bằng thoại |
| CampusCommuteController | Tiền khởi tạo 35k riêng, vé 7k, HasCompletedTrip khóa chuyến sau; xe đến khi gần trạm | Chưa dùng cho lịch xe lặp lại và M07 trễ xe; chưa có kinh tế xuyên game |
| YearOneMilestoneController | H6 → puzzle → card 32/150 → lẩu → CompleteExtendedYearOne | M06/08 có sequence cụ thể; chưa có M07 sau buổi H6 này |
| CalculusPuzzleModal + YearOneSceneSetup | Câu hiện tại là tổng 1…10, đáp án 55, timer 20s/retry | Review năm nhất cũ nói tích phân 1/2 đã lỗi thời; dùng WC_AND_PUZZLE_REVIEW và source mới hơn |
| MVPFlowController.Stage2 | YES → time-lapse 05:10 → báo thức “đi thi” → trễ thi → F → Stage3 năm 5 | Mất M09, M12–16 đầy đủ, toàn bộ Năm 3 và Năm 4; nhầm cúp tiết với bỏ thi |
| NO listener | Chạy thoại Long gây áp lực, không hoàn thành lựa chọn NO | Người chơi có thể ở lại modal; nhiều click có nguy cơ nhiều dialogue coroutine chung UI |
| Snooze routine | Biến snoozed được đặt nhưng không dùng để quyết định kết quả; hết 4s vẫn trễ thi | Input không ảnh hưởng outcome; không phải nhánh thức dậy |
| Stage3/PostUninstall | Bạn biến mất → uninstall → tự nhảy Năm 6 | Bỏ người mới, ba cuộc chia tay, học trả nợ và cổng đủ điều kiện |
| Stage4/OnTurnOffLightsFinal | Tự cấp A/bằng rồi tắt đèn → Finished | Chưa chơi đồ án/bảo vệ; M45 không được chạy sau tắt đèn |
| DeskActionModal/DormJourney | StudyProgress session-local; journey flags và objective riêng | Học ở bàn chưa phải điểm/tín chỉ; cần thống nhất quest/UI/state |
| Scripts đã tìm | Chưa thấy QuestManager/TimeManager/AcademicPortal/save persistence tương ứng roadmap | Những module trong roadmap vẫn là mục tiêu kiến trúc, chưa phải hệ thống đầy đủ đã có |

## 3. Các vấn đề cần sửa theo ưu tiên

### P0-01 — Phạm vi “còn lại” bỏ sót đầu game

Danh mục trước chỉ tính M07 và M09–45. Nhưng flow hiện tại không có bến xe, đăng ký KTX, bữa cơm M05 và các bước đầy đủ M03–04.
Đề xuất giữ 45 mission, bổ sung opening 4–6 phút thay vì mặc định M1 đã hoàn chỉnh.
M03 cần nhận chìa khóa từ M02, claim bed rồi mới đặt balo; móc lấy balo đi học có thể dùng sau claim.
Hiện hook có thể dùng ngay: khi tích hợp narrative, khóa bằng quest flag hoặc tách “xí chỗ” và “đeo balo” rõ ràng.

### P0-02 — Nhảy năm bỏ nguyên nhân và hậu quả

Thay chuỗi demo Stage2→Stage3→Stage4 bằng mission graph đầy đủ. Không nối asset mới vào nhánh demo tự cấp kết quả.
M08 phải mở M09 đăng ký môn rồi mới M10 gaming. M15 mở M16 gọi mẹ và M17 kiếm tiền; không nhảy Năm 5.
M32 mở M33–36 trả nợ; M36 mới mở M37. M43 có kết quả đạt mới mở M44, M44 mới mở M45 và credits.
Demo bốn stage có thể giữ trong chế độ test riêng, không làm đường tiến trình production.

### P0-03 — Lựa chọn có thật hay cốt truyện tuyến tính?

Master buộc YES, snooze và điểm F dù giới thiệu lựa chọn. Nếu không chốt, phải làm thêm hàng loạt nhánh/asset.
Đề xuất baseline: narrative tuyến tính về một nhân vật đã có quá khứ; lựa chọn nhỏ thay đổi phản ứng/chi phí, không giả vờ cho người chơi tránh toàn bộ arc.
M10: NO thực sự kết thúc buổi chơi và đi ngủ; YES tạo một đêm kiệt sức. Một chuỗi ngày đại diện sau đó cho thấy thói quen và kiến thức yếu; M15 vẫn là kết quả học kỳ, không quy mọi F cho một click.
M11: “Dậy ngay” đi học với năng lượng thấp hoặc Snooze mất một buổi. Điểm danh và lời thoại khác nhau; cùng hội tụ trước M12.
M14: không ép thanh 20% dù trả lời đúng; đánh giá phần thi dựa dữ liệu kiến thức/lịch sử học kỳ đã giới thiệu. Cho làm được phần mình biết, sau đó bảng tổng kết học kỳ giải thích F.
Nếu muốn lựa chọn học tốt tránh F hoàn toàn, đó là nhánh game khác cần thiết kế lại Năm 3–6; không âm thầm thêm ở giai đoạn asset.
M16: xin hỗ trợ hoặc tự xoay đều dẫn tới làm thêm; nếu mẹ hỗ trợ, chỉ một khoản sinh hoạt nhỏ, không xóa toàn bộ nợ.

### P0-04 — Mâu thuẫn môn và học vụ

- Bảng năm học ghi rớt Giải Tích 1, mission ghi Giải Tích 2; chọn Giải Tích 2 vì năm nhất đã qua bình thường.
- M15 khóa Cấu Trúc Dữ Liệu rồi M23 nói nợ môn này, chưa có bước mở khóa/đăng ký/học/trượt.
- M23 đã pass Giải Tích 2 nhưng roadmap M4 lại “pass Giải Tích 2”; không thi trả nợ cùng môn lần nữa nếu không có lý do.
- Năm 2 cúp quá số buổi bị cấm thi nhưng M14 vẫn vào phòng thi; phải phân biệt vắng tiết với cấm thi và bỏ thi.

Đề xuất graph học vụ hư cấu nội bộ game (không khẳng định quy chế thật):
Kỹ Thuật Lập Trình → Cấu Trúc Dữ Liệu → Hệ Điều Hành; Giải Tích 2/Vật Lý 1 ảnh hưởng tín chỉ và một số môn nền tảng được cấu hình riêng.
Cuối kỳ 5 pass Kỹ Thuật Lập Trình và Vật Lý 1; đầu kỳ 6 mở/đăng ký Cấu Trúc Dữ Liệu; cuối năm 3 pass Giải Tích 2 nhưng trượt Cấu Trúc Dữ Liệu vì quá tải.
Năm 4 tiến triển nhưng vẫn còn Cấu Trúc Dữ Liệu/Hệ Điều Hành; Năm 5 học lại theo đúng thứ tự kỳ9/kỳ10.
M11 baseline vắng buổi thứ 2, chưa tới ngưỡng cấm thi; không phát sinh vắng buổi thứ 3 ngoài màn hình rồi vẫn cho thi.
Điểm F do điểm thành phần/thi cuối kỳ; điểm danh là hậu quả riêng.

### P0-05 — Tín chỉ chưa có sổ cái

Hiện mới có mốc 32, 78, 136/150. “Thiếu18” ở M27 không rõ thiếu so với 150 hay điều kiện làm đồ án.
Đề xuất bảng cân đối mẫu để triển khai và cân bằng, chưa phải dữ liệu trường thật:

| Cuối năm | Credits đạt | Tăng trong năm | Ý nghĩa |
|---|---:|---:|---|
| 1 | 32 | 32 | Không nợ |
| 2 | 50 | 18 | Trượt 3 môn được nêu rõ |
| 3 | 78 | 28 | Pass một số nợ cũ; Cấu Trúc Dữ Liệu chưa đạt |
| 4 | 118 | 40 | Thiếu18 so với ngưỡng136 và còn2 tiên quyết |
| 5 | 136 | 18 | Đủ điều kiện đồ án |
| 6 | 150 | 14 | Đồ án năm cuối gồm các học phần/đánh giá tổng14 theo curriculum hư cấu |

M27 ghi “118/136 tín chỉ để ĐĂNG KÝ ĐỒ ÁN; còn2 môn tiên quyết”, không “thiếu18 để tốt nghiệp150”.
Phải phân bổ credits tới từng course/config và bảo đảm học phần đã pass không được cộng hai lần. Không gọi đồ án14 tín chỉ là quy định thật của Bách Khoa.
M43 đạt bảo vệ chưa đồng nghĩa đang cầm bằng vật lý; M45 mới trao bằng.

### P0-06 — Địa lý và nhân vật học khác trường

Đạm học HCMUS nhưng lại ngồi cùng tiết Bách Khoa M06; Long/Tộ cũng không học Bách Khoa.
Đề xuất sửa người ngồi cạnh ở H6 thành bạn cùng lớp NPC; phản ứng “Lươn thông minh” của Đạm chuyển về bàn học404 sau buổi học, Lươn kể và giải lại mẹo.
Giữ câu tổng1…10 như warm-up rất ngắn trước bài Giải Tích, tránh biến nó thành bài thi đại học; sau đó Lươn chủ quan vì giải mẹo nhanh.
M28 là buổi chụp chung tại khu ĐHQG sau các lễ riêng, không một lễ cấp bằng chung cho bốn trường.
M45 Lươn nhận bằng ở Bách Khoa, bạn đến dự; Bến Bình Đông là chuyến đi riêng ở khu Long sống, không đi bộ một bước từ KTX/H6 tới.
Bến xe/M01 và chuyến đi học/M06 là hai chuyến xe, hai ngày/địa điểm khác nhau; dùng tuyến xe hư cấu trong game, không trình bày lịch tuyến thực tế đã xác minh.
M07 nếu hụt xe: chạy tới điểm đón phụ/chuyến sau hoặc nhờ bạn chở; không chạy bộ từ KTX đến một cơ sở trường xa không xác định.

### P0-07 — Đạm đi du học rồi xuất hiện tiếp tế

M31 hoàn tất thủ tục du học, M34 lại đến404 lúc1h; M45 hội ngộ hai năm sau thiếu thông tin hành trình.
Đề xuất M31 chuyển ra khỏi KTX trước ngày bay, M34 là lần gặp trước khi lên đường; thêm tin nhắn “tuần sau tao bay”.
M45 có tin nhắn Đạm về nước dự lễ/thăm nhà. Nếu chọn Đạm đã bay ở M31 thì M34 phải là video call + tài liệu điện tử, khác pose/asset; baseline ưu tiên phương án trước ngày bay.

### P0-08 — Phòng có người mới nhưng kết thúc trống hoàn toàn

M29–31 có3 người mới; M44 bảo toàn phòng trống khiến họ vô lý biến mất.
Đề xuất chỉ giường/bàn/phần đồ Lươn trống; đồ3 bạn mới vẫn tồn tại. “Trống” là góc cũ của Lươn và cảm giác, không phải xóa toàn bộ căn phòng.
Người mới vắng vào buổi chiều, một lời chào/tin nhắn giải thích; Lươn tắt đèn trước khi ra, không khóa nhốt người khác.
Bàn giao chìa khóa tại quầy sau tắt đèn hoặc gửi quản lý trong exit sequence. Không dùng giường “chiếu trống” nếu hình nền vẫn bake đồ lên đó.

### P1-01 — Kinh tế có thể khóa game hoặc biến thành grind

M01 phạt50k nhiều lần, M04 mua35k, M05 ăn25k, vé7k trong khi runtime khởi tạo35k riêng; không có sổ tiền chung.
Đề xuất ví duy nhất; initial150k là cấu hình mẫu; phạt va chạm tối đa1 lần, không để số dư âm và luôn có tuyến đi tiếp không tốn tiền.
Chi phí hàng ngày ngoài các beat được mô phỏng trong time-lapse, không trừ tiền mỗi giây.
Nợ hơn3 triệu ở giá50k/máy tương đương hơn60 job, không phù hợp60–90 phút. Chơi3 job đại diện, sau đó montage lịch việc có bảng thu/chi trong nhiều tuần; không tự thưởng3 triệu cho1 job.
Tiền học phí tách tổng nợ và khoản cần trả cho kỳ hiện tại; miễn phí crush chỉ trừ doanh thu job đó.
Nếu cạn tiền, có job không cần vật tư/ứng trước từ khách hoặc bạn mời bữa ăn; không chặn đường chính bằng việc phải mua bánh mì.
M22/34 Long mời, có lời thoại rõ, không làm ví âm. Giữ bromance có tác dụng gợi ý/hỗ trợ nhưng không khóa ending tình bạn vì bỏ thoại tùy chọn.

### P1-02 — Lặp di chuyển và mini-game, nhịp60–90 phút

45mission không nên là45mini-game độc lập. Chọn8–10 cơ chế chung, còn lại là objective/thoại/tương tác ngắn.
Lần đầu đi đủ tuyến404 → WC→ xe→ H6; lần sau mở commute rút gọn với tương tác xuất phát/đến nơi. Không lặp toàn tuyến sau mỗi job.
Đêm Hồ Đá, chia tay và gọi mẹ không có timer; giữ quyền tiến thoại và cho lưu checkpoint.
Word/CV dùng cùng cơ chế format; M13/M39/M42 dùng code/log/test framework chung nhưng khác độ khó và mục tiêu.
Bảo vệ không yêu cầu kiến thức chuyên ngành ngoài những gì người chơi đã gặp ở M38–42; Q&A có nhật ký đề tài/gợi ý.

### P1-03 — Đề tài và tiến trình sản phẩm chưa thấy kết quả vật lý

Chốt một đề tài theo master. M38 dựng graph thiết bị; M39 demo phát hiện thiết bị lỗi; M40 thầy chỉ ra thiếu xác thực đầu vào; M41 bản sửa có kiểm thử/duyệt; M42 sửa lỗi null ở module giám sát; M43 chạy demo và trả lời chính các lựa chọn đã làm.
Không bắt gõ code dài; chọn đoạn sửa/kết nối block và chạy test. Có feedback giải thích.
M13 hạn nộp nên23:59:59, submit23:59:41; M42 bắt đầu22:45, deadline23:45, submit ví dụ23:44:32. Đây là đề xuất thống nhất đồng hồ, khác bản master đang ghi lẫn01:14:32/23:45.

### P1-04 — Input/modal/transition chưa có một chủ sở hữu

MVPFlow dialogue, yearOne dialogue, desk modal, journeyHUD đang tự quản. Khi mở rộng dễ cùng nhận E/Space và cùng ghi giờ/ánh sáng.
Dùng một presentation coordinator: ưu tiên transition > mini-game > dialogue > desk > world; mỗi frame input chỉ một consumer. Nhả phím trước khi modal mới nhận confirm.
Uninstall hiện nhận mọi left-click khi modal mở; cần hold đúng vùng nút (chuột/touch), thả thì pause/reset theo luật rõ, không bấm nền cũng tính.
HasCompletedTrip/HasRefreshed phải theo trip/day chứ không vĩnh viễn; reset khi ngủ/chuyển ngày, giữ inventory balo riêng.
Game clock tạm dừng trong thoại/menu, chỉ chạy trong challenge hoặc time-lapse được đánh dấu.
Mọi transition phải khôi phục camera/scale/pose/renderer/physics/controls/audio/light; không chỉ di chuyển transform.

### P1-05 — Fail/retry/save chưa được định nghĩa

Mỗi mini-game có success, recoverable failure và cancel. Timer hết không khiến mission không có lối ra.
Checkpoint trước thử thách và sau quest hoàn tất. Save gồm mission/substep, day/semester, course grades/credits, money ledger, inventory, roommates/room state, job state, choice flags, travel destination.
Không save “đang fade/đang hold/đang nhận tiền”; lưu stable state hoặc rollback về checkpoint. Load xong không tính lại vé, tín chỉ hay thưởng.
Pause/background trên mobile dừng timer/audio phù hợp; có nút touch cho mọi hành động. Thêm reduced motion/timer assist để người chơi tiếp cận được mini-game.

## 4. Flow đề xuất đủ 45 mission và cầu nối

Các state bên dưới là thiết kế đề xuất, chưa có đầy đủ trong runtime. Retry quay về checkpoint và không nhân đôi tiền/credits.

| M | Điểm vào | Hành động và điều kiện ra | Cầu nối / recovery |
|---|---|---|---|
| 01 | Xuống xe khách | Né người mời xe, tới trạm, mua vé | Đến sảnh KTX; có hỗ trợ nếu cạn tiền |
| 02 | Sảnh KTX | Nộp giấy, nhận chìa khóa và phòng 404 | Quản lý chỉ đường lên tầng 4 |
| 03 | Mở cửa 404 | Xí giường, đặt balo, siết ốc, cắm quạt | Thấy bụi và nhận danh sách mua đồ dọn |
| 04 | Phòng bụi | Mua bộ vệ sinh; lau 4 điểm trong 60 giây | Bạn đến lần lượt rồi rủ ăn; timeout chỉ retry phần dọn |
| 05 | Cả nhóm đói | Chọn cơm trong ngân sách và làm quen | Lịch học hiện trên điện thoại; về ngủ |
| 06 | Sáng đi học | Balo/WC/xe/H6; warm-up 55; học rồi về kể Đạm | Hẹn một buổi học khác; chưa cấp card cuối năm |
| 07 | Một sáng khác, 07:20 | Xếp hàng WC; đón xe hoặc tuyến dự phòng; điểm danh | Chuỗi ngày đại diện và kết quả thi: 32 tín chỉ; rủ lẩu |
| 08 | Đã 32/150, không nợ | Lấy bếp, cắm điện, thêm đồ, ăn, cụng ly | Ngủ; thẻ chuyển kỳ 3 và tin đăng ký môn mở M09 |
| 09 | Ngày đăng ký, 08:59 | Refresh, chọn lớp, xác nhận thời khóa biểu | Hiện lớp 06:45; tuần học đại diện dẫn tới M10 |
| 10 | Một tối 23:40 | YES chơi thêm hoặc NO đi ngủ thật | Sáng hôm sau khác năng lượng/điểm danh rồi hội tụ |
| 11 | Báo thức 06:30 | Dậy hoặc Snooze, ghi hậu quả riêng | Một tối khác nấu lẩu M12; không coi đây là buổi thi |
| 12 | Nghe chìa khóa khi nấu | Rút phích, cất nồi, giả ngủ trong 10 giây | Nhắc deadline C++; bị bắt có cảnh cáo hoặc retry |
| 13 | Tin nhắc bài C++ | Compile, tìm lỗi, patch, test, submit trước 23:59:59 | Điểm thành phần thấp; nhận lịch thi cuối kỳ |
| 14 | Ngày thi, đủ tư cách | Tới phòng, làm phần kiến thức đã học, nộp | Tin hẹn công bố điểm; không cấp F ngay từ ngủ quên |
| 15 | Portal công bố điểm | Xem 3 F và các môn bị khóa cụ thể | Mở hóa đơn học lại rồi nhận cuộc gọi mẹ |
| 16 | Hóa đơn, mẹ gọi | Trả lời theo lựa chọn; kết thúc cuộc gọi | Tin khách cạnh phòng có máy hỏng mở job đầu |
| 17 | Khách mang laptop | Sao lưu giả lập, USB, cài, bàn giao, nhận 50k | Khách giới thiệu thêm; Lươn muốn treo biển |
| 18 | Hoàn thành job đầu | Dán biển và nhận đơn mới | Job board và lịch học lại cùng được mở |
| 19 | Lịch học/ca sửa máy | Job đại diện, nghỉ/ăn, montage thu chi, nộp học phí | Kỳ 5 pass KTLT/VL1; kỳ 6 mở CTDL; khách nữ nhắn |
| 20 | Crush nhờ sửa | Scan virus, sửa và trả máy, miễn công | Tin hẹn tới quán; mượn áo Long |
| 21 | Tới quán theo tin | Gặp bạn trai crush và sửa máy theo checklist | Về trả áo; bạn nhận ra buồn và rủ Hồ Đá |
| 22 | Nhóm hẹn Hồ Đá | Ngồi, uống trà, ăn và nói chuyện không timer | Về ngủ; nhận tin điểm cuối năm |
| 23 | Portal năm 3 | Xem 78 tín chỉ; GT2 pass, CTDL fail | Đạm nhờ mang sách tới lab |
| 24 | Sách từ Đạm | Đến lab HCMUS, giao sách, nghe thảo luận | Về 404; Long nhờ chỉnh CV |
| 25 | CV trên laptop | Chỉnh định dạng, xuất và gửi CV | Vài ngày sau có mail đậu; Tộ chuẩn bị ca sáng |
| 26 | Buổi sáng bạn đi làm | Tiễn Tộ; xem lịch bạn bè và phòng dần vắng | Tin xét điều kiện đồ án của khóa |
| 27 | Đợt xét đồ án | Nộp đơn; thấy 118/136 và thiếu 2 tiên quyết | Bạn nhờ chụp ảnh chung; không cho đăng ký lặp vô hạn |
| 28 | Hẹn chụp tại ĐHQG | Chụp 3 bạn và trả lời phụ huynh | Về 404; Long báo dọn đầu kỳ 9 |
| 29 | Long đóng thùng | Bê đồ, chào, giường trống rồi người mới 1 vào | Một ngày khác Tộ báo chuyển trọ |
| 30 | Tộ rời đi | Nhận quà, ôm/chào; người mới 2 đến | Đạm nhắn hoàn tất thủ tục và ngày rời KTX |
| 31 | Đạm ở cửa | Thoại chia tay; người mới 3 đến | Đêm nhìn icon game; Đạm chưa bay |
| 32 | Chọn laptop | Uninstall, giữ đúng nút, hoàn tất | Đặt báo thức 06:30 và lịch trả nợ; chưa mở đồ án |
| 33 | Ngày mới 06:30 | Bật dậy, WC, đi học đúng giờ | Chuỗi ngày học đại diện; khuya có tiếng gõ cửa |
| 34 | Đang ôn lúc 01:00 | Mở cửa, lẩu tiếp tế, nhận tài liệu | Bạn rời; Đạm báo sắp bay; lịch chuyển tới kỳ thi |
| 35 | Đã ôn, đúng học kỳ | Kỳ 9 pass CTDL; kỳ 10 học/thi HĐH; hết nợ | Kết quả cập nhật; portal xét điều kiện |
| 36 | Portal có đủ điểm | Xác nhận 136/150 và hết nợ tiên quyết | Form đề tài và lịch GVHD đầu năm 6 |
| 37 | Lịch hẹn GVHD | Gặp thầy, chọn đề tài, đăng ký | Checklist nghiên cứu và nguồn tài liệu |
| 38 | Đề tài duyệt | Tra cứu, ghép sơ đồ, xác nhận thiết kế | Backlog module trên laptop |
| 39 | Backlog code | Sửa code/chạy test; demo đầu chạy được | Đặt lịch demo 1; ngủ và tới GVHD |
| 40 | Demo 1 | Chạy demo, nhận feedback, ghi việc sửa bảo mật | Về sửa sơ đồ/code; test mới pass để mở demo 2 |
| 41 | Demo 2 đạt | Duyệt, format 120 trang, in và đóng bìa | Nhắc hạn nộp; kiểm thử trước khi upload |
| 42 | Crash trước hạn | Tái hiện lỗi, đọc log, patch, test, submit | Receipt và backup; nhận lịch bảo vệ sau đó |
| 43 | Receipt và lịch hội đồng | Sơ mi, cắm máy chiếu, thuyết trình, Q&A, đạt A | Sai thì retry tại checkpoint/tập dượt; đạt mới mở M44 |
| 44 | Kết quả đạt | Gọi mẹ, gói đồ riêng, tắt đèn, khép cửa, trả khóa | Thư mời chuyển tới ngày lễ; chưa đánh Finished |
| 45 | Thư mời lễ | Nhận bằng A5, gặp gia đình/bạn; đi Bình Đông, khoác vai | Fade, credits, lưu hoàn thành, quay menu |

M35 có substep ở hai học kỳ để giữ thứ tự môn tiên quyết. M43 không tự cấp A khi chưa đạt; chế độ hỗ trợ phải hiển thị rõ.

## 5. Hợp đồng transition dùng chung

Mỗi cạnh mission graph cần đủ:

1. Điều kiện vào: previous complete, course/inventory/time/location flags; trigger không chạy hai lần.
2. Lý do diegetic: tin nhắn/lịch/lời rủ/tiếng chuông/giấy báo; objective kế tiếp xuất hiện trước khi giao quyền.
3. Destination: zone + spawn + hướng nhìn + camera + các lớp render đúng, không spawn trong collider.
4. Ownership: khóa player bằng token; đóng modal cũ, dừng âm loop; giữ visibility snapshot, không bật mọi renderer mù quáng.
5. Commit: trừ/nhận tiền, set flags, đổi NPC/courses chỉ một lần theo mission/substep ID.
6. Thời gian: ghi rõ cùng ngày/ngày mới/học kỳ mới; reset trạng thái hằng ngày, giữ item vĩnh viễn.
7. Release: fade hết, raycast overlay tắt, physics/controls/pose/audio/camera restore; nút confirm phải nhả trước khi nhận tiếp.
8. Cancel/load: quay stable checkpoint, không nằm trong màn đen, không nhân đôi item/tiền/credits; objective đúng.

Giữ sleep/fade ngắn cho chuyển địa điểm; thẻ học vụ cho đổi kỳ có thu/chi/môn đã pass. Không cần màn đen dài “6 tháng sau”, nhưng phải báo thời gian đã trôi.

## 6. Ngân sách nhịp chơi đề xuất

| Arc | Phút mục tiêu |
|---|---:|
| M01–08 nhập học và anh em | 12 |
| M09–16 sa sút/hậu quả | 12 |
| M17–23 mưu sinh/tình cảm | 11 |
| M24–28 tụt lại | 7 |
| M29–36 chia tay/phục hồi | 13 |
| M37–43 đồ án/bảo vệ | 15 |
| M44–45 kết thúc | 8 |
| Tổng tuyến chính | 78 |

78 phút là ngân sách thiết kế, chưa đo bằng playtest. Dự phòng retry/khám phá tới 90 phút; assist/thoại nhanh có thể gần 60 phút.
Mỗi arc chỉ 1–2 thử thách chính; mission còn lại là beat ngắn. Cần đo ít nhất 3 người chơi mới, không lấy thời gian automation làm thời lượng trải nghiệm.

## 7. Tác động lên danh mục asset — cần chỉnh trước generate

### Bổ sung phần bị bỏ sót M01–05

- Environment mới dự kiến: bến xe/trạm đầu game; sảnh tiếp nhận KTX; quán cơm/căn tin. Tái dùng campus/hallway khi phối cảnh phù hợp, không mặc định tạo 3 ảnh mới.
- NPC: quản lý KTX; người mời xe/xe ôm; người bán cơm (nếu hiện trên cảnh), bạn cùng lớp H6.
- Props: thùng nem chua; giấy nhập học/phiếu phòng (text UI); chìa khóa 404; xô/giẻ; quạt con cóc và tua vít rời nếu không tái dùng; khay cơm và các món để chọn.
- UI Unity: inventory kéo thả giấy, ví/vé, chọn món/budget, cleaning timer/4 điểm, phân phòng/claim bed.
- Không tăng sprite cho từng chuyển ngày; dùng clock/phone/card/light.

### Sửa các batch đã đề xuất

| Batch cũ | Điều chỉnh |
|---|---|
| C-02/C-03 | Cân nhắc 4 pose Hồ Đá ngồi ghế có cùng perspective; không trộn pose sàn khi dùng ghế cao |
| C-03 | dam_lab_pose đúng; bổ sung classmate_H6 ở batch nhân vật mở đầu; không để dam_writing vẫn được nhận diện là Đạm ở lớp Bách Khoa |
| P-01 | Tủ mở/đóng phải hợp layer/hinge, có chỗ chứa nồi thật; đồng hồ mặt trống runtime |
| P-04 | 3 bộ chăn người mới ở lại trong M44; không đặt mục tiêu phòng hoàn toàn trống |
| C-05 | Chốt advisor có ngồi trong 5 người hội đồng không trước khi tạo 5 portrait/pose; khác nhau bằng phụ kiện/palette nếu đủ |
| E-02 | Kỷ yếu ĐHQG là buổi chụp chung, không logo/một lễ chung 4 trường |
| E-03 | Nếu thư viện/văn phòng chỉ là modal trên backdrop trường thì gộp zone, giảm ảnh |
| E-05 | Bỏ mặc định 2 góc Bình Đông: 1 wide backdrop + camera crop đủ; chỉ góc 2 khi blockout chứng minh cần |
| C-06 | 4 pose khoác vai cần cùng scale/góc nhìn/anchor và có khoảng tay ghép, không generate độc lập rồi không chạm được |

Không có tổng batch mới cố định ở giai đoạn này: cần blockout mở đầu, roomstate và ending trước. Không tạo ảnh phụ chỉ để đủ 8 ô.
UI mới, branching flags, travel map/phone và course graph làm bằng code/data; không dồn vào imagegen.

## 8. Cổng kiểm tra trước generate và triển khai

### Cần chốt thiết kế trước

- Chọn baseline tuyến tính có lựa chọn nhỏ hội tụ như mục 3 hoặc mở rộng game thành các ending học tốt/trượt; đề xuất baseline tuyến tính.
- Chốt curriculum hư cấu và bảng tín chỉ; không ghép các con số mâu thuẫn vào UI.
- Chốt classmate ở H6, thời điểm Đạm bay, 3 người mới và góc riêng Lươn ở ending.
- Dùng blockout xác nhận zone/camera/door/seat/foreground/handanchor của cảnh cần ảnh mới.
- Sau đó mới khóa manifest và batch plan cập nhật cho toàn bộ M01–45.

### Kịch bản nghiệm thu cần có khi triển khai

1. Tuyến chuẩn M01→45 không nhảy mission; check entry/exit của từng mission.
2. NO gaming, dậy ngay, xin hỗ trợ, ăn miễn phí đều có lối đi và thoại phù hợp; không modal treo.
3. Cleaning/stealth/code/thi/Q&A sai/timeout/cancel có retry an toàn; tiền/grade không nhân đôi.
4. Thiếu tiền trước xe/cơm/học phí, energy 0, bỏ job đều có recovery không softlock.
5. M15 khóa CTDL→kỳ 5 pass KTLT→kỳ 6 mở CTDL→M23 fail→kỳ 9 pass→kỳ 10 HĐH→M36 unlock đúng thứ tự.
6. Save/load trước/sau mỗi cầu nối năm, khi đứng/ngồi/đi xe; không giữ transition lock và không charge vé2 lần.
7. Quay lại phòng sau 3 lần thay NPC: đúng 3 người mới; M44 chỉ góc Lươn trống; trả khóa xong vẫn tới M45.
8. Bảo vệ không đạt không được cấp A; tắt đèn không được đánh Finished trước nhận bằng/đi cùng anh em.
9. Touch/keyboard cùng kết quả; hold uninstall phải trúng nút; pause/background dừng timer; E/Space không xuyên modal.
10. Visual QA mọi zone/room state với ảnh thật sau generate; không xem static review này là đã test Unity.

## 9. Thứ tự thực hiện đề xuất

1. Chốt các P0 trong flow design, cập nhật MASTER_GAME_FLOW/roadmap/handover cùng một baseline.
2. Blockout M01–05 và các cầu nối còn thiếu bằng placeholder; dựng mission graph, ví/course/save và presentation ownership.
3. Playthrough đầy đủ với placeholder, thử nhánh/failure/transition trước khi đầu tưảnh.
4. Kiểm kêảnh cũ trực quan theo từng scene; sửa REMAINING_ASSET_INVENTORY và khóa batch manifest.
5. Generate theo milestone/batch đã khóa, slice/QA/tích hợp từng đợt; sau cùng sound/balance/mobile/release.

Rà soát này không tự sửa code hay các tài liệu thiết kế gốc để tránh biến đề xuất thành quyết định ngầm. Asset inventory được đánh dấu chưa khóa và bổ sung delta ở phần cuối.
