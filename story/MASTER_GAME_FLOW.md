# 🎓 STORY MANUSCRIPT & MISSION NARRATIVE: 6 NĂM ĐẠI HỌC BÁCH KHOA

> Tài liệu được tách theo [story/README.md](README.md). Khi có mâu thuẫn về canon/transition, dùng [PRODUCTION_FLOW_BASELINE.md](PRODUCTION_FLOW_BASELINE.md) và [GAMEPLAY_TRANSITIONS_PLAN.md](GAMEPLAY_TRANSITIONS_PLAN.md). Tiến độ asset không đồng nghĩa gameplay đã hoàn thành.
### "Từ Tân Sinh Viên Ngơ Ngác Đến Tấm Bằng Kỹ Sư Sau 6 Năm Đầy Sẹo"
* **Nhân vật chính (Player Controller):** Lê Văn Lươn (Thanh Hóa – Bách Khoa HCMUT)  
* **Thể loại:** Narrative adventure cô đọng với yếu tố life-sim (Top-down 2D / 2.5D)
* **Thời lượng trải nghiệm:** 60–90 phút (ưu tiên gameplay; montage/chuyển thời gian ngắn có chủ đích)

---

## 🏛️ 1. HỆ THỐNG VẬN HÀNH & NGUYÊN TẮC THIẾT KẾ (CORE ARCHITECTURE)

### 🔁 Vòng Lặp Nhiệm Vụ Nhân - Quả (Cause ➔ Action ➔ Result ➔ New Quest)
Mọi sự kiện trong game đều không tự dưng rơi xuống đầu nhân vật, mà được vận hành theo chuỗi liên hoàn:
```text
[NGUYÊN NHÂN]             [HÀNH ĐỘNG CỦA PLAYER]          [KẾT QUẢ VẬT LÝ]          [NHIỆM VỤ MỚI MỞ RA]
23:40 rủ làm ván rank  ➔  Player tự tay bấm [YES]     ➔  04:32 sáng kiệt sức    ➔  06:30 Spam Snooze cúp tiết
Cúp tiết quá 3 buổi   ➔  Vào phòng thi cắn bút      ➔  Bảng điểm dính chữ F   ➔  Đi kiếm tiền nộp học lại
Thiếu tiền học lại    ➔  Mở tiệm cài Win dạo KTX    ➔  Bận cày tiền mưu sinh  ➔  Chậm tiến độ tốt nghiệp
```

### ⏳ Cơ Chế Chuyển Tiếp Thời Gian (Seamless Day & Semester Transitions)
Không dùng màn hình đen ghi *"6 tháng sau..."* gây đứt gãy cảm xúc. Thời gian được chuyển tiếp thông qua **3 hình thức tương tác vật lý trực tiếp**:
1. **Chuyển Giờ / Đêm muộn (Time-lapse In-game):** Đồng hồ góc màn hình quay tít (`23:40` ➔ `01:15` ➔ `04:32`), ánh đèn tuýp nhấp nháy chuyển sang màu nhờ nhờ của tờ mờ sáng, rác vỏ mì tôm và lon nước ngọt tự tích tụ dưới chân ghế.
2. **Chuyển Ngày (Sleep & Wake-up Cycle):** Lươn trèo lên giường bấm `[ E ] Đi ngủ`. Màn hình đen 1.5 giây với tiếng quạt trần kẽo kẹt quay, sau đó tiếng chuông báo thức hoặc ánh nắng buổi sớm rọi qua cửa sổ báo hiệu ngày mới.
3. **Chuyển Học Kỳ & Năm Học (Academic Milestone Card):** Mỗi khi hoàn thành đợt thi cuối kỳ hoặc trả nợ môn, màn hình xuất hiện chiếc **Thẻ Tiến Độ Học Vụ (Student Academic Portal)** hiển thị:
   * `HỌC KỲ HIỆN TẠI`: Kỳ 1 ➔ Kỳ 12.
   * `TÍN CHỈ TÍCH LŨY`: XX / 150 tín chỉ.
   * `MÔN TIÊN QUYẾT (PREREQUISITE)`: Mở khóa / Khóa đỏ.
   * Player bấm `[ Space ] Xác nhận` để bước sang học kỳ tiếp theo.

### 📊 4 Chỉ Số Sinh Tồn Cốt Lõi (HUD Tối Giản)
* 💰 **Tiền (VND):** Cơm bụi 25k–35k, tiền học lại tín chỉ Bách Khoa, tiền cài Win dạo (+50k/máy).
* 📚 **Tiến độ học tập:** Tín chỉ tích lũy & Trạng thái môn tiên quyết.
* ❤️ **Tình anh em (Bromance):** Mức độ thấu hiểu giữa 4 đứa phòng 404.
* ⚡ **Năng lượng / Độ tỉnh táo:** Ảnh hưởng trực tiếp đến tốc độ di chuyển và khả năng giải đố.

---

## 📅 BẢNG TIẾN TRÌNH 6 NĂM HỌC VỤ & LÝ GIẢI KỸ THUẬT

```text
┌─────────┬───────────────┬──────────────────────────────┬────────────────────────────────────────────────────────────────────────┐
│ NĂM HỌC │ HỌC KỲ        │ TRẠNG THÁI HỌC VỤ            │ BẢN CHẤT HỌC THUẬT & HẬU QUẢ VẬN HÀNH TRONG GAME                       │
├─────────┼───────────────┼──────────────────────────────┼────────────────────────────────────────────────────────────────────────┤
│ NĂM 1   │ Kỳ 1 & Kỳ 2   │ BÌNH THƯỜNG (Normal)         │ Nhập học, đậu hết môn cơ bản, 4 đứa gắn kết thành bạn thân KTX 404.    │
│ NĂM 2   │ Kỳ 3 & Kỳ 4   │ CHẬM TIẾN ĐỘ (Behind)        │ Rớt Giải Tích 1 & Vật Lý 1 → Mất Prerequisite → Khóa toàn bộ môn sau! │
│ NĂM 3   │ Kỳ 5 & Kỳ 6   │ NỢ ĐỌNG / MƯU SINH (Debt)    │ Vừa cày Win kiếm tiền vừa học lại, nợ cũ giảm nhưng nợ mới phát sinh.   │
│ NĂM 4   │ Kỳ 7 & Kỳ 8   │ ÁP LỰC / BỊ TỤT LẠI         │ Học + tiền + môn nợ + đồ án nhỏ; chưa đủ đăng ký đồ án tốt nghiệp.     │
│ NĂM 5   │ Kỳ 9 & Kỳ 10  │ ÁP LỰC CAO NHẤT              │ Trả CTDL, chuẩn bị đồ án/internship, sửa máy, thiếu tiền, bạn bè rời đi.│
│ NĂM 6   │ Kỳ 11 & Kỳ 12 │ GIẢI QUYẾT / PAYOFF          │ Môn cuối → đủ điều kiện → đồ án/bảo vệ → học phí → rời 404 → tốt nghiệp│
└─────────┴───────────────┴──────────────────────────────┴────────────────────────────────────────────────────────────────────────┘
```

---

# 🕹️ CHI TIẾT 45 MISSIONS: CHUỖI NHIỆM VỤ THEO MẠCH NGUYÊN NHÂN - HỆ QUẢ

---

## 🟢 NĂM 1 — TÂN SINH VIÊN: "MỌI THỨ ĐỀU MỚI"
*Mục tiêu cốt lõi:* Khám phá KTX, định hình tính cách 4 nhân vật, hoàn thành năm nhất bình thường không sự cố.

### HỌC KỲ 1: BỠ NGỠ & NHẬN PHÒNG KTX 404
* **Mission 01: [Chuyến xe đầu tiên]**
  * *Nguyên nhân:* Rời quê Thanh Hóa vào Nam nhập học.
  * *Mục tiêu:* Thoát khỏi bến xe miền Đông, tới trạm xe buýt.
  * *Hành động gameplay:* Điều khiển Lươn (dép tổ ong, balo nặng, xách thùng nem chua) di chuyển luồn lách né các NPC xe ôm dù đang chèo kéo. Nếu va phải xe ôm bị trừ 50.000đ oan.
  * *Kết quả:* Chạy tới trạm xe buýt, bấm phím `[ E ] Bắt xe buýt số 33 (Trừ 7.000đ)` chạy thẳng về Làng Đại học.
* **Mission 02: [Thủ tục nhập môn KTX]**
  * *Nguyên nhân:* Cần chỗ ở giá rẻ để đi học.
  * *Mục tiêu:* Đăng ký phòng KTX Khu B.
  * *Hành động gameplay:* Bước xuống xe buýt, đi vào sảnh bàn tiếp đón. Mở balo kéo thả Giấy báo trúng tuyển Bách Khoa nộp cho cô quản lý. Chọn câu thoại: `"Dạ phòng rẻ không cô?"`.
  * *Kết quả:* Nhận chìa khóa đồng và phiếu phân phòng số 404.
* **Mission 03: [Cuộc đua xí giường]**
  * *Nguyên nhân:* Phòng KTX 4 người chỉ có 1 vị trí đắc địa nhất.
  * *Mục tiêu:* Chiếm chiếc giường tầng dưới cạnh cửa sổ và sát ổ điện.
  * *Hành động gameplay:* Leo bộ lên tầng 4, mở cánh cửa sắt phòng 404. Quan sát 4 góc giường. Người chơi chạy nhanh tới chiếc giường tầng dưới bên phải, bấm phím `[ Space ] Quẳng ba lô xí chỗ`. Bấm `[ E ]` lôi cây tuốc-nơ-vít dắt cạp quần ra vặn chặt lại ốc giường ọp ẹp, cắm cái quạt con cóc vào ổ điện chạy vù vù.
  * *Kết quả:* Xí thành công vị trí đắc địa nhất phòng.
* **Mission 04: [Khai hoang phòng 404]**
  * *Nguyên nhân:* Phòng bụi bặm sau 3 tháng hè.
  * *Mục tiêu:* Dọn sạch phòng trước khi đồng bọn tới.
  * *Hành động gameplay:* Cầm 35.000đ xuống căn tin mua xô nước, giẻ lau và chổi. Chạy quanh phòng bấm phím lau sạch 4 vũng bụi bẩn trong vòng 60 giây.
  * *Kết quả:* Sàn nhà sáng bóng kin kít. Lần lượt chứng kiến Đạm (lau bàn 3 lần), Long (xịt nước hoa), Tộ (ôm làn thịt trâu) bước vào phòng.
* **Mission 05: [Bữa cơm sinh viên đầu tiên]**
  * *Nguyên nhân:* Cả 4 đứa đói bụng sau ngày dài chuyển đồ.
  * *Mục tiêu:* Ăn bữa tối đầu tiên tại quán cơm bụi cổng KTX.
  * *Hành động gameplay:* Minigame gắp món cơm 25k (1 mặn 1 xào) trong giới hạn ngân sách 35k. Cả 4 đứa ngồi cùng bàn, bắt đầu hỏi han tên tuổi quê quán.
  * *Kết quả:* Mở khóa bảng chỉ số Quan hệ bạn bè (Bromance).

### HỌC KỲ 2: BẮT ĐẦU VÀO GUỒNG & GẮN KẾT
* **Mission 06: [Tiết học Giải Tích đầu tiên]**
  * *Nguyên nhân:* Bắt đầu học môn đại cương khó nhất Bách Khoa.
  * *Mục tiêu:* Giải bài tập trên giảng đường H6.
  * *Hành động gameplay:* Đạm (KHTN) ghi chép kín đặc trang giấy. Lươn chỉ ghi: *"Cái này thi không?"*. Thầy giáo gọi lên bảng giải một câu đố toán mẹo (Logic Puzzle). Người chơi thao tác giải toán bằng mẹo thực tế cực nhanh khiến Đạm liếc nhìn ngạc nhiên.
  * *Kết quả:* Chứng minh Lươn thực sự thông minh, tiếp thu cực nhanh.
* **Mission 07: [Đại chiến nhà tắm 07:20]**
  * *Nguyên nhân:* 8h00 có tiết, nhà tắm công cộng KTX quá tải.
  * *Mục tiêu:* Vệ sinh cá nhân và đón kịp xe buýt.
  * *Hành động gameplay:* Chạy xuống nhà tắm xếp hàng, đánh răng, thay đồ, chạy nước rút ra trạm xe buýt. Nếu trễ: "BUS LEFT", phải chạy bộ thở không ra hơi.
  * *Kết quả:* Lươn kịp vào lớp trước giờ điểm danh 1 phút.
* **Mission 08: [Bữa lẩu KTX gắn kết]**
  * *Nguyên nhân:* Mừng kết thúc Năm 1 thuận lợi, cả phòng thi qua hết các môn đại cương đầu tiên.
  * *Mục tiêu:* Nấu nồi lẩu mì tôm ăn mừng trong phòng.
  * *Hành động gameplay:* Lôi bếp từ giấu đáy balo ra cắm vào ổ điện Lươn xí được, bỏ mì tôm xúc xích vào nấu xì xụp. 4 thằng ngồi bệt dưới sàn gạch men cụng ly nước ngọt.
  * *Kết quả:* **KẾT THÚC NĂM 1 BÌNH THƯỜNG**. Thẻ tiến độ học vụ bật thông báo: `Năm 1 hoàn thành - Tích lũy 32/150 tín chỉ - Không nợ môn`. Lươn bắt đầu tự đắc: *"Đại học tưởng thế nào, dễ ợt!"*.

---

## 🟡 NĂM 2 — HAM CHƠI: "ĐẠI HỌC DỄ MÀ..."
*Mục tiêu cốt lõi:* Lột tả sự chủ quan, buông thả, rơi vào bẫy nghiện game, dẫn đến hậu quả thực tế: Rớt môn & Mất môn tiên quyết.

### HỌC KỲ 3: QUEN MÔI TRƯỜNG & CHỦ QUAN BUÔNG THẢ
* **Mission 09: [Cuộc chiến F5 đăng ký môn]**
  * *Nguyên nhân:* Bắt đầu học kỳ 3, sinh viên phải tự đăng ký thời khóa biểu.
  * *Mục tiêu:* Giật slot giảng viên dễ tính lúc 8h59 sáng.
  * *Hành động gameplay:* Mở web trường, bấm phím F5 liên tục canh cổng mở. Do Lươn mải ngủ dậy sát giờ, mạng nghẽn mạng sập.
  * *Kết quả:* Chọn trượt lớp ngon, bị hệ thống tự động nhét vào lớp Giải Tích 2 học lúc 6h45 sáng của thầy nổi tiếng "sát thủ tỉ lệ rớt 50%".
* **Mission 10: [Cám dỗ lúc 23:40 & Cú click YES định mệnh]**
  * *Nguyên nhân:* Lươn nghĩ mình thông minh không cần học nhiều; Long rủ đánh rank.
  * *Mục tiêu:* Đưa ra lựa chọn nghỉ ngơi hay cày game.
  * *Hành động gameplay:* Đánh xong trận rank lúc 23:40. Pop-up hiện giữa màn hình: `Sáng mai 6h45 có tiết Giải Tích. Làm thêm trận nữa không? [YES] / [NO]`. Người chơi tự tay rê chuột bấm `[ YES ]`.
  * *Kết quả:* Hiệu ứng Time-lapse kích hoạt. Kim đồng hồ quay tít: `23:40` ➔ `01:17` ➔ `03:05` ➔ `04:32 sáng`! Rác mì tôm ngập dưới chân, hai thằng kiệt sức ngã vật ra ngủ.
* **Mission 11: [06:30 Báo thức & Cúp tiết]**
  * *Nguyên nhân:* Thức tới 4h32 sáng khiến cơ thể kiệt quệ.
  * *Mục tiêu:* Thức dậy đi học đúng giờ.
  * *Hành động gameplay:* Chuông báo thức reo inh ỏi. Màn hình mờ căm. Nút bấm chớp nháy: `[ Space ] SNOOZE`. Người chơi đập phím Space tắt chuông.
  * *Kết quả:* Màn hình đen phụt 2 giây, bật sáng lại lúc **11h47 trưa**. Đạm đã đi học từ lâu. Lươn mất buổi điểm danh thứ 2 (chỉ còn 1 buổi là bị cấm thi).
* **Mission 12: [Trốn kiểm tra phòng KTX (Stealth 10s)]**
  * *Nguyên nhân:* Nấu ăn trộm lúc nửa đêm bị bảo vệ nghi ngờ.
  * *Mục tiêu:* Giấu toàn bộ thiết bị nấu nướng trong 10 giây.
  * *Hành động gameplay:* Tiếng chìa khóa lách cách ngoài cửa. Người chơi có 10 giây bấm phím: Rút phích cắm → Bê nồi lẩu nhét vào tủ quần áo → Nhảy tót lên giường trùm chăn vờ ngủ.
  * *Kết quả:* Thoát kiểm tra trong gang tấc nhưng phòng ám mùi thức ăn nồng nặc.
* **Mission 13: [Deadline lúc 23:59:41]**
  * *Nguyên nhân:* Mải chơi game cả tuần không làm bài tập lớn C++.
  * *Mục tiêu:* Nộp bài trước 23h59.
  * *Hành động gameplay:* Đồng hồ đếm ngược. Copy code trên mạng ➔ Bị lỗi biên dịch (Compile Error) ➔ Lên Stack Overflow tìm bản vá ➔ Chắp vá code ➔ Bấm nút Submit lúc **23:59:41**.
  * *Kết quả:* Qua ải sát nút, làm Lươn càng sinh tâm lý ỷ lại, coi thường việc học.

### HỌC KỲ 4: CÚ TÁT THỰC TẾ & MẤT ĐIỀU KIỆN TIÊN QUYẾT
* **Mission 14: [Cắn bút trong phòng thi]**
  * *Nguyên nhân:* Cúp học quá nhiều, kiến thức rỗng tuếch.
  * *Mục tiêu:* Hoàn thành bài thi cuối kỳ Giải Tích 2 và Vật Lý 1.
  * *Hành động gameplay:* Đề thi phát ra toàn kiến thức buổi sáng Lươn ngủ quên. Minigame làm bài thi: Người chơi cố bấm phím giải các câu hỏi nhưng thanh Tiến độ làm bài chỉ lẹt đẹt 20%. Cắn bút nộp bài sớm.
* **Mission 15: [Bảng điểm F & Giấy cảnh cáo học vụ]**
  * *Nguyên nhân:* Thi trượt vì không chịu học.
  * *Mục tiêu:* Kiểm tra kết quả học tập trên cổng đào tạo Bách Khoa.
  * *Hành động gameplay:* Đăng nhập portal. Màn hình hiện lên dòng chữ đỏ không nhạc nền:  
    `GIẢI TÍCH 2: F | VẬT LÝ 1: F | KỸ THUẬT LẬP TRÌNH: F`.  
    Thông báo nhấp nháy: **`CẢNH CÁO HỌC VỤ MỨC 1 - KHÓA ĐIỀU KIỆN TIÊN QUYẾT MÔN NĂM 3!`**.
  * *Hậu quả trực tiếp:* Không được đăng ký các môn cốt lõi năm 3 (Cấu Trúc Dữ Liệu, Hệ Điều Hành). Lươn chính thức **CHẬM TIẾN ĐỘ** so với bạn bè.
* **Mission 16: [Cuộc gọi lúc nửa đêm của mẹ]**
  * *Nguyên nhân:* Cầm tờ giấy báo nợ học phí học lại 3 môn (hơn 3 triệu đồng).
  * *Mục tiêu:* Quyết định có xin tiền gia đình hay không.
  * *Hành động gameplay:* Điện thoại rung: `MẸ`. Bắt máy. Nghe tiếng mẹ ho dặn dò ăn uống. Player có lựa chọn: Xin tiền hay Nuốt nước mắt nói dối. Lươn nghẹn ngào: *"Dạ con khỏe, trong này việc học ổn lắm mẹ..."*.
  * *Kết quả:* Lươn quyết định tự chịu trách nhiệm, tự kiếm tiền đóng tiền học lại!

---

## 🟠 NĂM 3 — MƯU SINH: "PHẢI TỰ LO THÔI"
*Mục tiêu cốt lõi:* Vừa làm thêm kiếm tiền nộp học phí nợ môn, vừa học lại nhưng tiến độ phục hồi chậm; nếm trải cảm giác thất tình sinh viên.

### HỌC KỲ 5: MỞ TIỆM SỬA MÁY PHÒNG 404
* **Mission 17: [Cài Win dạo 50K đầu tiên]**
  * *Nguyên nhân:* Cần tiền đóng học phí học lại, không muốn xin mẹ.
  * *Mục tiêu:* Kiếm được 50.000đ đầu tiên.
  * *Hành động gameplay:* Khách hàng phòng bên mang laptop lỗi sang. Cắm USB Boot, format ổ đĩa, cài Windows và Office, nhận tờ tiền 50.000đ đầu tiên bỏ vào hũ tiết kiệm.
* **Mission 18: [Dán biển tiệm sửa máy phòng 404 (Mở Side Jobs)]**
  * *Nguyên nhân:* Cần nhiều đơn hàng hơn để trả nợ học phí.
  * *Mục tiêu:* Quảng bá dịch vụ khắp KTX.
  * *Hành động gameplay:* Dán tấm bìa carton trước cửa phòng: `CÀI WIN - SỬA LAPTOP 404`. Mở hệ thống nhận việc tự do: Cứu dữ liệu, vệ sinh quạt, bẻ khóa wifi bán lại cho các phòng lân cận.
* **Mission 19: [Vòng xoáy vừa học vừa làm]**
  * *Nguyên nhân:* Ban ngày học lại môn nợ, ban đêm còng lưng sửa máy tính kiếm tiền.
  * *Mục tiêu:* Cân bằng thanh Năng lượng và thanh Tiền bạc.
  * *Hành động gameplay:* Điều khiển Lươn chạy ngược xuôi giữa giảng đường và KTX. Thanh Năng lượng luôn ở mức thấp, người chơi phải liên tục mua bánh mì gặm cầm hơi.
  * *Kết quả:* Đủ tiền nộp học phí học lại Giải Tích 2.

### HỌC KỲ 6: VA CHẠM TÌNH CẢM & BỜ HỒ ĐÁ
* **Mission 20: [Chiếc laptop của Crush]**
  * *Nguyên nhân:* Một bạn nữ trường Kinh Tế mang máy dính virus sang nhờ cứu.
  * *Mục tiêu:* Gây ấn tượng với crush.
  * *Hành động gameplay:* Thao tác diệt virus, cài phần mềm diệt mã độc. Sửa xong Lươn từ chối lấy 50k. Long khịa: *"50k đâu?"* → Lươn: *"Bạn bè giúp nhau"* → Long: *"Bạn bè dữ chưa"*.
* **Mission 21: [Cuộc hẹn sượng trân]**
  * *Nguyên nhân:* Crush nhắn tin rủ đi uống nước.
  * *Mục tiêu:* Đi gặp crush với hy vọng hẹn hò.
  * *Hành động gameplay:* Lươn mượn áo khoác xịn của Long, vuốt tóc đi tới quán nước. Đến nơi, crush đưa thêm một chiếc laptop: *"Máy bạn trai em bị sập nguồn anh xem hộ với"*. Objective đổi thành: `Sửa laptop của bạn trai crush`. Lươn sửa xong trong im lặng.
* **Mission 22: [Đêm trà đào Hồ Đá (Breathing Mission)]**
  * *Nguyên nhân:* Lươn thất tình, tự ti về hoàn cảnh nợ môn nghèo khó.
  * *Mục tiêu:* Giải tỏa tâm lý cùng anh em phòng 404.
  * *Hành động gameplay:* Long, Đạm, Tộ kéo Lươn ra Hồ Đá ăn bánh tráng nướng, uống trà đào 15k. Không nhiệm vụ phức tạp, chỉ là ngồi trò chuyện an ủi nhau dưới gió đêm Làng Đại học.
* **Mission 23: [Tổng kết Năm 3: Môn nợ giảm nhưng chưa hết]**
  * *Nguyên nhân:* Vừa làm vừa học khiến Lươn không thể học tối đa tín chỉ.
  * *Mục tiêu:* Kiểm tra bảng điểm cuối năm 3.
  * *Hành động gameplay:* Trả được Giải Tích 2, nhưng lại nợ môn Cấu Trúc Dữ Liệu. Tín chỉ tích lũy chỉ đạt 78/150 (trong khi bạn bè cùng khóa đã đạt trên 110 tín chỉ).

---

## 🔴 NĂM 4 — MỖI NGƯỜI MỘT HƯỚNG: "SAO TỤI NÓ ĐI NHANH VẬY?"
*Mục tiêu cốt lõi:* Khắc họa khoảng cách ngày càng xa giữa Lươn và bạn bè cùng phòng; đỉnh điểm là việc bị cấm làm đồ án tốt nghiệp cùng khóa.

### HỌC KỲ 7: PHÒNG 404 BẮT ĐẦU VẮNG BÓNG
* **Mission 24: [Đưa tài liệu cho Đạm tại Lab]**
  * *Nguyên nhân:* Đạm nhờ mang cuốn sách nghiên cứu sang trường KHTN.
  * *Mục tiêu:* Gặp Đạm tại phòng nghiên cứu.
  * *Hành động gameplay:* Lươn đi vào khu phòng lab lạnh toát của trường Tự Nhiên. Thấy Đạm đang thảo luận học thuật bằng tiếng Anh cùng giáo sư. Lần đầu tiên Lươn nhận ra: **Đạm đã bước sang một đẳng cấp hoàn toàn khác**.
* **Mission 25: [Review CV cho Long]**
  * *Nguyên nhân:* Long chuẩn bị đi thực tập doanh nghiệp IT.
  * *Mục tiêu:* Giúp Long format chỉnh sửa CV.
  * *Hành động gameplay:* Chỉnh font, căn lề, xuất file PDF giúp Long. Vài ngày sau Long nhảy cẫng lên hét: *"ĐẬU THỰC TẬP DOANH NGHIỆP RỒI MẤY NÍ!"*.
* **Mission 26: [Tộ đi làm full-time]**
  * *Nguyên nhân:* Tộ được nhận vào làm chính thức cho tòa soạn báo.
  * *Mục tiêu:* Tiễn Tộ đi làm ca sáng.
  * *Hành động gameplay:* Lươn tiễn Tộ đi ca sáng; các bạn khác cũng vắng phòng vì lịch học/việc. Ban ngày phòng có lúc chỉ còn Lươn, nhưng cả ba bạn vẫn đang ở KTX và chưa chuyển đi.

### HỌC KỲ 8: TỤT LẠI HOÀN TOÀN & BỊ CẤM LÀM ĐỒ ÁN CÙNG KHÓA
* **Mission 27: [Kiểm tra điều kiện làm đồ án tốt nghiệp]**
  * *Nguyên nhân:* Đợt xét duyệt làm đồ án tốt nghiệp của khóa K2x.
  * *Mục tiêu:* Nộp đơn xin đăng ký đề tài tốt nghiệp.
  * *Hành động gameplay:* Mở portal trường. Bảng điều kiện hiện dòng chữ đỏ chát chúa:  
    `TÍN CHỈ TÍCH LŨY: THIẾU 18 TÍN CHỈ | MÔN TIÊN QUYẾT CHƯA PASS: 2 MÔN`.  
    Dòng thông báo hệ thống: **`BẠN KHÔNG ĐỦ ĐIỀU KIỆN ĐĂNG KÝ ĐỒ ÁN TỐT NGHIỆP CÙNG KHÓA!`**.
* **Mission 28: [Chụp ảnh kỷ yếu cho bạn bè]**
  * *Nguyên nhân:* Bạn bè cùng khóa tốt nghiệp đúng hạn 4 năm.
  * *Mục tiêu:* Cầm máy ảnh chụp hình kỷ yếu cho Đạm, Long, Tộ.
  * *Hành động gameplay:* Cầm máy ảnh chụp từng người khoác áo cử nhân trước nhà điều hành ĐHQG. Một bác phụ huynh đi qua hỏi: *"Cháu cũng tốt nghiệp hả?"* → Lươn gượng cười: *"Dạ chưa cô, cháu còn học tiếp ạ"*.
  * *Kết quả:* **KẾT THÚC NĂM 4**: Khóa của Lươn tốt nghiệp ra trường, **LƯƠN PHẢI Ở LẠI HỌC TIẾP**.

---

## 🟣 NĂM 5 — "MÌNH CÓ KỊP TỐT NGHIỆP KHÔNG?"
*Mục tiêu cốt lõi:* Giai đoạn áp lực nhất: nợ môn + chuẩn bị đồ án + internship nếu hợp lệ + sửa laptop + tiền + gia đình, trong lúc bạn bè rời KTX. Chưa tốt nghiệp; không mở đời nhân viên full-time. Đồ án chính thức vẫn chờ M36. Chi tiết task/cơ hội internship/thu nhập CẦN AUTHOR, không dựng company hoặc offer của Lươn từ công việc Long/Tộ.

### ĐẦU NĂM 5: CHIA LY & CĂN PHÒNG ĐÔNG NHƯNG LẠ LẪM
* **Mission 29: [Long dọn đồ rời KTX]**
  * *Nguyên nhân:* Long đi làm công ty, dọn ra ngoài thuê căn hộ.
  * *Mục tiêu:* Giúp Long khiêng đồ rời phòng 404.
  * *Hành động gameplay:* Vác thùng đồ của Long xuống cổng KTX. Chiếc giường thứ nhất trống trơn. Ngay chiều hôm đó, một tân sinh viên năm nhất lạ mặt xách vali dọn vào thế chỗ.
* **Mission 30: [Tộ rời đi]**
  * *Nguyên nhân:* Tộ chuyển trọ gần tòa soạn.
  * *Mục tiêu:* Tiễn Tộ rời KTX.
  * *Hành động gameplay:* Tộ ôm tạm biệt Lươn, để lại tảng thịt trâu gác bếp cuối cùng. Chiếc giường thứ hai đón tiếp một người lạ khác.
* **Mission 31: [Đạm là người cuối cùng]**
  * *Nguyên nhân:* Đạm hoàn tất thủ tục du học và báo kế hoạch chuyển đi sau lần tiếp tế cuối.
  * *Mục tiêu:* Đối thoại với Đạm tại cửa phòng 404 trước ngày rời KTX.
  * *Hành động gameplay:* Đạm đứng ở cửa với giấy tờ/vali chuẩn bị, nhưng vẫn ở lại phòng sau cuộc nói chuyện:

    * Đạm: *"Mày định học tới bao giờ?"*  
    * Lươn: *"Tao tự lo được."*  
    * Đạm: *"Tao biết."* (Im lặng 3 giây) *"Vì tao biết mày làm được nên tao mới tức!"*.  
    Đạm hẹn ngày đi sau lần tiếp tế M34. Hai người mới đã vào hai giường trống của Long và Tộ; người mới thứ ba chưa được xếp vào phòng khi Đạm vẫn còn ở đó.

### HỌC KỲ 9: CÚ CLICK "UNINSTALL" & THỨC TỈNH
* **Mission 32: [Khoảnh khắc Uninstall Game]**
  * *Nguyên nhân:* Nhận ra không thể sống buông thả tiếp tục làm kẻ thất bại.
  * *Mục tiêu:* Gỡ bỏ cám dỗ lớn nhất cuộc đời.
  * *Hành động gameplay:* Lươn ngồi trước máy tính. Icon game hiện lên. Player điều khiển chuột chọn `Uninstall`. Người chơi phải **nhấn và GIỮ CHUỘT** xem thanh tiến trình: `12%... 45%... 78%... 100% UNINSTALLED`.
* **Mission 33: [06:30 Không Snooze!]**
  * *Nguyên nhân:* Quyết tâm đi học nghiêm túc.
  * *Mục tiêu:* Không ngủ nướng, đến lớp đúng giờ.
  * *Hành động gameplay:* Chuông báo thức 6h30 reo. Thay vì đập phím Snooze như Năm 2, Player nhấn phím **`[ W ] BẬT DẬY NGAY LẬP TỨC`**. Rửa mặt nước lạnh, xỏ dép tổ ong bước thẳng lên giảng đường.
* **Mission 34: [Bữa lẩu tiếp tế lúc nửa đêm]**
  * *Nguyên nhân:* Lươn cày bài kiệt sức trong phòng KTX xa lạ.
  * *Mục tiêu:* Nhận sự tiếp sức từ những người bạn cũ.
  * *Hành động gameplay:* Đang ôn lúc 1h sáng, Lươn nghe tiếng gõ cửa. Long, Tộ và Đạm mang đồ ăn đã chuẩn bị sẵn cùng tài liệu sang; cả nhóm nói chuyện ngắn ở khu sinh hoạt chung/hành lang để không đánh thức hai người mới trong phòng. Đạm đưa tài liệu cho Lươn: *"Ăn nhanh rồi học tiếp con chó"*. Đạm vẫn rời KTX vào sáng hôm sau; người mới thứ ba chỉ được chuyển vào khi giường đã trống.

### CUỐI NĂM 5 → ĐẦU NĂM 6: MÔN CUỐI VÀ ĐIỀU KIỆN ĐỒ ÁN
* **Mission 35: [Trả nợ môn cuối — trải qua hai năm]**
  * Cuối Năm 5 học/ôn/thi CTDL kỳ 9/10; kết quả PASS thực mới mở HĐH kỳ 11. Đầu Năm 6 đăng ký/học/assignment/ôn/thi HĐH theo lịch CẦN AUTHOR. Không gộp hai môn vào một attempt hoặc cấp PASS vì thanh progress 100%.
  * Routine/bus/H6/eligibility/attempt/submit/portal và recovery theo evidence. Nếu còn FAIL giữ reason và đường học/thi lại hợp lệ; không ép đạt để chạy câu chuyện. Save đã PASS không thi lại để kéo dài năm.
  * Cuối Năm 5 bàn giao toàn bộ chuẩn bị đồ án, internship history, ca khách, học phí/nợ, quan hệ và phòng 404; không reset hoặc tự tốt nghiệp.

## 🔵 NĂM 6 — "6 NĂM VỪA RỒI CÓ Ý NGHĨA GÌ?"
*Mục tiêu cốt lõi:* Hội tụ hậu quả năm trước, hoàn thành môn cuối và đồ án, thanh toán nghĩa vụ, rời 404, tốt nghiệp và nhìn lại hành trình. Dùng lại các hệ thống, không thêm một năm đi làm khó hơn.

* **Mission 36: [Cổng đào tạo bật đèn xanh — sau môn cuối, đầu Năm 6]**
  * Đăng nhập portal, đối soát course facts và reasons. Khi thật sự đủ 136/150 và hết nợ tiên quyết mới nhận form/lịch GVHD M37. Đây là quyền đăng ký đồ án, chưa phải công nhận tốt nghiệp. Mapping 118→136→150 CẦN AUTHOR, không tự sinh môn/tín chỉ.

### HỌC KỲ 11: ĐỒ ÁN TỐT NGHIỆP — NGHIÊN CỨU & BỊ BÁC BỎ
* **Mission 37: [Đăng ký đồ án & Gặp GVHD]**
  * *Nguyên nhân:* Bước vào học phần đồ án tốt nghiệp Bách Khoa.
  * *Mục tiêu:* Chọn đề tài và giảng viên hướng dẫn.
  * *Hành động gameplay:* Lên văn phòng bộ môn gặp thầy hướng dẫn khó tính. Thầy hỏi: *"Em học năm mấy rồi?"* → Lươn: *"Dạ năm 6 rồi thầy"*. Chọn đề tài: "Hệ thống tự động hóa và chẩn đoán lỗi thiết bị mạng" (tận dụng 5 năm kinh nghiệm sửa máy tính thực tế).
* **Mission 38: [Nghiên cứu & Thiết kế kiến trúc (Research & Design)]**
  * *Nguyên nhân:* Cần xây dựng nền tảng lý thuyết cho hệ thống.
  * *Mục tiêu:* Lên thư viện tra cứu tài liệu và vẽ sơ đồ kiến trúc hệ thống.
  * *Hành động gameplay:* Tìm kiếm tài liệu chuyên ngành, kéo thả các block sơ đồ luồng dữ liệu (Data Flow Diagram).
* **Mission 39: [Cày Code thâu đêm]**
  * *Nguyên nhân:* Xây dựng phần mềm thực tế.
  * *Mục tiêu:* Hoàn thành module xử lý lõi.
  * *Hành động gameplay:* Minigame gõ code và kiểm thử hàm. Đồng hồ góc màn hình nhảy: `22:43` ➔ `01:17` ➔ `03:51` ➔ `05:26 sáng`. Lần này Lươn thức đêm không phải vì cày game, mà vì trách nhiệm hoàn thành mục tiêu cuộc đời!
* **Mission 40: [Demo #1 & Cú sốc bị bắt đập đi xây lại]**
  * *Nguyên nhân:* Đợt nghiệm thu tiến độ đợt 1 với GVHD.
  * *Mục tiêu:* Trình bày demo phần mềm cho thầy xem.
  * *Hành động gameplay:* Chạy demo phần mềm. Thầy khoanh đỏ bản vẽ: *"Lý thuyết chắp vá quá, kiến trúc bảo mật này chưa đạt tiêu chuẩn kỹ sư Bách Khoa, làm lại một nửa!"*.
  * *Kết quả:* Lươn không hề nản lòng, mang máy về phòng quyết tâm viết lại toàn bộ module bảo mật chuẩn mực.

### HỌC KỲ 12: BẢO VỆ TỐT NGHIỆP & TẮT ĐÈN RỜI KTX
* **Mission 41: [Demo #2 được duyệt & Căn lề báo cáo]**
  * *Nguyên nhân:* Thầy duyệt bản demo phần mềm đợt 2, chuẩn bị nộp cuốn báo cáo tốt nghiệp.
  * *Mục tiêu:* Hoàn thiện cuốn báo cáo 120 trang.
  * *Hành động gameplay:* Minigame format văn bản Word: Chỉnh font Times New Roman, dãn dòng 1.5 line, căn lề trái 3cm, sửa lỗi nhảy số trang mục lục. Đem ra tiệm in bìa mạ vàng bóng bẩy.
* **Mission 42: [Fix bug phút chót (Deadline 01:14:32)]**
  * *Nguyên nhân:* Đêm trước giờ khóa cổng nộp báo cáo và mã nguồn lên hệ thống trường, server báo lỗi Crash đột ngột.
  * *Mục tiêu:* Khắc phục lỗi và submit bài trước giờ G.
  * *Hành động gameplay:* Timer đếm ngược 1 tiếng. Vận dụng toàn bộ kinh nghiệm lách lỗi, debug thực chiến 5 năm để truy tìm dòng code bị lỗi con trỏ. Sửa xong bấm nút `[ SUBMIT ]` thành công lúc 23h45!
* **Mission 43: [Bảo vệ đồ án trước Hội đồng Bách Khoa (Boss Fight)]**
  * *Nguyên nhân:* Ngày phán xét cuối cùng trước hội đồng 5 giáo sư đầu ngành Bách Khoa.
  * *Mục tiêu:* Thuyết trình tự tin và trả lời phản biện xuất sắc.
  * *Hành động gameplay:* Lươn mặc áo sơ mi trắng thẳng thớm, cắm dây máy chiếu. Bấm phím chuyển slide đúng nhịp. Trả lời các câu hỏi vặn vẹo hiểm hóc của hội đồng bằng kiến thức thực chiến vững vàng.
  * *Kết quả:* Thầy chủ tịch hội đồng mỉm cười gật đầu:  
    **`KẾT QUẢ: ĐẠT (ĐIỂM A)`**.  
    Lươn đã đạt phần bảo vệ; còn phải đối soát kết quả học vụ và học phí cuối trước xác nhận đủ điều kiện nhận bằng. A chỉ ghi khi thực đạt tiêu chí, không tự cấp từ lượt tập dượt.
* **Mission 44: [Cuộc gọi cho mẹ & TẮT ĐÈN PHÒNG 404 (Cú Chốt Cảm Xúc)]**
  * *Nguyên nhân:* Hoàn thành mọi nghĩa vụ học tập, kết thúc cuộc sống sinh viên KTX.
  * *Mục tiêu:* Báo tin cho mẹ và bàn giao lại phòng KTX.
  * *Hành động gameplay:*
    1. Bước ra hành lang giảng đường, điện thoại rung: `MẸ`. Bắt máy. Mẹ: *"Xong chưa con?"* → Lươn báo đúng state: *"Con bảo vệ xong rồi mẹ ơi"*; chỉ nói đã tốt nghiệp sau khi có xác nhận đầy đủ.
    2. Đối soát/đóng học phí cuối nếu còn nghĩa vụ có thật, xử lý ca/đồ khách tồn. Không tự cộng tiền hoặc sinh khoản nợ; số tiền/điều kiện CẦN AUTHOR. Về 404, tháo đồ của mình khỏi tường, gấp chăn, cất laptop/giáo trình/tuốc-nơ-vít vào balo.
    3. Giữ nguyên đồ đạc của ba người mới; Lươn chỉ dọn phần đồ của mình rồi nhìn lại góc phòng 404 lần cuối.
    4. Nhìn góc phòng lần cuối; “404 trống” là góc Lươn hoặc ký ức phòng cũ, không xóa đồ ba người mới. Bạn cũ đã chuyển đi Năm 5; Năm 6 là hội ngộ/tạm biệt, không dọn đi lần nữa.
    5. Prompt hiện lên: **`[ E ] Tắt đèn`**. Player bấm phím **E**. Bóng đèn tuýp phụt tắt cái *tách*. Lươn kéo cánh cửa sắt khép lại rồi tự tay trả chìa khóa KTX; không tắt đèn sinh hoạt của người mới đang sử dụng.
* **Mission 45: [Lễ tốt nghiệp & Nhìn lại sáu năm]**
  * *Nguyên nhân:* Ngày hội ngộ sau 6 năm thanh xuân.
  * *Mục tiêu:* Nhận bằng kỹ sư và sum họp cùng gia đình, bạn bè.
  * *Hành động gameplay:*
    * Bước xuống sảnh hội trường A5 Bách Khoa. Bố mẹ từ quê Thanh Hóa lặn lội vào ôm chầm lấy Lươn, tự hào nhìn con trai mặc áo cử nhân.
    * Long, Tộ và Đạm hội ngộ theo lịch có mặt đã author; Đạm cần cầu nối về từ du học, không teleport. Chưa author lịch về thì không giả đã có mặt. Hội thoại/flashback dựa trên history thực.
    * Sau lễ, cả nhóm đi từ khuôn viên trường tới bờ kênh Bến Bình Đông bằng một đoạn montage di chuyển. Họ uống bia tới sáng, nhắc lại chuyện cũ: thằng nào từng cúp học, thằng nào từng rớt môn, chuyện lẩu KTX và kết quả thực sự đã xảy ra trong save; không kể một nhánh chưa chơi như ký ức thật.
    * Objective cuối cùng: **`ĐI CÙNG ANH EM`**. Player điều khiển Lươn chạy lại khoác vai 3 người bạn. Màn hình mờ dần trong tiếng cười nói. Hồi tưởng Năm 1→6 từ event thật, trở lại góc Lươn đã trống, rồi thông điệp/credits. Chỉ lưu Finished sau khi hoàn thành tuyến tốt nghiệp. Sau credits có thể hiện tin “Ngày mai đi làm nhớ tới sớm nhé” nếu save có lời hẹn nhận việc hợp lệ; không tự tạo offer hoặc mở gameplay đi làm.

---

## 🎬 THÔNG ĐIỆP KẾT THÚC (CREDITS ROLL)

```text
══════════════════════════════════════════════════════════════════════════════
                                  PHÒNG 404

                 Có những người chỉ sống cùng ta vài năm,
              nhưng vài năm đó lại trở thành cả một thời thanh xuân.

                   Không quan trọng bạn đi nhanh hay đi chậm,
                     quan trọng là bạn đã không bỏ cuộc...

              Tưởng mình đến đây để học một ngành.
                    Cuối cùng lại học cách sống.
══════════════════════════════════════════════════════════════════════════════
```
