# Năm 6 — Payoff đời sinh viên phòng 404

Thiết kế cập nhật 26/10/2026 theo [baseline](PRODUCTION_FLOW_BASELINE.md), kế thừa [T-005](../tasks/backlog/T-005-year-five-gameplay-plan.md). Đây là plan, chưa triển khai runtime hoặc F8. Không mở thêm năm đi làm: **“6 năm vừa rồi có ý nghĩa gì?”**

## Flow đã chốt

```text
CUỐI NĂM 5 → ĐẦU NĂM 6 / KỲ 11
  → restore debt/credits/reasons, preparation/internship, ví/ca tồn/đồ khách
    history/quan hệ/fatigue, 404/người mới, time/pose/UI và lịch
  → recap nghĩa vụ còn thiếu, không reset hoặc gọi nhân vật đã tốt nghiệp
  ▼
[M35-B] MÔN CUỐI
  CTDL PASS → đăng ký HĐH → routine/lớp/bài tập/ôn → lịch thi/eligibility
  → chuẩn bị/balo/bus/H6 → attempt → nộp → receipt → ngày portal công bố
  ├─ FAIL/thiếu điều kiện → reasons + học bù/thi lại theo cửa author
  └─ PASS có evidence → commit một lần; người đã PASS không thi lại
  ▼
[M36] XÉT QUYỀN ĐỒ ÁN — ĐẦU NĂM 6, SAU ĐIỂM MÔN CUỐI
  → đối soát course facts → đủ 136/150 + tiên quyết mới mở form/lịch GVHD
  → chưa đủ thì giữ debt/recovery, không nhận form bằng điểm tương lai
  ▼
[M37] GẶP GVHD / ĐĂNG KÝ CHÍNH THỨC
  → xem lịch → routine/travel → gặp thầy → trình bày problem/phạm vi
  → đề tài: hệ thống tự động hóa và chẩn đoán lỗi thiết bị mạng
  → review preparation Năm 5 → điều chỉnh/duyệt → lưu kế hoạch milestone
  │ Không tạo đề tài thứ hai hoặc tự công nhận mọi prototype cũ đã đạt
  ▼
[M38–M41] RESEARCH → DESIGN → CODE/TEST → DEMO/FEEDBACK → FIX → REPORT
  → artifact/version/quality/test evidence thực, không progress +20%
  → sai design/bug/report thiếu → feedback rõ → sửa/retest có cost
  → demo 1 và demo 2 là lịch riêng, không cùng một đêm code
  → report 120 trang theo canon; phí in chỉ khi có giao dịch author
  │ Không lãng mạn hóa thiếu ngủ: thức khuya có cost và lựa chọn chia tải
  ▼
[CA REPAIR QUAN TRỌNG CUỐI — NÚT CON, CẦN AUTHOR]
  → chỉ khi có ca tồn hoặc customer/job hợp lệ, không tự spawn khách/reward
  → preview conflict với deadline đồ án → nhận/hoãn/từ chối theo consent
  → diagnosis/repair/test/handover/payment hoặc trả máy/hoàn khoản thực có
  → đóng hoặc bàn giao obligation rõ trước rời 404
  │ Vị trí trong lịch M38–M42 cần author; không tự chèn sau hạn nộp/bảo vệ
  ▼
[M42] DEADLINE CUỐI — 22:45 → HẠN 23:45 THEO BASELINE
  → reproduce → log → root cause → patch → test → chọn bản nộp
  ├─ Chưa đạt/trễ → hậu quả/cửa sửa/nộp lại CẦN AUTHOR, chưa mở bảo vệ
  └─ Submit hợp lệ → receipt timestamp thật → backup → lịch bảo vệ
  │ 23:44:32 là mẫu, không ép mọi lượt nộp đúng giây; không nhân receipt
  ▼
[M43] BẢO VỆ
  → xem project/report/build/eligibility → chuẩn bị → travel → setup
  → demo → câu hỏi/phản biện từ yêu cầu/kiến thức/evidence → kết quả
  ├─ Chưa đạt → reasons và recovery có cost/khung hợp lệ được author
  └─ Đạt → lưu kết quả; chỉ A nếu đáp ứng tiêu chí author, không ép A
  │ Lượt tập dượt/retry fixture khác kết quả chính thức, không rewrite save
  ▼
[M44-A] KẾT QUẢ + HỌC PHÍ CUỐI + NGHĨA VỤ
  → gọi mẹ báo “bảo vệ xong” đúng state, chưa tự nói đã có bằng
  → portal đối soát kết quả/credits và hóa đơn thực còn nợ
  ├─ Thiếu điểm/tiền → nêu chính xác reason/khoản thiếu → recovery hợp lệ
  │    Không tự cấp tiền, sinh nợ mới hoặc tịch thu reward để tạo conflict
  └─ Đủ → confirm thanh toán nếu cần → ledger/receipt một lần
       → xác nhận đủ điều kiện nhận bằng theo requirement đã author
  → giải quyết ca/đồ khách/tiền đã thu chưa hoàn tất
  │ Được rời KTX theo quyền ở khác với đủ bằng; không giữ chìa khóa để
  │ bypass ngày hết quyền ở. Nếu phải rời sớm: author nơi ở/lịch recovery,
  │ giữ trạng thái chưa đủ nhận bằng cho tới khi hoàn tất thật
  ▼
[M44-B] RỜI PHÒNG 404 — CHÍNH TAY NGƯỜI CHƠI
  → về 404 → xem đồ của mình/đồ người mới → tháo đồ mình khỏi tường
  → gấp chăn → cất laptop/giáo trình/tuốc-nơ-vít → lấy balo
  → kiểm tra góc mình không sót đồ → nhìn lại các chỗ từng có kỷ niệm
  → tắt đèn cá nhân/đèn chung khi không ảnh hưởng người đang dùng
  → khép cửa → xuống bàn giao/trả chìa khóa → receipt/checkpoint
  │ Chưa đóng đủ điều kiện lễ thì không chuyển sang nhận bằng
  │ Không xóa ba người mới hoặc đồ của họ để tạo phòng trống
  │ Long/Tộ/Đạm đã rời Năm 5: không cho chuyển đi lần hai
  ▼
[M45] LỄ TỐT NGHIỆP / NHÌN LẠI SÁU NĂM
  → có thư mời + eligibility thật → preview tới ngày lễ → routine/travel A5
  → nhận bằng → gặp gia đình/bạn bè theo availability đã author
  → hội ngộ/tạm biệt đời sinh viên → travel Bình Đông → đi cùng anh em
  → flashback Năm 1→6 theo history save: giường/bàn/laptop/lẩu/balo/Long
  → góc Lươn trống hoặc hình ảnh phòng cũ trong ký ức, không xóa người mới
  → “Tưởng mình đến đây để học một ngành. Cuối cùng lại học cách sống.”
  → credits → lưu Finished
  ▼
EPILOGUE SAU CREDITS
  ├─ Có offer/lời hẹn nhận việc + lịch bắt đầu hợp lệ
  │    → điện thoại: “Ngày mai đi làm nhớ tới sớm nhé.” → cut to black
  └─ Chưa có → kết thúc ở payoff 404; không tự tạo việc/lương/công ty
  → menu; không mở gameplay nhân viên/career progression
```

## Continuity và dữ liệu cần author

- Giữ 45 ID; M35 đi qua ranh giới năm, M36 đầu Năm 6. Lớp/thi HĐH kỳ 11, thời lượng recovery và các mốc M37–M43 phải có lịch khả thi; thiếu dữ liệu không tự giả một học kỳ chỉ dài một đêm. Nếu cần kéo dài vì FAIL, nói rõ chậm tiến độ, không ép PASS để khớp năm.
- “Môn cuối” là nợ thật từ snapshot; không thêm môn mới hoặc trượt giả. Mapping 118→136→150 và tiêu chí A/đủ tốt nghiệp/được nhận bằng, học phí còn nợ CẦN AUTHOR; A bảo vệ không tự tương đương mọi điều kiện đã đủ.
- Ca repair cuối cần author khách/lỗi/giá/duration hoặc chọn ca đang tồn, có quyền trả/hoãn và consequence. Không là nút phát tiền cứu phí cuối. Internship kết thúc/bàn giao lịch và đồ từ Năm 5, không tiếp tục thành full-time loop Năm 6.
- Đạm đã rời để du học; nếu có mặt ở lễ cần author hành trình/ngày về, không “bay về” tự động. Gia đình/bạn bè có hẹn/travel phù hợp. Không dựng fake history trong recap; một lời thoại cuối không thay quan hệ nhiều năm.
- Flashback là payoff sau gameplay, không thay thao tác đóng phí/dọn đồ/trả khóa. Đồ của Lươn trong save quyết định thứ gì cần cất; không lấy laptop đang là máy khách hoặc tháo đồ thuộc người mới.
- Mỗi thao tác có duration/cost; sleep/skip/travel có source→target/duration/missed events→confirm→commit/save/checkpoint. Save lỗi chưa báo hoàn tất; reload không duplicate credits/result/payment/repair/gift/receipt/key return/degree/Finished.
- Tài liệu runtime/review cũ là lịch sử triển khai, chưa được sửa code theo canon mới. Đặc biệt adapter Years45 còn cho Đạm rời M31, HĐH kỳ 10 và M36 cuối Năm 5; khi implement phải migration/đổi data trước kiểm thử, giữ save gốc. Không tuyên bố đã khớp runtime chỉ vì đổi tài liệu.

## Checkpoint nghiệm thu dự kiến

F8 production phải dùng state/restore hiện có, reset phiên/coroutine/timer/reservation/UI trước load; fixture tách save chơi chính. Snapshot giữ academic/attempts/project versions/evidence/internship history/repair/ownership/money/debt/room/inventory/relationship/history/fatigue/calendar/time/location/pose/UI. Chưa có các checkpoint mới trong runtime.

| Checkpoint | Thao tác | Kết quả mong đợi |
|---|---|---|
| Y6-ENTRY | Load handoff T-005 | Đủ debt/project/ví/ca tồn/room/history; chưa có bằng hoặc job giả |
| Y6-LAST-COURSE | Thi môn cuối, fail hoặc nộp/reload | Kết quả/reasons thực, không lặp credits; thiếu thì M36 chưa mở |
| Y6-ELIGIBILITY | Xét M36 | Đủ 136 + prerequisites mới mở M37; chưa tốt nghiệp |
| Y6-PROJECT | Mở preparation, sửa/test/demo | Artifact phải qua duyệt, không tự hoàn tất; feedback có recovery |
| Y6-LAST-REPAIR | Chọn nhận/hoãn, test/giao hoặc trả máy | Lịch/ledger/ownership đúng, không có tiền miễn phí |
| Y6-DEADLINE | Debug/nộp rồi reload | Receipt đúng version/time, một lần; trễ có recovery |
| Y6-DEFENSE | Demo/Q&A với state còn thiếu | Outcome theo evidence; tập dượt không cấp A hoặc bằng |
| Y6-FINAL-FEE | Đối soát/đóng phí hoặc đã hết nợ | Chỉ trừ khoản thật một lần; thiếu điều kiện chưa nhận bằng |
| Y6-PACK | Tháo đồ/gấp chăn/cất balo/nhìn lại | Chỉ dọn đồ Lươn, giữ người mới và đồ khách chưa xử lý |
| Y6-KEY | Khép cửa/trả khóa, reload | Key/receipt đúng một lần; chưa Finished |
| Y6-GRADUATION | Nhận bằng, hội ngộ/flashback/credits | Đủ điều kiện thật; history đúng nhánh; Finished sau credits |
| Y6-EPILOGUE | Load có/không có lời hẹn hợp lệ | Chỉ có tin công việc khi có nguồn, không thêm gameplay đi làm |

Người dùng tự nghiệm thu checkpoint → thao tác → kết quả mong đợi. Hiện mới rà tài liệu/tham chiếu, chưa chạy test/lint/compile/Unity Play Mode/build hoặc kiểm chứng runtime.
