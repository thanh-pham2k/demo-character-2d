# Năm 1 — Đặc tả storyboard trước khi tạo ảnh

## Mục đích và phạm vi

Bộ ảnh là reference để duyệt không gian, trình tự và lựa chọn trước khi dựng GIF. Mỗi ảnh là một trạng thái có thể nhận biết trong game, không phải sprite hoặc animation hoàn chỉnh. Không coi ảnh là bằng chứng flow đã chạy đúng trong Unity.

**Bối cảnh storyboard Năm 1 đã chọn:** phòng 404 có hai người ở là Luồn và Long. Đạm và Tộ là bạn, có thể ghé phòng hoặc tham gia ăn lẩu; không được mặc định là người ở cùng trong bản storyboard này. Tóm tắt câu chuyện: [YEAR_ONE_STORY_TWO_ROOMMATES.md](story/YEAR_ONE_STORY_TWO_ROOMMATES.md). Đây là hướng cho storyboard Năm 1, chưa thay thế canon nhiều năm hoặc thay đổi runtime; các tài liệu gốc vẫn mô tả phòng bốn người và cần quyết định/migration riêng nếu mở rộng thay đổi.

Nguồn gameplay: [T-001](tasks/active/T-001-year-one-gameplay-plan.md). Khi spec này và T-001 khác nhau, T-001 quyết định logic hiện tại. Các tình huống dưới đây là ví dụ cho storyboard, không tự tạo luật gameplay mới. Những đề xuất môi trường, bố cục và thời điểm chỉ là định hướng hình ảnh, cần duyệt trước khi implement.

## Khóa hình ảnh trước khi tạo batch

- **Style:** bám sprite và scene hiện tại: 2D game, nét pixel/painted, góc nhìn phù hợp scene, màu ấm, nhân vật nhỏ đủ thấy không gian. Không chuyển sang minh họa anime chân dung hoặc photorealistic. Tham khảo `mobile-2d/Assets/Sprites/MVP/environment/room_404_backdrop.png`, sprite `characters/luon_idle_0.png`, scene và các asset cùng nhóm.
- **Luồn:** tóc đen, áo sáng hình khủng long, quần olive, dép xanh trong phòng; mặc và mang balo theo trạng thái flow. Dùng đúng thiết kế asset hiện có.
- **Long và bạn cùng phòng:** tham chiếu sprite `characters/long_standing.png`, `long_sitting.png` và các nhân vật hiện có; giữ nhận diện nhất quán. Tránh sinh thêm nhân vật giống Luồn.
- **Phòng 404:** bố trí cho hai người ở, mỗi người một chỗ ngủ, bàn và laptop; giữ cửa, cửa sổ, giường/tầng/thang, công tắc và lối đi theo scene khi có thể. Hai người chơi game dùng **hai laptop riêng**, mỗi người một máy. Không đổi kiến trúc, góc camera hoặc nhân vật giữa các ảnh chỉ để tạo sự khác biệt. Nếu scene hiện tại khác, ghi nhận là đề xuất, không khẳng định đã sửa scene.
- **Tiến triển phòng:** thay ánh sáng và thêm dần sách, ghi chú, vật dụng cá nhân, đồ ăn và kỷ niệm. Đây là đề xuất mỹ thuật; không ngụ ý các vật có state hoặc gameplay đã implement. Bố cục không bắt buộc khớp sơ đồ trong tài liệu ý tưởng nếu khác scene thật.
- **Không gian khác:** quầy KTX, hành lang/cầu thang, WC, căn tin, trạm bus, H6 và lớp học dựa trên asset có sẵn. Phòng thi có thể dùng lớp học với bố trí phù hợp; không yêu cầu tạo map mới.
- **UI:** hình chỉ chừa vị trí đặt HUD, khung thoại, highlight và dashboard. Chữ, con số và biểu tượng chính xác được thêm sau bằng công cụ dựng, không yêu cầu model vẽ chữ nhỏ. Tất cả thời gian/giá/điểm chưa xác nhận để placeholder, không bịa.
- **Nhánh:** đánh dấu bằng ID và mũi tên khi dựng GIF; không đặt hai lựa chọn loại trừ nhau vào cùng một trạng thái như thể cùng xảy ra. Một ảnh có thể cho thấy các object có thể tương tác, nhưng không được gộp các thời điểm.
- **Tham chiếu, không phải hợp đồng kỹ thuật:** ảnh tạo mới có thể khác asset thật; pivot, sorting, vị trí va chạm, hiệu ứng và animation phải được duyệt riêng khi implement.

## Cấu trúc và quy ước

36 frame dưới đây bao phủ đủ 12 mốc, thêm các nhánh có ý nghĩa và một mẫu chuyển mốc. Đây là storyboard cô đọng, không bao gồm mọi tương tác lặp, từng câu thi, save/load hay mọi trạng thái lỗi.

Mỗi frame ghi **bối cảnh → hành động → thông tin cần overlay → nối tiếp**. Overlay là nội dung dựng sau, không nhất thiết đã tồn tại chính xác trong UI hiện tại. Số frame là ID cố định; ID nhánh có thể được trình bày thành đoạn riêng trong GIF.

### Day 0 — Nhận phòng

**01 — Quầy KTX, 14:20.** Luồn đeo balo tại quầy, quản lý và giấy tờ; thể hiện tự nộp giấy rồi nhận chìa khóa phòng 404. Overlay: mục tiêu/nhãn nhận khóa. → 02. Vali chỉ thêm nếu phù hợp asset và không thay đổi flow.

**02 — Hành lang tầng 4.** Luồn đứng trước cửa 404, chìa khóa trong tay; cửa và số phòng rõ. Overlay: mở khóa → bước vào. → 03.

**03 — Khám phá phòng và chọn giường.** Luồn và Long tìm hiểu căn phòng hai người; thể hiện lựa chọn tầng trên/tầng dưới và chỗ của Long rõ ràng, không vẽ thêm người ở. Nếu dùng hai dãy giường như T-001 hiện tại, highlight dãy và tầng có thể chọn; đây là điểm cần chốt khi chuyển bản storyboard sang scene hai người. → 04A/04B.

**04A — Giường trên.** Luồn tới thang, leo và kiểm tra chỗ nằm; thấy rõ giường đã chọn. → 05.

**04B — Giường dưới.** Luồn đi tới giường dưới và nằm thử, không leo thang. → 05. Đây là nhánh thay thế 04A.

**05 — Ổn định phòng.** Sau lựa chọn, balo được đặt tại chỗ đã chọn, Luồn dọn đồ và làm quen Long. Overlay: đặt balo → dọn đồ → trò chuyện. → 06.

**06 — Bữa đầu ở căn tin.** Quầy, người bán, khay và món; player xem giá, chọn món trong ngân sách rồi xác nhận ăn. Overlay dùng giá/món thực tế sau khi đối chiếu, không tự thêm thực đơn. Sau ăn tự về 404. → 07.

**07 — Tự do và chuyển ngày.** Trong 404, player có thể nói chuyện, xem đồ, nghỉ hoặc tới giường. Khi chọn ngủ, overlay preview nguồn/đích, thời gian và việc bị lỡ; nút hủy/xác nhận. Hủy hoặc lỗi lưu giữ state nguồn; xác nhận thành công mới chuyển mốc. Hình minh họa một cơ chế được tái sử dụng cho các ngày sau. → 08.

### Day 1 — Ngày học đầu

**08 — Routine và lịch.** Sáng tại phòng, thể hiện trạng thái sau thức dậy/vệ sinh, sách và balo cạnh bàn; overlay trình tự đánh răng → rửa mặt → thay đồ → xem lịch → lấy GT1/VL1 → đeo balo. Lịch 08:00 GT1, 10:00 VL1. Nếu cần thấy từng thao tác, dựng thêm insert từ asset hiện có. → 09.

**09 — Đi học.** Luồn mang balo, hành lang/trạm/bus/H6 theo tuyến có sẵn; một trạng thái ở trạm là đủ, montage có thể biểu đạt quãng đường. Overlay giờ xe theo lịch đã author. → 10.

**10 — Lớp học và về phòng.** Luồn ngồi trong lớp, giảng viên và bạn học; overlay vào lớp → ngồi → điểm danh → học, với GT1 08–10 và VL1 10–12. Sau tiết player về bằng bus và cất balo; chuyển cảnh thay vì vẽ đồng thời hai lớp. → 11.

### Day 2 — Hai lựa chọn riêng

**11 — Long rủ trước giờ học.** Trong 404, Long cạnh laptop, Luồn gần sách/balo; overlay lời mời và hai hướng đi học hoặc chơi. Lịch NMT 08–10, GT1 10–12. → 12A/12B.

**12A — Chơi và cảnh báo.** Luồn và Long ngồi tại **hai laptop riêng**, hai màn hình nhìn thấy, không thêm bản sao nhân vật; đồng hồ tiếp tục chạy. Overlay cảnh báo T-30/T-15/T-5 và lựa chọn rời game/tiếp tục theo state thực tế. → 13. Không tự gán mỗi trận một thời lượng.

**12B — Chọn đi học.** Luồn lấy sách, đeo balo và rời phòng; Long ở vị trí riêng. → 13. Đây là nhánh thay thế 12A.

**13 — Hậu quả đến lớp.** Cùng lớp H6 với overlay các trạng thái đúng giờ/trễ/vắng theo giờ thực tế; nếu hết giờ tiết thì không cấp nội dung bài học. Dựng biến thể từ một layout; không hiển thị đồng thời các kết quả loại trừ. → 14.

**14 — Lựa chọn buổi tối.** Sau khi về phòng/cất balo, bàn học và Long là hai hướng có thể chọn. Overlay học sớm → topic/bài tập; chơi cùng Long → thời gian/quan hệ, kiến thức chưa học vẫn cần bù. Không tự đặt mức tăng hoặc chi phí. → 15.

### Day 28–30 — Giữa kỳ

**15 — Day 28, dashboard ôn.** Tại bàn/laptop, phòng đã có thêm sách và ghi chú. Overlay GT1/VL1/NMT, attendance và topic đã học/còn yếu. → 16.

**16 — Ôn và feedback.** Luồn học ở bàn, sách/vở và topic đang chọn. Overlay topic, thời lượng thực tế từ state và feedback đã ổn/còn yếu; không hứa tự chuyển mức sau một thời lượng cụ thể. Có thể tiếp tục ôn hoặc ngủ chuyển Day 29. → 17.

**17 — Day 29, lớp bình thường.** H6; overlay VL1 08–10, NMT 10–12, điểm danh theo giờ. Đây không phải ngày thi. → 18.

**18 — Day 30, thi giữa kỳ.** H6/phòng thi, học sinh làm bài, giám thị; overlay GT1 08–09, VL1 10–11, NMT 13–14 và câu hỏi tương ứng. Mức chuẩn bị ảnh hưởng độ phù hợp của đề, kết quả theo câu trả lời. → 19.

**19 — Feedback giữa kỳ.** Luồn xem kết quả tại 404 hoặc UI phù hợp. Overlay STRONG/NEUTRAL/WEAK theo topic và gợi ý ôn; không ghi PASS/FAIL môn hay cấp tín chỉ. → 20.

### Day 92–99 — Chuẩn bị và thi HK1

**20 — Day 92, ngày ưu tiên.** Phòng và bàn nhiều tài liệu hơn; dashboard hiển thị attendance, bài tập, topic yếu, deadline 22:00, eligibility và thời gian còn lại. Choice: làm bài, học bù hoặc ôn; mỗi lựa chọn có chi phí. → 21.

**21 — Hậu quả deadline.** Cùng phòng vào gần hạn, overlay trạng thái bài và thời gian còn lại, cho thấy việc chọn hoạt động có thể bỏ lỡ deadline. Biến thể đã nộp/chưa nộp thể hiện SAFE/OVERDUE và eligibility; không giả định có thể nộp một bài chưa làm. → 22.

**22 — Day 99, thi cuối kỳ.** Player vào phòng thi sau kiểm tra eligibility; overlay 3 câu, 20 giây/câu, world clock dừng khi timer thi chạy. Biến thể không đủ điều kiện/bỏ thi cho thấy lý do, không vào câu hỏi. → 23.

**23 — Tổng kết HK1.** Overlay từng môn PASS/FAIL, lý do và tín chỉ thực tế; không hard-code tín chỉ mẫu. Player về 404, xem kết quả/nghỉ rồi preview sang Day 113. → 24A/24B.

### Day 113 — Bắt đầu HK2 và bus

**24A — Kịp bus.** Tại trạm, countdown/chuyến 07:40 và Luồn lên xe. → 26. Nhánh thay thế 24B.

**24B — Lỡ bus.** Xe đã rời trạm; overlay chuyến đã lỡ và lựa chọn chờ chuyến sau. Không bịa giờ chuyến sau hoặc giờ đến. → 25.

**25 — Recovery chuyến sau.** Player chờ, xe tiếp theo tới và player lên xe. Overlay giờ thực tế từ state và dự kiến nếu game có thông tin đó. → 26.

**26 — Điểm danh HK2.** H6 GT2 08–10/VL2 10–12; overlay đúng giờ/trễ/vắng theo thời điểm điểm danh thực tế, không tự biến trễ thành đúng giờ. Tan học, bus về và cất balo. → 27.

### Day 155 — Học và sinh hoạt phòng

**27 — Hai tiết ban ngày.** H6, overlay VL2 08–10 và Lập trình 10–12; player học/điểm danh theo trạng thái thực tế, sau đó về 404. → 28.

**28 — Buổi tối và hai laptop.** Bố cục phòng nhất quán, hai bàn hoặc vị trí máy phù hợp; Luồn và Long có thể chơi cùng trên **hai laptop**, hoặc player chọn học/nói chuyện/nghỉ. Overlay các hướng hoạt động; không gộp chúng thành một hành động bắt buộc. → 29A/29B.

**29A — Đèn còn sáng.** Đêm, Long muốn ngủ và trao đổi; player phản ứng, đứng dậy đi tới công tắc. Hai người là hai nhân vật nhất quán; không lặp Luồn. → 30.

**29B — Đèn đã tắt.** Biến thể không có xung đột bật đèn, tùy state. → 30. Không đặt lựa chọn “chơi thêm 30 phút” như luật đã chốt.

**30 — Nghỉ tại giường.** Ánh sáng phù hợp state, Luồn và Long tới đúng giường; Luồn leo thang nếu đã chọn tầng trên. Có thể nằm nghỉ rồi trở lại hoạt động hoặc chọn ngủ/preview Day 204. → 31.

### Day 204 — Học và chuẩn bị HK2

**31 — Lịch và lựa chọn buổi học.** Dashboard tại phòng: GT2 08–10, Lập trình 10–12; overlay đi học hoặc ưu tiên việc khác, bỏ khung giờ được ghi vắng. Nếu đi học, montage bus/H6/về phòng. → 32.

**32 — Ưu tiên trước deadline.** Sau lựa chọn buổi học, cùng 404 với dấu vết cuối năm; overlay attendance, bài tập, weak topics, eligibility, deadline và thời gian thực tế. Các hướng làm bài/học bù/ôn có chi phí, đến 22:00 kiểm tra deadline. Chuẩn bị đồ thi và preview Day 211. → 33.

### Day 211–212 — Thi, lẩu, bàn giao

**33 — Thi HK2.** Tái dùng layout thi cuối kỳ: GT2 08–09, VL2 10–11, Lập trình 13–14; cho thấy eligible, chờ, bỏ thi hoặc câu hỏi theo nhánh. → 34.

**34 — Tổng kết Năm 1.** Overlay sáu môn, PASS/FAIL, reason, tín chỉ và môn nợ lấy từ state. Không gắn game over khi thiếu 32 tín chỉ. → 35.

**35 — Chuẩn bị lẩu.** Trở lại 404, cất balo; bếp/nồi, túi nguyên liệu, khay và bàn xuất hiện theo thứ tự chuẩn bị → sắp chỗ → gọi bạn. Không vẽ bữa ăn đã hoàn thành trước các bước này. → 36.

**36 — Ăn lẩu và kết năm.** Cùng phòng, Luồn, Long và bạn ăn; thoại thay theo kết quả, kể cả còn môn nợ vẫn tham gia. Sau ăn, player tới đúng giường; nằm nghỉ không kết thúc năm, chọn ngủ mở preview Day 212 07:20. Hủy/lỗi lưu giữ Day 211; commit thành công bàn giao snapshot Year 2. Dựng các khoảnh khắc này thành montage từ cùng bố cục, không giả vờ xảy ra đồng thời.

## Kế hoạch batch khi được duyệt

- Khóa reference trước: scene 404, Luồn, Long, giường, lớp và trạm; dùng asset hiện có nếu đủ. Reference được cung cấp trực tiếp cho từng batch, không dựa vào trí nhớ model.
- Dự kiến **9 sheet × 4 frame**, mỗi sheet grid **2×2**, đọc trái → phải, trên → dưới. Cell cùng kích thước; lề ngoài 0, đường phân cách đen đồng nhất; không để hình vượt cell. Kích thước mục tiêu **768×512 px/cell, canvas 1536×1024 px**; nếu công cụ trả kích thước khác, ghi kích thước thực tế và crop theo đường phân cách đã kiểm tra.
- Batch 1: 01–03, 04A; batch 2: 04B–07; batch 3: 08–11; batch 4: 12A, 12B, 13, 14; batch 5: 15–18; batch 6: 19–22; batch 7: 23, 24A, 24B, 25; batch 8: 26–29A; batch 9: 29B, 30–32. **Còn 33–36 là batch 10**: tổng cộng **10 sheet × 4 frame**. Mỗi sheet phải mang cùng reference và style; nhánh đặt ID bằng metadata, không phụ thuộc chữ model vẽ.
- Sau generate: giữ sheet gốc; ghi prompt, reference, kích thước thực, grid, lề và tọa độ crop; cắt 40 cell, đặt tên theo ID, kiểm tra không lẫn/cụt hình. 36 là số thứ tự cuối, còn số ảnh thực tế là **40** vì có các nhánh A/B. Pivot/PPU: **không áp dụng** cho storyboard; nếu chuyển thành sprite Unity, xác định riêng sau.
- Chỉ tạo sheet mới cho các trạng thái cần minh họa; tái sử dụng ảnh/asset cũ khi đáp ứng. Các sheet storyboard là ảnh tổng hợp, không tuyên bố các cell là animation frame có thể ghép liền mạch.

## Giới hạn duyệt

- Cần đối chiếu scene và hình ảnh sau generate: nhận diện Long, số laptop, bố cục, đúng dãy/tầng giường, không lặp nhân vật và thứ tự hành động.
- UI, con số, outcome, save/load và thời gian phải đối chiếu source trước khi đưa vào bản GIF cuối; không dùng chữ model vẽ làm nguồn dữ liệu.
- Chưa có GIF, chưa tạo ảnh mới trong tài liệu này và chưa nghiệm thu runtime. Tài liệu F8: [MANUAL_CHECKPOINTS.md](MANUAL_CHECKPOINTS.md).
