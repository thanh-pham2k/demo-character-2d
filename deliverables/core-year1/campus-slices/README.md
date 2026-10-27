# Campus sheet: cắt vùng sơ bộ

Nguồn: `source.png` do người dùng cung cấp (1672×941, có nhãn). Các PNG crop sẵn trong thư mục này là bản sơ bộ: chưa làm sạch alpha, viền màu hoặc tách vật dính nhau. Nhãn nằm trong background; một số vật xuất hiện sẵn trong background và có thể bị trùng khi ghép. Các vùng sát nhau có thể chứa một phần vật lân cận. Không xem đây là sprite production.

Các nhóm table_chairs, chairs, shelf_cart, shelter_seats, lamp_bin, stairs_railings, trees_shrubs và desks là nhóm vật, chưa tách từng sprite.

`cleaned-sheet-draft.png` là bản thử xóa nhãn bằng imagegen. Kiểm tra trực quan cho thấy viền đỏ/vàng và pixel thừa vẫn còn; các nhóm vật vẫn dính nhau, nền còn chứa vật trùng. Không dùng bản này làm sprite production hoặc coi là đã làm sạch hoàn tất.

## Script cắt dùng cho Unity

`slice_for_unity.py` tạo bộ art đang dùng trong `mobile-2d/Assets/Showcase/Art/`.

```bash
python3 slice_for_unity.py --out ../../../mobile-2d/Assets/Showcase/Art --preview /tmp/campus_preview.png
```

Yêu cầu: Python 3 + Pillow + NumPy + SciPy.

Nguồn và cách cắt:

- **Nền**: `../references/canteen-campus-base-reference.png` (1672×941, lưới 2×2 không nhãn, rãnh đen 8 px tại `x 832..839`, `y 466..473`). Cắt 4 góc, inset 2 px vào trong rãnh. Kích thước ra: 828×462 (căn tin, bus) và 828×463 (cổng, giảng đường).
- **Đạo cụ**: `source.png` (sheet có nhãn). Rect đo bằng connected-component trên nền trắng, khai báo trong `PROPS`.

Vì sao không dùng các file `*.png` crop sẵn: kênh alpha của chúng bị hỏng (gần như không có pixel đục hoàn toàn, RGB bị lượng tử hóa), nên viền và độ trong suốt không dùng được. Cắt lại từ sheet giữ nguyên độ sâu màu.

Xử lý alpha cho đạo cụ:

1. Nền trắng = pixel có min channel ≥ 238 và nối với biên crop (flood fill), nên vùng trắng kín bên trong vật được giữ.
2. Đục lỗ trắng kín có diện tích ≥ 30 px và min channel ≥ 236 (khoảng trống giữa khung bàn và chân bàn). Tắt cho vật mà trắng là chi tiết thật (mặt đồng hồ, khay, chai).
3. Viền anti-alias sát nền được khử trắng bằng un-premultiply với ramp alpha 45 mức từ ngưỡng 225.
4. Bỏ mảnh nhỏ dính mép crop (< 20% diện tích mảnh lớn nhất) — đó là phần của vật bên cạnh trên sheet.
5. Trim về biên alpha, chừa 1 px trong suốt.

Danh sách đầu ra (rect trong `source.png`, kích thước sau trim):

| File | Rect | Kích thước | Ghi chú |
|---|---|---|---|
| `prop_canteen_table_set` | 789,61,1025,252 | 234×191 | cụm 1 bàn + 2 ghế dính nhau |
| `prop_canteen_chair` | 817,142,890,257 | 72×113 | 1 ghế |
| `prop_canteen_condiments` | 1183,208,1250,261 | 65×51 | |
| `prop_canteen_meal_tray` | 1260,218,1392,267 | 130×48 | |
| `prop_canteen_cart` | 1541,185,1638,281 | 95×94 | xe đẩy |
| `prop_classroom_desks_left` | 770,758,1017,933 | 247×173 | cụm nhiều bàn, cắt ở mép sheet |
| `prop_classroom_desks_right` | 1007,758,1266,930 | 257×172 | cụm nhiều bàn, cắt ở mép sheet |
| `prop_classroom_clock` | 1454,840,1504,890 | 48×48 | |

Manifest đầy đủ (nguồn, rect, kích thước, pivot, PPU, xử lý): `mobile-2d/Assets/Showcase/Art/campus_manifest.json`.

## Lỗi còn lại

- Cụm bàn ghế và cụm bàn giảng đường vẫn dính nhau; không tách bằng đoán hình.
- Còn highlight trắng nhỏ bên trong khung bàn/ghế; ngưỡng đục lỗ không phân biệt được hoàn toàn với highlight hợp lệ.
- Không có sprite riêng cho nhà chờ bus, cổng, cây: các vật này đã nằm trong nền và được dùng trực tiếp từ nền.
- Tỷ lệ/palette vẫn là concept, chưa chốt cho production.
