# Prompt tạo asset phòng 404 — 4 batch

## Mục tiêu và reference

Tạo bộ ảnh để dựng **một phòng 404 có hai người ở**: Luồn và Long. Mỗi người có một giường gác xép với **một nệm phía trên, bàn học và laptop bên dưới**. Hai khu nằm hai bên phòng, lối đi giữa dẫn tới cửa ở tường sau; cửa nằm bên phải cửa sổ.

- Reference bố cục đã duyệt: `deliverables/core-year1/room-layout-v3/03-furnished-day.png`.
- Reference nền trống: `deliverables/core-year1/room-layout-v3/01-empty-day.png`.
- Reference giường/bàn thử: `deliverables/core-year1/batch-05-loft-v2/sheet.png`. Đây là concept, chưa xác nhận ghép đúng tỉ lệ.
- Mọi batch phải dùng cùng reference, góc camera, palette, hướng sáng và kích thước tương quan. Không mặc định lật ngang tạo được phối cảnh đúng: phải thử ghép.
- Bắt đầu bằng **batch 1 và 2**, ghép nháp với nhân vật đại diện để duyệt tỉ lệ; sau đó chạy batch 3 và 4 theo tỉ lệ đã duyệt. Có thể chuẩn bị prompt song song, nhưng không coi tỉ lệ chưa duyệt là thông số sản xuất.

## Quy ước cho mọi batch

Mỗi batch là **một sheet nhiều ô**, đọc từ trái sang phải, từ trên xuống dưới. Ô bằng nhau, đường phân cách đen `#000000`, lề ngoài 0, mỗi asset có khoảng trống an toàn và không vượt ô. Nền trong mỗi ô trắng phẳng `#FFFFFF` để dễ xem; nền trắng **không phải alpha**. Sau khi sinh, lưu sheet nguồn, prompt, reference, kích thước thực và tọa độ crop; cắt từng ô và kiểm tra mép, vật bị cắt và nội dung lấn ô.

Canvas 1536×1024 px là **mục tiêu**, không bảo đảm công cụ tạo ảnh trả đúng. Nếu kích thước hoặc grid thực tế khác, ghi đúng kích thước/crop thực; không kéo giãn làm hỏng pixel art. PPU và pivot chưa chốt: ghi dự kiến và xác định sau khi ghép cảnh. Không tự tạo các frame animation từ một pose tĩnh.

**Khối phong cách dùng chung — thêm vào đầu mỗi prompt:**

```text
Asset cho game 2D đời sống sinh viên Việt Nam. Dùng ảnh phòng 404 được cung cấp làm reference: pixel-art/chibi, tường vàng cũ, sàn gạch, màu ấm, ánh sáng ban ngày, góc nhìn hơi từ trên xuống. Giữ cùng camera, đường nét và palette giữa tất cả batch. Không chữ, logo, watermark, người hoặc bóng nền gộp vào vật. Asset trong mỗi ô phải nguyên vẹn, không chạm mép hoặc lấn ô. Đây là thiết kế asset tách lớp, không phải screenshot nhiều vật gộp lại.
```

## Batch 1 — Nền và cửa

**Grid 2×2, 4 ô.** Mục tiêu sheet 1536×1024; ô dự kiến 768×512. Thứ tự: (1) nền phòng trống, (2) cửa sổ và cảnh ngoài, (3) cửa với khung, (4) công tắc. Nền là khung cảnh, không bắt buộc cùng kích thước vật lý với các prop; cửa sổ, cửa và công tắc phải được đặt trên cùng mặt phẳng tường theo ảnh reference.

```text
Tạo một sheet 2×2 với 4 ô bằng nhau theo thứ tự sau.
1. Nền kiến trúc phòng 404 trống: tường vàng, sàn gạch và lối đi giữa; tường sau chừa đúng vị trí cửa sổ bên trái và cửa bên phải theo reference. Không vẽ cửa sổ, cửa, công tắc, người hay nội thất vào nền. Đặt toàn bộ nền 16:9 bên trong ô, không méo phối cảnh.
2. Một cửa sổ có khung và cảnh trường/cây phía ngoài, nhìn chính diện theo tường sau, hoàn chỉnh và tách khỏi tường.
3. Một cửa phòng đóng kèm khung cửa, đúng góc nhìn tường sau; chỉ một tay nắm. Không vẽ số phòng hoặc chữ.
4. Một công tắc đèn tách riêng, góc nhìn phù hợp tường sau.
Giữ cửa đủ lớn so với nhân vật đại diện và nhỏ hơn không gian trống giữa cửa sổ với tường bên; không thay đổi bố cục giữa các asset.
```

**Lưu ý:** một ô sheet có thể không đủ độ phân giải cho nền 16:9 mục tiêu thử 960×540. Ghi kích thước thực; không tuyên bố đạt 960×540 nếu không đủ pixel. Cửa và công tắc cần alpha riêng; với cửa sổ, quyết định sau khi duyệt có cần tách cảnh ngoài thành layer riêng để đổi ngày/đêm hay không.

## Batch 2 — Giường gác xép

**Grid 2×2, 4 ô.** Mục tiêu sheet 1536×1024; ô dự kiến 768×512. Thứ tự: (1) khung giường, (2) nệm/chăn, (3) thang, (4) lớp che trước. Một mẫu trước; tạo biến thể trái/phải chỉ nếu ghép hoặc lật không khớp.

```text
Tạo một sheet 2×2 gồm các thành phần cùng một giường gác xép, mỗi thành phần riêng một ô, trên nền trắng #FFFFFF.
1. Khung giường kim loại gác xép: chỉ một vị trí nệm trên cao; không có nệm, giường hoặc kệ ở tầng dưới. Chừa khoảng trống đủ đặt bàn học và ghế bên dưới. Không gộp thang.
2. Một nệm và chăn/gối phù hợp chính xác với mặt giường phía trên, không có khung giường.
3. Thang kim loại độc lập, chiều cao và góc nhìn khớp với khung giường, đủ để leo từ sàn lên giường.
4. Thành phần lan can/chân giường phía trước, thiết kế như lớp foreground độc lập để che nhân vật khi phù hợp; phải khớp hình dạng và vị trí với khung ở ô 1.
Mỗi vật đầy đủ, không người, không nội thất phía dưới, không cắt cụt.
```

**Pivot dự kiến:** khung/thang tại điểm chạm sàn; nệm và foreground cần anchor riêng. Lớp foreground trong ảnh tạo ra chưa chắc khớp pixel với khung: kiểm tra kỹ và chỉnh/tạo lại nếu không khớp. Kiểm tra cả độ mở để bàn, ghế và nhân vật đặt vừa.

## Batch 3 — Khu học

**Grid 2×2, 4 ô.** Mục tiêu sheet 1536×1024; ô dự kiến 768×512. Thứ tự: (1) bàn, (2) ghế, (3) laptop, (4) đèn bàn. Mỗi ô một vật riêng, cùng góc nhìn với batch 2.

```text
Tạo một sheet 2×2 gồm: 1 bàn học bằng gỗ đặt vừa phía dưới giường gác xép trong reference; 1 ghế học; 1 laptop mở; 1 đèn bàn. Mỗi vật trong một ô độc lập, trên nền trắng #FFFFFF, không dính vật khác. Laptop không có chữ hoặc logo. Giữ hình dáng và góc nhìn để bàn, ghế và laptop có thể ghép dưới giường; ánh sáng cùng batch 2. Không vẽ nhân vật.
```

**Pivot dự kiến:** bàn/ghế tại điểm tiếp xúc sàn; laptop/đèn tại điểm tiếp xúc mặt bàn. Scene dùng **hai laptop riêng**; không cần tạo hai ảnh giống nhau. Nguồn sáng bật/tắt có thể xử lý riêng trong Unity; ảnh đèn là vật thể, không bake ánh sáng cả phòng.

## Batch 4 — Lưu trữ và đồ cá nhân

**Grid 4×2, 8 ô.** Mục tiêu sheet 1536×1024; ô dự kiến 384×512. Thứ tự: (1) tủ/kệ nhỏ, (2) chồng sách, (3) vở mở, (4) balo, (5) chậu cây, (6) đồng hồ báo thức, (7) khăn gấp, (8) cốc bút. Đây là đạo cụ phòng; đồ ăn/lẩu và UI được làm batch riêng khi cần.

```text
Tạo một sheet 4×2 với 8 đạo cụ phòng 404 độc lập, một vật mỗi ô: tủ/kệ nhỏ, chồng sách, vở mở, balo, chậu cây nhỏ, đồng hồ báo thức, khăn gấp, cốc bút. Nền trắng #FFFFFF, khoảng trống đều, không nhân vật, không chữ hoặc logo. Cùng palette và góc nhìn với các batch nội thất. Không vẽ bàn hoặc sàn phía dưới các vật.
```

**Pivot dự kiến:** ở điểm tiếp xúc mặt bàn/sàn. Kích thước ô không quyết định kích thước vật trong phòng; căn tỉ lệ sau khi ghép. Muốn khác biệt khu Luồn/Long thì thay vị trí hoặc đồ cá nhân trước khi tạo biến thể mới.

## Kiểm tra sau khi tạo 

1. Đủ ô, đúng vật, đúng thứ tự; không có nệm dưới và không gộp bàn/laptop vào khung giường. Lưu sheet và metadata thật.
2. Tách nền trắng/alpha theo phương pháp phù hợp; kiểm tra halo, lỗ thủng và mép pixel. Không tuyên bố đã trong suốt khi PNG còn nền trắng.
3. Ghép **nền + hai giường + hai bàn + cửa + nhân vật đại diện**. Kiểm tra cửa không bị che, lối đi tới cửa, thang, khoảng ngồi, tỉ lệ cửa–người và phối cảnh trái/phải.
4. Kiểm tra sự ăn khớp của nệm, thang, lớp foreground và vật đặt trên bàn. Nếu phải thay đổi thứ tự che, tách lớp thêm theo cảnh thực tế.
5. Chốt canvas sprite, pivot, PPU, scale, sorting và import settings sau khi ghép. Chưa cần tạo animation hoặc logic gameplay ở giai đoạn asset phòng.

**Lưu ý flow:** storyboard/T-001 cũ còn nhánh giường dưới, không phù hợp bố cục này. Không lấy nó làm yêu cầu tạo asset; quy tắc chọn chỗ ngủ mới cần được author riêng.
