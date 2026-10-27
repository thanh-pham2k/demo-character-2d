# Bộ prompt tạo asset core Năm 1

## Dùng trước khi chạy song song

- Đây là **prompt thử tỉ lệ và art direction**, chưa phải thông số production đã duyệt. Tạo phòng, Luồn, giường và bàn trước; ghép thử một cảnh ở kích thước màn hình mục tiêu rồi chốt PPU, pivot và tỉ lệ. Các batch còn lại có thể chạy song song sau khi có reference đã duyệt.
- Reference bố cục: `deliverables/T-001-gif-assets/year-one-v3/b01-03.png`. Ảnh này chứa nhân vật và nội thất, là reference, không phải nền tách lớp. Reference style bổ sung: `deliverables/T-001-gif-assets/core-preview/core-sheet.png`; dàn nhân vật ở sheet này chưa đúng nhận diện Long/Đạm/Tộ.
- Phòng 404 có **hai người ở: Luồn và Long**, mỗi người một giường gác xép phía trên, bàn học bên dưới và laptop riêng. Không có nệm tầng dưới. Đạm/Tộ là bạn tới thăm. Giữ bố cục, góc nhìn, kích thước tương quan và bảng màu nhất quán.
- Bố cục phòng điều chỉnh: cửa chính ở **tường sau, bên phải cửa sổ**; công tắc sát cửa, hai tường bên dành cho hai giường gác xép, lối đi giữa dẫn tới cửa. Bố cục này đang được duyệt bằng ảnh, chưa chứng minh va chạm/tỉ lệ trong Unity.
- Các số sau là **kích thước đầu ra sau xử lý dự kiến**, không bảo đảm công cụ tạo ảnh trả đúng pixel. Không ép thay đổi tỉ lệ hoặc nội suy làm nhòe: kiểm tra, cắt và căn canvas sau generate; nếu thiếu chi tiết hoặc bị cắt cụt thì tạo lại batch. Background 960×540 là kích thước thiết kế thử, không mặc định mỗi ô của sheet phải là 960×540.
- Mỗi prompt là **một sheet nhiều ô**, không tạo từng ảnh lẻ. Ô kích thước bằng nhau, viền đen #000000, không để nội dung lấn ô. Thứ tự đọc trái → phải, trên → dưới. Sau khi tạo: lưu sheet, prompt, reference, kích thước thực tế và tọa độ crop; slice từng ô và kiểm tra. Pivot/PPU nêu dưới đây là dự kiến, chưa được chứng minh phù hợp Unity.
- Ưu tiên dùng lại asset hiện có nếu đáp ứng được. Các sheet tạo mới là ảnh bitmap tham khảo; asset có thể cần xử lý tách lớp, chỉnh nét, alpha và kiểm tra bản quyền/nguồn trước khi dùng.

## Khối chung — thêm vào đầu mỗi prompt

```text
Tạo sheet asset cho game 2D đời sống sinh viên Việt Nam. Dùng ảnh phòng 404 đã cung cấp làm reference phong cách: tường vàng cũ, sàn gạch, màu ấm, nhân vật chibi pixel-art, camera game hơi nhìn từ trên xuống. Giữ cùng palette và góc nhìn; không chuyển sang anime chân dung hoặc ảnh chân thực. Mỗi ô có đường chia đen #000000 rõ ràng, không chồng nội dung giữa ô; không chữ, logo, watermark. Các ô là asset riêng để xem và slice; không vẽ UI giả chứa chữ.
```

## Batch 1 — Background KTX

Grid **2×2**; 4 ô cùng tỉ lệ **16:9**; đầu ra dự kiến mỗi nền 960×540. Một sheet không đảm bảo đủ độ phân giải: ghi kích thước thực tế, không upscale mù. Background cuối không có nhân vật, giường/bàn/cửa và đồ tương tác để tách lớp; chi tiết cố định như tường, sàn, cửa sổ được giữ. Cửa sổ/khoảng chừa cửa phải nhất quán với bản đồ scene.

```text
Tạo 4 background độc lập trong grid 2×2, cùng camera và tỉ lệ 16:9. Ô 1: phòng 404 nhìn theo reference, hai khu ngủ hai bên, cửa sổ giữa, chừa vị trí cửa chính và công tắc, sàn trống và lối đi rõ; không vẽ người hoặc giường, bàn, laptop. Ô 2: sảnh KTX và quầy lễ tân, không người, chừa chỗ quầy tương tác. Ô 3: hành lang tầng 4, chừa vị trí cửa 404 và lối cầu thang. Ô 4: WC dùng chung với gương và chỗ chậu rửa. Ánh sáng ban ngày đồng nhất, không gộp nhân vật.
```

Pivot: theo canvas; PPU: chưa chốt. **Cổng duyệt:** căn vị trí cửa, thang và lối đi; xác định những vật có thể tương tác cần tách trước khi dùng.

## Batch 2 — Background căn tin và campus

Grid **2×2**; 4 ô 16:9; đầu ra dự kiến mỗi nền 960×540. Cảnh lớp dùng chung cho thi; bố trí bàn/đạo cụ khác xử lý lớp riêng.

```text
Tạo 4 background 2×2 cùng phong cách và camera batch 1. Ô 1: căn tin sinh viên, sàn, tường, khu phục vụ, không nhân vật. Ô 2: trạm bus KTX với lối lên xe, không vẽ bus hoặc người vào nền. Ô 3: khu cổng H6, lối vào rõ, không người. Ô 4: giảng đường H6, bảng và không gian đủ đặt bàn ghế tách riêng; bố cục có thể dùng lại cho học và thi. Không chữ hoặc biển có nội dung giả.
```

Pivot/PPU: như batch 1 sau khi duyệt.

## Batch 3 — Luồn và Long: đứng/đi

Grid **4×2**; mỗi ô dự kiến canvas **64×128 px**, alpha trong suốt sau xử lý; nhân vật chiếm tỉ lệ tương tự, không chạm mép. Thứ tự: Luồn đứng phải, đi phải 1–3; Long đứng phải, đi phải 1–3. Thêm cùng reference sprite hiện có nếu phù hợp: `mobile-2d/Assets/Sprites/MVP/characters/luon_idle_0.png`, `long_standing.png`.

```text
Tạo sheet 4×2 gồm hai nhân vật game full-body tách biệt trên nền phẳng trắng #FFFFFF. Hàng 1: Luồn, tóc đen, áo sáng hình khủng long, quần short olive, dép xanh: đứng và ba pose đi sang phải. Hàng 2: Long theo ảnh reference Long: đứng và ba pose đi sang phải. Cùng góc nhìn, kích thước đầu/thân phù hợp, đường chân nằm cùng một hàng trong mỗi ô; mỗi nhân vật chỉ xuất hiện một lần mỗi ô. Không gộp bóng hoặc sàn vào nhân vật. Không thay thiết kế giữa các frame.
```

Pivot dự kiến: (0.5, 0) tại bàn chân; PPU chốt sau cảnh thử. Mirror chỉ sau khi kiểm tra phụ kiện bất đối xứng. 3 pose là điểm khởi đầu, chưa khẳng định đủ loop mượt. Nền trắng không đồng nghĩa alpha: tách nền và kiểm tra viền trước khi dùng.

## Batch 4 — Nhân vật phụ

Grid **3×2**, 6 ô, cùng chiều cao tham chiếu với Luồn. Đạm và Tộ theo `characters/dam_standing.png`, `to_standing.png`; NPC không tự mang thiết kế của nhân vật chính.

```text
Tạo 6 nhân vật full-body riêng trong grid 3×2 trên nền trắng #FFFFFF: Đạm theo reference, Tộ theo reference, quản lý KTX, người bán căn tin, giảng viên, sinh viên phụ. Mỗi ô một người ở tư thế đứng; cùng tỉ lệ cơ thể, góc nhìn, baseline bàn chân và palette với Luồn/Long. Chừa đủ khoảng trống quanh nhân vật, không đạo cụ lớn che thân, không chữ.
```

Pivot dự kiến giữa bàn chân; canvas tối thiểu 64×128 hoặc lớn hơn nếu nhân vật cần, nhưng chiều cao hiển thị theo cảnh thử. Không ép cả 6 nhân vật bằng cùng chiều cao vật lý.

## Batch 5 — Nội thất phòng

Grid **2×2** cho nhóm vật lớn; canvas mỗi ô đồng nhất trong sheet, nhưng từng vật có tỉ lệ tự nhiên. Asset phải khớp góc nhìn và tỉ lệ với reference phòng; tách layer che trước/sau nếu dùng được.

```text
Tạo 4 asset nội thất độc lập trên nền trắng #FFFFFF, grid 2×2: khung giường gác xép với **một nệm trên cao, khoảng trống bên dưới để đặt bàn, không có nệm dưới**; bàn học đặt vừa khoảng trống bên dưới; ghế; thang riêng. Mỗi ô một asset nguyên vẹn, không nhân vật, không cắt cụt. Thiết kế phù hợp căn phòng reference và hai người dùng. Không gộp bàn, thang hay laptop vào giường; giữ vật liệu và nét nhất quán.
```

Pivot dự kiến ở điểm tiếp xúc sàn; một giường là mẫu tái sử dụng cho hai người. Cửa lấy từ asset phù hợp hiện có hoặc tạo trong batch khác nếu cần. Kiểm tra khoảng trống dưới giường đủ để ghép bàn và ghế, sorting/occlusion và tỉ lệ khi ghép thử.

## Batch 6 — Đạo cụ nhỏ

Grid **4×2**, ô vuông; thử **64×64 px/cell** sau xử lý, nhưng giữ kích cỡ tương quan trong Unity theo cảnh, không coi kích thước cell là kích thước vật lý.

```text
Tạo 8 đạo cụ riêng, grid 4×2 trên nền trắng #FFFFFF, mỗi ô một vật: laptop mở; balo; chồng sách; chìa khóa; cốc bàn chải; công tắc đèn; khay cơm; nồi và bếp lẩu. Vật nguyên vẹn, cùng hướng sáng và nét game, không chữ, không bóng nền gộp, không chạm mép. Laptop sẽ được đặt hai bản trong phòng; không cần hai mẫu giống hệt.
```

Pivot dự kiến điểm tiếp xúc với sàn/bàn; alpha và viền cần kiểm tra. Nguyên liệu và bát đĩa nếu chưa đủ để kể flow thì gom thêm một batch sau duyệt.

## Batch 7 — Pose tương tác

Grid **4×2**, 8 ô. Dùng Luồn reference và **cùng canvas, baseline/pivot với batch 3** khi có thể. Đây là pose đại diện, chưa phải toàn bộ animation.

```text
Tạo 8 pose Luồn trên nền trắng #FFFFFF, grid 4×2: ngồi học, ngồi dùng laptop, mang balo đứng, nhặt sách, chạm công tắc, leo thang giường, nằm nghỉ, ngủ. Giữ đúng thiết kế Luồn và tỉ lệ batch nhân vật. Nhân vật tách khỏi nội thất; nếu pose cần ghế/thang làm mốc hãy chỉ gợi vị trí, không bake đồ vào nhân vật. Không cắt cụt cơ thể, không chữ.
```

Pivot không thể mặc định giống nhau cho ngồi/nằm/leo: ghi anchor theo từng loại và căn với prop thật trong Unity. Nếu action cần chuyển động, tạo batch frame riêng sau khi pose được duyệt.

## Batch 8 — UI

Grid **4×2**, gồm icon/khung đại diện; UI cuối nên được dựng bằng Unity, không bake chữ hay thông số.

```text
Tạo sheet UI 4×2 đồng bộ game 2D pixel-art: icon thời gian, lịch học, balo, sách, quan hệ, bus, nút lựa chọn, khung hội thoại. Ô đều, nền trắng #FFFFFF, nét rõ, không chữ, không số, không logo. Để các thành phần tách biệt, có khoảng trống quanh mép, phù hợp ghép thành UI Unity.
```

Icon thử 64×64 px; panel/nút phải xác định kích thước thực tế và 9-slice riêng, không ép khung hội thoại vào cùng kích thước icon khi sản xuất.

## Kiểm tra trước khi chốt

1. Khớp đúng số ô, thứ tự và không cắt lẫn; lưu sheet gốc và tọa độ crop thực tế.
2. Kiểm tra alpha, viền trắng, kích thước, pivot, baseline; thử Luồn cạnh giường/bàn trong phòng ở màn hình mục tiêu.
3. Kiểm tra hai laptop và hai chỗ ngủ; cửa/công tắc có vị trí tương tác. Xác định lớp trước/sau và bóng riêng.
4. Kiểm tra pose/animation bằng tay; không suy ra khả năng dùng trong Unity chỉ từ sheet preview.
5. Ghi PPU, import settings, frame order và các thay đổi đã duyệt; nếu cần tạo thêm, gom thành batch cùng loại.
