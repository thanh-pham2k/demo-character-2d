# Checkpoint xem cảnh campus (F8)

Mở Unity, chọn menu **Showcase → Checkpoint cảnh _F8** (phím tắt F8 nếu Unity nhận `_F8`; nếu không, dùng menu). Mỗi mục có hai nút:

- **Nạp & Play**: lưu scene đang sửa nếu cần, mở scene checkpoint ở chế độ Single, rồi vào Play Mode.
- **Chỉ mở cảnh**: mở scene ở Edit Mode, không vào Play Mode.

Cơ chế: dự án hiện không còn gameplay/save/F8 runtime (đã reset cho Year One), nên **checkpoint = mở lại scene từ đầu**. Mở Single là reset phiên trước: không còn coroutine, UI, pose hay reservation cũ. Không ghi gì vào save chơi chính; chỉ lưu id checkpoint gần nhất trong `EditorPrefs` (`Showcase.CampusCheckpoint.Last`). Nếu đang Play Mode, cửa sổ dừng Play trước rồi mới mở scene.

Tất cả kết quả dưới đây là **mong đợi, chưa kiểm tra runtime trong Unity**.

| Checkpoint (F8) | Thao tác | Kết quả mong đợi |
|---|---|---|
| **Phòng 404** | Nạp & Play | Mở `Assets/Scenes/Room404_AssetDraft.unity` đã chỉnh bố cục; thấy phòng, giường, bàn, ghế, cửa và Luồn. Không có HUD gameplay. |
| **Căn tin** | Nạp & Play | Nền căn tin lấp đầy khung 16:9; Luồn đứng giữa phải; cụm bàn + 2 ghế bên trái có khay và gia vị nằm trên mặt bàn (order 7); cụm bàn thứ hai bên phải (flipX, nhỏ hơn) và xe đẩy sát phải; caption “Căn tin” ở đáy trái. |
| **Trạm bus KTX** | Nạp & Play | Nền trạm chờ (nhà chờ, ghế, đèn, thùng rác nằm trong nền); Luồn đứng trước nhà chờ, chân không bị cắt; caption “Trạm bus KTX”. |
| **Cổng H6** | Nạp & Play | Nền cổng H6 (cổng, trụ, cây, bậc nằm trong nền); Luồn đứng giữa cổng, chân đặt trên mặt đường; caption “Cổng H6”. |
| **Giảng đường H6** | Nạp & Play | Nền lớp học; hai dãy bàn hai bên, dãy bàn tiền cảnh vẽ đè chân Luồn (order 6 > 5); đồng hồ treo tường bên phải (order 4, sau nhân vật); caption “Giảng đường H6”. |

## Duyệt bố cục phòng 404

Trong **Phòng 404**, chọn lần lượt **Giữa phòng**, **Bàn trái**, **Bàn phải**, **Giường trái**, **Giường phải**, **Cửa**. Mỗi nút mở lại scene rồi đặt sprite Luồn tại vị trí xem trong Play Mode. Kết quả mong đợi: nhân vật nằm trong khung, có thể so sánh tỷ lệ và khoảng trống quanh nội thất. Chọn vị trí khác khi đang Play để kiểm tra việc reset và nạp lại.

Đặt Game View 822 × 457 để so sánh với `Assets/Room404Draft/Sprites/Room404-reference-overlay.png`; có thể bật `Room404/ReferenceOverlay (enable to align)` để xem chồng ảnh. Cần đánh giá mép giường, khoảng cách tới mép màn hình, chân bàn/ghế, lối trước cửa và sự khớp với reference. Reference hiện có giường bị cắt ở mép, nên không thể vừa đòi trùng mọi pixel của nó vừa giữ giường nguyên vẹn; đánh giá thay đổi có chủ ý riêng.

Các vị trí này chỉ phục vụ kiểm tra hình: scene hiện không có controller di chuyển hoặc hệ thống tương tác. Không thể dùng chúng để xác nhận nhân vật đi được tới giường, bàn hay cửa. Chưa có ảnh render hoặc kết quả kiểm tra Unity cho bản chỉnh này.

## Ghi nhận từ render camera 822 × 457

Ảnh `Assets/Temp/room404-center.png` là ảnh camera Unity, không phải ảnh Play Mode. Đo gần đúng theo mép nhìn thấy: mép ngoài hai giường cách cạnh ảnh khoảng 32 px và 35 px (3,9% và 4,3% chiều rộng); cửa cao khoảng 143 px so với Luồn khoảng 76 px (1,88 lần); thân bàn cao khoảng 53 px (0,70 lần Luồn); ghế cao khoảng 54 px (0,71 lần Luồn). Các số là ước lượng thị giác, phụ thuộc đường biên và phần tính là thân bàn; cần xác nhận lại sau mọi thay đổi hoặc khi đổi aspect.

Sáu ảnh `room404-center`, `room404-desk-left`, `room404-desk-right`, `room404-bed-left`, `room404-bed-right`, `room404-door` trong `Assets/Temp/` cho thấy Luồn tĩnh ở từng vị trí. `room404-overlay.png` là ảnh chồng reference và bản render; sự khác biệt lớn ở cửa, cửa sổ và kích thước/vị trí nhóm nội thất vẫn chưa được giải quyết. Những ảnh này không chứng minh điều khiển, va chạm hoặc tương tác.

## Kiểm tra nhanh trong Edit Mode

Bấm **Chỉ mở cảnh** rồi xem Game view: caption vẫn hiện vì Canvas dùng `ScreenSpaceCamera`. Đổi scene bằng tay cũng cho kết quả tương đương; cửa sổ checkpoint chỉ thêm bước reset + Play.

## Ghi chú

- Scene không nằm trong Build Settings; Play Mode trong Editor không cần Build Settings.
- Ảnh chụp kiểm tra tĩnh (Edit Mode, `capture game_view --source camera`) nằm ở `mobile-2d/Assets/Temp/showcase/` — chỉ là ảnh đối chiếu, có thể xoá.
- Chi tiết art, scale, sorting và lỗi còn lại: `mobile-2d/Assets/Showcase/README.md`.
