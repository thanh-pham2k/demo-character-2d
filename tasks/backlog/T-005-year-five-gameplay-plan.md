# T-005 — Hoàn thiện flow gameplay Năm 5

## TLDR

**“Mình có kịp tốt nghiệp không?”** Năm 5 là đỉnh áp lực của đời sinh viên: môn nợ, chuẩn bị đồ án, internship có điều kiện, repair, tiền, gia đình và bạn bè rời 404 cùng tranh một lịch. Chưa tốt nghiệp, chưa mở đời sống nhân viên full-time; Năm 6 mới xử lý môn cuối, đăng ký/hoàn thiện đồ án, bảo vệ, dọn 404 và tốt nghiệp. Theme: sống với lựa chọn và hậu quả bốn năm trước.

Kế thừa [T-004](T-004-year-four-gameplay-plan.md), theo [baseline sáu năm](../../story/PRODUCTION_FLOW_BASELINE.md), [transition T-006](T-006-event-driven-transitions.md) và [gate T-007](T-007-vertical-slice-playtest.md). Spec này thay phiên bản career/workday/payroll/incident/promotion/job-change trước đây; các nhánh tốt nghiệp Năm 4/có việc toàn thời gian đầu Năm 5 đã bỏ khỏi tuyến chính. Task slice cũ vẫn ở T-007. Đây là thiết kế, chưa triển khai runtime hoặc checkpoint F8.

Giữ ID M29–M36, nhưng M35 trải qua cuối Năm 5 → đầu Năm 6: CTDL kỳ 9/10 → có PASS mới mở HĐH kỳ 11; M36 xét điều kiện đầu Năm 6, không cuối Năm 5 nữa. Lịch lớp/thi cụ thể **CẦN AUTHOR**. Chuẩn bị đồ án Năm 5 không đồng nghĩa đã đăng ký chính thức, không cấp tín chỉ đồ án hoặc bypass tiên quyết. Company/NPC/internship/task/thu nhập chưa có thì **CẦN AUTHOR**, không lấy offer của Long làm offer Lươn.

## Sơ đồ ASCII

```text
[Y5-ENTRY / CP-ENTRY] ĐẦU KỲ 9 — 404, SAU M28
  │ Input: 118/150 mục tiêu, CTDL nợ, HĐH khóa; không reset PASS/reasons
  │ Attendance/gaps/assignments, project nhỏ Năm 4/artifacts/knowledge
  │ Ví/nợ tiền/học phí/ledger, repair skill/reputation/customers/ca tồn
  │ Consent/backup/test/tools/parts, Long/Tộ/Đạm/gia đình/history/choices
  │ Room/giường/đồ đạc, time/position/pose/UI, fatigue/lịch/obligations
  ▼
Restore → recap những việc chưa xong → portal/lịch học/nợ và ngân sách
  ├─ Mâu thuẫn snapshot → giữ save, mapping/migration CẦN AUTHOR
  └─ Hợp lệ → lập kế hoạch CTDL trước, HĐH sau; chưa đủ đồ án tốt nghiệp
  │ Save cũ có graduation/job state từ route đã bỏ: không xóa âm thầm;
  │ giữ bản gốc, migration/fixture cô lập CẦN AUTHOR trước khi chuyển canon
  ▼
[M29] LONG DỌN ĐỒ — 404 → CỔNG KTX, GIỜ/DURATION CẦN AUTHOR
  → routine → kiểm tra lịch lớp/ca sửa → nhấc/khiêng đúng thùng Long
  → đưa xuống cổng → chào/tiễn → Long rời → giường trống
  → chiều đã author người mới 1 nhận đúng slot, không lấy đồ người còn ở
  │ Chậm giúp Long làm mất giờ học/repair theo lịch; quan hệ theo history
  ▼
[M30] MỘT NGÀY KHÁC — TỘ CHUYỂN TRỌ
  → nhận gói thịt khô → ôm/chào → tiễn Tộ → slot trống → người mới 2 vào
  │ Gift một lần, không tự đổi thành tiền hoặc xóa mệt ngoài rule author
  ▼
[M31] ĐẠM BÁO NGÀY ĐI — CỬA 404, NGÀY/GIỜ CẦN AUTHOR
  → nghe kế hoạch → đối thoại theo lịch sử thân/xa cách → hẹn sau M34
  → Đạm vẫn ở phòng; chưa người mới 3, chưa bay hoặc rời KTX
  │ Prototype Years45Activities còn departure/freshman3 ở M31: phải sửa khi
  │ implement theo baseline; task/spec không chứng minh runtime đã sửa
  ▼
[M32] UNINSTALL / LẬP KẾ HOẠCH — ĐÊM Ở 404
  → ngồi laptop → chọn game → giữ đúng nút gỡ theo mechanic hiện có
  ├─ Nhả/hủy trước hoàn tất → chưa commit gỡ, mất thời gian đã dùng
  └─ Hoàn tất → lưu app đã gỡ một lần → đặt báo thức 06:30
  → ghi lịch trả CTDL, dự kiến HĐH kỳ 11 và công việc đang cam kết
  │ Không cấp knowledge/credits hoặc xóa fatigue vì gỡ game
  ▼
[M??] CẦN AUTHOR — ĐĂNG KÝ KỲ 9/10 / CP-PLAN
  → routine → ngồi/portal → refresh → chọn lớp từ catalog → prerequisite
  → xem trùng lớp/repair/internship/hẹn gia đình → preview lịch/cost
  ├─ Sai điều kiện/trùng lịch/thiếu tiền → reasons → chỉnh draft/ưu tiên
  └─ Đủ → confirm → commit/save/checkpoint; chưa có biên lai thì chưa đóng phí
  │ CTDL chưa PASS → HĐH chưa mở; GT2/VL1/KTLT PASS giữ nguyên
  ▼
[M33] 06:30 — THỨC DẬY / NGÀY HỌC ĐẠI DIỆN
  → ngồi dậy/xuống đúng giường/thang → WC/vệ sinh → thay đồ → sách/balo
  → trạm → bus → H6 → vào lớp/điểm danh → học phần thực tham gia
  ├─ Kịp → attendance thật; không cấp cả môn từ một câu hỏi
  └─ Trễ/vắng → gap/reason thật → học bù có cost, không teleport cho kịp
  → bus về/404 → cất balo → xem bài, máy khách, project, ví và giờ ngủ
  │ Nhãn prototype 06:40 H6/06:45 lớp không thay duration routine/travel;
  │ cần author lịch khả thi, không ép “đúng giờ” bằng outcome sẵn
  ▼
[Y5-LOOP] HỌC + REPAIR + CHUẨN BỊ ĐỒ ÁN + INTERNSHIP + NGHỈ/QUAN HỆ
  ├─ Học → chọn gap → làm bài → test/sửa bài → submit/receipt
  ├─ Chuẩn bị đồ án → đọc yêu cầu → research → ghi chú/thiết kế/prototype
  │    → run/debug/test → lưu artifact theo version và bằng chứng
  │    Chưa đăng ký/duyệt nên chỉ là chuẩn bị; M37 phải review lại
  ├─ Repair → consent/backup → hỏi/test/diagnosis → báo giá/approve
  │    → sửa/linh kiện nếu hợp lệ → test → nghiệm thu/giao → ledger
  ├─ Internship hợp lệ → quy trình ngắn bên dưới; không auto nhận theo năm
  └─ Ăn/ngủ/gọi mẹ/gặp Long/bạn → duration thật, phản hồi theo history
  │ Một khoảng giờ chỉ làm được việc tương thích; không hoàn tất mọi nhánh
  ▼
[M??] CẦN AUTHOR — INTERNSHIP / CP-INTERN
  │ Company/NPC/nơi/eligibility/lịch/task/review/thu nhập nếu có cần author
  → xem cơ hội → CV từ kinh nghiệm thật → apply/interview nếu được yêu cầu
  ├─ Không đủ/chưa được nhận/từ chối → tiếp tục học/repair, không phạt mặc định
  └─ Có nhận hợp lệ → preview lịch/cost/overlap → xác nhận
       → routine/travel/check-in → đọc requirement → clarify khi chưa rõ
       → implement/fix → run/test → đọc review → discuss/fix/retest
       → submit/delivery → feedback có evidence → về 404
  │ Không workday grind/incident/promotion/job hopping; chỉ một task đại diện
  │ Không cộng lương vì DONE. Thu hỗ trợ nếu có chỉ tại event/ledger author
  │ Internship chưa có dữ liệu không được chặn đường học tiếp của Lươn
  ▼
[M??] CẦN AUTHOR — CRISIS / CP-CONFLICT
  │ Bài CTDL sắp hạn + prototype lỗi + máy khách chờ + khoản học phí đến hạn
  │ + việc internship/gia đình nếu thực có, fatigue đã tích lũy
  → xem thời gian/vốn/consent/điều kiện và hạn từng việc
  ├─ Ưu tiên học → làm/nộp thật; báo khách/internship nếu trễ, mất cơ hội thu
  ├─ Ưu tiên repair → test/giao mới thu tiền; mất giờ bài/prototype/ôn/ngủ
  ├─ Ưu tiên internship → xử lý task; việc trường/khách vẫn còn tồn
  ├─ Ưu tiên gia đình/nghỉ → báo/đổi hẹn nếu cho phép, chưa hoàn tất việc khác
  └─ Thương lượng/chia lịch → chỉ khả thi theo time/cost/consent thật
  → timeline tiến → deadline/overlap → consequence → history → recovery
  │ Không random burnout/bug, không “nhận job → có tiền”, không tự trợ cấp
  ▼
THIẾU NGỦ + TẢI TÍCH LŨY → FATIGUE THEO RULE CẦN AUTHOR
  → routine chậm/ít giờ tập trung → bỏ bước test hoặc đến lớp muộn
  → rework/gap/trễ hẹn → recap causal chain → nghỉ/giảm tải/học bù có cost
  │ Thất bại không reset năm; hủy UI không hoàn tác thời gian đã commit
  → quay Y5-LOOP khi còn ngày đại diện đã author
  ▼
[M34] 01:00 — TIẾP TẾ TỪ LONG/TỘ/ĐẠM
  → gõ cửa → người chơi mở → ra hành lang/khu sinh hoạt chung
  → nhận/chia đồ ăn, nghe/kể chuyện → nhận tài liệu CTDL → cảm ơn/tiễn
  │ Không đánh thức hai người mới; Đạm vẫn ở 404 đêm này
  → preview sáng sau → Đạm thực sự rời → giường trống → người mới 3 mới vào
  │ Rời KTX không phải bay ngay; chuyến bay và gặp lại cần lịch author
  ▼
[M35-A / CP-EXAM] CUỐI NĂM 5 — CTDL KỲ 9/10
  → xem eligibility/assignment/gaps → ôn hoặc nhận ca có conflict thật
  → routine/travel/H6 → attempt → nộp → receipt → ngày công bố điểm
  ├─ Chưa đạt → giữ FAIL/reasons; recovery/kỳ thi lại cần author, HĐH khóa
  └─ Đạt → PASS/credits một lần → mới có điều kiện đăng ký HĐH kỳ 11
  │ Không ép PASS để khớp storyline; không cho học môn sau bằng điểm tương lai
  ▼
[Y5-HANDOFF / CP-BRIDGE] CUỐI NĂM 5 → NĂM 6
  → recap debt còn thật + progress chuẩn bị đồ án + internship history
  → đối soát tiền/học phí/repair/khách/đồ khách, quan hệ và fatigue
  → preview đầu kỳ 11: môn cuối, lịch HĐH nếu đủ điều kiện, nghĩa vụ tồn
  → confirm → commit/save/checkpoint đầy đủ → chưa Finished/chưa tốt nghiệp
  │ Giữ quyền ở 404 theo dữ liệu; không tự chuyển nhà hoặc xóa ba người mới
  ▼
[M35-B] ĐẦU NĂM 6 — HĐH SAU CTDL PASS
  → đăng ký hợp lệ → học/assignment/ôn → thi/nộp → kết quả/recovery thật
  │ Người đã PASS ở save hợp lệ không bị bắt trượt/thi lại để kéo năm
  ▼
[M36] SAU ĐIỂM MÔN CUỐI — 136/150 + HẾT NỢ TIÊN QUYẾT
  → portal đối soát → đủ mới nhận form/lịch GVHD M37
  │ Chưa đủ thì nêu reason và recovery; không cấp bằng hoặc mở đồ án sớm
  ▼
NĂM 6: M37–M43 ĐỒ ÁN/BẢO VỆ → M44 HỌC PHÍ/DỌN GÓC 404/TRẢ KHÓA
  → M45 TỐT NGHIỆP/FLASHBACK/CREDITS → EPILOGUE VIỆC LÀM CÓ ĐIỀU KIỆN
```

Mỗi milestone phải author đủ thời gian/địa điểm, input, routine, thao tác, choice, duration, hậu quả, recovery, preview và commit/save/checkpoint. `[M??]` không phải ID/API đã tồn tại. Project nhỏ/chuẩn bị có artifact và quality thật: yêu cầu/research/design/code/test/report; run → log → root cause → patch → retest, không progress giả hoặc bug random. Repair skill/uy tín chỉ đổi theo evidence; từ chối trước cam kết không auto penalty. Phí/linh kiện/model/khách/giá, đề project/internship và ngày tuyệt đối thiếu nguồn đều **CẦN AUTHOR**.

```text
ACTION → time cost → clock advances → check overlap → consequence/history
  → commit state + effect → save → checkpoint
SLEEP/SKIP/TRAVEL → source/target time và nơi → duration → missed events
  → preview → confirm → commit → save → summary/checkpoint → trả input
Hủy preview → giữ mốc nguồn; save lỗi → chưa báo hoàn tất, retry cùng effect
```

Không montage qua choice/thi/repair/deadline hoặc tự cấp attendance/bài nộp. Ledger giữ nguồn/ID/time/amount/reason/receipt, gồm repair, học phí, sinh hoạt, linh kiện và thu/chi internship chỉ nếu được author; không payroll/salary/promote ngầm, không auto-zero hoặc double count montage. Thiếu tiền có nhánh ưu tiên/hoãn/thỏa thuận/nguồn hợp lệ được author; chưa có dữ liệu không tự vay/cấp tiền. Snapshot giữ time/pose/UI, course/attempts, project/internship artifacts, repair/ownership/payments, money/debt, room/inventory, fatigue/calendar và relationship/history.

## Definition of Done

- [ ] Bỏ route tốt nghiệp Năm 4 và đời nhân viên full-time Năm 5. Chốt Năm 5 áp lực sinh viên; Năm 6 là payoff và ending, đi làm chỉ sau credits có điều kiện.
- [ ] Handoff T-004 đầy đủ và không reset; save cũ từ route đã bỏ có mapping/migration riêng, giữ bản gốc. 118→136→150 phải có course/result mapping, không sinh môn/tín chỉ hoặc ép FAIL/PASS.
- [ ] M29/M30/M31/M34 đúng room occupancy: Long/Tộ rời trước, Đạm vẫn ở đêm tiếp tế, người mới 3 sau sáng Đạm rời. Khi triển khai sửa lệch prototype departure M31, không coi spec là runtime đã đạt.
- [ ] M32 gỡ game không cấp knowledge; M33 dùng duration routine/bus thật; mỗi ngày có học–bài–repair–tiền–quan hệ–ngủ, recovery có chi phí và recap nguyên nhân.
- [ ] Author project nhỏ/chuẩn bị đồ án và internship phù hợp sinh viên. Không đăng ký đồ án trước M36, không cấp credits cho preparation, không dùng internship giả để chặn progression. Có task thật với requirement/clarify/implement/test/review/delivery; không mở bộ simulator nhân viên mới.
- [ ] Có ít nhất một crisis môn nợ + chuẩn bị đồ án + khách + tài chính, thêm internship/gia đình khi thực có; không tối ưu mọi việc miễn phí, không random burnout hoặc game over vì một thất bại.
- [ ] CTDL kỳ 9/10 có evidence/attempt/result; HĐH kỳ 11 sau CTDL PASS; M36 chuyển sang đầu Năm 6. Author khung lớp/thi/recovery khả thi, không giới thiệu học kỳ mới như một đêm ngủ.
- [ ] Đủ duration/time/địa điểm/cost/rule cho từng event trước implement; timeline chung, source→target/duration/missed events→confirm→commit/save/checkpoint, không teleport, montage reward hoặc auto trợ cấp.
- [ ] Ledger/idempotency giữ money/fees/repair/internship và obligations xuyên năm; học phí cuối Năm 6 là khoản còn thực nợ, không tự sinh khoản phạt để tạo drama.
- [ ] Dùng lại production progress/session/store, Years45 adapter và time/portal/repair/ledger/relationship/dialogue/travel/inventory hiện có; chỉ mở rộng state cần thiết, không duplicate controller/save/wallet.
- [ ] F8 dùng cơ chế snapshot/restore hiện có, reset phiên/coroutine/timer/reservation/UI rồi restore; fixture tách save chính. Các checkpoint sau là yêu cầu, **chưa triển khai F8/runtime**:

| Checkpoint dự kiến | Thao tác | Kết quả mong đợi |
|---|---|---|
| CP-ENTRY | Load snapshot T-004 | Đúng 404/time/pose/UI, debt/ví/ca tồn/history; chưa tốt nghiệp |
| CP-PLAN | Chọn HĐH khi CTDL còn nợ, chỉnh lịch | Khóa đúng reason; không tự đăng ký/trừ phí hoặc xóa PASS |
| CP-ROOM | Load trước M31/M34 và tiến thời gian | Đạm còn ở đêm tiếp tế, người mới 3 chỉ vào sau giường trống |
| CP-INTERN | Nhận/từ chối opportunity hợp lệ, làm task | Có time/artifact/review; chưa tự offer/lương hoặc job full-time |
| CP-PROJECT | Run lỗi preparation, patch/test | Quality theo version/evidence, không cấp quyền đồ án hoặc progress giả |
| CP-REPAIR | Diagnosis sai, test lại/trả máy | Giữ consent/ownership, tốn thời gian, payment một lần |
| CP-CONFLICT | Ưu tiên học/repair/internship/ngủ | Việc khác chậm/lỡ thật; hậu quả/gap/fatigue/quan hệ có nguồn |
| CP-EXAM | Thi CTDL, nộp/reload hoặc fail | Attempt/result/credits một lần; fail giữ HĐH khóa và recovery |
| CP-BRIDGE | Chuyển đầu Năm 6 rồi reload | Giữ mọi progress/obligation/room; chưa M36/PASS/bằng/Finished giả |
| CP-ELIGIBILITY | Năm 6 sau môn cuối, xét M36 | Đủ mới mở M37, thiếu thì reasons; không auto tốt nghiệp |

- [ ] Verify restore giữa repair/project/internship/exam, save lỗi và hủy preview; không duplicate payment/reward/submit/course result/gift/room transition sau reload/retry.
- [ ] Slice ưu tiên một ngày học–repair–preparation, crisis và save/restore; internship chỉ thêm khi author đủ. Playtest/sửa core loop theo T-007 trước mở rộng; kiểm tra bridge Năm 6 và gate prerequisite riêng.
- [ ] Người dùng tự nghiệm thu gameplay/UI/animation qua checkpoint → thao tác → kết quả mong đợi. Hiện chỉ sửa spec/tham chiếu, chưa chạy test/lint/compile/Unity Play Mode/batch/build hoặc nghiệm thu runtime.
