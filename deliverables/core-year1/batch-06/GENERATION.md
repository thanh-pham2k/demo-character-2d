# Batch 06 — Đạo cụ nhỏ

Trạng thái: đang tạo. Mục đích: thử art direction, chưa dùng trong Unity. Grid dự kiến 4×2, 8 ô bằng nhau, đọc trái sang phải, trên xuống dưới; nền trắng #FFFFFF, lề đủ rộng, canvas 1536×1024 dự kiến, ô 384×512 dự kiến. Cell thực tế phụ thuộc công cụ, kích thước 64×64 chỉ là mục tiêu sau xử lý, không resize mù. Ô: laptop; balo; chồng sách; chìa khóa; cốc bàn chải; công tắc; khay cơm; nồi và bếp. Pivot đề xuất ở điểm tiếp xúc bàn/sàn, PPU chưa chốt.

Nguồn style: `deliverables/T-001-gif-assets/year-one-v3/b01-03.png`. Asset cũ trong `mobile-2d/Assets/Sprites/MVP` được xem là reference; mục tiêu batch là ứng viên đồng bộ với room chuẩn mới, chưa chứng minh thay thế asset cũ.

Prompt: Create one sprite design sheet, exactly 4 columns by 2 rows of eight equal cells, strict grid with solid black separators, flat pure white #FFFFFF backdrop, even generous margin around every object. Match the provided dorm reference: warm muted colors, crisp 2D chibi pixel-art game props, slightly elevated camera, consistent lighting. Reading left to right, top to bottom: 1 one open laptop, 2 one black student backpack, 3 one stack of textbooks, 4 one dorm key, 5 one toothbrush cup, 6 one wall light switch, 7 one student rice meal tray, 8 one hotpot on portable stove. Each object completely inside its own cell, isolated, no character, no text or watermark, no baked background shadow, coherent perspective. Preview for later manual inspection and slicing, not a finished Unity sprite atlas.

Kết quả thực tế: `1774×887` px, grid 4×2, crop loại 8 px mỗi phía để loại viền; tọa độ `(left, top, right, bottom)`, phải/dưới exclusive:
- `01-laptop.png`: `(8, 8, 436, 436)`
- `02-backpack.png`: `(452, 8, 879, 436)`
- `03-books.png`: `(895, 8, 1322, 436)`
- `04-key.png`: `(1338, 8, 1766, 436)`
- `05-toothbrush-cup.png`: `(8, 452, 436, 879)`
- `06-light-switch.png`: `(452, 452, 879, 879)`
- `07-meal-tray.png`: `(895, 452, 1322, 879)`
- `08-hotpot.png`: `(1338, 452, 1766, 879)`

Kiểm tra: đúng số vật và grid, vật không cắt cụt theo kiểm tra sheet. Nền trắng gộp trong ảnh (không có alpha); chưa tách lớp, chưa căn PPU, pivot, tỉ lệ với nhân vật hay import Unity. Không đảm bảo khớp pixel với scene; nồi và nguyên liệu gộp một ảnh, cần tách riêng nếu game cần thao tác.
