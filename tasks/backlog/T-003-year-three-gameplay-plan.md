# T-003 — Hoàn thiện flow gameplay Năm 3

## TLDR

Thiết kế tuyến liên tục M17–M23: nhận ca đầu, học lại và học tiếp, kiếm tiền sửa máy, cân đối lịch học–khách–giấc ngủ, quan hệ bạn bè, thi và xem kết quả; bàn giao yêu cầu mang sách cho Đạm ở M24 sang Năm 4. Đây là task/spec, chưa triển khai runtime hoặc checkpoint F8.

Nguồn: [T-002](T-002-year-two-gameplay-plan.md), [baseline](../../story/PRODUCTION_FLOW_BASELINE.md), [transitions](../../story/GAMEPLAY_TRANSITIONS_PLAN.md), [systems](../../story/GAMEPLAY_SYSTEMS_PLAN.md). Tái sử dụng production session/progress/store, module Year3 M16–M23 và activities hiện có; M16 thuộc bàn giao Năm 2, không chạy lại. Checklist prototype chỉ là nguồn thao tác, chưa chứng minh gameplay, thời gian, save hoặc agency đã đạt.

Giữ canon: M17 công 50.000đ; M19 kỳ 5 KTLT/VL1 đạt, kỳ 6 mở CTDL; M20 miễn công; M23 GT2 đạt, CTDL chưa đạt, tổng 78/150. Không tự áp kết quả nếu lịch sử chơi không hỗ trợ. **CẦN AUTHOR** cách nối từng kết quả với attendance, bài tập, kiến thức và bài thi; nếu nhánh chơi mâu thuẫn canon, dừng gate kết quả để chốt rule thay vì ép FAIL hoặc xóa PASS. Snapshot cuối Năm 2 có thể đang mâu thuẫn mapping môn Năm 1; phải giải quyết trước khi cấp credits Năm 3.

## Sơ đồ ASCII

Các nhãn Y3-* bên dưới là nút thiết kế, không phải API hay checkpoint đang tồn tại.

```text
[Y3-ENTRY] SAU M16 — 404, TRƯỚC NGÀY KHÁCH TỚI
  │ Input: credits, PASS/FAIL + reasons, debt, prerequisites, tiền/hóa đơn
  │ + relationship, story/history, attendance/gaps, inventory, lựa chọn Năm 1–2
  │ + giường/đồ đạc/phòng 404, lịch, thời gian, vị trí/pose và UI
  ▼
Kiểm tra handoff → recap lời mẹ, học phí chưa trả và lịch còn lại
  ├─ Thiếu/mâu thuẫn snapshot → giữ save, yêu cầu mapping/migration rõ ràng
  └─ Hợp lệ → dùng state thật; không reset ví, quan hệ hoặc môn đã PASS
  │ Preview ngày sau từ M16; giờ/ngày cụ thể CẦN AUTHOR
  ▼
Routine sáng ở 404 → xuống đúng giường/thang → vệ sinh → thay đồ
  → xem lịch → tới bàn, dọn chỗ nhận máy; mỗi bước dùng thời gian
  → [CP-FIRST] trước khách đầu tiên
  │
  ▼
[M17] KHÁCH ĐẦU TIÊN MANG LAPTOP TỚI SỬA — 404 / BÀN SỬA
  │ Giờ hẹn CẦN AUTHOR; input: cuộc gọi kết thúc, khách đã hẹn, bàn sẵn
  ▼
Tiếp khách → nhận máy → hỏi triệu chứng → ghi tình trạng/đồ nhận kèm
  → hỏi quyền kiểm tra/sao lưu/cài lại → quan sát và thử bật máy
  │ Prototype: khách cạnh phòng, máy không khởi động được
  │ Không suy ra mọi ca không boot đều cần cài lại
  ▼
[CP-DIAG] TRƯỚC CHỌN BƯỚC KIỂM TRA / DIAGNOSIS
  → chọn kiểm tra theo triệu chứng và dụng cụ/kiến thức đang có
  → đọc kết quả → so bằng chứng → đưa diagnosis và phương án
  ├─ Chưa đủ bằng chứng → kiểm tra thêm; thời gian vẫn trôi
  ├─ Chẩn đoán sai → test không xác nhận → mất thời gian; sửa diagnosis
  │    └─ Đã hứa sai/trễ → thông báo khách, hậu quả uy tín cần author
  └─ Đủ bằng chứng → báo phạm vi, công 50.000đ, thời gian dự kiến
  │ Loại máy, thông số, kết quả test và ngưỡng đúng/sai CẦN AUTHOR
  ▼
Khách đồng ý phạm vi và báo giá?
  ├─ Không → thương lượng theo lựa chọn đã author hoặc trả máy an toàn
  └─ Có → nhận sửa, ghi hẹn và quyền dữ liệu → mở quy trình
  │ Có thể từ chối trước nhận: không có thu nhập, không mất uy tín vô cớ
  │ Ca đầu bị từ chối/thất bại: giữ học vụ/free time; cách hẹn lại CẦN AUTHOR
  │ Không tự chuyển M18 hoặc sinh khách/reward thay thế
  ▼
Sao lưu GIẢ LẬP Documents/Pictures → đối chiếu checksum
  → cắm USB → mở boot menu → chọn UEFI USB
  → chọn System, giữ Data → cài driver → kiểm tra khởi động/dữ liệu
  │ Chỉ mở thao tác sau consent + backup hợp lệ; không chạm dữ liệu thật
  ├─ Sai thứ tự/thao tác nguy hiểm → feedback, chưa xác nhận thành công
  │    → quay bước an toàn; thời gian đã dùng và gap không bị xóa
  ├─ Test chưa đạt → quay kiểm tra/diagnosis → sửa trong phạm vi đã đồng ý
  ├─ Không sửa được → báo khách → hoàn trả máy/backup và khoản còn phải trả
  │    → đóng ca thất bại; hậu quả/khôi phục lịch hẹn CẦN AUTHOR
  └─ Test đạt → khách kiểm tra → giao máy → nhận công đúng một lần
  │ Giới hạn thời gian, thông số test, xử lý hủy giữa ca CẦN AUTHOR
  ▼
Commit kết quả ca + ledger + history/kinh nghiệm → Save → Checkpoint
  │ Save lỗi: chưa báo nhận tiền/thành công; retry cùng giao dịch
  │ Reload: giữ ca và payment đã commit, không nhận công lần hai
  ▼
[M18] SAU BÀN GIAO CA ĐẦU — 404
  │ Input: ca M17 thành công; thời điểm tiếp nối CẦN AUTHOR
  ▼
Khách giới thiệu thêm → tới vị trí cạnh bàn → đặt/gắn biển dịch vụ
  → mở job board và lịch học lại → xem thời gian trống thật
  ├─ Chưa nhận ca → giữ thời gian học/nghỉ, chưa có tiền ca đó
  └─ Nhận ca → ghi phạm vi, hẹn giao, chi phí và conflict đã preview
  │ Treo biển/đọc board/đặt lịch có duration CẦN AUTHOR
  ▼
[M19-A] ĐĂNG KÝ KỲ 5 — 404 / PORTAL, MỐC CẦN AUTHOR
  │ Input: snapshot môn, debt, prerequisites, ví và hóa đơn
  ▼
Routine → ngồi bàn → mở portal → xem môn nợ và lớp đủ điều kiện
  → chọn học lại/học tiếp từ catalog đã author → kiểm tra trùng giờ
  ├─ KTLT chưa đạt → CTDL khóa; HĐH tiếp tục khóa
  ├─ Môn đã PASS → không bắt học lại hoặc cấp lại credits
  ├─ Không đủ tiền/điều kiện → hiện lý do, chưa xác nhận đăng ký/thanh toán
  │    → điều chỉnh lịch/ưu tiên ca đủ sức; hỗ trợ/giãn phí CẦN AUTHOR
  └─ Đủ điều kiện → chốt lịch + hóa đơn/biên lai → save/checkpoint
  │ Không thêm môn hoặc suy ra điều kiện học phí chưa có trong data
  ▼
[Y3-DAY] NGÀY ĐẠI DIỆN — LỊCH KỲ 5 RỒI KỲ 6
  │ Mỗi ngày có ngày/giờ/lớp/ca cụ thể CẦN AUTHOR
  ▼
404 → thức dậy → WC/vệ sinh → thay đồ → xem lịch → lấy sách/balo
  → hành lang/cầu thang → trạm → bus → H6 → tìm lớp → ngồi/điểm danh
  ├─ Đúng giờ/trễ → học phần còn trong khung, ghi attendance thực tế
  └─ Hết khung → vắng + gap theo nội dung bỏ lỡ; chưa tự cấp FAIL cả môn
  → tan học → bus về KTX → 404 → cất balo → xem bài/khách/deadline
  │ Travel/routine đều tiến timeline; không teleport từ giường tới lớp
  ▼
[Y3-EVENING] CHỌN VIỆC TRONG THỜI GIAN CÒN LẠI
  ├─ Sửa máy → tiến đúng bước ca; thu chỉ sau nghiệm thu/bàn giao
  ├─ Học/assignment → chọn phần yếu → làm bài → ghi tiến độ/kiến thức thật
  ├─ Bạn bè/Long/gia đình → tương tác theo history, dùng thời gian
  └─ Ăn/nghỉ/ngủ → tác dụng năng lượng theo rule được author
  │ Nhận nhiều ca: tăng cơ hội thu nhưng tăng tải và nguy cơ trễ
  │ Ưu tiên học: giữ ca từ chối/không có tiền, tăng phần thực sự học
  │ Cân bằng: theo lịch thực tế, không tự cấp outcome trung bình
  ▼
[M19-B] HAI CA ĐẠI DIỆN SAU CA ĐẦU — 404 / BÀN SỬA
  │ Ca mạng: xin phép/sao lưu cấu hình → kiểm tra cáp → DHCP
  │ → ping gateway → khách nghiệm thu → giao/ghi ledger
  │ Nghỉ giữa ca: cất dụng cụ → ăn → nghỉ → xem lịch tiếp theo
  │ Ca nóng: hỏi/xin phép → chẩn đoán → tắt nguồn/rút sạc
  │ → vệ sinh khe gió → lắp → đo nhiệt/test → bàn giao/ghi ledger
  ├─ Sai diagnosis/test fail → mất thời gian → kiểm tra lại hoặc trả máy
  └─ Không đủ kỹ năng/dụng cụ → từ chối/hoãn có thỏa thuận; không tự sửa được
  │ Không bắt tháo máy cho lỗi phần mềm/mạng
  │ Không lặp job vô hạn để farm tiền/skill hoặc vượt deadline
  ▼
[CP-JOB] TRƯỚC NHẬN/TỪ CHỐI CA CÓ TẢI LỚN
  │ Dùng ca đã author; ca lớn riêng hiện CẦN AUTHOR
  → xem số giờ còn lại + chi phí + lịch giao + tiết học/assignment
  ├─ Từ chối → không thu, giữ thời gian; xem lại kế hoạch đóng phí
  ├─ Đổi hẹn/phạm vi → chỉ hiệu lực khi khách đồng ý
  └─ Nhận → đặt việc vào timeline, khóa vốn/linh kiện nếu rule có author
  ▼
[CP-CONFLICT] DEADLINE BÀI TẬP VS KHÁCH CẦN MÁY GẤP
  │ Bài của môn đã đăng ký; đề, hạn và job liên quan CẦN AUTHOR
  ├─ Ưu tiên bài → thao tác/làm/test/nộp → receipt; báo khách nếu trễ
  ├─ Ưu tiên khách → sửa/test/giao → bài còn thiếu hoặc trễ theo giờ thật
  └─ Chia thời gian/đàm phán → xem khả năng hoàn tất, không bảo đảm cả hai
  ▼
[CP-DEADLINE] TRƯỚC HẠN — LƯU TIẾN ĐỘ BÀI VÀ MÁY RIÊNG
  → tiếp tục việc đã chọn; tới hạn xử lý đúng một lần
  ├─ Hoàn thành đúng hạn → receipt/khách nghiệm thu theo event thật
  └─ Trễ/chưa hoàn thành → thông báo hậu quả, recovery đã author
       → học bù/chỉnh bài/đổi lịch khách nếu còn cửa, không đổi receipt cũ
  ▼
THỨC KHUYA SỬA TIẾP HAY DỪNG NGỦ?
  ├─ Tiếp tục → preview giờ kết thúc/giờ học sáng → mệt/giảm giờ ngủ
  │    → báo thức → dậy muộn/trễ/vắng theo giờ routine + bus thật
  └─ Dừng → cất máy an toàn → báo tiến độ khách → ngủ theo kế hoạch
  │ 02:00 chỉ là tình huống minh họa; timestamp cụ thể CẦN AUTHOR
  → recap nguyên nhân → lịch ngày tiếp theo → quay Y3-DAY nếu còn ngày
  ▼
[M19-C] THU CHI QUA CÁC TUẦN → ĐÓNG HỌC PHÍ
  │ Mở sổ: từng ca/thu/chi/số dư; preview các event trong montage
  │ Prototype có montage ròng 1.200.000đ và hóa đơn 900.000đ
  │ Chỉ là dữ liệu hiện có, CẦN AUTHOR xác nhận nguồn/đối soát từng khoản
  ├─ Không khớp event/thiếu tiền → chưa commit montage/biên lai
  │    → lập lại lịch kiếm tiền hợp lệ; không tự trợ cấp hoặc vay
  └─ Khớp → commit các giao dịch chưa ghi → xác nhận nộp phí → receipt
  │ Không cộng cả tiền từng ca lẫn montage chứa chính ca đó
  ▼
[M19-D] CUỐI KỲ 5 — THI KTLT/VL1, NGÀY/GIỜ CẦN AUTHOR
  → xem eligibility/gaps/bài nộp → chọn ôn hoặc nhận việc còn kịp
  → routine 404/balo/bus/H6 → vào phòng → làm bài → nộp attempt
  ├─ Thiếu điều kiện/vắng/làm sai → ghi reason; recovery CẦN AUTHOR
  └─ Đủ bằng chứng đạt → portal kỳ 5 xác nhận KTLT/VL1 → credits một lần
  │ Canon đạt phải có nguyên nhân; không dùng một câu mẫu để cấp cả môn
  ▼
KỲ 6 → kiểm tra KTLT PASS → đăng ký CTDL → chốt lịch → save/checkpoint
  │ HĐH khóa đến khi CTDL đạt; không đổi lịch sử môn đã PASS
  → ngày học/học bài/ca sửa tiếp tục theo Y3-DAY
  → tin khách nữ nhờ sửa mở M20 ở khoảng trống đã author
  │
  ▼
[M20] CRUSH NHỜ SỬA — 404, GIỜ HẸN CẦN AUTHOR
  │ Input: lịch hợp lệ, kiến thức/dụng cụ hiện có, quan hệ/history giữ nguyên
  → routine trước hẹn → nhận máy → hỏi lỗi → xin phép scan/sao lưu
  → cập nhật mẫu → quét → đọc bằng chứng → cách ly theo consent
  → tạo điểm khôi phục → xử lý startup độc hại → restart/test dữ liệu
  ├─ Test fail → kiểm tra lại hoặc báo không sửa được; giờ vẫn trôi
  └─ Test đạt → khách nghiệm thu/trả máy → xác nhận miễn công
  │ Không có tiền công; chi phí thực có vẫn phải có ledger riêng
  │ Nhận/hoãn/từ chối: tác động lịch/quan hệ cần author, chưa tự mở lời hẹn
  → lưu ca + miễn công → nhận tin hẹn quán → hỏi Long mượn áo
  ▼
[M21] QUÁN TRONG TIN HẸN — NGÀY/GIỜ VÀ TRAVEL CẦN AUTHOR
  → kiểm tra lịch học/ca → mặc áo mượn → đi tới quán → tìm bàn
  → chào crush và bạn trai → hỏi máy → đồng ý kiểm tra/phạm vi
  → backup cấu hình → kiểm tra mạng → sửa DNS khi bằng chứng phù hợp
  → test truy cập → bàn giao; phí nếu có CẦN AUTHOR, không tự cộng tiền
  ├─ Không nhận/không sửa được → trả máy, hội thoại/hệ quả cần author
  └─ Hoàn tất → phản ứng Lươn → về KTX/404 → cởi/trả áo Long
  → Long nhận ra buồn → nhóm rủ Hồ Đá
  │ Không tự phát sinh quan hệ tình cảm/reward vì sửa máy
  ▼
[M22] HỒ ĐÁ — HẸN NHÓM, KHÔNG COUNTDOWN HỘI THOẠI
  → xem lịch còn lại → nhận hẹn/đổi hẹn nếu được author
  → di chuyển → chọn chỗ ghế cạnh bạn → ngồi cùng Long/Tộ/Đạm
  → chia trà/bánh → kể hoặc nghe → nhận phản hồi → kết thúc khi muốn
  │ Thời gian từng hoạt động CẦN AUTHOR; không ép timer chọn câu trả lời
  │ Ăn/uống có cost chỉ khi có nguồn và giao dịch đã author
  → về 404 → cất đồ → xem lịch thi/công bố điểm → ngủ
  │ Nếu còn bài thi: đi qua EXAM bên dưới; nếu đã thi đủ: tới portal
  │ Không công bố kết quả các attempt chưa xảy ra
  ▼
[CP-EXAM] THI CUỐI KỲ 6 / GT2 THEO KỲ ĐÃ AUTHOR
  │ Vị trí trong lịch M20–M22 CẦN AUTHOR; không mặc định sau Hồ Đá
  → xem ca chưa giao và giờ thi sáng mai → ôn / sửa tiếp / ngủ
  → preview hậu quả → commit thời gian → báo thức → routine/bus/H6
  → kiểm tra eligibility → ngồi thi → làm theo kiến thức đã học → nộp
  ├─ Vắng/không đủ điều kiện/làm sai → ghi evidence và reason thật
  └─ Nộp hợp lệ → lưu attempt; chưa tự publish kết quả học kỳ
  → thi môn tiếp theo theo lịch; xong mới mở ngày portal
  ▼
[CP-PORTAL] TRƯỚC M23 — NGÀY CÔNG BỐ ĐIỂM CẦN AUTHOR
  → preview nguồn/đích/duration + ca/hạn còn tồn → confirm/commit/save
  ▼
[M23] PORTAL CUỐI NĂM 3 — 404 / LAPTOP
  │ Input: attempts/attendance/bài nộp/gaps và các giao dịch đã lưu
  → routine → ngồi bàn → đăng nhập → mở bảng điểm
  → xem từng môn + reasons + recap choice học/sửa/ngủ và hậu quả
  → canon: GT2 đạt, CTDL chưa đạt, HĐH vẫn khóa, tổng 78/150
  ├─ Dữ liệu không giải thích được canon/credits → gate CẦN AUTHOR
  │    Không ép trượt CTDL, không sinh môn để bù 50 → 78
  └─ Hợp lệ → commit kết quả một lần → xem debt/prerequisites
  → recap ví/học phí/linh kiện/ca chưa trả, uy tín và quan hệ
  → xem hướng trả nợ; không tự mở HĐH hoặc xóa ca đang dở
  ▼
[CP-BRIDGE] SAU KẾT QUẢ, TRƯỚC TIN ĐẠM — 404
  → Đạm nhờ mang sách tới lab HCMUS → đọc tin/nhận yêu cầu
  → kiểm tra vật phẩm và lịch hẹn; ngày/giờ/điều kiện nhận sách CẦN AUTHOR
  → preview sang M24, không tự coi sách đã giao
  → commit/save/checkpoint handoff Năm 4
  ▼
[M24] BRIDGE NĂM 4 — MANG SÁCH TỚI LAB
  Giữ toàn bộ học vụ/tiền/uy tín/quan hệ/history/phòng + ca tồn/lịch hẹn
```

Quy tắc chung cho **mỗi nút**: input gồm snapshot tích lũy và điều kiện entry ghi trong graph; routine tùy giờ/địa điểm; thao tác, choice, hậu quả và recovery theo nhánh. Mỗi thao tác quan trọng phải có duration được author trước khi triển khai. Nếu thiếu lịch/cost/rule, hiện **CẦN AUTHOR**, không dùng giá trị mặc định để hợp thức hóa transition.

```text
ACTION → kiểm tra điều kiện → preview duration/cost/hậu quả biết trước
  → thao tác → timeline tiến → xử lý event overlap theo thứ tự thời gian
  → giữ choice cần người chơi xử lý → commit effect + state → save → checkpoint

SLEEP / SKIP / TRAVEL
  → preview source time / target time / duration / nơi đến
  → liệt kê lớp, hạn bài, hẹn khách, giờ thi có thể bỏ lỡ
  ├─ Hủy → không đổi state
  └─ Confirm → giải quyết event trung gian theo rule đã author
       → commit/save thành công → summary → checkpoint → trả quyền điều khiển
       Save lỗi → không báo hoàn tất; giữ snapshot nhất quán để retry
```

Không skip qua choice hoặc tự biến montage thành đã đi học/đã sửa. Preview phải tính lại khi lịch hoặc tiến độ đổi. Duration sửa sai vẫn được ghi; hủy UI không hoàn tác thời gian/effect đã commit. Retry checkpoint kiểm thử là restore snapshot cô lập, không phải recovery miễn phí trong save chính.

Skill: tích lũy từ bước kiểm tra/sửa/test thực sự hoàn thành, có bằng chứng và không farm thao tác lặp. Lộ trình mong muốn software → SSD/RAM → nhiệt → pin/sạc → diagnosis phức tạp cần author job, dụng cụ, consent, thời lượng và tiêu chí mở khóa. Baseline hiện chỉ đủ ca phần mềm, mạng, nóng và các ca story; không tự chèn ca SSD/RAM/pin hoặc đảo mission để giả đạt lộ trình. Lập mapping progression trước khi mở rộng; phần vượt năng lực cho phép hoãn/từ chối/trả máy.

Ledger: dùng chung ví production; mỗi khoản có nguồn event, lý do, chiều thu/chi, số tiền, thời điểm và định danh chống lặp. Thu chỉ từ bàn giao ca hoặc nguồn story hợp lệ đã author; chi chỉ từ học phí, linh kiện, sinh hoạt hoặc story có giao dịch. Báo giá/đọc hóa đơn không đổi ví. Deposit/hoàn tiền chỉ dùng nếu rule được author; không tự tạo. Một action có reward/receipt/course result phải lưu cùng trạng thái hoàn tất theo cơ chế hiện có; lỗi/reload không tách payment khỏi bàn giao.

## Definition of Done

- [ ] Handoff T-002 được chốt: mapping course IDs, credits, PASS/FAIL/reasons, môn nợ, prerequisites, ví/học phí, relationship, history, attendance/gaps, inventory, 404 và choices; không chạy lại M16 hoặc ghi đè PASS.
- [ ] Author lịch kỳ 5/kỳ 6, ngày học đại diện, đăng ký/đóng phí, khách, deadline, thi, công bố điểm và M24. Chọn vị trí EXAM trong timeline M20–M22; mọi kết quả đều sau attempt, không có thời gian đi ngược.
- [ ] Mỗi milestone có đủ thời gian/địa điểm, input, routine, thao tác, choice, duration, hậu quả, recovery, preview và commit → save → checkpoint như graph; mọi ô thiếu mang nhãn CẦN AUTHOR trước triển khai.
- [ ] Chốt diagnosis/test/consent, dấu hiệu lỗi, điều kiện an toàn và nhánh trả máy cho từng ca. Ca đầu đơn giản, có thao tác thực; không thay bằng nút nhận tiền hoặc cutscene. Sai diagnosis tốn thời gian và có đường kiểm tra lại.
- [ ] M18 chỉ mở sau job đầu hoàn tất; author đường tiếp tục hợp lệ khi từ chối/thất bại M17. Không softlock vì thiếu tiền/năng lượng; recovery có nguồn, chi phí và điều kiện rõ, không farm job/cấp tiền vô cớ.
- [ ] Chốt course catalog/lớp, assignments, gaps, eligibility, đề/đánh giá và ngưỡng PASS/FAIL; nối kỳ 5 KTLT/VL1 → đăng ký kỳ 6 CTDL → M23 GT2 đạt/CTDL chưa đạt. Chốt GT2 thi ở kỳ nào và đối soát 78 credits bằng course facts, không invent môn/điểm.
- [ ] Chốt rule canon–agency trước runtime: nhánh ôn tốt/thi tốt không bị ép FAIL, nhánh thiếu evidence không tự PASS. Mâu thuẫn phải được author giải quyết, không coi là acceptance đã đạt.
- [ ] Mỗi choice có effect thật hoặc cơ hội bị mất: học/khách/ngủ, deadline/khách gấp, thi sáng/ca tồn, học phí/học lại, Long/bạn bè/công việc. Reasons truy về event; không tự đổi attendance, energy, uy tín hoặc relationship.
- [ ] Chốt ledger từng ca và montage, đối soát dữ liệu prototype 1.200.000đ/900.000đ trước dùng; không trùng tiền ca, không âm ví ngoài rule, không tự cộng hỗ trợ. Miễn công M20 là không thu, không xóa chi phí đã xảy ra.
- [ ] Tái sử dụng controller/session/wallet/save/time/portal hiện có; module Year3 hiện chỉ là adapter/checklist. Không tạo hệ thống học vụ, thời gian hoặc lưu mới song song. Vertical slice M17 → M18 → một ngày M19 có một conflict chạy trước mở rộng toàn tuyến, sau gate T-007 và phần transition T-006 cần thiết.
- [ ] Snapshot bền vững bao gồm repair progress/consent/backup/test/đồ khách, course attempts/receipts, lịch và state bàn giao; checkpoint restore đúng time, position, pose, inventory, money, repair/course state, relationship, story, 404 và UI.
- [ ] Trước restore: dừng phiên cũ, coroutine/timer, reservation và UI; khôi phục snapshot rồi mới trả input. Save kiểm thử tách save chơi chính. Không duplicate reward/payment/submission/exam result khi reload, retry hoặc mở portal nhiều lần.
- [ ] F8 production phải được nối vào cơ chế checkpoint hiện có; F8 của Room404_MVP chưa chứng minh production hỗ trợ. Các checkpoint sau là yêu cầu triển khai, **chưa có trong catalog hiện tại**:

| Checkpoint dự kiến | Thao tác thủ công | Kết quả mong đợi |
|---|---|---|
| CP-FIRST | Load trước khách, nhận máy | Đúng giờ/404/pose/UI; còn đủ state T-002; chưa có công ca đầu |
| CP-DIAG | Chọn kiểm tra sai rồi kiểm tra lại | Tốn thời gian, giữ evidence/consent; chưa được giao máy/nhận công |
| CP-JOB | Nhận rồi thử lại nhánh từ chối ở save thử riêng | Lịch/cost/thu nhập khác theo choice; không phát reward lúc nhận |
| CP-CONFLICT | Chọn bài hoặc ca gấp | Tiến độ, thời gian, quan hệ/uy tín và việc bị trễ có nguyên nhân |
| CP-DEADLINE | Đi qua hạn và reload | Receipt hoặc late reason đúng timestamp, chỉ ghi một lần |
| CP-EXAM | Thức sửa tiếp hoặc ngủ rồi đi thi | Giờ dậy/arrival/eligibility/attempt bám state, không teleport |
| CP-PORTAL | Xem điểm, đóng/mở và reload | Credits/result không lặp; CTDL/HĐH theo facts, recap có reasons |
| CP-BRIDGE | Load, nhận tin Đạm, chuyển M24 | Mang đúng state sang Năm 4; chưa tự giao sách hoặc xóa ca tồn |

- [ ] Mỗi checkpoint có fixture hợp lệ và biến thể fail/recovery cần thiết; gồm UI mở giữa ca/bài thi, save lỗi và hủy preview để người dùng kiểm tra restore/idempotency.
- [ ] Nghiệm thu thủ công gameplay/UI/animation qua checkpoint → thao tác → kết quả mong đợi; ghi evidence riêng. Task này chỉ tạo spec: chưa chạy test, lint, compile, Unity Play Mode, build hoặc nghiệm thu runtime.
