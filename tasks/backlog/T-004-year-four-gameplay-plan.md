# T-004 — Hoàn thiện flow gameplay Năm 4

## TLDR

Nối snapshot T-003 vào kỳ 7–8: lập kế hoạch học vụ, project có thao tác thực, sửa máy và kiếm tiền, áp lực deadline/giấc ngủ, định hướng nghề nghiệp và khoảng cách với bạn bè. Theme: **Chọn mình muốn trở thành ai**. Mọi lựa chọn đi qua cause → state change → visible consequence → history; không reset progression hoặc hoàn thành miễn phí mọi việc.

Nguồn: [T-003](T-003-year-three-gameplay-plan.md), [baseline](../../story/PRODUCTION_FLOW_BASELINE.md), [manuscript](../../story/MASTER_GAME_FLOW.md), [systems](../../story/GAMEPLAY_SYSTEMS_PLAN.md), [transition T-006](T-006-event-driven-transitions.md). Đây là task/spec, chưa triển khai runtime/F8. Reuse production progress/session/store và adapter `Years45`; tham khảo workbench `Year6` khi phù hợp, không coi checklist prototype là nghiệm thu gameplay.

**Canon đã chốt:** không tốt nghiệp, bảo vệ đồ án tốt nghiệp hoặc bắt đầu đời sống đi làm ở Năm 4. Project trong task này là **đồ án nhỏ/học kỳ**, nội dung/môn chủ quản/criteria CẦN AUTHOR; không bypass CTDL→HĐH hoặc quyền đăng ký đồ án. M27 xét 118/136 và chỉ rõ thiếu điều kiện, M28 chụp kỷ yếu cho bạn bè, M29 mở chia tay đầu Năm 5. CV Long vẫn của Long; Lươn chỉ có bước chuẩn bị CV/internship nếu có dữ liệu author, không offer giả. Năm 6 mới trả môn cuối, hoàn thiện/bảo vệ đồ án, rời 404 và tốt nghiệp; đi làm chỉ tease sau credits. Xem [baseline mới](../../story/PRODUCTION_FLOW_BASELINE.md).

## Sơ đồ ASCII

```text
[Y4-ENTRY / CP-ENTRY] ĐẦU KỲ 7 — 404, MỐC CẦN AUTHOR
  │ Nhận snapshot sau M23 và tin Đạm; chưa giao sách, chưa chạy lại M23
  │ Credits/PASS/FAIL/reasons/debt/prerequisites, attendance/gaps/assignments
  │ Ví/nợ tiền/học phí, skill/reputation/ca tồn, đồ nghề/linh kiện/inventory
  │ Quan hệ Long/Tộ/Đạm/gia đình, choices/history, phòng 404/giường/đồ đạc
  │ Time/position/pose/UI, lịch hẹn, consent/backup/test/tiền ca đang dở
  ▼
Restore → recap năm trước → kiểm tra môn nợ, tiền và việc chưa xong
  ├─ Snapshot thiếu/mâu thuẫn → giữ save, gate mapping/migration CẦN AUTHOR
  └─ Hợp lệ → giữ state thật; không auto-zero ví, clear debt hoặc cấp TC
  ▼
Routine theo giờ → xuống đúng giường/thang → vệ sinh/thay đồ → tới bàn
  → mở portal → xem graduation progress và các điều kiện TÁCH RIÊNG
  │ Điều kiện đăng ký đồ án ≠ điều kiện tốt nghiệp
  → xem môn đạt/chưa đạt/chưa mở + credits còn thiếu + lý do + hướng học
  → lập kế hoạch kỳ 7–8; đây chưa mặc định là học kỳ cuối của Lươn
  ▼
[M??] CẦN AUTHOR — REGISTRATION / CP-REG, 404 / PORTAL
  │ Catalog/lớp/giờ mở cổng/học phí/khung đăng ký CẦN AUTHOR
  → laptop → portal → refresh → xem lớp → chọn học lại/học tiếp đủ điều kiện
  → check prerequisite → check trùng lớp/ca/hẹn → preview schedule + cost
  ├─ Cổng chưa mở → chờ/thử lại có thời gian; không đăng ký sớm
  ├─ CTDL chưa PASS → HĐH khóa; GT2 đã PASS không bắt thi lại
  ├─ Trùng lịch/thiếu điều kiện → lý do → sửa draft
  ├─ Thiếu tiền → chưa ghi đóng phí; điều chỉnh kế hoạch/nguồn hợp lệ
  └─ Confirm hợp lệ → commit đăng ký/thanh toán đúng rule → save/checkpoint
  │ Đóng draft không tự đăng ký; chưa có rule hỗ trợ thì CẦN AUTHOR
  ▼
[M24] MANG SÁCH CHO ĐẠM — 404 → LAB HCMUS, KỲ 7
  │ Input: yêu cầu từ T-003, sách và hẹn; giờ/lộ trình/duration CẦN AUTHOR
  → xem lịch → lấy đúng sách → balo → rời KTX → travel tới lab
  → tìm Đạm → giao sách → nghe/đọc thảo luận về log/sơ đồ/chẩn đoán mạng
  ├─ Chưa có sách/đến ngoài hẹn → phản hồi, quay lấy/đổi lịch có cost
  └─ Giao đúng → đánh dấu giao một lần, chọn hỏi/nghe theo knowledge/history
  → thấy hướng nghiên cứu của Đạm → về 404 → Long nhờ xem CV
  │ Không tự cấp skill nghiên cứu, đề tài, giáo sư mới hoặc đồ án đã duyệt
  ▼
[M25] REVIEW CV CHO LONG — 404 / LAPTOP
  │ Input: Long nhờ, file CV; buổi làm và ngày có email CẦN AUTHOR
  → nhận file → đọc → chỉnh font/căn lề → xem preview → xuất PDF
  → kiểm tra bản xuất → gửi Long → lưu phiên bản/receipt một lần
  ├─ Sai format/thiếu bản xuất → feedback → sửa và kiểm tra lại, tốn giờ
  └─ Hoàn tất → vài ngày sau email thực tập của LONG theo lịch đã author
  │ Không coi đây là CV/offer của Lươn; không cộng tiền hoặc nghề cho Lươn
  ▼
[M26] TIỄN TỘ ĐI CA SÁNG — 404 / LỐI RA KTX
  │ Sau M25; ngày/giờ CẦN AUTHOR; routine phải bắt đầu đủ sớm
  → dậy/vệ sinh → gặp Tộ → nói chuyện/tiễn → xem lịch bạn bè
  → Long đi làm, Tộ có ca, Đạm ở lab → phòng vắng theo lịch
  │ Ba bạn vẫn ở KTX; chưa dọn giường, chưa spawn người thay thế
  → người chơi chọn giờ học/project/ca sửa/hẹn gặp còn phù hợp
  ▼
[Y4-LOOP] NGÀY ĐẠI DIỆN KỲ 7 RỒI KỲ 8 — LỊCH CẦN AUTHOR
  → 404 → vệ sinh/thay đồ → xem lịch → sách/balo → trạm → bus → H6
  → tìm lớp → điểm danh → học/làm phần thực sự tham gia
  ├─ Trễ → học phần còn lại; ghi thiếu nội dung theo giờ arrival thật
  └─ Vắng → attendance/gap có nguồn event, chưa tự FAIL cả môn
  → về KTX/404 → cất balo → xem bài, project, máy khách và giờ ngủ
  → chọn học/assignment | project | repair | bạn bè/gia đình | ăn/nghỉ
  │ Các nhánh dùng cùng timeline; chiếm một khoảng giờ thì mất cơ hội khác
  ▼
[M??] CẦN AUTHOR — NHẬN PROJECT / CP-PROJECT, 404 / PORTAL HOẶC NƠI HẸN
  │ Chỉ mở khi đồ án nhỏ, eligibility, đề/yêu cầu và người review đã author
  │ Đây không phải đăng ký đồ án tốt nghiệp; quyền đó vẫn khóa ở M27
  → đọc yêu cầu → xác định problem → hỏi rõ phạm vi → lập kế hoạch mốc
  → preview lịch/required work/knowledge requirement/cost/deadline
  → xác nhận → lưu artifact ban đầu; không +20% từ nút “Làm đồ án”
  ▼
[PROJECT-WORK / CP-MILESTONE] 404 / LAPTOP, KHUNG GIỜ CẦN AUTHOR
  → Requirements: đọc/rà yêu cầu → lập tiêu chí chấp nhận
  → Research: chọn nguồn đã author → đối chiếu thông tin → ghi nhận hiểu biết
  → Design: ghép/chỉnh thiết kế → kiểm tra đáp ứng yêu cầu
  → Implementation: đọc code → sửa phần cần thiết → compile mô phỏng
  → Testing: chọn case theo yêu cầu → chạy → xem pass/fail + log
  ├─ Fail → tái hiện → đọc log → inspect code → chọn nguyên nhân
  │    → patch → test lại → fail thì quay debug; mọi vòng đều dùng thời gian
  └─ Pass → lưu artifact và test evidence của đúng phiên bản
  → Report: điền phần có evidence → kiểm tra thiếu/mâu thuẫn → xuất/preview
  → Demo/review: chạy bản đã chuẩn bị → nhận feedback → ghi backlog
  ├─ Crash/thiếu report/không giải thích được → reason cụ thể → fix/ôn/bổ sung
  └─ Đạt tiêu chí mốc → lưu kết quả; không tự pass final hoặc cấp credits
  │ Bug đến từ code/state đã author, không random. Patch đổi bản code làm
  │ test evidence cũ cần kiểm tra lại; implementation xong ≠ testing xong
  ▼
[M??] CẦN AUTHOR — REPAIR KHÓ HƠN / CP-REPAIR, 404 / BÀN SỬA
  │ Job phải khớp skill/dụng cụ vốn có, không auto-unlock theo năm
  → nhận máy → hỏi triệu chứng → consent → quan sát/bật/test phù hợp
  → đặt các giả thuyết → test từng khả năng → đọc bằng chứng → diagnosis
  ├─ Thiếu kỹ năng/dụng cụ → từ chối/giới hạn phạm vi hoặc hẹn lại hợp lệ
  └─ Có khả năng → báo giá/cost/hẹn giao → khách approve/reject
       ├─ Reject → trả máy; không thu công, không phạt uy tín mặc định
       └─ Approve → mua linh kiện NẾU được author và đủ ví/tồn kho
            → nhận hàng theo thời gian thực → thao tác an toàn → repair
            → stress test theo ca → kiểm tra dữ liệu → khách nghiệm thu
            ├─ Fail → diagnosis lại/sửa tiếp hoặc trả máy/refund theo rule
            └─ Đạt → giao máy → ledger/payment một lần → update reputation
  │ Model, lỗi, component, giá, stock/giao hàng, test threshold CẦN AUTHOR
  │ RAM/SSD → thermal → pin/nguồn → đa triệu chứng là mục tiêu progression;
  │ thiếu ca tiền đề từ Năm 3 thì author cầu nối, không giả đã có kinh nghiệm
  ▼
[M??] CẦN AUTHOR — MID-YEAR CRISIS / CP-CONFLICT
  │ 404: project còn lỗi + hạn gần + máy khách chưa xong + lịch thi sắp tới
  │ Chỉ mở khi có đủ work đang dở và lịch author tạo conflict thật
  → xem tổng việc, thời gian khả dụng, hẹn/cost và mức chuẩn bị
  ├─ Ưu tiên project → debug/test/report → ca khách chậm, báo/đổi hẹn
  ├─ Ưu tiên khách → sửa/test/giao mới có tiền → ít giờ project/ôn/ngủ
  ├─ Ưu tiên ôn/ngủ → giữ phần chưa xong; không tự hoàn tất project/repair
  └─ Thương lượng/chia việc → chỉ khả thi theo consent và duration thật
  → preview việc sẽ lỡ → xác nhận → thao tác → clock/overlap → consequence
  │ Không vừa nhận ca đã +Money/Reputation; từ chối giữ giờ, không auto-phạt
  ▼
THIẾU NGỦ TÍCH LŨY?
  ├─ Có → energy theo giờ ngủ → routine chậm theo rule được author
  │    → arrival muộn/ít giờ debug/test → nguy cơ lỗi do bước bị bỏ/đọc sai
  └─ Nghỉ đủ → hồi phục theo rule; công việc dở và hạn vẫn còn
  │ Không random hỏng máy/trừ điểm. Hiện causal chain trên recap
  → tới deadline → submit bản thực có / ghi trễ hoặc chưa nộp
  → receipt + version + timestamp → save một lần
  → còn ngày đại diện? quay Y4-LOOP; đổi kỳ cần preview và đăng ký lại
  ▼
[M??] CẦN AUTHOR — THI CUỐI KỲ / CP-EXAM
  │ Từng môn có lịch/eligibility/đề/rule outcome; chưa tự invent môn/điểm
  → xem lịch và ca/project tồn → chọn ôn/repair/ngủ với cost thật
  → routine → giấy tờ/balo → bus/H6 → kiểm tra eligibility → phòng thi
  ├─ Không đủ điều kiện/vắng → ghi reason → recovery ở kỳ được phép
  └─ Hợp lệ → attempt → làm bài → submit → lưu kết quả attempt một lần
  │ Kết quả môn dùng attendance + knowledge + assignments + recovery
  │ + bài thi theo rule author; không chỉ dựa vào biến progress
  → công bố điểm theo ngày riêng → portal → đối soát từng môn/credits
  │ Chốt đợt điểm nào phải có trước M27; không xét bằng điểm tương lai
  ▼
[M27 / CP-ELIGIBILITY] XÉT ĐĂNG KÝ ĐỒ ÁN — KỲ 8 / 404 / PORTAL
  → chọn đợt xét → preview dữ liệu → nộp hồ sơ một lần → xem kết quả
  │ Canon: 118/136, thiếu 18 để ĐĂNG KÝ ĐỒ ÁN; CTDL chưa đạt, HĐH khóa
  │ Không gọi 136 là ngưỡng tốt nghiệp; GT2 đã đạt không thi trả nợ lại
  ├─ Chưa đủ → chỉ rõ từng điều kiện, môn/reason và credits còn thiếu
  │    → kế hoạch trả CTDL rồi HĐH đúng kỳ → bridge Năm 5
  └─ Nếu facts thực đủ khác tuyến mục tiêu → author xử lý mapping/route
       Không xóa PASS hoặc cấp bằng; chưa có nhánh tốt nghiệp Năm 4
  │ 78 → 118 phải đối soát bằng course facts đã author, không cộng TC mù
  │ Refresh/apply lại cùng state không đổi kết quả/nhân đôi receipt
  ▼
[M28] CHỤP ẢNH CHO BẠN BÈ — ĐHQG, NGÀY/GIỜ CẦN AUTHOR
  │ Nhận hẹn sau M27 → chuẩn bị máy ảnh → travel → gặp Long/Tộ/Đạm
  → chọn khung/chụp từng bạn → xem ảnh → chụp lại nếu cần → lưu/chia sẻ
  → phụ huynh hỏi → trả lời theo học vụ thật và history quan hệ
  │ Đây là lễ của bạn bè trên tuyến canon; chưa trao bằng cho Lươn
  → phản hồi bạn bè/gia đình theo tích lũy nhiều năm, không một choice reset
  → về 404, giữ đồ/giường bạn bè → xem kế hoạch học tiếp
  ▼
[CP-BRIDGE / TUYẾN CANON] CUỐI NĂM 4 → ĐẦU KỲ 9 / M29
  → recap môn/credits, project dở, ví/nợ, repair, quan hệ và hướng nghề
  → Long báo dọn đầu kỳ 9 → preview đúng ngày → commit/save/checkpoint
  → NĂM 5: M29 bê đồ/chào Long → giường trống rồi người mới 1 vào
  │ Tộ chưa chuyển trước M30; Đạm chưa rời trước sau M34
  │ Chưa Finished, chưa happy ending hoặc xóa ca/đồ/ví khi đổi năm
```

Phần project kết thúc bằng demo/review **đồ án nhỏ**: kiểm tra code/test/report → chuẩn bị → tới nơi review theo lịch → demo → giải thích → nhận feedback → fix/retest → submit đúng phiên bản → receipt. Thiếu report/bug/kiến thức yếu tạo reason và recovery có cost; không cấp bằng hoặc dùng kết quả này thay đồ án tốt nghiệp.

Chuẩn bị nghề nghiệp chỉ là xem kỹ năng thiếu, ghi kinh nghiệm thật cho CV và đọc cơ hội internship được author; không mở loop công ty/phỏng vấn nhận việc full-time/ending. Cuối năm snapshot giữ academic/project/economy/repair/social/room/living/calendar/fatigue/obligations để T-005 tiếp nhận, chưa Finished.

Mỗi nút kế thừa state tích lũy; input/địa điểm/routine/choice/hậu quả/recovery nằm trong graph. Tất cả thời điểm, duration, tiêu chí chất lượng, giá và rule chưa có nguồn phải ghi **CẦN AUTHOR** trước runtime. Các nút `[M??]` không tự trở thành mission canon hoặc tên API.

Project có state theo artifact: mỗi phần requirements/design/implementation/testing/report/final demo phải có required work, time cost, knowledge requirement, deadline, quality state và evidence hoàn thành. Không suy ra pass vì đã đọc đủ checklist. Research/compile/debug trong graph là gameplay mô phỏng cho người chơi, không phải chỉ dẫn agent chạy compile project Unity.

Repair/reputation: skill chỉ tăng từ trải nghiệm kiểm tra/sửa/test có evidence; không farm vòng lặp. Sửa đúng/đúng hẹn, sửa đúng/trễ và chẩn đoán sai có rule khác nhau cần author. Trễ có thể vẫn nhận công nếu thỏa thuận; không nhân đôi hoặc tự mất tiền ngoài ledger. Từ chối chưa cam kết không có penalty mặc định. Recovery trả máy/refund phải giữ ownership, backup, khoản đã thu và lịch hẹn; không invent refund nếu chưa có tiền thu.

```text
ACTION → validate → biết duration/cost → thao tác → clock advances
  → xử lý overlap theo thứ tự thời gian → hậu quả → history
  → commit state/effect → save → checkpoint

SLEEP / SKIP / TRAVEL
  → Source time/place → Target time/place → Duration
  → preview lớp/thi/deadline/hẹn khách/bạn bè có thể bỏ lỡ
  ├─ Hủy → giữ state nguồn
  └─ Confirm → giữ choice trung gian cần người chơi, không montage bỏ qua
       → commit → save → summary/checkpoint → trả quyền đúng pose/UI
       Save lỗi → chưa báo hoàn tất, retry không nhân effect
```

Montage chỉ tổng hợp event đã author, không tự tạo attendance, bài nộp, project work hoặc tiền. Preview tính lại khi lịch thay đổi. Hủy UI không hoàn tác action/thời gian đã commit; retry fixture kiểm thử không phải recovery miễn phí trong save chính.

Ledger dùng ví hiện có: thu repair sau nghiệm thu/bàn giao, job/freelance hoặc story chỉ khi có author; chi học phí, sinh hoạt, linh kiện, project hoặc khoản story rõ nguồn. Mỗi giao dịch có event ID, timestamp, lý do, số tiền, chiều thu/chi và chống lặp. Báo giá/đọc hóa đơn/apply không tự đổi ví; không auto-zero lúc sang năm. Khoản ngoài khả năng chi phải có nhánh hoãn/từ chối/nguồn hợp lệ, không tự vay hoặc cấp hỗ trợ.

## Definition of Done

- [ ] Giữ canon sáu năm: Năm 4 chỉ có đồ án nhỏ; không bảo vệ tốt nghiệp/graduation route/career ending. Author đề/yêu cầu/môn chủ quản của project nhỏ và cơ hội chuẩn bị internship, không dời M37–M45.
- [ ] Handoff T-003 đầy đủ, đúng time/location/pose/UI và ca đang dở; giữ academic, economy, repair skill/reputation/tools/parts, social, room và history. M24 giao sách đúng một lần.
- [ ] M24–M28 giữ chuỗi nhân quả: lab → CV Long → email Long → tiễn Tộ/lịch bạn bè → xét đồ án → ảnh kỷ yếu → đầu kỳ 9/M29. Long/Tộ/Đạm chưa dọn đi trong Năm 4; không nhầm graduation/job của bạn thành state Lươn.
- [ ] Author kỳ 7–8, catalog/lớp/fees, lịch từng routine/thi/project/ca repair/story/portal và duration; đủ mười trường cho mỗi milestone: thời gian/địa điểm, input, routine, interaction, choice, time cost, consequence, recovery, preview, commit/save/checkpoint.
- [ ] Registration refresh/check prerequisite/conflict/preview/confirm/save có nhánh thiếu tiền/không đủ điều kiện. CTDL → HĐH không bị bypass; môn đã PASS không học lại để farm credits.
- [ ] Author course facts giải thích 78 → 118 và hai điều kiện CTDL/HĐH tại M27; thiếu 18 so với 136 để đăng ký đồ án, không phải tốt nghiệp. Nếu gameplay facts khác canon, author giải quyết, không ép FAIL hoặc xóa PASS để khớp số.
- [ ] Project có artifact, yêu cầu, kiến thức, duration, deadline và quality theo từng phần. Debug dùng lỗi deterministic có nguồn; test đúng phiên bản; demo/report/Q&A có reason và recovery, không nút progress hoặc cutscene thay gameplay.
- [ ] Có ít nhất một crisis project lỗi + hạn gần + máy khách chờ + thi sắp tới; mọi lựa chọn làm mất thời gian/cơ hội thật. Thiếu ngủ ảnh hưởng qua chuỗi causal được author, không penalty ngẫu nhiên.
- [ ] Repair nhiều giả thuyết/component chỉ mở với skill/tools hợp lệ; có consent, báo giá/approve, linh kiện/cost/thời gian giao nếu cần, stress test, bàn giao/ledger. Chốt job/giá/linh kiện/test và reputation rules trước triển khai, không invent model hay reward.
- [ ] Exam dùng eligibility/attendance/knowledge/assignments/recovery/attempt; fail giữ reason và kế hoạch học lại, không reset năm. Demo đồ án nhỏ khác kết quả course và không thay quyền đăng ký đồ án tốt nghiệp.
- [ ] M27 chỉ xét quyền đăng ký đồ án, có reasons/recovery; project demo chỉ là đồ án nhỏ. CV Lươn nếu có khác CV Long; không invent offer/company/lương/giáo sư/đề tài/điểm hoặc điều kiện tốt nghiệp.
- [ ] Relationship payoff dùng history nhiều năm, gồm lựa chọn thật/giấu với gia đình; một final choice không ghi đè mọi quan hệ. Recap chỉ đọc event thật, không auto happy ending.
- [ ] Tiền có ledger, không trùng montage/ca/job/story; save/reload không duplicate reward/payment/submit/attempt/offer/ảnh/giao sách. Hóa đơn/preview không tự trừ tiền; giữ ví/nợ/ca tồn qua năm.
- [ ] Snapshot cuối năm có credits/PASS/FAIL/reasons/graduation state/knowledge; project artifacts/CV/interview/job state; ví/nợ/repair income; skill/reputation/job history; Long/bạn bè/gia đình; major choices, phòng, inventory, lịch và obligations chưa xong.
- [ ] Reuse session/controller/store/wallet/course/repair/time/UI hiện có; nối F8 production vào cơ chế state/restore, không coi F8 Room404_MVP là evidence production. Trước restore phải reset phiên/coroutine/timer/reservation/UI, rồi restore snapshot đầy đủ. Fixture tách save chơi chính.
- [ ] Checkpoint dự kiến bên dưới phải được tạo khi triển khai; hiện **chưa có runtime/F8 mới**. Các checkpoint project/demo chưa author là acceptance bị chặn nội dung; không có fixture tốt nghiệp/phỏng vấn nhận việc/ending Năm 4:

| Checkpoint | Thao tác người dùng | Kết quả mong đợi |
|---|---|---|
| CP-ENTRY | Load đầu Năm 4, xem portal/ca tồn | Đủ state T-003, đúng time/pose/UI, chưa giao sách/nhận lại reward |
| CP-REG | Refresh/chọn HĐH khi CTDL chưa đạt; sửa draft | Khóa đúng reason; confirm chỉ lưu lịch hợp lệ, không trừ tiền hai lần |
| CP-PROJECT | Nhận project hợp lệ hoặc thử khi chưa đủ điều kiện | Chỉ mở đúng loại/phạm vi; không vượt gate đồ án hoặc tự tăng progress |
| CP-MILESTONE | Run lỗi, đọc log, patch rồi retest | Thời gian tiến, quality/evidence theo artifact, fail có recovery |
| CP-REPAIR | Diagnosis sai/khách reject hoặc test lại | Giữ consent/data/ownership; không tự thu hoặc phạt từ chối |
| CP-CONFLICT | Chọn project/khách/ôn/ngủ ở các fixture riêng | Deadline, tiền và giờ ngủ khác thật; không hoàn tất tất cả miễn phí |
| CP-EXAM | Đi thi sau đêm thức sửa, submit rồi reload | Arrival/eligibility theo timeline, attempt/result chỉ lưu một lần |
| CP-DEMO | Demo có bug/report thiếu, fix/review | Outcome có reason đúng evidence; không tự nhận điểm đồ án |
| CP-ELIGIBILITY | Xét M27 | Nêu điều kiện đăng ký đồ án còn thiếu; không cấp bằng hoặc mở đồ án tốt nghiệp |
| CP-BRIDGE | Recap và chuyển Năm 5 | Snapshot đầy đủ, chưa Finished, không reset ví/room/ca tồn |

- [ ] Fixture có biến thể fail/recovery, save lỗi, hủy preview và restore giữa project/repair/exam; kiểm tra đúng time/position/pose/course/project/repair/inventory/money/relationship/story/UI sau load.
- [ ] Ưu tiên slice: entry/registration → một ngày học → một project milestone hợp lệ → ca repair → conflict → save/restore; playtest theo T-007 → sửa core loop → mới mở rộng story/route. Không author đủ project thì chưa tuyên bố slice đạt.
- [ ] Người dùng nghiệm thu gameplay/UI/animation theo checkpoint → thao tác → kết quả mong đợi, ghi evidence riêng. Task này chỉ tạo spec và rà diff/tham chiếu; chưa chạy test/lint/compile/Unity Play Mode/build hoặc kiểm chứng runtime.
